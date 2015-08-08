module Gridlines where

import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window


main : Signal.Signal Graphics.Element.Element
main =
    Signal.map2 setWindowAsCollage Window.dimensions 
       (makeGridlines (getGridCoords Window.dimensions) Window.dimensions)

setWindowAsCollage : (Int, Int) 
                     -> List Graphics.Collage.Form 
                     -> Graphics.Element.Element
setWindowAsCollage windowDimensions gridlines =
    Graphics.Collage.collage (fst windowDimensions) (snd windowDimensions) gridlines


cellHeight = 17
cellWidth = 100

type alias Liner = Int -> (Int, Int) -> Graphics.Collage.Form
type alias LineSpacer = Int -> (Int, Int) -> List Int
type alias GridCoords = { horz : List Int, vert : List Int }

getGridCoords : Signal.Signal (Int, Int) -> Signal.Signal GridCoords
getGridCoords winDims =
    let
        getGridCoords' : (Int, Int) -> GridCoords
        getGridCoords' (width, height) =
            let
                numCols = width // cellWidth
                numRows = height // cellHeight
                spacer numRowsOrCols space =
                    let 
                        multiplyBySpace n = n * space
                    in
                        List.map multiplyBySpace [1..numRowsOrCols]
            in               
                { horz = (spacer numRows cellHeight), vert = (spacer numCols cellWidth) }
    in  
        Signal.map getGridCoords' winDims 


makeGridlines : Signal.Signal GridCoords 
                -> Signal.Signal (Int, Int) 
                -> Signal.Signal (List Graphics.Collage.Form)
makeGridlines gridCoords windowDimensions =
    let
        makeGridlines' gridCoords (width, height) =
            let
                makeVerts (w, h) pos =
                  makeGridline (w, h) [(toFloat pos, 0),(toFloat pos, toFloat h)]
                makeHorzs (w, h) pos =
                  makeGridline (w, h) [(0, (toFloat pos)), ((toFloat w), (toFloat pos))]
            in            
                List.concat [List.map (makeVerts (width, height)) gridCoords.vert
                 ,List.map (makeHorzs (width, height)) gridCoords.horz]
    in
        Signal.map2 makeGridlines' gridCoords windowDimensions

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






