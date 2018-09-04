// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        var parentElement = document.getElementById('deviceready');

        window.addEventListener('native.hidekeyboard', keyboardHideHandler);
        window.addEventListener('native.showkeyboard', keyboardShowHandler);
        function keyboardHideHandler(e) {
            document.getElementById("addProductPGC").style.height = '93%';
            document.getElementById("editProductPGC").style.height = '93%';
            document.getElementById("contactPGC").style.height = '93%';
        }
        function keyboardShowHandler(e) {
            document.getElementById("addProductPGC").style.height = '80%';
            document.getElementById("editProductPGC").style.height = '80%';
            document.getElementById("contactPGC").style.height = '80%';
        }

        //cordova.plugins.Keyboard.disableScroll(true);
        cordova.plugins.Keyboard.hideKeyboardAccessoryBar(false);

        initAd();

    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };

    function initAd() {
        registerAdEvents();
    }


    function registerAdEvents() {
        $(document).on('onAdFailLoad', function (e) {
        });
        $(document).on('onAdLoaded', function (e) {
        });
        $(document).on('onAdPresent', function (e) {
        });
        $(document).on('onAdLeaveApp', function (e) {
        });
        $(document).on('onAdDismiss', function (e) {
        });
    }

    

} )();