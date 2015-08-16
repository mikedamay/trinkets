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
makeRowHeader (width, height) (headerWidth, headerHeight) row =
    makeRowHeaderForm (width, height) headerHeight
      [(0, (toFloat (height - row))), ((toFloat headerWidth), (toFloat (height - row)))]

makeRowHeaderForm : (Int, Int) -> Int -> Graphics.Collage.Path -> Graphics.Collage.Form
makeRowHeaderForm (width, height) headerHeight path =
    let
        myLineStyle : Color.Color -> Graphics.Collage.LineStyle
        myLineStyle  clr =
            let
                localDefaultLine =
                  Graphics.Collage.defaultLine
            in
                { localDefaultLine | color <- Color.gray }
    in
        Graphics.Collage.move ( -(toFloat width) / 2, -(toFloat height) / 2 - (toFloat headerHeight))
          <| Graphics.Collage.traced (myLineStyle Color.gray) path

makeColHeader : (Int, Int) -> (Int, Int) -> Int -> Graphics.Collage.Form
makeColHeader (width, height) (headerWidth, headerHeight) colPos =
    makeColHeaderForm (width, height) headerWidth headerHeight
      [(toFloat colPos, 0), ((toFloat colPos), (toFloat headerHeight))]

makeColHeaderForm : (Int, Int) -> Int -> Int -> Graphics.Collage.Path -> Graphics.Collage.Form
makeColHeaderForm (width, height) headerWidth headerHeight path =
    let
        myLineStyle : Color.Color -> Graphics.Collage.LineStyle
        myLineStyle  clr =
            let
                localDefaultLine =
                  Graphics.Collage.defaultLine
            in
                { localDefaultLine | color <- Color.gray }
    in
        Graphics.Collage.move ( -(toFloat width) / 2 + (toFloat headerWidth)
          ,(toFloat height) / 2 - (toFloat headerHeight))
          <| Graphics.Collage.traced (myLineStyle Color.gray) path
