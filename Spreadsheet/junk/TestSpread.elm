import Gridlines
import Graphics.Element
import Graphics.Collage
import Signal
import Window

main : Signal.Signal Graphics.Element.Element
main =
    Signal.map (Graphics.Collage.collage 500 500) (Gridlines.doGridLines Window.dimensions)