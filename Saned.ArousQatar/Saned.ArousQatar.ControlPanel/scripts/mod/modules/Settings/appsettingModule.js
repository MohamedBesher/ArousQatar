angular
    .module('appsettingModule', [
        'SignalrDataModule'
    ])
  .controller('appsettingController', ['$scope', '$http', '$q', 'SignalrDataFactory',
      function ($scope, $http, $q, SignalrDataFactory) {
          'use strict';
          //debugger;
          $scope.validateSettingForm = {};
          $scope.form = {};
          $scope.canResetValidationForm = function () {
              return $scope.form.validateSettingForm.$dirty;
          };
          $scope.resetValidationForm = function () {
              $scope.validateSettingForm.isRequireApproveInRegister = '';
              $scope.validateSettingForm.isRequireApproveToAddTripInRegister = '';
              $scope.validateSettingForm.isRequireCheckBalanceBeforeAddTrip = '';
              $scope.validateSettingForm.numberOfTripBeforeDiscountFromBalance = '';
              $scope.validateSettingForm.disCountPrecent = '';
              $scope.form.validateSettingForm.$setPristine();
          };
          $scope.canSubmitValidationForm = function () {
              return $scope.form.validateSettingForm.$valid;
          };
          //Load Setting
          $scope.loadEditData = function () {

              SignalrDataFactory.GetSingle('/setting/').then(function (result) {
                  $scope.validateSettingForm = result.data;
              });
          };

          //Save Setting
          $scope.postSetting = function () {
              var settingEdit = {
                  isRequireApproveInRegister: $scope.validateSettingForm.isRequireApproveInRegister,
                  isRequireApproveToAddTripInRegister: $scope.validateSettingForm.isRequireApproveToAddTripInRegister,
                  isRequireCheckBalanceBeforeAddTrip: $scope.validateSettingForm.isRequireCheckBalanceBeforeAddTrip,
                  numberOfTripBeforeDiscountFromBalance: $scope.validateSettingForm.numberOfTripBeforeDiscountFromBalance,
                  disCountPrecent: $scope.validateSettingForm.disCountPrecent

              };
              // TODO :: 


              SignalrDataFactory.Put('setting/Save/', settingEdit).then(function (result) {

                  // TODO ::
                  if (result.status === 200) {
                    alert(" تم التعديل بنجاح");
                  }
              });

          };
          $scope.submitValidationForm = function () {
              debugger;
              $scope.postSetting();
          };
          $scope.loadEditData();
      }]);