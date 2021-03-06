angular
    .module('cityModule', [
    'ngAnimate',
    'ngGrid',
    'SignalrDataModule'
    ])
  .controller('cityController2', ['$scope', '$filter', '$http', '$q', 'SignalrDataFactory',
      function ($scope, $filter, $http, $q, SignalrDataFactory) {
          //'use strict';
          //-----------------------------
          //Constants (Ng-Hide)
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
                  SignalrDataFactory.Delete('Municipality/DeleteCity/', $scope.gridOptions.selectedItems[city].id).then(function (result) {
                      $scope.MainData = $scope.MainData.filter(function (obj) {
                          obj.id !== id;

                      });

                  });

              }
              $scope.setPagingData($scope.MainData, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
              $scope.gridOptions.$gridScope.toggleSelectAll(false, false);


          };
          $scope.cityEditObj = {};
          //Load Edit
          $scope.loadEditData = function () {
              $scope.isAdding = false;
              $scope.isEdtiing = true;
              var editedCity = $scope.gridOptions.selectedItems[0];

              $scope.gridOptions.$gridScope.toggleSelectAll(false, false);
              SignalrDataFactory.GetSingle('Municipality/ViewCity/', editedCity.id).then(function (result) {
                  $scope.cityEditObj = result.data;
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
              pageSizes: [1, 10, 25, 50, 100],
              pageSize: 1,
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
              setTimeout(function () {
                  var pagingViewModel = {
                      pageNumber: $scope.pagingOptions.currentPage,
                      pageSize: $scope.pagingOptions.pageSize,
                      filter: searchText,
                      isDelete: false

                  };
                  var data;
                  if (searchText) {
                      var ft = searchText.toLowerCase();

                      SignalrDataFactory.PostPaging('Municipality/GetAll', pagingViewModel).then(function (result) {
                          $scope.MainData = result.data.items.filter(function (item) {
                              //return JSON.stringify(item).toLowerCase().indexOf(ft) !== -1;
                              $scope.totalServerItems = result.data.TotalCount;
                              return JSON.stringify(item);
                          });
                          $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                      });
                  } else {

                      SignalrDataFactory.PostPaging('Municipality/GetAll', pagingViewModel).then(function (result) {
                          $scope.MainData = result.data.items;
                          $scope.totalServerItems = result.data.TotalCount;
                          $scope.setPagingData($scope.MainData, result.data.page, result.data.totalCount);

                      });


                  }
              }, 100);
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
                  { field: 'longitude', displayName: 'خط الطول' },
                  { field: 'latitude', displayName: 'خط العرض' },
                   { field: 'name', displayName: 'البلدة' },
                  { displayName: 'المسلسل', cellTemplate: '<div>{{$parent.$index + 1}}</div>' }


              ]
          };
          //-----------------------------------------------------------------
          //----------------------- NG-GRID CONFIGURATION -------------------
          //-----------------------------------------------------------------

      }]);