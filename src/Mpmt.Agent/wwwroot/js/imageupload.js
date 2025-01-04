function onimageUploadchange(data, id, hideid, textid) {
    
    readURL(data, id, textid);
    $('#' + hideid).attr("hidden", true);

}
function readURL(input, id, textid) {

    if (input.files && input.files[0]) {
        if (input.files[0].type.split('/')[0] != 'image') {
            $('#' + id).attr('src', "");
            document.getElementById(textid).innerHTML = 'Invalid file extension'
            $('#' + textid).removeAttr('hidden');
            return false
        }
        else if (input.files[0].size < (5 * 1024 * 1024)) {
            var reader = new FileReader();
            reader.onload = function (e) {

                var data = e.target.result;
                document.getElementById(textid).innerHTML = ''
                $('#' + id).attr('src', e.target.result);
                $('#' + id).removeAttr('hidden');
            }
            reader.readAsDataURL(input.files[0]);
        }
        else {
            $('#' + id).attr('src', "");
            document.getElementById(textid).innerHTML = 'Invalid Image File Size (<5 MB)'
            $('#' + textid).removeAttr('hidden');
        }
    }
};

function onimageAddchange(data, id, textid) {
    readURL(data, id, textid);


}
