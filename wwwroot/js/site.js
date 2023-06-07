// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function copyToClipboard(element) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
}

function showHide(element1, element2) {
    const myElement2 = document.getElementById(element2);
    myElement2.style.display = "none";
    const myElement1 = document.getElementById(element1);
    myElement1.style.display = "block";
}

var loadingTime;

function loaderFn() {
    var loadingTime = setTimeout(showPage, 2000);
}

function showPage() {
        document.getElementById("loader").style.display = "none";
    
}

