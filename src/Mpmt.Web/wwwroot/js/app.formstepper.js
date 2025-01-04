var FormStepper = {
    settings: {
        formSelector: undefined,
        validateSteps: true,
        validationSkipSteps: [],
        validators: [],
        currentStep: 1,
        totalSteps: 1,
    },
    init: function (settings) {
        this.settings = $.extend({}, this.settings, settings);

        if (this.settings.validateSteps)
            $.validator.unobtrusive.parse(this.settings.formSelector);

        this.showStep();
        this.setButtons();
    },
    isStepValid: function () {
        let selectorCurrentStep = '#formStep' + this.settings.currentStep;

        let $fieldsUnobtrusiveEnabled = $(selectorCurrentStep + " input");
        $.validator.unobtrusive.parse($fieldsUnobtrusiveEnabled);
        let isUnobtrusiveEnabledValid = $fieldsUnobtrusiveEnabled.valid();

        let $enabledFields = $(selectorCurrentStep + " :input:not(:disabled)");
        let isEnabledFieldsValid = true;
        $enabledFields.each(function () {
            if (!$(this).valid()) {
                isEnabledFieldsValid = false;
                //return false;
            }
        });

        // here
        let isCustomValidationsPassed = true;
        let customErrorsText = '';
        if (this.settings.validators) {
            for (const validator of this.settings.validators) {
                if (validator.step === this.settings.currentStep && validator.selectorValidationSummary && validator.list) {
                    for (const valObj of validator.list) {
                        if (typeof valObj.validator === 'function') {
                            let isValid = valObj.validator();
                            if (!isValid) {
                                isCustomValidationsPassed = false;
                                customErrorsText += '<li class="text-danger">' + valObj.message + '</li>'
                            }
                        }
                    }
                }
                if (!isCustomValidationsPassed) {
                    $(validator.selectorValidationSummary).html(customErrorsText);
                } else {
                    $(validator.selectorValidationSummary).html("");
                }
            }
        }
        // here end

        var isAllFieldsValid = isUnobtrusiveEnabledValid && isEnabledFieldsValid && isCustomValidationsPassed;
        return isAllFieldsValid;
    },
    setButtons: function () {
        if (!this.settings.formSelector)
            return;

        // first & final
        if (this.settings.currentStep === 1 && this.settings.currentStep === this.settings.totalSteps) {
            $(this.settings.formSelector + ' #btnSubmit').show();
            $(this.settings.formSelector + ' #btnPrev').hide();
            $(this.settings.formSelector + ' #btnNext').hide();
            return;
        }

        // first
        if (this.settings.currentStep === 1 && this.settings.currentStep < this.settings.totalSteps) {
            $(this.settings.formSelector + ' #btnSubmit').hide();
            $(this.settings.formSelector + ' #btnPrev').hide();
            $(this.settings.formSelector + ' #btnNext').show();
            $(this.settings.formSelector + ' #btnNext').text('Next');
            return;
        }

        // intermediary
        if (this.settings.currentStep < this.settings.totalSteps) {
            $(this.settings.formSelector + ' #btnSubmit').hide();
            $(this.settings.formSelector + ' #btnPrev').show();
            $(this.settings.formSelector + ' #btnPrev').text('Previous');
            $(this.settings.formSelector + ' #btnNext').show();
            $(this.settings.formSelector + ' #btnNext').text('Next');
            return;
        }

        // final
        if (this.settings.currentStep === this.settings.totalSteps) {
            $(this.settings.formSelector + ' #btnNext').hide();
            $(this.settings.formSelector + ' #btnPrev').show();
            $(this.settings.formSelector + ' #btnPrev').text('Previous');
            $(this.settings.formSelector + ' #btnSubmit').show();
            return;
        }
    },
    showStep: function (step = undefined) {

        if (step && typeof step === "number")
            this.settings.currentStep = step;

        const currentFormStepSelector = this.settings.formSelector + ' #' + 'formStep' + this.settings.currentStep;
        const formStepsWithoutCurrentStepSelector = this.settings.formSelector + ' .modal-form-step' + ':not(' + currentFormStepSelector + ')';

        $(formStepsWithoutCurrentStepSelector).hide();
        $(currentFormStepSelector).show();
    },

    prev: function (successCallback = null) {
        if (!this.settings.formSelector)
            return;

        if (this.settings.currentStep === 1 || this.settings.currentStep < 1)
            return;

        if (successCallback)
            successCallback();

        this.settings.currentStep = Math.max(this.settings.currentStep - 1, 1);

        this.showStep();
        this.setButtons();
    },
    next: function (successCallback = null) {
        if (!this.settings.formSelector)
            return;

        if (this.settings.currentStep >= this.totalSteps)
            return;

        if (this.settings.validateSteps && $.inArray(this.settings.currentStep, this.settings.validationSkipSteps) == -1) {
            let isCurrentStepValid = this.isStepValid();
            if (!isCurrentStepValid)
                return;
        }

        if (successCallback)
            successCallback();

        this.settings.currentStep = Math.min(this.settings.currentStep + 1, this.settings.totalSteps);

        this.showStep();
        this.setButtons();
    }
}