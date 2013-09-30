var dialogueStore = dialogueStore || {};

dialogueStore.initValidator = function () {

    if ($.validator) {
        $.validator.setDefaults({
            errorClass: 'has-error',
            highlight: function (element, errorClass, validClass) {
                console.log("highlight");
                $(element).closest(".form-group").addClass(errorClass).removeClass(validClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                console.log("unhighlight");
                $(this).closest(".form-group").removeClass(errorClass).addClass(validClass);
            }
        });
    }

    $('span.field-validation-valid, span.field-validation-error').each(function () {
        if (!$(this).hasClass('help-inline')) {
            $(this).addClass('help-inline');
        }
    });
};

dialogueStore.initAutosizer = function () {
    $('textarea').autosize({ append: "\n" }); // Source: http://www.jacklmoore.com/autosize
};

dialogueStore.initDatePicker = function () {
    Date.format = 'dd/mm/yy';
    $.datepicker.regional['en-GB'] = {
        closeText: 'Done',
        prevText: 'Prev',
        nextText: 'Next',
        currentText: 'Today',
        monthNames: ['January', 'February', 'March', 'April', 'May', 'June',
                'July', 'August', 'September', 'October', 'November', 'December'],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        weekHeader: 'Wk',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };

    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        currentText: 'Today',
        duration: 'fast',
        showStatus: true,
        maxDate: new Date(2030, 1 - 1, 1),
        minDate: new Date(1940, 1 - 1, 1),
        showAnim: 'drop',
        showButtonPanel: false,
        yearRange: '1900:2030',
        dateFormat: 'dd/mm/yy',
        altFormat: "dd/mm/yy"
    });
};

$(function () {

    $("input[type=checkbox]").each(function () {
        if (this.checked) {
            // Checkbox is checked
            $(this).closest("tr").addClass("success");
        }
    });

    $("input[type=checkbox]").on("click", function () {
        if (this.checked) {
            // Checkbox is checked
            $(this).closest("tr").addClass("success");
        } else {
            // Checkbox is unchecked
            $(this).closest("tr").removeClass("success");
        }
    });

    dialogueStore.initValidator();
    dialogueStore.initAutosizer();
    dialogueStore.initDatePicker();

    $('input[type=date]').each(function (index, element) {
        $(element).attr("type", "text");

        if (!$(element).hasClass('date')) {
            $(element).addClass('date');
        }
    });

    $('input[type=date]').datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy'
    });

    $('.date').datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy'
    });

    // Show validation summary in a dismissable Bootstrap alert box
    var closeBtnHtml = "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>";
    $('.validation-summary-errors').addClass("alert").addClass("alert-dismissable alert-danger").prepend(closeBtnHtml);

    toastr.options.positionClass = 'toast-top-center';
    toastr.options.positionClass = "toast-bottom-full-width";

    $('form label').each(function (index, element) {
        if (!$(element).hasClass('control-label')) {
            $(element).addClass('control-label');
        }
    });

    $('form input').each(function (index, element) {
        if (!$(element).hasClass('form-control')) {
            if (element.type != 'checkbox' && element.type != 'hidden') {
                $(element).addClass('form-control');
            }
        }
    });

    $('form select, form textarea').each(function (index, element) {
        if (!$(element).hasClass('form-control')) {
            $(element).addClass('form-control');
        }
    });

    $('.input-validation-error').each(function () {
        console.log("error field");
    });

    $('textarea').each(function (index, element) {
        if (!$(element).hasClass('autosize')) {
            $(element).addClass('autosize');
        }

        $(element).autosize({ append: "\n" }); // Source: http://www.jacklmoore.com/autosize
    });
});