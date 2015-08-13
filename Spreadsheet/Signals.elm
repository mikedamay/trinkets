import Graphics.Element
import Graphics.Collage
import Mouse
import Window
import Text
import Time
import Grid exposing (GridCoords, getGridCoordsEx)
import Gridlines exposing (makeGridlines)

main : Signal.Signal Graphics.Element.Element
main =
    let
        display : ((Int, Int), (Int, Int), (Int, Int), Time.Time, GridCoords) -> Graphics.Element.Element
        display ((width, height), (mouse_x,mouse_y), (x,y), t, gridCoords ) =
            Graphics.Collage.collage width height (List.concat [[
              formAt (x - width // 2, -y + height //2) "mike was here"
              ,formAt (0,0) (toString t)
              ,formAt (100,100) (toString width ++ "," ++ toString height)
              ,formAt (200,200) (toString mouse_x ++ "," ++ toString mouse_y)

              ], (makeGridlines gridCoords (width, height))])
    in

        Signal.map display (Signal.map5 (,,,,)
            Window.dimensions
            Mouse.position
            (Signal.sampleOn Mouse.clicks Mouse.position)
            (Time.every Time.second)
            (Signal.map getGridCoordsEx Window.dimensions)
            )

        --Signal.map display (Signal.map model2Tuple (doModel event))

model2Tuple : Model -> ((Int, Int), (Int, Int), (Int, Int), Time.Time, GridCoords)
model2Tuple model =
    (model.dims, model.mousepos, model.pos, model.time, model.gridCoords)

type alias Model = {dims : (Int, Int), pos : (Int, Int)
                     , text : String, time : Time.Time, mousepos : (Int, Int)
                     , gridCoords : GridCoords }
initialGridCoords = {horz=[], vert=[]}

initialModel : Model
initialModel =
    { dims = (500, 500), pos = (0,0), text = "mike was here", time = 0, mousepos = (0,0), gridCoords = initialGridCoords }

doModel : Signal.Signal Event -> Signal.Signal Model
doModel ev =
    let 
        doModel' ev mod = 
            case ev of 
              Dims d -> let
                            updatedMod = {mod | gridCoords <- (getGridCoordsEx d)}
                            updatedMod2 = {updatedMod | dims <- d}
                        in
                            updatedMod2
              Pos (x,y) -> {mod | pos <- (x,y)}
              Timer t -> {mod | time <- t}
              MousePos (x,y) -> {mod | mousepos <- (x,y)}
    in
        Signal.foldp doModel' initialModel ev

type Event = Dims (Int, Int) | Pos (Int, Int) | Timer Time.Time | MousePos (Int, Int)

event : Signal.Signal Event
event =
  Signal.mergeMany [
                   Signal.map Dims Window.dimensions
                   ,Signal.map MousePos Mouse.position
                   ,Signal.map Pos (Signal.sampleOn Mouse.clicks Mouse.position)
                   ,Signal.map Timer (Time.every Time.second)
                   ]

formAt : (Int, Int) -> String -> Graphics.Collage.Form
formAt (x,y) str =
    Graphics.Collage.move (toFloat x, toFloat y) 
      <| Graphics.Collage.text 
      <| Text.fromString str
                  
                  
                  
                  
                  
                  
                  