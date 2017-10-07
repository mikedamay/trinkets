describe( "Layout", function() {
    var Layout = speuappdev_heatMapEngine2_ns.Layout;
    var Rectangle = speuappdev_heatMapEngine2_ns.testableState.Rectangle;
    it ( "should be able to calculate the total area of new tiles", function() {
        var lo = Layout([{area:100}]);
        var commitTiles = lo.testableState.commitTiles;
        var committedTiles = [];
        var totalArea = commitTiles(
          [{area:100}], 1, Rectangle(0,0,50,50),'height', 50, committedTiles );
        expect( totalArea ).toEqual(100); 
        totalArea = commitTiles(
          [{area:100}, {area:50}], 2, Rectangle(0,0,50,50),'height', 50, committedTiles );
        expect( totalArea ).toEqual(150);
        totalArea = commitTiles(
        [{area:100}, {area:50}], 1, Rectangle(0,0,50,50),'height', 50, committedTiles );
        expect( totalArea ).toEqual(100);
    });
    it ( "should be able to calculate the position and dimensions of new tiles placed vertically", function() {
        var lo = Layout([{area:100}]);
        var commitTiles = lo.testableState.commitTiles;
        var committedTiles = [];
        var totalArea = commitTiles(
          [{area:9}, {area:9}, {area:9}], 3, Rectangle(20,30,25,9),'height', 9, committedTiles );
        expect( committedTiles.length ).toEqual(3);
        expect( committedTiles[0].get_top() ).toEqual(30);
        expect( committedTiles[1].get_top() ).toEqual(33);
        expect( committedTiles[2].get_top() ).toEqual(36);
        expect( committedTiles[2].get_left() ).toEqual(20);
    });
    it ( "should be able to calculate the position and dimensions of new tiles placed horizontally", function() {
        var lo = Layout();
        var commitTiles = lo.testableState.commitTiles;
        var committedTiles = [];
        var totalArea = commitTiles(
          [{area:9}, {area:9}, {area:9}], 3, Rectangle(20,30,9,25),'width', 9, committedTiles );
        expect( committedTiles.length ).toEqual(3);
        expect( committedTiles[0].get_left() ).toEqual(20);
        expect( committedTiles[1].get_left() ).toEqual(23);
        expect( committedTiles[2].get_left() ).toEqual(26);
        expect( committedTiles[2].get_top() ).toEqual(30);
    });
    it ( "should be able to create tiles for the heatmap data", function() {
        var lo = Layout();
        var tiles = lo.layoutTiles(
          [
          {area:4}
          ,{area:4}
          ,{area:4}
          ,{area:4}
          ]
          );
        expect( tiles[0].get_area()).toEqual(4);
        expect( tiles[3].get_area()).toEqual(4);
        expect( tiles[3].get_left()).toEqual(2);
        expect( tiles[3].get_top()).toEqual(2);

    });
});