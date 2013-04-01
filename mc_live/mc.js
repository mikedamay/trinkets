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

    document.createSvg = function (tagName)
    {
        var svgNS = "http://www.w3.org/2000/svg";
        return this.createElementNS(svgNS, tagName);
    };

    var grid = function (numberPerHSide, numberPerVSide, dims, pixelsAcross, pixelsDown)
    {

        function makeText(size, number)
        {
            var text = document.createSvg("text");
            text.appendChild(document.createTextNode('t' + "_" + number));
            text.setAttribute("fill", "#aaaaaa");
            text.setAttribute("font-size", 15    );
            text.setAttribute("style", "text-anchor: middle;");
            text.setAttribute("x", size.width / 2);
            text.setAttribute("y", size.height / 8);
            text.setAttribute("id", "t" + number);
            return text;
        }

        function makeBox(size, number)
        {
            var box = document.createSvg("rect");
            box.setAttribute("width", size.width);
            box.setAttribute("height", size.height);
            box.setAttribute("stroke", "#cccccc");
            box.setAttribute("stroke-width", "2");
            box.setAttribute("fill", "white");
            box.setAttribute("id", "b" + number);
            return box;
        }

        var svg = document.createSvg("svg");
        svg.setAttribute("width", pixelsAcross);
        svg.setAttribute("height", pixelsDown);
        svg.setAttribute("viewBox", [0, 0, numberPerHSide * dims.width, numberPerVSide * dims.height].join(" "));


        for (var i = 0; i < numberPerVSide; i++)
        {
            for (var j = 0; j < numberPerHSide; j++)
            {
                var g = document.createSvg("g");
                g.setAttribute("transform", ["translate(", j * dims.width, ",", i * dims.height, ")"].join(""));
                var number = numberPerHSide * i + j;
                var box = makeBox(dims, number);
                g.appendChild(box);
                var text = makeText(dims, number);
                g.appendChild(text);
                svg.appendChild(g);
            }
        }
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
        return svg;
    };

    function populateGridWithPhases(grid, phases)
    {
        for ( var ii = 0; ii < phases.length; ii++ ) {
            document.getElementById("t"+ii).textContent=phases[ii].phase_name;
        }
    };

    var container = document.getElementById("container");
    container.appendChild(grid(MPG_NUM_COLS, MPG_NUM_ROWS, {width:300, height:100}, 900, 800));
    populateGridWithPhases(grid, populated_phases);

}
