describe( "column", function() {
    var Column = speuappdev_heatMapEngine_ns.testableState.Column;
    var Tile = speuappdev_heatMapEngine_ns.testableState.Tile;

    it("should be able to add, count and get a tile", function() {
        var col = Column();
        expect(col.tileCount()).toEqual(0);
        col.addTile(Tile({area:49}));
        expect(col.tileCount()).toEqual(1);
        expect(col.get_tile(0).get_area()).toEqual(49);
    });
    it("should be able to get and set a width fraction", function()
    {
        var col = Column();
        col.set_widthFraction(0.1);
        expect( col.get_widthFraction()).toEqual(0.1);
    });
    it("should be able to calculate the aggregate area of its child tiles", function() {
        var col = Column();
        col.addTile(Tile({area:49}));
        col.addTile(Tile({area:10000}));
        col.addTile(Tile({area:.0005}));
        expect( col.get_area()).toEqual(10049.0005);
    });
});