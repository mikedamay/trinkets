import Graphics.Element
import Graphics.Collage
import Mouse
import Window
import Text
import Time
import Grid exposing (GridCoords, getGridCoords)
import Gridlines exposing (makeGridlines)

main : Signal.Signal Graphics.Element.Element
main =
    let
        display : Model -> Graphics.Element.Element
        display model =
            let
                (width, height) = model.dims
                (mouse_x, mouse_y) = model.mousepos
                (x, y) = model.click
                t = model.time
                gridCoords = model.gridCoords
            in
                Graphics.Collage.collage width height (List.concat [[
                  formAt (x - width // 2, -y + height //2) (toString gridCoords.horz)
                  ,formAt (0,0) (toString t)
                  ,formAt (100,100) (toString width ++ "," ++ toString height)
                  ,formAt (200,200) (toString mouse_x ++ "," ++ toString mouse_y)
                  ], (makeGridlines gridCoords (width, height))])
    in
        Signal.map display (doModel event)

type alias Model = {dims : (Int, Int), click : (Int, Int)
                     , text : String, time : Time.Time, mousepos : (Int, Int)
                     , gridCoords : GridCoords }
initialGridCoords = {horz=[], vert=[]}

initialModel : Model
initialModel =
    { dims = (500, 500), click = (0,0), text = "mike was here", time = 0, mousepos = (0,0), gridCoords = initialGridCoords }

doModel : Signal.Signal Event -> Signal.Signal Model
doModel ev =
    let
        addGridCoords mod d =
            let
                updatedMod = {mod | gridCoords <- (getGridCoords d)}
                updatedMod2 = {updatedMod | dims <- d}
            in
                updatedMod2
        doModel' ev mod =
            case ev of
              Dims d -> addGridCoords mod d
              Click (x,y) -> {mod | click <- (x,y)}
              Timer t -> {mod | time <- t}
              MousePos (x,y) -> {mod | mousepos <- (x,y)}
    in
        Signal.map2 addGridCoords (Signal.foldp doModel' initialModel ev) Window.dimensions
                -- we need the first call to addGridCoords to catch the initial dimensions
                -- we need the other call to addGridCoords to catch changes to the dimensions

type Event = Dims (Int, Int) | Click (Int, Int) | Timer Time.Time | MousePos (Int, Int)

event : Signal.Signal Event
event =
  Signal.mergeMany [
                   Signal.map Dims Window.dimensions
                   ,Signal.map MousePos Mouse.position
                   ,Signal.map Click (Signal.sampleOn Mouse.clicks Mouse.position)
                   ,Signal.map Timer (Time.every Time.second)
                   ]

formAt : (Int, Int) -> String -> Graphics.Collage.Form
formAt (x,y) str =
    Graphics.Collage.move (toFloat x, toFloat y) 
      <| Graphics.Collage.text 
      <| Text.fromString str
                  
                  
                  
                  
                  
                  
                  