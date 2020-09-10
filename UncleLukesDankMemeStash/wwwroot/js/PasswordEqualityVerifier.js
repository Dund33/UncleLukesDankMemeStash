function checkEq() {
    let password1 = $("#Password").val();
    let password2 = $("#PasswordConf").val();

    if (password1 === password2) {
        $("#submit").attr("disabled", false);
        $("#error").text("");
    }
    else {
        $("#submit").attr("disabled", true);
        $("#error").text("Passwords are not matching");
    }  
}




