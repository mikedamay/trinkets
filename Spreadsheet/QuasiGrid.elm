import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text

type alias Vec = (Float, Float)

main : Signal.Signal Graphics.Element.Element
main = doCollage (Time.fps 1)

doCollage : Signal.Signal Time.Time -> Signal.Signal Graphics.Element.Element
doCollage timeSignal = 
  let staticCollageToElement fm fm2 = Graphics.Collage.collage 500 500 (List.concat [[fm], fm2])
  in Signal.map2 staticCollageToElement (moveForm timeSignal) (doGridLines timeSignal)

moveForm : Signal.Signal Time.Time -> Signal.Signal (Graphics.Collage.Form)
moveForm timeSignal = 
  let myForm =
    Graphics.Collage.toForm ( Graphics.Element.show "mike was here" )
  in Signal.map Basics.fst (Signal.foldp moveFormOneStep (myForm, (1, 1)) timeSignal)

moveFormOneStep : Float -> (Graphics.Collage.Form, Vec) -> (Graphics.Collage.Form, Vec)
moveFormOneStep t (fm, vec) =
  let newVec = (constrainMovement t vec)
      x = t / 500
  in (Graphics.Collage.move (x * fst newVec, 0) fm, newVec)


constrainMovement : Time.Time -> (Float, Float) -> (Float, Float)
constrainMovement t (direction, magnitude) =
  if | magnitude > 20 -> (-1, 20)
     | magnitude < 0 -> (1, 0)
     | otherwise -> (direction, magnitude + direction)


doGridLines :  Signal.Signal Time.Time -> Signal.Signal (List Graphics.Collage.Form)
doGridLines timeSignal = Signal.map gridLines timeSignal
  
gridLines : Time.Time -> List Graphics.Collage.Form
gridLines t =
  let ctr = 0 in
  if ctr == 0 then
    []
  else
    (fm ctr) :: (gridLines t)

fm : Int -> Graphics.Collage.Form
fm num =
  Graphics.Collage.move (toFloat num * 10, 0) (Graphics.Collage.text
    (Text.fromString (toString num)))


myLineStyle : Color.Color -> Graphics.Collage.LineStyle
myLineStyle  clr =
  { myDefaultLine | color <- Color.gray }

myDefaultLine =
  Graphics.Collage.defaultLine
  

