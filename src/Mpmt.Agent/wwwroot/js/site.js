
function disableFormFields(containerSelector) {
    if (!containerSelector)
        throw new Error('Invalid containerSelector.');

    $(containerSelector + ' :input').prop('disabled', true);
}

function enableFormFields(containerSelector) {
    if (!containerSelector)
        throw new Error('Invalid containerSelector.');

    $(containerSelector + ' :input').prop('disabled', false);
}

function hideFormSection(selector) {
    if (!selector)
        throw new Error('Invalid selector.');

    disableFormFields(selector);
    $(selector).hide();
}

function showFormSection(selector) {
    if (!selector)
        throw new Error('Invalid selector.');

    enableFormFields(selector);
    $(selector).show();
}


function setLoadWaiting(enable) {
    var $busyEl = $('.ajax-loading-busy');

    if (enable) {
        $busyEl.removeClass('display-none');
    } else {
        $busyEl.addClass('display-none');
    }
}

//$(document).ready(function () {


//AjaxCall = (Url ,back = false) => {
//    $('li').removeClass("active");
//    $('a').removeClass("text-info");
//    // var data = new {ajaxcall = true}
//    var decodeurl = decodeURIComponent(Url).split("//");
//    var urlsplit = "/" + decodeurl[1]
//    $.ajax({
//        type: "GET",
//        data: {
//            "ajaxcall": true,
//        },
//        url: urlsplit,
//        success: function (res) {
//            if (!back) {
//                window.history.pushState(null, null, urlsplit)
//            }                       
//            $("#replace").html(res);
//        },
//        error: function (err) {

//            location.reload();
//        }

//    })



//}
//    $(window).on('popstate', function (event) {
//        var url = "test/" + event.target.location.pathname
//        var back = true;
//        AjaxCall(url, back);
//    //alert(event.target.location.href)
//    //window.history.back();
//});

//window.addEventListener('popstate', e => {
//    console.log(e.currentTarget.);
//})
//window.onhashchange = function ()
//{
//    console.log("click")
//}
//$(document).ready(function () {

//    if (window.history && window.history.pushState) {

//        //window.history.pushState('forward', null, './#forward');

//        $(window).on('popstate', function () {
//            //alert('Back button was pressed.');
//            window.history.go(-1);
//        });

//    }
//});

sendmail = (url, customerId, uniqueReferenceNo) => {
    var decodeurl = decodeURIComponent(url) + "?customerId=" + customerId + "&uniqueReferenceNo=" + uniqueReferenceNo;
    $('#sendmail').hide()
    $.ajax({
        type: "POST",
        url: decodeurl,
        success: function (res) {
            if (res) {
                new PNotify({
                    // title: 'Success ',
                    text: 'Mail Send',
                    type: 'Success',

                    animation: {
                        effect_in: 'fade',
                        effect_out: 'slide'
                    }
                });
            } else {
                new PNotify({
                    // title: 'Success ',
                    text: 'Sorry CouldNot Send Mail',
                    type: 'Error',

                    animation: {
                        effect_in: 'fade',
                        effect_out: 'slide',

                    }
                });

            }

        },
        beforeSend: function () {
            $('.loader').show();


        },
        complete: function () {
            $('.loader').hide();

        },
        error: function (res) {
            //error code here
            new PNotify({
                // title: 'Success ',
                text: 'Internal Server Error',
                type: 'Error',

                animation: {
                    effect_in: 'fade',
                    effect_out: 'slide'
                }

            });
        }
    });
    $('#sendmail').show()

}



ShowPopUp = (Url, title = '', aclass = null) => {
    var decodeurl = decodeURIComponent(Url);

    $.ajax({
        type: "GET",
        url: decodeurl,
        success: function (res) {
            aclass == null ? $("#add-new .modal-dialog").addClass("modal-lg") : $("#add-new .modal-dialog").removeClass().addClass('modal-dialog').addClass(aclass);
            $("#add-new .modal-body").html(res);
            $("#add-new .modal-body").append("<div id='modalloading'  class='loader'><center><span class='fa fa-spinner fa-spin fa-3x'></span></center></div >");
            $("#add-new .modal-title").html(title);
            $("#add-new").modal({ backdrop: 'static', keyboard: false });
            $("#add-new").modal('show');

        },
        beforeSend: function () {
            $('.loader').show();
        },
        complete: function () {
            $('.loader').hide();
        },
        error: function (err) {

            $('.loader').hide();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', decodeurl, true);

            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        // Request was successful, process the response
                        //console.log(xhr.responseText);
                    } else if (xhr.status === 403) {
                        // "403 Forbidden" error
                        //console.log("Access forbidden. You don't have permission to access this resource.!!");
                        // Handle the error gracefully, e.g., show an error message to the user
                        alert("Access forbidden. You don't have permission to access this resource.!!");
                    } else {
                        // Other error cases
                        //console.log("An error occurred: " + xhr.status);
                        // Handle other status codes as needed
                        alert("An error occurred: " + xhr.status);
                    }
                }
            };

            xhr.send();
            console.log("error");

            new PNotify({
                // title: 'Success ',
                text: 'Internal Server Error',
                type: 'Error',

                animation: {
                    effect_in: 'fade',
                    effect_out: 'slide'
                }

            });
        }


    })

}
PopUPAjaxPost = form => {
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.isvalid) {
                $("#TablePartial").html("");
                $("#TablePartial").html(res.html);
                $("#add-new .modal-body").html("");
                $("#add-new .modal-title").html("");
                $("#add-new").modal('hide');
            }
            else {
                $("#add-new .modal-body").html(res.html);
                $("#add-new .modal-dialog").addClass("modal-sm")
            }
        },
        beforeSend: function () {
            $('.loader').show();
        },
        complete: function () {
            $('.loader').hide();
        },
        error: function (err) {
            location.reload();
        }
    })
    return false;
}
PopUPAjaxPost3 = form => {
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {

            $("#customerlist").html("");
            $("#customerlist").html(res);
            $("#add-new .modal-body").html("");
            $("#add-new .modal-title").html("");
            $("#add-new").modal('hide');


        },
        beforeSend: function () {
            $('.loader').show();
        },
        complete: function () {
            $('.loader').hide();
        },
        error: function (err) {
            console.log(err);
        }
    })

}

PopUPAjaxPost2 = form => {
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.isvalid) {
                $("#TablePartial").html("");
                $("#TablePartial").html(res.html);
                $("#add-new .modal-body").html("");
                $("#add-new .modal-title").html("");
                $("#add-new").modal('hide');
            }
            else
                /*   $("#add-new .modal-dialog").addClass("modal-sm");*/
                $("#add-new .modal-body").html(res);
        },
        beforeSend: function () {
            $('.loader').show();
        },
        complete: function () {
            $('.loader').hide();
        },
        error: function (err) {
            console.log(err);
        }
    })
    return false;
}

function CascadeDropdown(Url, value, ReplaceId) {
    var url = Url + value;
    var decodeurl = decodeURIComponent(url);
    $.ajax({
        async: true,
        type: "GET",
        url: decodeurl,
        success: function (res) {
            var items = '<option value="0">--Select--</option > ';
            $.each(res, function (i, IncommingValue) {
                items += "<option value='" + IncommingValue.value + "'>" + IncommingValue.text + "</option>";
            });
            $("#" + ReplaceId).html(items);
        }
    })
};



PopUPAjaxDelete = (Url, title) => {
    var decodeurl = decodeURIComponent(Url);
    $.ajax({
        type: "GET",
        url: decodeurl,
        success: function (res) {
            $("#add-new .modal-dialog").removeClass("modal-lg");
            $("#add-new .modal-dialog").addClass("modal-md");
            $("#add-new .modal-body").html(res);
            $("#add-new .modal-body").append("<div id='modalloading'  class='loader'><center><span class='fa fa-spinner fa-spin fa-3x'></span></center></div >");
            $("#add-new .modal-title").html(title);
            $("#add-new").modal({ backdrop: 'static', keyboard: false });
            $("#add-new").modal('show');
        },
        error: function (err) {

            $('.loader').hide();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', decodeurl, true);

            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        // Request was successful, process the response
                        //console.log(xhr.responseText);
                    } else if (xhr.status === 403) {
                        // "403 Forbidden" error
                        //console.log("Access forbidden. You don't have permission to access this resource.!!");
                        // Handle the error gracefully, e.g., show an error message to the user
                        alert("Access forbidden. You don't have permission to access this resource.!!");
                    } else {
                        // Other error cases
                        //console.log("An error occurred: " + xhr.status);
                        // Handle other status codes as needed
                        alert("An error occurred: " + xhr.status);
                    }
                }
            };

            xhr.send();
            console.log("error");

            new PNotify({
                // title: 'Success ',
                text: 'Internal Server Error',
                type: 'Error',

                animation: {
                    effect_in: 'fade',
                    effect_out: 'slide'
                }

            });
        }
    })

}

PopUPAjaxDeleteOverlay = (Url, title) => {
    var decodeurl = decodeURIComponent(Url);
    $.ajax({
        type: "GET",
        url: decodeurl,
        success: function (res) {
            $("#add-new-overlay .modal-dialog").removeClass("modal-lg");
            $("#add-new-overlay .modal-dialog").addClass("modal-md");
            $("#add-new-overlay .modal-body").html(res);
            $("#add-new-overlay .modal-body").append("<div id='modalloading'  class='loader'><center><span class='fa fa-spinner fa-spin fa-3x'></span></center></div >");
            $("#add-new-overlay .modal-title").html(title);
            $("#add-new-overlay").modal({ backdrop: 'static', keyboard: false });
            $("#add-new-overlay").modal('show');
        },
        error: function (err) {

            $('.loader').hide();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', decodeurl, true);

            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        // Request was successful, process the response
                        //console.log(xhr.responseText);
                    } else if (xhr.status === 403) {
                        // "403 Forbidden" error
                        //console.log("Access forbidden. You don't have permission to access this resource.!!");
                        // Handle the error gracefully, e.g., show an error message to the user
                        alert("Access forbidden. You don't have permission to access this resource.!!");
                    } else {
                        // Other error cases
                        //console.log("An error occurred: " + xhr.status);
                        // Handle other status codes as needed
                        alert("An error occurred: " + xhr.status);
                    }
                }
            };

            xhr.send();
            console.log("error");

            new PNotify({
                // title: 'Success ',
                text: 'Internal Server Error',
                type: 'Error',

                animation: {
                    effect_in: 'fade',
                    effect_out: 'slide'
                }

            });
        }
    })

}


function readURL(input, id) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {

            var data = e.target.result;

            $('#' + id).attr('src', e.target.result);

        }
        reader.readAsDataURL(input.files[0]);
    }
};

//ChangePagesize = (url, data) => {

//    var decodeurl = decodeURIComponent(url).split("/");
//    var urlsplit = decodeurl[1];
//    console.log(url, "Url");

//    var FilterDetails = {
//        SearchName: $('#tblSearch_' + urlsplit).val(),
//        CurrentPageValue: JSON.stringify(data),
//        PageSizer: $('#pagesize_' + urlsplit).find('option:selected').val()
//    };
//    var jsonData = JSON.stringify(FilterDetails);
//    $.ajax({
//        type: "POST",
//        url: url,
//        data: jsonData,
//        contentType: "application/json",
//        dataType: "json",
//        success: function(result) {
//            console.log(result.html);
//            $("#TablePartial").html("");
//            $("#TablePartial").html(result.html);

//        },
//        error: function (response) {
//            alert("Error: " + response);
//        }
//    });

//}




ChangePagesize2 = (url, data) => {
    var FilteDetails = {
        CustomerId: $('#1CustomerId').val(),
        FullName: $('#1FullName').val(),
        MobileNo: $('#1MobileNo').val(),
        Email: $('#1Email').val(),
        RegisteredDateFrom: $('#1RegisteredDateFrom').val(),
        RegisteredDateTo: $('#1RegisteredDateTo').val(),
        CustomerStatusCode: "",
        KycStatusCode: "",
        SearchVal: $('#1SearchVal').val(),
        PageNumber: parseInt(data),
        PageSize: $('#1PageSize').val(),
        OrderBy: "",


    };
    var Url = url + "?ajaxcall=true"
    var jsonData = JSON.stringify(FilteDetails);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonData,
        contentType: "application/json",

        success: function (result) {
            console.log(result);

            $("#customerlist").html("");
            $("#customerlist").html(result);

        },
        error: function (result) {
            console.log(result)
        }

    });
    return false;
}

//Sorting = (url, data, sortExpression) => {
//    var decodeurl = decodeURIComponent(url).split("/");
//    var urlsplit = decodeurl[1];

//    var FilterDetails = {
//        SearchName: $('#tblSearch_' + urlsplit).val(),
//        CurrentPageValue: data,
//        PageSizer: $('#pagesize_' + urlsplit).find('option:selected').val(),
//        sortExpression: sortExpression,
//    };

//    var jsonData = JSON.stringify(FilterDetails);
//    $.ajax({
//        type: "POST",
//        url: url,
//        data: jsonData,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (result) {
//            console.log(result.html);
//            $("#TablePartial").html("");
//            $("#TablePartial").html(result.html);

//        },
//        error: function (response) {
//            alert("Error: " + response);
//        }
//    });
//}





function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 46 || charCode > 57 || charCode == 47)) {
        return false;
    }

    return true;
}
function changeMenuIsactive(Url, MenuId, Action) {
    var decodeurl = decodeURIComponent(Url);
    var value = Action.checked;

    $.ajax({
        url: decodeurl,
        type: 'POST',
        data: { Id: MenuId, IsActive: value },
        success: function (res) {
            /*window.location.reload();*/
        }
    })
}

function changeDisplayOrder(Url, MenuId, DisplayOrder) {
    var decodeurl = decodeURIComponent(Url);
    var value = DisplayOrder.value;

    $.ajax({
        url: decodeurl,
        type: 'POST',
        data: { Id: MenuId, DisplayOrderValue: value },
        success: function (res) {
            /*window.location.reload();*/
        }
    })
}

$('input[type="number"]').click(function () {
    if (this.value < 0) {
        this.value = 0;
    }
});

function resetForm(formId) {
    const form = document.getElementById(formId);
    form.reset();
}
function onimagechange(data, id) {
    var isValid = validateImageFile(data, id);
    if (isValid) {
        $('#' + id).removeAttr('hidden');
        readURL(data, id);
    }
}
function validateImageFile(fileInput, id) {
    var file = fileInput.files[0];

    if (file) {
        var allowedExtensions = ["jpg", "jpeg", "png", "svg", "gif", "bmp"];
        var fileExtension = file.name.split(".").pop().toLowerCase();

        if (!allowedExtensions.includes(fileExtension)) {
            fileInput.value = ""; // Clear the file input
            $(id + "0").html("Invalid image format. Only JPEG, PNG, SVG, GIF, and BMP formats are allowed.");

            alert("Invalid image format. Only JPEG, PNG, SVG, GIF, and BMP formats are allowed.");
            return false;
        }
        // Check file size
        var fileSize = file.size / 1024; // Size in KB
        var maxFileSize = 1000; // Maximum file size in KB

        if (fileSize > maxFileSize) {
            fileInput.value = ""; // Clear the file input
            $(id + "0").html("File size exceeds the maximum limit of 1000 KB.");

            alert("File size exceeds the maximum limit of 1000 KB.");
            return false;
        }
    }
    return true;
}
EmployeeRole = (url) => {
    var decodeurl = decodeURIComponent(url);

    $.ajax({
        type: "POST",
        url: decodeurl,
        success: function (res) {
            if (res) {
                new PNotify({
                    // title: 'Success ',
                    text: 'Mail Send',
                    type: 'Success',

                    animation: {
                        effect_in: 'fade',
                        effect_out: 'slide'
                    }
                });
            } else {
                new PNotify({
                    // title: 'Success ',
                    text: 'Sorry CouldNot Send Mail',
                    type: 'Error',

                    animation: {
                        effect_in: 'fade',
                        effect_out: 'slide',

                    }
                });

            }

        },
        beforeSend: function () {
            $('.loader').show();


        },
        complete: function () {
            $('.loader').hide();

        },
        error: function (res) {
            $('.loader').hide();
            //error code here
            new PNotify({
                // title: 'Success ',
                text: 'Internal Server Error',
                type: 'Error',

                animation: {
                    effect_in: 'fade',
                    effect_out: 'slide'
                }

            });
        }
    });


}


//document.getElementByClass("add-btn").addEventListener("click", function () {
//    alert(1);
//    // Disable the button immediately
//    this.disabled = true;

//    // Use an AJAX request to check if the user has access
//    fetch('/checkAccessUrl')
//        .then(response => {
//            if (!response.ok) {
//                // Re-enable the button if access is denied
//                document.getElementByClass("add-btn").disabled = false;
//            }
//        })
//        .catch(error => {
//            console.error('Error checking access:', error);
//            // Re-enable the button if there's an error
//            document.getElementByClass("add-btn").disabled = false;
//        });
//});

// Call the function on any input field with a value of zero


