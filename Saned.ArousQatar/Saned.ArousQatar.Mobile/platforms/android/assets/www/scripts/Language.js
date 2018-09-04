var language = {
    changeControlText: function (options,callBack) {
        var language = options.Language;
        var value = options.value;

        GetAllControls(language, value, function (result) {
            callBack(result);
        });
    }
};

function GetAllControls(lang, value, callBack) {
    var langDict = [];
    langDict.push(
            { valueAR: 'اسم الدخول', valueEN: 'User name' },
            { valueAR: 'كلمة المرور', valueEN: 'Password' },
            { valueAR: 'نسيت كلمة المرور', valueEN: 'Forget password' },
            { valueAR: 'تسجيل الدخول', valueEN: 'Sign in' },
            { valueAR: 'لست مسجلاً لدينا ؟', valueEN: 'Not registered yet ?' },
            { valueAR: 'حساب جديد', valueEN: 'New account' },
            { valueAR: 'تخطى', valueEN: 'Pass' });

    var control = langDict.filter(function (obj) {
        return obj.valueAR.toLowerCase() === value.toLowerCase() || obj.valueEN.toLowerCase() === value.toLowerCase();
    })[0];

    if (typeof control != 'undefined' || control != null) {
        if (lang === 'EN') {
            callBack(control.valueEN);
        }
        else {
            callBack(control.valueAR);
        }
    }
    else {
        callBack('none');
    }
}