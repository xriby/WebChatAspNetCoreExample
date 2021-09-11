var errorMessage = "Произошла ошибка при выполнении запроса";

function OnAddSuccess(data) {
    console.log(data);
    if (data.Status == "Success") {
        window.location.reload(false);
    }
    else if (data.Status == "Failure") {
        alert(data.ErrorMessage);
    } else {
        alert(errorMessage);
    }
}

function OnAddFailure() {
    alert(errorMessage);
}