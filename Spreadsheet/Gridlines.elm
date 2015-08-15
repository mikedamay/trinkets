module Gridlines where
import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window
import Grid exposing (GridCoords, getGridCoords)

makeGridlines gridCoords (width, height) =
    let
        makeVerts (w, h) pos =
          makeGridline (w, h) [(toFloat pos, 0),(toFloat pos, toFloat h)]
        makeHorzs (w, h) pos =
          makeGridline (w, h) [(0, (toFloat (height - pos))), ((toFloat w), (toFloat (height - pos)))]
    in
        List.concat [List.map (makeVerts (width, height)) gridCoords.vert
         ,List.map (makeHorzs (width, height)) gridCoords.horz]

makeGridline : (Int, Int) -> Graphics.Collage.Path -> Graphics.Collage.Form
makeGridline (width, height) path =
    let
        myLineStyle : Color.Color -> Graphics.Collage.LineStyle
        myLineStyle  clr =
            let
                localDefaultLine =
                  Graphics.Collage.defaultLine
            in
                { localDefaultLine | color <- Color.gray }
    in
        Graphics.Collage.move ( -(toFloat width) / 2, -(toFloat height) / 2)
          <| Graphics.Collage.traced (myLineStyle Color.gray) path