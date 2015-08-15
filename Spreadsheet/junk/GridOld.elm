module Gridlines where

import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window


main : Signal.Signal Graphics.Element.Element
main =
    Signal.map (Graphics.Collage.collage 500 500) 
      (merge (doGridLines Window.dimensions) (makeGridlines (getGridCoordsWrapper Window.dimensions) Window.dimensions))


merge : Signal.Signal (List Graphics.Collage.Form) 
        -> Signal.Signal (List Graphics.Collage.Form) 
        -> Signal.Signal (List Graphics.Collage.Form)
merge l gc =
    let
        -- mergeOne fm coords = List.concat [fm, coords]
        mergeOne fm coords = coords
    in          
        Signal.map2 mergeOne l gc



cellHeight = 17
cellWidth = 100

type alias Liner = Int -> (Int, Int) -> Graphics.Collage.Form
type alias LineSpacer = Int -> (Int, Int) -> List Int
type alias GridCoords = { horz : List Int, vert : List Int }

{-
    takes a signal of dimensions and returns a set of grid lines fitting those dimensions
-}
doGridLines : Signal.Signal (Int, Int) -> Signal.Signal (List Graphics.Collage.Form)
doGridLines windowSignal =
    let
        doGridLinesEx (width, height) =
            let
                vgl = horzOrVert (width // cellWidth)
                hgl = horzOrVert (height // cellHeight)
            in
                List.concat [(vgl (width, height) vfm),
                (hgl (width, height) hfm)]
    in
        Signal.map doGridLinesEx windowSignal

horzOrVert : Int -> (Int, Int) -> Liner -> List Graphics.Collage.Form
horzOrVert ctr (width, height) fm =
  if ctr == 0 then
    []
  else
    (fm ctr (width, height)) :: (horzOrVert (ctr - 1) (width, height) fm)


getGridCoordsWrapper : Signal.Signal (Int, Int) -> Signal.Signal GridCoords
getGridCoordsWrapper winDims =
    Signal.map getGridCoords winDims 

getGridCoords : (Int, Int) -> GridCoords
getGridCoords (width, height) =
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
      

hfm : Liner
hfm ctr (width, height) =
    vhfm (width, height)
      [(0, toFloat ctr * cellHeight),(toFloat width, toFloat (ctr * cellHeight) )]

vfm : Liner
vfm ctr (width, height) =
    vhfm (width, height)
    [(toFloat ctr * cellWidth, 0),(toFloat (ctr * cellWidth), toFloat height)]

vhfm : (Int, Int) -> Graphics.Collage.Path -> Graphics.Collage.Form
vhfm (width, height) path = 
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

makeGridlines : Signal.Signal GridCoords 
                -> Signal.Signal (Int, Int) 
                -> Signal.Signal (List Graphics.Collage.Form)
makeGridlines gridCoords windowDimensions =
    Signal.map2 doMakeGridlines gridCoords windowDimensions


doMakeGridlines : GridCoords -> (Int, Int) -> List Graphics.Collage.Form
doMakeGridlines gridCoords (width, height) =
    let
        makeVerts (w, h) pos =
            vhfm (w, h) [(toFloat pos, 0),(toFloat pos, toFloat h)]
        makeHorzs (w, h) pos =
            vhfm (w, h) [(0, (toFloat pos)), ((toFloat w), (toFloat pos))]
    in            
        List.concat [List.map (makeVerts (width, height)) gridCoords.vert
         ,List.map (makeHorzs (width, height)) gridCoords.horz]





