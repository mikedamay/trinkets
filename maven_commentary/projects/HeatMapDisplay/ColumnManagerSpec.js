describe("ColumnManager", function() {
    var ColumnManager = speuappdev_heatMapEngine_ns.testableState.ColumnManager;
    var Column = speuappdev_heatMapEngine_ns.testableState.Column;
    var Tile = speuappdev_heatMapEngine_ns.testableState.Tile;

    it( "should add tiles consistently", function() {
        var cm = ColumnManager();
        cm.addTile(-1, Tile({area:44}));
        cm.addTile(0, Tile({area:22}));
        cm.addTile(-1, Tile({area:11}));
        expect(cm.totalTileArea()).toEqual(77);
        expect( cm.columnCount()).toEqual(2);
    });
    it("should be able to adjust width fractions to equal 1", function() {
        var cm = ColumnManager();
        var syncWidthFractions = cm.testableState.syncWidthFractions;
        var set_totalArea = cm.testableState.set_totalArea;
        var get_column = cm.testableState.get_column;
        cm.addColumn(Column());
        get_column(0).addTile(Tile({area:80}));
        cm.addColumn(Column());
        get_column(1).addTile(Tile({area:20}));
        set_totalArea(80+20);
        syncWidthFractions( );
        expect(get_column(0).get_widthFraction()).toEqual(0.8);
        expect(get_column(1).get_widthFraction()).toEqual(0.2);
    });
    it("should be able to create an array of pseudo columns for squareness trials", function() {
        var cm = ColumnManager();
        var buildPseudoColumnManager = cm.testableState.buildPseudoColumnManager;
        var set_totalArea = cm.testableState.set_totalArea;
        var get_column = cm.testableState.get_column;
        cm.addColumn(Column());
        get_column(0).addTile(Tile({area:80}));
        get_column(0).set_widthFraction( 0.8 );
        cm.addColumn(Column());
        get_column(1).addTile(Tile({area:20}));
        get_column(1).set_widthFraction( 0.2 );
        set_totalArea(80+20);
        var pcols = buildPseudoColumnManager( 0, 55 );
        expect( pcols.columnArea(0) ).toEqual(80+55);
        pcols = buildPseudoColumnManager( -1, 23 );
        expect( pcols.columnArea(2) ).toEqual(23);
    });
});