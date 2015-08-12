import Graphics.Element
import Graphics.Collage
import Mouse
import Window
import Text


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
        showStuff : Model -> Graphics.Element.Element
        showStuff model =
            let
                x = fst model.pos
                y = snd model.pos
                width = fst model.dims
                height = snd model.dims
            in
                Graphics.Collage.collage width height [
                  (Graphics.Collage.move ((toFloat x) - (toFloat (width // 2))
                    , -(toFloat y) + toFloat (height // 2))
                  (Graphics.Collage.text (Text.fromString "mike was here")))
                  ]          
    in
        Signal.map showStuff (doModel event)

type alias Model = {dims : (Int, Int), pos : (Int, Int), text : String}

initialModel : Model
initialModel =
    { dims = (500, 500), pos = (0,0), text = "mike was here" }

doModel : Signal.Signal Event -> Signal.Signal Model
doModel ev =
    let 
        doModel' ev mod = 
            case ev of 
              Dims d -> {mod | dims <- d}
              Pos (x,y) -> {mod | pos <- (x,y)}
    in
        Signal.foldp doModel' initialModel ev

type Event = Dims (Int, Int) | Pos (Int, Int)

event : Signal.Signal Event
event =
  Signal.mergeMany [
                   Signal.map Dims Window.dimensions
                   ,Signal.map Pos (Signal.sampleOn Mouse.clicks Mouse.position)                   
                   ]
