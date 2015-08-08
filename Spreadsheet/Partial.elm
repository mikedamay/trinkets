import Graphics.Element
import Graphics.Collage
import Text


main : Graphics.Element.Element
main = Graphics.Collage.collage 100 100
         [Graphics.Collage.text (Text.fromString (toString doPartial))]

doPartial : Int
doPartial =
  let
    add5 = add 5
  in
    add5 10
    
    
add : Int -> Int -> Int
add a b = a + b
