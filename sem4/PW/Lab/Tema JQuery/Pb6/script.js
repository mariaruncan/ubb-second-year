let matrix = [];
let solution = [];

let free = {i: 0, j: 0};
let n;

$(document).ready(function() {
    $("#table").empty().hide();
    $("#msg").empty().hide();
    $("#button").click(initGame);
});

function initGame(){
    n = parseInt($("#nr").val());
    if(n < 2){
        alert("Nr of rows/cols must be greater than 2!");
        return;
    }

    const table = $("#table");
    table.show();
    $("#msg").show();
    $("div").hide();

    // generare nr de la 1 la n^2
    const nums = [];
    for(let i = 1; i < n * n; i++)
        nums.push(i);
    nums.push(-1);

    // construirea solutiei
    solution = [];
    for(let i = 0; i < n; i++){
        const line = [];
        for(let j = 0; j < n; j++)
            line.push(nums[i * n + j]);
        solution.push(line);
    }
    console.log("Solution: ", solution);

    // construire joc random
    nums.sort(() => Math.random() - 0.5);
    console.log("Nums after shuffle: ", nums);
    matrix = [];
    for (let i = 0; i < n; i++) {
        const line = [];
        for (let j = 0; j < n; j++)
            line.push(nums.pop());
        matrix.push(line);
    }

    if(checkIfSolution()){
        const temp = matrix[0];
        matrix[0] = matrix[n - 1];
        matrix[n - 1] = temp;
    }
    console.log("Matrix: ", matrix);

    // creare tabel cu nr
    for(let i = 0; i < n; i++){
        const tr = $("<tr></tr>");
        for(let j = 0; j < n; j++){
            if(matrix[i][j] !== -1)
                tr.append(
                    $("<td></td>").text(matrix[i][j]).attr("id", "td" + i + j)
                );
            else {
                free.i = i;
                free.j = j;
                tr.append(
                    $("<td></td>").attr("id", "td" + i + j)
                );
            }
        }
        table.append(tr);
    }

    $(document).on("keydown", function pressed (event) {
        const key = event.key;
        switch(key){
            case "ArrowLeft":
                if(free.j < n - 1){
                    const temp = {i: free.i, j: free.j + 1};
                    switchCells(temp);
                }
                break;
            case "ArrowRight":
                if(free.j > 0){
                    const temp = {i: free.i, j: free.j - 1};
                    switchCells(temp);
                }
                break;
            case "ArrowUp":
                if(free.i < n - 1){
                    const temp = {i: free.i + 1, j: free.j};
                    switchCells(temp);
                }
                break;
            case "ArrowDown":
                if(free.i > 0){
                    const temp = {i: free.i - 1, j: free.j};
                    switchCells(temp);
                }
                break;
        }

        if(checkIfSolution()){
            $("#msg").text("You won!");
            $("body").off("keypress");
            $("#nr").val("").attr("placeholder", "no of rows/cols");
            $(document).off("keydown");
            setTimeout(() => {
                $("div").show();
                $("#table").empty().hide();
                $("#msg").text("").hide();
            }, 2000);
        }
    });
}

// interschimba celula libera cu alta
function switchCells(temp){
    const tdFree = $("#td" + free.i + free.j);
    const tdTemp = $("#td" + temp.i + temp.j);
    tdFree.text(tdTemp.text());
    tdTemp.text("");
    matrix[free.i][free.j] = matrix[temp.i][temp.j];
    matrix[temp.i][temp.j] = -1;
    free.i = temp.i;
    free.j = temp.j;
}

function checkIfSolution(){
    for(let i = 0; i < n; i++)
        for(let j = 0; j < n; j++)
            if(matrix[i][j] !== solution[i][j])
                return false;
    return true;
}
