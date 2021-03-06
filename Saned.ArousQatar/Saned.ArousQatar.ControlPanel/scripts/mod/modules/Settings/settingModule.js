angular
    .module('settingModule', [
        'ngAnimate',
        'ngGrid',
        'SignalrDataModule',
        'naif.base64'

    ])
    .directive('pwCheck', [
        function () {
            return {
                require: 'ngModel',
                link: function (scope, elem, attrs, ctrl) {
                    var firstPassword = '#' + attrs.pwCheck;
                    elem.add(firstPassword).on('keyup', function () {
                        scope.$apply(function () {
                            // console.info(elem.val() === $(firstPassword).val());
                            ctrl.$setValidity('pwmatch', elem.val() === $(firstPassword).val());
                        });
                    });
                }
            }
        }
    ])
    .controller('categoryController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {

            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'إدارة التصنيفات',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة تصنيف',
                EditDivTitle: 'تعديل تصنيف',
                Cancel: 'إلغاء',
                Setting: 'الإعدادات',
                Save: 'حفظ',
                Name: 'اسم التصنيف',
                IconName: 'اسم الاقونة',
                ImageUrl: 'الصوره',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Search: 'بحث',
                List: 'قائمة التنصنيفات'
            };
            //
            $scope.CategoryAddObj = {};
            $scope.CategoryEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;

                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };
            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;
            };
            // Save/Add Category (Call Api Method)
            $scope.eventAddCategory = function () {
                blockUI();
                //Insert Catgory
                var category = {
                    id: 0,
                    Name: $scope.CategoryAddObj.name,
                    IconName: $scope.CategoryAddObj.iconName,
                    ImageUrl: $scope.CategoryAddObj.imageUr,
                    ImageFilename: $scope.CategoryAddObj.Image.filename,
                    ImageBase64: $scope.CategoryAddObj.Image.base64

                };
                SignalrDataFactory.Post("category/add", category).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                        $scope.eventCloseForm();
                    }

                });

            };
            // Save/Edit Category (Call Api Method)
            $scope.eventEditCategory = function () {
                blockUI();
                var category = {
                    id: $scope.CategoryEditObj.id,
                    Name: $scope.CategoryEditObj.name,
                    IconName: $scope.CategoryEditObj.iconName,
                    ImageUrl: $scope.CategoryEditObj.imageUr
                };
                if ($scope.CategoryEditObj.Image) {
                    category.ImageFilename = $scope.CategoryEditObj.Image.filename;
                    category.ImageBase64 = $scope.CategoryEditObj.Image.base64;
                }

                SignalrDataFactory.Post("category/edit/" + $scope.CategoryEditObj.id, category).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

                    }

                });


            };
            // View Category Record To Edit
            $scope.eventViewCategory = function () {
                blockUI();
                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedCategory = $scope.gridOptions.selectedItems[0];
                //
                SignalrDataFactory.GetSingle('category/', editedCategory.id).then(function (result) {
                    unBlockUI();
                    $scope.CategoryEditObj = result.data;
                    $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.CategoryAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.CategoryEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.CategoryAddObj = {};
                $scope.form.CategoryAddObj.$setPristine(true);
                $scope.form.CategoryAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.CategoryEditObj = {};
                $scope.form.CategoryEditObj.$setPristine(true);
                $scope.form.CategoryEditObj.$setUntouched();
            };
            //
            $scope.deleteCategory = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete('category/delete/', id).then(function (result) {
                    if (result.status === 200) {
                        unBlockUI();
                        if (result.data.isDeleted) {
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                        } else {
                            alert(result.data.errorMessage);
                        }
                    }
                });
                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
            };
            //2- Load List Of Categories [ng-Grid]
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
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;

                SignalrDataFactory.PostPaging('category/GetAllAdmin', pagingViewModel).then(function (result) {
                    unBlockUI();
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                rowHeight: 50,
                columnDefs:
                [
                    { field: 'imageUrl', cellTemplate: '<img ng-src="http://api.arousqatar.com/uploads/{{row.getProperty(\'imageUrl\')}}" class="img-circle img-responsive" style="height:50px;width:50px;margin-left: 25%;" />', displayName: 'صورة التصنيف' },
                    { field: 'iconName', displayName: 'الايقونه' },
                    { field: 'name', displayName: 'الاسم' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ],
                afterSelectionChange: function (data) {
                    if (Object.prototype.toString.call(data) !== '[object Array]') {
                        $scope.eventCloseForm();
                    }
                }
            };
        }
    ])
    .controller('PackagesController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "advertisementPrice";
            // 
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'الباقات',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة باقة',
                EditDivTitle: 'تعديل باقة',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Period: 'الفترة(الايام)',
                Price: 'السعر',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'قائمة الباقات'
            };
            //
            $scope.PackagesAddObj = {};
            $scope.PackagesEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;

                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };
            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;
            };
            // Save/Add Packages (Call Api Method)
            $scope.eventAddPackages = function () {
                blockUI();
                //Insert Packages
                var Packages = {
                    Id: $scope.PackagesAddObj.id,
                    Period: $scope.PackagesAddObj.period,
                    Price: $scope.PackagesAddObj.price
                };
                SignalrDataFactory.Post(controller + "/update/", Packages).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        if (result.data.error) {
                            alert(result.data.error)
                        } else {
                            //Hide Div After Save
                            $scope.eventCloseForm();
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                        }
                    }
                });

            };
            // Save/Edit Packages (Call Api Method)
            $scope.eventEditPackages = function () {
                debugger;
                //Insert Packages
                blockUI();
                var Packages = {
                    Id: $scope.PackagesEditObj.id,
                    Period: $scope.PackagesEditObj.period,
                    Price: $scope.PackagesEditObj.price
                };
                SignalrDataFactory.Post(controller + "/update/", Packages).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {

                        if (result.data.error) {
                            alert(result.data.error)
                        } else {
                            //Hide Div After Save
                            $scope.eventCloseForm();
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                        }
                    }

                });

            };
            // View Packages Record To Edit
            $scope.eventViewPackages = function () {
                blockUI();
                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedPackages = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/GetSingleAdmin/', editedPackages.id).then(function (result) {
                    unBlockUI();
                    //$scope.PackagesEditObj = result.data;
                    $scope.PackagesEditObj.period = parseInt(result.data.period);
                    $scope.PackagesEditObj.price = result.data.price;
                    $scope.PackagesEditObj.id = result.data.id;
                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.PackagesAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.PackagesEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.PackagesAddObj = {};
                $scope.form.PackagesAddObj.$setPristine(true);
                $scope.form.PackagesAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.PackagesEditObj = {};
                $scope.form.PackagesEditObj.$setPristine(true);
                $scope.form.PackagesEditObj.$setUntouched();
            };
            //
            $scope.deletePackages = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        if (result.data.isDeleted) {
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                        } else {
                            alert(result.data.errorMessage);
                        }
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };
            //2- Load List Of Categories [ng-Grid]
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
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;
                SignalrDataFactory.PostPaging(controller + '/allAdmin', pagingViewModel).then(function (result) {
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);
                    unBlockUI();
                });

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
                afterSelectionChange: function (data) {
                    if (Object.prototype.toString.call(data) !== '[object Array]') {
                        $scope.eventCloseForm();
                    }
                },
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    { field: 'price', displayName: 'السعر' },
                    { field: 'period', displayName: 'الفترة(الايام)' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('ContactUsController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "contactInformation";
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'معلومات الاتصال',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة معلومات',
                EditDivTitle: 'تعديل معلومات',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Contact: 'بيانات وسيلة التواصل',
                IconName: 'الايقونة',
                ChooseContactType: 'اختر نوع وسيلة التواصل',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'معلومات الاتصال'
            };
            $scope.ContactUsAddObj = {};
            $scope.ContactUsEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;

                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };
            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;


            };
            // Save/Add ContactUs (Call Api Method)
            $scope.eventAddContactUs = function () {
                blockUI();
                //Insert ContactUs
                var ContactUs = {
                    Id: 0,
                    Contact: $scope.ContactUsAddObj.contact,
                    ContactTypeId: $scope.ContactUsAddObj.contactTypeId,
                    IconName: $scope.ContactUsAddObj.iconName
                };
                SignalrDataFactory.Post(controller + "/update", ContactUs).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();

                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

                    }

                });

            };
            // Save/Edit ContactUs (Call Api Method)
            $scope.eventEditContactUs = function () {
                //Insert ContactUs
                blockUI();
                var ContactUs = {
                    Id: $scope.ContactUsEditObj.id,
                    Contact: $scope.ContactUsEditObj.contact,
                    ContactTypeId: $scope.ContactUsEditObj.contactTypeId,
                    IconName: $scope.ContactUsEditObj.iconName
                };
                SignalrDataFactory.Post(controller + "/update", ContactUs).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });

            };
            // View ContactUs Record To Edit
            $scope.eventViewContactUs = function () {
                blockUI();
                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedContactUs = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/GetSingleAdmin/', editedContactUs.id).then(function (result) {
                    unBlockUI();
                    $scope.ContactUsEditObj = result.data;

                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.ContactUsAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.ContactUsEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.ContactUsAddObj = {};
                $scope.form.ContactUsAddObj.$setPristine(true);
                $scope.form.ContactUsAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.ContactUsEditObj = {};
                $scope.form.ContactUsEditObj.$setPristine(true);
                $scope.form.ContactUsEditObj.$setUntouched();
            };
            //
            $scope.deleteContactUs = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };

            SignalrDataFactory.GetAll("contactType/GetAll/").then(function (result) {
                $scope.ContactType = result.data;
            });
            //2- Load List Of Categories [ng-Grid]
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
            $scope.ChangePaidedMethod = function () {
                if (event.target.value == "1") {
                 
                }
                else if (event.target.value == "1") {

                }
                else if (event.target.value == "1") {

                }
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
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;

                SignalrDataFactory.PostPaging(controller + '/allAdmin', pagingViewModel).then(function (result) {
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);
                    unBlockUI();

                });

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
                afterSelectionChange: function (data) {
                    if (Object.prototype.toString.call(data) !== '[object Array]') {
                        $scope.eventCloseForm();
                    }
                },
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [


                    { field: 'iconName', displayName: 'الايقونة' },
                    { field: 'contact', displayName: 'وسيلة الاتصال' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('ContactTypeController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "contactType";
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'معلومات الاتصال',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة معلومات',
                EditDivTitle: 'تعديل معلومات',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Type: 'النوع',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'معلومات الاتصال'
            };
            //
            $scope.ContactTypeAddObj = {};
            $scope.ContactTypeEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;

                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };
            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;


            };
            // Save/Add ContactType (Call Api Method)
            $scope.eventAddContactType = function () {
                //Insert ContactType
                var ContactType = {
                    Id: 0,
                    Type: $scope.ContactTypeEditObj.type
                };
                SignalrDataFactory.Post(controller + "/update", ContactType).then(function (result) {
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();

                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

                    }

                });

            };
            // Save/Edit ContactType (Call Api Method)
            $scope.eventEditContactType = function () {
                //Insert ContactType

                var ContactType = {
                    Id: $scope.ContactTypeEditObj.id,
                    Type: $scope.ContactTypeEditObj.type
                };
                SignalrDataFactory.Post(controller + "/update/", ContactType).then(function (result) {
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });

            };
            // View ContactType Record To Edit
            $scope.eventViewContactType = function () {

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedContactType = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/', editedContactType.id).then(function (result) {

                    $scope.ContactTypeEditObj = result.data;

                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.ContactTypeAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.ContactTypeEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.ContactTypeAddObj.name = null;
                $scope.form.ContactTypeAddObj.$setPristine(true);
                $scope.form.ContactTypeAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.ContactTypeEditObj.Name = null;
                $scope.form.ContactTypeEditObj.$setPristine(true);
                $scope.form.ContactTypeEditObj.$setUntouched();
            };
            //
            $scope.deleteContactType = function () {

                var id = $scope.gridOptions.selectedItems[0].Id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };
            //2- Load List Of Categories [ng-Grid]
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

                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;

                SignalrDataFactory.PostPaging(controller + '/all', pagingViewModel).then(function (result) {
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.TotalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.TotalCount);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    { field: 'type', displayName: 'النوع' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('AdvertisementController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            // controller name
            var controller = "advertisement";

            //preview image
            //$scope.previewImage = function () {
            //    debugger;
            //    var input = this;
            //    if (input.files && input.files[0]) {
            //        var reader = new FileReader();

            //        reader.onload = function (e) {
            //            $('#preview-image').attr('src', e.target.result);
            //        }

            //        reader.readAsDataURL(input.files[0]);
            //    }

            //}

            // extend date function to add days to a given date
            Date.prototype.addDays = function (days) {
                var dat = new Date(this.valueOf());
                dat.setDate(dat.getDate() + days);
                return dat;
            }
            // localize object
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'الاعلانات المدفوعة والغير مدفوعة',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة اعلان',
                EditDivTitle: 'تعديل اعلان',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                ChooseCategory: 'اختر الصنف',
                ChooseAdvertismentPrice: 'اختر الباقة',
                Description: 'الوصف',
                IsPaided: 'مدفوع',
                Name: 'العنوان',
                ImageUrl: 'الصورة',
                PaidEdPrice: 'السعر',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'معلومات الاتصال',
                startDate: 'تاريخ بداية الأعلان',
                endDate: 'تاريخ نهاية الأعلان',
                IsActive : 'فعال'
            };

            // add object 
            $scope.AdvertisementAddObj = {};
            // edit object
            $scope.AdvertisementEditObj = {};

            $scope.form = {};

            // update end date in case of start date changed 
            $scope.startDateChanged = function () {
                updateEndDate();
            }

            // if user change package period we will reset both start date and end date
            $scope.advertismentPriceIdChanged = function () {
                if ($scope.hideEdit) {
                    $scope.AdvertisementAddObj.endDate = '';
                    $scope.AdvertisementAddObj.startDate = '';
                } else {

                    $scope.AdvertisementEditObj.endDate = '';
                    $scope.AdvertisementEditObj.startDate = '';
                }
            }

            // update end date by add package period days to start date 
            function updateEndDate() {
                var currentAd = {};
                if ($scope.hideEdit) {
                    currentAd = $scope.AdvertisementAddObj;
                } else if ($scope.hideAdd) {
                    currentAd = $scope.AdvertisementEditObj;
                }

                var startDate = currentAd.startDate;
                var periodInDays;
                if (startDate && currentAd.advertismentPriceId) {
                    for (var i = 0; i < $scope.AdvertismentPrice.length; i++) {
                        if ($scope.AdvertismentPrice[i].id == currentAd.advertismentPriceId) {
                            periodInDays = parseInt($scope.AdvertismentPrice[i].period);
                        }
                    }
                    if ($scope.hideEdit) {
                        $scope.AdvertisementAddObj.endDate = startDate;
                        $scope.AdvertisementAddObj.endDate = $scope.AdvertisementAddObj.endDate.addDays(periodInDays)
                    } else {
                        $scope.AdvertisementEditObj.endDate = startDate;
                        $scope.AdvertisementEditObj.endDate = $scope.AdvertisementEditObj.endDate.addDays(periodInDays)

                    }
                } else {
                    if ($scope.hideEdit) {
                        $scope.AdvertisementAddObj.endDate = '';
                    } else {
                        $scope.AdvertisementEditObj.endDate = '';
                    }
                }
            }

            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            $scope.hideIsPaid = true;

            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {
                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;
                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };

            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.AdvertisementAddObj.$valid;
            };

            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.AdvertisementEditObj.$valid;
            };

            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {

                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;
            };

            // Save/Add Advertisement (Call Api Method)
            $scope.eventAddAdvertisement = function () {
                blockUI();
                //Insert Advertisement
                if ($scope.AdvertisementAddObj.isPaided == "0") {
                    $scope.AdvertisementAddObj.isPaided = false;
                } else {
                    $scope.AdvertisementAddObj.isPaided = true;
                }

                var Advertisement = {
                    Id: 0,
                    CategoryId: $scope.AdvertisementAddObj.categoryId,
                    Description: $scope.AdvertisementAddObj.description,
                    IsPaided: $scope.AdvertisementAddObj.isPaided,
                    Name: $scope.AdvertisementAddObj.name,
                    ImageUrl: $scope.AdvertisementAddObj.imageUrl,
                    PaidEdPrice: $scope.AdvertisementAddObj.paidEdPrice,
                    AdvertismentPriceId: $scope.AdvertisementAddObj.advertismentPriceId,
                    ImageFilename: $scope.AdvertisementAddObj.Image.filename,
                    ImageBase64: $scope.AdvertisementAddObj.Image.base64,
                    IsActive: $scope.AdvertisementAddObj.isActive
                };

                if ($scope.AdvertisementAddObj.startDate) {
                    Advertisement.StartDate = $scope.AdvertisementAddObj.startDate.toDateString();
                }

                if ($scope.AdvertisementAddObj.endDate) {
                    Advertisement.EndDate = $scope.AdvertisementAddObj.endDate.toDateString();
                }

                SignalrDataFactory.Post(controller + "/save", Advertisement).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.categoryId, $scope.filterOptions.isPaided, $scope.filterOptions.userId);
                    }
                });

            };
           
            // Save/Edit Advertisement (Call Api Method)
            $scope.eventEditAdvertisement = function () {

              
                blockUI();
                //Insert Advertisement
                if ($scope.AdvertisementEditObj.isPaided == "0") {
                    $scope.AdvertisementEditObj.isPaided = false;
                } else {
                    $scope.AdvertisementEditObj.isPaided = true;
                }
                //image.id - id
                //image.upload.base64, imageUrl
                var images = [];
                angular.forEach($scope.AdvertisementEditObj.imagesList, function (value, key) {
                    var base = "";
                    if (!$.isEmptyObject(value.upload) &&
                        value.upload != null &&
                        typeof value.upload !== "undefined" &&
                        typeof value.upload.base64 !== "undefined" &&
                        value.upload.base64 !== "" &&
                        !Number.isInteger(value.imageUrl))
                    {

                        base = value.upload.base64;
                        this.push({ id: value.id, imageUrl: base });
                    } else if (value.imageUrl !== "" && value.id !== 0) {
                        this.push({ id: value.id, imageUrl:"" });

                        
                    }

                       
                }, images);

                //console.log(log);
                //return;

                var Advertisement = {
                    Id: $scope.AdvertisementEditObj.id,
                    CategoryId: $scope.AdvertisementEditObj.categoryId,
                    Cost: $scope.AdvertisementEditObj.cost,
                    Description: $scope.AdvertisementEditObj.description,
                    IsPaided: $scope.AdvertisementEditObj.isPaided,
                    Name: $scope.AdvertisementEditObj.name,
                    ImageUrl: $scope.AdvertisementEditObj.imageUrl,
                    PaidEdPrice: $scope.AdvertisementEditObj.paidEdPrice,
                    AdvertismentPriceId: $scope.AdvertisementEditObj.advertismentPriceId,
                    IsActive: $scope.AdvertisementEditObj.isActive,
                   ImagesList: images
                   // ImagesList: $scope.AdvertisementEditObj.imagesList


                };

                if ($scope.AdvertisementEditObj.Image) {
                    Advertisement.ImageFilename = $scope.AdvertisementEditObj.Image.filename;
                    Advertisement.ImageBase64 = $scope.AdvertisementEditObj.Image.base64;
                }

                if ($scope.AdvertisementEditObj.startDate) {
                    Advertisement.StartDate = $scope.AdvertisementEditObj.startDate.toDateString();
                }

                if ($scope.AdvertisementEditObj.endDate) {
                    Advertisement.EndDate = $scope.AdvertisementEditObj.endDate.toDateString();
                }

                SignalrDataFactory.Post(controller + "/SaveAds/", Advertisement)
                    .then(function (result) {
                        if (result.status === 200) {
                            //Hide Div After Save
                            $scope.eventCloseForm();
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.categoryId, $scope.filterOptions.isPaided, $scope.filterOptions.userId);

                        }
                        unBlockUI();
                    });

            };

            // View Advertisement Record To Edit
            $scope.eventViewAdvertisement = function () {
                blockUI();
                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedAdvertisement = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/GetSingleAdmin/', editedAdvertisement.id).then(function (result) {

                    unBlockUI();
                    if (result.data.startDate) {
                        result.data.startDate = new Date(result.data.startDate);
                    }
                    if (result.data.endDate) {
                        result.data.endDate = new Date(result.data.endDate);
                    }
                    console.log(result.data);
                    $scope.AdvertisementEditObj = result.data;

                    //#region Make Image 12 images                                     
                   var imgesLength = $scope.AdvertisementEditObj.imagesList.length;
                   for (var i = imgesLength; i < 12; i++) {
                       var newItem = {
                           id: 0,
                           imageUrl: "",
                           imageId: i,
                           upload: {}
                       }
                       $scope.AdvertisementEditObj.imagesList.push(newItem);

                    }
                
                    //#endregion 
                    if ($scope.AdvertisementEditObj.isPaided == false) {
                        $scope.AdvertisementEditObj.isPaided = "0";
                        $scope.hideIsPaid = true;
                    } else {
                        $scope.AdvertisementEditObj.isPaided = "1";
                        $scope.hideIsPaid = false;
                    }

                });
            }


            $scope.resetValidationAddForm = function () {
                $scope.AdvertisementAddObj = {};
                $scope.form.AdvertisementAddObj.$setPristine(true);
                $scope.form.AdvertisementAddObj.$setUntouched();
            };

            $scope.resetValidationEditForm = function () {
                $scope.AdvertisementEditObj = {};
                $scope.form.AdvertisementEditObj.$setPristine(true);
                $scope.form.AdvertisementEditObj.$setUntouched();
            };

            $scope.deleteAdvertisement = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/Deleteads/', id).then(function (result) {
                    if (result.status === 200) {
                        unBlockUI();
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.categoryId, $scope.filterOptions.isPaided, $scope.filterOptions.userId);
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };

            $scope.ChangePaidedMethod = function () {
                if (event.target.value == "1") {
                    $scope.hideIsPaid = false;
                } else {
                    $scope.hideIsPaid = true;
                    $scope.AdvertisementAddObj.advertismentPriceId = null;
                    $scope.AdvertisementEditObj.advertismentPriceId = null;
                }
            };

            SignalrDataFactory.GetAll("category/GetCategories/").then(function (result) {
                $scope.Categories = result.data;
            });

            SignalrDataFactory.GetAll("users/GetUsers/").then(function (result) {

                result.data.unshift({ id: "", name: " " });
                $scope.Users = result.data;
            });

            SignalrDataFactory.GetAll("advertisementPrice/GetAll").then(function (result) {
                $scope.AdvertismentPrice = result.data;
            });

            // validate start date and end date in angular js 
            // validate for add new ads
            $scope.$watch('AdvertisementAddObj.startDate', validateAddDates);
            $scope.$watch('AdvertisementAddObj.endDate', validateAddDates);

            function validateAddDates() {
                if (Object.keys($scope.AdvertisementAddObj).length === 0 && $scope.AdvertisementAddObj.constructor === Object) return;
                if ($scope.form.AdvertisementAddObj.startDate.$error.invalidDate || $scope.form.AdvertisementAddObj.endDate.$error.invalidDate) {
                    $scope.form.AdvertisementAddObj.startDate.$setValidity("endBeforeStart", true);  //already invalid (per validDate directive)
                } else {
                    //depending on whether the user used the date picker or typed it, this will be different (text or date type).  
                    //creating a new date object takes care of that.  

                    if ($scope.AdvertisementAddObj.endDate) {
                        var endDate = new Date($scope.AdvertisementAddObj.endDate);
                        var startDate = new Date($scope.AdvertisementAddObj.startDate);
                        $scope.form.AdvertisementAddObj.endDate.$setValidity("endBeforeStart", endDate >= startDate);
                    }
                }
            }

            // validate for add edit ads
            $scope.$watch('AdvertisementEditObj.startDate', validateEditDates);
            $scope.$watch('AdvertisementEditObj.endDate', validateEditDates);



            function validateEditDates() {

                if (Object.keys($scope.AdvertisementEditObj).length === 0 && $scope.AdvertisementEditObj.constructor === Object) return;
                if ($scope.form.AdvertisementEditObj.startDate.$error.invalidDate || $scope.form.AdvertisementEditObj.endDate.$error.invalidDate) {
                    $scope.form.AdvertisementEditObj.startDate.$setValidity("endBeforeStart", true);  //already invalid (per validDate directive)
                } else {
                    //depending on whether the user used the date picker or typed it, this will be different (text or date type).  
                    //creating a new date object takes care of that.  

                    if ($scope.AdvertisementEditObj.endDate) {
                        var endDate = new Date($scope.AdvertisementEditObj.endDate);
                        var startDate = new Date($scope.AdvertisementEditObj.startDate);
                        $scope.form.AdvertisementEditObj.endDate.$setValidity("endBeforeStart", endDate >= startDate);
                    }
                }
            }


            //2- Load List Of Categories [ng-Grid]

            $scope.MainData = [];
            $scope.filterOptions = {
                filterText: '',
                useExternalFilter: true,
                categoryId: '',
                isPaided: '',
                userId: ''
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

            $scope.getPagedDataAsync = function (pageSize, page, searchText, categoryIdFilter, isPaidedFilter, userIdFilter, isExpired) {
                debugger;
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText,
                    CategoryIdFilter: categoryIdFilter,
                    IsPaidedFilter: isPaidedFilter,
                    UserIdFilter: userIdFilter.id,
                    IsExpired: isExpired
                };
                var data;
                SignalrDataFactory.PostPaging(controller + '/GetAllFilterdAdmin', pagingViewModel).then(function (result) {

                    unBlockUI();
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                });
            };
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.categoryId, $scope.filterOptions.isPaided, $scope.filterOptions.userId, $scope.filterOptions.isExpired);

            $scope.$watch('pagingOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                   
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.categoryId, $scope.filterOptions.isPaided, $scope.filterOptions.userId,$scope.filterOptions.isExpired);
                }
            }, true);

            $scope.$watch('filterOptions', function (newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.categoryId, $scope.filterOptions.isPaided, $scope.filterOptions.userId,$scope.filterOptions.isExpired);
                }
            }, true);

            $scope.gridOptions = {
                data: 'myData',
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                rowHeight: 50,
                columnDefs:
                [
                    { field: 'isActive', cellTemplate: '<div>{{row.getProperty(\'isActive\') ? "نعم" : "لا" }}</div>', displayName: 'فعال' },
                  { field: 'isPaided', cellTemplate: '<div>{{row.getProperty(\'isPaided\') ? "مدفوع" : "مجانى" }}</div>', displayName: 'حالة الأعلان' },
                    { field: 'isExpired', cellTemplate: '<div>{{row.getProperty(\'isExpired\') ? "لا" : "نعم" }}</div>', displayName: 'تم الدفع' },
                    { field: 'numberOfLikes', displayName: 'المعجبين', width: 80 },
                    { field: 'numberOfViews', displayName: 'المشاهدات ', width: 80 },
                    { field: 'paidEdPrice', displayName: 'السعر',width:80 },
                    { field: 'fullName', displayName: 'اسم المستخدم' },
                    { field: 'categoryName', displayName: 'الصنف' },
                    { field: 'cost', displayName: 'سعر المنتج' },
                    { field: 'name', displayName: 'العنوان', cellTemplate: '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)}}</div>' },
                    { field: 'imageUrl', cellTemplate: '<img ng-src="http://api.arousqatar.com/uploads/{{row.getProperty(\'imageUrl\')}}" class="img-circle img-responsive" style="height:50px;width:50px;margin-left: 25%;" />', displayName: 'الصورة', width: 70 },
                    { displayName: 'م', cellTemplate: '<div>{{$parent.$index + 1}}</div>' ,width:20 }
                ],
                afterSelectionChange: function (data) {
                    if (Object.prototype.toString.call(data) !== '[object Array]') {
                        $scope.eventCloseForm();
                    }
                }
            };
        }
    ])
    .controller('ComplaintsController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "complaint";
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'الشكاوى',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة شكوى',
                EditDivTitle: 'تعديل شكوى',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Message: 'سبب الشكوى',
                IconName: 'الايقونة',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'قائمة الشكاوى'
            };
            //
            $scope.ComplaintsAddObj = {};
            $scope.ComplaintsEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;

                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };
            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;


            };
            // Save/Add Complaints (Call Api Method)
            $scope.eventAddComplaints = function () {
                //Insert Complaints
                var Complaints = {
                    Id: 0,
                    Message: $scope.ComplaintsAddObj.message,
                    AdvertismentId: $scope.ComplaintsAddObj.advertismentId,
                };
                SignalrDataFactory.Post(controller + "/save", Complaints).then(function (result) {
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();

                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

                    }

                });

            };
            // Save/Edit Complaints (Call Api Method)
            $scope.eventEditComplaints = function () {
                //Insert Complaints


                var Complaints = {
                    Id: $scope.ComplaintsEditObj.id,
                    Message: $scope.ComplaintsEditObj.message,
                    AdvertismentId: $scope.ComplaintsEditObj.advertismentId

                };
                SignalrDataFactory.Post(controller + "/save", Complaints).then(function (result) {
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });

            };
            // View Complaints Record To Edit
            $scope.eventViewComplaints = function () {

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedComplaints = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/', editedComplaints.id).then(function (result) {

                    $scope.ComplaintsEditObj = result.data;

                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.ComplaintsAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.ComplaintsEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.ComplaintsAddObj.name = null;
                $scope.form.ComplaintsAddObj.$setPristine(true);
                $scope.form.ComplaintsAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.ComplaintsEditObj.Name = null;
                $scope.form.ComplaintsEditObj.$setPristine(true);
                $scope.form.ComplaintsEditObj.$setUntouched();
            };
            //
            $scope.deleteComplaints = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };

            SignalrDataFactory.GetAll("contactType/GetAll/").then(function (result) {
                $scope.ContactType = result.data;
            });
            //2- Load List Of Categories [ng-Grid]
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
                //$scope.totalServerItems = pageSize;
                console.log(pageSize);

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
            };
            $scope.getPagedDataAsync = function (pageSize, page, searchText) {
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;

                SignalrDataFactory.PostPaging(controller + '/all/advertisement/admin', pagingViewModel).then(function (result) {
                    unBlockUI();
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, $scope.pagingOptions.pageSize);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [

                    { field: 'message', displayName: 'الشكوى', cellTemplate: '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)|limitTo:15 }}...</div>' },
                    { field: 'camplaintUser', displayName: 'اسم مقدم الشكوى' },
                    { field: 'email', displayName: ' بريد مقدم الشكوى ', cellTemplate: '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)|limitTo:15}}...</div>' },
                    { field: 'phoneNumber', displayName: 'هاتف مقدم الشكوى' },
                    { field: 'complainedEmail', displayName: 'بريد المشكو منه', cellTemplate: '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)|limitTo:15}}...</div>' },
                    { field: 'complainedPhoneNumber', displayName: 'هاتف المشكو منه' },
                    { field: 'complainedUser', displayName: ' اسم المشكو منه' },
                    { field: 'advertisementName', displayName: 'الاعلان', cellTemplate: '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)|limitTo:20}}...</div>' },
                    { displayName: 'م', cellTemplate: '<div>{{$parent.$index + 1}}</div>',width:'20px' }
                ]
            };


        }
    ])
    .controller('ContactUsMessagesController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "contactUsMessage";
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'الرسائل',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة شكوى',
                EditDivTitle: 'تعديل شكوى',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Message: 'سبب الشكوى',
                IconName: 'الايقونة',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'قائمةالرسائل'
            };
           
         
        
          
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
        
        
         
          
            //
            $scope.deleteContactUsMessages = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };

          
            //2- Load List Of Categories [ng-Grid]
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
                //$scope.totalServerItems = pageSize;
                console.log(pageSize);

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
            };
            $scope.getPagedDataAsync = function (pageSize, page, searchText) {
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;

                SignalrDataFactory.PostPaging(controller + '/GetAll', pagingViewModel).then(function (result) {
                    unBlockUI();
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, $scope.pagingOptions.pageSize);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    { field: 'message', displayName: 'محتوى الرسالة', cellTemplate: '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)|limitTo :80}}</div>' },
                    { field: 'email', displayName: 'البريد الالكترونى',width:'15%' },
                    { field: 'phone', displayName: 'رقم الهاتف',width:'15%' },
                    { field: 'name', displayName: 'اسم المرسل ', width: '15%' },
                    { displayName: 'م', cellTemplate: '<div>{{$parent.$index + 1}}</div>', width: '30px' }
                ]
            };


        }
    ])
    .controller('allUserController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'المستخدمين',
                Search: 'بحث',
                List: 'قائمة المستخدمين ',
                ViewButtom: 'التفاصيل',
                ViewDivTitle: 'أستعراض التفاصيل',
                Name: 'الاسم بالكامل',
                City: " مدينة",
                Email: "البريد الإلكتروني",
                PhoneNumber: "الجوال",
                UserName: 'اسم المستخدم',
                IsApprove: 'الحالة',
                LoginProvider: 'تم التسجيل عن طريق',
                AnimalCount: 'عدد الحيوانات',
                ReportFromCount: 'عدد البلاغات المقدمة',
                ReportTo: 'الشكاوي',
                ChatSenderCount: 'عدد الرسائل المرسلة',
                ChatRecieverCount: 'عدد الرسائل المستقبلة',
                CommentCount: 'عدد التعليقات',
                GiveUpRequestsCount: 'عدد طلبات التخلي',
                MessagesCount: 'عدد المرسلات',
                SucessStoryCount: 'عدد قصص النجاح',
                VolunteerCount: 'عدد طلبات التبني',
                Close: 'غلق',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                ChooseCity: " مدينة",
                Password: 'كلمة المرور',
                ConfirmPassword: 'تأكيد كلمة المرور',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من  ',
                ConfirmPasswordMath: 'كلمة المرور وتأكيد كلمة المرور لا ينطبقان',
                role: 'نوع الحساب',
                chooseRole: 'اختار نوع الحساب',
                roleRequired: 'نوع الحساب مطلوب',
                enterValidUserName: 'ادخل اسم مستخدم صالح'
            };
            var controller = "Users";

            $scope.eventShowEditDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;
            };

            // View Advertisement Record To Edit
            $scope.eventViewUser = function () {

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedUser = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/GetUserById/', editedUser.id).then(function (result) {

                    $scope.UserEditObj = result.data;
                });
            }

            $scope.deleteUsers = function () {
                blockUI();
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    unBlockUI();
                    if (result.status === 200) {
                        if (result.data.error) {
                            alert(result.data.error);
                        } else {
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                        }
                    }
                });
                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
            };


            $scope.form = {};
            $scope.roles = [];

            //SignalrDataFactory.GetAll("city/GetAll/").then(function (result) {
            //    $scope.Cities = result.data;
            //});
            //activate();
            //function activate() {
            //    //TODO : Get all roles from DB 
            //    $scope.roles = [{ id: 'dcb33a89-7c67-48b6-93f4-123c9ed2e93f', name: 'Administrator' },
            //                    { id: 'b3ae5992-a427-462c-bbf3-d3d35b7dd841', name: 'User' }];
            //}

            $scope.UserAddObj = {};
            $scope.UserEditObj = {};

            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.UserAddObj.$valid;
            };

            $scope.eventAddUser = function () {


                var user = {
                    Name: $scope.UserAddObj.name,
                    Email: $scope.UserAddObj.email,
                    PhoneNumber: $scope.UserAddObj.PhoneNumber,
                    UserName: $scope.UserAddObj.userName,
                    Password: $scope.UserAddObj.password,
                    ConfirmPassword: $scope.UserAddObj.confirmPassword,
                    Role: 'User'
                };

                SignalrDataFactory.Post("Account/Register", user).then(function (result) {
                    if (result.status === 200) {
                        //Hide Div After Save
                        $scope.eventCloseForm();
                        $scope.getPagedDataAsync(
                            $scope.pagingOptions.pageSize,
                            $scope.pagingOptions.currentPage,
                            $scope.filterOptions.filterText);
                    }

                });
            }

            $scope.hideFromsInDiv = true;
            $scope.hideView = true;
            $scope.UserInfo = {};
            $scope.eventShowViewDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideView = false;
            };


            $scope.eventVieUserInfo = function () {
                $scope.eventShowViewDiv();
                //Read Select Item From ng-grad
                $scope.UserInfo = $scope.gridOptions.selectedItems[0];

            }
            $scope.eventCloseForm = function () {
                $scope.hideView = false;
                $scope.hideFromsInDiv = true;
            };

            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {

                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
            };

            //2- Load List Of [ng-Grid]
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
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };

                SignalrDataFactory.PostPaging(controller + '/GetAllUserAdmin/', pagingViewModel).then(function (result) {
                    unBlockUI();
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                });

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
            var statusTemplate = '<div>{{COL_FIELD== true?"أنثي":(COL_FIELD== null? "غير محدد":"ذكر")}}</div>';
            $scope.gridOptions = {
                data: 'myData',
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    //{ cellTemplate: "<div>{{row.entity.IsApprove==true?'تمت الموافقة':'لم تتم الموافقة'}}</div>", displayName: 'تأكيد البريد' },
                    //{ field: 'City', displayName: 'المدينة' },
                    { field: 'socialText', displayName: 'بواسطة التواصل الاجتماعي' },
                    { field: 'phoneNumber', displayName: 'الهاتف' },
                    { field: 'email', displayName: 'البريد الإلكتروني' },
                    { field: 'userName', displayName: 'اسم المستخدم' },
                    { field: 'name', displayName: 'الاسم' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('badRequestsController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'بلاغات الإساءة',
                Search: 'بحث',
                List: 'قائمةبلاغات الإساءة ',

            };
            var controller = "BadUseRequest";

            //2- Load List Of [ng-Grid]
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

                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText

                };
                var data;
                SignalrDataFactory.PostPaging(controller + '/GetAll', pagingViewModel).then(function (result) {
                    $scope.MainData = result.data.Items;
                    $scope.totalServerItems = result.data.TotalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.TotalCount);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    { field: 'CreateDate', displayName: 'تاريخ الطلب', cellFilter: 'date:\'yyyy-MM-dd\'' },
                    { field: 'Description', displayName: 'وصف' },
                    { field: 'ReportedUser', displayName: 'ضد' },
                    { field: 'CurrentUser', displayName: 'مقدم الطلب' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>', width: "7%" }
                ]
            };
        }
    ])
    .controller('shelterController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "AnimalShelter";


            $scope.Cities = {};
            $scope.Users = {};
            // 
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'إدارة الملاجىء',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة ملجأ',
                EditDivTitle: 'تعديل ملجأ',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Setting: 'الإعدادات',
                Name: 'اسم الملجأ',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من  ',
                ChooseCity: " مدينة",
                ChooseAdmin: "مدير الملجأ",
                Email: "البريد الإلكتروني",
                Mobile: "الجوال",
                FbLink: "رابط الفيس بوك",
                TwitterLink: "رابط تويتر",
                GooglePlusLink: "رابط جوجل بلس",
                LinkedInLink: "رابط لنكد ان",
                Description: "نبذة",
                Search: 'بحث',
                List: 'قائمة الملاجىء '


            };
            //
            $scope.ShelterAddObj = {};
            $scope.ShelterEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;
            //Cancel Add/Edit Form(Close Form Without Save)
            $scope.eventCloseForm = function () {

                //Hide Devs
                $scope.hideFromsInDiv = true;
                $scope.hideAdd = true;
                $scope.hideEdit = true;

                //emtpy input from date
                $scope.resetValidationAddForm();
                $scope.resetValidationEditForm();
            };
            //Clicked by the Add button (Show Dev)
            $scope.eventShowAddDiv = function () {
                $scope.hideFromsInDiv = false;
                $scope.hideAdd = false;
                $scope.hideEdit = true;
                SignalrDataFactory.GetAll("Users/GetShelterAdmin/0").then(function (result) {
                    $scope.Users = result.data;
                });
            };
            //Clicked by the Edit button (Show Dev)
            $scope.eventShowEditDiv = function () {

                $scope.hideFromsInDiv = false;
                $scope.hideEdit = false;
                $scope.hideAdd = true;


            };
            // Save/Add Shelter (Call Api Method)
            $scope.eventAddShelter = function (e) {
                console.log(e);
                //Insert Shelter
                var shelter = {
                    id: 0,
                    name: $scope.ShelterAddObj.name,
                    description: $scope.ShelterAddObj.description,
                    email: $scope.ShelterAddObj.email,
                    mobile: $scope.ShelterAddObj.mobile,
                    cityId: $scope.ShelterAddObj.cityId,
                    adminId: $scope.ShelterAddObj.adminId,
                    fbLink: $scope.ShelterAddObj.fbLink,
                    googlePlusLink: $scope.ShelterAddObj.googlePlusLink,
                    twitterLink: $scope.ShelterAddObj.twitterLink,
                    linkedInLink: $scope.ShelterAddObj.linkedInLink,
                };
                SignalrDataFactory.Post(controller + "/save", shelter).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };
            // Save/Edit Shelter (Call Api Method)
            $scope.eventEditShelter = function (flow) {

                //var fd = new FormData();
                //console.log("Send TO UPLOAD " + fd);
                //fd.append("file", flow);
                //$http.post("http://localhost:15165/api/Uploader/SaveFile", fd, {
                //    withCredentials: true,
                //    //headers: { 'Content-Type': undefined },
                //    transformRequest: angular.identity
                //});
                //flow.upload();
                ;

                //Update Shelter
                var shelter = {
                    id: $scope.ShelterEditObj.Id,
                    name: $scope.ShelterEditObj.Name,
                    description: $scope.ShelterEditObj.Description,
                    email: $scope.ShelterEditObj.Email,
                    mobile: $scope.ShelterEditObj.Mobile,
                    cityId: $scope.ShelterEditObj.CityId,
                    adminId: $scope.ShelterEditObj.AdminId,
                    fbLink: $scope.ShelterEditObj.FbLink,
                    googlePlusLink: $scope.ShelterEditObj.GooglePlusLink,
                    twitterLink: $scope.ShelterEditObj.TwitterLink,
                    linkedInLink: $scope.ShelterEditObj.LinkedInLink,
                };
                SignalrDataFactory.Post(controller + "/save/", shelter).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };
            // View Shelter Record To Edit
            $scope.eventViewShelter = function () {

                var editedShelter = $scope.gridOptions.selectedItems[0];
                SignalrDataFactory.GetAll("Users/GetShelterAdmin/" + editedShelter.Id).then(function (result) {
                    $scope.Users = result.data;
                });

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad

                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/View/', editedShelter.Id).then(function (result) {
                    $scope.ShelterEditObj = result.data;


                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.ShelterAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.ShelterEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.ShelterAddObj.name = null;
                $scope.ShelterAddObj.description = null;
                $scope.ShelterAddObj.cityId = null;
                $scope.ShelterAddObj.adminId = null;
                $scope.ShelterAddObj.email = null;
                $scope.ShelterAddObj.mobile = null;
                $scope.ShelterAddObj.fbLink = null;
                $scope.ShelterAddObj.twitterLink = null;
                $scope.ShelterAddObj.googlePlusLink = null;
                $scope.ShelterAddObj.linkedInLink = null;

                $scope.form.ShelterAddObj.$setPristine(true);
                $scope.form.ShelterAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.ShelterEditObj.Name = null;
                $scope.Users = {};
                $scope.form.ShelterEditObj.$setPristine(true);
                $scope.form.ShelterEditObj.$setUntouched();
            };
            //
            $scope.deleteShelter = function () {

                var id = $scope.gridOptions.selectedItems[0].Id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }
                });


                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


            };


            //SignalrDataFactory.GetAll("city/GetAll/").then(function(result) {
            //    $scope.Cities = result.data;
            //});


            //2- Load List Of Categories [ng-Grid]
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

                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;
                SignalrDataFactory.PostPaging(controller + '/Get', pagingViewModel).then(function (result) {
                    $scope.MainData = result.data.Items;
                    $scope.totalServerItems = result.data.TotalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.TotalCount);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [
                    { field: 'VolunteersCount', displayName: 'عدد طلبات التطوع فى الملجأ' },
                    { field: 'AnimalsCount', displayName: 'عدد الحيوانات فى الملجأ' },
                    { field: 'Name', displayName: 'الاسم' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('CommentsController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "comment";
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'التعليقات',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة تعليق',
                EditDivTitle: 'تعديل تعليق',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Message: 'سبب التعليق',
                IconName: 'الايقونة',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من ',
                Setting: 'الإعدادات',
                Search: 'بحث',
                List: 'قائمة التعليقات'
            };
            //
            $scope.ComplaintsAddObj = {};
            $scope.ComplaintsEditObj = {};
            $scope.form = {};
            // Variables to control view and hide form to add or edit
            $scope.hideFromsInDiv = true;
            $scope.hideAdd = true;
            $scope.hideEdit = true;

            $scope.deleteComments = function () {
                var id = $scope.gridOptions.selectedItems[0].id;
                SignalrDataFactory.Delete(controller + '/delete/', id).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }
                });
                $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
            };

            $scope.MainData = [];
            $scope.filterOptions = {
                filterText: '',
                useExternalFilter: true
            };
            $scope.totalServerItems = 1000;

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
                blockUI();
                var pagingViewModel = {
                    PageNumber: $scope.pagingOptions.currentPage,
                    PageSize: $scope.pagingOptions.pageSize,
                    Filter: searchText
                };
                var data;
                SignalrDataFactory.PostPaging(controller + '/GetPagedCommentsAdmin', pagingViewModel).then(function (result) {
                    unBlockUI();
                    $scope.MainData = result.data.items;
                    $scope.totalServerItems = result.data.totalCount;
                    $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                });

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
                multiSelect: false,
                enablePaging: true,
                showFooter: true,
                totalServerItems: 'totalServerItems',
                pagingOptions: $scope.pagingOptions,
                filterOptions: $scope.filterOptions,
                selectedItems: [],
                columnDefs:
                [

                    { field: 'message', displayName: 'التعليق' },
                    { field: 'userFirstName', displayName: 'اسم المستخدم' },
                    { field: 'advertismentName', displayName: 'الأعلان' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])



