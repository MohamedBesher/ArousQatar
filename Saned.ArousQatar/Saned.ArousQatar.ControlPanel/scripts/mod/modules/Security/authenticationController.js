angular
.module('theme.core.authentication_controller', ['theme.core.services'])
.controller('AuthenticationController', ['$scope', '$rootScope', '$location', '$theme', 'AuthenticationService',
    function ($scope, $rootScope, $location, $theme, authenticationService) {

        $theme.set('fullscreen', true);

        $scope.errormesage = 'errormesage';
        $scope.$on('$destroy', function () {
            $theme.set('fullscreen', false);
        });
        //authenticationService.ClearCredentials();

        $scope.login = function () {
            $scope.dataLoading = true;
            authenticationService.Login($scope.username, $scope.password, function (response) {
                if (response.access_token) {

                    authenticationService.SetCredentials(response.access_token, response.token_type, response.expires_in);
                    $location.path('/');

                } else {
                    console.log(response.message);
                    $scope.error = response.message;
                    $scope.dataLoading = false;
                }
            }
            );
        };

        $scope.validateLoginForm = {};
        $scope.form = {};

        $scope.canResetValidationForm = function () {
            return $scope.form.validateLoginForm.$dirty;
        };

        $scope.resetValidationForm = function () {
            $scope.validateLoginForm.username = '';
            //$scope.validateLoginForm.required = '';
            //$scope.validateLoginForm.minlength = '';
            //$scope.validateLoginForm.maxlength = '';
            //$scope.validateLoginForm.rangelength = '';
            //$scope.validateLoginForm.pattern = '';
            //$scope.validateLoginForm.email = '';
            //$scope.validateLoginForm.url = '';
            //$scope.validateLoginForm.digits = '';
            //$scope.validateLoginForm.digits_min = '';
            //$scope.validateLoginForm.digits_max = '';
            //$scope.validateLoginForm.digits_minmax = '';
            //$scope.validateLoginForm.alphanumeric = '';
            $scope.validateLoginForm.password = '';
            //$scope.validateLoginForm.confirm_password = '';
            //$scope.validateLoginForm.terms = '';
            $scope.form.validateLoginForm.$setPristine();
        };

        $scope.canSubmitValidationForm = function () {
            return $scope.form.validateLoginForm.$valid;
        };

        $scope.submit = function () {

            $scope.dataLoading = true;
            authenticationService.Login($scope.username, $scope.password, function (response) {
                console.log("---Trace AuthenticationController-- In submit response'" + response + "' ");
                if (response.access_token) {
                    //$scope.$apply(function () {
                    debugger;
                    if ($scope.rememberMe)
                        authenticationService.SetRememberMe($scope.username, $scope.password);

                    authenticationService.SetCredentials(response.access_token, response.token_type, response.expires_in);
                    $location.path('/');
                    //});
                    //$timeout(function () {
                    //    $scope.someData = someData;
                    //}, 0);

                } else {
                    $scope.dataLoading = false;
                    //$scope.$apply(function () {
                    $scope.error = appUserMessage.loginFail;
                    //});
                }
            }
           );

        };

    }]);