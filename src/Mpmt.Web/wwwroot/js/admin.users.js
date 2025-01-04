
var FormHandler = {
    settings: {
        method: 'POST',
        actionUrl: undefined,
        selector: undefined,
        containerSelector: undefined
    },

    init: function (settings, successCallback, checkvalid = true, validatorfunctions = undefined) {
     
        if (!settings)
            throw new Error('parameter settings is requeired!');

        if (!settings.selector)
            throw new Error('selector is not defined in settings');

        if (!settings.actionUrl)
            throw new Error('actionUrl is not defined in settings');

        if (!settings.containerSelector)
            throw new Error('containerSelector is not defined in settings');

        var formSettings = $.extend({}, this.settings, settings);
        var $formEl = $(formSettings.selector);
        if (checkvalid) {
            $.validator.unobtrusive.parse($formEl);
        }

        $formEl.on('submit', function (e) {

            e.preventDefault();
            if (checkvalid) {
                if (!$(this).valid()) {
                    return false;
                }
            }

            //var dataToHash = $(this).serialize(); 
            //console.log(dataToHash, " daata to hash");
            //var hashedData = CryptoJS.AES.encrypt(dataToHash.toString(), "secretkey").toString();
            //console.log(hashedData, "hashed daata");


            //var bytes = CryptoJS.AES.decrypt(hashedData, "secretkey");
            //var decrypted = bytes.toString(CryptoJS.enc.Utf8);
            //console.log(decrypted, "Decrypted daata");

            var selectorFieldsSubmit = formSettings.selector + " button[type='submit'], " + formSettings.selector + " input[type='submit']";
            $(selectorFieldsSubmit).prop("disabled", true);

            $.ajax({
                cache: false,
                url: formSettings.actionUrl,
                type: 'POST',
                data: new FormData(this),
                contentType: false,
                processData: false,
                success: function (res) {

                    if (successCallback && typeof successCallback === 'function') {
                        successCallback();
                    }
                },
                beforeSend: function () {

                    $('.loader').show();
                },
                error: function (err) {

                    switch (err.status) {
                        case 400:
                            $(formSettings.containerSelector).html(err.responseText);
                            // Reattach handlers
                            //FormHandler.init(formSettings);
                            break;
                        case 404:
                            new PNotify({
                                title: 'Error notice',
                                text: 'There was some Issue.',
                                type: 'error',
                                animation: {
                                    effect_in: 'fade',
                                    effect_out: 'slide'
                                }

                            });
                            console.log("Not Found!");
                            break;
                        default:
                            console.log(err.responseText);
                        // handle default
                    }
                },
                complete: function () {
                    $(selectorFieldsSubmit).prop("disabled", false);
                    $('.loader').hide();
                }
            });
        });
    }
}