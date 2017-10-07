describe( "SquarenessCalculator", function() {
    var SquarenessCalculator = speuappdev_heatMapEngine2_ns.testableState.SquarenessCalculator;
    var Sequence = speuappdev_heatMapEngine2_ns.testableState.Sequence;

    it ( "should be able to calculate squareness for a given array of areas", function() {
        var sc = SquarenessCalculator();
        var squareness = sc.testableState.squareness;
        var s1 = squareness(
          [{area:200}
          ,{area:200}
          ,{area:200}
          ,{area:200}
          ]
          , {area:0}, 300
          );
        expect(s1).toEqual((75/(200/75))*4);
    });
    it ( "should be able to calculate squareness for a given array of areas with an extra area thrown in", function() {
        var sc = SquarenessCalculator();
        var squareness = sc.testableState.squareness;
        var s1 = squareness(
          [{area:200}
          ,{area:200}
          ,{area:200}
          ,{area:200}
          ]
          , {area:800}, 300
          );
        expect(s1).toEqual(((300*0.125)/(200/(300*0.125)))*4 + 150/(800/150));
    });
    it ( "should include all the areas when the length will provoke an exact square"
      , function() {
        var sc = SquarenessCalculator();
        var numAreas = sc.squarestTileCount(
            Sequence(
              [
                  {area:200}
                  ,{area:200}
                  ,{area:200}
                  ,{area:200}
              ]
              )
              ,Math.sqrt(200) * 4
          );
        expect( numAreas ).toEqual(4);
    });
    it ( "should include all areas when the length is very large"
      , function() {
        var sc = SquarenessCalculator();
        var numAreas = sc.squarestTileCount(
            Sequence(
              [
                  {area:200}
                  ,{area:200}
                  ,{area:200}
                  ,{area:200}
              ]
              )
              ,1000
          );
        expect( numAreas ).toEqual(4);
    });
    it ( "should be able to determine the number of areas constituting the squarest configuration for a given length"
      , function() {
        var sc = SquarenessCalculator();
        var numAreas = sc.squarestTileCount(
            Sequence(
              [
                  {area:200}
                  ,{area:200}
                  ,{area:200}
                  ,{area:200}
              ]
              )
              ,Math.sqrt(200) * 2

          );
        expect( numAreas ).toEqual(2);
    });
});
