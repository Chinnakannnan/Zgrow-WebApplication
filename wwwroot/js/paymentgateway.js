


$(document).ready(function () {/*
    $('input[type="radio"]').change(function () {
        var selectedOption = $('input[name="option"]:checked').val();
        $('#result').text('Selected option: ' + selectedOption);



    });
*/
});



function initiatePayment() {
    alert("hai")
    var data = $("#PaymentGatewayForm").serialize();
    alert(data)
    
    $.ajax({
        type: 'POST',
        url: '/PaymentGateway/InitiatePayment',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8', // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
        data: data,
        success: function (result) {



 
            }
        },
        error: function () {
            //alert('Failed to receive the Data');
            // console.log('Failed ');
        }
    })
}