$(document).ready(function () {
    getModel();
    getProducer();
    getRAM();
    getMemory();
    getOS();
    $("select").change(getData);
    $("#reset").click(resetFilters);
    getData();
});

function resetFilters() {
    $("select").val("null").trigger("change");
}

function getOS() {
    $.get("database/getOS.php", function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            populateSelect(obj, "#os");
        }
    });
}

function getMemory() {
    $.get("database/getInternal.php", function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            populateSelect(obj, "#internal_memory");
        }
    });
}

function getRAM() {
    $.get("database/getRAM.php", function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            populateSelect(obj, "#RAM_memory");
        }
    });
}

function getModel() {
    $.get("database/getModels.php", function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            populateSelect(obj, "#model");
        }
    });
}

function getProducer() {
    $.get("database/getProducers.php", function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            populateSelect(obj, "#producer");
        }
    });
}

function populateSelect(obj, selectId) {
    const list = $(selectId);
    list.append($("<option></option>").val("null"));
    for(let i = 0; i < obj.length; i ++){
        list.append(
            $("<option></option>")
                .val(obj[i])
                .text(obj[i])
        );
    }
}

function getData() {
    let url = "getData.php?";
    url += "producer=" + $("#producer").val();
    url += "&model=" + $("#model").val();
    url += "&ram=" + $("#RAM_memory").val();
    url += "&memory=" + $("#internal_memory").val();
    url += "&os=" + $("#os").val();
    $.get(url, function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            drawTable(obj);
        }
    });
}

function drawTable(obj) {
    console.log(obj);

    const table = $("#result");
    table.empty().append(
        $("<tr></tr>")
            .append($("<th></th>").text("Id"))
            .append($("<th></th>").text("Producer"))
            .append($("<th></th>").text("Model"))
            .append($("<th></th>").text("RAM"))
            .append($("<th></th>").text("Memory"))
            .append($("<th></th>").text("OS"))
    );
    obj.forEach(x => {
        table.append(
            $("<tr></tr>")
                .append($("<td></td>").text(x["id"]))
                .append($("<td></td>").text(x["producer"]))
                .append($("<td></td>").text(x["model"]))
                .append($("<td></td>").text(x["ram"]))
                .append($("<td></td>").text(x["memory"]))
                .append($("<td></td>").text(x["os"]))
        )
    });
}

