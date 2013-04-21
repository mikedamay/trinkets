var speuappdev_heatMapEngine_ns = new function() {
    this.testableState = {};     // strictly for use of the test runner

    /// main entry point
    /// usage eg.: speuappdev_heatMapEngine_ns.drawHeatMap( [{area:49}...], {left:20...}); 
    this.drawHeatMap = function(heatMapData, paramsIn) {
        assert( typeof(paramsIn) === 'undefined'
          || typeof(paramsIn) === 'object'
          && typeof(paramsIn.left) === 'number'
          && typeof(paramsIn.top) === 'number'
          && typeof(paramsIn.width) === 'number'
          && typeof(paramsIn.height) === 'number'
          );

        var params = function(paramsArg) {
            var p;
            if ( typeof(paramsArg) === 'undefined') {
                p = { left:20, top:20, width:500, height:500 };
            }
            else {
                p = paramsArg;
            }
            if ( typeof( p.applyLogScale ) !== 'boolean') {
                p.applyLogScale = false;
            }
            if ( typeof( p.reverseDisplay ) !== 'boolean') {
                p.reverseDisplay = false;
            }
            if ( typeof( p.sortDirection ) !== 'string') {
                p.sortDirection = false;
            }
            return p;
        }(paramsIn);
        var lo = this.Layout(params.sortDirection,  SquarenessCalculator(params.applyLogScale));
        lo.addTiles(heatMapData);
        var rr = this.Renderer({left:params.left,top:params.top,width:params.width,height:params.height});
        rr.renderLayout(lo, params.reverseDisplay);
    };

    /// tile contains the data to draw an appropriate rectangle on the heatmap
    /// the layout contains columns and each column contains tiles.
    var Tile = this.testableState.Tile = function(spec) {
        assert( spec !== null, "the object passed to the tile constructor cannot be null");
        assert( typeof(spec.area) !== 'undefined', "the object passed to the tile constructor must contain a member 'area'");
        //assert( spec.area > 0, 'all tiles, tile(), must have an area greater than 0.  Otherwise, anarchy.');
        var publicState = {};
        publicState.get_area = function() {
            return spec.area;
        };
        /// if you add arbitrary porperties to the tile when you creeate it
        /// then you can query them with this
        publicState.get_property = function(propertyName) {
            return spec[propertyName];
        };
        return publicState;
    };

    /// a column contains one or more tiles.
    /// the heatmap is divided into a number of columsn
    var Column = this.testableState.Column = function() {
        var tiles = [];
        var publicState = {};
        var widthFraction = 1;
        var totalArea = 0;

        publicState.addTile = function( tile ) {
            assert( tile !== null, "the object passed to the column.addTile() cannot be null");
            assert( typeof(tile.get_area) === 'function', "the object passed to the column.addTile() must be a 'tile' and contain a member 'get_area'");
            assert( tile.get_area() > 0, 'all tiles, column.addTile(), must have an area greater than 0.  Otherwise, anarchy.');
            tiles.push(tile);
            totalArea += tile.get_area();
        };
        publicState.get_tile = function(index) {
            assert( index >= 0 && index < tiles.length, "column.get_tile index out of range");
            return tiles[index];
        };
        publicState.tileCount = function() {
            return tiles.length;
        };
        publicState.get_widthFraction = function() {
            return widthFraction;
        };
        publicState.set_widthFraction = function (w) {
            assert( typeof(w) === 'number', "column.get_widthFraction requires a number to be passed");
            assert( w > 0 && w <= 1, "something has gone horribly wrong. column.get_widthFraction requires a value between 0 and 1.  gets "+w);
            widthFraction = w;
        };
        publicState.get_area = function() {
            return totalArea;
        };
        return publicState;
    };

    /// this is a wrapper round a column object.  Pseudo columns are used when
    /// finding the best fit for a tile.  They enable us to do what-if assessments
    /// without affecting real columns
    /// @param column - is the real column that this pseudo column represents
    /// @param additionalArea - this is the change in the area caused by our
    /// what-if scenario.
    var PseudoColumn = this.testableState.PseudoColumn = function(column, additionalArea){
        var publicState = {};
        var widthFraction;

        publicState.get_area = function () {
            return column.get_area() + additionalArea;
        };
        publicState.get_widthFraction = function() {
            return widthFraction;
        };
        publicState.set_widthFraction = function (w) {
            assert( typeof(w) === 'number', "pseudoColumn.get_widthFraction requires a number to be passed");
            assert( w > 0 && w <= 1, "something has gone horribly wrong. pseudoColumn.get_widthFraction requires a value between 0 and 1.  gets "+w);
            widthFraction = w;
        };
        publicState.tileCount = function() {
            return column.tileCount() + (additionalArea > 0 ? 1 : 0);
        }
        publicState.get_tile = function(index) {
            if ( index >= 0 && index < column.tileCount() ) {
                return column.get_tile(index);
            }
            else if ( additionalArea > 0 && index === column.tileCount() ) {
                return Tile( {area:additionalArea} );
            }
            else
            {
                assert( false, "pseudoColumn.get_tile(): index out of range at " + index );
            }
        }
        return publicState;
    };

    /// column manager ensures that data bout the columns is consistent
    /// in particular the width fraction of each column.
    /// to create a normal column manager pass in no parameters.
    /// to create a pseudo column manager pass in the columns and the total area
    /// from the original column manager.
    var ColumnManager = this.testableState.ColumnManager
      = function(columnsArg, totalArea) {
        var publicState = {};
        publicState.testableState = {};
        if (typeof(totalArea) === 'undefined' ) {
            totalArea = 0;
        }
        var columns;
        if ( typeof(columnsArg) === 'undefined' ) {
            columns = [];
        }
        else {
            columns = columnsArg;
        }
        publicState.testableState.columns = columns;

        /// smae semantics as reduce() below
        var doReduce = function( fn, reduceValue, columns ) {
            for ( var ii = 0; ii < columns.length; ii++ ) {
                reduceValue = fn(columns[ii], reduceValue);
            }
            return reduceValue;
        };
        publicState.testableState.set_widthFraction = function(colIndex, w ) {
            columns[colIndex].set_widthFraction(w);
        };
        /// @param columns - an array of column ojbects
        //   - typically the layout's one and only array of column objects
        /// @param columnToChange - this is an index to the column where a pseudo tile will
        ///   be added typically to see the effect on the squareness.  If -1
        ///   is passed then an extra pseudo column will be created and included
        ///   in the returned array.  When not null the columnToChange must be
        ///   one of the columns in the array, columns.
        /// @param additionalArea - this is added into the pseudo column which is to change.
        ///   0 will be added to all other columns
        /// @returns an array of pseudoColumn objects.  Each pseudo column object
        ///    wraps one of the layout's column objects for every column object..
        var buildPseudoColumns = publicState.testableState.buildPseudoColumns
          = function (columns, columnToChange, additionalArea)
        {
            assert( typeof(additionalArea) === 'number', "Layout.buildPseudoColumns: the additional area must be a simple number");
            var pseudoColumns = [];
            for ( var ii = 0; ii < columns.length; ii++) {
                if (ii === columnToChange)
                {
                    pseudoColumns.push(PseudoColumn(columns[ii], additionalArea));
                }
                else
                {
                    pseudoColumns.push(PseudoColumn(columns[ii], 0));
                }
            }
            // after pseudo columns have been created for existing columns
            // optionally create a completely new column
            if ( columnToChange == -1 ) {
                var trialCol = Column();
                trialCol.addTile( Tile({area:additionalArea}));
                pseudoColumns.push(PseudoColumn(trialCol, 0));
                    // the underlying column is set up with the additional area
                    // so it's not required for the pseudo column - pass 0 instead.
            }
            return pseudoColumns;
        };
        /// make the width fraction associated with each column equal
        /// with the ratio of the area of that column divided by
        /// the total area
        var syncWidthFractions = publicState.testableState.syncWidthFractions
          = function() {
            var dummy;
            doReduce(function( column, dummy) {column.set_widthFraction( column.get_area() / totalArea)}
            , dummy, columns);
        };
        publicState.buildPseudoColumnManager = publicState.testableState.buildPseudoColumnManager
          = function(selectedColumn, additionalArea) {
            var pcols = buildPseudoColumns(columns, selectedColumn, additionalArea );
            var pcm = ColumnManager(pcols, totalArea + additionalArea);
            return pcm;
        };
        publicState.totalTileArea = function() {
            return totalArea;
        };
        var set_totalArea = publicState.testableState.set_totalArea
          = function(t) {
            totalArea = t;
        }
        /// executes the function fn for each tile in the column
        /// and accumulates the result in the object reduceValue
        publicState.reduce = function( fn, reduceValue ) {
            assert( typeof(fn) === 'function', "the first parameter to layout.reduce s/be a function");
            return doReduce(fn, reduceValue, columns);
        };
        publicState.columnCount = function() {
            return columns.length;
        };
        publicState.testableState.get_column = function(index) {
            assert( index >= 0 && index < columns.length, "ColumnManager.get_column index out of range");
            return columns[index];
        };
        publicState.addColumn = function( col ) {
            columns.push(col);
        }
        publicState.get_tile = function( colIndex, tileIndex) {
            assert( colIndex >= 0 && colIndex < columns.length
              , "ColumnManager.get_tile() column index out of range");
            return columns[colIndex].get_tile(tileIndex);
        };
        publicState.columnArea = function( colIndex) {
            assert( colIndex >= 0 && colIndex < columns.length
              , "ColumnManager.columnArea column index out of range");
            return columns[colIndex].get_area();
        };
        publicState.get_widthFraction = function( colIndex) {
            assert( colIndex >= 0 && colIndex < columns.length
              , "ColumnManager.get_widthFraction() column index out of range");
            return columns[colIndex].get_widthFraction();
        };
        publicState.tileCount = function( colIndex ) {
            assert( colIndex >= 0 && colIndex < columns.length
              , "ColumnManager.tileCount() index out of range");
            return columns[colIndex].tileCount();
        }
        /// @param index - column to which tile will be added, -1 indicates create a new column
        publicState.addTile = function( index, tile ) {
            assert( index === -1 || index >= 0 && index < columns.length
              ,"ColumnManager.addTile: index out of range - " + index );
            var col;
            if ( index === -1 ) {
                col = Column();
                columns.push(col);
            }
            else
            {
                col = columns[index];
            }
            col.addTile(tile);
            totalArea += tile.get_area();
            syncWidthFractions();
        };
        syncWidthFractions();
        return publicState;
    };

    var SquarenessCalculator = this.testableState.SquarenessCalculator = function(applyLogScale) {
        var publicState = {};
        if ( typeof(applyLogScale) === 'undefined' ) {
            applyLogScale = false;
        }
        publicState.calculateSquareness
          = function(columnManager) {
            var areaAdjuster = applyLogScale ? function(ta) {return Math.log(1+ta);} : function(ta) {return ta;};
            var squareness = 0;
            for ( var ii = 0; ii < columnManager.columnCount(); ii++ ) {
                var widthFraction = columnManager.get_widthFraction(ii);
                assert( widthFraction > 0, "calculateSquareness: widthFraction <= 0 - BAD");
                var columnArea = columnManager.columnArea(ii);
                for ( var jj = 0; jj < columnManager.tileCount(ii); jj++ ) {
                    var tile = columnManager.get_tile(ii, jj);
                    var tileArea = tile.get_area();
                    var heightFraction = tileArea / columnArea;
                    assert( widthFraction > 0, "calculateSquareness: heightFraction <= 0 - BAD");
                    var ratio = heightFraction / widthFraction;
                    if ( ratio < 1 ) {
                        ratio = 1 / ratio;
                    }
                    squareness += (ratio * areaAdjuster(tileArea));
                }
            }
            return squareness;
        };
        return publicState;
    };

    /// layout is the model for the heatmap.  It is divided into
    /// columns which contain the rectangles (tiles)
    this.Layout = function ( sortDirection, squarenessCalculatorArg) {
        var publicState = {};
        publicState.testableState = {};
        var columnManager = publicState.testableState.columnManager = ColumnManager();
        var squarenessCalculator;
        if ( typeof(squarenessCalculatorArg) === 'undefined') {
            squarenessCalculator = SquarenessCalculator();
        }
        else {
            squarenessCalculator = squarenessCalculatorArg;
        }
        var comparer;
        if ( sortDirection && sortDirection === "asc") {
            comparer = function(a,b) {return a.area - b.area};
                // a - b == reverse order of area
        }
        else if ( sortDirection && sortDirection === 'desc') {
            comparer = function(a,b) {return b.area - a.area};
        }
        else {
            comparer = null;
        }

        /// key routine that determines which column a new tile of the additionalArea should
        /// be added to by trying to add it to each column.  The trial that results in the layout
        /// with the least "squareness" indicaes which column should be the target.
        /// @returns -1 if a new column should be created, otherwise the index of the column
        ///   to which the tile should be added
        var findBestColumn = publicState.testableState.findBestColumn = function(additionalArea, columnManager) {
            assert( typeof(additionalArea) === 'number');
            if (columnManager.columnCount() === 0 ) {
                return -1;        // this is the first tile to ba added to the layout
                                    // so it can only go in one place - a new column, columns[0]
            }
            var rowCount = columnManager.tileCount(0);
            if (rowCount === 0 ) {
                return 0;  // not sure what we're doing here as an empty column should never exist
            }
            var squareness = tryFit(columnManager, -1, additionalArea);
                    // start by seeing what happens if we add a new column
            var bestColumnNo = -1;
            var trialSquareness;
            for ( var ii = 0; ii < columnManager.columnCount(); ii++ ) {
                trialSquareness = tryFit( columnManager, ii, additionalArea );
                if ( trialSquareness < squareness )
                {
                    squareness = trialSquareness;
                    bestColumnNo = ii;
                }
            }
            return bestColumnNo;
        };

        var tryFit = publicState.testableState.tryFit
            = function( columnManager, columnToChange, additionalArea ) {
            var pseudoColumnManager = columnManager.buildPseudoColumnManager(columnToChange, additionalArea);
            var squareness = squarenessCalculator.calculateSquareness( pseudoColumnManager);
            return squareness;
        };

        /// @param tileSpecs - e.g. [{area:49}, {area:17}, {area:1}]
        ///   the elements of the array can include arbitrary properties
        publicState.addTiles = function(tileSpecs) {
            if ( comparer ) {
                tileSpecs.sort(comparer);
            }
            for ( var ii = 0; ii < tileSpecs.length; ii++ ) {
                publicState.addTile(tileSpecs[ii]);
            }
        };
        publicState.addTile = function(tileSpec) {
            assert( tile !== null, "the object passed to the layout.addTile() cannot be null");
            var tile = Tile(tileSpec);
            if ( tile.get_area() <= 0 ) {
                return;     // quietly forget about zero area tiles
            }
            var selectedColumn;
            selectedColumn = findBestColumn(tile.get_area(), columnManager);
            columnManager.addTile(selectedColumn, tile);
        };
        publicState.columnCount = function() {
            return columnManager.columnCount();
        };
        publicState.totalTileArea = function() {
            return columnManager.totalTileArea();
        };
        publicState.get_tile = function( colIndex, tileIndex) {
            return columnManager.get_tile(colIndex, tileIndex);
        };
        publicState.columnArea = function( colIndex) {
            return columnManager.columnArea(colIndex);
        };
        publicState.tileCount = function( colIndex ) {
            return columnManager.tileCount(colIndex);
        }
        publicState.get_widthFraction = function( colIndex ) {
            return columnManager.get_widthFraction(colIndex);
        }
        return publicState;
    };

    /// this has sole charge of rendering the constructed layout to the browser page
    this.Renderer = function(canvasRect) {
        assert( typeof(canvasRect.left) === 'number' && typeof(canvasRect.top) === 'number'
          && typeof(canvasRect.width) === 'number' && typeof(canvasRect.height) === 'number'
          ,"canvasRect passed to renderer constructor must contain left, top, width, height");
        assert( canvasRect.width > 0 && canvasRect.height > 0 );

        var publicState = {};

        var renderTile = function(tile, left, top, height, width ) {
           writeTileHTML(left, top, width, height, 0 );
        };
        var writeTileHTML = function( left, top, width, height, backgroundColor ) {
            debugPrint( "left="+left+" top="+top+" width="+width+" height=" +height);

            var divTag = document.createElement("div");
            divTag.style.position = "absolute";
            divTag.style.left = left+"px";
            divTag.style.top = top+"px";
            divTag.style.width = width+"px";
            divTag.style.height = height+"px";
            divTag.style.border = "2px solid rgb(153,0,153)";
            divTag.style.backgroundColor = "white";
            divTag.innerHTML = "xx";
            var divDataPanel = document.getElementById("DataPanel");
            divDataPanel.appendChild(divTag);
            //document.body.appendChild(divTag);
        };
        publicState.renderLayout = function( layout, reverse ) {
            try {
                var scale = canvasRect.width * canvasRect.height / (layout.totalTileArea());
                var left = canvasRect.left;
                var ii;
                var jj;
                if ( typeof(reverse) === 'undefined' ) {
                    reverse = false;
                }
                var firstcol = reverse ? function() { ii = layout.columnCount() - 1 } : function() { ii = 0 };
                var lastcol = reverse ? function() { return ii >= 0 } : function() { return ii < layout.columnCount() };
                var inccol = reverse ? function() { ii-- } : function() { ii++ };
                var firsttile = reverse ? function() { jj = layout.tileCount(ii) - 1 } : function() { jj = 0 };
                var lasttile = reverse ? function() { return jj >= 0 } : function() { return jj < layout.tileCount(ii) };
                var inctile = reverse ? function() { jj-- } : function() { jj++ };
                for ( firstcol(); lastcol(); inccol() ) {
                    var top = canvasRect.top;
                    var width = canvasRect.width * layout.get_widthFraction(ii);
                    debugPrint( "renderLayout col="+ii+" width="+width);
                    for ( firsttile(); lasttile(); inctile() ) {
                        var height = (layout.get_tile(ii, jj).get_area() / width)*scale;
                        renderTile(layout.get_tile(ii, jj), left, top, height, width );
                        top += height;
                    }
                    left += width;
                }
            }
            catch ( ex ) {
                alert( "renderLayout:" + ex );
            }
        };
        return publicState;
    };


    var debugPrint = function(str) {
//        document.writeln( str + "<br>" );
    };
    var assert = function(cond, message ) {
        if (!cond ) {
            throw { name: "assertionFailure", message: message };
        }
    };
};
