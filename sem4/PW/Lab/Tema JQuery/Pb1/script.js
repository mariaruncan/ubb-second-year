$(document).ready(function() {
    $("#leftList").dblclick(function() {
        const x = $("#leftList option:selected");
        $("#rightList").append(x);
        $("#leftList").remove(x);
    });

    $("#rightList").dblclick(function() {
        const x = $("#rightList option:selected");
        $("#leftList").append(x);
        $("#rightList").remove(x);
    });
});