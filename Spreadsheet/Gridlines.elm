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
          makeGridline (w, h) (toFloat gridCoords.headers.width, toFloat gridCoords.headers.height)
          [(toFloat pos, 0),(toFloat pos, toFloat h)]
        makeHorzs (w, h) pos =
          makeGridline (w, h) (toFloat gridCoords.headers.width, toFloat gridCoords.headers.height)
          [(0, (toFloat (height - pos))), ((toFloat w), (toFloat (height - pos)))]
    in
        List.concat [
        List.map (makeVerts (width, height)) gridCoords.vert
         ,List.map (makeHorzs (width, height)) gridCoords.horz
         ,List.map (makeRowHeader (width, height)
                                  (gridCoords.headers.width, gridCoords.headers.height))
                                  gridCoords.horz
         ,List.map (makeColHeader (width, height)
                                  (gridCoords.headers.width, gridCoords.headers.height))
                                  gridCoords.vert
         ]

makeGridline : (Int, Int) -> (Float, Float)-> Graphics.Collage.Path -> Graphics.Collage.Form
makeGridline (width, height) (headerWidth, headerHeight) path =
    let
        myLineStyle : Color.Color -> Graphics.Collage.LineStyle
        myLineStyle  clr =
            let
                localDefaultLine =
                  Graphics.Collage.defaultLine
            in
                { localDefaultLine | color <- Color.gray }
    in
        Graphics.Collage.move ( -(toFloat width) / 2 + headerWidth, -(toFloat height) / 2 - headerHeight)
          <| Graphics.Collage.traced (myLineStyle Color.gray) path

makeRowHeader : (Int, Int) -> (Int, Int) -> Int -> Graphics.Collage.Form
makeRowHeader (width, height) (headerWidth, headerHeight) rowPos =
    makeHeaderForm (width, height)
      (offsetBy (0,toFloat headerHeight) [(0, (toFloat rowPos)), ((toFloat headerWidth), (toFloat rowPos))])

makeColHeader : (Int, Int) -> (Int, Int) -> Int -> Graphics.Collage.Form
makeColHeader (width, height) (headerWidth, headerHeight) colPos =
    makeHeaderForm (width, height)
      (offsetBy (toFloat headerWidth,0) [(toFloat colPos, 0), ((toFloat colPos), (toFloat headerHeight))])

makeHeaderForm : (Int, Int) -> Graphics.Collage.Path -> Graphics.Collage.Form
makeHeaderForm (width, height) path =
    let
        myLineStyle : Color.Color -> Graphics.Collage.LineStyle
        myLineStyle  clr =
            let
                localDefaultLine =
                  Graphics.Collage.defaultLine
            in
                { localDefaultLine | color <- Color.gray }
    in
        Graphics.Collage.traced (myLineStyle Color.gray) (windowToCollage (width, height) path)

offsetBy : (Float, Float) -> Graphics.Collage.Path -> Graphics.Collage.Path
offsetBy ((offX, offY) as offset) path =
    List.map (\(x, y) -> (x + offX, y + offY)) path

-- convert window coordinates into collage coordinates
-- collage origin (0,0) is in the centre of the collage.  Window origin top left
-- to map from window to collage x : subtract half the width from the coord
--                               y : subtract the coord from half the height
-- e.g. for a window 600 across x 800 down
--      0,0 -> -300,400
--      20,30 -> -280, 370
windowToCollage : (Int, Int) -> Graphics.Collage.Path -> Graphics.Collage.Path
windowToCollage (windowWidth, windowHeight) path =
    let
        mapPath (windowWidth, windowHeight) path =
            List.map (\(coordX, coordY) -> (-windowWidth / 2 + coordX, windowHeight / 2 - coordY)) path
    in
        mapPath (toFloat windowWidth, toFloat windowHeight) path