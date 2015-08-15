module Mm where

import Graphics.Element
import Mouse

main : Signal Graphics.Element.Element
main =
  Signal.map Graphics.Element.show Mouse.position
  
