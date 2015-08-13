module Grid where

import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window

cellHeight = 17
cellWidth = 100

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

getGridCoordsEx : (Int, Int) -> GridCoords
getGridCoordsEx (width, height) =
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








