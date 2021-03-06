
angular
  .module('theme.demos.ng_grid', [
    'ngGrid'
  ])
  .controller('MatrixController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {
      'use strict';
      $scope.filterOptions = {
          filterText: '',
       
      };
      $scope.totalServerItems = 0;
      $scope.pagingOptions = {
          pageSizes: [25, 50, 100],
          pageSize: 25,
          currentPage: 1
      };
      $scope.columnDefs = [
          {
              field: 'Id',
              displayName: 'الرقم',
              enableFiltering: false,
              //CellTemplate: "<div>{{$index + 1}}</div>"
          },
          {
              field: 'Name',
              displayName: 'المدينة',
              
          }
      ];

      $scope.setPagingData = function (data, page, pageSize) {
          var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
          $scope.myData = pagedData;
          $scope.totalServerItems = data.length;
          if (!$scope.$$phase) {
              $scope.$apply();
          }
      };
      $scope.getPagedDataAsync = function (pageSize, page, searchText) {
          setTimeout(function () {
              var data;
              if (searchText) {
                  var ft = searchText.toLowerCase();
                  $http.get(ApiURL.StaticCity).success(function (largeLoad) {
                      data = largeLoad.filter(function (item) {
                          return JSON.stringify(item).toLowerCase().indexOf(ft) !== -1;
                      });
                      $scope.setPagingData(data, page, pageSize);
                  });
              } else {
                  $http.get(ApiURL.StaticCity).success(function (largeLoad) {
                      $scope.setPagingData(largeLoad, page, pageSize);
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
          columnDefs: $scope.columnDefs,
          totalServerItems: 'totalServerItems',
          useExternalFilter: false,
          pagingOptions: $scope.pagingOptions,
          filterOptions: $scope.filterOptions,
          enableFiltering: true,
          multiSelect: false,


      };

  }]);