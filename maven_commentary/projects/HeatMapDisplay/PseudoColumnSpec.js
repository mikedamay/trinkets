describe("pseudoColumn", function() {
    var PseudoColumn = speuappdev_heatMapEngine_ns.testableState.PseudoColumn;
    var Column = speuappdev_heatMapEngine_ns.testableState.Column;
    var Tile = speuappdev_heatMapEngine_ns.testableState.Tile;

    it("should be able to be manipulated without affecting underlying column", function() {
        var columns = [
            Column()
            ,Column()
            ,Column()
        ];
        var pseudoColumns = [
            PseudoColumn(columns[0], 0)
            ,PseudoColumn(columns[1], 10)
            ,PseudoColumn(columns[2], 0)
        ];
        columns[0].addTile(Tile({area:17}));
        columns[1].addTile(Tile({area:19}));
        columns[2].addTile(Tile({area:33}));
        expect( pseudoColumns[0].get_area()).toEqual(17);
        expect( pseudoColumns[1].get_area()).toEqual(29);
        expect( pseudoColumns[2].get_area()).toEqual(33);
        var colTot = 17+19+33;
        var pseuTot = 17+19+10+33;
        columns[0].set_widthFraction(17/colTot);
        columns[1].set_widthFraction(19/colTot);
        columns[2].set_widthFraction(33/colTot);
        pseudoColumns[0].set_widthFraction(17/pseuTot);
        pseudoColumns[1].set_widthFraction(29/pseuTot);
        pseudoColumns[2].set_widthFraction(33/pseuTot);
        expect( pseudoColumns[0].get_widthFraction()).toEqual(17/pseuTot);
        expect( pseudoColumns[1].get_widthFraction()).toEqual(29/pseuTot);
        expect( pseudoColumns[2].get_widthFraction()).toEqual(33/pseuTot);
        expect( columns[0].get_widthFraction()).toEqual(17/colTot);
        expect( columns[1].get_widthFraction()).toEqual(19/colTot);
        expect( columns[2].get_widthFraction()).toEqual(33/colTot);
    });
    it( "should be able to serve up the additional area as an extra tile", function() {
        var col = Column();
        col.addTile( Tile({area:22}) );
        var pcol = PseudoColumn(col, 23 );
        expect( pcol.tileCount()).toEqual(2);
        expect( pcol.get_tile(1).get_area()).toEqual(23);
        expect( pcol.get_tile(0).get_area()).toEqual(22);
    });
});