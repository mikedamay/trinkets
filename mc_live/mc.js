/**
 * Created with IntelliJ IDEA.
 * User: Mike May
 * Date: 31/03/13
 * Time: 10:05
 * To change this template use File | Settings | File Templates.
 */

$(function() {doStartup();});

function doStartup() {
    $('#mainButton').click(doStuff);
    alert('hello');
}

function doStuff() {
    var v = $('<div>stuff using a variable</div>')
    v.insertAfter('#mike');
}

