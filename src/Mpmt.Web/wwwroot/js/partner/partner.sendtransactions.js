var TxnSummaryContext = {};

const AddTxnFormHandler = {
    settings: {
        method: 'POST',
        actionUrl: undefined,
        selector: undefined,
        containerSelector: undefined
    },

    init: function (settings, successCallback, checkvalid = true) {
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
            if (checkvalid && !$(this).valid())
                return false;

            var selectorFieldsSubmit = formSettings.selector + " button[type='submit'], " + formSettings.selector + " input[type='submit']";
            $(selectorFieldsSubmit).prop("disabled", true);

            if (!formSettings.processIdUrl)
                throw new Error('Unable to get processId.');

            var processRefId;
            try { processRefId = crypto.randomUUID(); }
            catch (err) { }
            if (!processRefId) {
                processRefId = "10000000-1000-4000-8000-100000000000".replace(/[018]/g, function (c) {
                    return (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
                });
            }
            var processIdPayload = { referenceId: processRefId };
            var formData = new FormData(this);

            $.ajax({
                cache: false,
                contentType: "application/json",
                url: formSettings.processIdUrl,
                type: 'POST',
                data: JSON.stringify(processIdPayload),
                success: function (res) {
                    if (!res.data)
                        return;

                    formData.append('ProcessId', res.data);
                    $.ajax({
                        cache: false,
                        url: formSettings.actionUrl,
                        type: 'POST',
                        data: formData,
                        contentType: false,
                        processData: false,
                        success: function (res) {
                            if (successCallback && typeof successCallback === 'function') {
                                successCallback();
                            }
                            console.log('success');
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
                                    break;
                                default:
                                    // handle default;
                            }
                        },
                        complete: function () {
                            $(selectorFieldsSubmit).prop("disabled", false);
                            $('.loader').hide();
                        }
                    });
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
                            break;
                        default:
                            console.log(err.responseText);
                        // handle default
                    }

                    $(selectorFieldsSubmit).prop("disabled", false);
                    $('.loader').hide();
                }
            });
        });
    }
}

function setTxnSummaryContext(obj) {
    TxnSummaryContext = $.extend({}, TxnSummaryContext, obj);
}

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

function getTxnChargeDetailsPayload() {
    let paymentType = $('#addTransactionForm #paymentType').val();
    let sourceCurrency = $('#addTransactionForm #sourceCurrency').val();
    let sourceAmount = $('#addTransactionForm #sourceAmount').val();
    let destinationCurrency = $('#addTransactionForm #destinationCurrency').val();

    return {
        PaymentType: paymentType,
        SourceCurrency: sourceCurrency,
        SourceAmount: sourceAmount,
        DestinationCurrency: destinationCurrency
    }
}

function getTxnChargeDetailsPayloadForNepToOthers() {
    let paymentType = $('#addTransactionForm #paymentType').val();
    let sourceCurrency = $('#addTransactionForm #destinationCurrency').val();
    let sourceAmount = $('#addTransactionForm #receivingAmount').val();
    let destinationCurrency = $('#addTransactionForm #sourceCurrency').val();

    return {
        PaymentType: paymentType,
        SourceCurrency: sourceCurrency,
        SourceAmount: sourceAmount,
        DestinationCurrency: destinationCurrency
    }
}


function toggleChargeFields() {
    var isSourceAmountDisabled = $("#addTransactionForm #sourceAmount").prop("disabled");

    if (isSourceAmountDisabled) {
        $("#addTransactionForm #sourceAmount").prop("disabled", false);
        $('#addTransactionForm #sourceCurrency').prop("disabled", false);
    } else {
        $("#addTransactionForm #sourceAmount").prop("disabled", true);
        $('#addTransactionForm #sourceCurrency').prop("disabled", true);
    }
}

function resetTxnChargeDetails() {
    $('#addTransactionForm #sourceAmount').val('0');
    $('#addTransactionForm #totalFees').text('0.0');
    $('#addTransactionForm #netSendingAmount').text('0.0');
    $('#addTransactionForm #conversionRate').text('0.0');
    $('#addTransactionForm #receivingAmount').val('0');
}

function setTxnChargeDetails(data) {
    debugger;
    $('#addTransactionForm #hfNetSendingAmount').val(data.netSendingAmount);

    $('#addTransactionForm #hfServiceCharge').val(data.serviceCharge);
    $('#addTransactionForm #hfPartnerServiceCharge').val(data.partnerServiceCharge);
    $('#addTransactionForm #hfConversionRate').val(data.conversionRate);
    $('#addTransactionForm #hfNetRecievingAmountNPR').val(data.receivingAmountNPR);

    //$('#addTransactionForm #sourceAmount').val(data.sendingAmount);
    $('#addTransactionForm #totalFees').text(data.serviceCharge + ' ' + data.sourceCurrency);
    $('#addTransactionForm #netSendingAmount').text(data.netSendingAmount + ' ' + data.sourceCurrency);
    $('#addTransactionForm #conversionRate').text(data.conversionRate);
    $('#addTransactionForm #receivingAmount').val(data.receivingAmountNPR);
}
function setTxnChargeForNepali(data) {
    $('#addTransactionForm #hfNetSendingAmount').val(data.netSendingAmount);
    $('#addTransactionForm #sourceAmount').val(data.netSendingAmount);
    $('#addTransactionForm #hfServiceCharge').val(data.serviceCharge);
    $('#addTransactionForm #hfPartnerServiceCharge').val(data.partnerServiceCharge);
    $('#addTransactionForm #hfConversionRate').val(data.conversionRate);
    $('#addTransactionForm #hfNetRecievingAmountNPR').val(data.receivingAmountNPR);

    //$('#addTransactionForm #sourceAmount').val(data.sendingAmount);
    $('#addTransactionForm #totalFees').text(data.serviceCharge + ' ' + data.sourceCurrency);
    $('#addTransactionForm #netSendingAmount').text(data.netSendingAmount + ' ' + data.sourceCurrency);
    $('#addTransactionForm #conversionRate').text(data.conversionRate);
    $('#addTransactionForm #receivingAmount').val(data.receivingAmountNPR);
}
function setFocusOnFields() {
    $('#addTransactionForm #sourceAmount').focus();
}

function showErrorsMessages(data) {
    if (data.errors && data.errors.length > 0) {
        let errorsText = '';
        for (let i = 0; i < data.errors.length; i++)
            errorsText += '<li class="text-danger">' + data.errors[i] + '</li>'

        $("#formErrorsWrapper").html(errorsText);
    }
}

function clearErrorMessages() {
    $("#formErrorsWrapper").html("");
    $(".validation-summary-errors").html("");
}

function getExistingSenderList(url, data) {
    setLoadWaiting(1);
    $.ajax({
        cache: false,
        contentType: "application/json",
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        success: function (res) {
            $('#addTransactionForm #existingSenderListContainer').html(res);
        },
        error: function (xhr, status, error) {
            $('#addTransactionForm #existingSenderListContainer')
                .html('<div class="my-2 col-md-12 d-flex justify-content-center align-items-center"><p>No records found.</p></div>');
        },
        complete: function () {
            setLoadWaiting(0);
        }
    });
}

function getExistingReciverList(url, data) {
    setLoadWaiting(1);
    $.ajax({
        cache: false,
        contentType: "application/json",
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        success: function (res) {
            $('#addTransactionForm #existingReciverListContainer').html(res);
        },
        error: function (xhr, status, error) {
            $('#addTransactionForm #existingReciverListContainer')
                .html('<div class="my-2 col-md-12 d-flex justify-content-center align-items-center"><p>No records found.</p></div>');
        },
        complete: function () {
            setLoadWaiting(0);
        }
    });
}
function ConvertAmmountRecevingToSender(url, data) {
    if (data.SourceAmount <= 0) {
        resetTxnChargeDetails();
        setFocusOnFields();
        return;
    }

    toggleChargeFields();
    setLoadWaiting(1);

    $.ajax({
        cache: false,
        contentType: "application/json",
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        success: function (res) {
            setLoadWaiting(0);
            clearErrorMessages();
            if (res.data == null || res.data == undefined) {
                resetTxnChargeDetails();
                setFocusOnFields();
            }
            else {
                setTxnChargeForNepali(res.data);
                setTxnSummaryContext({

                    paymentType: data.PaymentType,
                   // sourceCurrency: data.SourceCurrency,
                    destinationCurrency: data.DestinationCurrency,
                    sendingAmount: res.data.sendingAmount,
                    serviceCharge: res.data.serviceCharge,
                   // netSendingAmount: res.data.netSendingAmount,
                    conversionRate: res.data.conversionRate,
                   // receivingAmountNPR: res.data.receivingAmountNPR,
                    existingSender: false,
                    bankName: "",
                    accountHolder: "",
                    accountNumber: "",
                    branch: ""
                });
            }
        },
        error: function (xhr, status, error) {
            setLoadWaiting(0);
            resetTxnChargeDetails();
            showErrorsMessages(xhr.responseJSON);
        },
        complete: function () {
            setLoadWaiting(0);
            toggleChargeFields();
            setFocusOnFields();
        }
    });
}
function getSenderTxnChargeDetails(url, data) {
    debugger;
    if (data.SourceAmount <= 0) {
        resetTxnChargeDetails();
        setFocusOnFields();
        return;
    }

    toggleChargeFields();
    setLoadWaiting(1);

    $.ajax({
        cache: false,
        contentType: "application/json",
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        success: function (res) {
            setLoadWaiting(0);
            clearErrorMessages();
            if (res.data == null || res.data == undefined) {
                resetTxnChargeDetails();
                setFocusOnFields();
            }
            else {
                setTxnChargeDetails(res.data);
                setTxnSummaryContext({

                    paymentType: data.PaymentType,
                    sourceCurrency: data.SourceCurrency,
                    destinationCurrency: data.DestinationCurrency,
                    sendingAmount: res.data.sendingAmount,
                    serviceCharge: res.data.serviceCharge,
                    netSendingAmount: res.data.netSendingAmount,
                    conversionRate: res.data.conversionRate,
                    receivingAmountNPR: res.data.receivingAmountNPR,
                    existingSender: false,
                    bankName: "",
                    accountHolder: "",
                    accountNumber: "",
                    branch: ""
                });
            }
        },
        error: function (xhr, status, error) {
            setLoadWaiting(0);
            resetTxnChargeDetails();
            showErrorsMessages(xhr.responseJSON);
        },
        complete: function () {
            setLoadWaiting(0);
            toggleChargeFields();
            setFocusOnFields();
        }
    });

}
