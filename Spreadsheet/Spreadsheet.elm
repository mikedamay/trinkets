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
    doCollage

doCollage : Signal.Signal Graphics.Element.Element
doCollage =
    Signal.map (Graphics.Collage.collage 500 500)
      (Signal.map2 prepareForRender (Signal.foldp doModel initialModel event) Window.dimensions)

prepareForRender : Model -> (Int, Int) -> List Graphics.Collage.Form
prepareForRender model (width, height) =
    List.concat [[ moveForm model.mover, handleClick model.clicker
      ], Gridlines.makeGridlines model.gridder (width, height)]

moveForm : (Float, Float) -> Graphics.Collage.Form
moveForm pos =
  let
      myForm =
        Graphics.Collage.toForm ( Graphics.Element.show "mike was here" )
  in
      Graphics.Collage.move pos myForm


type Event = Mover Time.Time | Gridder GridCoords | Clicker (Int, Int) | Dimmer (Int, Int)
event : Signal Event
event = Signal.mergeMany [Signal.map Mover (Time.fps 30)
                          ,Signal.map Gridder (getGridCoords Window.dimensions)
                          ,Signal.map Clicker (Signal.sampleOn Mouse.clicks Mouse.position)
                          ,Signal.map Dimmer Window.dimensions
                         ]

type alias Model = {mover : (Float, Float), gridder : GridCoords, clicker : (Int, Int), dimmer : (Int, Int) }
initialModel : Model
initialModel = {mover=(1,1), gridder = {horz=[], vert=[]}, clicker = (200,200), dimmer = (500, 500) }

doModel : Event -> Model -> Model
doModel event model =
    case event of
        Mover t     -> {model | mover <- constrainMovement model.mover }
        Gridder g   -> {model | gridder <- g }
        Clicker p   -> {model | clicker <- p }
        Dimmer d    -> {model | dimmer <- d }

constrainMovement : (Float, Float) -> (Float, Float)
constrainMovement (direction, magnitude) =
  if | magnitude > 20 -> (-1, 20)
     | magnitude < 0 -> (1, 0)
     | otherwise -> (direction, magnitude + direction)

handleClick : Pos -> Graphics.Collage.Form
handleClick (x, y) =
    Graphics.Collage.move (200,200) (Graphics.Collage.toForm (Graphics.Element.show (x,y)))



-- ******************************************************************************************

