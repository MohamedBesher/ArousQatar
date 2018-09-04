'use strict';
angular
    .module('loginModule', [])

    .controller('loginController', ['$scope', 'progressLoader', '$location', 'authService', 'appSettings', '$theme', function 
        ($scope, progressLoader, $location, authService, appSettings, $theme) {

        activateLogin();

        function activateLogin()
        {
            unBlockUI();
        }
        $scope.showerror = false;
        $theme.set('fullscreen', true);
        $scope.$on('$destroy', function () {
            $theme.set('fullscreen', false);
        });
        $scope.errormesage = 'errormesage';

        $scope.validateLoginForm = {};
        $scope.form = {};

        $scope.canResetValidationForm = function () {
            return $scope.form.validateLoginForm.$dirty;
        };

        $scope.resetValidationForm = function () {
            $scope.loginData.userName = '';
            $scope.loginData.password = '';
            $scope.loginData.useRefreshTokens = false;
            $scope.showerror = false;
            $scope.form.validateLoginForm.$setPristine();
        };

        $scope.canSubmitValidationForm = function () {
            return $scope.form.validateLoginForm.$valid;
        };

        $scope.loginData = {
            userName: "",
            password: "",
            useRefreshTokens: false
        };

        $scope.message = "";

        $scope.login = function () {
            progressLoader.start();
            authService.login($scope.loginData).then(function (response) {
                $scope.showerror = false;
                $location.path('/');

            },
             function (err) {
                 if (err.error_description === "The user name or password is incorrect.")
                     $scope.message = "خطأ في اسم المستخدم أو كلمة السر.";
                 else
                     $scope.message = err.error_description;

                 $scope.showerror = true;
             });
        };

        $scope.authExternalProvider = function (provider) {

            var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

            var externalProviderUrl = appSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                        + "&response_type=token&client_id=" + appSettings.clientId
                                                                        + "&redirect_uri=" + redirectUri;
            window.$windowScope = $scope;

            var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
        };

        $scope.authCompletedCB = function (fragment) {

            $scope.$apply(function () {

                if (fragment.haslocalaccount == 'False') {

                    authService.logOut();

                    authService.externalAuthData = {
                        provider: fragment.provider,
                        userName: fragment.external_user_name,
                        externalAccessToken: fragment.external_access_token
                    };

                    $location.path('/associate');

                }
                else {
                    //Obtain access token and redirect to orders
                    var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                    authService.obtainAccessToken(externalData).then(function (response) {

                        $location.path('/');

                    },
                 function (err) {
                     $scope.error = true;
                     $scope.message = err.error_description;
                 });
                }

            });
        }
    }]);
