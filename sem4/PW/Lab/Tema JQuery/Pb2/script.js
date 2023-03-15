$(document).ready(function (){
    $("#buttonTrimite").click(function() {
        const nume = $("#nume");
        const data = $("#data");
        const varsta = $("#varsta");
        const email = $("#email");

        let error = "";

        if(nume.val().length < 3) {
            error += "Numele trebuie sa fie mai lung de 3 caractere!\n";
            nume.css("border", "2px solid red");
        }
        else
            nume.css("border", "1px solid black");

        if(!data.val().match(/^[0-3]?[0-9]\/[01]?[0-9]\/[0-9]{4}$/) &&  // dd/mm/yyyy
            !data.val().match(/^[0-3]?[0-9]-[01]?[0-9]-[0-9]{4}$/) &&  //  dd-mm-yyyy
            !data.val().match(/^[0-3]?[0-9]\.[01]?[0-9]\.[0-9]{4}$/)){ //  dd.mm.yyyy
            error += "Formatul datei este incorect! (dd/mm/yyyy, dd-mm-yyyy sau dd.mm.yyyy)\n";
            data.css("border", "2px solid red");
        }
        else
            data.css("border", "1px solid black");

        if(isNaN(varsta.val())){
            varsta.css("border", "2px solid red");
            error += "Varsta trebuie sa fie un numar!\n";
        }
        else if(varsta.val() < 1 || varsta.val() > 100){
            varsta.css("border", "2px solid red");
            error += "Varsta trebuie sa fie in intervalul [1, 100]!\n";
        }
        else
            varsta.css("border", "1px solid black");

        if(!email.val().match(/^.{3,}@.{3,}\..{2,}$/)){
            error += "Email incorect! (abc@def.gh)\n";
            email.css("border", "2px solid red");
        }
        else
            email.css("border", "1px solid black");

        const p = $("#err");
        if(error !== ""){
            p.css("color", "red");
            p.text(error);
        }
        else{
            p.css("color", "green");
            p.text("Totul este corect!");
        }
    });
});