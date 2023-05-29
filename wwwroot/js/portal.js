(function ($) {
    "use strict"; // Start of use strict

    // Toggle the side navigation
    $("#sidebarToggle, #sidebarToggleTop").on('click', function (e) {
        $("body").toggleClass("sidebar-toggled");
        $(".sidebar").toggleClass("toggled");
        if ($(".sidebar").hasClass("toggled")) {
            $('.sidebar .collapse').collapse('hide');
        };
    });

    // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
    $('body.fixed-nav .sidebar').on('mousewheel DOMMouseScroll wheel', function (e) {
        if ($(window).width() > 768) {
            var e0 = e.originalEvent,
                delta = e0.wheelDelta || -e0.detail;
            this.scrollTop += (delta < 0 ? 1 : -1) * 30;
            e.preventDefault();
        }
    });

    // Scroll to top button appear
    $(document).on('scroll', function () {
        var scrollDistance = $(this).scrollTop();
        if (scrollDistance > 100) {
            $('.scroll-to-top').fadeIn();
        } else {
            $('.scroll-to-top').fadeOut();
        }
    });

    // Smooth scrolling using jQuery easing
    $(document).on('click', 'a.scroll-to-top', function (e) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: ($($anchor.attr('href')).offset().top)
        }, 1000, 'easeInOutExpo');
        e.preventDefault();
    });
    //show_hide_password
    $(".show_hide_password a").on('click', function (event) {
        event.preventDefault();
        if ($('.show_hide_password input').attr("type") == "text") {
            $('.show_hide_password input').attr('type', 'password');
            $('.show_hide_password i').addClass("fa-eye-slash");
            $('.show_hide_password i').removeClass("fa-eye");
        } else if ($('.show_hide_password input').attr("type") == "password") {
            $('.show_hide_password input').attr('type', 'text');
            $('.show_hide_password i').removeClass("fa-eye-slash");
            $('.show_hide_password i').addClass("fa-eye");
        }
    });
    // file upload 
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        var leftRightStrings = fileName.split('.');
        var fName = leftRightStrings[0];
        var fExtention = leftRightStrings[1];
        var lengthFname = fName.length;
        if (lengthFname > 15) {
            fileName = fName.substr(0, 8) + "..." + fName.substr(-5) + "." + fExtention;
        }
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);

        var filesCount = $(this)[0].files.length;

        var textbox = $(this).prev();

        if (filesCount === 1) {
            var fileName = $(this).val().split('\\').pop();
            textbox.text(fileName);
        } else {
            textbox.text(filesCount + ' files selected');
        }

        if (typeof (FileReader) != "undefined") {
            var dvPreview = $("#profilePreview");
            dvPreview.html("");
            $($(this)[0].files).each(function () {
                var file = $(this);
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $("<img />");
                    img.attr("src", e.target.result);
                    dvPreview.append(img);
                }
                reader.readAsDataURL(file[0]);
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }


    });

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

        var href = $(e.target).attr('href');
        var $curr = $(".stepper-tab  a[href='" + href + "']").parent();

        $('.stepper-tab li').removeClass();

        $curr.addClass("active");
        $curr.prevAll().addClass("visited");
    });
})(jQuery); // End of use strict

$(document).ready(function () {
    $('#userDetails').DataTable();
    $('#cardApproval').DataTable();
    $('#fundApproval').DataTable();
    $('#fundApproval1').DataTable();



    $('#complaintsubmit').click(function () {
     
        var subject = $("#complaintsubject").val();
        var Comment = $("#complaintcommand").val();

        if (Comment == '') {
            DispalyError("Please Something write in Command section");
            return;
        }

        $.ajax({
            type: 'POST',
            url: '/ProfilePage/RaiseComplaint?Subject=' + subject + "&Comment=" + Comment,
            
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (result) {
                if (result.result = 'true') {
                    DispalySuccess("Complaint Raised Successfully. Your problem will solve sortly");
                }
                else {
                    DispalyError("Complaint Raising failed");
                }
            }
        });;
    });
    
});
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

function showHideFlex(element1, element2) {
    const myElement2 = document.getElementById(element2);
    myElement2.style.display = "none";
    const myElement1 = document.getElementById(element1);
    myElement1.style.display = "flex";
}


var otp_inputs = document.querySelectorAll(".otp__digit")
var mykey = "0123456789".split("")
otp_inputs.forEach((_) => {
    _.addEventListener("keyup", handle_next_input)
})

function handle_next_input(event) {
    let current = event.target
    let index = parseInt(current.classList[1].split("__")[2])
    current.value = event.key

    if (event.keyCode == 8 && index > 1) {
        current.previousElementSibling.focus()
    }
    if (index < 6 && mykey.indexOf("" + event.key + "") != -1) {
        var next = current.nextElementSibling;
        next.focus()
    }
    var _finalKey = ""
    for (let { value } of otp_inputs) {
        _finalKey += value
    }
}
/*Numeric Validation*/
function AllowAlphabet(evt) { return filterInputCommonForAllValidation(0, evt, ''); }

function AllowNumericAndAlphabet(evt) { return filterInputCommonForAllValidation(2, evt, ''); }

function AllowNumeric(evt) { return filterInputCommonForAllValidation(1, evt, ''); }

function AllowNumericWithoutDecimal(evt) { return filterInputCommonForAllValidation(4, evt, ''); }

function AllowNumericWithDecimal(evt) { return filterInputCommonForAllValidation(3, evt, ''); }

function AllowCustomFormat(evt, custom) { return filterInputCommonForAllValidation(6, evt, custom); }

/*Common For All*/
function filterInputCommonForAllValidation(filterType, evt, customfrm) {
    var keyCode, Char, inputField, filter = '';
    var alpha = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ. ';

    var num = '0123456789.';
    var number = '0123456789';
    var splchr = '/';
    var splchr = '@_~$&*#/-';
    if (window.event) {
        keyCode = window.event.keyCode;
        evt = window.event;
    }
    else if (evt) keyCode = evt.which;
    else return true;

    if (filterType == 0) filter = alpha;
    else if (filterType == 1) filter = num;
    else if (filterType == 2) filter = alpha + num;
    else if (filterType == 3) filter = num;
    else if (filterType == 4) filter = number;
    else if (filterType == 5) filter = alpha + num + splchr;
    else if (filterType == 6) filter = customfrm;

    if (filter == '') return true;

    inputField = evt.srcElement ? evt.srcElement : evt.target || evt.currentTarget;
    if ((keyCode == null) || (keyCode == 0) || (keyCode == 8) || (keyCode == 9) || (keyCode == 13) || (keyCode == 27)) return true;
    Char = String.fromCharCode(keyCode);
    if ((filter.indexOf(Char) > -1)) return true;
    else return false;
}



 
