import Random exposing (..)
import Graphics.Element

type alias Model =
        { badGuys : List (Float,Float)
        , seed : Seed
        }

possiblyAddBadGuy : Model -> Model
possiblyAddBadGuy model =
        let (addProbability, seed') =
              generate (float 0 1) model.seed
        in
            if addProbability < 0.9
              then
                { model |
                    seed <- seed'
                }
              else
                let (position, seed'') =
                      generate (pair (float 0 100) (float 0 100)) seed'
                in
                    { model |
                        badGuys <- (position :: model.badGuys),
                        seed <- seed''
                    }

main : Graphics.Element.Element
main =
    let mod = {badGuys = [(1.0,1.0)], seed=(initialSeed 31415)}
    in Graphics.Element.show (possiblyAddBadGuy mod)