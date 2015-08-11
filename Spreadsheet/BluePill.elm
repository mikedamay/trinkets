import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window
import Mouse
-- import Gridlines exposing (doGridlines)
--type alias GridCoords = List Int
doGridlines = Signal.map (\_ -> []) (Time.fps 1)  -- dummy routine while testing on elm site

type alias Vec = (Float, Float)

type alias Pos = (Int, Int)

main : Signal.Signal Graphics.Element.Element
main =
    doCollage2
    --doCollage (Time.fps 1) Window.dimensions

type Event = Mover Time.Time | Gridder GridCoords | Clicker (Int, Int) | Dimmer (Int, Int)

event : Signal Event
event = Signal.mergeMany [Signal.map Mover (Time.fps 30)
                          ,Signal.map Gridder (getGridCoords Mouse.position)
                          ,Signal.map Clicker (Signal.sampleOn Mouse.clicks Mouse.position)
                          ,Signal.map Dimmer Window.dimensions
                         ]

type alias Model = {mover : (Float, Float), gridder : GridCoords, clicker : (Int, Int), dimmer : (Int, Int) }

initialModel : Model
initialModel = {mover=(1,1), gridder = {horz=[], vert=[]}, clicker = (200,200), dimmer = (500, 500) }

doModel : Event -> Model -> Model
doModel event model =
    case event of
        Mover t     -> {model | mover <- constrainMovement t model.mover }
        Gridder g   -> {model | gridder <- g }
        Clicker p   -> {model | clicker <- p }
        Dimmer d    -> {model | dimmer <- d }

--prepareForRender : Model -> List (Float, Float)
prepareForRender : Model -> List Graphics.Collage.Form
prepareForRender model =
--    [ model.mover, (toFloat (fst model.clicker), 0) ]
    [ moveForm2 model.mover, handleClick model.clicker ]
--[ Graphics.Collage.move (0,0) <| Graphics.Collage.filled Color.blue (Graphics.Collage.circle 10) ]

doCollage2 : Signal.Signal Graphics.Element.Element
doCollage2 =
    --Signal.map Graphics.Element.show (Signal.map prepareForRender (Signal.foldp doModel initialModel event))
    Signal.map (Graphics.Collage.collage 500 500)  (Signal.map prepareForRender (Signal.foldp doModel initialModel event))

doCollage : Signal.Signal Time.Time -> Signal.Signal (Int, Int) -> Signal.Signal Graphics.Element.Element
doCollage timeSignal windowSignal =
  let
      staticCollageToElement fm fm2 fm3 (width, height) =
        Graphics.Collage.collage width height (List.concat [[fm], fm2, [fm3]])
  in
      Signal.map4 staticCollageToElement
        (moveForm timeSignal)
        doGridlines
        (Signal.map handleClick (Signal.sampleOn Mouse.clicks Mouse.position))
        windowSignal

moveForm2 : (Float, Float) -> Graphics.Collage.Form
moveForm2 pos =
  let
      myForm =
        Graphics.Collage.toForm ( Graphics.Element.show "mike was here" )
  in
      Graphics.Collage.move pos myForm

moveForm : Signal.Signal Time.Time -> Signal.Signal (Graphics.Collage.Form)
moveForm timeSignal =
  let
      myForm =
        Graphics.Collage.toForm ( Graphics.Element.show "mike was here" )
  in
      Signal.map Basics.fst (Signal.foldp moveFormOneStep (myForm, (1, 1)) timeSignal)

moveFormOneStep : Float -> (Graphics.Collage.Form, Vec) -> (Graphics.Collage.Form, Vec)
moveFormOneStep t (fm, vec) =
  let
      newVec = (constrainMovement t vec)
      x = t / 500
  in
      (Graphics.Collage.move (x * fst newVec, 0) fm, newVec)


constrainMovement : Time.Time -> (Float, Float) -> (Float, Float)
constrainMovement t (direction, magnitude) =
  if | magnitude > 20 -> (-1, 20)
     | magnitude < 0 -> (1, 0)
     | otherwise -> (direction, magnitude + direction)

handleClick : Pos -> Graphics.Collage.Form
handleClick (x, y) =
    Graphics.Collage.move (200,200) (Graphics.Collage.toForm (Graphics.Element.show (x,y)))



-- ******************************************************************************************
cellHeight = 17
cellWidth = 100

type alias GridCoords = { horz : List Int, vert : List Int }

getGridCoords : Signal.Signal (Int, Int) -> Signal.Signal GridCoords
getGridCoords winDims =
    let
        getGridCoords' : (Int, Int) -> GridCoords
        getGridCoords' (width, height) =
            let
                numCols = width // cellWidth
                numRows = height // cellHeight
                spacer numRowsOrCols space =
                    let 
                        multiplyBySpace n = n * space
                    in
                        List.map multiplyBySpace [1..numRowsOrCols]
            in               
                { horz = (spacer numRows cellHeight), vert = (spacer numCols cellWidth) }
    in  
        Signal.map getGridCoords' winDims 











