module Gridlines where

import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window

cellHeight = 17
cellWidth = 100

type alias Liner = Int -> (Int, Int) -> Graphics.Collage.Form

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

hfm : Liner
hfm ctr (width, height) =
    vhfm (width, height)
      [(0, toFloat ctr * cellHeight),(toFloat width, toFloat (ctr * cellHeight) )]

vfm : Liner
vfm ctr (width, height) =
    vhfm (width, height)
    [(toFloat ctr * cellWidth, 0),(toFloat (ctr * cellWidth), toFloat height)]

vhfm (width, height) path = 
    let
        myLineStyle  clr =
            let
                localDefaultLine =
                  Graphics.Collage.defaultLine
            in
                { localDefaultLine | color <- Color.gray }
    in
        Graphics.Collage.move ( -(toFloat width) / 2, -(toFloat height) / 2) 
          <| Graphics.Collage.traced (myLineStyle Color.gray) path



