angular.module('theme.core.Municipality', [])
  .controller('MunicipalityController', ['$scope', '$http', function ($scope, $http) {
      //'use strict';

      // Variable To Set Data Return By calling WebAPI
      $scope.Municipalities = [];

      // Variable To Set Total Rows Numbers
      $scope.totalServerItems = 0;

      $scope.currentPage =1;

      $scope.pageSize = 10;

      $scope.numberOfPages = function () {
          $scope.totalServerItems = $scope.data.length;
          if (!$scope.$$phase) {
              $scope.$apply();
          }
          return Math.ceil($scope.Municipalities.length / $scope.pageSize);
      }

      $scope.municipalityInRange = function () {
          $scope.getPagedData();
          return this.Municipalities.slice(this.currentPage * this.pageSize, this.currentPage * pageSize);
      };

      $scope.getPagedData = function() {
            $http.get(ApiURL.StaticCity).success(function(largeLoad) {
                this.Municipalities = largeLoad.filter(function (item) {
                    return JSON.stringify(item) !== -1;
                });
                
            });
        }

      /*
        $scope.setPagingData = function (data, page, pageSize) {
            var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
            $scope.Municipalitiesd = pagedData;
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
                            return JSON.stringify(item.Name).toLowerCase().indexOf(ft) !== -1;
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
  
        */
    }]);