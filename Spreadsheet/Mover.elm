import Graphics.Element
import Graphics.Collage
import Time

type alias Vec = (Float, Float)

main : Signal.Signal Graphics.Element.Element
main = doCollage (Time.fps 1)

doCollage : Signal.Signal Time.Time -> Signal.Signal Graphics.Element.Element
doCollage timeSignal = 
  let staticCollageToElement fm = Graphics.Collage.collage 500 500 [fm]
  in Signal.map staticCollageToElement (moveForm timeSignal)

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

mainy : Signal.Signal Graphics.Element.Element
mainy = Signal.map Graphics.Element.show (Signal.foldp constrainMovement (1, 1) (Time.fps 1))

doStuff t (d, m) = (d,m)
