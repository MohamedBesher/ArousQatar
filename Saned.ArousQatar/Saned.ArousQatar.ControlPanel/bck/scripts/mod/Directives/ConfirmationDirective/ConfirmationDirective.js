angular.module('ConfirmationModule', ['ui.bootstrap']).service('modalService', ['$modal',
    function ($modal) {

        var modalDefaults = {
            backdrop: true,
            keyboard: true,
            modalFade: true,
            templateUrl: '/scripts/mod/Directives/ConfirmationDirective/Modal.html',
        };

        var modalOptions = {
            closeButtonText: 'غلق',
            actionButtonText: 'نعم',
            headerText: 'Proceed?',
            bodyText: 'Perform this action?'
        };

        this.showModal = function (customModalDefaults, customModalOptions) {
            if (!customModalDefaults) customModalDefaults = {};
            customModalDefaults.backdrop = 'static';
            return this.show(customModalDefaults, customModalOptions);
        };

        this.show = function (customModalDefaults, customModalOptions) {
            //Create temp objects to work with since we're in a singleton service
            var tempModalDefaults = {};
            var tempModalOptions = {};

            //Map angular-ui modal custom defaults to modal defaults defined in service
            angular.extend(tempModalDefaults, modalDefaults, customModalDefaults);

            //Map modal.html $scope custom properties to defaults defined in service
            angular.extend(tempModalOptions, modalOptions, customModalOptions);

            if (!tempModalDefaults.controller) {
                tempModalDefaults.controller = function ($scope, $modalInstance) {
                    $scope.modalOptions = tempModalOptions;
                    $scope.modalOptions.ok = function (result) {
                        $modalInstance.close(result);
                    };
                    $scope.modalOptions.close = function (result) {
                        $modalInstance.dismiss('cancel');
                    };
                };
            }

            return $modal.open(tempModalDefaults).result;
        };

    }])
        .directive('confButton', function () {
            return {
                restrict: 'EA',
                replace: false,
                scope: {
                    modalbtntext: '@',
                    btntext: '@',
                    iconclass: '@',
                    btnclass:'@',
                    callback: '&',
                    disabled: '='


                },
                templateUrl : '/scripts/mod/Directives/ConfirmationDirective/ConfrimationDirect.html',
                controller: ['$scope', '$element', '$attrs', '$transclude', 'modalService',
                    function ($scope, $element, $attsr, $transclude, modalService) {
                        $scope.disableButton = $scope.disabled;
                        $scope.open = function () {
                            var bodyMessage = '';
                            if ($scope.modalbtntext.toLowerCase() == "تعديل") {
                                bodyMessage = "هل انت متأكد من التعديل؟";
                            }
                            else{
                                bodyMessage = 'هل انت متأكد من حذف هذا العنصر؟';
                            }
                            var modalOptions = {
                                closeButtonText: 'إلغاء',
                                actionButtonText: $scope.modalbtntext,
                                headerText: 'من فضلك اكد طلبك',
                                bodyText: bodyMessage
                            };

                            modalService.showModal({}, modalOptions).then(function (result) {
                                $scope.callback();
                            });
                        };
                    }],
             


            };
    });

    