import Graphics.Element
import Graphics.Collage
import Time
import Color
import Text
import Window

type alias Vec = (Float, Float)

main : Signal.Signal Graphics.Element.Element
main =
    doCollage (Time.fps 1) Window.dimensions

doCollage : Signal.Signal Time.Time -> Signal.Signal (Int, Int) -> Signal.Signal Graphics.Element.Element
doCollage timeSignal windowSignal =
  let
      staticCollageToElement fm fm2 (width, height) =
        Graphics.Collage.collage width height (List.concat [[fm], fm2])
  in
      Signal.map3 staticCollageToElement (moveForm timeSignal) (doGridLines windowSignal) windowSignal

moveForm : Signal.Signal Time.Time -> Signal.Signal (Graphics.Collage.Form)
moveForm timeSignal =
  let
      myForm =
        Graphics.Collage.toForm ( Graphics.Element.show "mike was here" )
  in
      Signal.map Basics.fst (Signal.foldp moveFormOneStep (myForm, (1, 1)) timeSignal)

moveFormOneStep : Float -> (Graphics.Collage.Form, Vec) -> (Graphics.Collage.Form, Vec)
moveFormOneStep t (fm, vec) =
  let
      newVec = (constrainMovement t vec)
      x = t / 500
  in
      (Graphics.Collage.move (x * fst newVec, 0) fm, newVec)


constrainMovement : Time.Time -> (Float, Float) -> (Float, Float)
constrainMovement t (direction, magnitude) =
  if | magnitude > 20 -> (-1, 20)
     | magnitude < 0 -> (1, 0)
     | otherwise -> (direction, magnitude + direction)

cellHeight = 17
cellWidth = 100

type alias Liner = Int -> (Int, Int) -> Graphics.Collage.Form

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



