describe("Layout", function() {
    var Layout = speuappdev_heatMapEngine_ns.Layout;
    var ColumnManager = speuappdev_heatMapEngine_ns.testableState.ColumnManager;
    var Tile = speuappdev_heatMapEngine_ns.testableState.Tile;

    it( "should be able to calculate the total area of the contained tiles", function() {
        var lo = Layout();
        lo.addTile({area:10000});
        expect(lo.totalTileArea()).toEqual(10000);
        lo.addTile({area:49});
        expect(lo.totalTileArea()).toEqual(10049);
        lo.addTile({area:1});
        expect(lo.totalTileArea()).toEqual(10050);
        lo.addTile({area:0.0005});
        expect(lo.totalTileArea()).toEqual(10050.0005);
    });
    it( "should be able to find the best column to fit a tile into the square", function() {
        var lo = Layout();
        var findBestColumn = lo.testableState.findBestColumn;
//        var columns = lo.testableState.columns;
        var cm = lo.testableState.columnManager;
        lo.addTile({area:49});
        var col = findBestColumn(14, cm);
        expect( col ).toEqual(-1);
        lo.addTile({area:14});
        var col = findBestColumn(13, cm);
        expect( col ).toEqual(1);
    });
    it( "should accept tiles in a bunch", function() {
        var lo = Layout();
        lo.addTiles(
            [
                { area:100}
                , {area:200}
                , {area:300}
                , {area:400}
                , {area:500}
                , {area:600}
                , {area:700}
            ]
        );
        expect(lo.totalTileArea()).toEqual(100+200+300+400+500+600+700);
    });
});