/// <reference path="../js/jquery-2.1.0.js" />
/// <reference path="../js/framework7.min.js" />

//myApp.onPageInit('pagename', function (page) {
//    var s = document.getElementsByTagName('script')[0];
//    var sc = document.createElement('script');
//    sc.type = 'text/javascript'; sc.async = false;
//    sc.src = 'path/to/javascript.js'; s.parentNode.insertBefore(sc, s);
//});



var myApp = new Framework7({
    pushState: true,
    swipeBackPage: false,
    swipePanel: false,
    panelsCloseByOutside: true,
    init: false,
    animateNavBackIcon: true,
    modalButtonOk: 'تم',
    modalButtonCancel: 'إلغاء'
});

var $$ = Dom7;
var lang = 'En';
var serviceURL = 'http://arousqatar.saned-projects.com/';
//var hostUrl = 'http://testsaned.webwayeg.com/www/';
var hostUrl = 'http://arousqatar.saned-projects.com/';
var appToken = '';
var userId = 0;
userId = localStorage.getItem("UID");
var markers = [];
var user;
var allCities = [];
var allCategories = [];
var allDurations = [];
var BackIsClicked = false;
var scrollLoadsBefore = false;
var initSplashPage = true;
var initLoginPage = true;
var initForgetPasswordPage = true;
var initSignupPage = true;
var initActivationPage = true;
var initCategoriesPage = true;
var initProductsPage = true;
var initFavouritePage = true;
var initUserPage = true;
var initProfilePage= true;
var initAddProductPage = true;
var initEditProductPage = true;
var initChatPage = true;
var initProductDetailsPage = true;
var initContactPage = true;
var initSearch = true;
var initChangePasswordPage = true;
var initResetPasswordPage = true;
var initSideMenu = true;
var initContactProductOwner = true;
var initLikeProductPopup = true;
var initFavouriteProductPopup = true;
var initShareProductPopup = true;
var initReportProductPopup = true;
var initAllChats = true;
var initNotifications = true;
var initSearchResults = true;
var clientId = 'consoleApp';
var clientSecret = '123@abc';
var linkBackSearch = false;
var linkBackAddProduct = false;
var lastMsgSentDate = new Date();
var messageInterval;
var linkBackEditProduct = false;
var currentPage = '';
var publicImageURI = [];
var myPhotoBrowserPopupDark;
var AdvertismentCounter = 0;


var mainView = myApp.addView('.view-main', {
    dynamicNavbar: true,
    domCache: true
});

$$(document).on('pageInit', function (e) {
    page = e.detail.page;
    currentPage = page.name;

    if (page.name === 'splash') {
        setTimeout(function () {
            initSplashPage = true;
            if (localStorage.getItem('USName')) {
                if (localStorage.getItem('UserEntersCode') == "false") {
                    if (localStorage.getItem('loginUsingSocial') == 'true') {
                        mainView.router.loadPage({ pageName: 'categories' });
                    } else {
                        mainView.router.loadPage({ pageName: 'activation' });
                    }
                }
                else {
                    mainView.router.loadPage({ pageName: 'categories' });
                }
            }
            else {
                if (localStorage.getItem('UID')) {
                    mainView.router.loadPage({ pageName: 'signup' });
                }
                else {
                    GoToLoginPage(page);
                    mainView.router.loadPage({ pageName: 'login' });
                }
            }
        }, 3000);
        
    }
    else if (page.name === 'login') {
        if (initLoginPage == true) {
            initLoginPage = true;
        }
    }
    else if (page.name === 'forgetPass') {
        initForgetPassword = true;
    }
    else if (page.name === 'signup') {
        initSignupPage = true;
    }
    else if (page.name === 'activation') {
        initActivationPage = true;
    }
    else if (page.name === 'categories') {
        initCategoriesPage = true;
    }
    else if (page.name === 'products') {
        initProductsPage = true;
    }
    else if (page.name === 'favourite') {
        initFavouritePage = true;
    }
    else if (page.name === 'user') {
        initUserPage = true;
    }
    else if (page.name === 'profile') {
        initProfilePage = true;
    }
    else if (page.name === 'addProduct') {
        initAddProductPage = true;
        linkBackAddProduct = false;
    }
    else if (page.name === 'chat') {
        initChatPage = true;
    }
    else if (page.name === 'productDetails') {
        initProductDetailsPage = true;
    }
    else if (page.name === 'contact') {
        initContactPage = true;
    }
    else if (page.name === 'search') {
        initSearch = true;
        linkBackSearch = false;
    }
    else if (page.name === 'changePassword') {
        initChangePasswordPage = true;
    }
    else if (page.name === 'resetPassword') {
        initResetPasswordPage = true;
    }
    else if (page.name === 'allChats') {
        initAllChats = true;
    }
    else if (page.name === 'editProduct') {
        initEditProductPage = true;
        linkBackEditProduct = false;
    }
    else if (page.name === 'notification') {
        initNotifications = true;
    }
    else if (page.name === 'searchResults') {
        initSearchResults = true;
    }
});

$$(".map").click(function () {
    $$(".overlay").hide();
});

$$('#tab1').on('show', function () {
    $$(".filter").hide();
});

$$('#tab2').on('show', function () {
    $$(".filter").hide();
});

var calendarAddProductStart = myApp.calendar({
    input: '#addProductStartDate',
    dateFormat: 'mm-dd-yyyy',
    closeOnSelect: true,
    onChange: function (p, values, displayValues) {
        if (linkBackAddProduct == false) {
            if (values.length > 0) {
                var start = new Date();
                var end = new Date(values[0]);

                var yStart = start.getFullYear();
                var mStart = start.getMonth() + 1;
                var dStart = start.getDate();

                var yEnd = end.getFullYear();
                var mEnd = end.getMonth() + 1;
                var dEnd = end.getDate();

                var CurrentDate = [yStart, mStart, dStart];
                var dateToCompare = [yEnd, mEnd, dEnd];

                var older = CheckDateOlderThanToday(CurrentDate, dateToCompare);

                if (older == true) {
                    myApp.alert('من فضلك إختر تاريخ أكبر من تاريخ اليوم .', 'تنبيه', function () {
                        $('#addProductStartDate').val('');
                        calendarAddProductStart.close();
                        calendarAddProductStart.setValue([]);
                    });
                }
            }
        }
        else {
            linkBackAddProduct = false;
        }

    }
});

var calendarEditProductStart = myApp.calendar({
    input: '#editProductStartDate',
    dateFormat: 'mm-dd-yyyy',
    closeOnSelect: true,
    onChange: function (p, values, displayValues) {
        if (linkBackEditProduct == false) {
            if (values.length > 0) {
                var start = new Date();
                var end = new Date(values[0]);

                var yStart = start.getFullYear();
                var mStart = start.getMonth() + 1;
                var dStart = start.getDate();

                var yEnd = end.getFullYear();
                var mEnd = end.getMonth() + 1;
                var dEnd = end.getDate();

                var CurrentDate = [yStart, mStart, dStart];
                var dateToCompare = [yEnd, mEnd, dEnd];

                var older = CheckDateOlderThanToday(CurrentDate, dateToCompare);

                if (older == true) {
                    myApp.alert('من فضلك إختر تاريخ أكبر من تاريخ اليوم .', 'تنبيه', function () {
                        $('#editProductStartDate').val('');
                        calendarEditProductStart.close();
                        calendarEditProductStart.setValue([]);
                    });
                }
            }
        }
        else {
            linkBackEditProduct = false;
        }

    }
});

window.document.addEventListener('backbutton', function (event) {
    var currentPage1 = mainView.activePage.name;
    if (currentPage1 == 'splash' || currentPage1 == 'login' || currentPage1 == 'categories') {
        myApp.confirm('هل تريد الخروج من التطبيق ؟', 'تأكيد', function () {
            ExitApplication();
        });
    }
    else {
        if ($('.smart-select-picker.modal-in').length > 0) {
            myApp.closeModal('.smart-select-picker.modal-in');
        }
        if (currentPage1 == 'addProduct') {
            $('#txtAddProductPrice').val('');
            $('#txtAddProductDescription').val('');
            $('#lblProductImage').html('صور الإعلان');
            $('#addProductStartDate').val('');
            linkBackAddProduct = true;
            selectAddProductDuration.innerHTML = '';
            selectAddProductCategory.innerHTML = '';
            calendarAddProductStart.close();
            calendarAddProductStart.setValue([]);
            publicImageURI = [];
            var photos = [];
            DrawAllProductPhotos(photos, 4);
            mainView.router.back();
        }
        else if (currentPage1 == 'editProduct') {
            $('#txtEditProductPrice').val('');
            linkBackEditProduct = true;
            publicImageURI = [];
            selectEditProductDuration.innerHTML = '';
            selectEditProductCategory.innerHTML = '';
            calendarEditProductStart.close();
            calendarEditProductStart.setValue([]);
            mainView.router.back();
        }
        else if (currentPage == 'chat') {
            UpdateUserStatus(false, function (res) {
                BackIsClicked = true;
                mainView.router.back();
            });
        }
        else if (currentPage == 'productDetails') {
            if (typeof myPhotoBrowserPopupDark != 'undefined' && myPhotoBrowserPopupDark != null) {
                myPhotoBrowserPopupDark.close();
            }
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'profile') {
            localStorage.setItem('maxUserProducts', 0);
            localStorage.setItem('lastIndex', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'categories') {
            localStorage.setItem('maxCategories', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'products') {
            localStorage.setItem('maxProducts', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'favourite') {
            localStorage.setItem('maxFavorites', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'notification') {
            localStorage.setItem('maxNotifications', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'productDetails') {
            localStorage.setItem('maxComments', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'searchResults') {
            localStorage.setItem('maxSearchResults', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else if (currentPage == 'allChats') {
            localStorage.setItem('maxAllChats', 0);
            BackIsClicked = true;
            mainView.router.back();
        }
        else {
            BackIsClicked = true;
            mainView.router.back();
        }
    }
});

window.document.addEventListener('pause', function (event) {
    UpdateUserStatus(false, function (res) {
    });
});

window.document.addEventListener('resume', function (event) {
    var currentPage = mainView.activePage.name;
    if (currentPage == 'chat') {
        UpdateUserStatus(true, function (res) {
        });
    }
});

function onConfirmAr(buttonIndex) {
    if (buttonIndex == 1) {
        if (device.platform.indexOf('dr') > 0)
            navigator.app.exitApp();
    }
}

function ExitApplication() {
    if (navigator.app) {
        navigator.app.exitApp();
    }
    else if (navigator.device) {
        navigator.device.exitApp();
    }
}

function onConfirmExit(buttonIndex) {
    if (buttonIndex == 1) {
        if (device.platform.indexOf('dr') > 0) {
            ExitApplication();
        }
    }
}

function checkConnection() {
    var networkState = navigator.connection.type;
    return networkState;
}

document.addEventListener("offline", onOffline, false);

function onOffline() {
    // Handle the offline event
    navigator.notification.alert('انت غير متصل بالانترنت , من فضلك أعد تشغيل البرنامج بعد التأكد من الإنترنت .', function () {
        ExitApplication();
    }, 'خطأ', 'موافق');
}

function ChangeLanguage() {
    $('.lang').each(function (index) {
        var propType = this.nodeName;
        var element = $(this);
        var options = {};
        if (propType === 'INPUT') {
            var options = {
                Language: 'AR',
                value: $(this).attr('placeholder').trim()
            };
            language.changeControlText(options, function (translatedValue) {
                $(element).attr('placeholder', translatedValue);
            });
        }
        else {
            var options = {
                Language: 'AR',
                value: $(this).text().trim()
            };
            language.changeControlText(options, function (translatedValue) {
                propType = this.nodeName;
                $(element).html(translatedValue);
            });
        }
        
        
    });
}

function arabictonum(arabicnumstr) {
    var num = 0;
    var c;
    for (var i = 0; i < arabicnumstr.length; i++) {
        c = arabicnumstr.charCodeAt(i);
        num += c - 1632;
        num *= 10;
    }
    return num / 10;
}

function groupBy(collection, property) {
    var i = 0, val, index,
        values = [], result = [];
    for (; i < collection.length; i++) {
        val = collection[i][property];
        index = values.indexOf(val);
        if (index > -1)
            result[index].push(collection[i]);
        else {
            values.push(val);
            result.push([collection[i]]);
        }
    }
    return result;
}

function ClearBodyAfterGoogleMap() {
    console.log('clear');
    $('span').each(function () {
        var span = $(this);
        if ($(this).text() === 'BESbewy') {
            $(this).remove();
        }
    });

    $('div').each(function () {
        var div = $(this);
        if ($(this).hasClass('pac-container pac-logo hdpi') && $(this).children().length == 0) {
            $(this).remove();
        }
    });
}

function GetCurrentDateTime(date) {
    var currentdate = new Date();
    if (date == '') {
        currentdate = new Date();
    }
    else {
        currentdate = date;
    }

    var month = parseInt((currentdate.getMonth() + 1));
    var day = parseInt(currentdate.getDate());
    var datetime;

    if (month < 10) {
        if (day < 10) {
            datetime = currentdate.getFullYear() + "-0" + (currentdate.getMonth() + 1) + "-0" + currentdate.getDate();
            //+ " T"+ currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
        }
        else {
            datetime = currentdate.getFullYear() + "-0" + (currentdate.getMonth() + 1) + "-" + currentdate.getDate();
            //+ " T" + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
        }
    }
    else {
        if (day < 10) {
            datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1) + "-0" + currentdate.getDate();
            //+ " T" + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
        }
        else {
            datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1) + "-" + currentdate.getDate();
            //+ " T" + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
        }
    }

    return datetime;
}

function geocodeLatLng(Lat, Lang, callBack) {
    var latlng = { lat: parseFloat(Lat), lng: parseFloat(Lang) };
    var geocoder = new google.maps.Geocoder;

    geocoder.geocode({ 'location': latlng }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                var result = results[0].formatted_address;
                callBack(result);
            } else {
                callBack('');
            }
        } else {
            callBack('');
        }
    });
}

function InitMapSearchBox(map, markers, selectedAddress) {
    if (selectedAddress != '') {
        $('#pac-input').val(selectedAddress);
    }

    var geocoder = new google.maps.Geocoder();
    if (geocoder) {
        geocoder.geocode({
            'address': selectedAddress
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                    map.setCenter(results[0].geometry.location);

                    markers = [];

                    // Create a marker for each place.
                    markers.push(new google.maps.Marker({
                        map: map,
                        title: results[0].formatted_address,
                        position: results[0].geometry.location
                    }));


                    var lat = results[0].geometry.location.lat();
                    var lang = results[0].geometry.location.lng();


                    localStorage.setItem('FacilityAddressLatitude', lat);
                    localStorage.setItem('FacilityAddressLongtitude', lang);
                    localStorage.setItem('FacilityAddress', $('#pac-input').val());

                    var infoWindow = new google.maps.InfoWindow();

                    for (var i = 0; i < markers.length; i++) {
                        var data = markers[i];
                        var myLatlng = new google.maps.LatLng(data.position.lat(), data.position.lng());
                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            map: map,
                            title: data.title
                        });

                        (function (marker, data) {
                            google.maps.event.addListener(marker, "click", function (e) {
                                infoWindow.setContent("<div style = 'width:200px;min-height:40px'>" + data.title + "</div>");
                                infoWindow.open(map, marker);
                            });
                        })(marker, data);
                    }

                }
                else {
                    alert("No results found");
                }
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    }
}

function initMap(markers, fromPage, selectedAddress, callback) {
    var mapDiv;
    var flightPlanCoordinates = [];

    if (fromPage == 'addTrip') {
        mapDiv = document.getElementById('mapTrip');
    }
    else {
        mapDiv = document.getElementById('mapTransport');
    }

    

    var map = new google.maps.Map(mapDiv, {
        center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
        zoom: 10,
        mapTypeId: 'terrain'
    });

    if (markers.length > 0) {
        map = new google.maps.Map(mapDiv, {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 10,
            mapTypeId: 'terrain'
        });

        var infoWindow = new google.maps.InfoWindow();

        for (var i = 0; i < markers.length; i++) {
            var data = markers[i];
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            //flightPlanCoordinates.push(new google.maps.LatLng(data.lat, data.lng));
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title
            });

            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    infoWindow.setContent("<div style = 'width:200px;min-height:40px'>" + data.title + "</div>");
                    infoWindow.open(map, marker);
                });
            })(marker, data);
        }
    }

    callback(map);

    //var flightPath = new google.maps.Polyline({
    //    path: flightPlanCoordinates,
    //    geodesic: true,
    //    strokeColor: '#FF0000',
    //    strokeOpacity: 1.0,
    //    strokeWeight: 2
    //});

    //flightPath.setMap(map);
}

function SetUserInLocalStorage(usrName, usrPass, usrId) {
    localStorage.setItem('USName', usrName);
    localStorage.setItem('UPass', usrPass);
    localStorage.setItem('UserId', usrId);
}

function ShowLoader(pageName) {
    //var divPage = document.getElementById(pageName + 'Page');
    //var divLoader = document.createElement('div');
    //var hdrWait = document.createElement('h3');
    //var loadImage = document.createElement('img');

    //divLoader.className += 'loader divLoader';
    //loadImage.src = 'img/load.svg';

    //var lang = localStorage.getItem('lang');

    //if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
    //    hdrWait.innerHTML = 'برجاء الإنتظار';
    //}
    //else {
    //    hdrWait.innerHTML = 'Please wait';
    //}

    //divLoader.appendChild(loadImage);
    //divLoader.appendChild(hdrWait);
    //divPage.appendChild(divLoader);
    //$(".loader").fadeIn("slow");

    $('[data-page].page-on-center').block({
        css: {
            backgroundColor: '#FFF',
            border: 'none'
        },
        message: '<div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div><div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div>',
        overlayCSS: {
            backgroundColor: '#FFF',
            opacity: 1,
        }
    });

}

function HideLoader() {
    //$(".loader").fadeOut("slow");
    //$('div').each(function () {
    //    var div = $(this);
    //    if ($(this).hasClass('loader divLoader')) {
    //        $(this).remove();
    //    }
    //});
    $('[data-page]').each(function () { $(this).unblock() });
}

function RefreshToken(pageName, CallType, MethodName, callback) {
    var token = localStorage.getItem('refreshToken');
    ShowLoader(pageName);
    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": "application/x-www-form-urlencoded",
            "cache-control": "no-cache",
            "postman-token": "a7924ea4-7d97-e2f6-5b56-33cbffb586a7"
        },
        data: { 'refresh_token': token, 'grant_type': 'refresh_token', 'client_id': clientId, 'client_secret': clientSecret },
        async: true,
        crossDomain: true,
        success: function (result) {

        },
        error: function (error) {

        }
    }).done(function (result) {
        var res = result;
        HideLoader();
        callback(res);
    })
        .fail(function (error, textStatus) {
            HideLoader();
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");

            var responseText = JSON.parse(error.responseText);

            if (error.status === 401) {
                myApp.alert('لا يمكن إعادة تنشيط رمز التحقق الخاص بك لإنتهاء صلاحيته .', 'خطأ', function () {
                    localStorage.removeItem('appToken');
                    localStorage.removeItem('USName');
                    localStorage.removeItem('refreshToken');
                    localStorage.removeItem('userLoggedIn');
                    localStorage.removeItem('Visitor');
                    localStorage.removeItem('loginUsingSocial');
                    mainView.router.loadPage({ pageName: 'login' });
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
                    else {
                        myApp.alert('يوجد خطأ في عملية التسجيل .', 'خطأ', function () { });
                    }
                }
                else if (typeof responseText.message != 'undefined') {
                    var message = responseText.message;
                    callback(null);
                }

            }
        });
}

function GetToken(pageName, CallType, MethodName, userName, password, callback) {
    ShowLoader(pageName);
    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": "application/x-www-form-urlencoded",
            "cache-control": "no-cache",
            "postman-token": "a7924ea4-7d97-e2f6-5b56-33cbffb586a7"
        },
        data: { 'userName': userName, 'Password': password, 'grant_type': 'password', 'client_id': clientId, 'client_secret': clientSecret },
        async: true,
        crossDomain: true,
        success: function (result) {

        },
        error: function (error) {

        }
    }).done(function (result) {
        var res = result;
        HideLoader();
        callback(res);
    })
        .fail(function (error, textStatus) {
            HideLoader();
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");

            var responseText = JSON.parse(error.responseText);

            if (typeof responseText.error_description != 'undefined') {
                var error_description = responseText.error_description;
                if (error_description === 'The user name or password is incorrect.') {
                    myApp.alert('خطأ في اسم الدخول أو كلمة المرور .', 'خطأ', function () { });
                }
                else if (error_description === '1-User are Arhieve') {
                    myApp.alert('تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', 'خطأ', function () { });
                }
                else if (error_description === '2-Email Need To Confirm') {
                    myApp.alert('حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإليكتروني .', 'خطأ', function () {
                        localStorage.setItem('UserEntersCode', 'false');
                        mainView.router.loadPage({ pageName: 'activation' });
                    });
                }
                else {
                    myApp.alert('يوجد خطأ في عملية التسجيل .', 'خطأ', function () { });
                }
            }
            else if (typeof responseText.message != 'undefined') {
                var message = responseText.message;
                callback(null);
            }
        });
}

function CallService(pageName, CallType, MethodName, dataVariables, callback) {
    //if (MethodName != 'api/home' && MethodName != 'api/User/ChangeImage' && MethodName != 'api/chat/SaveMessage' &&
    //    MethodName != 'api/category/GetHomeCategory' && MethodName != 'api/advertisement/GetHomeAds' && MethodName != 'api/chat/GetMessage' &&
    //    MethodName != 'api/advertisement/UserAds' && MethodName != 'api/User/GetUserInfo' && MethodName != 'api/Account/RegisterExternal' &&
    //    MethodName != 'api/chat/GetChatByAdvertisment' && MethodName != 'api/chat/GetChatByUser' && MethodName != 'api/favorite/GetAll' &&
    //    MethodName != 'api/advertisement/Search' && MethodName != 'api/advertisement/GeAdsByCategoryId' && MethodName != 'api/comment/GetAll' &&
    //    pageName != 'chat1' && pageName != 'SideMenu') {
    //    ShowLoader(pageName);
    //}
    ShowLoader(pageName);
    var token = localStorage.getItem('appToken');
    var contentType = "application/json";

    if (MethodName.indexOf('api/User/GetUserInfo') > -1 || MethodName.indexOf('api/Account/ReSendConfirmationCode') > -1 ||
        MethodName.indexOf('api/User/ChangeStatus/') > -1 || MethodName.indexOf('api/advertismentTransaction/save') > -1) {
        contentType = "application/x-www-form-urlencoded";
    }

    if (dataVariables != '' && dataVariables != null && MethodName.indexOf('api/advertismentTransaction/save') == -1) {
        dataVariables = JSON.stringify(dataVariables);
    }

    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": contentType,
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
        //if (MethodName != 'api/home' && MethodName != 'api/User/ChangeImage' && MethodName != 'api/chat/SaveMessage' &&
        //    MethodName != 'api/category/GetHomeCategory' && MethodName != 'api/advertisement/GetHomeAds' && MethodName != 'api/chat/GetMessage' &&
        //    MethodName != 'api/advertisement/UserAds' && MethodName != 'api/User/GetUserInfo' && MethodName != 'api/Account/RegisterExternal' &&
        //    MethodName != 'api/chat/GetChatByAdvertisment' && MethodName != 'api/chat/GetChatByUser' && MethodName != 'api/favorite/GetAll' &&
        //    MethodName != 'api/advertisement/Search' && MethodName != 'api/advertisement/GeAdsByCategoryId' && MethodName != 'api/comment/GetAll' &&
        //    pageName != 'chat1' && pageName != 'SideMenu') {
        //    HideLoader();
        //}
        HideLoader();
        callback(res);
    })
        .fail(function (error, textStatus) {
            //if (MethodName != 'api/home' && MethodName != 'api/User/ChangeImage' && MethodName != 'api/chat/SaveMessage' &&
            //    MethodName != 'api/category/GetHomeCategory' && MethodName != 'api/advertisement/GetHomeAds' && MethodName != 'api/chat/GetMessage' &&
            //    MethodName != 'api/advertisement/UserAds' && MethodName != 'api/User/GetUserInfo' && MethodName != 'api/Account/RegisterExternal' &&
            //    MethodName != 'api/chat/GetChatByAdvertisment' && MethodName != 'api/chat/GetChatByUser' && MethodName != 'api/favorite/GetAll' &&
            //    MethodName != 'api/advertisement/Search' && MethodName != 'api/advertisement/GeAdsByCategoryId' && MethodName != 'api/comment/GetAll' &&
            //    pageName != 'chat1' && pageName != 'SideMenu') {
            //    HideLoader();
            //}
            HideLoader();
            var pp = pageName;
            console.log(JSON.stringify(dataVariables));
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");
            if (typeof error.responseText != 'undefined' && error.responseText.indexOf("<!DOCTYPE html>") == -1) {
                var responseText = JSON.parse(error.responseText);

                if (error.status === 401) {

                    myApp.alert('مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', 'خطأ', function () {
                        RefreshToken(pageName, CallType, 'Token', function (result) {
                            localStorage.setItem('appToken', result.access_token);
                            localStorage.setItem('refreshToken', result.refresh_token);
                            myApp.alert('تم تنشيط رمز التحقق الخاص بك , يرجي تسجيل دخولك مرة أخري  .', 'نجاح', function () {
                                //localStorage.removeItem('USName');
                                //localStorage.removeItem('userLoggedIn');
                                //localStorage.removeItem('Visitor');
                                //localStorage.removeItem('loginUsingSocial');
                                //mainView.router.loadPage({ pageName: 'login' });
                                mainView.router.loadPage({ pageName: 'categories' });
                            });
                        });
                    });
                }
                else if (error.status == 500) {
                    myApp.alert('خطأ في الخدمة', 'خطأ', function () {
                        callback(null);
                    });
                }
                else {
                    if (typeof responseText.error_description != 'undefined') {
                        var error_description = responseText.error_description;
                        if (error_description === 'The user name or password is incorrect.') {
                            myApp.alert('خطأ في اسم الدخول أو كلمة المرور .', 'خطأ', function () { });
                        }
                        else if (error_description === '1-User are Arhieve') {
                            myApp.alert('تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', 'خطأ', function () { });
                        }
                        else if (error_description === '2-Email Need To Confirm') {
                            myApp.alert('حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإليكتروني .', 'خطأ', function () {
                                localStorage.setItem('UserEntersCode', 'false');
                                mainView.router.loadPage({ pageName: 'activation' });
                            });
                        }
                        else {
                            myApp.alert('يوجد خطأ في عملية التسجيل .', 'خطأ', function () { });
                        }
                    }
                    else if (typeof responseText.message != 'undefined' && responseText.message != "The request is invalid.") {
                        var message = responseText.message;
                        if (MethodName == 'Api/Account/ChangePassword' && message == 'An error has occurred.') {
                            message = 'كلمة السر القديمة غير صحيحة .';
                        }
                        myApp.alert(message, 'خطأ', function () { });
                    }
                    else if (typeof responseText.modelState != 'undefined') {
                        if (typeof responseText.modelState.email != 'undefined') {
                            myApp.alert('هذا البريد الإليكتروني مستخدم من قبل .', 'خطأ', function () { });
                        }
                        else {
                            var messages = responseText.modelState[""];
                            var message = "";
                            if (messages.length > 0) {
                                if (messages[0] === 'Incorrect password.') {
                                    myApp.alert('كلمة السر القديمة غير صحيحة .', 'خطأ', function () { });
                                }
                                else if (messages[0] === "Email Must Be Unique") {
                                    myApp.alert("البريد الإلكتروني مسجل من قبل", 'خطأ', function () { });
                                }
                                else if (messages[0] === "The Password must be at least 6 characters long.") {
                                    myApp.alert("كلمة السر 6 حروف على الاقل", 'خطأ', function () { });
                                }
                                else if (messages[0] === "The password and confirmation password do not match.") {
                                    myApp.alert("لا تتطابق كلمة السر مع تأكيد كلمة السر", 'خطأ', function () { });
                                }
                                else if (messages[0].startsWith('Name') && messages[0].endsWith('is already taken.')) {
                                    myApp.alert("اسم المستخدم مسجل من قبل", 'خطأ', function () { });
                                }
                                else if (messages[0].startsWith('Phone Number') && messages[0].endsWith('is already taken.')) {
                                    myApp.alert("رقم الجوال مسجل من قبل", 'خطأ', function () { });
                                }
                                else if (messages[0].startsWith('User name') && messages[0].endsWith('can only contain letters or digits.')) {
                                    myApp.alert("اسم المستخدم يمكن ان يحتوي فقط على حروف وارقام", 'خطأ', function () { });
                                }
                                else if (messages[0].indexOf('Invalid token') > -1) {
                                    myApp.alert("كود التفعيل غير صحيح  , برجاء تفقد البريد الالكترونى", 'خطأ', function () { });
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
                        callback(null);
                    }
                }
                callback(null);
            }
            else {
                callback(null);
            }
        });
}

$$('.spanNotificationsNav').on('click', function () {
    
    mainView.router.loadPage({ pageName: 'notification' });
});


function HideAllSignupControls() {
    $('#liName').css('display', 'none');
    $('#liFullName').css('display', 'none');
    $('#liMobile').css('display', 'none');
    $('#liEmail').css('display', 'none');
    $('#liPassword').css('display', 'none');
    $('#liConfirmPassword').css('display', 'none');
    $('.lblError').removeClass('activeError');
    $('.lblError').removeClass('slideInDown');
}

function ClearAllSignupControls() {
    $('#txtFullName').val('');
    $('#txtName').val('');
    $('#txtMobile').val('');
    $('#txtEmail').val('');
    $('#txtPassword').val();
    $('#txtConfirmPassword').val('');

    $('.smart-select .item-after').html('');

    $('.lblError').removeClass('activeError');
    $('.lblError').removeClass('slideInDown');
}

function cleatTimer() {
    clearInterval(messageInterval);
    messageInterval = undefined;
}

function FillNotifications(pageName, callBack) {
    var notifications = 0;
    allUnReadNotifications = 0;

    var params = {
        "pageNumber": 1,
        "pageSize": 8
    };

    CallService(pageName, "POST", "api/Notification/GetAll", params, function (notifications) {
        if (notifications != null && notifications.length > 0) {
            allUnReadNotifications = notifications[0].notifiedCount;
        }
        callBack(allUnReadNotifications);
    });
}

function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function UpdateUserStatus(status, callBack) {
    if (localStorage.getItem('userLoggedIn')) {
        var userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));

        CallService('chat', "POST", "api/User/ChangeStatus/" + status, '', function (result) {
            if (result != null) {
                callBack(1);
            }
            else {
                language.openFrameworkModal('خطأ', 'خطأ في تغيير حالة المستخدم.', 'alert', function () {
                    callBack(0);
                });
            }
        });
    }
}

function PayOnline(AdvertisementId, pageName, callBack) {
    var url = 'http://ArousQatar.saned-projects.com/paypal/PaymentWithPaypal/' + AdvertisementId;
    var ref = cordova.InAppBrowser.open(url, '_blank', 'location=no,toolbar=no,zoom=no');
    var initPay = true;

    ref.addEventListener('loadstart', function (e) {
        var url = e.url;

        if (url.indexOf('Error') > -1 || url.indexOf('error') > -1) {
            localStorage.setItem('IsPaid', 'false');
            ref.close();
        }

        var paymentId = null;
        var token = null;
        var PayerID = null;

        if (url.indexOf('Paypal/Success/') > -1 || url.indexOf('paypal/success/') > -1) {
            paymentId = url.split('Success/')[1];
        }

        if (paymentId) {
            var params = {
                "PaymentId": paymentId,
                "AdvertismentId": AdvertisementId
            };

            CallService(pageName, "POST", "api/advertismentTransaction/save", params, function (res) {
                if (res != null && res > 0) {
                    if (initPay == true) {
                        initPay = false;

                        myApp.alert('تمت عملية الدفع بنجاح', 'نجاح', function () {
                            var pNotificationStatus = document.getElementById('pNotificationStatus');
                            pNotificationStatus.innerHTML = 'اعلان مدفوع';
                            GetBackInHistory();
                        });
                    }
                    ref.close();
                }
                else {
                    if (initPay == true) {
                        initPay = false;

                        myApp.alert('حدثت مشكلة أثناء عملية الدفع.', 'خطأ', function () {
                            var pNotificationStatus = document.getElementById('pNotificationStatus');
                            pNotificationStatus.innerHTML = 'حدثت مشكلة أثناء عملية الدفع';
                            GetBackInHistory();
                        });
                        ref.close();
                    }
                }
            });
        }
    });

    ref.addEventListener('exit', function (e) {
        GetBackInHistory();
    });
}

function GetBackInHistory() {
    for (var i = 0; i < mainView.history.length; i++) {
        if (mainView.history[i] === '#splash') mainView.history.splice(i, 1);
        if (mainView.history[i] === '#login') mainView.history.splice(i, 1);
    }
    mainView.router.back();
}

function getProductImage(callback) {
    navigator.camera.getPicture(function (imageURI) {
        localStorage.setItem('PPhoto', 'set');
        callback(imageURI);
    }, function (message) {}, {
        quality: 50,
        destinationType: navigator.camera.DestinationType.DATA_URL,
        sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
        allowEdit: true,
        encodingType: Camera.EncodingType.JPEG,
        targetWidth: 300,
        targetHeight: 300,
        popoverOptions: CameraPopoverOptions,
        saveToPhotoAlbum: false,
        correctOrientation: true
    });
}

function getEditProductImage(callback) {
    navigator.camera.getPicture(function (imageURI) {
        localStorage.setItem('PPhoto', 'set');
        callback(imageURI);
    }, function (message) {}, {
        quality: 50,
        destinationType: navigator.camera.DestinationType.DATA_URL,
        sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
        allowEdit: true,
        encodingType: Camera.EncodingType.JPEG,
        targetWidth: 300,
        targetHeight: 300,
        popoverOptions: CameraPopoverOptions,
        saveToPhotoAlbum: false,
        correctOrientation: true
    });
}

function getImage() {
    navigator.camera.getPicture(uploadPhoto, function (message) {
        
    }, {
        quality: 50,
        destinationType: navigator.camera.DestinationType.DATA_URL,
        sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
        allowEdit: true,
        encodingType: Camera.EncodingType.JPEG,
        targetWidth: 300,
        targetHeight: 300,
        popoverOptions: CameraPopoverOptions,
        saveToPhotoAlbum: false,
        correctOrientation: true
    });
}

function uploadPhoto(imageURI) {
    imgUserAccount.src = 'data:image/jpeg;base64, ' + imageURI;

    var status = document.getElementById('status');
    var imgUploading = document.getElementById('imgUploading');
    var linkAddPhoto = document.getElementById('linkAddPhoto');

    $('#linkAddPhoto').hide();
    $('#imgUploading').show();

    var params = {
        'ImageFilename': 'img.jpg',
        'ImageBase64': imageURI
    };

    CallService('profile', "POST", "api/users/saveimage", params, function (res) {
        if (res != null) {
            myApp.alert('تم رفع الصورة بنجاح .', 'نجاح', function () {
                $('#linkAddPhoto').show();
                $('#imgUploading').hide();

                var totalPhotoUrl = hostUrl + 'Uploads/' + res;
                localStorage.setItem('usrPhoto', res);
                var user = JSON.parse(localStorage.getItem('userLoggedIn'));
                user.photoUrl = res;
                localStorage.setItem('userLoggedIn', JSON.stringify(user));
            });
        }
        else {
            myApp.alert('خطأ في إضافة صورة للمستخدم.', 'خطأ', function () { });
        }
    });
}

function UserNameIsEmailOrPhone(email, mobile) {
    if (email != '' && mobile == '') {
        localStorage.setItem('userUses', 'Email');
    }
    else if (email == '' && mobile != '') {
        localStorage.setItem('userUses', 'Mobile');
    }
    else {
        localStorage.setItem('userUses', 'None');
    }
}

function GetAllCities(fromPage, callBack) {
    allCities = [];
    allCities.push({ id: 1, name: 'الرياض', icon: 'ionicons ion-android-checkmark-circle' });
    allCities.push({ id: 2, name: 'جدة', icon: 'ionicons ion-android-checkmark-circle' });
    allCities.push({ id: 3, name: 'مكة المكرمة', icon: 'ionicons ion-android-checkmark-circle' });
    allCities.push({ id: 4, name: 'المدينة المنورة', icon: 'ionicons ion-android-checkmark-circle' });
}

function GetAllCategories(fromPage, callBack) {
    allCategories = [];

    if (fromPage == 'addProduct') {
        var selectAddProductCategory = document.getElementById('selectAddProductCategory');
        var selectAddProductDuration = document.getElementById('selectAddProductDuration');
        selectAddProductCategory.innerHTML = '';
        selectAddProductDuration.innerHTML = '';
        myApp.smartSelectAddOption('#linkAddProductDuration select', '<option value="" selected disabled></option>');
        myApp.smartSelectAddOption('#linkAddProductCategory select', '<option value="" selected disabled></option>');
    }
    else if (fromPage == 'editProduct') {
        var selectEditProductCategory = document.getElementById('selectEditProductCategory');
        var selectEditProductDuration = document.getElementById('selectEditProductDuration');
        selectEditProductCategory.innerHTML = '';
        selectEditProductDuration.innerHTML = '';
        myApp.smartSelectAddOption('#linkEditProductDuration select', '<option value="" disabled></option>');
        myApp.smartSelectAddOption('#linkEditProductCategory select', '<option value="" disabled></option>');
    }

    CallService(fromPage, "GET", "api/category/GetCategories", '', function (result) {
        if (result != null) {
            for (var c = 0; c < result.length; c++) {
                allCategories.push({ id: result[c].id, name: result[c].name, iconName: result[c].iconName });
            }
            callBack(allCategories);
        }
    });
}

function GetAllDurations(fromPage, callBack) {
    allDurations = [];
    CallService(fromPage, "GET", "api/advertisementPrice/GetAll", '', function (result) {
        if (result != null) {
            for (var c = 0; c < result.length; c++) {
                allDurations.push({ id: result[c].id, price: result[c].price, period: result[c].period, overAllCount: result[c].overAllCount });
            }
            callBack(allDurations);
        }
    });
}

function GetAllMessages(photo, fromPage, chatId, pageNumber, pageSize, lastSentDate, callBack) {
    var params = {};
    if (lastSentDate === '') {
        params = {
            'ChatId': chatId,
            'PageNumber': pageNumber,
            'PageSize': pageSize
        };
    }
    else {
        params = {
            'ChatId': chatId,
            'PageNumber': pageNumber,
            'PageSize': pageSize,
            'LastSentDate': lastSentDate
        };
    }

    CallService(fromPage, "POST", "api/chat/GetMessage", params, function (result) {
        if (result != null && result.length > 0) {
            var messageIds = [];
            allMessages.forEach(function (item) {
                var val = item.id
                messageIds.push(val);
            });

            for (var m = 0; m < result.length; m++) {
                if (messageIds.indexOf(result[m].id) == -1) {
                    if (lastSentDate === '') {
                        allMessages.splice(0, 0, { id: result[m].id, chatId: result[m].chatId, senderId: result[m].senderId, messageContent: result[m].messageContent, sentDate: result[m].sentDate, senderPhoto: photo });
                    }
                    else {
                        allMessages.push({ id: result[m].id, chatId: result[m].chatId, senderId: result[m].senderId, messageContent: result[m].messageContent, sentDate: result[m].sentDate, senderPhoto: photo });
                    }

                    if (m == 0) {
                        lastMsgSentDate = result[m].sentDate;
                    }
                }
            }
            callBack(allMessages);
        }
        else {
            callBack(allMessages);
        }
    });
}

function GetCommentDuration(date1, date2) {
    //Get 1 day in milliseconds
    var one_day = 1000 * 60 * 60 * 24;

    var reqDate = new Date(date1.split('-')[0], (parseInt(date1.split('-')[1]) - 1), date1.split('-')[2]);

    // Convert both dates to milliseconds
    var date1_ms = reqDate.getTime();
    var date2_ms = date2.getTime();

    // Calculate the difference in milliseconds
    var difference_ms = date2_ms - date1_ms;
    //take out milliseconds
    difference_ms = difference_ms / 1000;
    var seconds = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    var minutes = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    var hours = Math.floor(difference_ms % 24);
    var days = Math.floor(difference_ms / 24);

    //return 'Since : ' + days + ' d, ' + hours + ' h, ' + minutes + ' m, and ' + seconds + ' s';

    if (parseInt(days) > 0 && parseInt(days) >= 365) {
        return parseInt(days / 365) + ' سنة ';
    }
    else if (parseInt(days) > 0 && parseInt(days) >= 30) {
        return parseInt(days / 30) + ' شهر ';
    }
    else if (parseInt(days) > 0 && parseInt(days) < 30) {
        return days + ' أيام ';
    }
    else {
        return 'اليوم';
    }
}

function GetMonth(component) {
    switch (parseInt(component)) {
        case 01 || 1:
            return 00;
            break;
        case 02 || 2:
            return 01;
            break;
        case 03 || 3:
            return 02;
            break;
        case 04 || 4:
            return 03;
            break;
        case 05 || 5:
            return 04;
            break;
        case 06 || 6:
            return 05;
            break;
        case 07 || 7:
            return 06;
            break;
        case 08 || 8:
            return 07;
            break;
        case 09 || 9:
            return 08;
            break;
        case 10:
            return 09;
            break;
        case 11:
            return 10;
            break;
        case 12:
            return 11;
            break;
    }
}

function GetDayOfWeek(component) {
    var weekday = new Array(7);
    weekday[0] = "الأحد"; weekday[1] = "الإثنين"; weekday[2] = "الثلاثاء"; weekday[3] = "الأربعاء"; weekday[4] = "الخميس"; weekday[5] = "الجمعة"; weekday[6] = "السبت";

    return weekday[component];
}

function CheckDateOlderThanToday(CurrentDate, dateToCompare) {
    var value = new Date(CurrentDate[0], GetMonth(CurrentDate[1]), CurrentDate[2]);
    var tripDate = new Date(dateToCompare[0], GetMonth(dateToCompare[1]), dateToCompare[2]);

    var difference_ms = tripDate.getTime() - value.getTime();
    difference_ms = difference_ms / 3600000;
    var days = Math.floor(difference_ms / 24);
    if (days >= 0) {
        return false;
    }
    else {
        return true;
    }
}

function findElement(divToFind) {
    var divToFindWithId = document.getElementById(divToFind);
    if (typeof divToFindWithId != 'undefined' && divToFindWithId != null) {
        return true;
    }
    else {
        return false;
    }
}

function CreatePickerModal(reqProduct, pageName, commentId, elementId) {
    if (typeof commentId != 'undefined') {
        myApp.pickerModal('<div id="divReplyComment"  class="picker-modal reply-comment-picker_' + elementId + '">' +
                                    '<div class="view view-main">' +
                                    '<div class="toolbar">' + '<div class="toolbar-inner">' +
                                    '<div class="left"></div>' +
                                    '<div class="right"><a id="linkCloseReplyComment_' + elementId + '"  href="#" class="close-picker close-picker-btn">إلغاء</a></div>' +
                                    '</div></div><div class="picker-modal-inner">' +
                                    '<div class="content-block">' +
                                    '<div class="list-block">' +
                                    '<ul><li class="align-top">' +
                                    '<div class="item-content">' +
                                    '<div class="item-inner">' +
                                    '<div class="item-input" style="border-radius: 5px;">' +
                                    '<textarea id="txtReplyComment_' + elementId + '"></textarea>' +
                                    '</div>' +
                                    '</div>' +
                                    '</div>' +
                                    '</li>' +
                                    '<a id="linkPopupReplyComment_' + elementId + '" href="#" class="button button-big button-fill color-green" style="margin: 10px 2em">رد</a>' +
                                    '</ul>' +
                                    '</div>' +
                                    '</div></div></div>');

        $$('.reply-comment-picker_' + elementId).on('opened', function () {
            $('#txtReplyComment_' + elementId).val('');
            GoToReplyComment(reqProduct, pageName, commentId, elementId);
        });
    }
    else {
        myApp.pickerModal('<div id="divAddComment' + pageName + '"  class="picker-modal add-comment-picker_' + pageName + '">' +
                                    '<div class="view view-main">' +
                                    '<div class="toolbar">' + '<div class="toolbar-inner">' +
                                    '<div class="left"></div>' +
                                    '<div class="right"><a id="linkCloseAddComment_' + pageName + '"  href="#" class="close-picker close-picker-btn">إلغاء</a></div>' +
                                    '</div></div><div class="picker-modal-inner">' +
                                    '<div class="content-block">' +
                                    '<div class="list-block">' +
                                    '<ul><li class="align-top">' +
                                    '<div class="item-content">' +
                                    '<div class="item-inner addCommentTxt">' +
                                    '<div class="item-input" style="border-radius: 5px;">' +
                                    '<textarea id="txtAddComment_' + pageName + '"></textarea>' +
                                    '</div>' +
                                    '</div>' +
                                    '</div>' +
                                    '</li>' +
                                    '<a id="linkPopupAddComment_' + pageName + '" href="#" class="button button-big button-fill color-green" style="margin: 10px 2em">إضافة</a>' +
                                    '</ul>' +
                                    '</div>' +
                                    '</div></div></div>');

        $$('.add-comment-picker_' + pageName).on('opened', function () {
            //$('#txtReplyComment_' + pageName).val('');
            GoToAddComment(reqProduct, pageName);
        });
    }
}

function BuildComment(reqProduct, pageName, comment, newAddedComment) {
    var liComment = document.createElement('li');
    var divChatOwner = document.createElement('div');
    var divChatAvatar = document.createElement('div');
    var imgAvatar = document.createElement('img');
    var divChat = document.createElement('div');
    var divArrow = document.createElement('div');
    var divChatText = document.createElement('div');
    var divChatTime = document.createElement('div');
    var divChatAction = document.createElement('div');
    var linkDeleteProfileComment = document.createElement('a');
    var iDeleteProfileComment = document.createElement('i');
    var iChatOwner = document.createElement('i');
    var pCommentText = document.createElement('p');
    //var linkReportProfileComment = document.createElement('a');

    var res = JSON.parse(localStorage.getItem('userLoggedIn'));
    if (res != null && res.id == comment.applicationUserId) {
        if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
            localStorage.setItem('usrPhoto', res.photoUrl);
            imgAvatar.src = hostUrl + "Uploads/" + localStorage.getItem('usrPhoto');
        }
        else {
            if (localStorage.getItem('usrPhoto')) {
                imgAvatar.src = hostUrl + "Uploads/" + localStorage.getItem('usrPhoto');
            }
            else {
                imgAvatar.src = 'img/1.jpg';
            }
        }
    }
    else {
        if (typeof comment.photoUrl != 'undefined' && comment.photoUrl != null && comment.photoUrl != '' && comment.photoUrl != ' ') {
            imgAvatar.src = hostUrl + "Uploads/" + comment.photoUrl;
        }
        else {
            imgAvatar.src = 'img/1.jpg';
        }
    }

    
    

    iDeleteProfileComment.className = 'ionicons ion-android-delete';

    divChatAvatar.className = 'commentavatar';
    divChatOwner.className = 'commentowner';
    divChat.className = 'column-comment-right chat';
    divArrow.className = 'arrow-comment-globe';
    divChatText.className = 'comment-text';
    divChatTime.className = 'comment-time';
    divChatAction.className = 'comment-action';

    linkDeleteProfileComment.appendChild(iDeleteProfileComment);
    linkDeleteProfileComment.setAttribute('id', 'linkRemove' + pageName + '_' + comment.id);
    //linkReportProfileComment.setAttribute('id', 'linkReport' + pageName + '_' + comment.id);   

    iChatOwner.className = 'ionicons ion-person';
    divChatOwner.appendChild(iChatOwner);
    if (typeof comment.userFullName != 'undefined' && comment.userFullName != null && comment.userFullName != '' && comment.userFullName != ' ') {
        divChatOwner.innerHTML += comment.userFullName;
    }
    else {
        divChatOwner.innerHTML += comment.userFirstName;
    }

    

    divChatTime.innerHTML = comment.createDate.split("T")[0];
    pCommentText.innerHTML = comment.message;

    divChatText.appendChild(divChatOwner);
    divChatText.appendChild(pCommentText);

    if (res != null && res.id == comment.applicationUserId) {
        divChatAction.appendChild(linkDeleteProfileComment);
    }

    divChatAvatar.appendChild(imgAvatar);
    liComment.appendChild(divChatAvatar);
    divChat.appendChild(divArrow);
    
    divChat.appendChild(divChatText);
    divChat.appendChild(divChatTime);
    divChat.appendChild(divChatAction);

    liComment.setAttribute('id', 'comment_' + pageName + comment.id);
    liComment.className = 'liComment';
    liComment.appendChild(divChat);

    return liComment;
}

function DrawComments(reqProduct, comments, elementName, pageName, startIndex, itemsPerLoad) {
    var ulProductDetailsComments = document.getElementById(elementName);
    //ulProductDetailsComments.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    initAddComment = true;
    initReplyPopUp = true;

    if (maxNumber > comments.length) {
        maxNumber = comments.length;
    }
    var divComment;

    for (var i = 0; i < comments.length; i++) {
        var divComment = BuildComment(reqProduct, pageName, comments[i], false);
        ulProductDetailsComments.appendChild(divComment);

        //$('#linkReportCmnt' + pageName + '_' + comments[i].id).unbind().on('click', function () {
        //    var elemId = $(this).attr('id');
        //    var commentId = elemId.split('_')[1];
        //    myApp.confirm('هل انت متاكد من الإبلاغ عن هذا التعليق؟', 'تأكيد', function () {
        //        GoToReportComment(reqProduct, pageName, commentId, commentId);
        //    });
        //});

        $('#linkRemove' + pageName + '_' + comments[i].id).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var commentId = elemId.split('_')[1];
            myApp.confirm('هل انت متاكد من حذف هذا التعليق؟', 'تأكيد', function () {
                GoToRemoveComment(reqProduct, pageName, commentId, commentId);
            });
        });
    }
}

function DrawUserDetails(reqUser) {
    
}

function DrawAdvertisment(Advertisments) {
    $('#linkImgHeader').hide();
    if (Advertisments != null && Advertisments.length > 0) {
        var divCategoriesInHome = document.getElementById('divCategoriesInHome');
        var linkImgHeader = document.getElementById('linkImgHeader');
        var linkImg = document.createElement('a');
        var imgAdvHeaderCategories = document.getElementById('imgAdvHeaderCategories');
        var divColAdv = document.createElement('div');
        var divAdv = document.createElement('div');
        var imgAdv = document.createElement('img');

        imgAdv.setAttribute('onerror', 'replaceURL(this,"inner")');
        imgAdvHeaderCategories.setAttribute('onerror', 'replaceURL(this,"head")');

        var rand = GetRandomElemFromList(Advertisments);

        linkImg.setAttribute('id', 'linkAdvImg_' + rand.id);
        localStorage.setItem('advId', JSON.stringify(rand));

        divColAdv.className += 'col-100';
        divAdv.className += 'adv1';


        imgAdv.setAttribute('onerror', 'replaceURL(this,"product")');
        imgAdvHeaderCategories.setAttribute('onerror', 'replaceURL(this,"product")');

        if (typeof rand.imageUrl != 'undefined' && rand.imageUrl != null && rand.imageUrl != '' && rand.imageUrl != ' ') {
            imgAdv.src = hostUrl + '/Uploads/' + rand.imageUrl;
            imgAdvHeaderCategories.src = hostUrl + '/Uploads/' + rand.imageUrl;
        }
        else {
            imgAdv.src = 'img/adv_inner.jpg';
            imgAdvHeaderCategories.src = 'img/adv_head.jpg';
        }


        linkImg.appendChild(imgAdv);
        divAdv.appendChild(linkImg);
        divColAdv.appendChild(divAdv);
        divCategoriesInHome.appendChild(divColAdv);

        $('#linkAdvImg_' + rand.id).unbind().on('click', function () {

            var elemId = $(this).attr('id');
            var advId = elemId.split('_')[1];
            var reqAdv;
            for (var i = 0; i < Advertisments.length; ++i) {
                if (Advertisments[i].id == advId) {
                    reqAdv = Advertisments[i];
                    break;
                }
            }
            mainView.router.loadPage({ pageName: 'productDetails', query: { product: reqAdv, fromPage: 'categories' } });
        });


        $('#linkImgHeader').unbind().on('click', function () {

            var advertisment = JSON.parse(localStorage.getItem('advId'));
            mainView.router.loadPage({ pageName: 'productDetails', query: { product: advertisment, fromPage: 'categories' } });
        });
    }
    else {
        $('#imgAdvHeaderCategories').show();
        var divCategoriesInHome = document.getElementById('divCategoriesInHome');
        var imgAdvHeaderCategories = document.getElementById('imgAdvHeaderCategories');
        var divColAdv = document.createElement('div');
        var divAdv = document.createElement('div');
        var imgAdv = document.createElement('img');

        imgAdv.setAttribute('onerror', 'replaceURL(this,"inner")');
        imgAdvHeaderCategories.setAttribute('onerror', 'replaceURL(this,"head")');

        divColAdv.className += 'col-100';
        divAdv.className += 'adv1';

        imgAdv.src = 'img/adv_inner.jpg';
        imgAdvHeaderCategories.src = 'img/adv_head.jpg';



        divAdv.appendChild(imgAdv);
        divColAdv.appendChild(divAdv);
        divCategoriesInHome.appendChild(divColAdv);

    }
    
}

function DrawCategoriesInHome(categories, advertisments, startIndex, ItemsPerLoad) {
    var divCategoriesInHome = document.getElementById('divCategoriesInHome');
    //divCategoriesInHome.innerHTML = '';
    var counter = 0;
    var advCounter = 0;
    var catNumBeforeAdv = 4;
    var indexOfFirstAdv = parseInt(catNumBeforeAdv - 1);
    var indexToRepeatEachAdv = parseInt(indexOfFirstAdv * 2);

    for (var c = 0; c < categories.length; c++) {
        var divCategory = document.createElement('div');
        var linkCategory = document.createElement('a');
        var divCard = document.createElement('div');
        var divCardContent = document.createElement('div');
        var divCardContentInner = document.createElement('div');
        var imgCategory = document.createElement('img');
        var divCardFooter = document.createElement('div');
        var divContentBlock = document.createElement('div');
        var bContentBlock = document.createElement('b');

        divCategory.className += 'col-50 divCategoryInHome';
        linkCategory.setAttribute('id', 'linkCategoryHome_' + categories[c].id);
        divCard.className += 'card';
        divCardContent.className += 'card-content';
        divCardContentInner.className += 'card-content-inner';

        imgCategory.setAttribute('onerror', 'replaceURL(this,"product")');

        if (typeof categories[c].imageUrl != 'undefined' && categories[c].imageUrl != null && categories[c].imageUrl != '' && categories[c].imageUrl != ' ') {
            imgCategory.src = hostUrl + 'Uploads/' + categories[c].imageUrl;
        }
        else {
            imgCategory.src = 'img/no-product.png';
        }

        divCardFooter.className += 'card-footer';
        divContentBlock.className += 'content-block';
        bContentBlock.innerHTML = categories[c].name;

        divContentBlock.appendChild(bContentBlock);
        divCardFooter.appendChild(divContentBlock);

        divCardContentInner.appendChild(imgCategory);
        divCardContent.appendChild(divCardContentInner);
        divCard.appendChild(divCardContent);
        divCard.appendChild(divCardFooter);

        linkCategory.appendChild(divCard);
        divCategory.appendChild(linkCategory);
        divCategoriesInHome.appendChild(divCategory);

        $('#linkCategoryHome_' + categories[c].id).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var catId = elemId.split('_')[1];
            var reqCategory;
            for (var i = 0; i < categories.length; ++i) {
                if (categories[i].id == catId) {
                    reqCategory = categories[i];
                    break;
                }
            }
            mainView.router.loadPage({ pageName: 'products', query: { category: reqCategory } });
        });

        if (categories.length < catNumBeforeAdv) {
            if (c == parseInt(categories.length - 1)) {
                    DrawAdvertisment(advertisments);
            }
        }
        else {
            if (c == indexOfFirstAdv) {
                counter = 0;
                advCounter = 0;
                DrawAdvertisment(advertisments);
            }
            else {
                if (counter > 0 && counter % indexToRepeatEachAdv == 0) {
                    DrawAdvertisment(advertisments);
                }
            }
        }

        counter++;

    }
}

function DrawProductsInCategory(allProductsInCategory, startIndex, itemsPerLoad) {
    var divProductsInCategory = document.getElementById('divProductsInCategory');
    //divProductsInCategory.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    if (maxNumber > allProductsInCategory.length) {
        maxNumber = allProductsInCategory.length;
    }

    for (var productIndex = 0; productIndex < allProductsInCategory.length; productIndex++) {
        var divProductInCategory = document.createElement('div');
        var linkCategory = document.createElement('a');
        var divCard = document.createElement('div');
        var divCardContent = document.createElement('div');
        var divCardContentInner = document.createElement('div');
        var imgCategory = document.createElement('img');
        var divCardFooter = document.createElement('div');
        var divContentBlock = document.createElement('div');
        var bContentBlock = document.createElement('b');

        var divCardOption = document.createElement('div');
        var divContentBlockInOption = document.createElement('div');
        var divRowInOption = document.createElement('div');
        var divFirstColInRowInOption = document.createElement('div');
        var divSecondColInRowInOption = document.createElement('div');
        var divThirdColInRowInOption = document.createElement('div');
        var iFirstInRowInOption = document.createElement('i');
        var iSecondInRowInOption = document.createElement('i');
        var iThirdInRowInOption = document.createElement('i');
        var spanFirstColInRowInOption = document.createElement('span');
        var spanSecondColInRowInOption = document.createElement('span');
        var spanThirdColInRowInOption = document.createElement('span');

        divProductInCategory.className += 'divProductInCategory ';
        divProductInCategory.className += 'col-50';
        linkCategory.setAttribute('id', 'linkProductInCategory_' + allProductsInCategory[productIndex].id);
        divCard.className += 'card';
        divCardContent.className += 'card-content';
        divCardContentInner.className += 'card-content-inner';

        imgCategory.setAttribute('onerror', 'replaceURL(this,"product")');

        if (typeof allProductsInCategory[productIndex].imageUrl != 'undefined' && allProductsInCategory[productIndex].imageUrl != null && allProductsInCategory[productIndex].imageUrl != '' && allProductsInCategory[productIndex].imageUrl != ' ') {
            imgCategory.src = hostUrl + 'Uploads/' + allProductsInCategory[productIndex].imageUrl;
        }
        else {
            imgCategory.src = 'img/no-product.png';
        }

        divCardFooter.className += 'card-footer';
        divContentBlock.className += 'content-block';
        bContentBlock.innerHTML = allProductsInCategory[productIndex].name;

        divContentBlock.appendChild(bContentBlock);
        divCardFooter.appendChild(divContentBlock);

        divCardContentInner.appendChild(imgCategory);
        divCardContent.appendChild(divCardContentInner);

        divCardOption.className += 'card-option';
        divContentBlockInOption.className += 'content-block';
        divRowInOption.className += 'row';
        divFirstColInRowInOption.className += 'col-33';
        divSecondColInRowInOption.className += 'col-33';
        divThirdColInRowInOption.className += 'col-33';
        iFirstInRowInOption.className += 'ionicons ion-android-textsms';
        iSecondInRowInOption.className += 'ionicons ion-eye';
        iThirdInRowInOption.className += 'ionicons ion-android-favorite';
        spanFirstColInRowInOption.innerHTML = allProductsInCategory[productIndex].comments;
        spanSecondColInRowInOption.innerHTML = allProductsInCategory[productIndex].numberOfViews;
        spanThirdColInRowInOption.innerHTML = allProductsInCategory[productIndex].numberOfLikes;

        divFirstColInRowInOption.appendChild(iFirstInRowInOption);
        divFirstColInRowInOption.appendChild(spanFirstColInRowInOption);
        divSecondColInRowInOption.appendChild(iSecondInRowInOption);
        divSecondColInRowInOption.appendChild(spanSecondColInRowInOption);
        divThirdColInRowInOption.appendChild(iThirdInRowInOption);
        divThirdColInRowInOption.appendChild(spanThirdColInRowInOption);

        divRowInOption.appendChild(divFirstColInRowInOption);
        divRowInOption.appendChild(divSecondColInRowInOption);
        divRowInOption.appendChild(divThirdColInRowInOption);

        divContentBlockInOption.appendChild(divRowInOption);
        divCardOption.appendChild(divContentBlockInOption);



        divCardContent.appendChild(divCardOption);
        divCard.appendChild(divCardContent);
        divCard.appendChild(divCardFooter);

        linkCategory.appendChild(divCard);
        divProductInCategory.appendChild(linkCategory);

        divProductsInCategory.appendChild(divProductInCategory);

        $('#linkProductInCategory_' + allProductsInCategory[productIndex].id).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var productId = elemId.split('_')[1];
            var reqProduct;
            for (var i = 0; i < allProductsInCategory.length; ++i) {
                if (allProductsInCategory[i].id == productId) {
                    reqProduct = allProductsInCategory[i];
                    break;
                }
            }
            mainView.router.loadPage({ pageName: 'productDetails', query: { product: reqProduct, fromPage: 'products' } });
        });
    }
}

function DrawSearchResults(allSearchResults, startIndex, itemsPerLoad) {
    var divSearchResults = document.getElementById('divSearchResults');
    //divProductsInCategory.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    if (maxNumber > allSearchResults.length) {
        maxNumber = allSearchResults.length;
    }

    for (var productIndex = 0; productIndex < allSearchResults.length; productIndex++) {
        var divProductInCategory = document.createElement('div');
        var linkCategory = document.createElement('a');
        var divCard = document.createElement('div');
        var divCardContent = document.createElement('div');
        var divCardContentInner = document.createElement('div');
        var imgCategory = document.createElement('img');
        var divCardFooter = document.createElement('div');
        var divContentBlock = document.createElement('div');
        var bContentBlock = document.createElement('b');

        var divCardOption = document.createElement('div');
        var divContentBlockInOption = document.createElement('div');
        var divRowInOption = document.createElement('div');
        var divFirstColInRowInOption = document.createElement('div');
        var divSecondColInRowInOption = document.createElement('div');
        var divThirdColInRowInOption = document.createElement('div');
        var iFirstInRowInOption = document.createElement('i');
        var iSecondInRowInOption = document.createElement('i');
        var iThirdInRowInOption = document.createElement('i');
        var spanFirstColInRowInOption = document.createElement('span');
        var spanSecondColInRowInOption = document.createElement('span');
        var spanThirdColInRowInOption = document.createElement('span');

        divProductInCategory.className += 'divSearchResult ';
        divProductInCategory.className += 'col-50';
        linkCategory.setAttribute('id', 'linkSearchResults_' + allSearchResults[productIndex].id);
        divCard.className += 'card';
        divCardContent.className += 'card-content';
        divCardContentInner.className += 'card-content-inner';

        imgCategory.setAttribute('onerror', 'replaceURL(this,"product")');

        if (typeof allSearchResults[productIndex].imageUrl != 'undefined' && allSearchResults[productIndex].imageUrl != null && allSearchResults[productIndex].imageUrl != '' && allSearchResults[productIndex].imageUrl != ' ') {
            imgCategory.src = hostUrl + 'Uploads/' + allSearchResults[productIndex].imageUrl;
        }
        else {
            imgCategory.src = 'img/no-product.png';
        }

        divCardFooter.className += 'card-footer';
        divContentBlock.className += 'content-block';
        bContentBlock.innerHTML = allSearchResults[productIndex].name;

        divContentBlock.appendChild(bContentBlock);
        divCardFooter.appendChild(divContentBlock);

        divCardContentInner.appendChild(imgCategory);
        divCardContent.appendChild(divCardContentInner);

        divCardOption.className += 'card-option';
        divContentBlockInOption.className += 'content-block';
        divRowInOption.className += 'row';
        divFirstColInRowInOption.className += 'col-33';
        divSecondColInRowInOption.className += 'col-33';
        divThirdColInRowInOption.className += 'col-33';
        iFirstInRowInOption.className += 'ionicons ion-android-textsms';
        iSecondInRowInOption.className += 'ionicons ion-eye';
        iThirdInRowInOption.className += 'ionicons ion-android-favorite';
        spanFirstColInRowInOption.innerHTML = allSearchResults[productIndex].comments;
        spanSecondColInRowInOption.innerHTML = allSearchResults[productIndex].numberOfViews;
        spanThirdColInRowInOption.innerHTML = allSearchResults[productIndex].numberOfLikes;

        divFirstColInRowInOption.appendChild(iFirstInRowInOption);
        divFirstColInRowInOption.appendChild(spanFirstColInRowInOption);
        divSecondColInRowInOption.appendChild(iSecondInRowInOption);
        divSecondColInRowInOption.appendChild(spanSecondColInRowInOption);
        divThirdColInRowInOption.appendChild(iThirdInRowInOption);
        divThirdColInRowInOption.appendChild(spanThirdColInRowInOption);

        divRowInOption.appendChild(divFirstColInRowInOption);
        divRowInOption.appendChild(divSecondColInRowInOption);
        divRowInOption.appendChild(divThirdColInRowInOption);

        divContentBlockInOption.appendChild(divRowInOption);
        divCardOption.appendChild(divContentBlockInOption);



        divCardContent.appendChild(divCardOption);
        divCard.appendChild(divCardContent);
        divCard.appendChild(divCardFooter);

        linkCategory.appendChild(divCard);
        divProductInCategory.appendChild(linkCategory);

        divSearchResults.appendChild(divProductInCategory);

        $('#linkSearchResults_' + allSearchResults[productIndex].id).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var productId = elemId.split('_')[1];
            var reqProduct;
            for (var i = 0; i < allSearchResults.length; ++i) {
                if (allSearchResults[i].id == productId) {
                    reqProduct = allSearchResults[i];
                    break;
                }
            }
            mainView.router.loadPage({ pageName: 'productDetails', query: { product: reqProduct, fromPage: 'searchResults' } });
        });
    }
}

function DrawUserProducts(pageName,userProducts, startIndex, itemsPerLoad) {
    var divUserProducts = document.getElementById('divUserProducts');
    //divUserProducts.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    if (maxNumber > userProducts.length) {
        maxNumber = userProducts.length;
    }

    for (var productIndex = 0; productIndex < userProducts.length; productIndex++) {
        var divUserProduct = document.createElement('div');
        var linkCategory = document.createElement('a');
        var divCard = document.createElement('div');
        var divDeleteAdv = document.createElement('div');
        var iDeleteAdv = document.createElement('i');
        var divCardContent = document.createElement('div');
        var divCardContentInner = document.createElement('div');
        var imgCategory = document.createElement('img');
        var divCardFooter = document.createElement('div');
        var divContentBlock = document.createElement('div');
        var bContentBlock = document.createElement('b');

        var divCardOption = document.createElement('div');
        var divContentBlockInOption = document.createElement('div');
        var divRowInOption = document.createElement('div');
        var divFirstColInRowInOption = document.createElement('div');
        var divSecondColInRowInOption = document.createElement('div');
        var divThirdColInRowInOption = document.createElement('div');
        var iFirstInRowInOption = document.createElement('i');
        var iSecondInRowInOption = document.createElement('i');
        var iThirdInRowInOption = document.createElement('i');
        var spanFirstColInRowInOption = document.createElement('span');
        var spanSecondColInRowInOption = document.createElement('span');
        var spanThirdColInRowInOption = document.createElement('span');

        divUserProduct.className += 'divUserProduct ';
        divUserProduct.className += 'col-50';
        linkCategory.setAttribute('id', 'linkUserProduct_' + userProducts[productIndex].id);
        divCard.className += 'card';
        divDeleteAdv.className = 'deleteBtn';
        iDeleteAdv.className = 'ionicons ion-trash-b';
        divCardContent.className += 'card-content';
        divCardContentInner.className += 'card-content-inner';

        imgCategory.setAttribute('onerror', 'replaceURL(this,"product")');

        if (typeof userProducts[productIndex].imageUrl != 'undefined' && userProducts[productIndex].imageUrl != null && userProducts[productIndex].imageUrl != '' && userProducts[productIndex].imageUrl != ' ') {
            imgCategory.src = hostUrl + 'Uploads/' + userProducts[productIndex].imageUrl;
        }
        else {
            imgCategory.src = 'img/no-product.png';
        }

        divCardFooter.className += 'card-footer';
        divContentBlock.className += 'content-block';
        bContentBlock.innerHTML = userProducts[productIndex].name;

        divContentBlock.appendChild(bContentBlock);
        divCardFooter.appendChild(divContentBlock);

        divCardContentInner.appendChild(imgCategory);
        divCardContent.appendChild(divCardContentInner);

        divCardOption.className += 'card-option';
        divContentBlockInOption.className += 'content-block';
        divRowInOption.className += 'row';
        divFirstColInRowInOption.className += 'col-33';
        divSecondColInRowInOption.className += 'col-33';
        divThirdColInRowInOption.className += 'col-33';
        iFirstInRowInOption.className += 'ionicons ion-android-textsms';
        iSecondInRowInOption.className += 'ionicons ion-eye';
        iThirdInRowInOption.className += 'ionicons ion-android-favorite';
        spanFirstColInRowInOption.innerHTML = userProducts[productIndex].comments;
        spanSecondColInRowInOption.innerHTML = userProducts[productIndex].numberOfViews;
        spanThirdColInRowInOption.innerHTML = userProducts[productIndex].numberOfLikes;

        divFirstColInRowInOption.appendChild(iFirstInRowInOption);
        divFirstColInRowInOption.appendChild(spanFirstColInRowInOption);
        divSecondColInRowInOption.appendChild(iSecondInRowInOption);
        divSecondColInRowInOption.appendChild(spanSecondColInRowInOption);
        divThirdColInRowInOption.appendChild(iThirdInRowInOption);
        divThirdColInRowInOption.appendChild(spanThirdColInRowInOption);

        divRowInOption.appendChild(divFirstColInRowInOption);
        divRowInOption.appendChild(divSecondColInRowInOption);
        divRowInOption.appendChild(divThirdColInRowInOption);

        divContentBlockInOption.appendChild(divRowInOption);
        divCardOption.appendChild(divContentBlockInOption);

        divDeleteAdv.setAttribute('id', 'divDeleteAdv_' + userProducts[productIndex].id);
        divDeleteAdv.appendChild(iDeleteAdv);

        divCardContent.appendChild(divCardOption);
        divCard.appendChild(divCardContent);
        divCard.appendChild(divCardFooter);

        linkCategory.appendChild(divCard);
        divUserProduct.appendChild(divDeleteAdv);
        divUserProduct.appendChild(linkCategory);
        divUserProduct.setAttribute('id', 'divUserProduct_' + userProducts[productIndex].id);

        divUserProducts.appendChild(divUserProduct);

        $('#linkUserProduct_' + userProducts[productIndex].id).unbind().on('click', function () {
            var elemId = $(this).attr('id');
            var productId = elemId.split('_')[1];
            var reqProduct;
            for (var i = 0; i < userProducts.length; ++i) {
                if (userProducts[i].id == productId) {
                    reqProduct = userProducts[i];
                    break;
                }
            }
            mainView.router.loadPage({ pageName: 'productDetails', query: { product: reqProduct, fromPage: pageName } });
        });

        $('#divDeleteAdv_' + userProducts[productIndex].id).unbind().on('click', function () {
            var elemId = $(this).attr('id');
            var productId = elemId.split('_')[1];

            myApp.confirm('هل انت متاكد من حذف هذا الإعلان؟', 'تأكيد', function () {
                CallService('profile', "DELETE", "api/advertisement/delete/" + productId, '', function (res) {
                    if (res != null && res == 1) {
                        var item = document.getElementById('divUserProduct_' + productId);
                        item.parentNode.removeChild(item);
                        if (localStorage.getItem('lastIndex')) {
                            var storedLastIndex = localStorage.getItem('lastIndex');
                            var storedMax = localStorage.getItem('maxUserProducts');
                            localStorage.setItem('lastIndex', parseInt(parseInt(storedLastIndex) - 1));
                            localStorage.setItem('maxUserProducts', parseInt(parseInt(storedMax) - 1));
                            var spanProfileProductNum = document.getElementById('spanProfileProductNum');
                            spanProfileProductNum.innerHTML = parseInt(parseInt(storedMax) - 1) + ' إعلانات ';
                        }
                    }
                });
            });
        });
    }
}

function DrawProductDetails(reqProduct) {
    var spanProductDetailsComments = document.getElementById('spanProductDetailsComments');
    var spanProductDetailsSeen = document.getElementById('spanProductDetailsSeen');
    var spanProductDetailsLikes = document.getElementById('spanProductDetailsLikes');
    var spanProductDetailsAddedSince = document.getElementById('spanProductDetailsAddedSince');
    var pProductDetailsOwnerName = document.getElementById('pProductDetailsOwnerName');
    var pProductDetailsOwnerPhoneNumber = document.getElementById('pProductDetailsOwnerPhoneNumber');
    var pProductDetailsName = document.getElementById('pProductDetailsName');
    var pProductDetailsDescription = document.getElementById('pProductDetailsDescription');
    var imgMainProductImage = document.getElementById('imgMainProductImage');
    var imgAdvertisementOwner = document.getElementById('imgAdvertisementOwner');
    

    localStorage.setItem('reqProduct', JSON.stringify(reqProduct));

    var days = GetCommentDuration(reqProduct.createDate.split('T')[0], new Date());

    spanProductDetailsComments.innerHTML = reqProduct.comments;
    spanProductDetailsSeen.innerHTML = reqProduct.numberOfViews;
    spanProductDetailsLikes.innerHTML = reqProduct.numberOfLikes;
    spanProductDetailsAddedSince.innerHTML = days;
    pProductDetailsOwnerName.innerHTML = reqProduct.fullName;
    pProductDetailsName.innerHTML = reqProduct.name;
    if (typeof reqProduct.phoneNumber != 'undefined' && reqProduct.phoneNumber != null) {
        pProductDetailsOwnerPhoneNumber.innerHTML = reqProduct.phoneNumber;
    }
    else {
        pProductDetailsOwnerPhoneNumber.innerHTML = 'لا يوجد رقم مسجل';
    }

    pProductDetailsDescription.innerHTML = reqProduct.description;

    imgMainProductImage.setAttribute('onerror', 'replaceURL(this,"product")');


    var user = JSON.parse(localStorage.getItem('userLoggedIn'));
    imgAdvertisementOwner.src = 'img/logo.png';


    if (reqProduct.photoUrl != null && typeof reqProduct.photoUrl != 'undefined' && reqProduct.photoUrl != null && reqProduct.photoUrl != '' && reqProduct.photoUrl != ' ') {
        imgAdvertisementOwner.src = hostUrl + 'Uploads/' + reqProduct.photoUrl;
    }


    if (typeof reqProduct.imageUrl != 'undefined' && reqProduct.imageUrl != null && reqProduct.imageUrl != '' && reqProduct.imageUrl != ' ') {
        imgMainProductImage.src = hostUrl + 'Uploads/' + reqProduct.imageUrl;
    }
    else {
        imgMainProductImage.src = 'img/no-product.png';
    }


    var photos = JSON.parse(reqProduct.jsonImages);
    var photosArray = [];

    for (var i = 0; i < photos.length; i++) {
        var photoObj = { url: hostUrl + 'Uploads/' + photos[i].ImageUrl, caption: reqProduct.name };
        photosArray.push(photoObj);
    }

    if (photosArray.length > 0) {
        myPhotoBrowserPopupDark = myApp.photoBrowser({
            photos: photosArray,
            theme: 'dark',
            backLinkText: 'إغلاق',
            ofText: 'من',
            type:'popup'
        });

        $$('#imgOverlayMainProductImage').on('click', function () {
            myPhotoBrowserPopupDark.open();
        });
    }

    var isLiked = reqProduct.isLiked;
    var isFavorit = reqProduct.isFavorit;
    var isReport = reqProduct.isReport;

    if (isLiked != null && isLiked == true) {
        $("#btnLikeProductDetails").addClass('like');
    }
    else {
        $("#btnLikeProductDetails").removeClass('like');
    }

    if (isFavorit != null && isFavorit == true) {
        $("#btnFavouriteProductDetails").addClass('fave');
    }
    else {
        $("#btnFavouriteProductDetails").removeClass('fave');
    }

    if (isReport != null && isReport == true) {
        $("#btnReportProductDetails").addClass('pan');
    }
    else{
        $("#btnReportProductDetails").removeClass('pan');
    }

    initContactProductOwner = true;
    initLikeProductPopup = true;
    initFavouriteProductPopup = true;
    initShareProductPopup = true;
    initReportProductPopup = true;

    $("#linkContactWithProductOwner").hide();
    $("#btnUpdateAdvertisment").show();

    if (!reqProduct.isOwner) {
        $("#linkContactWithProductOwner").show();
        $("#btnUpdateAdvertisment").hide();
    }



    //----------------------------------------------------------------------------------------------------
    $("#linkContactWithProductOwner").unbind().on("click", function () {
        var clickedLink = this;
        myApp.popover('.popover-contactWithOwner', clickedLink);
    });
    $$('.popover-contactWithOwner').on('opened', function () {
        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
        GoToContactProductOwnerOptions(storedProduct);
    });


    //----------------------------------------------------------------------------------------------------
    $("#btnReportProductDetails").unbind().on("click", function () {
        if (isReport != null && isReport == false && !$(this).hasClass('pan')) {
            var clickedLink = this;
            myApp.popover('.popover-report', clickedLink);
        }
        else {
            if (isReport == true) {
                myApp.alert('لقد قمت بالإبلاغ عن هذا المنتج من قبل .', 'خطأ', function () { });
            }
            else {
                myApp.alert('أنت غير مسجل للإبلاغ عن هذا المنتج أو رمز التحقق بك غير مفعل , من فضلك أعد تسجيل الدخول .', 'خطأ', function () { });
            }
        }
       
    });
    $$('.popover-report').on('opened', function () {
        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
        GoToReportProduct(storedProduct);
    });
    //----------------------------------------------------------------------------------------------------


    $("#btnLikeProductDetails").unbind().on("click", function () {
        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
        if (isLiked != null && isLiked == false) {
            var params = {
                'AdvertismentId': storedProduct.id
            };

            CallService('productDetails', "POST", "api/like/add", params, function (res) {
                if (res != null && res == 1) {
                    isLiked = true;
                    storedProduct.numberOfLikes = parseInt(parseInt(storedProduct.numberOfLikes) + 1);
                    spanProductDetailsLikes.innerHTML = storedProduct.numberOfLikes;
                    localStorage.setItem('reqProduct', JSON.stringify(storedProduct));
                    $("#btnLikeProductDetails").addClass('like');
                }
                else {
                    $("#btnLikeProductDetails").removeClass('like');
                }
            });
        }
        else {
            if (isLiked == true) {
                var params = {
                    
                };

                CallService('productDetails', "POST", "api/like/delete/" + storedProduct.id, params, function (res) {
                    if (res != null && res == 1) {
                        isLiked = false;
                        if (parseInt(storedProduct.numberOfLikes) > 0) {
                            storedProduct.numberOfLikes = parseInt(parseInt(storedProduct.numberOfLikes) - 1);
                        }
                        spanProductDetailsLikes.innerHTML = storedProduct.numberOfLikes;
                        localStorage.setItem('reqProduct', JSON.stringify(storedProduct));
                        $("#btnLikeProductDetails").removeClass('like');
                    }
                    else {
                        $("#btnLikeProductDetails").addClass('like');
                    }
                });
            }
            else {
                myApp.alert('أنت غير مسجل لعمل إعجاب أو رمز التحقق بك غير مفعل , من فضلك أعد تسجيل الدخول .', 'خطأ', function () { });
            }
        }

    });

    $("#btnFavouriteProductDetails").unbind().on("click", function () {
        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
        if (isFavorit != null && isFavorit == false) {
            var params = {
                'AdvertismentId': storedProduct.id
            };

            CallService('productDetails', "POST", "api/favorite/add", params, function (res) {
                if (res != null && res == 1) {
                    isFavorit = true;
                    $("#btnFavouriteProductDetails").addClass('fave');
                }
                else {
                    $("#btnFavouriteProductDetails").removeClass('fave');
                }
            });
        }
        else {
            if (isFavorit == true) {
                var params = {
                    
                };

                CallService('productDetails', "POST", "api/favorite/delete/" + storedProduct.id, params, function (res) {
                    if (res != null && res == 1) {
                        isFavorit = false;
                        $("#btnFavouriteProductDetails").removeClass('fave');
                    }
                    else {
                        $("#btnFavouriteProductDetails").addClass('fave');
                    }
                });
            }
            else {
                myApp.alert('أنت غير مسجل لإضافة المنتج للمفضلة أو رمز التحقق بك غير مفعل , من فضلك أعد تسجيل الدخول .', 'خطأ', function () { });
            }
        }
        
    });
   
    $("#btnShareProductDetails").unbind().on("click", function () {
        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
        var image = '';
        if (typeof storedProduct.imageUrl != 'undefined' && storedProduct.imageUrl != null && storedProduct.imageUrl != '' && storedProduct.imageUrl != ' ') {
            image = hostUrl + 'Uploads/' + storedProduct.imageUrl;
        }
        else {
            image = hostUrl + 'Uploads/' + 'logo.png';
        }

        window.plugins.socialsharing.share('', storedProduct.name, [''], image, function (result) {
            console.log('result: ' + result);
        },
      function (msg) {
          console.log("Sharing failed with message: " + msg);
          myApp.alert('حدث خطأ أثناء مشاركة الإعلان .', 'خطأ', function () { });
      });
    });
    
}

function DrawFavourites(favourites, startIndex, itemsPerLoad) {
    var divFavorites = document.getElementById('divFavorites');
    //divFavorites.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    if (maxNumber > favourites.length) {
        maxNumber = favourites.length;
    }

    for (var productIndex = 0; productIndex < favourites.length; productIndex++) {
        var divFavorite = document.createElement('div');
        var linkCategory = document.createElement('a');
        var divCard = document.createElement('div');
        var divCardContent = document.createElement('div');
        var divCardContentInner = document.createElement('div');
        var imgCategory = document.createElement('img');
        var divCardFooter = document.createElement('div');
        var divContentBlock = document.createElement('div');
        var bContentBlock = document.createElement('b');

        var divCardOption = document.createElement('div');
        var divContentBlockInOption = document.createElement('div');
        var divRowInOption = document.createElement('div');
        var divFirstColInRowInOption = document.createElement('div');
        var divSecondColInRowInOption = document.createElement('div');
        var divThirdColInRowInOption = document.createElement('div');
        var iFirstInRowInOption = document.createElement('i');
        var iSecondInRowInOption = document.createElement('i');
        var iThirdInRowInOption = document.createElement('i');
        var spanFirstColInRowInOption = document.createElement('span');
        var spanSecondColInRowInOption = document.createElement('span');
        var spanThirdColInRowInOption = document.createElement('span');

        divFavorite.className += 'divFavorite ';
        divFavorite.className += 'col-50';
        linkCategory.setAttribute('id', 'linkFavorite_' + favourites[productIndex].advertismentId);
        divCard.className += 'card';
        divCardContent.className += 'card-content';
        divCardContentInner.className += 'card-content-inner';

        imgCategory.setAttribute('onerror', 'replaceURL(this,"product")');

        if (typeof favourites[productIndex].imageUrl != 'undefined' && favourites[productIndex].imageUrl != null && favourites[productIndex].imageUrl != '' && favourites[productIndex].imageUrl != ' ') {
            imgCategory.src = hostUrl + 'Uploads/' + favourites[productIndex].imageUrl;
        }
        else {
            imgCategory.src = 'img/no-product.png';
        }

        divCardFooter.className += 'card-footer';
        divContentBlock.className += 'content-block';
        bContentBlock.innerHTML = favourites[productIndex].name;

        divContentBlock.appendChild(bContentBlock);
        divCardFooter.appendChild(divContentBlock);

        divCardContentInner.appendChild(imgCategory);
        divCardContent.appendChild(divCardContentInner);

        divCardOption.className += 'card-option';
        divContentBlockInOption.className += 'content-block';
        divRowInOption.className += 'row';
        divFirstColInRowInOption.className += 'col-33';
        divSecondColInRowInOption.className += 'col-33';
        divThirdColInRowInOption.className += 'col-33';
        iFirstInRowInOption.className += 'ionicons ion-android-textsms';
        iSecondInRowInOption.className += 'ionicons ion-eye';
        iThirdInRowInOption.className += 'ionicons ion-android-favorite';
        spanFirstColInRowInOption.innerHTML = favourites[productIndex].comments;
        spanSecondColInRowInOption.innerHTML = favourites[productIndex].numberOfViews;
        spanThirdColInRowInOption.innerHTML = favourites[productIndex].numberOfLikes;

        divFirstColInRowInOption.appendChild(iFirstInRowInOption);
        divFirstColInRowInOption.appendChild(spanFirstColInRowInOption);
        divSecondColInRowInOption.appendChild(iSecondInRowInOption);
        divSecondColInRowInOption.appendChild(spanSecondColInRowInOption);
        divThirdColInRowInOption.appendChild(iThirdInRowInOption);
        divThirdColInRowInOption.appendChild(spanThirdColInRowInOption);

        divRowInOption.appendChild(divFirstColInRowInOption);
        divRowInOption.appendChild(divSecondColInRowInOption);
        divRowInOption.appendChild(divThirdColInRowInOption);

        divContentBlockInOption.appendChild(divRowInOption);
        divCardOption.appendChild(divContentBlockInOption);



        divCardContent.appendChild(divCardOption);
        divCard.appendChild(divCardContent);
        divCard.appendChild(divCardFooter);

        linkCategory.appendChild(divCard);
        divFavorite.appendChild(linkCategory);

        divFavorites.appendChild(divFavorite);

        $('#linkFavorite_' + favourites[productIndex].advertismentId).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var productId = elemId.split('_')[1];
            var reqProduct;
            for (var i = 0; i < favourites.length; ++i) {
                if (favourites[i].advertismentId == productId) {
                    reqProduct = favourites[i];
                    break;
                }
            }
            mainView.router.loadPage({ pageName: 'productDetails', query: { product: reqProduct, fromPage: 'favourite' } });
        });
    }
}

function DrawSideMenu(notificationsLength) {
    var divSideMenu = document.getElementById('divSideMenu');
    divSideMenu.innerHTML = '';

    var divLogoContainer = document.createElement('div');
    var imgLogo = document.createElement('img');
    divLogoContainer.className += 'logo-container';
    imgLogo.src = 'img/logo.png';
    divLogoContainer.appendChild(imgLogo);
    divSideMenu.appendChild(divLogoContainer);

    var linkMenuToCategories = document.createElement('a');
    var linkMenuToProfile = document.createElement('a');
    var linkMenuToFavourite = document.createElement('a');
    var linkMenuToContact = document.createElement('a');
    var linkMenuToChangePassword = document.createElement('a');
    var linkMenuToLogOut = document.createElement('a');
    var linkMenuToNotifcations = document.createElement('a');
    var lblLogOut = document.createElement('label');
    var lblNotifications = document.createElement('label');
    var hrLineInMenu = document.createElement('hr');

    var iCategory = document.createElement('i');
    var iProfile = document.createElement('i');
    var iFavourite = document.createElement('i');
    var iContact = document.createElement('i');
    var iChangePassword = document.createElement('i');
    var iLogOut = document.createElement('i');
    var iNotification = document.createElement('i');
    var spanNotification = document.createElement('span');
    iCategory.className += 'ionicons ion-android-laptop';
    iProfile.className += 'ionicons ion-ios-person-outline';
    iFavourite.className += 'ionicons ion-android-star';
    iContact.className += 'ionicons ion-android-phone-portrait';
    iChangePassword.className += 'icon ion-ios-locked';
    iLogOut.className += 'icon ion-log-out';
    iNotification.className += 'ionicons ion-android-notifications-none';
    lblLogOut.setAttribute('id', 'lblLogout');
    lblNotifications.innerHTML = 'الإشعارات';
    spanNotification.setAttribute('id', 'spanNotificationsNum');

    linkMenuToCategories.setAttribute('id', 'linkMenuToCategories');
    linkMenuToCategories.className += 'close-panel';
    linkMenuToCategories.appendChild(iCategory);
    linkMenuToCategories.innerHTML += 'الرئيسيه';

    linkMenuToProfile.setAttribute('id', 'linkMenuToProfile');
    linkMenuToProfile.className += 'close-panel';
    linkMenuToProfile.appendChild(iProfile);
    linkMenuToProfile.innerHTML += 'الصفحة الشخصية';

    linkMenuToFavourite.setAttribute('id', 'linkMenuToFavourite');
    linkMenuToFavourite.className += 'close-panel';
    linkMenuToFavourite.appendChild(iFavourite);
    linkMenuToFavourite.innerHTML += 'المفضلات';

    linkMenuToContact.setAttribute('id', 'linkMenuToContact');
    linkMenuToContact.className += 'close-panel';
    linkMenuToContact.appendChild(iContact);
    linkMenuToContact.innerHTML += 'إتصل بنا';

    linkMenuToChangePassword.setAttribute('id', 'linkMenuToChangePassword');
    linkMenuToChangePassword.className += 'close-panel';
    linkMenuToChangePassword.appendChild(iChangePassword);
    linkMenuToChangePassword.innerHTML += 'تغيير كلمة السر';

    linkMenuToLogOut.setAttribute('id', 'linkMenuToLogOut');
    linkMenuToLogOut.className += 'close-panel';
    linkMenuToLogOut.appendChild(iLogOut);
    linkMenuToLogOut.appendChild(lblLogOut);

    linkMenuToNotifcations.setAttribute('id', 'linkMenuToNotifcations');
    linkMenuToNotifcations.className += 'close-panel';
    linkMenuToNotifcations.appendChild(iNotification);
    linkMenuToNotifcations.appendChild(lblNotifications);
    linkMenuToNotifcations.appendChild(spanNotification);

    divSideMenu.appendChild(linkMenuToCategories);
    divSideMenu.appendChild(linkMenuToProfile);
    if (localStorage.getItem('Visitor') != true && localStorage.getItem('Visitor') != 'true') {
        divSideMenu.appendChild(linkMenuToNotifcations);
    }
    divSideMenu.appendChild(linkMenuToFavourite);
    divSideMenu.appendChild(linkMenuToContact);
    divSideMenu.appendChild(linkMenuToChangePassword);
    divSideMenu.appendChild(linkMenuToLogOut);
    divSideMenu.appendChild(hrLineInMenu);

    $('#spanNotificationsNum').html(notificationsLength);

    $('#linkMenuToCategories').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        if (currentPage === 'categories') {
            GoToCategoriesPage('refresh');
        }
        else {
            mainView.router.loadPage({ pageName: 'categories' });
        }
    });

    $('#linkMenuToProfile').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        mainView.router.loadPage({ pageName: 'profile' });
    });

    $('#linkMenuToNotifcations').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        mainView.router.loadPage({ pageName: 'notification' });
    });

    $('#linkMenuToFavourite').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        mainView.router.loadPage({ pageName: 'favourite' });
    });

    $('#linkMenuToContact').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        mainView.router.loadPage({ pageName: 'contact' });
    });

    $('#linkMenuToChangePassword').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        mainView.router.loadPage({ pageName: 'changePassword' });
    });

    $('#linkMenuToLogOut').unbind().on('click', function () {
        
        UpdateUserStatus(false, function () { });
        cleatTimer();
        localStorage.removeItem('appToken');
        localStorage.removeItem('USName');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userLoggedIn');
        localStorage.removeItem('Visitor');
        localStorage.removeItem('loginUsingSocial');
        initSplashPage = true;
        initLoginPage = true;
        initForgetPasswordPage = true;
        initSignupPage = true;
        initChangePasswordPage = true;
        initResetPasswordPage = true;
        initSideMenu = true;
        mainView.router.loadPage({ pageName: 'login' });
    });

    var categoriesLinks=$$('#divSideMenu a.linkCategoryInPanel').length;

    if (allCategories.length > 0 && categoriesLinks == 0 && allCategories.length > categoriesLinks) {

        for (var c = 0; c < allCategories.length; c++) {
            var linkCategory = document.createElement('a');
            var iCategory = document.createElement('i');

            iCategory.className += allCategories[c].iconName;

            linkCategory.setAttribute('id', 'linkCategoryInMenu_' + allCategories[c].id);
            linkCategory.className += 'close-panel linkCategoryInPanel';

            linkCategory.appendChild(iCategory);
            linkCategory.innerHTML += allCategories[c].name;

            divSideMenu.appendChild(linkCategory);


            $('#linkCategoryInMenu_' + allCategories[c].id).unbind().on('click', function () {
                
                var elemId = $(this).attr('id');
                var catId = elemId.split('_')[1];
                var reqCategory;
                for (var i = 0; i < allCategories.length; ++i) {
                    if (allCategories[i].id == catId) {
                        reqCategory = allCategories[i];
                        break;
                    }
                }
                UpdateUserStatus(false, function () { });

                if (currentPage === 'products') {
                    localStorage.setItem('reqCategory', JSON.stringify(reqCategory));
                    GoToProductsPage('refresh');
                }
                else {
                    mainView.router.loadPage({ pageName: 'products', query: { category: reqCategory } });
                }

            });
        }
    }

    $('#linkMenuToProfile').hide();
    $('#linkMenuToFavourite').hide();

    if (localStorage.getItem('userLoggedIn')) {
        lblLogOut.innerHTML = 'تسجيل الخروج';
    }
    else {
        if (localStorage.getItem('USName')) {
            lblLogOut.innerHTML = 'تسجيل الخروج';
        }
        else {
            $('#linkMenuToChangePassword').css('display', 'none');
            lblLogOut.innerHTML = 'تسجيل دخول';
        }
    }

    if (localStorage.getItem('loginUsingSocial') && localStorage.getItem('loginUsingSocial') == 'true') {
        $('#linkMenuToChangePassword').hide();
    }


    if (localStorage.getItem('USName')) {
        $('#linkMenuToProfile').show();
        $('#linkMenuToFavourite').show();
    }
}

function loadSideMenuLinks() {
    if (localStorage.getItem('Visitor') == true || localStorage.getItem('Visitor') == 'true') {
        $('.spanNotificationsNav').hide();
        if (currentPage != 'addProduct' && currentPage != 'editProduct') {
            GetAllCategories('SideMenu', function (res) {
                DrawSideMenu(0);
            });
        }
        else {
            DrawSideMenu(0);
        }
    }
    else {
        $('.spanNotificationsNav').show();
        FillNotifications('SideMenu', function (res1) {
            $('.spanNotificationsNav').html(res1);
            if (currentPage != 'addProduct' && currentPage != 'editProduct') {
                GetAllCategories('SideMenu', function (res) {
                    DrawSideMenu(res1);
                });
            }
            else {
                DrawSideMenu(res1);
            }
        });
    }
}

function GetChatInformation(fromPage, chatId, callBack) {
    if (parseInt(chatId) > 0) {
        CallService(fromPage, "GET", "api/chat/GetHeader/" + chatId, '', function (result) {
            callBack(result);
        });
    }
}

function GetChatRequestById(fromPage, requestId, callBack) {
    CallService(fromPage, "GET", "api/chat/GetHeaderbyChatRequest/" + requestId, '', function (result) {
        callBack(result);
    });
}

function GetRandomElemFromList(list) {
    var rand = list[Math.floor(Math.random() * list.length)];

    return rand;
}

function UploadProductPhotos(productPhotoId, ProductImages) {
    getProductImage(function (imageURI) {
        var imgRequired = document.getElementById('imgEditProduct_' + productPhotoId);
        imgRequired.src = 'data:image/jpeg;base64, ' + imageURI;

        var photoObj = { id: 0, imageUrl: imageURI };

        var reqProductPhoto = publicImageURI.filter(function (el) {
            return el.id == productPhotoId;
        })[0];

        var dbPhoto = ProductImages.filter(function (el) {
            return el.Id == productPhotoId;
        })[0];

        if (typeof dbPhoto != 'undefined' && dbPhoto != null) {
            reqProductPhoto.id = productPhotoId;
        }
        else {
            reqProductPhoto.id = 0;
        }

        reqProductPhoto.imageUrl = imageURI;

        publicImageURI[publicImageURI.indexOf(reqProductPhoto)] = reqProductPhoto;

        for (var i = 0; i < publicImageURI.length; i++) {
            var dbPhoto = ProductImages.filter(function (el) {
                return el.Id == publicImageURI[i].id;
            })[0];

            if (typeof dbPhoto == 'undefined' || dbPhoto == null) {
                publicImageURI[i].id = 0;
            }
        }

    });
}

function DrawAllProductPhotos(ProductImages, noOfElements) {
    var photos = [];
    publicImageURI = [];

    if (ProductImages.length == 0) {
        var divProductPhotos = document.getElementById('divProductPhotos');
        divProductPhotos.innerHTML = '';

        for (var index = 0; index < noOfElements; index++) {
            var divCol = document.createElement('div');
            var divImgContent = document.createElement('div');
            var linkOverlay = document.createElement('a');
            var iElement = document.createElement('i');
            var imgProduct = document.createElement('img');

            divCol.className = 'col-25';
            divImgContent.className = 'imgContent';
            linkOverlay.className = 'delOverlay';
            iElement.className = 'icon ion-ios-reverse-camera-outline';
            imgProduct.setAttribute('src', 'img/no-product.png');
            imgProduct.setAttribute('id', 'imgProduct_' + parseInt(index + 1));
            linkOverlay.setAttribute('id', 'linkAddProductPhoto_' + parseInt(index + 1));

            linkOverlay.appendChild(iElement);
            divImgContent.appendChild(linkOverlay);
            divImgContent.appendChild(imgProduct);
            divCol.appendChild(divImgContent);
            divProductPhotos.appendChild(divCol);

            $('#linkAddProductPhoto_' + parseInt(index + 1)).unbind().on('click', function () {
                
                var elemId = $(this).attr('id');
                var productPhotoId = elemId.split('_')[1];

                getProductImage(function (imageURI) {
                    var imgRequired = document.getElementById('imgProduct_' + productPhotoId);
                    imgRequired.src = 'data:image/jpeg;base64, ' + imageURI;

                    var photoObj = { id: 0, imageUrl: imageURI };
                    publicImageURI.push(photoObj);
                });
            });
        }
    }
    else {
        var divEditProductPhotos = document.getElementById('divEditProductPhotos');
        divEditProductPhotos.innerHTML = '';

        for (var index = 0; index < ProductImages.length; index++) {
            var photoObj = { id: ProductImages[index].Id, imageUrl: '' };
            publicImageURI.push(photoObj);
        }

        if (ProductImages.length < noOfElements) {
            for (var index = ProductImages.length; index <= parseInt(noOfElements - ProductImages.length) ; index++) {
                var photoObj = { id: parseInt(index + 1), imageUrl: '' };
                publicImageURI.push(photoObj);
            }
        }

        for (var index = 0; index < noOfElements; index++) {
            var divCol = document.createElement('div');
            var divImgContent = document.createElement('div');
            var linkOverlay = document.createElement('a');
            var iElement = document.createElement('i');
            var imgProduct = document.createElement('img');

            divCol.className = 'col-25';
            divImgContent.className = 'imgContent';
            linkOverlay.className = 'delOverlay';
            iElement.className = 'icon ion-ios-reverse-camera-outline';
            imgProduct.setAttribute('src', 'img/no-product.png');

            var photoIndex = ProductImages.indexOf(ProductImages[index]);
            var reqProductPhoto;

            if (photoIndex > -1) {
                reqProductPhoto = ProductImages[photoIndex];
                imgProduct.setAttribute('id', 'imgEditProduct_' + reqProductPhoto.Id);
                linkOverlay.setAttribute('id', 'linkEditProductPhoto_' + reqProductPhoto.Id);
            }
            else {
                imgProduct.setAttribute('id', 'imgEditProduct_' + parseInt(index + 1));
                linkOverlay.setAttribute('id', 'linkEditProductPhoto_' + parseInt(index + 1));
            }

            linkOverlay.appendChild(iElement);
            divImgContent.appendChild(linkOverlay);
            divImgContent.appendChild(imgProduct);
            divCol.appendChild(divImgContent);
            divEditProductPhotos.appendChild(divCol);

            if (photoIndex > -1) {
                $('#linkEditProductPhoto_' + reqProductPhoto.Id).unbind().on('click', function () {
                    
                    var elemId = $(this).attr('id');
                    var productPhotoId = elemId.split('_')[1];
                    UploadProductPhotos(productPhotoId, ProductImages);
                });
            }
            else {
                $('#linkEditProductPhoto_' + parseInt(index + 1)).unbind().on('click', function () {
                    
                    var elemId = $(this).attr('id');
                    var productPhotoId = elemId.split('_')[1];
                    UploadProductPhotos(productPhotoId, ProductImages);
                });
            }
        }

        for (var index = 0; index < ProductImages.length; index++) {
            var reqProductPhoto = ProductImages[index];
            var imgRequired = document.getElementById('imgEditProduct_' + reqProductPhoto.Id);
            imgRequired.src = hostUrl + '/Uploads/' + reqProductPhoto.ImageUrl;

        }

    }
}

function DrawChatMessages(newMessages, callBack) {
    var chatPageContent = document.getElementById('chatPageContent');
    var divChatMessages = document.getElementById('divChatMessages');
    divChatMessages.innerHTML = '';

    if (localStorage.getItem('userLoggedIn')) {
        var user = JSON.parse(localStorage.getItem('userLoggedIn'));

        for (var m = 0; m < newMessages.length; m++) {
            var liMessage = document.createElement('li');
            var divMessage = document.createElement('div');
            var divArrow = document.createElement('div');
            var divMessageText = document.createElement('div');
            var divMessageTime = document.createElement('div');
            var divMessageAvatar = document.createElement('div');
            var imgMessageAvatar = document.createElement('img');

            if (newMessages[m].senderId === user.id) {
                divMessage.className = 'w-clearfix column-right chat right';
                divArrow.className = 'arrow-globe right';
                divMessageText.className = 'chat-text right';
                divMessageTime.className = 'chat-time right';
            }
            else {
                divMessage.className = 'column-right chat';
                divArrow.className = 'arrow-globe';
                divMessageText.className = 'chat-text';
                divMessageTime.className = 'chat-time';
                divMessageAvatar.className = 'chatavatar';
                if (typeof newMessages[m].senderPhoto != 'undefined' && newMessages[m].senderPhoto != null && newMessages[m].senderPhoto != '' && newMessages[m].senderPhoto != ' ') {
                    imgMessageAvatar.src = hostUrl + 'Uploads/' + newMessages[m].senderPhoto;
                }
                else {
                    imgMessageAvatar.src = 'img/profile.jpg';
                }
            }

            if (typeof newMessages[m].messageContent != 'undefined' && newMessages[m].messageContent != null &&
                newMessages[m].messageContent != '' && newMessages[m].messageContent != ' ') {
                divMessageText.innerHTML = newMessages[m].messageContent;
            }
            else {
                divMessageText.innerHTML = 'مرحبا بك , أريد التحدث معك .';
            }


            var timestamp = newMessages[m].sentDate;
            var date_test = new Date(timestamp.split(".")[0].replace(/-/g, "/").replace(/T/g, " "));
            var millisecs = date_test.getTime() + parseInt("1" + timestamp.split(".")[1]);

            divMessageTime.innerHTML = timestamp.split(".")[0].replace(/-/g, "/").replace(/T/g, " ");

            divMessageAvatar.appendChild(imgMessageAvatar);
            divMessage.appendChild(divArrow);
            divMessage.appendChild(divMessageText);
            divMessage.appendChild(divMessageTime);

            if (newMessages[m].senderId != user.id) {
                liMessage.appendChild(divMessageAvatar);
            }

            liMessage.appendChild(divMessage);
            divChatMessages.appendChild(liMessage);

        }
        if (isChatDrawn == false) {
            isChatDrawn = true;
            divChatMessages.scrollTop = divChatMessages.scrollHeight;
            chatPageContent.scrollTop = chatPageContent.scrollHeight;
        }
        callBack(true);
    }
    else {
        callBack(false);
    }
}

function DrawAllChats(chats, reqProduct, startIndex, itemsPerLoad) {
    var ulChats = document.getElementById('ulChats');
    //ulChats.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    if (maxNumber > chats.length) {
        maxNumber = chats.length;
    }

    for (var m = 0; m < chats.length; m++) {
        var liItemContent = document.createElement('li');
        var linkChat = document.createElement('a');
        var divItemMedia = document.createElement('div');
        var imgItemMedia = document.createElement('img');
        var divItemInner = document.createElement('div');
        var divItemTitle = document.createElement('div');
        var pLastConversation = document.createElement('p');

        liItemContent.className = 'item-content';
        divItemMedia.className = 'item-media';
        if (typeof chats[m].receiverPhotoUrl != 'undefined' && chats[m].receiverPhotoUrl != null && chats[m].receiverPhotoUrl != '' && chats[m].receiverPhotoUrl != ' ') {
            imgItemMedia.src = hostUrl + 'Uploads/' + chats[m].receiverPhotoUrl;
        }
        else {
            imgItemMedia.src = 'img/profile.jpg';
        }
        divItemInner.className = 'item-inner';
        divItemTitle.className = 'item-title chatTitle';
        pLastConversation.className = 'pLastConversation';

        divItemTitle.innerHTML = chats[m].receiverName;

        var timestamp = chats[m].sentDate;
        var date_test = new Date(timestamp.split(".")[0].replace(/-/g, "/").replace(/T/g, " "));
        var millisecs = date_test.getTime() + parseInt("1" + timestamp.split(".")[1]);

        pLastConversation.innerHTML = 'اخر محادثة ' + timestamp.split(".")[0].replace(/-/g, "/").replace(/T/g, " ");

        divItemMedia.appendChild(imgItemMedia);
        divItemInner.appendChild(divItemTitle);
        divItemInner.appendChild(pLastConversation);

        linkChat.setAttribute('id', 'linkChat_' + chats[m].id);

        linkChat.appendChild(divItemMedia);
        linkChat.appendChild(divItemInner);
        liItemContent.appendChild(linkChat);

        ulChats.appendChild(liItemContent);

        $('#linkChat_' + chats[m].id).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var chatId = elemId.split('_')[1];
            var reqChat;
            for (var i = 0; i < chats.length; ++i) {
                if (chats[i].id == chatId) {
                    reqChat = chats[i];
                    break;
                }
            }
            GetChatInformation('allChats', reqChat.id, function (request) {
                if (request != null) {
                    mainView.router.loadPage({ pageName: 'chat', query: { fromPage: 'allChats', chatId: reqChat.id, chatHeader: request } });
                }
            });
        });
    }
}

function DrawNotifications(notifications, startIndex, itemsPerLoad) {
    var divNotifications = document.getElementById('divNotifications');
    //divNotifications.innerHTML = '';

    var maxNumber = parseInt(startIndex + itemsPerLoad);

    if (maxNumber > notifications.length) {
        maxNumber = notifications.length;
    }

    for (var elemIndex = 0; elemIndex < notifications.length; elemIndex++) {
        var divCol = document.createElement('div');
        var linkItem = document.createElement('a');
        var divCard = document.createElement('div');
        var pNotification = document.createElement('div');

        if (notifications[elemIndex].notified == true) {
            divCard.className = 'card notificationRead';
        }
        else {
            divCard.className = 'card notificationUnRead';
        }

        divCard.setAttribute('id', 'divNotificationCard_' + notifications[elemIndex].id);
        pNotification.innerHTML = notifications[elemIndex].content;
        linkItem.setAttribute('id', 'linkNotification_' + notifications[elemIndex].id);
        divCol.className = 'col-100 divColNotification';

        divCard.appendChild(pNotification);
        linkItem.appendChild(divCard);
        divCol.appendChild(linkItem);

        divNotifications.appendChild(divCol);

        $('#linkNotification_' + notifications[elemIndex].id).unbind().on('click', function () {
            
            var elemId = $(this).attr('id');
            var notificationId = elemId.split('_')[1];
            var reqElement;
            for (var i = 0; i < notifications.length; ++i) {
                if (notifications[i].id == notificationId) {
                    reqElement = notifications[i];
                    break;
                }
            }

            var requestId = reqElement.chatRequestId;

            if (reqElement.notified == false) {
                var params = {};
                CallService('notification', "POST", "api/Notification/MarkAsRead/" + reqElement.id, params, function (res) {
                    if (res != null && res == 1) {
                        $('#divNotificationCard_' + reqElement.id).removeClass('notificationUnRead');
                        $('#divNotificationCard_' + reqElement.id).addClass('notificationRead');
                        GetChatRequestById('notification', requestId, function (res) {
                            if (res != null) {
                                GetChatInformation('notification', res, function (request) {
                                    if (request != null) {
                                        mainView.router.loadPage({ pageName: 'chat', query: { fromPage: 'notification', chatId: request.id, chatHeader: request } });
                                    }
                                });
                            }
                        });
                    }
                });
            }
            else {
                GetChatRequestById('notification', requestId, function (res) {
                    if (res != null) {
                        GetChatInformation('notification', res, function (request) {
                            if (request != null) {
                                mainView.router.loadPage({ pageName: 'chat', query: { fromPage: 'notification', chatId: request.id, chatHeader: request } });
                            }
                        });
                    }
                });
            }
        });
    }
}

function GoToContactProductOwnerOptions(reqProduct) {
    if (initContactProductOwner == true) {
        initContactProductOwner = false;
        localStorage.setItem('reqProduct', JSON.stringify(reqProduct));

        $('#linkCallOwner').unbind().on('click', function () {
            
            var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
            myApp.closeModal('.popover-contactWithOwner');

            if (localStorage.getItem('Visitor') == "false") {
                var number = storedProduct.phoneNumber;
                if (typeof number != 'undefined' && number != null && number != '' && number != ' ') {
                    window.plugins.CallNumber.callNumber(function () {
                        myApp.closeModal('.popover-contactWithOwner');
                    }, function () {
                        myApp.alert('غير قادر علي الإتصال بهذا الهاتف .');
                    }, number, true);
                }
                else {
                    myApp.alert('مالك الإعلان لا يوجد له رقم جوال حتي الآن', 'خطأ');
                }
            }
            else {
                myApp.alert('لا يمكنك الإتصال بمالك الإعلان ...من فضلك سجل حسابك أولا', 'خطأ');
            }

        });

        $('#linkMessageOwner').unbind().on('click', function () {
            
            var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
            myApp.closeModal('.popover-contactWithOwner');

            if (localStorage.getItem('Visitor') == "false" && localStorage.getItem('userLoggedIn')) {
                var user = JSON.parse(localStorage.getItem('userLoggedIn'));
                var number = storedProduct.phoneNumber;
                var userEmail = null;
                var userNumber = user.phoneNumber;

                if (typeof number != 'undefined' && number != null && number != '' && number != ' ') {

                    if (typeof user.email != 'undefined' && user.email != null && user.email != '' && user.email != ' ') {
                        userEmail = user.email;
                    }
                    else {
                        var storedMail = localStorage.getItem('socialEmail');
                        if (typeof storedMail != 'undefined' && storedMail != null && storedMail != '' && storedMail != ' ') {
                            userEmail = storedMail;
                        }
                    }

                    var messageContent = 'مرحبا بك,' + ' \n ';
                    messageContent += 'أريد التحدث معك بخصوص الإعلان' + ' \n\n ';
                    messageContent += 'بياناتي :' + ' \n ';
                    if (userEmail != null && userEmail != '' && userEmail != ' ') {
                        messageContent += 'البريد الإليكتروني : ' + userEmail + ' \n\n ';
                    }
                    if (userNumber != null && userNumber != '' && userNumber != ' ') {
                        messageContent += 'الجوال : ' + userNumber + ' \n\n ';
                    }
                    var message = messageContent;

                    var options = {
                        replaceLineBreaks: true,
                        android: {
                            intent: 'INTENT'
                        }
                    };

                    sms.send(number, message, options, function () {
                    }, function (e) {
                        var error = e;
                        myApp.alert('خطأ في إرسال الرسالة .', 'خطأ');
                    });
                }
                else {
                    myApp.alert('مالك الإعلان لا يوجد له رقم جوال حتي الآن', 'خطأ');
                }
            }
            else {
                myApp.alert('لا يمكنك إرسال رسالة لمالك الإعلان ...من فضلك سجل حسابك أولا', 'خطأ');
            }

        });

        $('#linkChatWithOwner').unbind().on('click', function () {
            
            myApp.closeModal('.popover-contactWithOwner');
            if (localStorage.getItem('Visitor') == "false" && localStorage.getItem('userLoggedIn')) {
                var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));

                var params = {
                    'AdvertismentId': storedProduct.id
                };

                CallService('productDetails', "POST", "api/chat/AddRequest", params, function (res) {
                    if (res != null) {
                        localStorage.setItem('chatId', res);
                        GetChatInformation('productDetails', res, function (request) {
                            if (request != null) {
                                localStorage.setItem('chatHeader', JSON.stringify(request));
                                var ChatId = localStorage.getItem('chatId');
                                mainView.router.loadPage({ pageName: 'chat', query: { fromPage: 'productDetails', chatId: ChatId, chatHeader: request } });
                            }
                        });
                    }
                });
            }
            else {
                myApp.alert('لا يمكنك محادثة صاحب الإعلان ...من فضلك سجل حسابك أولا .', 'خطأ', function () { });
            }
            
        });

    }
}

function GoToReportProduct(reqProduct) {
    if (initReportProductPopup == true) {
        initReportProductPopup = false;
        $('#txtReportProductMessage').val('');
        
        $('#linkReportProduct').unbind().on('click', function () {
            
            var message = $('#txtReportProductMessage').val().trim();

            if (message != '' && message != ' ') {
                var params = {
                    'AdvertismentId': reqProduct.id,
                    'Message': message
                };

                myApp.closeModal('.popover-report');

                CallService('productDetails', "POST", "api/complaint/save", params, function (res) {
                    if (res != null && res > 0) {
                        isReport = true;
                        $("#btnReportProductDetails").addClass('pan');
                        myApp.alert('تم الإبلاغ عن هذا المنتج بنجاح , شكرا لإهتمامك .', 'نجاح', function () { });
                    }
                    else {
                        myApp.alert('خطأ في الإبلاغ عن هذا المنتج', 'خطأ', function () {
                            $("#btnReportProductDetails").removeClass('pan');
                        });
                    }
                });
            }
            else {
                myApp.alert('من فضلك أدخل نص الشكوي .', 'خطأ', function () {});
            }
            
        });

        $('#linkCloseReportProduct').unbind().on('click', function () {
            
            $('#txtReportProductMessage').val('');
            myApp.closeModal('.popover-report');
        });

    }
}

function GoToLoginPage(page) {
    if (typeof page != 'undefined') {

        $("#buttonLogin").unbind().on("click", function () {
            var loginEmail = $("#LoginEmail").val(), loginPass = $("#LoginPassword").val();

            loginEmail = loginEmail.trim();

            if ((loginEmail != '' && loginEmail != ' ' && loginEmail != null) && loginPass != null) {

                GetToken('login', "POST", "token", loginEmail, loginPass, function (res) {
                    if (res != null) {
                        localStorage.setItem('appToken', res.access_token);
                        localStorage.setItem('USName', res.userName);
                        localStorage.setItem('refreshToken', res.refresh_token);
                        localStorage.setItem('Visitor', false);
                        localStorage.setItem('loginUsingSocial', false);
                        CallService('login', "POST", "api/User/GetUserInfo", '', function (res) {
                            if (res != null) {
                                localStorage.setItem('userLoggedIn', JSON.stringify(res));
                                if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                    localStorage.setItem('usrPhoto', res.photoUrl);
                                }
                                else {
                                    localStorage.removeItem('usrPhoto');
                                }

                                mainView.router.loadPage({ pageName: 'categories' });

                            }
                        });
                    }
                    else {
                        myApp.alert('لا يمكن التحقق من البيانات .', 'خطأ', function () { });
                    }
                });

            }
            else {
                myApp.alert('من فضلك أدخل إسم الدخول وكلمة المرور', 'تنبيه', function () { });
            }
        });

        $('#btnSignUp').unbind().on("click", function () {
            mainView.router.loadPage({ pageName: 'signup' });
        });

        $('#btnForgetPassword').unbind().on("click", function () {
            mainView.router.loadPage({ pageName: 'forgetPass' });
        });

        $('#btnGoToHomeDirectly').unbind().on("click", function () {
            localStorage.setItem('Visitor', true);
            mainView.router.loadPage({ pageName: 'categories' });
        });

        // login with FB
        $("#buttonFB").unbind().on("click", function () {
            var fbLoginFailed = function (error) {
                console.log(error);
                facebookConnectPlugin.login(["public_profile", "email", "user_about_me"], function (userData) {
                    var userFullName = '';
                    if (userData.authResponse) {
                        var accessToken = userData.authResponse.accessToken;
                        facebookConnectPlugin.api(userData.authResponse.userID + "/?fields=id,email,first_name,last_name", ["public_profile"],
                       function (result) {
                           userFullName = result.first_name + ' ' + result.last_name;
                           var registerObj = {};
                           if (localStorage.getItem('FacebookObject')) {
                               registerObj = JSON.parse(localStorage.getItem('FacebookObject'));
                           }
                           else {
                               registerObj = {
                                   "Provider": "Facebook",
                                   "userId": userData.authResponse.userID,
                                   "name": userFullName,
                                   "ExternalAccessToken": accessToken
                               };
                           }
                           CallService('login', "POST", "api/Account/RegisterExternal", registerObj, function (res) {
                               if (res != null) {
                                   localStorage.setItem('USName', res.userName);
                                   localStorage.setItem('appToken', res.access_token);
                                   localStorage.setItem('Visitor', false);
                                   localStorage.setItem('loginUsingSocial', true);
                                   localStorage.setItem('FacebookObject', JSON.stringify(registerObj));
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
                        function (error) {
                            console.log('ERROR:' + error);
                        });
                    }
                }, function () { });
            }

            var fbLoginSuccess = function (userData) {
                if (userData.authResponse) {

                    var accessToken = userData.authResponse.accessToken;
                    var userFullName = '';

                    facebookConnectPlugin.api(userData.authResponse.userID + "/?fields=id,email,first_name,last_name", ["public_profile"],
                        function (result) {
                            localStorage.setItem('socialEmail', result.email);
                            userFullName = result.first_name + ' ' + result.last_name;
                            var registerObj = {};
                            if (localStorage.getItem('FacebookObject')) {
                                registerObj = JSON.parse(localStorage.getItem('FacebookObject'));
                            }
                            else {
                                registerObj = {
                                    "Provider": "Facebook",
                                    "userId": userData.authResponse.userID,
                                    "name": userFullName,
                                    "ExternalAccessToken": accessToken
                                };
                            }
                            CallService('login', "POST", "api/Account/RegisterExternal", registerObj, function (res) {
                                if (res != null) {
                                    localStorage.setItem('USName', res.userName);
                                    localStorage.setItem('appToken', res.access_token);
                                    localStorage.setItem('Visitor', false);
                                    localStorage.setItem('loginUsingSocial', true);
                                    localStorage.setItem('FacebookObject', JSON.stringify(registerObj));
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
                        function (error) {
                            console.log('ERROR:' + error);
                        });

                    
                }
            }

            facebookConnectPlugin.login(["public_profile", "email"], fbLoginSuccess, fbLoginFailed);

        });

        // login with G+    
        $("#buttonGoogle").unbind().on("click", function () {
            callGoogle();
        });

        $("#buttonTwitter").unbind().on("click", function () {
            TwitterConnect.login(function (data) {
                var accessToken = data.token;
                var userFullName = '';

                TwitterConnect.showUser(function (result) {
                    var userFullName = result.name;
                    localStorage.setItem('socialEmail', result.email);
                    var registerObj = {};
                    if (localStorage.getItem('TwitterObject')) {
                        registerObj = JSON.parse(localStorage.getItem('TwitterObject'));
                    }
                    else {
                        registerObj = {
                            "Provider": "Twitter",
                            "userId": data.userId,
                            "name": userFullName,
                            "ExternalAccessToken": accessToken
                        };
                    }
                    CallService('login', "POST", "api/Account/RegisterExternal", registerObj, function (res) {
                        if (res != null) {
                            localStorage.setItem('USName', res.userName);
                            localStorage.setItem('appToken', res.access_token);
                            localStorage.setItem('Visitor', false);
                            localStorage.setItem('loginUsingSocial', true);
                            localStorage.setItem('TwitterObject', JSON.stringify(registerObj));
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
                    }, function (error) {
                        console.log('Error logging in');
                        console.log(error);
                    });

                }, function (error) {
                    showNotification('Error retrieving user profile');
                });
                
                

            }, function (error) {
                console.log('Error in Login');
            });
        });

    }
}

function GoToSplashPage(page) {
    if (typeof page != 'undefined') {
        if (initSplashPage == true) {
            initSplashPage = false;
        }
    }
}

function GoToForgetPasswordPage(page) {
    if (typeof page != 'undefined') {
        $('#txtForgetPasswordEmail').val('');

        if (initForgetPassword == true) {
            initForgetPassword = false;

            $('#btnBackFromForgetPassword').unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'login' });
            });

            $('#btnSendEmail').unbind().on("click", function () {
                FValidation.ValidateAll('forgetPassword', function (valid) {
                    if (valid == true) {
                        var params = {
                            'email': $('#txtForgetPasswordEmail').val()
                        }

                        CallService('forgetPass', "POST", "api/Account/ForgetPassword", params, function (res) {
                            if (res != null) {
                                localStorage.setItem('confirmationMail', $('#txtForgetPasswordEmail').val());
                                myApp.alert('تم إرسال الكود لبريدك الإليكتروني بنجاح .', 'نجاح', function () { mainView.router.loadPage({ pageName: 'resetPassword' }); });
                            }
                            else {
                                myApp.alert('خطأ في إرسال الكود لبريدك الإليكتروني .', 'خطأ', function () { });
                            }
                        });
                    }
                });
            });
        }
    }
}

function GoToResetPasswordPage(page) {
    if (typeof page != 'undefined') {
        $('#txtResetCode').val('');
        $('#txtResetPassword').val('');
        $('#txtResetConfirmPassword').val('');

        if (initResetPasswordPage == true) {
            initResetPasswordPage = false;

            $('#btnResetPasswordBackToHome').unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'login' });
            });

            $('#btnResetPassword').unbind().on("click", function () {
                FValidation.ValidateAll('resetPassword', function (valid) {
                    if (valid == true) {
                        var params = {
                            'code': $('#txtResetCode').val(),
                            'email': localStorage.getItem('confirmationMail'),
                            'password': $('#txtResetPassword').val(),
                            'confirmPassword': $('#txtResetConfirmPassword').val()
                        }

                        CallService('resetPassword', "POST", "api/Account/ResetPassword", params, function (res) {
                            if (res != null) {
                                localStorage.removeItem('confirmationMail');
                                myApp.alert('تم تغيير كلمة المرور القديمة بنجاح .', 'نجاح', function () { mainView.router.loadPage({ pageName: 'login' }); });
                            }
                            else {
                                myApp.alert('خطأ في تغيير كلمة المرور القديمة.', 'خطأ', function () { });
                            }
                        });
                    }
                });
            });
        }
    }
}

function GoToChangePasswordPage(page) {
    if (typeof page != 'undefined') {
        $('#txtChangeOldPassword').val('');
        $('#txtChangeNewPassword').val('');
        $('#txtChangeConfirmNewPassword').val('');

        if (initChangePasswordPage == true) {
            initChangePasswordPage = false;

            $('#btnPasswordBackToHome').unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'categories' });
            });

            $('#btnChangePassword').unbind().on("click", function () {
                FValidation.ValidateAll('changePassword', function (res) {
                    if (res == true) {
                        var params = {
                            'oldPassword': $('#txtChangeOldPassword').val(),
                            'newPassword': $('#txtChangeNewPassword').val(),
                            'ConfirmPassword': $('#txtChangeConfirmNewPassword').val()
                        }

                        CallService('changePassword', "POST", "Api/Account/ChangePassword", params, function (res) {
                            if (res != null) {
                                myApp.alert('تم تعديل كلمة السر بنجاح .', 'نجاح', function () { mainView.router.loadPage({ pageName: 'categories' }); });
                            }
                            else {
                                myApp.alert('خطأ في تعديل كلمة السر.', 'خطأ', function () { });
                            }
                        });


                    }
                });
            });
        }
    }
}

function GoToSignUpPage(page) {
    ClearAllSignupControls();
    HideAllSignupControls();
    if (typeof page != 'undefined') {
        //loadSideMenuLinks();
        var UID = localStorage.getItem('UID');
        var UPhoto = localStorage.getItem('usrPhoto');
        var UName = localStorage.getItem('UName');
        var UEmail = localStorage.getItem('UEmail');
        var signUpType;

        if (!localStorage.getItem('userLoggedIn') && UID != null && UID != '') {
            $('#txtFullName').val(UName);
            $('#txtEmail').val(UEmail);
        }

        $('#liName').css('display', '');
        $('#liMobile').css('display', '');
        $('#liFullName').css('display', '');
        $('#liEmail').css('display', '');
        $('#liPassword').css('display', '');
        $('#liConfirmPassword').css('display', '');

        $('#txtFullName').val('');

        if (localStorage.getItem('userLoggedIn')) {
            var user = JSON.parse(localStorage.getItem('userLoggedIn'));
            $('#txtFullName').val(user.name);
            $('#txtMobile').val(user.phoneNumber);

            signUpType = 'editProfile';

            $('#btnGoToCode').html('تعديل');

            $('#liName').css('display', 'none');
            $('#liEmail').css('display', 'none');
            //$('#liMobile').css('display', 'none');
            $('#liPassword').css('display', 'none');
            $('#liConfirmPassword').css('display', 'none');
        }
        else {
            signUpType = 'signup';
            $('#btnGoToCode').html('تسجيل حساب جديد');
        }

        
        var mobile = $('#txtMobile').val();
        var email = $('#txtEmail').val();

        UserNameIsEmailOrPhone(email, mobile);

        if (initSignupPage == true) {
            initSignupPage = false;

            $("#btnBackToHome").unbind().on("click", function () {
                $('#txtFullName').val('');
                mainView.router.back();
            });

            $("#btnGoToCode").unbind().on("click", function () {
                if (localStorage.getItem('userLoggedIn')) {
                    var user = JSON.parse(localStorage.getItem('userLoggedIn'));
                    signUpType = 'editProfile';
                }
                else {
                    signUpType = 'signup';
                }

                FValidation.ValidateAll(signUpType, function (res) {
                    if (res == true) {
                        if (localStorage.getItem('userLoggedIn')) {
                            var params = {
                                'name': $('#txtFullName').val(),
                                'PhoneNumber': $('#txtMobile').val()
                            }
                            CallService('signup', "POST", "api/User/ChangeInfo", params, function (res1) {
                                if (res1 != null) {

                                    var userloggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));
                                    userloggedIn.name = params.name;
                                    userloggedIn.phoneNumber = params.PhoneNumber;

                                    localStorage.setItem('userLoggedIn', JSON.stringify(userloggedIn));
                                    //mainView.router.loadPage({ pageName: 'categories' });
                                    mainView.router.back();
                                }
                                else {
                                    myApp.alert('خطأ في تعديل بيانات المستخدم.', 'خطأ', function () { });
                                }
                            });
                        }
                        else {
                            var user = {
                                'userName': $('#txtName').val(),
                                'Name': $('#txtFullName').val(),
                                'Email': $('#txtEmail').val(),
                                'PhoneNumber': $('#txtMobile').val(),
                                'Password': $('#txtPassword').val(),
                                'ConfirmPassword': $('#txtConfirmPassword').val(),
                                'Role': 'User'
                            }

                            CallService('signup', "POST", "api/Account/Register", user, function (res2) {
                                if (res2 != null) {
                                    localStorage.setItem('USName', 'مستخدم');
                                    localStorage.setItem('UserID', res2);
                                    localStorage.setItem('UserEntersCode', false);
                                    myApp.alert('تم تسجيل المستخدم بنجاح .', 'نجاح', function () { mainView.router.loadPage({ pageName: 'activation' }); });
                                }
                                else {
                                    myApp.alert('خطأ في تسجيل مستخدم جديد.', 'خطأ', function () { });
                                }
                            });
                        }

                    }
                });

            });
        }
    }

}

function GoToActivationPage(page) {
    if (typeof page != 'undefined') {

        if (initActivationPage == true) {
            initActivationPage = false;

            $('#btnSendCode').unbind().on('click', function () {
                
                var txtCode = $('#txtCode').val();
                var userId = localStorage.getItem('UserID');

                CallService('activation', "POST", "api/Account/ConfirmEmail", { "userId": userId, "code": txtCode }, function (res) {
                    if (res != null) {
                        localStorage.setItem('Visitor', false);
                        localStorage.setItem('UserEntersCode', true);
                        mainView.router.loadPage({ pageName: 'login' });
                    }
                });
            });

            $('#btnReSendCode').unbind().on('click', function () {
                
                var userId = localStorage.getItem('UserID');

                CallService('activation', "POST", "api/Account/ReSendConfirmationCode/" + userId, '', function (res) {
                    if (res != null) {
                        myApp.alert('تم إعادة إرسال الكود بنجاح .', 'نجاح', function () { });
                    }
                });
            });
        }
    }
}

function GoToCategoriesPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();

        var validCategories = [];
        var validAdvertisments = [];
        var loading = false;
        var lastIndex = 4;
        var maxItems = validCategories.length;
        var itemsPerLoad = 4;
        scrollLoadsBefore = false;

        var divCategoriesInHome = document.getElementById('divCategoriesInHome');
        divCategoriesInHome.innerHTML = '';

        var params = {
            "PageNumber": 1,
            "PageSize": 4
        };

        $('#infiniteLoaderCategories img').css('display', '');
        $('#divCategoriesInHome').show();
        $('#divNotificationCategories').hide();

        CallService('categories', "POST", "api/category/GetHomeCategory", params, function (res) {
            if (res != null && res.length > 0) {
                $('#divCategoriesInHome').show();
                $('#divNotificationCategories').hide();
                
                validCategories = res;
                loading = false;
                lastIndex = 4;
                maxItems = validCategories[0].overallCount;
                itemsPerLoad = 4;
                localStorage.setItem('maxCategories', maxItems);

                var paramsAdv = {
                    "PageNumber": 1,
                    "PageSize": 4
                };

                CallService('categories', "POST", "api/advertisement/GetHomeAds", paramsAdv, function (resAdv) {
                    if (resAdv != null && resAdv.length > 0) {
                        validAdvertisments = resAdv;
                    }

                    divCategoriesInHome.innerHTML = '';
                    DrawCategoriesInHome(validCategories, validAdvertisments, 0, itemsPerLoad);
                });
            }
            else {
                $('#divCategoriesInHome').hide();
                $('#divNotificationCategories').show();
                $('#infiniteLoaderCategories img').css('display', 'none');
            }
        });


        if (initCategoriesPage == true) {
            initCategoriesPage = false;

            myApp.attachInfiniteScroll($$('#divInfiniteCategories'));

            $$('#divInfiniteCategories').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;

                    lastIndex = $$('#divCategoriesInHome div.divCategoryInHome').length;
                    maxItems = localStorage.getItem('maxCategories');

                    if (lastIndex >= maxItems || scrollLoadsBefore == true) {
                        $('#infiniteLoaderCategories img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderCategories img').css('display', '');

                    var params = {
                        'PageNumber': parseInt(parseInt(lastIndex / 4) + 1),
                        'PageSize': 4
                    }

                    CallService('categories', "POST", "api/category/GetHomeCategory", params, function (res) {
                        if (res != null && res.length > 0) {
                            var validCategories = res;
                            lastIndex = 4;
                            maxItems = validCategories[0].overallCount;
                            itemsPerLoad = 4;

                            var paramsAdv = {
                                "PageNumber": parseInt(parseInt(lastIndex / 4) + 1),
                                "PageSize": 4
                            };

                            CallService('categories', "POST", "api/advertisement/GetHomeAds", paramsAdv, function (resAdv) {
                                if (resAdv != null && resAdv.length > 0) {
                                    validAdvertisments = resAdv;
                                }
                                DrawCategoriesInHome(validCategories, validAdvertisments, 0, itemsPerLoad);
                                lastIndex = $$('#divCategoriesInHome div.divCategoryInHome').length;
                            });
                        }
                        else {
                            $('#divCategoriesInHome').hide();
                            $('#divNotificationCategories').show();
                            $('#infiniteLoaderCategories img').css('display', 'none');
                        }
                    });
                }, 1000);
            });

            $("#linkAddProductInCategories").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkCategoriesSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });
        }
    }
}

function GoToSearchPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        $('#txtSearch').val('');

        if (initSearch == true) {
            initSearch = false;

            $("#linkGoSearch").unbind().on("click", function () {
                var searchArray = [];
                var selectedProductName = $('#txtSearch').val();

                if (selectedProductName != null) {
                    searchArray = [{
                        productName: selectedProductName
                    }];
                }

                $('#txtSearch').val('');
                linkBackSearch = true;
                mainView.router.loadPage({ pageName: 'searchResults', query: { searchParams: searchArray } });
            });
        }
    }

}

function GoToProductsPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        var reqCategory;

        if (page === 'refresh') {
            var storedCategory = JSON.parse(localStorage.getItem('reqCategory'));
            reqCategory = storedCategory.id;
        }
        else {
            if (typeof page.query.category != 'undefined') {
                if (typeof page.query.category.name != 'undefined') {
                    reqCategory = page.query.category.id;
                }
                else {
                    reqCategory = page.query.category;
                }
            }
        }

        var divProductsInCategory = document.getElementById('divProductsInCategory');
        divProductsInCategory.innerHTML = '';

        var validProducts = [];
        var loading = false;
        var lastIndex = 6;
        var maxItems = validProducts.length;
        var itemsPerLoad = 6;
        scrollLoadsBefore = false;

        var params = {};
        var methodName = '';

        $('#divProductsInCategory').show();
        $('#divNotificationProductsInCategory').hide();
        $('#infiniteLoaderProducts img').css('display', '');
        $('#linkImgHeaderProducts').hide();

        params = {
            'PageNumber': 1,
            'PageSize': 6,
            'CategoryId': reqCategory
        }

        localStorage.setItem('catId', reqCategory);

        methodName = 'api/advertisement/GeAdsByCategoryId';

        CallService('products', "POST", methodName, params, function (res) {
            if (res != null && res.length > 0) {
                $('#divProductsInCategory').show();
                $('#divNotificationProductsInCategory').hide();
                $('#infiniteLoaderProducts img').css('display', '');
                $('#linkImgHeaderProducts').show();


                var rand = GetRandomElemFromList(res);

                imgHeaderProducts.setAttribute('onerror', 'replaceURL(this,"product")');

                if (typeof rand.imageUrl != 'undefined' && rand.imageUrl != null && rand.imageUrl != '' && rand.imageUrl != ' ') {
                    imgHeaderProducts.src = hostUrl + 'Uploads/' + rand.imageUrl;
                }
                else {
                    imgHeaderProducts.src = 'img/no-product.png';
                }

                localStorage.setItem('randProduct', JSON.stringify(rand));


                var validProducts = res;

                loading = false;
                lastIndex = 6;
                maxItems = validProducts[0].overAllCount;
                itemsPerLoad = 6;
                localStorage.setItem('maxProducts', maxItems);

                DrawProductsInCategory(validProducts, 0, itemsPerLoad);

                if (validProducts.length <= itemsPerLoad) {
                    $('#infiniteLoaderProducts img').css('display', 'none');
                }
            }
            else {
                $('#divProductsInCategory').hide();
                $('#divNotificationProductsInCategory').show();
                $('#infiniteLoaderProducts img').css('display', 'none');
                $('#linkImgHeaderProducts').hide();
            }
        });

        if (initProductsPage == true) {
            initProductsPage = false;

            myApp.attachInfiniteScroll($$('#divInfiniteProducts'));

            $$('#divInfiniteProducts').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;
                    lastIndex = $$('#divProductsInCategory div.divProductInCategory').length;
                    maxItems = localStorage.getItem('maxProducts');
                    if (lastIndex >= maxItems || scrollLoadsBefore == true) {
                        $('#infiniteLoaderProducts img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderProducts img').css('display', '');

                    var params = {};

                    var storedId = localStorage.getItem('catId');

                    params = {
                        'PageNumber': parseInt(parseInt(lastIndex / 6) + 1),
                        'PageSize': 6,
                        'CategoryId': storedId
                    }

                    methodName = 'api/advertisement/GeAdsByCategoryId';

                    CallService('products', "POST", methodName, params, function (res1) {
                        if (res1 != null && res1.length > 0) {
                            var validProducts = res1;

                            loading = false;
                            lastIndex = 6;
                            maxItems = validProducts[0].overAllCount;
                            itemsPerLoad = 6;
                            DrawProductsInCategory(validProducts, 0, itemsPerLoad);
                            lastIndex = $$('#divProductsInCategory div.divProductInCategory').length;
                        }
                        else {
                            $('#divProductsInCategory').hide();
                            $('#divNotificationProductsInCategory').show();
                            $('#infiniteLoaderProducts img').css('display', 'none');
                            $('#linkImgHeaderProducts').hide();
                        }
                    });
                }, 1000);
            });


            $("#linkAddProductInProducts").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkProductsSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $('#linkBackProducts').unbind().on("click", function () {
                localStorage.setItem('maxProducts', 0);
                localStorage.removeItem('randProduct');
                if (mainView.history.length == 1 && '#'+currentPage == mainView.history[0])
                {
                    mainView.router.loadPage({ pageName: 'categories' });
                }
                else {
                    mainView.router.back();
                }
            });

            $('#linkImgHeaderProducts').unbind().on("click", function () {
                var storedRandProduct = JSON.parse(localStorage.getItem('randProduct'));
                mainView.router.loadPage({ pageName: 'productDetails', query: { product: storedRandProduct, fromPage: 'products' } });
            });
        }
    }
}

function GoToSearchResultsPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        var reqCategory;
        var searchParams;

        if (page === 'refresh') {
            var storedCategory = JSON.parse(localStorage.getItem('reqCategory'));
            reqCategory = storedCategory.id;
            searchParams = [];
        }
        else {
            searchParams = page.query.searchParams;
        }

        var divSearchResults = document.getElementById('divSearchResults');
        divSearchResults.innerHTML = '';

        var validProducts = [];
        var loading = false;
        var lastIndex = 6;
        var maxItems = validProducts.length;
        var itemsPerLoad = 6;
        scrollLoadsBefore = false;

        var params = {};
        var methodName = '';

        $('#divSearchResults').show();
        $('#divNotificationSearchResults').hide();
        $('#infiniteLoaderSearchResults img').css('display', '');
        $('#linkImgHeaderSearchResults').hide();

        var searchObject = searchParams[0];
        localStorage.setItem('searchObj', JSON.stringify(searchObject));
        params = {
            'PageNumber': 1,
            'PageSize': 6,
            'FIlter': searchObject.productName
        }

        methodName = 'api/advertisement/Search';

        CallService('searchResults', "POST", methodName, params, function (res) {
            if (res != null && res.length > 0) {
                $('#divSearchResults').show();
                $('#divNotificationSearchResults').hide();
                $('#infiniteLoaderSearchResults img').css('display', '');
                $('#linkImgHeaderSearchResults').show();

                var rand = GetRandomElemFromList(res);

                imgHeaderSearchResults.setAttribute('onerror', 'replaceURL(this,"product")');

                if (typeof rand.imageUrl != 'undefined' && rand.imageUrl != null && rand.imageUrl != '' && rand.imageUrl != ' ') {
                    imgHeaderSearchResults.src = hostUrl + 'Uploads/' + rand.imageUrl;
                }
                else {
                    imgHeaderSearchResults.src = 'img/no-product.png';
                }

                localStorage.setItem('randProduct', JSON.stringify(rand));

                var validProducts = res;

                loading = false;
                lastIndex = 6;
                maxItems = validProducts[0].overAllCount;
                itemsPerLoad = 6;
                localStorage.setItem('maxSearchResults', maxItems);

                DrawSearchResults(validProducts, 0, itemsPerLoad);

                if (validProducts.length <= itemsPerLoad) {
                    $('#infiniteLoaderSearchResults img').css('display', 'none');
                }
            }
            else {
                $('#divSearchResults').hide();
                $('#divNotificationSearchResults').show();
                $('#infiniteLoaderSearchResults img').css('display', 'none');
                $('#linkImgHeaderSearchResults').hide();
            }
        });

        if (initSearchResults == true) {
            initSearchResults = false;

            myApp.attachInfiniteScroll($$('#divInfiniteSearchResults'));

            $$('#divInfiniteSearchResults').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;
                    lastIndex = $$('#divSearchResults div.divSearchResult').length;
                    maxItems = localStorage.getItem('maxSearchResults');
                    if (lastIndex >= maxItems || scrollLoadsBefore == true) {
                        $('#infiniteLoaderSearchResults img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderSearchResults img').css('display', '');

                    var params = {};

                    var storedSearchObj = JSON.parse(localStorage.getItem('searchObj'));
                    params = {
                        'PageNumber': parseInt(parseInt(lastIndex / 6) + 1),
                        'PageSize': 6,
                        'FIlter': storedSearchObj.productName
                    }

                    methodName = 'api/advertisement/Search';

                    CallService('searchResults', "POST", methodName, params, function (res1) {
                        if (res1 != null && res1.length > 0) {
                            var validProducts = res1;

                            loading = false;
                            lastIndex = 6;
                            maxItems = validProducts[0].overAllCount;
                            itemsPerLoad = 6;
                            DrawSearchResults(validProducts, 0, itemsPerLoad);
                            lastIndex = $$('#divSearchResults div.divSearchResult').length;
                        }
                        else {
                            $('#divSearchResults').hide();
                            $('#divNotificationSearchResults').show();
                            $('#infiniteLoaderSearchResults img').css('display', 'none');
                            $('#linkImgHeaderSearchResults').hide();
                        }
                    });
                }, 1000);
            });


            $("#linkAddProductInSearchResults").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkResultsSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $('#linkBackSearchResults').unbind().on("click", function () {
                localStorage.setItem('maxSearchResults', 0);
                localStorage.removeItem('randProduct');
                mainView.router.back();
            });

            $("#linkImgHeaderSearchResults").unbind().on("click", function () {
                var storedRandProduct = JSON.parse(localStorage.getItem('randProduct'));
                mainView.router.loadPage({ pageName: 'productDetails', query: { product: storedRandProduct, fromPage: 'searchResults' } });
            });
        }

    }
}

function GoToProductDetails(page, FromPage, Product) {
    if (typeof page != 'undefined') {
        var reqProduct;
        var fromPage;
        var productId = 0;

        if (page === 'refresh') {
            reqProduct = Product;
            fromPage = FromPage;
            productId = Product.id;
        }
        else if (page.query.fromPage == "favourite") {
            reqProduct = page.query.product;
            fromPage = page.query.fromPage;
            productId = reqProduct.advertismentId;
        }
        else {
            reqProduct = page.query.product;
            fromPage = page.query.fromPage;
            productId = reqProduct.id;
        }

        var imgMainProductImage = document.getElementById('imgMainProductImage');
        imgMainProductImage.src = 'img/no-product.png';

        $("#btnChats").hide();
        if (localStorage.getItem('USName')) {
            $("#btnChats").show();
        }

        $('#ulProductDetailsComments').show();
        $('#divNotificationProductDetails').hide();
        $('#infiniteLoaderProductDetails img').css('display', '');

        var allComments = [];
        var loading = false;
        var lastIndex = 4;
        var maxFilteredItems = allComments.length;
        var itemsPerLoad = 4;
        scrollLoadsBefore = false;

        var ulProductDetailsComments = document.getElementById('ulProductDetailsComments');
        ulProductDetailsComments.innerHTML = '';

        CallService('productDetails', "GET", "api/advertisement/" + productId, '', function (res) {
            if (res != null) {
                localStorage.setItem('reqProduct', JSON.stringify(res));

                var pNotificationStatus = document.getElementById('pNotificationStatus');
                if (res.isPaided && res.isExpired) {
                    pNotificationStatus.innerHTML = 'لم يتم الدفع حتي الآن';
                }
                else if (res.isPaided && !res.isExpired) {
                    pNotificationStatus.innerHTML = 'اعلان مدفوع';
                }
                else if (!res.isPaided) {
                    pNotificationStatus.innerHTML = 'اعلان مجاني';
                }

                DrawProductDetails(res);

                var params = {
                    'PageNumber': 1,
                    'PageSize': 4,
                    'Id': res.id
                };

                CallService('productDetails', "POST", "api/comment/GetAll", params, function (result) {
                    if (result != null && result.length > 0) {
                        allComments = result;
                        $('#ulProductDetailsComments').show();
                        $('#divNotificationProductDetails').hide();
                        loading = false;
                        lastIndex = 4;
                        maxFilteredItems = allComments[0].overAllCount;
                        localStorage.setItem('maxComments', maxFilteredItems);
                        itemsPerLoad = 4;
                        DrawComments(res, allComments, 'ulProductDetailsComments', 'productDetails', 0, itemsPerLoad);
                        $('#infiniteLoaderProductDetails img').css('display', 'none');
                    }
                    else {
                        $('#ulProductDetailsComments').hide();
                        $('#divNotificationProductDetails').show();
                        $('#infiniteLoaderProductDetails img').css('display', 'none');
                    }
                });
            }
        });

        if (initProductDetailsPage == true) {
            initProductDetailsPage = false;

            myApp.attachInfiniteScroll($$('#divInfiniteProductDetails'));

            $$('#divInfiniteProductDetails').on('infinite', function () {
                if (loading) return;
                loading = true;

                var storedMax = localStorage.getItem('maxComments');
                lastIndex = $$('#ulProductDetailsComments li.liComment').length;
                if (lastIndex >= storedMax || scrollLoadsBefore == true) {
                    $('#infiniteLoaderProductDetails img').css('display', 'none');
                    return;
                }

                if (lastIndex > 0) {
                    $('#infiniteLoaderProductDetails img').css('display', '');
                }

                setTimeout(function () {
                    loading = false;
                    var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
                    var params = {
                        'PageNumber': parseInt(parseInt(lastIndex / 4) + 1),
                        'PageSize': itemsPerLoad,
                        'Id': storedProduct.id
                    }
                    CallService('productDetails', "POST", "api/comment/GetAll", params, function (res) {
                        if (res != null && res.length > 0) {
                            $('#ulProductDetailsComments').show();
                            $('#divNotificationProductDetails').hide();
                            $('#infiniteLoaderProductDetails img').css('display', '');
                            var allComments = res;
                            loading = false;
                            lastIndex = 4;
                            maxFilteredItems = allComments[0].overAllCount;
                            localStorage.setItem('maxComments', maxFilteredItems);
                            itemsPerLoad = 4;
                            DrawComments(storedProduct, allComments, 'ulProductDetailsComments', 'productDetails', 0, itemsPerLoad);
                            lastIndex = $$('#ulProductDetailsComments li.liComment').length;
                        }
                        else {
                            $('#ulProductDetailsComments').hide();
                            $('#divNotificationProductDetails').show();
                            $('#infiniteLoaderProductDetails img').css('display', 'none');
                        }
                    });
                }, 1000);
            });

            $("#linkProductDetailsSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $("#linkAddProductInProductDetails").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#btnChats").unbind().on("click", function () {
                var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
                mainView.router.loadPage({ pageName: 'allChats', query: { reqProduct: storedProduct } });
            });

            $("#btnUpdateAdvertisment").unbind().on("click", function () {
                var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
                mainView.router.loadPage({ pageName: 'editProduct', query: { reqProduct: storedProduct } });
            });

            $('#linkProductDetailsAddComment').unbind().on('click', function () {
                
                var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));
                if (localStorage.getItem('USName')) {
                    CreatePickerModal(storedProduct, 'productDetails');
                }
                else {
                    myApp.alert('لا يمكنك إضافة تعليق...من فضلك سجل دخولك أولا', 'خطأ');
                }
            });

            $('#linkBackProductDetails').unbind().on("click", function () {
                localStorage.setItem('maxComments', 0);
                if (typeof myPhotoBrowserPopupDark != 'undefined' && myPhotoBrowserPopupDark != null) {
                    myPhotoBrowserPopupDark.close();
                }
                mainView.router.back();
            });

        }

    }
}

function GoToFavouritePage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        var divFavorites = document.getElementById('divFavorites');
        divFavorites.innerHTML = '';

        var validFavorites = [];
        var loading = false;
        var lastIndex = 6;
        var maxItems = validFavorites.length;
        var itemsPerLoad = 6;
        scrollLoadsBefore = false;

        $('#divFavorites').show();
        $('#divNotificationFavorites').hide();
        $('#infiniteLoaderFavorites img').css('display', '');

        var params = {
            "PageNumber": 1,
            "PageSize": 6
        };

        CallService('favourite', "POST", "api/favorite/GetAll", params, function (res) {
            if (res != null && res.length > 0) {
                $('#divFavorites').show();
                $('#divNotificationFavorites').hide();
                $('#infiniteLoaderFavorites img').css('display', '');
                var validFavorites = res;

                loading = false;
                lastIndex = 6;
                maxItems = validFavorites[0].overAllCount;
                itemsPerLoad = 6;
                localStorage.setItem('maxFavorites', maxItems);

                DrawFavourites(validFavorites, 0, itemsPerLoad);

                if (validFavorites.length <= itemsPerLoad) {
                    $('#infiniteLoaderFavorites img').css('display', 'none');
                }
            }
            else {
                $('#divFavorites').hide();
                $('#divNotificationFavorites').show();
                $('#infiniteLoaderFavorites img').css('display', 'none');
            }
        });

        

        if (initFavouritePage == true) {
            initFavouritePage = false;

            myApp.attachInfiniteScroll($$('#divInfiniteFavorites'));

            $$('#divInfiniteFavorites').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;
                    lastIndex = $$('#divFavorites div.divFavorite').length;
                    maxItems = localStorage.getItem('maxFavorites');
                    if (lastIndex >= maxItems || scrollLoadsBefore == true) {
                        $('#infiniteLoaderFavorites img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderFavorites img').css('display', '');


                    var params = {
                        "PageNumber": parseInt(parseInt(lastIndex / 6) + 1),
                        "PageSize": 6
                    };

                    CallService('favourite', "POST", "api/favorite/GetAll", params, function (res1) {
                        if (res1 != null && res1.length > 0) {
                            var validFavorites = res1;

                            loading = false;
                            lastIndex = 6;
                            maxItems = validFavorites[0].overAllCount;
                            itemsPerLoad = 6;
                            DrawFavourites(validFavorites, 0, itemsPerLoad);
                            lastIndex = $$('#divFavorites div.divFavorite').length;
                        }
                        else {
                            $('#divFavorites').hide();
                            $('#divNotificationFavorites').show();
                            $('#infiniteLoaderFavorites img').css('display', 'none');
                        }
                    });
                }, 1000);
            });

            $("#linkAddProductInFavourite").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkFavoritesSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $('#linkBackFavorites').unbind().on("click", function () {
                localStorage.setItem('maxFavorites', 0);
                mainView.router.back();
            });
        }
    }
}

function GoToUserPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        if (initUserPage == true) {
            initUserPage = false;

            $("#linkAddProductInUser").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkUserSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $("#linkBackUser").unbind().on("click", function () {
                mainView.router.back();
            });
        }
    }
}

function GoToProfilePage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        var validUserProducts = [];
        var loading = false;
        var lastIndex = 4;
        var maxItems = validUserProducts.length;
        var itemsPerLoad = 4;
        scrollLoadsBefore = false;

        $('#divUserProducts').show();
        $('#divNotificationUserProducts').hide();
        $('#infiniteLoaderProfile img').css('display', '');

        CallService('profile', "POST", "api/User/GetUserInfo", '', function (res) {
            if (res != null) {
                localStorage.setItem('userLoggedIn', JSON.stringify(res));
                if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                    localStorage.setItem('usrPhoto', res.photoUrl);
                }
                else {
                    localStorage.removeItem('usrPhoto');
                }

                var spanProfileName = document.getElementById('spanProfileName');
                var spanProfileMobile = document.getElementById('spanProfileMobile');
                var spanProfileEmail = document.getElementById('spanProfileEmail');
                var spanProfileProductNum = document.getElementById('spanProfileProductNum');

                spanProfileName.innerHTML = res.name;

                if (typeof res.phoneNumber != 'undefined' && res.phoneNumber != null && res.phoneNumber != '' && res.phoneNumber != ' ') {
                    spanProfileMobile.innerHTML = res.phoneNumber;
                }
                else {
                    spanProfileMobile.innerHTML = 'لا يوجد';
                }

                if (typeof res.email != 'undefined' && res.email != null && res.email != '' && res.email != ' ') {
                    spanProfileEmail.innerHTML = res.email;
                }
                else {
                    spanProfileEmail.innerHTML = 'لا يوجد';
                }

                spanProfileProductNum.innerHTML = res.adsCount+ ' إعلانات ';

                if (localStorage.getItem('usrPhoto')) {
                    imgUserAccount.src = hostUrl + "Uploads/" + localStorage.getItem('usrPhoto');
                }

                var divUserProducts = document.getElementById('divUserProducts');
                divUserProducts.innerHTML = '';

                var params = {
                    "PageNumber": 1,
                    "PageSize": 4
                };

                CallService('profile', "POST", "api/advertisement/UserAds", params, function (res) {
                    if (res != null && res.length > 0) {
                        $('#divUserProducts').show();
                        $('#divNotificationUserProducts').hide();
                        var validUserProducts = res;

                        loading = false;
                        lastIndex = 4;
                        maxItems = validUserProducts[0].overAllCount;
                        itemsPerLoad = 4;
                        localStorage.setItem('maxUserProducts', maxItems);
                        localStorage.setItem('lastIndex', lastIndex);

                        DrawUserProducts('profile', validUserProducts, 0, itemsPerLoad);

                        if (validUserProducts.length <= itemsPerLoad) {
                            $('#infiniteLoaderProfile img').css('display', 'none');
                        }
                    }
                    else {
                        $('#divUserProducts').hide();
                        $('#divNotificationUserProducts').show();
                        $('#infiniteLoaderProfile img').css('display', 'none');
                    }
                });

            }
        });

        

        if (initProfilePage == true) {
            initProfilePage = false;

            myApp.attachInfiniteScroll($$('#divInfiniteProfile'));

            $$('#divInfiniteProfile').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;
                    if (localStorage.getItem('lastIndex')) {
                        lastIndex = localStorage.getItem('lastIndex');
                    }
                    else {
                        lastIndex = $$('#divUserProducts div.divUserProduct').length;
                    }
                    maxItems = localStorage.getItem('maxUserProducts');

                    localStorage.setItem('maxUserProducts', maxItems);
                    localStorage.setItem('lastIndex', lastIndex);
                    if (parseInt(lastIndex) >= parseInt(maxItems) || scrollLoadsBefore == true) {
                        $('#infiniteLoaderProfile img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderProfile img').css('display', '');


                    var params = {
                        "PageNumber": parseInt(parseInt(Math.ceil(lastIndex / 4)) + 1),
                        "PageSize": 4
                    };

                    CallService('profile', "POST", "api/advertisement/UserAds", params, function (res1) {
                        if (res1 != null && res1.length > 0) {
                            var validUserProducts = res1;

                            loading = false;
                            lastIndex = 4;
                            maxItems = validUserProducts[0].overAllCount;
                            itemsPerLoad = 4;
                            DrawUserProducts('profile', validUserProducts, 0, itemsPerLoad);
                            lastIndex = $$('#divUserProducts div.divUserProduct').length;
                            localStorage.setItem('maxUserProducts', maxItems);
                            localStorage.setItem('lastIndex', lastIndex);
                        }
                        else {
                            $('#divUserProducts').hide();
                            $('#divNotificationUserProducts').show();
                            $('#infiniteLoaderProfile img').css('display', 'none');
                        }
                    });
                }, 1000);
            });

            $('#linkEditProfile').unbind().on('click', function () {
                mainView.router.loadPage({ pageName: 'signup' });
            });

            $("#linkAddPhoto").unbind().on("click", function () {
                if (localStorage.getItem('userLoggedIn')) {
                    getImage();
                }
            });

            $("#linkAddProductInProfile").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkProfileSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $('#linkBackProfile').unbind().on("click", function () {
                localStorage.setItem('maxUserProducts', 0);
                localStorage.setItem('lastIndex', 0);
                mainView.router.back();
            });
        }
    }
}

function GoToContactPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        var divContactAddresses = document.getElementById('divContactAddresses');
        var divContactEmails = document.getElementById('divContactEmails');
        var divContactMobiles = document.getElementById('divContactMobiles');
        var divContactFaxes = document.getElementById('divContactFaxes');

        var iconContactAddress = document.getElementById('iconContactAddress');
        var iconContactEmail = document.getElementById('iconContactEmail');
        var iconContactMobile = document.getElementById('iconContactMobile');
        var iconContactFax = document.getElementById('iconContactFax');

        $('#txtContactName').val('');
        $('#txtContactMobile').val('');
        $('#txtContactEmail').val('');
        $('#txtContactMessage').val('');

        $('#divContactAddress').hide();
        $('#divContactEmail').hide();
        $('#divContactMobile').hide();
        $('#divContactFax').hide();

        divContactAddresses.innerHTML = '';
        divContactEmails.innerHTML = '';
        divContactMobiles.innerHTML = '';
        divContactFaxes.innerHTML = '';

        var params = {
            'PageNumber': 0,
            'PageSize': 0
        };

        CallService('contact', "POST", "api/contactInformation/GetAll", params, function (res) {
            if (res != null && res.length > 0) {
                var allContacts = res;
                var emailContacts = allContacts.filter(function (el) {
                    return el.contactTypeId == 1;
                });
                var mobileContacts = allContacts.filter(function (el) {
                    return el.contactTypeId == 2;
                });
                var addressContacts = allContacts.filter(function (el) {
                    return el.contactTypeId == 3;
                });
                var faxContacts = allContacts.filter(function (el) {
                    return el.contactTypeId == 4;
                });

                if (emailContacts != null && emailContacts.length > 0) {
                    $('#divContactEmail').show();
                    iconContactEmail.className = emailContacts[0].iconName;
                    for (var e = 0; e < emailContacts.length; e++) {
                        var pEmailContact = document.createElement('p');
                        var linkEmailContact = document.createElement('a');

                        linkEmailContact.setAttribute('id', 'linkEmailContact_' + emailContacts[e].id);

                        pEmailContact.innerHTML = emailContacts[e].contact;

                        linkEmailContact.appendChild(pEmailContact);

                        divContactEmails.appendChild(linkEmailContact);

                        $('#linkEmailContact_' + emailContacts[e].id).unbind().on('click', function () {
                            
                            var elemId = $(this).attr('id');
                            var productId = elemId.split('_')[1];
                            var reqContact;
                            for (var i = 0; i < emailContacts.length; ++i) {
                                if (emailContacts[i].id == productId) {
                                    reqContact = emailContacts[i];
                                    break;
                                }
                            }

                            location.href = "mailto:" + reqContact.contact + '?subject=' + 'مرحبا' + '&body=' + 'مرحبا بك اخي', '_system', 'location=no,toolbar=no,zoom=no';
                        });

                    }
                }

                if (mobileContacts != null && mobileContacts.length > 0) {
                    $('#divContactMobile').show();
                    iconContactMobile.className = mobileContacts[0].iconName;
                    for (var e = 0; e < mobileContacts.length; e++) {
                        var pMobileContact = document.createElement('p');
                        var linkMobileContact = document.createElement('a');

                        linkMobileContact.setAttribute('id', 'linkMobileContact_' + mobileContacts[e].id);

                        pMobileContact.innerHTML = mobileContacts[e].contact;

                        linkMobileContact.appendChild(pMobileContact);

                        divContactMobiles.appendChild(linkMobileContact);

                        $('#linkMobileContact_' + mobileContacts[e].id).unbind().on('click', function () {
                            
                            var elemId = $(this).attr('id');
                            var productId = elemId.split('_')[1];
                            var reqContact;
                            for (var i = 0; i < mobileContacts.length; ++i) {
                                if (mobileContacts[i].id == productId) {
                                    reqContact = mobileContacts[i];
                                    break;
                                }
                            }

                            document.addEventListener('deviceready', function () {
                                var number = reqContact.contact;
                                window.plugins.CallNumber.callNumber(function () {
                                    
                                }, function () {
                                    myApp.alert('غير قادر علي الإتصال بهذا الهاتف .');
                                }, number, true);
                            }, false);
                        });
                    }
                }

                if (addressContacts != null && addressContacts.length > 0) {
                    $('#divContactAddress').show();
                    iconContactAddress.className = addressContacts[0].iconName;
                    for (var e = 0; e < addressContacts.length; e++) {
                        var pAddressContact = document.createElement('p');
                        pAddressContact.innerHTML = addressContacts[e].contact;

                        divContactAddresses.appendChild(pAddressContact);
                    }
                }

                if (faxContacts != null && faxContacts.length > 0) {
                    $('#divContactFax').show();
                    iconContactFax.className = faxContacts[0].iconName;
                    for (var e = 0; e < faxContacts.length; e++) {
                        var pFaxContact = document.createElement('p');
                        pFaxContact.innerHTML = faxContacts[e].contact;

                        divContactFaxes.appendChild(pFaxContact);
                    }
                }
            }
        });

        if (initContactPage == true) {
            initContactPage = false;

            $("#btnSendContactMessage").unbind().on("click", function () {
                FValidation.ValidateAll('contact', function (res) {
                    if (res == true) {
                        var params = {
                            'Name': $('#txtContactName').val(),
                            'Phone': $('#txtContactMobile').val(),
                            'Email': $('#txtContactEmail').val(),
                            'Message': $('#txtContactMessage').val()
                        };

                        CallService('contact', "POST", "api/contactUsMessage/add", params, function (res) {
                            if (res != null) {
                                myApp.alert('تمت إرسال رسالة إلي الإدارة بنجاح .', 'نجاح', function () {
                                    $('#txtContactName').val('');
                                    $('#txtContactMobile').val('');
                                    $('#txtContactEmail').val('');
                                    $('#txtContactMessage').val('');
                                });
                            }
                            else {
                                myApp.alert('خطأ في إضافة إعلان جديد.', 'خطأ', function () { });
                            }
                        });
                    }
                });
            });

            $("#linkAddProductInContact").unbind().on("click", function () {
                if (localStorage.getItem('USName')) {
                    mainView.router.loadPage({ pageName: 'addProduct' });
                }
                else {
                    myApp.alert('يجب عليك التسجيل أولا لإضافة إعلان .', 'خطأ', function () { });
                }
            });

            $("#linkContactSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });

            $('#linkBackContact').unbind().on('click', function () {
                
                $('#txtContactName').val('');
                $('#txtContactMobile').val('');
                $('#txtContactEmail').val('');
                $('#txtContactMessage').val('');
                mainView.router.back();
            });

        }
    }
}

function GoToAddProductPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();

        for (var i = 0; i < mainView.history.length; i++) {
            if (mainView.history[i] === '#addProduct') mainView.history.splice(i, 1);
        }

        var selectAddProductCategory = document.getElementById('selectAddProductCategory');
        var selectAddProductDuration = document.getElementById('selectAddProductDuration');
        selectAddProductCategory.innerHTML = '';
        selectAddProductDuration.innerHTML = '';

        $("#freeRadioButton").prop("checked", true);
        $("#specialRadioButton").prop("checked", false);

        var imageName = '';
        localStorage.setItem('PPhoto', '');

        var photos = [];
        DrawAllProductPhotos(photos, 4);

        $('#addProductStartDate').val('');
        linkBackAddProduct = true;
        calendarAddProductStart.close();
        calendarAddProductStart.setValue([]);

        GetAllDurations('addProduct', function (res1) {
            $('#linkAddProductCategory .item-after').html('');
            $('#linkAddProductDuration .item-after').html('');
            $('#txtAddProductName').val('');
            $('#txtAddProductPrice').val('');
            $('#txtAddProductDescription').val('');
            $("#freeRadioButton").prop("checked", true);
            $("#specialRadioButton").prop("checked", false);

            $('.lblError').removeClass('activeError');
            $('.lblError').removeClass('slideInDown');

            var selectAddProductDuration = document.getElementById('selectAddProductDuration');
            selectAddProductDuration.innerHTML = '';
            myApp.smartSelectAddOption('#linkAddProductDuration select', '<option value="" selected disabled></option>');

            GetAllCategories('addProduct', function (res) {
                var selectAddProductCategory = document.getElementById('selectAddProductCategory');
                selectAddProductCategory.innerHTML = '';

                myApp.smartSelectAddOption('#linkAddProductCategory select', '<option value="" selected disabled></option>');

                for (var c = 0; c < allCategories.length; c++) {
                    myApp.smartSelectAddOption('#linkAddProductCategory select', '<option value="' + allCategories[c].id + '">' + allCategories[c].name + '</option>');
                }

                $('#linkAddProductCategory').unbind().on("click", function () {
                    myApp.smartSelectOpen('#linkAddProductCategory');
                });
            });

            for (var c = 0; c < allDurations.length; c++) {
                myApp.smartSelectAddOption('#linkAddProductDuration select', '<option value="' + allDurations[c].id + '" price="' + allDurations[c].price + '">' + allDurations[c].period + '</option>');
            }

            $('#linkAddProductCategory').unbind().on('click', function () {
                
                myApp.smartSelectOpen('#linkAddProductCategory');
            });
            $('#linkAddProductDuration').unbind().on('click', function () {
                
                myApp.smartSelectOpen('#linkAddProductDuration');
            });

        });

        $('#liAddProductDuration').hide();
        $('#liAddProductPrice').hide();

        $('input:radio[name=my-radio]').change(function () {
            if (this.value == '1') {
                $('#liAddProductDuration').show();
                $('#liAddProductPrice').show();
            }
            else {
                $('#liAddProductDuration').hide();
                $('#liAddProductPrice').hide();
            }
        });

        $('#selectAddProductDuration').unbind().on('change', function () {
            $('#txtAddProductPrice').val($(this).find(":selected").attr('price'));
        });

        $('#linkBackAddProduct').unbind().on('click', function () {
            
            $('#txtAddProductPrice').val('');
            $('#txtAddProductDescription').val('');
            $('#lblProductImage').html('صور الإعلان');
            $('#addProductStartDate').val('');
            linkBackSearch = true;
            var selectAddProductCategory = document.getElementById('selectAddProductCategory');
            var selectAddProductDuration = document.getElementById('selectAddProductDuration');
            selectAddProductCategory.innerHTML = '';
            selectAddProductDuration.innerHTML = '';
            calendarAddProductStart.close();
            calendarAddProductStart.setValue([]);
            publicImageURI = [];
            var photos = [];
            DrawAllProductPhotos(photos, 4);
            if ($('.smart-select-picker.modal-in').length > 0) {
                myApp.closeModal('.smart-select-picker.modal-in');
            }
            mainView.router.back();
        });

        $("#linkSearchAddProduct").unbind().on("click", function () {
            mainView.router.loadPage({ pageName: 'search' });
        });

        $('#btnAddProduct').unbind().on("click", function () {
            if (localStorage.getItem('USName')) {
                    FValidation.ValidateAll('addProduct', function (res) {
                        if (res == true) {
                            var name = $('#txtAddProductName').val();
                            var price = $('#txtAddProductPrice').val();
                            var addProductCategory = $('#selectAddProductCategory option:selected').val();
                            var addProductDuration = $('#selectAddProductDuration option:selected').val();
                            var description = $('#txtAddProductDescription').val();
                            var isSpecial = $('input[name=my-radio]:checked').val();
                            var image = '';

                            var dateObj = Date.now().toString();

                            imageName = 'Img.jpg';

                            if (parseInt(isSpecial) == 0) {
                                isSpecial = false;
                                addProductDuration = null;
                            }
                            else {
                                isSpecial = true;
                            }

                            if (localStorage.getItem('productPhoto')) {
                                image = localStorage.getItem('productPhoto');
                            }

                            if (typeof price == 'undefined' || price == null || price == '' || price == ' ') {
                                price = 0;
                            }

                            var params = {
                                'CategoryId': addProductCategory,
                                'IsPaided': isSpecial,
                                'StartDate': '',
                                'EndDate': '',
                                'IsArchieved': false,
                                'Description': description,
                                'Name': name,
                                'advertismentPriceId':addProductDuration,
                                'PaidEdPrice': price,
                                'ImagesViewModels': JSON.stringify(publicImageURI)
                            };

                            CallService('addProduct', "POST", "api/advertisement/save", params, function (res) {
                                if (res != null && res > 0) {
                                    myApp.alert('تمت إضافة الإعلان بنجاح .', 'نجاح', function () {
                                        $('#linkAddProductCategory .item-after').html('');
                                        $('#linkAddProductDuration .item-after').html('');
                                        $('#txtAddProductName').val('');
                                        $('#txtAddProductPrice').val('');
                                        $('#txtAddProductDescription').val('');
                                        publicImageURI = [];
                                        var photos = [];
                                        DrawAllProductPhotos(photos, 4);
                                        localStorage.removeItem('productPhoto');
                                        localStorage.removeItem('PPhoto');
                                        if (isSpecial == true) {
                                            PayOnline(res, 'addProduct', function (result) {
                                                
                                            });
                                        }
                                        else {
                                            GetBackInHistory();
                                        }
                                    });
                                }
                                else {
                                    myApp.alert('خطأ في إضافة إعلان جديد.', 'خطأ', function () { });
                                }
                            });


                        }
                    });
            }
            else {
                myApp.alert('لا يمكنك إضافة منتج...من فضلك سجل دخولك أولا', 'خطأ');
            }
        });

    }
}

function GoToEditProductPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();

        var reqProduct = page.query.reqProduct;
        localStorage.setItem('reqProduct', JSON.stringify(page.query.reqProduct));

        var selectEditProductCategory = document.getElementById('selectEditProductCategory');
        var selectEditProductDuration = document.getElementById('selectEditProductDuration');
        selectEditProductCategory.innerHTML = '';
        selectEditProductDuration.innerHTML = '';

        var imageName = '';

        if (reqProduct.jsonImages != '') {
            var photos = JSON.parse(reqProduct.jsonImages);
            DrawAllProductPhotos(photos, 4);
            localStorage.setItem('PPhoto', 'set');
        }
        else {
            var photos = [];
            DrawAllProductPhotos(photos, 4);
            localStorage.setItem('PPhoto', '');
        }

        
        linkBackEditProduct = true;
        calendarEditProductStart.close();
        calendarEditProductStart.setValue([]);

        GetAllDurations('editProduct', function (res1) {
            $('#linkEditProductCategory .item-after').html('');
            $('#linkEditProductDuration .item-after').html('');
            $('#txtEditProductName').val(reqProduct.name);
            
            $('#txtEditProductDescription').val(reqProduct.description);

            if (reqProduct.isPaided) {
                $("#freeEditRadioButton").prop("checked", false);
                $("#specialEditRadioButton").prop("checked", true);

                $('#txtEditProductPrice').val(reqProduct.paidEdPrice);
                $('#liEditProductDuration').show();
                $('#liEditProductPrice').show();
                if (reqProduct.isExpired) {
                    $('#liEditProductDuration').removeAttr('disabled');
                    $('#liEditProductPrice').removeAttr('disabled');
                }
                else {
                    $('#liEditProductDuration').attr('disabled', 'disabled');
                    $('#liEditProductPrice').attr('disabled', 'disabled');
                }
            }
            else {
                $("#freeEditRadioButton").prop("checked", true);
                $("#specialEditRadioButton").prop("checked", false);

                $('#txtEditProductPrice').val('');
                $('#liEditProductDuration').hide();
                $('#liEditProductPrice').hide();
                $('#liEditProductDuration').removeAttr('disabled');
                $('#liEditProductPrice').removeAttr('disabled');
            }

            $('.lblError').removeClass('activeError');
            $('.lblError').removeClass('slideInDown');

           
            var selectEditProductDuration = document.getElementById('selectEditProductDuration');
            selectEditProductDuration.innerHTML = '';

            myApp.smartSelectAddOption('#linkEditProductDuration select', '<option value="" disabled></option>');

            GetAllCategories('editProduct', function (res) {
                var selectEditProductCategory = document.getElementById('selectEditProductCategory');
                selectEditProductCategory.innerHTML = '';

                myApp.smartSelectAddOption('#linkEditProductCategory select', '<option value="" disabled></option>');

                for (var c = 0; c < allCategories.length; c++) {
                    if (allCategories[c].id == reqProduct.categoryId) {
                        $('#linkEditProductCategory .item-after').html(allCategories[c].name);
                        myApp.smartSelectAddOption('#linkEditProductCategory select', '<option value="' + allCategories[c].id + '" selected>' + allCategories[c].name + '</option>');
                    }
                    else {
                        myApp.smartSelectAddOption('#linkEditProductCategory select', '<option value="' + allCategories[c].id + '">' + allCategories[c].name + '</option>');
                    }
                }

                $('#linkEditProductCategory').unbind().on("click", function () {
                    myApp.smartSelectOpen('#linkEditProductCategory');
                });
            });


            for (var c = 0; c < allDurations.length; c++) {
                if (reqProduct.isPaided && allDurations[c].price == reqProduct.paidEdPrice) {
                    $('#linkEditProductDuration .item-after').html(allDurations[c].period);
                    myApp.smartSelectAddOption('#linkEditProductDuration select', '<option value="' + allDurations[c].id + '" price="' + allDurations[c].price + '" selected>' + allDurations[c].period + '</option>');
                }
                else {
                    myApp.smartSelectAddOption('#linkEditProductDuration select', '<option value="' + allDurations[c].id + '" price="' + allDurations[c].price + '">' + allDurations[c].period + '</option>');
                }
            }

            $('#linkEditProductCategory').unbind().on('click', function () {
                
                myApp.smartSelectOpen('#linkEditProductCategory');
            });
            $('#linkEditProductDuration').unbind().on('click', function () {
                
                myApp.smartSelectOpen('#linkEditProductDuration');
            });

        });

        $('#liEditProductDuration').attr('disabled', 'disabled');
        $('#liEditProductPrice').attr('disabled', 'disabled');

        if (reqProduct.isExpired == true) {
            $('#liEditProductDuration').removeAttr('disabled');
            $('#liEditProductPrice').removeAttr('disabled');
        }

        $('input:radio[name=my-Edit-radio]').change(function () {
            if (this.value == '1' && reqProduct.isExpired == true) {
                $('#liEditProductDuration').show();
                $('#liEditProductPrice').show();
                $('#liEditProductDuration').removeAttr('disabled');
                $('#liEditProductPrice').removeAttr('disabled');
            }
            else {
                if (this.value == '0') {
                    $('#liEditProductDuration').hide();
                    $('#liEditProductPrice').hide();
                }
                else {
                    $('#liEditProductDuration').show();
                    $('#liEditProductPrice').show();
                }
                $('#liEditProductDuration').attr('disabled', 'disabled');
                $('#liEditProductPrice').attr('disabled', 'disabled');
            }
        });

        $('#selectEditProductDuration').unbind().on('change', function () {
            $('#txtEditProductPrice').val($(this).find(":selected").attr('price'));
        });

        $('#btnEditProduct').unbind().on("click", function () {
            if (localStorage.getItem('USName')) {
                FValidation.ValidateAll('editProduct', function (res) {
                    if (res == true) {
                        var name = $('#txtEditProductName').val();
                        var price = $('#txtEditProductPrice').val();
                        var EditProductCategory = $('#selectEditProductCategory option:selected').val();
                        var EditProductDuration = $('#selectEditProductDuration option:selected').val();
                        var description = $('#txtEditProductDescription').val();
                        var isSpecial = $('input[name=my-Edit-radio]:checked').val();
                        var isExpired = reqProduct.isExpired;
                        var image = '';

                        var dateObj = Date.now().toString();

                        imageName = 'Img.jpg';

                        if (parseInt(isSpecial) == 0) {
                            isSpecial = false;
                            addProductDuration = null;
                        }
                        else {
                            isSpecial = true;
                        }

                        if (localStorage.getItem('productPhoto')) {
                            image = localStorage.getItem('productPhoto');
                        }

                        if (typeof price == 'undefined' || price == null || price == '' || price == ' ') {
                            price = 0;
                        }

                        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));

                        var params = {
                            'Id': storedProduct.id,
                            'CategoryId': EditProductCategory,
                            'IsPaided': isSpecial,
                            'StartDate': '',
                            'EndDate': '',
                            'IsArchieved': false,
                            'Description': description,
                            'Name': name,
                            'advertismentPriceId': EditProductDuration,
                            'PaidEdPrice': price,
                            'ImagesViewModels': JSON.stringify(publicImageURI)
                        };

                        CallService('editProduct', "POST", "api/advertisement/save", params, function (res) {
                            if (res != null && res > 0) {
                                myApp.alert('تم تعديل الإعلان بنجاح .', 'نجاح', function () {
                                    $('#linkEditProductCategory .item-after').html('');
                                    $('#linkEditProductDuration .item-after').html('');
                                    $('#txtEditProductName').val('');
                                    $('#txtEditProductPrice').val('');
                                    $('#txtEditProductDescription').val('');
                                    localStorage.removeItem('productPhoto');
                                    localStorage.removeItem('PPhoto');
                                    if (isSpecial == true && isExpired == true) {
                                        PayOnline(res, 'editProduct', function (result) {
                                            
                                        });
                                    }
                                    else {
                                        GetBackInHistory();
                                    }
                                });
                            }
                            else {
                                myApp.alert('خطأ في تعديل الإعلان .', 'خطأ', function () { });
                            }
                        });


                    }
                });
            }
            else {
                myApp.alert('لا يمكنك إضافة منتج...من فضلك سجل دخولك أولا', 'خطأ');
            }
        });

        $('#linkBackEditProduct').unbind().on('click', function () {
            
            $('#txtEditProductPrice').val('');
            $('#txtEditProductDescription').val('');
            linkBackEditProduct = true;
            publicImageURI = [];
            selectEditProductDuration.innerHTML = '';
            selectEditProductCategory.innerHTML = '';
            calendarEditProductStart.close();
            calendarEditProductStart.setValue([]);
            if ($('.smart-select-picker.modal-in').length > 0) {
                myApp.closeModal('.smart-select-picker.modal-in');
            }
            mainView.router.back();
        });

        $("#linkSearchEditProduct").unbind().on("click", function () {
            mainView.router.loadPage({ pageName: 'search' });
        });

    }
}

function GoToAddComment(reqProduct, pageName) {
    $('#linkPopupAddComment_' + pageName + '').unbind().on('click', function () {
        
        var txtComment = $('#txtAddComment_' + pageName).val().trim();
        var userLoggedName = localStorage.getItem('USName');
        var spanProductDetailsComments = document.getElementById('spanProductDetailsComments');

        var params = {
            'AdvertismentId': reqProduct.id,
            'Message': txtComment
        };

        CallService(pageName, "POST", "api/comment/Add", params, function (result) {
            if (result != null) {
                myApp.closeModal('.add-comment-picker_' + pageName);

                var newComment = BuildComment(reqProduct, pageName, result, result);
                var ulProductDetailsComments = document.getElementById('ulProductDetailsComments');
                ulProductDetailsComments.appendChild(newComment);

                var commentsDrawn = $('#ulProductDetailsComments li.liComment').length;
                spanProductDetailsComments.innerHTML = commentsDrawn;

                $('#ulProductDetailsComments').show();
                $('#divNotificationProductDetails').hide();

                //if (pageName === 'drPage') {
                //    var divCommentsToDraw = document.getElementById('divCommentsToDraw');
                //    divCommentsToDraw.appendChild(newComment);
                //}
                //else {
                //    var divCommentsToDraw = document.getElementById('divAllMyComments');
                //    divCommentsToDraw.appendChild(newComment);
                //}

                $('#linkRemove' + pageName + '_' + result.id).unbind().on('click', function () {
                    
                    var elemId = $(this).attr('id');
                    var commentId = elemId.split('_')[1];
                    myApp.confirm('هل انت متاكد من حذف هذا التعليق؟', 'تأكيد', function () {
                        GoToRemoveComment(reqProduct, pageName, commentId, commentId);
                    });
                });

            }
            else {
                myApp.alert('خطأ في إضافة تعليق جديد .', 'خطأ', function () { });
            }
        });


    });

    $('#linkCloseComment').unbind().on('click', function () {
        
        myApp.closeModal('.add-comment-picker_' + pageName);
    });
}

function GoToRemoveComment(reqProduct, pageName, commentId, elementId) {
    var spanProductDetailsComments = document.getElementById('spanProductDetailsComments');

    CallService(pageName, "DELETE", "api/Comment/delete/" + commentId, '', function (result) {
        if (result != null && result == "") {
            myApp.alert('تم حذف التعليق بنجاح .', 'نجاح', function () {
                var item = document.getElementById("comment_" + pageName + commentId);
                item.parentNode.removeChild(item);

                var ulProductDetailsComments = document.getElementById('ulProductDetailsComments');
                var commentsDrawn = $('#ulProductDetailsComments li.liComment').length;
                spanProductDetailsComments.innerHTML = commentsDrawn;

                if (commentsDrawn > 0) {
                    $('#ulProductDetailsComments').show();
                    $('#divNotificationProductDetails').hide();
                }
                else {
                    $('#ulProductDetailsComments').hide();
                    $('#divNotificationProductDetails').show();
                }
            });
        }
        else {
            myApp.alert('خطأ في حذف التعليق .', 'خطأ', function () { });
        }
    });
}

function GoToAllChats(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();
        var reqProduct = page.query.reqProduct;
        var validChats = [];
        var loading = false;
        var lastIndex = 8;
        var maxItems = validChats.length;
        var itemsPerLoad = 8;
        var scrollLoadsBefore = false;

        localStorage.setItem('reqProduct', JSON.stringify(page.query.reqProduct));

        var ulChats = document.getElementById('ulChats');
        ulChats.innerHTML = '';


        $('#divNotificationAllChats').hide();
        $('#divAllChats').show();
        $('#infiniteLoaderAllChats img').css('display', '');

        CallService('allChats', "POST", "api/User/GetUserInfo", '', function (res) {
            if (res != null) {
                localStorage.setItem('userLoggedIn', JSON.stringify(res));


                var params = {};
                var methodName = '';

                if (reqProduct.isOwner) {
                    params = {
                        "AdvertismentId": reqProduct.id,
                        "PageNumber": 1,
                        "PageSize": 8
                    };
                    methodName = 'api/chat/GetChatByAdvertisment';
                }
                else {
                    var userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));
                    params = {
                        "AdvertismentId": reqProduct.id,
                        "PageNumber": 1,
                        "PageSize": 8,
                        'UserId': userLoggedIn.id
                    };
                    methodName = 'api/chat/GetChatByUser';
                }

                $('#infiniteLoaderAllChats img').css('display', '');

                CallService('allChats', "POST", methodName, params, function (res) {
                    if (res != null && res.length > 0) {
                        $('#divAllChats').show();
                        $('#divNotificationAllChats').hide();
                        $('#infiniteLoaderAllChats img').css('display', '');
                        var validChats = res;
                        loading = false;
                        lastIndex = 8;
                        maxItems = validChats[0].overallCount;
                        itemsPerLoad = 8;
                        localStorage.setItem('maxAllChats', maxItems);

                        var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));

                        DrawAllChats(validChats, storedProduct, lastIndex, itemsPerLoad);

                        if (validChats.length < itemsPerLoad) {
                            $('#infiniteLoaderAllChats img').css('display', 'none');
                        }
                    }
                    else {
                        $('#divNotificationAllChats').show();
                        $('#divAllChats').hide();
                        $('#infiniteLoaderAllChats img').css('display', 'none');
                    }
                });
            }
        });

        if (initAllChats == true) {
            initAllChats = false;

            myApp.attachInfiniteScroll($$('#divInfiniteAllChat'));

            $$('#divInfiniteAllChat').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;
                    lastIndex = $$('#ulChats li.item-content').length;
                    maxItems = localStorage.getItem('maxAllChats');
                    if (lastIndex >= maxItems || scrollLoadsBefore == true) {
                        $('#infiniteLoaderAllChats img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderAllChats img').css('display', '');

                    var storedProduct = JSON.parse(localStorage.getItem('reqProduct'));

                    var params = {};
                    var methodName = '';

                    if (storedProduct.isOwner) {
                        params = {
                            "AdvertismentId": storedProduct.id,
                            "PageNumber": parseInt(parseInt(lastIndex / 8) + 1),
                            "PageSize": 8
                        };
                        methodName = 'api/chat/GetChatByAdvertisment';
                    }
                    else {
                        var userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));
                        params = {
                            "AdvertismentId": storedProduct.id,
                            "PageNumber": parseInt(parseInt(lastIndex / 8) + 1),
                            "PageSize": 8,
                            'UserId': userLoggedIn.id
                        };
                        methodName = 'api/chat/GetChatByUser';
                    }

                    CallService('allChats', "POST", methodName, params, function (res1) {
                        if (res1 != null) {
                            validChats = res1;
                            DrawAllChats(validChats, storedProduct, lastIndex, itemsPerLoad);
                            lastIndex = $$('#ulChats li.item-content').length;
                        }
                        else {
                            $('#divNotificationAllChats').show();
                            $('#divAllChats').hide();
                            $('#infiniteLoaderAllChats img').css('display', 'none');
                        }
                    });
                }, 1000);
            });

            $('#linkBackAllChats').unbind().on('click', function () {
                localStorage.setItem('maxAllChats', 0);
                cleatTimer();
                mainView.router.back();
            });

            $("#linkAllChatsSearch").unbind().on("click", function () {
                mainView.router.loadPage({ pageName: 'search' });
            });
        }

    }
}

function GoToChatPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks();

        UpdateUserStatus(true, function (res) {
        });

        //var reqProduct = page.query.reqProduct;
        var chatId = page.query.chatId;
        var chatHeader = page.query.chatHeader;

        var pageNumber = 1;
        var pageSize = 10;
        lastMsgSentDate = new Date();
        allMessages = [];
        var userImage;
        isChatDrawn = false;

        var iElementChat = document.getElementById('iElementChat');
        localStorage.setItem('chatId', chatId);

        var divChatMessages = document.getElementById('divChatMessages');
        divChatMessages.className = 'chatui';

        var divForInsideLoader = document.createElement('div');
        var divLoaderInside = document.createElement('div');
        var divSpinner = document.createElement('div');
        divSpinner.className += 'divSpinner';
        divLoaderInside.className += 'loaderInside divLoaderInside';
        divForInsideLoader.className += 'divForInsideLoader';

        divLoaderInside.appendChild(divSpinner);
        divForInsideLoader.appendChild(divLoaderInside);
        divChatMessages.appendChild(divForInsideLoader);
        $(".loaderInside").fadeIn("slow");

        var userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));

        if (userLoggedIn.id === chatHeader.userOneId) {
            userImage = chatHeader.pictureUserTwo;
        }
        else {
            userImage = chatHeader.pictureUserOne;
        }

        GetAllMessages(userImage, 'chat', chatId, pageNumber, pageSize, '', function (messages) {
            $(".loaderInside").fadeOut("slow");
            $('div').each(function () {
                var div = $(this);
                if ($(this).hasClass('loaderInside divLoaderInside')) {
                    $(this).remove();
                }
            });
            divChatMessages.className = 'chatuiAfterLoader';

            DrawChatMessages(messages, function (isMessageDrawn) {
                if (isMessageDrawn == true) {
                    if (messageInterval) {
                        cleatTimer();
                    }

                    var storedChatId = localStorage.getItem('chatId');

                    messageInterval = setInterval(function () {
                        GetAllMessages(userImage, 'chat1', storedChatId, pageNumber, pageSize, lastMsgSentDate, function (messages) {
                            if (messages != null && messages.length > 0) {
                                DrawChatMessages(messages, function (resDrawn) { });
                            }
                        });

                        userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));
                    }, 1000);

                }
                else {

                }
            });

        });

        if (initChatPage == true) {
            initChatPage = false;

            $('#linkBackChat').unbind().on('click', function () {
                
                UpdateUserStatus(false, function (res) {
                    $('#txtChatMessage').val('');
                    $('#txtChatTransportPrice').val('');
                    mainView.router.back();
                });
            });

            $('#linkChatSearch').unbind().on('click', function () {
                
                cleatTimer();
                UpdateUserStatus(false, function (res) {
                    mainView.router.loadPage({ pageName: 'search' });
                });
            });

            if (localStorage.getItem('userLoggedIn')) {
                var user = JSON.parse(localStorage.getItem('userLoggedIn'));

                $('#linkChatSendNewMessage').unbind().on('click', function () {
                    
                    var msg = $('#txtChatMessage').val();
                    var storedChatId = JSON.parse(localStorage.getItem('chatId'));

                    if (typeof msg != 'undefined' && msg != '' && msg != ' ' && msg != null) {
                        var params = {
                            'ChatId': storedChatId,
                            'MessageContent': msg.trim(),
                        };

                        var date = new Date();
                        var msgDate = date.getUTCFullYear() + '-' +
                            ('00' + (date.getUTCMonth() + 1)).slice(-2) + '-' +
                            ('00' + date.getUTCDate()).slice(-2) + 'T' +
                            ('00' + date.getUTCHours()).slice(-2) + ':' +
                            ('00' + date.getUTCMinutes()).slice(-2) + ':' +
                            ('00' + date.getUTCSeconds()).slice(-2) + ':' +
                            ('00' + date.getUTCMilliseconds()).slice(-2);

                        //lastMsgSentDate = msgDate;

                        CallService('chat', "POST", "api/chat/SaveMessage", params, function (result) {
                            if (result != null) {
                                var newMessages = [];
                                $('#txtChatMessage').val('');
                            }
                        });
                    }
                });

            }
            else {
                myApp.alert('من فضلك سجل دخولك أولا.', 'خطأ', function () { });
            }
        }

    }
}

function GotoNotificationPage(page) {
    if (typeof page != 'undefined') {
        loadSideMenuLinks('notification');
        var divNotifications = document.getElementById('divNotifications');
        divNotifications.innerHTML = '';

        var validNotifications = [];
        var loading = false;
        var lastIndex = 10;
        var maxItems = validNotifications.length;
        var itemsPerLoad = 10;
        var scrollLoadsBefore = false;

        $('#divNotifications').show();
        $('#divNoNotifications').hide();
        $('#infiniteLoaderAllNotifications img').css('display', '');

        var params = {
            "pageNumber": 1,
            "pageSize": 10
        };

        CallService('notification', "POST", "api/Notification/GetAll", params, function (notifications) {
            if (notifications != null && notifications.length > 0) {
                $('#divNotifications').show();
                $('#divNoNotifications').hide();
                validNotifications = notifications;
                loading = false;
                lastIndex = 10;
                maxItems = validNotifications[0].overallCount;
                itemsPerLoad = 10;
                localStorage.setItem('maxNotifications', maxItems);

                DrawNotifications(validNotifications, 0, itemsPerLoad);

                if (validNotifications.length < itemsPerLoad) {
                    $('#infiniteLoaderAllNotifications img').css('display', 'none');
                }
            }
            else {
                $('#divNotifications').hide();
                $('#divNoNotifications').show();
                $('#infiniteLoaderAllNotifications img').css('display', 'none');
            }
        });



        if (initNotifications == true) {
            initNotifications = false;

            $("#linkNotificationSearch").unbind().on('click', function () {
                
                mainView.router.loadPage({ pageName: 'search' });
            });

            myApp.attachInfiniteScroll($$('#divInfiniteAllNotifications'));

            $$('#divInfiniteAllNotifications').on('infinite', function () {
                if (loading) return;
                loading = true;

                setTimeout(function () {
                    loading = false;
                    lastIndex = $$('#divNotifications div.divColNotification').length;
                    maxItems = localStorage.getItem('maxNotifications');
                    if (lastIndex >= maxItems || scrollLoadsBefore == true) {
                        $('#infiniteLoaderAllNotifications img').css('display', 'none');
                        return;
                    }

                    $('#infiniteLoaderAllNotifications img').css('display', '');

                    var params = {
                        "pageNumber": parseInt(parseInt(lastIndex / 10) + 1),
                        "pageSize": 10
                    }

                    CallService('notification', "POST", "api/Notification/GetAll", params, function (notifications1) {
                        if (notifications1 != null && notifications1.length > 0) {
                            validNotifications = notifications1;
                            DrawNotifications(validNotifications, 0, itemsPerLoad);
                            lastIndex = $$('#divNotifications div.divColNotification').length;
                        }
                        else {
                            $('#divNotifications').hide();
                            $('#divNoNotifications').show();
                            $('#infiniteLoaderAllNotifications img').css('display', 'none');
                        }
                    });
                }, 1000);
            });

            $("#linkNotificationBack").unbind().on('click', function () {
                localStorage.setItem('maxNotifications', 0);
                mainView.router.back();
            });

        }
    }
}

myApp.onPageBeforeInit('splash', function (page) {
    GoToSplashPage(page);
}).trigger();

myApp.onPageAfterAnimation('login', function (page) {
    cleatTimer();
    currentPage = 'login';
    GoToLoginPage(page);
    //ChangeLanguage();
}).trigger();

myApp.onPageAfterAnimation('forgetPass', function (page) {
    currentPage = 'forgetPass';
    GoToForgetPasswordPage(page);
}).trigger();

myApp.onPageAfterAnimation('signup', function (page) {
    cleatTimer();
    currentPage = 'signup';
    GoToSignUpPage(page);
}).trigger();

myApp.onPageAfterAnimation('activation', function (page) {
    currentPage = 'activation';
    GoToActivationPage(page);
}).trigger();

myApp.onPageAfterAnimation('categories', function (page) {
    cleatTimer();
    currentPage = 'categories';
    GoToCategoriesPage(page);
}).trigger();

myApp.onPageAfterAnimation('products', function (page) {
    cleatTimer();
    currentPage = 'products';
    GoToProductsPage(page);
}).trigger();

myApp.onPageAfterAnimation('favourite', function (page) {
    cleatTimer();
    currentPage = 'favourite';
    GoToFavouritePage(page);
}).trigger();

myApp.onPageAfterAnimation('user', function (page) {
    cleatTimer();
    currentPage = 'user';
    GoToUserPage(page);
}).trigger();

myApp.onPageAfterAnimation('profile', function (page) {
    cleatTimer();
    currentPage = 'profile';
    GoToProfilePage(page);
}).trigger();

myApp.onPageAfterAnimation('addProduct', function (page) {
    cleatTimer();
    currentPage = 'addProduct';
    GoToAddProductPage(page);
}).trigger();

myApp.onPageAfterAnimation('productDetails', function (page) {
    cleatTimer();
    currentPage = 'productDetails';
    GoToProductDetails(page);
}).trigger();

myApp.onPageAfterAnimation('chat', function (page) {
    cleatTimer();
    currentPage = 'chat';
    GoToChatPage(page);
}).trigger();

myApp.onPageAfterAnimation('contact', function (page) {
    cleatTimer();
    currentPage = 'contact';
    GoToContactPage(page);
}).trigger();

myApp.onPageAfterAnimation('search', function (page) {
    cleatTimer();
    currentPage = 'search';
    GoToSearchPage(page);
}).trigger();

myApp.onPageAfterAnimation('searchResults', function (page) {
    cleatTimer();
    currentPage = 'searchResults';
    GoToSearchResultsPage(page);
}).trigger();

myApp.onPageAfterAnimation('changePassword', function (page) {
    cleatTimer();
    currentPage = 'changePassword';
    GoToChangePasswordPage(page);
}).trigger();

myApp.onPageAfterAnimation('resetPassword', function (page) {
    currentPage = 'resetPassword';
    GoToResetPasswordPage(page);
}).trigger();

myApp.onPageAfterAnimation('editProduct', function (page) {
    currentPage = 'editProduct';
    GoToEditProductPage(page);
}).trigger();

myApp.onPageAfterAnimation('allChats', function (page) {
    cleatTimer();
    currentPage = 'allChats';
    GoToAllChats(page);
}).trigger();

myApp.onPageAfterAnimation('notification', function (page) {
    cleatTimer();
    currentPage = 'notification';
    GotoNotificationPage(page);
}).trigger();

myApp.init();
