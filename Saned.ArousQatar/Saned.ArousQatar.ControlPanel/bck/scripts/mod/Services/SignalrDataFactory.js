angular
    .module('SignalrDataModule', [])
    .factory('SignalrDataFactory', ['$rootScope', '$http', 'appSettings', '$location', function ($rootScope, $http, appSettings, $location) {
        var apiUrl = appSettings.cfrtsApiUrl;
        // $.connection.hub.url = appSettings.signalrHubConnection;

        var Groups = [
          "CityAllModelsHubPublicGroupName",
          "QuotationAllModelsHubPublicGroupName",
          "ProductsAllModelsHubPublicGroupName",
          'ProductCategoryAllModelsHubPublicGroupName',
          "ProductSubCategoryAllModelsHubPublicGroupName",
          "UnitsAllModelsHubPublicGroupName"

        ];
        var service = {
            GetAll: GetAll,
            GetSingle: GetSingle,
            Post: Post,
            Put: Put,
            Delete: Delete,
            PostPaging: PostPaging,
            FetchObject: FetchObject,
            InitateConnection: InitateConnection,
            InvokeClientMethod: InvokeClientMethod,
            InvokeServerMethod: InvokeServerMethod

        };

        return service;
        //SingalR
        var proxy;
        function InitateConnection(group) {
            //proxy = $.connection.allmodelsHub;
            //$.connection.hub.start().done(function () {
            //    console.log('Hub Connected');
            //    //proxy.server.jointogroup(group);

            //});

        }

        function InvokeClientMethod(MethodName, CallBack) {
            console.log(MethodName + " Client method is invoked");

            $rootScope.proxy.on(MethodName, CallBack);

        }


        function InvokeServerMethod(MethodName, Groupname, msg) {
            $rootScope.proxy.server[MethodName](Groupname, msg);

        }




        // CRUD Functions
        function GetAll(Url) {
            return $http.get(apiUrl + Url).then(function (data) {
                return data;
            });
        }
        function GetSingle(url, id) {
            if (typeof id === 'undefined') {
                return $http.get(apiUrl + url).success(function (data) {
                    return data;
                });
            }
                else
            {
                return $http.get(apiUrl + url + id).success(function (data) {
                    return data;});
            }
          
        }
        function Post(Url, Data) {
            return $http({
                method: 'POST',
                data: JSON.stringify(Data),
                url: apiUrl + Url,
                contentType: "application/json"
            }).success(function (response) {
                console.log("Data was Edited");
            }).error(function (error) {
                console.log(error);
            });
        }
        function PostPaging(Url, Data) {
            return $http({
                method: 'POST',
                data: JSON.stringify(Data),
                url: apiUrl + Url,
                contentType: "application/json"
            }).success(function (response) {
                return response;
            }).error(function (error) {
                console.log(error);
            });
        }
        function Put(Url, Data) {
            return $http({
                method: 'POST',
                data: JSON.stringify(Data),
                url: apiUrl + Url,
                contentType: "application/json"
            }).success(function (response) {
                console.log("Data was edited");
            }).error(function (error) {
                console.log(error);

            });
        }



        function Delete(Url, ID) {
            return $http({
                method: "DELETE",
                url: apiUrl + Url + ID

            }).success(function (response) {
                //return true; // Return true or false to Indicate the success of the post
                console.log("Data was Edited");
            }).error(function (error, status) {
                console.log("error.error.message" + error.Message);
                if (status === 400) {
                    console.log("error.error.message400" + error.Message);
                    if (error.Message === "100") {
                        document.getElementById("errors").innerHTML = "لا يمكن حذف العنصر لانه مرتبط بعناصر اخري داخل النظام";
                        $('#messageModal').modal('show');
                    }
                     
                }
                console.log(error);
                //return false;
            });

        }
        //Paramters : Key , ApiLink
        //This method get the querystring key and return the value then
        //Get the object from the database
        function FetchObject(key, apiLink) {
            var ID = $location.search()[key];
            return GetSingle(apiLink, ID);
        }
    }]);

