var dict = [];
var fieldsAllowedLengths = [];
var fieldsCompare = [];
var fieldsFormat = [];
var allFieldsAreCorrect = [];
var fieldsErrors = [];

var FValidation = {
    ValidateAll: function (formName,callback) {
        dict = [];
        fieldsAllowedLengths = [];
        fieldsCompare = [];
        fieldsFormat = [];
        allFieldsAreCorrect = [];
        fieldsErrors = [];


        FillArrays(formName, function (arraysAreFilled) {
            if (arraysAreFilled == true) {
                CheckFields(dict, fieldsAllowedLengths, function (errors) {
                    //$('.lblError').css('display', 'none');
                    $('.lblError').removeClass('activeError');
                    $('.lblError').removeClass('slideInDown');

                    var allFieldsValid = true;
                    if (errors != null && errors.length > 0) {
                        var allErrors = errors;

                        for (var i = 0; i < dict.length; i++) {
                            var fieldError = allErrors.filter(function (obj) {
                                return obj.field === dict[i].key;
                            })[0];

                            var errorsFound = fieldError.errors.split(',');

                            if (errorsFound.length > 0) {
                                for (var e = 0; e < errorsFound.length; e++) {
                                    var error = errorsFound[e];
                                    if (error != '') {
                                        //$('#lbl' + fieldError.field + error).css('display', '');
                                        $('#lbl' + fieldError.field + error).addClass('activeError');
                                        $('#lbl' + fieldError.field + error).addClass('slideInDown');
                                        var fieldCorrect = allFieldsAreCorrect.filter(function (obj) {
                                            return obj.Field === dict[i].key;
                                        })[0];
                                        fieldCorrect.IsCorrect = false;
                                    }
                                    else {
                                        //$('#lbl' + fieldError.field + 'Required').css('display', 'none');
                                        //$('#lbl' + fieldError.field + 'NumberOnly').css('display', 'none');
                                        //$('#lbl' + fieldError.field + 'AllowedLength').css('display', 'none');
                                        //$('#lbl' + fieldError.field + 'Format').css('display', 'none');
                                        //$('#lbl' + fieldError.field + 'Compare').css('display', 'none');
                                        $('#lbl' + fieldError.field + 'Required').removeClass('activeError');
                                        $('#lbl' + fieldError.field + 'NumberOnly').removeClass('activeError');
                                        $('#lbl' + fieldError.field + 'AllowedLength').removeClass('activeError');
                                        $('#lbl' + fieldError.field + 'Format').removeClass('activeError');
                                        $('#lbl' + fieldError.field + 'Compare').removeClass('activeError');
                                        $('#lbl' + fieldError.field + 'Required').removeClass('slideInDown');
                                        $('#lbl' + fieldError.field + 'NumberOnly').removeClass('slideInDown');
                                        $('#lbl' + fieldError.field + 'AllowedLength').removeClass('slideInDown');
                                        $('#lbl' + fieldError.field + 'Format').removeClass('slideInDown');
                                        $('#lbl' + fieldError.field + 'Compare').removeClass('slideInDown');
                                        var fieldCorrect = allFieldsAreCorrect.filter(function (obj) {
                                            return obj.Field === dict[i].key;
                                        })[0];
                                        fieldCorrect.IsCorrect = true;
                                    }
                                }
                            }
                            else {
                                //$('#lbl' + fieldError.field + 'Required').css('display', 'none');
                                //$('#lbl' + fieldError.field + 'NumberOnly').css('display', 'none');
                                //$('#lbl' + fieldError.field + 'AllowedLength').css('display', 'none');
                                //$('#lbl' + fieldError.field + 'Format').css('display', 'none');
                                //$('#lbl' + fieldError.field + 'Compare').css('display', 'none');

                                $('#lbl' + fieldError.field + 'Required').removeClass('activeError');
                                $('#lbl' + fieldError.field + 'NumberOnly').removeClass('activeError');
                                $('#lbl' + fieldError.field + 'AllowedLength').removeClass('activeError');
                                $('#lbl' + fieldError.field + 'Format').removeClass('activeError');
                                $('#lbl' + fieldError.field + 'Compare').removeClass('activeError');
                                $('#lbl' + fieldError.field + 'Required').removeClass('slideInDown');
                                $('#lbl' + fieldError.field + 'NumberOnly').removeClass('slideInDown');
                                $('#lbl' + fieldError.field + 'AllowedLength').removeClass('slideInDown');
                                $('#lbl' + fieldError.field + 'Format').removeClass('slideInDown');
                                $('#lbl' + fieldError.field + 'Compare').removeClass('slideInDown');
                                var fieldCorrect = allFieldsAreCorrect.filter(function (obj) {
                                    return obj.Field === dict[i].key;
                                })[0];
                                fieldCorrect.IsCorrect = true;
                            }

                        }
                    }

                    for (var fc = 0; fc < allFieldsAreCorrect.length; fc++) {
                        if (allFieldsAreCorrect[fc].IsCorrect == false) {
                            allFieldsValid = false;
                            break;
                        }
                    }
                    callback(allFieldsValid);
                });
            }
        });



    }
};


function CheckFields(fieldsValues, fieldsAllowedLengths, callbackError) {
    if (fieldsValues.length > 0) {
        for (var i = 0; i < fieldsValues.length; i++) {
            var fieldName = fieldsValues[i].key;
            var fieldValue = fieldsValues[i].value;

            var allFieldErros = fieldsErrors.filter(function (obj) {
                return obj.field === fieldName;
            })[0];

            if (fieldValue == null || typeof fieldValue == 'undefined' || fieldValue == '') {
                if (allFieldErros.errors == "") {
                    allFieldErros.errors = "Required";
                }
                else {
                    allFieldErros.errors += ",Required";
                }
            }
            else {
                var fieldAllowedLength = fieldsAllowedLengths.filter(function (obj) {
                    return obj.field === fieldName;
                });

                if (fieldAllowedLength.length > 0) {
                    if (fieldName == fieldAllowedLength[0].field) {
                        if (fieldAllowedLength[0].ValidationOnCount == true) {
                            if (fieldAllowedLength[0].CheckEquality == true) {
                                if (fieldValue.length != fieldAllowedLength[0].allowedLength) {
                                    if (allFieldErros.errors == "") {
                                        allFieldErros.errors = "AllowedLength";
                                    }
                                    else {
                                        allFieldErros.errors += ",AllowedLength";
                                    }
                                }
                            }
                            else {
                                if (fieldValue.length < fieldAllowedLength[0].allowedLength) {
                                    if (allFieldErros.errors == "") {
                                        allFieldErros.errors = "AllowedLength";
                                    }
                                    else {
                                        allFieldErros.errors += ",AllowedLength";
                                    }
                                }
                            }
                        }
                        else {
                            if (parseInt(fieldValue) > parseInt(fieldAllowedLength[0].allowedLength)) {
                                if (allFieldErros.errors == "") {
                                    allFieldErros.errors = "AllowedLength";
                                }
                                else {
                                    allFieldErros.errors += ",AllowedLength";
                                }
                            }
                        }

                    }

                    if (fieldName.toLowerCase().indexOf('password') == -1) {
                        if (isNaN(fieldValue) || !isFinite(fieldValue)) {
                            if (allFieldErros.errors == "") {
                                allFieldErros.errors = "NumberOnly";
                            }
                            else {
                                allFieldErros.errors += ",NumberOnly";
                            }
                        }
                    }

                }

                var fieldFormat = fieldsFormat.filter(function (obj) {
                    return obj.field === fieldName;
                });

                if (fieldFormat.length > 0) {
                    var re = '';
                    if (fieldFormat[0].type == 'Email') {
                        re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                    }
                    if (fieldFormat[0].type == 'Name') {
                        re = /^[A-Za-z0-9]*$/;
                    }
                    if (!re.test(fieldValue)) {
                        if (allFieldErros.errors == "") {
                            allFieldErros.errors = "Format";
                        }
                        else {
                            allFieldErros.errors += ",Format";
                        }
                    }
                }

                if (fieldName.toLowerCase().indexOf('confirmpassword') > -1 || fieldName.toLowerCase().indexOf('confirmnew') > -1) {
                    var fieldToCompare;
                    var fieldToCompare;

                    //{ fieldToCompare: "ChangeNewPassword", fieldToCompareWith: "ChangeConfirmNewPassword" });

                    if (fieldName.indexOf('reset') > -1) {
                        fieldToCompare = fieldsValues.filter(function (obj) {
                            return obj.key === 'resetPassword';
                        })[0].value;

                        fieldToCompareWith = fieldsValues.filter(function (obj) {
                            return obj.key === 'resetConfirmPassword';
                        })[0].value;
                    }
                    else if (fieldName.indexOf('Change') > -1) {
                        fieldToCompare = fieldsValues.filter(function (obj) {
                            return obj.key === 'ChangeNewPassword';
                        })[0].value;

                        fieldToCompareWith = fieldsValues.filter(function (obj) {
                            return obj.key === 'ChangeConfirmNewPassword';
                        })[0].value;
                    }
                    else {
                        fieldToCompare = fieldsValues.filter(function (obj) {
                            return obj.key === 'Password';
                        })[0].value;

                        fieldToCompareWith = fieldsValues.filter(function (obj) {
                            return obj.key === 'ConfirmPassword';
                        })[0].value;
                    }

                    if (fieldToCompare != fieldToCompareWith) {
                        if (allFieldErros.errors == "") {
                            allFieldErros.errors = "Compare";
                        }
                        else {
                            allFieldErros.errors += ",Compare";
                        }
                    }
                }

            }
        }

        callbackError(fieldsErrors);
    }

}

function FillArrays(formName, arraysAreFilled) {
    if (formName == 'signup') {
        allFieldsAreCorrect = [
            { Field: "Name", IsCorrect: false },
            { Field: "Mobile", IsCorrect: false },
            { Field: "FullName", IsCorrect: false },
            { Field: "Email", IsCorrect: false },
            { Field: "Password", IsCorrect: false },
            { Field: "ConfirmPassword", IsCorrect: false }];

        fieldsErrors.push(
            { field: "Name", errors: "" },
            { field: "Mobile", errors: "" },
            { field: "FullName", errors: "" },
            { field: "Email", errors: "" },
            { field: "Password", errors: "" },
            { field: "ConfirmPassword", errors: "" });

        dict.push(
            { key: "Name", value: $('#txtName').val().trim() },
            { key: "Mobile", value: $('#txtMobile').val() },
            { key: "FullName", value: $('#txtFullName').val().trim() },
            { key: "Email", value: $('#txtEmail').val().trim() },
            { key: "Password", value: $('#txtPassword').val() },
            { key: "ConfirmPassword", value: $('#txtConfirmPassword').val() });

        fieldsAllowedLengths.push(
            { field: "Mobile", allowedLength: 8, ValidationOnCount: true, CheckEquality: true },
            { field: "Password", allowedLength: 6, ValidationOnCount: true, CheckEquality: false });

        fieldsCompare.push(
            { fieldToCompare: "Password", fieldToCompareWith: "ConfirmPassword" });

        fieldsFormat.push(
            { field: "Email", type: 'Email' },
            { field: "Name", type: 'Name' });

        arraysAreFilled(true);
    }
    else if (formName == 'editProfile') {
        allFieldsAreCorrect = [
            { Field: "FullName", IsCorrect: false },
            { Field: "Mobile", IsCorrect: false }];

        fieldsErrors.push(
            { field: "FullName", errors: "" },
            { field: "Mobile", errors: "" });

        dict.push(
            { key: "FullName", value: $('#txtFullName').val().trim() },
            { key: "Mobile", value: $('#txtMobile').val() });

        fieldsAllowedLengths.push(
            { field: "Mobile", allowedLength: 8, ValidationOnCount: true, CheckEquality: true });

        arraysAreFilled(true);
    }
    else if (formName == 'changePassword') {
        allFieldsAreCorrect = [
           { Field: "ChangeOldPassword", IsCorrect: false },
           { Field: "ChangeNewPassword", IsCorrect: false },
           { Field: "ChangeConfirmNewPassword", IsCorrect: false }];

        fieldsErrors.push(
            { field: "ChangeOldPassword", errors: "" },
            { field: "ChangeNewPassword", errors: "" },
            { field: "ChangeConfirmNewPassword", errors: "" });

        dict.push(
            { key: "ChangeOldPassword", value: $('#txtChangeOldPassword').val() },
            { key: "ChangeNewPassword", value: $('#txtChangeNewPassword').val() },
            { key: "ChangeConfirmNewPassword", value: $('#txtChangeConfirmNewPassword').val() });

        fieldsAllowedLengths.push(
            { field: "ChangeNewPassword", allowedLength: 6, ValidationOnCount: true, CheckEquality: false });

        fieldsCompare.push(
            { fieldToCompare: "ChangeNewPassword", fieldToCompareWith: "ChangeConfirmNewPassword" });

        //fieldsFormat.push(
        //    { field: "Email", type: 'Email' });

        arraysAreFilled(true);
    }
    else if (formName == 'forgetPassword') {
        allFieldsAreCorrect = [
           { Field: "forgetPasswordEmail", IsCorrect: false }];

        fieldsErrors.push(
            { field: "forgetPasswordEmail", errors: "" });

        dict.push(
            { key: "forgetPasswordEmail", value: $('#txtForgetPasswordEmail').val().trim() });

        //fieldsAllowedLengths.push(
        //    { field: "ChangeNewPassword", allowedLength: 6, ValidationOnCount: true, CheckEquality: false });

        //fieldsCompare.push(
        //    { fieldToCompare: "ChangeNewPassword", fieldToCompareWith: "ChangeConfirmNewPassword" });

        fieldsFormat.push(
            { field: "forgetPasswordEmail", type: 'Email' });

        arraysAreFilled(true);
    }
    else if (formName == 'resetPassword') {
        allFieldsAreCorrect = [
           { Field: "resetCode", IsCorrect: false },
           { Field: "resetPassword", IsCorrect: false },
           { Field: "resetConfirmPassword", IsCorrect: false }];

        fieldsErrors.push(
            { field: "resetCode", errors: "" },
            { field: "resetPassword", errors: "" },
            { field: "resetConfirmPassword", errors: "" });

        dict.push(
            { key: "resetCode", value: $('#txtResetCode').val().trim() },
            { key: "resetPassword", value: $('#txtResetPassword').val() },
            { key: "resetConfirmPassword", value: $('#txtResetConfirmPassword').val() });

        fieldsAllowedLengths.push(
            { field: "resetPassword", allowedLength: 6, ValidationOnCount: true, CheckEquality: false });

        fieldsCompare.push(
            { fieldToCompare: "resetPassword", fieldToCompareWith: "resetConfirmPassword" });

        //fieldsFormat.push(
        //    { field: "Email", type: 'Email' });

        arraysAreFilled(true);
    }
    else if (formName == 'contact') {
        allFieldsAreCorrect = [
            { Field: "ContactName", IsCorrect: false },
            { Field: "ContactMobile", IsCorrect: false },
            { Field: "ContactEmail", IsCorrect: false },
            { Field: "ContactMessage", IsCorrect: false }];

        fieldsErrors.push(
            { field: "ContactName", errors: "" },
            { field: "ContactMobile", errors: "" },
            { field: "ContactEmail", errors: "" },
            { field: "ContactMessage", errors: "" });

        dict.push(
            { key: "ContactName", value: $('#txtContactName').val().trim() },
            { key: "ContactMobile", value: $('#txtContactMobile').val() },
            { key: "ContactEmail", value: $('#txtContactEmail').val().trim() },
            { key: "ContactMessage", value: $('#txtContactMessage').val().trim() });

        fieldsAllowedLengths.push(
            { field: "ContactMobile", allowedLength: 8, ValidationOnCount: true, CheckEquality: true });

        //fieldsCompare.push(
        //    { fieldToCompare: "Password", fieldToCompareWith: "ConfirmPassword" });

        fieldsFormat.push(
            { field: "ContactEmail", type: 'Email' });

        arraysAreFilled(true);
    }
    else if (formName == 'addProduct') {
        var isSpecial = $('input[name=my-radio]:checked').val();
        if (parseInt(isSpecial) == 0) {
            allFieldsAreCorrect = [
            { Field: "AddProductName", IsCorrect: false },
            { Field: "AddProductCategory", IsCorrect: false },
            { Field: "AddProductImage", IsCorrect: false },
            { Field: "AddProductDescription", IsCorrect: false }];

            fieldsErrors.push(
                { field: "AddProductName", errors: "" },
                { field: "AddProductCategory", errors: "" },
                { field: "AddProductImage", errors: "" },
                { field: "AddProductDescription", errors: "" });

            var image = '';
            if (localStorage.getItem('PPhoto')) {
                image = localStorage.getItem('PPhoto');
            }

            dict.push(
                { key: "AddProductName", value: $('#txtAddProductName').val().trim() },
                { key: "AddProductCategory", value: $('#selectAddProductCategory option:selected').val() },
                { key: "AddProductImage", value: image },
                { key: "AddProductDescription", value: $('#txtAddProductDescription').val().trim() });
        }
        else {
            allFieldsAreCorrect = [
             { Field: "AddProductName", IsCorrect: false },
             { Field: "AddProductDuration", IsCorrect: false },
             { Field: "AddProductPrice", IsCorrect: false },
             { Field: "AddProductCategory", IsCorrect: false },
             { Field: "AddProductImage", IsCorrect: false },
             { Field: "AddProductDescription", IsCorrect: false }];

            fieldsErrors.push(
                { field: "AddProductName", errors: "" },
                { field: "AddProductDuration", errors: "" },
                { field: "AddProductPrice", errors: "" },
                { field: "AddProductCategory", errors: "" },
                { field: "AddProductImage", errors: "" },
                { field: "AddProductDescription", errors: "" });

            var image = '';
            if (localStorage.getItem('PPhoto')) {
                image = localStorage.getItem('PPhoto');
            }

            dict.push(
                { key: "AddProductName", value: $('#txtAddProductName').val().trim() },
                { key: "AddProductDuration", value: $('#selectAddProductDuration option:selected').val() },
                { key: "AddProductPrice", value: $('#txtAddProductPrice').val() },
                { key: "AddProductCategory", value: $('#selectAddProductCategory option:selected').val() },
                { key: "AddProductImage", value: image },
                { key: "AddProductDescription", value: $('#txtAddProductDescription').val().trim() });
        }

        
        //fieldsAllowedLengths.push(
        //    { field: "AddProductPrice", allowedLength: 10, ValidationOnCount: true, CheckEquality: true });

        //fieldsCompare.push(
        //    { fieldToCompare: "Password", fieldToCompareWith: "ConfirmPassword" });

        //fieldsFormat.push(
        //    { field: "ContactEmail", type: 'Email' });

        arraysAreFilled(true);
    }
    else if (formName == 'editProduct') {
        var isSpecial = $('input[name=my-Edit-radio]:checked').val();
        if (parseInt(isSpecial) == 0) {
            allFieldsAreCorrect = [
            { Field: "EditProductName", IsCorrect: false },
            { Field: "EditProductCategory", IsCorrect: false },
            { Field: "EditProductImage", IsCorrect: false },
            { Field: "EditProductDescription", IsCorrect: false }];

            fieldsErrors.push(
                { field: "EditProductName", errors: "" },
                { field: "EditProductCategory", errors: "" },
                { field: "EditProductImage", errors: "" },
                { field: "EditProductDescription", errors: "" });

            var image = '';
            if (localStorage.getItem('PPhoto')) {
                image = localStorage.getItem('PPhoto');
            }

            dict.push(
                { key: "EditProductName", value: $('#txtEditProductName').val().trim() },
                { key: "EditProductCategory", value: $('#selectEditProductCategory option:selected').val() },
                { key: "EditProductImage", value: image },
                { key: "EditProductDescription", value: $('#txtEditProductDescription').val().trim() });
        }
        else {
            allFieldsAreCorrect = [
             { Field: "EditProductName", IsCorrect: false },
             { Field: "EditProductDuration", IsCorrect: false },
             { Field: "EditProductPrice", IsCorrect: false },
             { Field: "EditProductCategory", IsCorrect: false },
             { Field: "EditProductImage", IsCorrect: false },
             { Field: "EditProductDescription", IsCorrect: false }];

            fieldsErrors.push(
                { field: "EditProductName", errors: "" },
                { field: "EditProductDuration", errors: "" },
                { field: "EditProductPrice", errors: "" },
                { field: "EditProductCategory", errors: "" },
                { field: "EditProductImage", errors: "" },
                { field: "EditProductDescription", errors: "" });

            var image = '';
            if (localStorage.getItem('PPhoto')) {
                image = localStorage.getItem('PPhoto');
            }

            dict.push(
                { key: "EditProductName", value: $('#txtEditProductName').val().trim() },
                { key: "EditProductDuration", value: $('#selectEditProductDuration option:selected').val() },
                { key: "EditProductPrice", value: $('#txtEditProductPrice').val() },
                { key: "EditProductCategory", value: $('#selectEditProductCategory option:selected').val() },
                { key: "EditProductImage", value: image },
                { key: "EditProductDescription", value: $('#txtEditProductDescription').val().trim() });
        }


        //fieldsAllowedLengths.push(
        //    { field: "AddProductPrice", allowedLength: 10, ValidationOnCount: true, CheckEquality: true });

        //fieldsCompare.push(
        //    { fieldToCompare: "Password", fieldToCompareWith: "ConfirmPassword" });

        //fieldsFormat.push(
        //    { field: "ContactEmail", type: 'Email' });

        arraysAreFilled(true);
    }
}
