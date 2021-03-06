angular
    .module('userModule', [
        'ngAnimate',
        'ngGrid',
        'SignalrDataModule'
    ])
    .controller('userController', [
        '$scope', '$filter', '$http', '$q', '$location', 'SignalrDataFactory',
        function ($scope, $filter, $http, $q, $location, SignalrDataFactory) {
            'use strict';
            //-----------------------------
            $scope.isPosting = true;
            $scope.isEditing = true;
            $scope.showDiv = true;
            $scope.DismissForm = function () {
                $scope.isPosting = true;
                $scope.isEditing = true;
                $scope.showDiv = true;
            }
            $scope.showEditDv = function () {
                //Clicked by the Add button
                $scope.isPosting = false;
                $scope.isEditing = true;
                $scope.showDiv = false;

            }
            $scope.cityPost = {};
            $scope.postCity = function () {
                var city = {
                    id: null,
                    name: $scope.cityPost.Name,
                    isDeleted: false,
                    Longitude: $scope.cityPost.longitude,
                    Latitude: $scope.cityPost.latitude

                };

                SignalrDataFactory.Post("Municipality/SaveCity/", city).then(function (result) {
                    if (result.status === 200) {
                        $scope.MainData.push(result.data);
                        $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                    }

                });


                $scope.isPosting = true;
                $scope.isEditing = true;
                $scope.showDiv = true;
                $scope.cityPost = {};
            };
            //Deleteing all seleceted cities 
            $scope.deleteCity = function () {
                for (city in $scope.gridOptions.selectedItems) {
                    var id = $scope.gridOptions.selectedItems[city].id;
                    SignalrDataFactory.Delete('Users/Delete/', $scope.gridOptions.selectedItems[city].id).then(function (result) {
                        $scope.MainData = $scope.MainData.filter(function (obj) {
                            obj.id !== id;

                        });

                    });

                }
                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };
            $scope.userEditObj = {};

            //Load User Date
            $scope.loadEditData = function () {
                $scope.isAdding = false;
                $scope.isEdtiing = true;

                var edituser = $scope.gridOptions.selectedItems[0];

                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle('User/GetDetails/', edituser.id).then(function (result) {
                    $scope.userEditObj = result.data;
                    $scope.showDiv = false;
                    $scope.isPosting = true;
                    $scope.isEditing = false;
                });
            }
            //Editing Send
            $scope.editCitySend = function () {
                var cityEdit = {
                    id: $scope.cityEditObj.id,
                    Name: $scope.cityEditObj.name,
                    Longitude: $scope.cityEditObj.longitude,
                    Latitude: $scope.cityEditObj.latitude,
                    IsDeleted: $scope.cityEditObj.isDeleted

                };
                // TODO :: 


                SignalrDataFactory.Put('Municipality/SaveCity/', cityEdit).then(function (result) {

                    // TODO ::
                    if (result.status === 200) {
                        for (var i = 0; i < $scope.MainData.length; i++) {
                            if ($scope.MainData[i].id === result.data.id) {
                                $scope.MainData[i].name = result.data.name;
                                $scope.MainData[i].isDeleted = result.isDeleted;
                                $scope.MainData[i].longitude = result.longitude;
                                $scope.MainData[i].latitude = result.latitude;
                                break;
                            }
                        }
                        $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                    }
                });
                $scope.isPosting = true;
                $scope.isEditing = true;
                $scope.showDiv = true;
            }
            //-----------------------------------------------------------------
            //----------------------- NG-GRID CONFIGURATION -------------------
            //$scope.MainData is the Array containing the elements in the grid-
            $scope.MainData = [];
            $scope.filterOptions = {
                filterText: '',
                useExternalFilter: true
            };
            $scope.totalServerItems = 0;
            $scope.pagingOptions = {
                pageSizes: [10, 25, 50, 100],
                pageSize: 10,
                currentPage: 1
            };
            $scope.setPagingData = function (data, page, pageSize) {
                //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
                $scope.myData = data; //pagedData;
                $scope.totalServerItems = pageSize;
                console.log(pageSize);

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
            };
            $scope.getPagedDataAsync = function (pageSize, page, searchText) {
                //setTimeout(function () {
                var pagingViewModel = {
                    pageNumber: $scope.pagingOptions.currentPage,
                    pageSize: $scope.pagingOptions.pageSize,
                    filter: searchText,
                    isDelete: false

                };
                var data;
                if (searchText) {
                    var ft = searchText.toLowerCase();

                    SignalrDataFactory.PostPaging('User/GetAll', pagingViewModel).then(function (result) {
                        $scope.MainData = result.data.items.filter(function (item) {
                            //return JSON.stringify(item).toLowerCase().indexOf(ft) !== -1;
                            $scope.totalServerItems = result.data.TotalCount;
                            return JSON.stringify(item);
                        });
                        $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                    });
                } else {

                    SignalrDataFactory.PostPaging('User/GetAll', pagingViewModel).then(function (result) {
                        $scope.MainData = result.data.items;
                        $scope.totalServerItems = result.data.TotalCount;
                        $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                    });


                }
                //}, 100);
            };

            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

            $scope.$watch('pagingOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
                }
            }, true);
            $scope.$watch('filterOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
                }
            }, true);
            $scope.gridOptions = {
                data: 'myData',
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    //{ field: 'longitude', displayName: 'خط الطول' },
                    //{ field: 'latitude', displayName: 'خط العرض' },
                    { field: 'id', displayName: 'رقم' },
                    { field: 'name', displayName: 'الاسم' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
            //-----------------------------------------------------------------
            //----------------------- NG-GRID CONFIGURATION -------------------
            //-----------------------------------------------------------------
            $scope.go = function (path) {
                $location.path(path).search({ param: $scope.gridOptions.selectedItems[0].id });
            };
        }
    ])
    .controller('userProfileController', [
        '$scope', '$filter', '$http', '$q', '$location', 'SignalrDataFactory',
        function ($scope, $filter, $http, $q, $location, SignalrDataFactory) {
            'use strict';
            //-----------------------------
            $scope.userEditObj = {};

            $scope.userlblObj = {
                Name: 'الاسم بالكامل',
                Email: 'البريد الإلكتروني',
                UserName: 'اسم المستخدم',
                Municipality: 'المدينة',
                PhoneNumber: 'رقم الهاتف',
                IsApprove: 'الحالة',
                Gender: 'النوع',
                Approved: "تمت الموافقة",
                NeedApprove: "يحتاج إلي الموافقة"
            };
            //Load User Date
            $scope.loadEditData = function () {

                SignalrDataFactory.GetSingle('User/GetDetails/', $location.search().param).then(function (result) {
                    $scope.userEditObj = result.data;

                });
            };
            $scope.loadEditData();
            $scope.go = function (path) {
                $location.path(path).search({ param: $scope.gridOptions.selectedItems[0].id });
            };
        }
    ])
    .controller('userTransController', [
        '$scope', '$filter', '$http', '$location', 'SignalrDataFactory',
        function ($scope, $filter, $http, $location, SignalrDataFactory) {

            //0-  Names And Labal
            $scope.userNameObj = {
                AddBalance: 'إضافة رصيد',
                Amount: 'المبلغ',
                Notes: 'ملاحظة',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FieldIsNumber: 'يجب إدخال أرقام فقط',
                FieldMin: 'يجب إدخال رقم اكبر من الصفر',
                FielMaxlength: 'النص المكتوب اكبر من 250 حرف'
            };
            //1- Add Balance [View/hide Controls In Div]
            $scope.BalanceAddObj = {};
            $scope.form = {};

            // Variables to control view and hide form to add or edit
            $scope.hidePosting = true;
            $scope.hideFromsInDiv = true;

            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hidePosting = true;
                $scope.hideFromsInDiv = true;

                //emtpy input from date
                $scope.resetValidationForm();
            };

            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hidePosting = false;

            };

            // Save Balance (Call Api Method)
            $scope.eventPostBalance = function () {
                //Insert Balance
                var balance = {
                    id: null,
                    notes: $scope.BalanceAddObj.notes,
                    amount: $scope.BalanceAddObj.amount,
                    userId: $location.search().param


                };
                SignalrDataFactory.Post("Transaction/AddBalance/", balance).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };

            //
            $scope.canSubmitValidationForm = function () {
                return $scope.form.BalanceAddObj.$valid;
            };
            //

            $scope.resetValidationForm = function () {


                $scope.BalanceAddObj.amount = null;
                $scope.BalanceAddObj.notes = null;

                $scope.form.BalanceAddObj.$setPristine(true);
                $scope.form.BalanceAddObj.$setUntouched();

            };

            //2- Load List Of Transaction [ng-Grid]
            $scope.MainData = [];
            $scope.filterOptions = {
                filterText: '',
                useExternalFilter: true
            };
            $scope.totalServerItems = 0;
            $scope.pagingOptions = {
                pageSizes: [10, 25, 50, 100],
                pageSize: 10,
                currentPage: 1
            };
            $scope.setPagingData = function (data, page, pageSize) {

                $scope.myData = data;
                $scope.totalServerItems = pageSize;
                console.log(pageSize);

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
            };
            $scope.getPagedDataAsync = function (pageSize, page, searchText) {
                //setTimeout(function () {
                var pagingViewModel = {
                    pageNumber: $scope.pagingOptions.currentPage,
                    pageSize: $scope.pagingOptions.pageSize,
                    filter: searchText,
                    userId: $location.search().param

                };
                var data;
                if (searchText) {
                    var ft = searchText.toLowerCase();

                    SignalrDataFactory.PostPaging('Transaction/GetAll', pagingViewModel).then(function (result) {
                        $scope.MainData = result.data.items.filter(function (item) {
                            $scope.totalServerItems = result.data.TotalCount;
                            return JSON.stringify(item);
                        });
                        $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                    });
                } else {

                    SignalrDataFactory.PostPaging('Transaction/GetTransactions', pagingViewModel).then(function (result) {
                        $scope.MainData = result.data.items;
                        $scope.totalServerItems = result.data.TotalCount;
                        $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                    });


                }
                //}, 100);
            };
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
            $scope.$watch('pagingOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
                }
            }, true);
            $scope.$watch('filterOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
                }
            }, true);
            $scope.gridOptions = {
                data: 'myData',
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    { field: 'notes', displayName: 'ملحوظة' },
                    { field: 'transactionDate', displayName: 'تاريخ المعاملة' },
                    { displayName: 'نوع المعاملة', cellTemplate: "<div>{{row.entity.isDepositAmount==true?'إيداع':'سحب'}}</div>" },
                    { field: 'amount', displayName: 'المبلغ' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('userOrderController', [
        '$scope', '$filter', '$http', '$location', 'SignalrDataFactory',
        function ($scope, $filter, $http, $location, SignalrDataFactory) {


            //2- Load List Of Transaction [ng-Grid]
            $scope.MainData = [];
            $scope.filterOptions = {
                filterText: '',
                useExternalFilter: true
            };
            $scope.totalServerItems = 0;
            $scope.pagingOptions = {
                pageSizes: [10, 25, 50, 100],
                pageSize: 10,
                currentPage: 1
            };
            $scope.setPagingData = function (data, page, pageSize) {

                $scope.myData = data;
                $scope.totalServerItems = pageSize;
                console.log(pageSize);

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
            };
            $scope.getPagedDataAsync = function (pageSize, page, searchText) {
                //setTimeout(function () {
                var pagingViewModel = {
                    pageNumber: $scope.pagingOptions.currentPage,
                    pageSize: $scope.pagingOptions.pageSize,
                    filter: searchText,
                    userId: $location.search().param

                };
                var data;
                if (searchText) {
                    var ft = searchText.toLowerCase();
                    debugger;
                    SignalrDataFactory.PostPaging('Transaction/GetListOrders', pagingViewModel).then(function (result) {
                        $scope.MainData = result.data.items.filter(function (item) {
                            $scope.totalServerItems = result.data.TotalCount;
                            return JSON.stringify(item);
                        });
                        $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                    });
                } else {

                    SignalrDataFactory.PostPaging('Transaction/GetListOrders', pagingViewModel).then(function (result) {
                        $scope.MainData = result.data.items;
                        $scope.totalServerItems = result.data.TotalCount;
                        $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                    });


                }
                //}, 100);
            };
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
            $scope.$watch('pagingOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
                }
            }, true);
            $scope.$watch('filterOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
                }
            }, true);
            $scope.gridOptions = {
                data: 'myData',
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                        
                           { displayName: 'حالة الدفع', cellTemplate: "<div>{{row.entity.isPaided==true?'مدفوع':'غيرمدفوع'}}</div>" },
                   { displayName: 'نسبة الإدارة', field: 'applicationAmount' },
                    { displayName: 'الربح', field: 'totlaAmount' },
  { displayName: 'عدد النقلات على هذه الرحلة', field: 'luggageCount' },
                  { displayName: 'مكان المغادرة', field: 'departure' },
                   { displayName: 'موعد المغادرة', field: 'departureDate', cellFilter: 'date:\'yyyy-MM-dd\'' },
                  { displayName: 'مكان الوصول', field: 'destination' },
                   { displayName: 'موعد الوصول', field: 'destinatioDate' , cellFilter: 'date:\'yyyy-MM-dd\''},
                   { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ]);