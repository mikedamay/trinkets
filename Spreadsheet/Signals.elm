import Graphics.Element
import Graphics.Collage
import Mouse
import Window
import Text
import Time

mainx : Signal.Signal Graphics.Element.Element
mainx =
    let
        showStuff (x,y) (width, height)
            = Graphics.Collage.collage width height [
              (Graphics.Collage.move ((toFloat x) - (toFloat (width // 2)), -(toFloat y) + toFloat (height // 2))
              (Graphics.Collage.text (Text.fromString "mike was here")))]
    in
        Signal.map2 showStuff (Signal.sampleOn Mouse.clicks Mouse.position) Window.dimensions
        

main : Signal.Signal Graphics.Element.Element
main =
    let
        showStuff : ((Int, Int), (Int, Int), (Int, Int), Time.Time) -> Graphics.Element.Element
        showStuff ((width, height), (mouse_x,mouse_y), (x,y), t ) =
            Graphics.Collage.collage width height [
              formAt (x - width // 2, -y + height //2) "mike was here"
              ,formAt (0,0) (toString t)
              ,formAt (100,100) (toString width ++ "," ++ toString height)
              ,formAt (200,200) (toString mouse_x ++ "," ++ toString mouse_y)
              ]          
    in
      {-
        Signal.map showStuff (Signal.map4 (,,,)
            Window.dimensions
            Mouse.position
            (Signal.sampleOn Mouse.clicks Mouse.position)
            (Time.every Time.second))
       -}
        Signal.map showStuff (Signal.map model2Tuple (doModel event))

model2Tuple : Model -> ((Int, Int), (Int, Int), (Int, Int), Time.Time)
model2Tuple model =
    (model.dims, model.mousepos, model.pos, model.time)

type alias Model = {dims : (Int, Int), pos : (Int, Int)
                     , text : String, time : Time.Time, mousepos : (Int, Int) }

initialModel : Model
initialModel =
    { dims = (500, 500), pos = (0,0), text = "mike was here", time = 0, mousepos = (0,0) }

doModel : Signal.Signal Event -> Signal.Signal Model
doModel ev =
    let 
        doModel' ev mod = 
            case ev of 
              Dims d -> {mod | dims <- d}
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
                  
                  
                  
                  
                  
                  
                  