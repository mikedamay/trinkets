describe("Tile", function() {
  var Tile = speuappdev_heatMapEngine_ns.testableState.Tile;

  it("should be able to return its area", function() {
    var myTile = Tile({area:55});
    expect(myTile.get_area()).toEqual(55);

  });

  it("should be able to return an arbitrary property"), function() {
    var myTile = Tile({area:55,arbitraryProp:"something"});
    expect(myTile.get_property("arbitraryProp")).toEqual("something");
  }

});