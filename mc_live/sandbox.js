/**
 * Created with IntelliJ IDEA.
 * User: Mike May
 * Date: 21/04/13
 * Time: 16:31
 * To change this template use File | Settings | File Templates.
 */


var application2 = new function() {
   function abc() {return 7;}
   this.def = function def() {return abc();}
};


$.extend( application2, new function() {
    this.doublex = function doublex(x) { return x*2 + this.def(); }
}

);

alert( "hello Helen " + application2.doublex(5));
