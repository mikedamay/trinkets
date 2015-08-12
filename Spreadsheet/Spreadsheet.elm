import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window
import Mouse
import Gridlines exposing (doGridlines)
import Grid exposing (GridCoords, getGridCoords)
--type alias GridCoords = { horz : List Int, vert : List Int }
--doGridlines = Signal.map (\_ -> []) (Time.fps 1)  -- dummy routine while testing on elm site
--getGridCoords p = {horz=[], vert=[]}

type alias Vec = (Float, Float)
type alias Pos = (Int, Int)

main : Signal.Signal Graphics.Element.Element
main =
--    debug
    doCollage

doCollage : Signal.Signal Graphics.Element.Element
doCollage =
    Signal.map (Graphics.Collage.collage 1280 894)
      (Signal.map2 prepareForRender (Signal.foldp doModel initialModel event) Window.dimensions)

debug =
    let
        debug' gridCoords = Graphics.Element.show gridCoords.horz
    in
        Signal.map debug' (getGridCoords Window.dimensions)

prepareForRender : Model -> (Int, Int) -> List Graphics.Collage.Form
prepareForRender model (width, height) =
    let
        gl = Gridlines.makeGridlines model.gridder (width, height)
    in
    List.concat [[ moveForm (model.debug_val, model.debug_ctr), handleClick model.clicker]
      ,Gridlines.makeGridlines model.gridder (width, height)
--      ], Gridlines.makeGridlines {horz=[50,100], vert=[50,100]} (width, height)
--      , [Graphics.Collage.filled Color.blue (Graphics.Collage.circle 10)]
--      , [Graphics.Collage.move (100,100) (Graphics.Collage.toForm (Graphics.Element.show gl))]
      ]

moveForm : (String, Int) -> Graphics.Collage.Form
moveForm pos =
  let
      myForm =
        --Graphics.Collage.toForm ( Graphics.Element.show "mike was here" )
        Graphics.Collage.toForm ( Graphics.Element.show pos )
  in
      Graphics.Collage.move (-100, 100) myForm


type Event = Mover Time.Time | Gridder GridCoords | Clicker (Int, Int) | Dimmer (Int, Int)
event : Signal Event
event = Signal.mergeMany [Signal.map Mover (Time.fps 0.1)
                          ,Signal.map Gridder (getGridCoords Window.dimensions)
                          ,Signal.map Clicker (Signal.sampleOn Mouse.clicks Mouse.position)
                         ]

type alias Model = {mover : (Float, Float)
                    , gridder : GridCoords
                    , clicker : (Int, Int)
                    , debug_ctr : Int
                    , debug_val : String
                    }

initialModel : Model
initialModel = {mover=(1,1), gridder = {horz=[50, 100], vert=[50, 100]}
               ,clicker = (200,200)
               ,debug_ctr = 0, debug_val = "" }

doModel : Event -> Model -> Model
doModel event model =
    case event of
        Mover t     -> {model | mover <- constrainMovement model.mover }
        --Mover t     -> {model | debug_ctr <- model.debug_ctr + 1}
        Gridder g   -> {model | gridder <- g }
        --Gridder g   -> {model | debug_val <- "gridder"}
        Clicker p   -> {model | clicker <- p }
        --Clicker p   -> {model | debug_val <- "clicker"}

constrainMovement : (Float, Float) -> (Float, Float)
constrainMovement (direction, magnitude) =
  if | magnitude > 20 -> (-1, 20)
     | magnitude < 0 -> (1, 0)
     | otherwise -> (direction, magnitude + direction)

handleClick : Pos -> Graphics.Collage.Form
handleClick (x, y) =
    Graphics.Collage.move (200,200) (Graphics.Collage.toForm (Graphics.Element.show (x,y)))
