let isChanged = false;
let lastId;

$(document).ready(function () {
    getIds();
    $("#submitButton").click(saveData);
    $("input").change(function () {
        isChanged = true;
    });
});

function populateSelect(ids) {
    const list = $("#listClients");
    lastId = ids[0];
    for(let i = 0; i < ids.length; i ++){
        list.append(
            $("<option></option>")
                .val(ids[i])
                .attr("name", "clientId")
                .text(ids[i])
        );
    }
    getClientData();
    list.on("change", getClientData);
}

function getIds(){
    // const request = new XMLHttpRequest();
    // request.onreadystatechange = function () {
    //     if(request.readyState === 4 && request.status === 200) {
    //         const obj = JSON.parse(request.responseText);
    //         console.log(obj);
    //         populateSelect(obj);
    //     }
    // };
    // request.open("GET", "getClientIds.php", true);
    // request.send('');

    $.get("getClientIds.php", function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            console.log(obj);
            populateSelect(obj);
        }
    });
}

function getClientData() {
    if(isChanged) {
        const res = confirm("Do you want to save before changing client data?");
        if(res)
            saveData();
        isChanged = false;
    }

    // const request = new XMLHttpRequest();
    // request.onreadystatechange = function () {
    //     if(request.readyState === 4 && request.status === 200) {
    //         const obj = JSON.parse(request.responseText);
    //         populateForm(obj);
    //     }
    // };
    // request.open("GET", "getClientData.php?clientId=" + $("#listClients").val(), true);
    // request.send('');

    $.get("getClientData.php?clientId=" + $("#listClients").val(), function(data, status){
        if(status === "success"){
            const obj = JSON.parse(data);
            populateForm(obj);
        }
    });
}

function populateForm(obj) {
    $("#firstname").val(obj["firstname"]);
    $("#lastname").val(obj["lastname"]);
    $("#phoneNumber").val(obj["phoneNumber"]);
    $("#email").val(obj["email"]);

    lastId = $("#listClients").val();
}

function saveData() {
    let url = "saveClientData.php?";
    url += "firstname=" + $("#firstname").val();
    url += "&lastname=" + $("#lastname").val();
    url += "&phoneNumber=" + $("#phoneNumber").val();
    url += "&email=" + $("#email").val();
    url += "&clientId=" + lastId;

    // const request = new XMLHttpRequest();
    // request.onreadystatechange = function () {
    //     if(request.readyState === 4 && request.status === 200) {
    //         if(request.responseText !== "ok")
    //             alert("Can not save!");l
    //     }
    // };
    // request.open("GET", url, true);
    // request.send('');

    $.get(url, function(data, status){
        if(status === "success"){
            if(data !== "ok")
                alert("Can not save!");l
        }
    });
}
