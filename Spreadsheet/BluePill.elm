import Mouse
import Window
import Random exposing (Seed, generate, initialSeed)
import Color exposing (Color, black, gray, lightBlue, lightGray, lightRed, white)
import Time exposing (Time, fps, inSeconds, every, second)
import Graphics.Collage exposing (Form, circle, collage, filled
                                  , move, scale, text, toForm)
import Text exposing (Text)
import Signal exposing (Signal, filter, foldp, map, map2, mergeMany, sampleOn)
import List exposing (isEmpty)
import Graphics.Element exposing (Element, centered, color
                                  , container, middle, show)

-- CONFIG
speed = 500
spawnInterval = 57 / speed
sizePill = 15
sizePlayer = sizePill

(width, height) = (400, 400)
(hWidth, hHeight) = (width / 2, height / 2)

-- HELPER FUNCTIONS
relativeMouse : (Int, Int) -> (Int, Int) -> (Int, Int)
relativeMouse (ox, oy) (x, y) = (x - ox, -(y - oy))

center : (Int, Int) -> (Int, Int)
center (w, h) = ( w // 2, h // 2)  -- MM!!! replaced div

type alias Vec = (Float, Float)

vecAdd : Vec -> Vec -> Vec
vecAdd (ax, ay) (bx, by) = (ax + bx, ay + by)

vecSub : Vec -> Vec -> Vec
vecSub (ax, ay) (bx, by) = (ax - bx, ay - by)

vecLen : Vec -> Float
vecLen (x, y) = sqrt (x * x + y * y)

vecMulS : Vec -> Time -> Vec
vecMulS (x, y) t = (x * t, y * t)

tf : Float -> Float -> String -> Form
tf y scl str = (Text.fromString str) |> Text.color gray
                          |> centered    -- MM!! centered
                          |> toForm
                          |> scale scl
                          |> move (0, y)

-- INPUT
delta = (fps 30)
input = map2 (,) (map inSeconds delta)
             (sampleOn delta (map2 relativeMouse (map center Window.dimensions) Mouse.position))

--rand fn sig = map fn (Random.float sig)  -- MM!!!  - revisit Random.Generator
--rand fn sig = fn (Random.float 0 1 )
rand fn sig = fn 0.5
randX = rand (\r -> (width * r) - hWidth)
randCol = rand (\r -> if r < 0.1 then lightBlue else defaultPill.col)

interval = (every (second * spawnInterval))

event : Signal Event
event = mergeMany [ map Tick input
--                ,map2 (\x col -> Add (newPill x col)) (randX interval) (randCol interval)  -- MM!!!
                ,map (\_ -> Add defaultPill) interval
                ,map (\_ -> Click) Mouse.clicks ] -- Mouse.isClicked is deprecated

-- MODEL
type alias Pill = {pos:Vec, vel:Vec, rad:Float, col:Color}

defaultPill = { pos = (0, hHeight)
               ,vel = (0, -speed)
               ,rad = sizePill
               ,col = lightRed 
               }

defaultPlayer = { defaultPill | pos <- (0, -hHeight - sizePlayer)
                              , rad <- sizePlayer
                              , col <- black }

type State = Start | Play | Over
type alias Game = {player:Pill, pills:List Pill, score:Int, state:State, seed: Seed}

defaultGame = { player = defaultPlayer
               ,pills = []
               ,score = 0
               ,state = Start 
               ,seed = (initialSeed 31425)}

newPill : Float -> Color -> Pill
newPill x col = { defaultPill | pos <- (x, hHeight)
                              , col <- col }

newRandomPill : Pill -> Game -> ( Pill, Game)
newRandomPill pill g =
    let
        (randXX, seed' ) = generate (Random.float 0 1) g.seed
        (colval, seed'' ) = generate (Random.float 0 1) seed'
    in
        ({ pill | pos <- (width * randXX, -hWidth), col <- lightBlue}
        ,{ g | seed <- seed'' })

-- UPDATE
type Event = Tick (Time, (Int, Int)) | Add Pill | Click

stepPlay : Event -> Game -> Game
stepPlay event g =
    case event of
        Tick (t, mp) -> let hit pill = (vecLen <| vecSub g.player.pos pill.pos) < g.player.rad + pill.rad
                            unculled = List.filter (\{pos} -> snd pos > -hHeight) g.pills
                            untouched = List.filter (not << hit) unculled
                            touched = List.filter hit unculled
                            hitColor c = not <| isEmpty <| List.filter (\{col} -> col == c) touched
                            hitBlue = hitColor lightBlue
                            hitRed = hitColor lightRed
                            out = let (x, y) = mp in abs (toFloat x) > hWidth || abs (toFloat y) > hHeight
                            g' = { g | player <- stepPlayer mp g.player
                                     , pills  <- List.map (stepPill t) untouched
                                     , score  <- if hitBlue then g.score + 1 else g.score }
                        in  if hitRed || out then { defaultGame | score <-  g'.score
                                                                , state <- Over } else g'
        Add p        -> let (p', g') = (newRandomPill p g )
                        in  { g' | pills <- p' :: g.pills }
        Click        -> g

click : Event -> Bool
click event =
    case event of
        Click -> True
        _     -> False

stepGame : Event -> Game -> Game
stepGame event ({state} as g) =
    let playGame = { defaultGame | state <- Play }
        toPlay = if click event then playGame else g
    in case state of
        Play  -> stepPlay event g
        _     -> toPlay

stepPlayer : (Int, Int) -> Pill -> Pill
stepPlayer (x, y) p = { p | pos <- (toFloat x, toFloat y) }

stepPill : Time -> Pill -> Pill
stepPill t p = { p | pos <- vecAdd p.pos <| vecMulS p.vel t }


-- DISPLAY
render : (Int, Int) -> Game -> Element
render (w, h) g =
    let formPill {rad, col, pos} = circle rad |> filled col
                                              |> move pos
        txts = case g.state of
            Start -> [ tf  70 4 "BluePiLL"
                      ,tf   0 2 "Click to Start" ]
            Play  -> [ tf   0 4 (toString  g.score)]
            Over  -> [ tf  70 4 "Game Over"
                      ,tf   0 4 (toString  g.score)
                      ,tf -50 2 "Click to Restart" ]
        forms = txts ++ (List.map formPill <| g.player :: g.pills)
    in  color lightGray <| container w h middle
                        <| color white
                        <| collage width height forms


main = map2 render Window.dimensions (foldp stepGame defaultGame event)
