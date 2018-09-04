angular.module('theme.core.municipality_Controller', ['theme.core.services'])
  .controller('MunicipalityController', ['$scope', '$http',  function ($scope, $http) {
      //'use strict';
      $scope.$on('onResponseError', function (event, args) {
          alert(args);
          debugger;
          //console.log("mmmmmmmmmmmmmmmmmmmmmmmmmmmm");
      $scope.setPagingData(args, $scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize);
          // $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
         // $scope.setPagingData
      });
      // Variable To Set Data Return By calling WebAPI
      $scope.Municipalities = [];

      // Variable To Set Total Rows Numbers
      $scope.totalServerItems = 0;
      $scope.maxSize = "5";
      $scope.pagingOptions = {
          pageSize: 10,
          currentPage: 1
      };
      $scope.numberOfPages = 1;

      $scope.id = "";
      $scope.name = "";
      $scope.isDeleted = "";

      $scope.filterOptions = {
          filterText: '',

      };
      $scope.$watch('filterOptions', function (newVal, oldVal) {
          if (newVal !== oldVal) {
              $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
          }
      }, true);

      $scope.$watch('pagingOptions', function (newVal, oldVal) {
          if (newVal !== oldVal) {
              $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
          }
      }, true);
      // Update It IF you want event in each row
      $scope.uaHandle = function ($index) {
          // console.log(ua);
          // this.Municipalities.splice($index, 1);
      };

      $scope.idSelectedVote = null;
      $scope.setSelected = function (idSelectedVote) {
          console.log("---Trace MunicipalityController-- In setSelected idSelectedVote'" + idSelectedVote + "' ");
          $scope.idSelectedVote = idSelectedVote;
      };


      // UPDAET
      $scope.getPagedDataAsync = function (pageSize, page, searchText) {
          //setTimeout(function () {


          var data;
          if (searchText) {
              console.log("---Trace MunicipalityController-- In getPagedDataAsync searchText'" + searchText + "' ");
              var ft = searchText.toLowerCase();
              $http.get(ApiURL.GetCities).then(function (largeLoad) {
                  data = largeLoad.data.filter(function (item) {
                      return JSON.stringify(item).toLowerCase().indexOf(ft) !== -1;
                  });
                  $scope.setPagingData(data, page, pageSize);
              }, function (error) {
                  // $scope.data.error = { message: error, status: status };
                  console.log("AAAAAAAAAAAAAAAAAA" + error.status);
                  //if (error.status === 401)
                  //    authenticationService.Relogin();
              });
          } else {
              console.log("---Trace MunicipalityController-- In getPagedDataAsync searchText'" + searchText + "' ");
              $http.get(ApiURL.GetCities).success(function (largeLoad) {
                  console.log("BBBBBBBBBBBBBBBB" + largeLoad);
                  $scope.setPagingData(largeLoad, page, pageSize);
              }
              //, function (error) {
              //    // $scope.data.error = { message: error, status: status };
              //    console.log("AAAAAAAAAAAAAAAAAA" + error.status);

              //    //if (error.status === 401) {
              //    //    authenticationService.Relogin();
              //    //    $scope.getPagedDataAsync(pageSize, page, searchText);

              //    //}
              //    // console.log("---Trace MunicipalityController-- In Run Get Is '" + $cookieStore.get('globals') + "' ");


              //}

              );
          }
          //}, 100);
      };

      $scope.setPagingData = function (data, page, pageSize) {
          var pagedData = data.slice((page - 1) * pageSize, page * pageSize);

          $scope.Municipalities = pagedData;
          $scope.totalServerItems = data.length;
          $scope.numberOfPages = Math.ceil($scope.totalServerItems / $scope.pagingOptions.pageSize);

          if (!$scope.$$phase) {
              $scope.$apply();
          }
      };

      $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);




  }]);