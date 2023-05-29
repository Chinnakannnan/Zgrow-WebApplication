 
function refreshcaptcha() {
    $.ajax({
        type: 'POST',
        url: '/Login/CaptchaRefresh',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8', // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
        success: function (result) {           
            var newCaptchaImage = result.captchaImage;
            $('#captchadefault').attr('src', 'data:image/png;base64,' + newCaptchaImage);           
            }
    })
}
 

function LoadLogout() {
    alert("logout")
    $.ajax(
        {
            type: 'POST',
            dataType: 'JSON',
            url: "/Login/Signout",
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            success: function () {
                alert("logout2")
               // localStorage.setItem("app-logout", 'logout' + Math.random());
                window.location.href = "/Login/Login";
                window.location.reload();
            }
        });
}

 
function submitLogin() {

    var data = $("#loginForm").serialize();  
    alert("hai")
    $.ajax({
        type: 'POST',
        url: '/Login/LoginVaildate',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8', // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
        data: data,
        success: function (result) {
            alert("hai2")
            alert(result)
            if (result == 'admin') {
                window.location.href = "/Admin/Index";
            } else if (result == 'user') {
                alert("user2")
                window.location.href = "/Home/Index";
            }           
            else if (result == '5') {
                $('#displaydangermessage').text("Account has block temporarily.");
                $('#alertPopup').modal('show');
            } 
            else {
                $('#displaydangermessage').text("Technical Error");
                $('#alertPopup').modal('show');
            }
        },
        error: function () {
            //alert('Failed to receive the Data');
            // console.log('Failed ');
        }
    })
}
 