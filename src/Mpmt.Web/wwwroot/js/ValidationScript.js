
function isPhoneNumber(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}
function lettersOnly(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
    if (charCode > 32 && (charCode < 65 || charCode > 90) &&
        (charCode < 97 || charCode > 122)) {
        return false;
    }
    return true;
}

function containsOnlySpecialCharacters(event) {

    // Define a regular expression to match only "-" "/", and "\" characters
    const regex = /^[\/\-\\\.]+$/;

    // Test if the input matches the regex, and return false if it does not
    if (!regex.test(event.key)) {
        return false;
    }

    // If the input matches the regex, return true
    return true;
}
function validateDateInput(event) {
    const input = event.target;
    const inputValue = input.value;

    // Allow certain key codes (such as arrow keys, backspace, etc.)
    if ([8, 9, 37, 39, 46].includes(event.keyCode)) {
        return true;
    }

    // Check if the input value matches the YYYY-MM-DD format
    const pattern = /^\d{4}-\d{2}-\d{2}$/;
    if (!pattern.test(inputValue)) {
        event.preventDefault();
        return false;
    }

    // Additional validation if needed (e.g., checking month and day ranges)

    return true;
}

