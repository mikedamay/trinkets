module Grid where

import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window

cellHeight = 21
cellWidth = 100
rowHeaderWidth = 50
colHeaderHeight = 25

type alias GridCoords = { headers : {width : Int, height : Int }, horz : List Int, vert : List Int }

getGridCoords : (Int, Int) -> GridCoords
getGridCoords (width, height) =
        let
            numCols = (width - rowHeaderWidth) // cellWidth
            numRows = (height - colHeaderHeight) // cellHeight
            spacer numRowsOrCols space =
                let
                    multiplyBySpace n = n * space
                in
                    List.map multiplyBySpace [0..numRowsOrCols]
        in
            { headers = { width = rowHeaderWidth, height = colHeaderHeight }
              ,horz = (spacer numRows cellHeight), vert = (spacer numCols cellWidth) }

initialGridCoords = {headers={width = rowHeaderWidth, height = colHeaderHeight}, horz=[], vert=[]}







