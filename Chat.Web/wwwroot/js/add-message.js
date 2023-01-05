var errorMessage = "Произошла ошибка при выполнении запроса";

function OnAddSuccess(data) {
    console.log(data);
    if (data.status === "Success") {
        window.location.reload(false);
    }
    else if (data.status === "Failure") {
        alert(data.ErrorMessage);
    } else {
        alert(errorMessage);
    }
}

function OnAddFailure() {
    alert(errorMessage);
}