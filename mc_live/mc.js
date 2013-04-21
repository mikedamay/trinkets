/**
 * Created with IntelliJ IDEA.
 * User: Mike May
 * Date: 31/03/13
 * Time: 10:05
 * To change this template use File | Settings | File Templates.
 */

var populated_phases = [
    {phase_name: "validate", default_plugin : {
       group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "intialize", default_plugin : {
       group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "generate-sources", default_plugin : {
       group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "process-sources", default_plugin : {
       group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "generate-resources", default_plugin : {
       group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "process-resources", default_plugin : {
       group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-resources-plugin"
        , default_goal: "resources"
        , prefix: "resources"
        , default_exeuction_id : "default-resources"}}
    ,{phase_name: "compile", default_plugin : {
       group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-compiler-plugin"
        , default_goal: "compile"
        , prefix: "compile"
        , default_exeuction_id : "default-compile"}}
    ,{phase_name: "process-classes", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "generate-test-sources", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "process-test-sources", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "generate-test-resources", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "process-test-resources", default_plugin : {
        group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-resources-plugin"
        , default_goal: "testResources"
        , prefix: "resources"
        , default_exeuction_id : "default-testResources"}}
    ,{phase_name: "test-compile", default_plugin : {
        group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-compiler-plugin"
        , default_goal: "testCompile"
        , prefix: "compile"
        , default_exeuction_id : "default-testCompile"}}
    ,{phase_name: "process-test-classes", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "test", default_plugin : {
        group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-surefire-plugin"
        , default_goal: "test"
        , prefix: "surefire"
        , default_exeuction_id : "default-test"}}
    ,{phase_name: "prepare-package", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "package", default_plugin : {
        group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-jar-plugin"
        , default_goal: "jar"
        , prefix: "jar"
        , default_exeuction_id : "default-jar"}}
    ,{phase_name: "pre-integration-test", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "integration-test", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "pot-integration-test", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "verify", default_plugin : {
        group_id: ""
        , artifact_id: ""
        , default_goal: ""
        , prefix: ""
        , default_exeuction_id : ""}}
    ,{phase_name: "install", default_plugin : {
        group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-install-plugin"
        , default_goal: "install"
        , prefix: "install"
        , default_exeuction_id : "default-install"}}
    ,{phase_name: "deploy", default_plugin : {
        group_id: "org.apache.maven.plugins"
        , artifact_id: "maven-deploy-plugin"
        , default_goal: "deploy"
        , prefix: "deploy"
        , default_exeuction_id : "default-deploy"}}
];

$(function ()
{
    doStartup2();
});

function doStartup()
{
    $('#mainButton').click(doStuff);
    alert('hello');
}

function doStuff()
{
    var v = $('<div>stuff using a variable</div>')
    v.insertAfter('#mike');
}

function doStartup2()
{
    var MPG_NUM_COLS = 3;
    var MPG_NUM_ROWS = 8;
    var PHASE_BOX_MARGIN = 5;
    var PHASE_BOX_CURVE = 10;

    document.createSvg = function (tagName)
    {
        var svgNS = "http://www.w3.org/2000/svg";
        return this.createElementNS(svgNS, tagName);
    };

    var grid = function (left, top, numberPerHSide, numberPerVSide, boxDims, pixelsAcross, pixelsDown)
    {

        function makePhaseText(boxSize, number)
        {
            var text = document.createSvg("text");
            var node = document.createTextNode('');
//            var node = document.createTextNode('t' + "_" + number);
            text.appendChild(node);
            text.setAttribute("fill-opacity", 0.5)
//            node.setAttribute("style", "fill:blue;")
            text.setAttribute("font-size", 16    );
            text.setAttribute("style", "text-anchor: middle;");
            text.setAttribute("x", boxSize.width / 2);
            text.setAttribute("y", 5 + boxSize.height / 8);
            text.setAttribute("fill", "black");
            text.setAttribute("id", "t" + number);
            return text;
        }

        function makePhaseBox(boxSize, number)
        {
            var box = document.createSvg("rect");
            box.setAttribute("width", boxSize.width - PHASE_BOX_MARGIN * 2);
            box.setAttribute("height", boxSize.height - PHASE_BOX_MARGIN * 2);
            box.setAttribute("x", PHASE_BOX_MARGIN );
            box.setAttribute("y", PHASE_BOX_MARGIN );
            box.setAttribute("rx", PHASE_BOX_CURVE );
            box.setAttribute("ry", PHASE_BOX_CURVE );
            box.setAttribute("stroke", "#cccccc");
            box.setAttribute("stroke-width", "1");
            box.setAttribute("fill", "white");
            box.setAttribute("fill-opacity", 0)
            box.setAttribute("id", "b" + number);
            return box;
        }

        var svg = document.createSvg("svg");
        svg.setAttribute("x", 50);
        svg.setAttribute("y", 50);
        svg.setAttribute("width", pixelsAcross);
        svg.setAttribute("height", pixelsDown);
        svg.setAttribute("fill", "green");
        svg.setAttribute("fill-opacity", 1);
//        svg.setAttribute("viewBox", [0, 0, numberPerHSide * dims.width, numberPerVSide * dims.height].join(" "));
//        svg.setAttribute("viewBox", [0,0,pixelsAcross, pixelsDown].join(" "));


        for (var i = 0; i < numberPerVSide; i++)
        {
            for (var j = 0; j < numberPerHSide; j++)
            {
                var g = document.createSvg("g");
                g.setAttribute("transform", ["translate(", j * boxDims.width, ",", i * boxDims.height, ")"].join(""));
                var number = makeBoxNumber(numberPerHSide, i, j);
                g.setAttribute("id", "g" + number);
                var box = makePhaseBox(boxDims, number);
                g.appendChild(box);
                var text = makePhaseText(boxDims, number);
                g.appendChild(text);
                svg.appendChild(g);
            }
        }
        var oneTooMany  = svg.getElementById("g" + 23 );
        svg.removeChild(oneTooMany);

        svg.addEventListener(
                "click",
                function (e)
                {
                    var id = e.target.id;
                    if (id)
                    {
                        alert(id);
                    }
                },
                false);
        var svgParent = document.createSvg("svg");
        svgParent.setAttribute("x", 0);
        svgParent.setAttribute("y", 0);
        svgParent.setAttribute("width", pixelsAcross + 100);
        svgParent.setAttribute("height", pixelsDown + 100);
//        svgParent.setAttribute("viewBox", [0, 0, pixelsAcross + 100, pixelsDown + 100].join(" "));
        svgParent.setAttribute("fill", "red");
        svgParent.setAttribute("fill-opacity", 0.5);
        var parentRect = document.createSvg("rect");
        parentRect.setAttribute("width", svgParent.getAttribute("width"));
        parentRect.setAttribute("height", svgParent.getAttribute("height"));
        parentRect.setAttribute("fill", "#aabbcc");
        svgParent.appendChild(parentRect);
        svgParent.appendChild(svg);
        return svgParent;
    };
    makeBoxNumber: function makeBoxNumber(boxesPerRow, rowNum, colNum ) {
        if ( rowNum % 2 === 0) {
            return boxesPerRow * rowNum + colNum;
        }
        else {
            return boxesPerRow * (rowNum + 1 ) - (colNum + 1);
        }
    }
    function populateGridWithPhaseNames(grid, phases)
    {
        for ( var ii = 0; ii < phases.length; ii++ ) {
            document.getElementById("t"+ii).textContent= '' + (ii + 1) + ' - ' + phases[ii].phase_name;
        }
    };

    var container = document.getElementById("container");
    container.appendChild(grid(200, 200, MPG_NUM_COLS, MPG_NUM_ROWS, {width:200, height:100}, 600, 800));
    populateGridWithPhaseNames(grid, populated_phases);

}
