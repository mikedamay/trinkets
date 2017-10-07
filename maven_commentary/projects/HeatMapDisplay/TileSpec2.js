describe ("Tile", function() {
   var Tile = speuappdev_heatMapEngine2_ns.testableState.Tile;
   it( "should return its position and dimensions", function() {
       var tile = Tile(10, 20, 30, 40 );
       expect(tile.get_left()).toEqual(10);
       expect(tile.get_top()).toEqual(20);
       expect(tile.get_width()).toEqual(30);
       expect(tile.get_height()).toEqual(40);
   });
    it("should return its area", function() {
        var tile = Tile(0,0, 5.5, 5.5 );
        expect( tile.get_area()).toEqual(5.5*5.5);
    });
});
describe ("Rectangle", function() {
   var Rectangle = speuappdev_heatMapEngine2_ns.testableState.Rectangle;
   it( "should return its position and dimensions", function() {
       var rect = Rectangle(10, 20, 30, 40 );
       expect(rect.get_left()).toEqual(10);
       expect(rect.get_top()).toEqual(20);
       expect(rect.get_width()).toEqual(30);
       expect(rect.get_height()).toEqual(40);
   });
    it("should return its area", function() {
        var rect = Rectangle(0,0, 5.5, 5.5 );
        expect( rect.get_area()).toEqual(5.5*5.5);
    });
    it("should be able to calculate remaining rectangle with height given", function() {
        var rect = Rectangle(30, 40, 500, 300 );
        var rectRem = rect.get_remainingRect('height', 70000 );
        expect(rectRem.get_area()).toEqual(((500 - (70000 / 300)) * 300));
        expect(rectRem.get_top()).toEqual(40);
        expect(rectRem.get_left()).toEqual(30 + (70000 / 300));
    });
    it("should be able to calculate remaining rectangle with width given", function() {
        var rect = Rectangle(30, 40, 500, 300 );
        var rectRem = rect.get_remainingRect('width', 70000 );
        expect(rectRem.get_area()).toEqual(((300 - (70000 / 500)) * 500));
        expect(rectRem.get_top()).toEqual(40 + (70000 / 500));
        expect(rectRem.get_left()).toEqual(30);
    });
    it( "should return the appropriate shortest side depending on its dimensions", function() {
        var rect = Rectangle(30, 40, 500, 300 );
        expect( rect.shortestSide() ).toEqual('height');
        rect = Rectangle(30, 40, 300, 500 );
        expect( rect.shortestSide() ).toEqual('width');
        rect = Rectangle(30, 40, 500, 500 );
        expect( rect.shortestSide() ).toEqual('height');
    });
});