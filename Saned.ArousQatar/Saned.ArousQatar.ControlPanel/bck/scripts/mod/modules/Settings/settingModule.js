angular
    .module('settingModule', [
        'ngAnimate',
        'ngGrid',
        'SignalrDataModule'
    ])
    .controller('categoryController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {

            // 
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'إدارة التصنيفات',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة تصنيف',
                EditDivTitle: 'تعديل تصنيف',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Name: 'اسم التصنيف',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من 150 حرف'
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
                //Insert Catgory
                var category = {
                    id: 0,
                    name: $scope.CategoryAddObj.name
                };
                SignalrDataFactory.Post("Category/save", category).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };
            // Save/Edit Category (Call Api Method)
            $scope.eventEditCategory = function () {
                //Insert Category
                var category = {
                    id: $scope.CategoryEditObj.Id,
                    name: $scope.CategoryEditObj.Name
                };
                SignalrDataFactory.Post("Category/save/", category).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };
            // View Category Record To Edit
            $scope.eventViewCategory = function () {

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedCategory = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle('Category/View/', editedCategory.Id).then(function (result) {
                    $scope.CategoryEditObj = result.data;

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
                $scope.CategoryAddObj.name = null;
                $scope.form.CategoryAddObj.$setPristine(true);
                $scope.form.CategoryAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.CategoryEditObj.Name = null;
                $scope.form.CategoryEditObj.$setPristine(true);
                $scope.form.CategoryEditObj.$setUntouched();
            };
            //
            $scope.deleteCategory = function () {

                var id = $scope.gridOptions.selectedItems[0].Id;
                SignalrDataFactory.Delete('Category/delete/', id).then(function (result) {
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
                SignalrDataFactory.PostPaging('Category/Get', pagingViewModel).then(function (result) {
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
                    { field: 'AnimalsCount', displayName: 'عدد الحيوانات فى التصنيف' },
                    { field: 'Name', displayName: 'الاسم' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
                ]
            };
        }
    ])
    .controller('cityController', [
        '$scope', '$location', '$filter', '$http', '$q', 'SignalrDataFactory',
        function ($scope, $location, $filter, $http, $q, SignalrDataFactory) {
            var controller = "City";
            // 
            $scope.pagelabalObj = {
                Dashboard: 'الصفحة الرئيسية',
                PageTitle: 'إدارة المدن',
                AddNewButtom: 'إضافة',
                EditButtom: 'تعديل',
                AddDivTitle: 'إضافة مدينة',
                EditDivTitle: 'تعديل مدينة',
                Cancel: 'إلغاء',
                Save: 'حفظ',
                Name: 'اسم المدينة',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من 150 حرف'
            };
            //
            $scope.CityAddObj = {};
            $scope.CityEditObj = {};
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
            // Save/Add City (Call Api Method)
            $scope.eventAddCity = function () {
                //Insert City
                var city = {
                    id: 0,
                    name: $scope.CityAddObj.name
                };
                SignalrDataFactory.Post(controller + "/save", city).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };
            // Save/Edit City (Call Api Method)
            $scope.eventEditCity = function () {
                //Insert City
                var city = {
                    id: $scope.CityEditObj.Id,
                    name: $scope.CityEditObj.Name
                };
                SignalrDataFactory.Post(controller + "/save/", city).then(function (result) {
                    if (result.status === 200) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    }

                });
                //Hide Div After Save
                $scope.eventCloseForm();
            };
            // View City Record To Edit
            $scope.eventViewCity = function () {

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedCity = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/View/', editedCity.Id).then(function (result) {
                    $scope.CityEditObj = result.data;

                });
            }
            //
            $scope.canSubmitValidationAddForm = function () {
                return $scope.form.CityAddObj.$valid;
            };
            //
            $scope.canSubmitValidationEditForm = function () {
                return $scope.form.CityEditObj.$valid;
            };
            //
            $scope.resetValidationAddForm = function () {
                $scope.CityAddObj.name = null;
                $scope.form.CityAddObj.$setPristine(true);
                $scope.form.CityAddObj.$setUntouched();
            };
            //
            $scope.resetValidationEditForm = function () {
                $scope.CityEditObj.Name = null;
                $scope.form.CityEditObj.$setPristine(true);
                $scope.form.CityEditObj.$setUntouched();
            };
            //
            $scope.deleteCity = function () {

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
                    { field: 'SheltersCount', displayName: 'عدد الملاجىء' },
                     { field: 'UsersCount', displayName: 'عدد المستخدمين' },
                    { field: 'AnimalsCount', displayName: 'عدد الحيوانات ' },
                    { field: 'Name', displayName: 'الاسم' },
                    { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }
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
                Name: 'اسم الملجأ',
                RequireNote: 'حميع الحقول التي تحتوي على علامة * مطلوبة',
                FieldRequired: 'هذا الحقل مطلوب',
                FielMaxlength: 'النص المكتوب اكبر من 150 حرف',
                ChooseCity: "أختر مدينة",
                Email: "البريد الإلكتروني",
                Mobile: "الجوال",
                FbLink: "رابط الفيس بوك",
                TwitterLink: "رابط تويتر",
                GooglePlusLink: "رابط جوجل بلس",
                LinkedInLink: "رابط لنكد ان",
                Description: "نبذة"




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
            $scope.eventAddShelter = function () {
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
            $scope.eventEditShelter = function () {
                //Insert Shelter
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

                $scope.eventShowEditDiv();
                //Read Select Item From ng-grad
                var editedShelter = $scope.gridOptions.selectedItems[0];
                $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
                SignalrDataFactory.GetSingle(controller + '/View/', editedShelter.Id).then(function (result) {
                    $scope.ShelterEditObj = result.data;
                    console.log($scope.ShelterEditObj.Id);
                    SignalrDataFactory.GetAll("Users/GetShelterAdmin/" + $scope.ShelterEditObj.Id).then(function (result) {
                        $scope.Users = result.data;
                    });
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



            SignalrDataFactory.GetAll("city/GetAll/").then(function (result) {
                $scope.Cities = result.data;
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
    ]);