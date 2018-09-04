var accessToken;
var UserData = null;

function ShowLoader(pageName) {
    var divPage = document.getElementById(pageName + 'Page');
    var divLoader = document.createElement('div');
    var divCube = document.createElement('div');
    var divCubeP1 = document.createElement('div');
    var divCubeP2 = document.createElement('div');
    var divCubeP3 = document.createElement('div');
    var divCubeP4 = document.createElement('div');
    var hdrWait = document.createElement('h3');

    divLoader.className += 'loader divLoader';
    divCube.className += 'cssload-thecube';
    divCubeP1.className += 'cssload-cube cssload-c1';
    divCubeP2.className += 'cssload-cube cssload-c2';
    divCubeP3.className += 'cssload-cube cssload-c3';
    divCubeP4.className += 'cssload-cube cssload-c4';
    hdrWait.innerHTML = 'برجاء الإنتظار';

    divCube.appendChild(divCubeP1);
    divCube.appendChild(divCubeP2);
    divCube.appendChild(divCubeP3);
    divCube.appendChild(divCubeP4);
    divLoader.appendChild(divCube);
    divLoader.appendChild(hdrWait);
    divPage.appendChild(divLoader);
    $(".loader").fadeIn("slow");
}

function HideLoader() {
    $(".loader").fadeOut("slow");
    $('div').each(function () {
        var div = $(this);
        if ($(this).hasClass('loader divLoader')) {
            $(this).remove();
        }
    });
}

var googleapi = {
    authorize: function (options) {
        var deferred = $.Deferred();
        var authUrl = 'https://accounts.google.com/o/oauth2/auth?' + $.param({
            client_id: options.client_id,
            redirect_uri: options.redirect_uri,
            response_type: 'code',
            scope: options.scope
        });

        var authWindow = window.open(authUrl, '_blank', 'location=no,toolbar=no');

        $(authWindow).on('loadstart', function (e) {
            var url = e.originalEvent.url;
            var code = /\?code=(.+)$/.exec(url);
            var error = /\?error=(.+)$/.exec(url);

            if (code || error) {
                authWindow.close();
            }

            if (code) {
                var x = code[1];

                $.post('https://accounts.google.com/o/oauth2/token', {
                    code: code[1],
                    client_id: options.client_id,
                    client_secret: options.client_secret,
                    redirect_uri: options.redirect_uri,
                    grant_type: 'authorization_code'
                }).done(function (data) {
                    deferred.resolve(data);
                }).fail(function (response) {
                    console.log(response.responseJSON);
                });
            } else if (error) {
                //The user denied access to the app
                deferred.reject({
                    error: error[1]
                });
            }
        });

        return deferred.promise();
    }
};

function CallService1(pageName, CallType, MethodName, dataVariables, callback) {
    if (MethodName != 'api/home' && pageName != 'SideMenu') {
        ShowLoader(pageName);
    }
    var token = localStorage.getItem('appToken');
    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": "application/x-www-form-urlencoded",
            "cache-control": "no-cache",
            "authorization": "bearer " + token
        },
        data: dataVariables,
        async: true,
        crossDomain: true,
        success: function (result) {

        },
        error: function (error) {

        }
    }).done(function (result) {
        var res = result;
        if (MethodName != 'api/home' && pageName != 'SideMenu') {
            HideLoader();
        }
        callback(res);
    })
        .fail(function (error, textStatus) {
            if (MethodName != 'api/home' && pageName != 'SideMenu') {
                HideLoader();
            }
            var pp = pageName;
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");
            if (typeof error.responseText != 'undefined') {
                var responseText = JSON.parse(error.responseText);

                if (error.status === 401) {

                    myApp.alert('مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', 'خطأ', function () {
                        RefreshToken(pageName, CallType, 'Token', function (result) {
                            localStorage.setItem('appToken', result.access_token);
                            localStorage.setItem('refreshToken', result.refresh_token);
                            myApp.alert('تم تنشيط رمز التحقق الخاص بك , يرجي تسجيل دخولك مرة أخري  .', 'نجاح', function () {
                                localStorage.removeItem('USName');
                                localStorage.removeItem('userLoggedIn');
                                localStorage.removeItem('Visitor');
                                localStorage.removeItem('loginUsingSocial');
                                mainView.router.loadPage({ pageName: 'login' });
                            });
                        });
                    });
                }
                else {

                    if (typeof responseText.error_description != 'undefined') {
                        var error_description = responseText.error_description;
                        if (error_description === 'The user name or password is incorrect.') {
                            myApp.alert('خطأ في البريد الإليكتروني أو كلمة المرور .', 'خطأ', function () { });
                        }
                        else if (error_description === '1-User are Arhieve') {
                            myApp.alert('تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', 'خطأ', function () { });
                        }
                        else if (error_description === '2-Email Need To Confirm') {
                            myApp.alert('حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإليكتروني .', 'خطأ', function () {
                                localStorage.setItem('UserEntersCode', 'false');
                                mainView.router.loadPage({ pageName: 'userCode' });
                            });
                        }
                        else if (error_description === '3-Wating To Approve') {
                            myApp.alert('يجب تأكيدك من خلال المندوب .', 'خطأ', function () { });
                        }
                        else {
                            myApp.alert('يوجد خطأ في عملية التسجيل .', 'خطأ', function () { });
                        }
                    }
                    else if (typeof responseText.message != 'undefined' && responseText.message != "The request is invalid.") {
                        var message = responseText.message;
                        myApp.alert(message, 'خطأ', function () { });
                    }
                    else if (typeof responseText.modelState != 'undefined') {
                        var messages = responseText.modelState[""];
                        var message = "";
                        if (messages.length > 0) {
                            if (messages[0] === 'Incorrect password.') {
                                myApp.alert('كلمة السر القديمة غير صحيحة .', 'خطأ', function () { });
                            }
                            else {
                                for (var m = 0; m < messages.length; m++) {
                                    message += "- [" + messages[m] + " ] - ";
                                }

                                myApp.alert(message, 'خطأ', function () { });
                            }
                        }
                        else {
                            callback(null);
                        }

                    }
                    else {
                        callback(null);
                    }
                }
            }
            else {
                myApp.alert('خطأ في الخدمة .', 'خطأ', function () { });
                callback(null);
            }
        });
}


function callGoogle() {
    googleapi.authorize({
        client_id: '11459241082-e98ko6sbncbv5ij8k6untl7503hb69jt.apps.googleusercontent.com',
        client_secret: 'QfmDtCqrTSTJgl1kPHGUXAP5',
        redirect_uri: 'http://localhost',
        scope: 'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile'
    }).done(function (data) {
        accessToken = data.access_token;
        localStorage.setItem('ACST', accessToken);
        getDataProfile(function (res) {
            return res;
        });
        
    });
}

function getDataProfile(callBack) {
    var term = null;
    var token = localStorage.getItem('ACST');

    var accessToken = token;

   var grantUrl = 'https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=' + accessToken;

    $.ajax({
        type: 'GET',
        url: grantUrl,
        async: false,
        contentType: "application/json",
        dataType: 'jsonp',
        success: function (nullResponse) {
            var response = nullResponse;
            var userFullName = '';
            localStorage.setItem('socialEmail', nullResponse.email);

            var userInfoUrl = 'https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=' + accessToken;
            $.ajax({
                type: 'GET',
                url: userInfoUrl,
                async: false,
                contentType: "application/json",
                dataType: 'jsonp',
                success: function (userInfo) {
                    var result = userInfo;
                    userFullName = userInfo.name;
                    var registerObj = {};
                    if (localStorage.getItem('GoogleObject')) {
                        registerObj = JSON.parse(localStorage.getItem('GoogleObject'));
                    }
                    else {
                        registerObj = {
                            "Provider": "Google",
                            "userId": response.user_id,
                            "name": userFullName,
                            "ExternalAccessToken": accessToken
                        };
                    }

                    CallService1('login', "POST", "api/Account/RegisterExternal", registerObj , function (res) {
                        if (res != null) {
                            localStorage.setItem('USName', res.userName);
                            localStorage.setItem('appToken', res.access_token);
                            localStorage.setItem('Visitor', false);
                            localStorage.setItem('loginUsingSocial', true);
                            localStorage.setItem('GoogleObject', JSON.stringify(registerObj));
                            CallService('login', "POST", "api/User/GetUserInfo", '', function (res1) {
                                if (res1 != null) {
                                    res1.userName = res1.name;
                                    localStorage.setItem('userLoggedIn', JSON.stringify(res1));
                                    if (typeof res1.photoUrl != 'undefined' && res1.photoUrl != null && res1.photoUrl != '' && res1.photoUrl != ' ') {
                                        localStorage.setItem('usrPhoto', res1.photoUrl);
                                    }
                                    else {
                                        localStorage.removeItem('usrPhoto');
                                    }

                                    mainView.router.loadPage({ pageName: 'categories' });

                                }
                            });
                            //mainView.router.loadPage({ pageName: 'categories' });
                        }
                    });
                },
                fail: function (error) {
                    console.log(error);
                }
            });
                

           
        },
        error: function (e) {
            //Handle the error
            console.log(e);
        }
    });

   

    disconnectUser();
}

function disconnectUser() {
    var token = localStorage.getItem('ACST');
    var revokeUrl = 'https://accounts.google.com/o/oauth2/revoke?token=' + token;

    $.ajax({
        type: 'GET',
        url: revokeUrl,
        async: false,
        contentType: "application/json",
        dataType: 'jsonp',
        success: function (nullResponse) {
            accessToken = null;
            //console.log(JSON.stringify(nullResponse));
            //console.log("-----signed out..!!----" + accessToken);
        },
        error: function (e) {
            //Handle the error
            console.log(e);
        }
    });
}