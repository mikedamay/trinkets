var speuappdev_heatMapEngine2_ns = new function() {
    this.testableState = {};     // strictly for use of the test runner

    /// main entry point
    /// usage eg.: speuappdev_heatMapEngine_ns.drawHeatMap( [{area:49}...], {left:20...});
    this.drawHeatMap = function(heatMapData) {
        var lo = this.Layout();
        tiles = lo.layoutTiles(heatMapData );
        var rr = this.Renderer({left:20,top:20,width:500,height:500});
        rr.renderLayout(tiles);
    };

    this.Layout = function Layout() {
        var publicState = {};
        publicState.testableState = {};

        var calcTotalArea = function(heatMapData, numTiles) {
            if ( typeof numTiles !== 'number' ) {
                 numTiles = heatMapData.length;
            }
            var tot = 0;
                for ( var ii = 0; ii < numTiles; ii++ ) {
                tot += heatMapData[ii].area;
            }
            return tot;
        };

        var sc = SquarenessCalculator();
        /// adds tiles to the committedTiles array from heatMap data
        /// @param nuMTiles - the number of tiles to add
        /// @param remainingRect provides coordinates for the tiles
        /// @param side - the side along which the tiles are aligned.
        /// @param length - the length of the side along which the tiles are alinged.
        /// #param committedTiles - array into which tiles are added.
        var commitTiles = publicState.testableState.commitTiles = function (heatMapData
          , numTiles, remainingRect, side, length, committedTiles)
        {
            var totalArea = calcTotalArea(heatMapData, numTiles);
            var curTop = remainingRect.get_top();
            var curLeft = remainingRect.get_left();
            for (var ii = 0; ii < numTiles; ii++)
            {
                var width;
                var height;
                if (side === 'width')
                {
                    width = length * (heatMapData[ii].area / totalArea );
                    height = heatMapData[ii].area / width;
                }
                else
                {
                    height = length * (heatMapData[ii].area / totalArea );
                    width = heatMapData[ii].area / height;
                }
                var tile = Tile(curLeft, curTop, width, height);
                committedTiles.push(tile);
                if (side === 'width')
                {
                    curLeft += tile.get_width();
                }
                else
                {
                    curTop += tile.get_height();
                }
            }
            return totalArea;
        };
        /// @return an array of tiles for displaying
        publicState.layoutTiles = function layoutTiles(heatMapData ) {
            assert( typeof heatMapData === 'object' && typeof heatMapData.push === 'function'
              , "Layout.layouTiles: heatMapData must be an array of areas");
            var committedTiles = [];
            var area = calcTotalArea(heatMapData );
            var sideLength = Math.sqrt(area);
            var rect = Rectangle(0,0, sideLength, sideLength );

            layoutRectangle(heatMapData, rect, committedTiles );
            return committedTiles;
        };
        var layoutRectangle = publicState.testableState.layoutRectangle
          = function layoutRectangle(heatMapData, remainingRect, committedTiles ) {
            assert( typeof heatMapData === 'object' && typeof heatMapData.push === 'function'
              , "Layout.layouTiles: heatMapData must be an array of areas");
            assert( typeof committedTiles === 'object' && typeof committedTiles.push === 'function'
              , "Layout.layouTiles: committedTiles must be an array of areas");
            assert( typeof remainingRect === 'object' && typeof remainingRect.get_remainingRect === 'function'
              , "Layout.layouTiles: remainingRect must be a Rectangle");

            var side = remainingRect.shortestSide();
            var length = side === 'width' ? remainingRect.get_width() : remainingRect.get_height();
            var seqAreas = Sequence(heatMapData);
            var numTiles = sc.tileCountForSide(seqAreas, length);
            assert( numTiles <= seqAreas.length()
              , "the squareness calculator has allocated more tiles than are available" );
            var totalArea = commitTiles(heatMapData, numTiles, remainingRect, side, length, committedTiles);
            if ( heatMapData.length > numTiles ) {
                remainingRect = remainingRect.get_remainingRect(side, totalArea );
                layoutRectangle(heatMapData.slice(numTiles), remainingRect, committedTiles );
            }
        };
        return publicState;
    };

    /// tile contains the data to draw an appropriate rectangle on the heatmap
    /// the layout contains columns and each column contains tiles.
    var Tile = this.testableState.Tile = function(left, top, width, height) {
        assert( typeof(left) === 'number'
          && typeof(top) === 'number'
          && typeof(width) === 'number'
          && typeof(height) === 'number'
        );
        var publicState = {};
        publicState.get_left = function() {
            return left;
        };
        publicState.get_top = function() {
            return top;
        };
        publicState.get_width = function() {
            return width;
        };
        publicState.get_height = function() {
            return height;
        };
        publicState.get_area = function() {
            return width * height;
        };
//        /// if you add arbitrary porperties to the tile when you creeate it
//        /// then you can query them with this
//        publicState.get_property = function(propertyName) {
//            return spec[propertyName];
//        };
        return publicState;
    };

    var Rectangle = this.testableState.Rectangle = function(left, top, width, height) {
        assert( width > 0 && height > 0, "cannot handle a rectangle with zero area or an anti-rectangle");
        var publicState = Tile(left, top, width, height);
        var get_area = publicState.get_area;
        var get_left = publicState.get_left;
        var get_top = publicState.get_top;
        var get_width = publicState.get_width;
        var get_height = publicState.get_height;
        /// @param givenSide the side that is already fixed must be 'height' or 'width'
        ///
        publicState.get_remainingRect = function (givenSide, areaToRemove ) {
            var newLeft;
            var newTop;
            var newWidth;
            var newHeight;
            if ( givenSide === 'height') {
                var removedWidth = areaToRemove / get_height();
                newLeft = get_left() + removedWidth;
                newTop = get_top();
                newWidth = get_width() - removedWidth;
                newHeight = get_height();
            }
            else {
                assert( givenSide === 'width', "get_reaminingRect: side must be either 'width' or 'height'");
                var removedHeight = areaToRemove / get_width();
                newLeft = get_left();
                newTop = get_top() + removedHeight;
                newWidth = get_width();
                newHeight = get_height() - removedHeight;
            }
            return Rectangle(newLeft, newTop, newWidth, newHeight );
        };
        publicState.shortestSide = function shortestSide() {
            return width < height ? 'width' : 'height';  
        };
        return publicState;
    };

    var sumAreas = this.testableState.sumAreas = function(areaData) {
        assert( areaData != null );
        assert( typeof areaData === 'object' );
        assert( typeof areaData[0].area === 'number' );
        var totalArea = 0;
        for ( var ii = 0; ii < areaData.length; ii++ ) {
            totalArea += areaData[ii].area;
        }
        return totalArea;
    };
    /// utility class that converts an array to a lisp style sequence with head and tail etc.
    var Sequence = this.testableState.Sequence = function Sequence(arr, pos)
    {
        if ( typeof pos === 'undefined') {
            pos = 0;
        }
        var publicState = {};
        var head = publicState.head = function() {
            if ( arr.length > pos ) {
                return arr[pos];
            }
            else
            {
                return null;
            }
        };
        var isEmpty = publicState.isEmpty = function () {
            return head() === null;
        };

        var tail = publicState.tail = function () {
            assert( pos  < arr.length, "Squence.tail: attempt to tail an  empty sequence");
            return Sequence( arr, pos + 1);
        };
        var length = publicState.length = function length() {
            return arr.length - pos;
        }
        return publicState;
    };

    var SquarenessCalculator = this.testableState.SquarenessCalculator = function() {
        var publicState = {};
        publicState.testableState = {};

        /// for a set of areas plus an optional extra area this calculates
        /// the aggregate squareness factor given that all areas have to have one side whose
        /// totals add up to length
        var squareness = publicState.testableState.squareness = function (areas, extraArea, length) {
            assert( typeof areas !== 'undefined' && areas !== null
              , "SquarenessCalculator.squareness: requires an non-empty array of areas" );
            assert( typeof areas.push !== 'undefined'
               , "SquarenessCalculator.squareness: requires an array of area");
            assert( extraArea !== 'undefined' && extraArea != null
              , "SquarenessCalculator.squareness: requires an extra area of type {area:nn}.  It can can have an area of 0" );
            assert( typeof length === 'number', "SquarenessCalculator.squareness: requires a length");
            var totalArea = function() {
                var tot = 0;
                for ( var ii = 0; ii < areas.length; ii++ ) {
                    tot += areas[ii].area;
                }
                return tot;
            }() + extraArea.area;
            var sq = 0;
            for ( var ii = 0; ii < areas.length; ii++ ) {
                var height = length * areas[ii].area / totalArea;
                var width = areas[ii].area / height;
                sq += (Math.max( height / width, width / height ));
            }
            if ( extraArea.area > 0 ) {
                height = length * extraArea.area / totalArea;
                width = extraArea.area / height;
                sq += (Math.max( height / width, width / height ));
            }
            return sq;
        };
        /// returns the optimal number of sequential tiles in the sequence
        /// which provide the squarest tiles for the given length
        /// @param seqAreas sequence of type {area:nnn}
        /// @param length - this is typically either the width or the height of
        ///   a retangle along which the tiles are placed.
        var squarestTileCount = publicState.squarestTileCount
          = function(seqAreas, length) {
            assert( !seqAreas.isEmpty()
              , "squarenesCalculateor.squarestTileCount: cannot handle an empty sequence of tiles");
            assert( typeof seqAreas.head().area === 'number'
              , "the tile sequence must contain raw objects containing an area property");
            var doSquarestTileCount = function doSquarestTileCount(seqAreasPart, length, areasPart) {
                if (seqAreasPart.isEmpty())  {
                    return areasPart.length;
                }
                if ( squareness(areasPart, seqAreas.head(), length) <= squareness(areasPart, {area:0}, length)) {
                    areasPart.push(seqAreasPart.head());
                    return doSquarestTileCount(seqAreasPart.tail(), length, areasPart );
                }
                else
                {
                    return areasPart.length;
                }
            };
            var areas = [seqAreas.head()];
            var areaCount = doSquarestTileCount( seqAreas.tail(), length, areas );
            return areaCount;
        };
        publicState.tileCountForSide = function tileCountForSide(seqAreas, length ) {
            return squarestTileCount(seqAreas, length );
        };
        return publicState;
    };
    /// this has sole charge of rendering the constructed layout to the browser page
    this.Renderer = function(canvasRect) {
        assert( typeof(canvasRect.left) === 'number' && typeof(canvasRect.top) === 'number'
          && typeof(canvasRect.width) === 'number' && typeof(canvasRect.height) === 'number'
          ,"canvasRect passed to renderer constructor must contain left, top, width, height");
        assert( canvasRect.width > 0 && canvasRect.height > 0 );

        var publicState = {};

        var renderTile = function(tile, left, top, width, height ) {
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
        var calcTotalArea = function(tiles) {
            var tot = 0;
            for ( var ii = 0; ii < tiles.length; ii++ ) {
                tot += tiles[ii].get_area();
            }
            return tot;
        };
        publicState.renderLayout = function( tiles, reverse ) {
            try {

                if ( typeof(reverse) === 'undefined' ) {
                    reverse = false;
                }
                var totalArea = calcTotalArea(tiles);
                var xScale = canvasRect.width / Math.sqrt(totalArea);
                var yScale = canvasRect.height / Math.sqrt(totalArea);
                for ( var ii = 0; ii < tiles.length; ii++ ) {
                    renderTile(tiles[ii]
                      , canvasRect.left + tiles[ii].get_left() * xScale
                      , canvasRect.top + tiles[ii].get_top() * yScale
                      , tiles[ii].get_width() * xScale
                      , tiles[ii].get_height() * yScale
                      );
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