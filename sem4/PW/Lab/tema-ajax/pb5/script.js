$(document).ready(function () {
    $("#root").on("click", expand);
});

let clickedSpan = null;

function expand() {
    $("#fileContent").empty();
    clickedSpan = $(this);
    let path = constructPath(clickedSpan);
    console.log(path);
    requestContent(path);
}

function constructPath(span) {
    let path = [span.text()];
    while(true) {
        if(span.attr("id") === "root")
            break;
        let parent = span.parent().parent().siblings("span");
        path.unshift(parent.text());
        if(parent.attr("id") === "root")
            break;
        span = parent;
    }

    path = path.reduce((prev, curr) => prev + "/" + curr, ".");
    return path;
}

function requestContent(path) {
    $.get("getContent.php?file=" + path, function(data, status){
        if(status === "success"){
            let obj =  JSON.parse(data);
            displayStuff(obj);
        }
    });
}

function displayStuff(obj) {
    console.log(obj);
    if(obj["type"] === "directory") {
        clickedSpan.off("click", expand);
        clickedSpan.on("click", collapse);
        const parent = clickedSpan.parent();
        const ul = $("<ul></ul>");
        const files = obj["content"];
        for(let i = 0; i < files.length; i++){
            ul.append(
                $("<li></li>").append(
                    $("<span></span>").text(files[i]).on("click", expand)
                )
            );
        }
        parent.append(ul);
    }
    else if(obj["type"] === "file"){
        $("#fileContent").text(obj["content"]);
    }
    else {
        alert(obj["content"]);
    }
}

function collapse() {
    $("#fileContent").empty();
    clickedSpan = $(this);
    clickedSpan.parent().empty().append(clickedSpan);
    clickedSpan.off("click", collapse);
    clickedSpan.on("click", expand);
}