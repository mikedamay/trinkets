import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window
import Mouse
-- import Gridlines exposing (doGridLines)
doGridLines sig = Signal.map (\a -> []) sig  -- dummy routine while testing on elm site

type alias Vec = (Float, Float)

type alias Pos = (Int, Int)

main : Signal.Signal Graphics.Element.Element
main =
    Signal.map handleClick (Signal.sampleOn Mouse.clicks Mouse.position)
    
handleClick : Pos -> Graphics.Element.Element    
handleClick (x, y) =
    Graphics.Collage.collage 500 500 [Graphics.Collage.toForm (Graphics.Element.show (x,y))]

mainx : Signal.Signal Graphics.Element.Element
mainx =
    doCollage (Time.fps 1) Window.dimensions

doCollage : Signal.Signal Time.Time -> Signal.Signal (Int, Int) -> Signal.Signal Graphics.Element.Element
doCollage timeSignal windowSignal =
  let
      staticCollageToElement fm fm2 (width, height) =
        Graphics.Collage.collage width height (List.concat [[fm], fm2])
  in
      Signal.map3 staticCollageToElement (moveForm timeSignal) (doGridLines windowSignal) windowSignal

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




