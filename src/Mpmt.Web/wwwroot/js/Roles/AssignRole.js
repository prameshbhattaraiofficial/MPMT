//validation All Columns
function ValidationAllfunc(Methods) {
    var viewvalue = $('#viewAll').is(':checked');
    var Createvalue = $('#CreateAll').is(':checked');
    var Editvalue = $('#EditAll').is(':checked');
    var Deletevalue = $('#DeleteAll').is(':checked');
    switch (Methods) {
        case "View":
            if (viewvalue == true) {
                $('.ViewCheck').prop('checked', true);
            } else {
                $('#CreateAll').prop('checked', false);
                $('#EditAll').prop('checked', false);
                $('#DeleteAll').prop('checked', false);
                $('.ViewCheck').prop('checked', false);
                $('.CreateCheck').prop('checked', false);
                $('.EditCheck').prop('checked', false);
                $('.DeleteCheck').prop('checked', false);
            }
            break;
        case "Create":
            if (viewvalue == false) {
                Createvalue == true ? $('#CreateAll').prop('checked', true) : $('#CreateAll').prop('checked', false)
                $('#viewAll').prop('checked', true);
                $('.ViewCheck').prop('checked', true);
            }
            Createvalue == true ? $('.CreateCheck').prop('checked', true) : $('.CreateCheck').prop('checked', false);
            break;
        case "Edit":
            if (viewvalue == false) {
                Editvalue == true ? $('#EditAll').prop('checked', true) : $('#EditAll').prop('checked', false)
                $('#viewAll').prop('checked', true);
                $('.ViewCheck').prop('checked', true);
            }
            Editvalue == true ? $('.EditCheck').prop('checked', true) : $('.EditCheck').prop('checked', false);
            break;
        case "Delete":
            if (viewvalue == false) {
                Deletevalue == true ? $('#DeleteAll').prop('checked', true) : $('#DeleteAll').prop('checked', false)
                $('#viewAll').prop('checked', true);
                $('.ViewCheck').prop('checked', true);
            }
            Deletevalue == true ? $('.DeleteCheck').prop('checked', true) : $('.DeleteCheck').prop('checked', false);
            break;
        default:
            break;
    }
}
//Post-checkbox-Data
function PostCheckBox() {
    var permissions = new Array();
    $("#PermissionCheckbox TBODY TR").each(function () {
        var row = $(this);
        var Permission = {};
        Permission.Name = row.find("TD").eq(0).html();
        Permission.viewper = row.find("TD").eq(1).find('.ViewCheck').is(':checked') ? true : false;
        Permission.Createper = row.find("TD").eq(2).find('.CreateCheck').is(':checked') ? true : false;
        Permission.Updateper = row.find("TD").find('.EditCheck').is(':checked') ? true : false;
        Permission.Deleteper = row.find("TD").find('.DeleteCheck').is(':checked') ? true : false;
        Permission.menuid = row.find("TD").find('.menuval').val();
        Permission.RoleId = $("#roleid").val();
        permissions.push(Permission);
    });
    debugger;
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        type: "POST",
        url: "/Admin/Roles/AssignRole",
        data: JSON.stringify(permissions),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            // Include the anti-forgery token in the request headers
            "RequestVerificationToken": token
        },
        success: function (result) {
            if (result == true) {

                $("#add-new .modal-body").html("");
                $("#add-new .modal-title").html("");
                $("#add-new").modal('hide');

            }
            else {
                /*swal("Oops", "Permission Updated Failed", "error");*/
            }
        },
        beforeSend: function () {

            $('.ajax-loading-busy').removeClass('display-none');
        },
        error: function (xhr, status, error) {

            console.error('POST request error:', error);

        },
        complete: function () {
            $('.ajax-loading-busy').addClass('display-none');
        }
    });

}
//Validate Indivisual Columns
function ValidationListFun(method, value, parentid) {
    debugger;
    var viwcheck = $('#viewper-' + value).is(':checked');
    var Viewid = $('#viewper-' + value);
    var Createid = $('#createper-' + value);
    var Editid = $('#editper-' + value);
    var Deleteid = $('#deleteper-' + value);
    //parentid
    var PViewid = $('#viewper-' + parentid);
    var checkboxes = document.querySelectorAll('.Row-' + value + ' input[type="checkbox"]');

    

    switch (method) {
        case "View":
            if (viwcheck == false) {
                $('#viewAll').prop('checked', false);
                $('#CreateAll').prop('checked', false);
                $('#EditAll').prop('checked', false);
                $('#DeleteAll').prop('checked', false);
                Createid.prop('checked', false);
                Editid.prop('checked', false);
                Deleteid.prop('checked', false);
            }
            debugger;
            if (parentid>0) {
                PViewid.prop('checked', true);
            }
            else {
                for (var i = 0; i < checkboxes.length; i++) {
                    checkboxes[i].checked = false;
                }
            }
            
            break;
        case "create":
            if (Createid.is(':checked') == false) {
                $('#CreateAll').prop('checked', false);
            }
            else
                Viewid.prop('checked', true);
            if (parentid > 0) {
                PViewid.prop('checked', true);
            }
            break;
        case "edit":
            if (Editid.is(':checked') == false) {
                $('#EditAll').prop('checked', false);
            }
            else
                Viewid.prop('checked', true);
            if (parentid > 0) {
                PViewid.prop('checked', true);
            }
            break;
        case "delete":
            if (Deleteid.is(':checked') == false) {
                $('#DeleteAll').prop('checked', false);
            }
            else
                Viewid.prop('checked', true);
            if (parentid > 0) {
                PViewid.prop('checked', true);
            }
            break;
        default:
    }
}