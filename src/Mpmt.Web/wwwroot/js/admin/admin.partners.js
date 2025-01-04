
function copyText(fieldid) {
    var copyText = document.getElementById(fieldid);

    // Select the text field
    copyText.select();
    copyText.setSelectionRange(0, 99999); // For mobile devices

    // Copy the text inside the text field
    navigator.clipboard.writeText(copyText.value);
}

function downloadRsaFileAsPem(url, bodyObj) {
    if (!url)
        throw new Error('Invalid url.');

    if (!bodyObj)
        throw new Error('Invalid body.');

    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(bodyObj),
        contentType: "application/json",
        success: function (data) {
            var blob = new Blob([data], { type: 'application/x-pem-file' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = bodyObj.CredentialType + '.pem';
            link.click();
        },
        error: function (ex) {
            console.log('Error downloading file.');
        }
    });
}