angular
    .module('SharedDataModule', [])
    .service('SharedDataService', ['$rootScope', '$http', function ($rootScope, $http) {
        var ShareDictionary = [];
        this.setData = function (key,value) {
            ShareDictionary[key] = value;
        }
        this.getData = function (key) {
           
            return ShareDictionary[key];
        }

    }]);

