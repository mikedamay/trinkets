import Graphics.Element
import Window
import Mouse
import Signal exposing ((<~))
import Graphics.Collage
import Color
import Text
import Time

main : Signal Graphics.Element.Element
main =
  Signal.map doCollage2 (Time.fps 1)


doCollage2 : Time.Time -> Graphics.Element.Element
doCollage2 t =
  --Graphics.Element.show t
  Graphics.Collage.collage 500 500 [ Graphics.Collage.toForm (Graphics.Element.show t), Graphics.Collage.move (100,100) (fm (truncate (5.5 * t)))] -- (forms 10)

forms : Int -> List Graphics.Collage.Form
forms ctr =
  if ctr == 0 then
    []
  else
    (fm ctr) :: (forms (ctr - 1))

fm : Int -> Graphics.Collage.Form
fm num =
  Graphics.Collage.move (toFloat num * 10, 0) (Graphics.Collage.text
    (Text.fromString (toString num)))


mainy : Signal Graphics.Element.Element
mainy =
  Signal.map2 doCollage Window.dimensions Mouse.position

doCollage : (Int, Int) -> (Int, Int) -> Graphics.Element.Element
doCollage (width, height) (x, y) =
  let h = height - 10
      w = width - 10
  in Graphics.Element.color Color.white <|
     Graphics.Collage.collage w h
     [Graphics.Collage.move(toFloat (x - w//2), toFloat (-y + h//2))
     <| Graphics.Collage.filled  Color.blue <| Graphics.Collage.circle 10
     ,
     (Graphics.Collage.traced
       (myLineStyle Color.lightGray)
       [(0,0),(200,300)])
     ]

myLineStyle : Color.Color -> Graphics.Collage.LineStyle
myLineStyle  clr =
  { myDefaultLine | color <- Color.gray }

myDefaultLine =
  Graphics.Collage.defaultLine
