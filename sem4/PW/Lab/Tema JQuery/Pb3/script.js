let matrix = [];
let remainingCells = 0;

function initMatrix(){
    const noRows = $("#noRows").val();
    const noCols = $("#noCols").val();
    if(noRows < 1 || noCols < 1){
        alert("Numbers must be > 0!");
        return;
    }
    if(noRows % 2 === 1 && noCols % 2 === 1){
        alert("At least one number must be even!");
        return;
    }

    const table = $("#table");
    $("div").hide();
    table.show();
    $("#congrats").show();

    let nums = [];
    for(let i = 0; i < (noRows * noCols) / 2; i++){
        nums.push(i + 1);
        nums.push(i + 1);
    }

    matrix = [];
    nums = nums.sort(() => Math.random() - 0.5)
    for(let i = 0; i < noRows; i++){
        const row = [];
        for(let j = 0; j < noCols; j++)
            row.push(nums.pop());
        matrix.push(row);
    }
    console.log(matrix);

    for(let i = 0; i < noRows; i++){
        const tr = $("<tr></tr>")
        for(let j = 0; j < noCols; j++){
            const td = $("<td></td>")
            td.text(matrix[i][j]);
            tr.append(td);
        }
        table.append(tr);
    }
    const tds = $("td");
    tds.addClass("covered");
    tds.click(show);

    remainingCells = noRows * noCols;
}

let firstUncovered = false, secondUncovered = false;

function show(){
    const td = $(this);
    if(td.hasClass("covered")){
        if(!firstUncovered){
            firstUncovered = td;
            td.removeClass("covered").addClass("uncovered");
        }
        else if(!secondUncovered){
            td.removeClass("covered").addClass("uncovered");
            secondUncovered = td;
            if(firstUncovered.text() !== secondUncovered.text()){
                setTimeout(() => {
                    firstUncovered.removeClass("uncovered").addClass("covered");
                    secondUncovered.removeClass("uncovered").addClass("covered");
                    firstUncovered = secondUncovered = false;
                }, 1000);
            }
            else{
                firstUncovered = secondUncovered = false;
                remainingCells -= 2;
            }
        }
    }

    if(remainingCells  === 0){
        $("#congrats").html("You won!");
        setTimeout(() => {
            $("div").show();
            $("#table").empty().hide();
            $("#congrats").hide();
            $("#noRows").val(null);
            $("#noCols").val(null);
            $("#startButton").click(initMatrix);
        }, 2000)
    }
}

$(document).ready(function() {
    $("#table").hide();
    $("#congrats").hide();
    $("#startButton").click(initMatrix);
});