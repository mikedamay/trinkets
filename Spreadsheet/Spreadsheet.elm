import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window
import Mouse
-- import Gridlines exposing (doGridlines)
doGridlines = Signal.map (\_ -> []) (Time.fps 1)  -- dummy routine while testing on elm site

type alias Vec = (Float, Float)

type alias Pos = (Int, Int)

main : Signal.Signal Graphics.Element.Element
main =
    doCollage (Time.fps 1) Window.dimensions

type Event = Mover Time.Time | Gridder GridCoords | Clicker (Int, Int) | Dimmer (Int, Int)

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




