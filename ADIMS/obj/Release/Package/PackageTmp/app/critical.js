(function ($) {

    $('.moistureReadingDone').on('change', function () {

        var a = $('.moistureReadingDone').val();

        if (a == "Yes")
        {
            $('.moistureContentInGrain').show();
        } else {
            $('.moistureContentInGrain').hide();
        }

    });


    $('.cropInsurance').change(function ()
    {
        if (this.checked)
        {
            $('.policyNumber').prop('disabled', false);
            $('.crop').prop('disabled', false);
            $('.insurer').prop('disabled', false);
            $('.insuranceType').prop('disabled', false);
        } else {
            $('.policyNumber').prop('disabled', true);
            $('.crop').prop('disabled', true);
            $('.insurer').prop('disabled', true);
            $('.insuranceType').prop('disabled', true);
        }
    })


})($);