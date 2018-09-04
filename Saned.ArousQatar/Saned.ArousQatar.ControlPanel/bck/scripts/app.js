"use strict";
angular
    .module('themesApp', [
        'theme',
        'theme.demos',
        'cityModule',
        'settingModule',
        'appsettingModule',
        'userModule',
        'ConfirmationModule',
        'SharedDataModule',
        'SharedDataModule',
        'SharedDataModule',
        'LocalStorageModule',
        'authServiceModule',
        'tokenModule',
        'loginModule'

    ])
    .constant('appSettings', {
        //ResourceServerUrl: 'https://localhost:44363/',
        //hubConnectionUrl: 'https://localhost:44363/',              //used in run() function  app.js file
        cfrtsApiUrl: 'http://localhost:15165/api/',               // used in SignalrDataFactory.js file 
        //signalrHubConnection: 'https://localhost:44363/signalr',   // used in SignalrDataFactory.js file
        //identityProvider: 'https://localhost:44317/identity',      // used in common.services.js file and need to modify in callback.html also
        //signalrHubs: 'https://localhost:44363/signalr/hubs',       // used in index.html page
        apiServiceBaseUri: 'http://localhost:15165/',
        clientId: 'ngAuthApp'
    })
    .config([
        '$provide', '$routeProvider', '$httpProvider', function ($provide, $routeProvider, $httpProvider) {
            'use strict';
            $routeProvider
                .when('/', {
                    templateUrl: 'views/index.html',
                    resolve: {
                        loadCalendar: [
                            '$ocLazyLoad', function ($ocLazyLoad) {
                                return $ocLazyLoad.load([
                                    'bower_components/fullcalendar/fullcalendar.js'
                                ]);
                            }
                        ]
                    }
                })
                .when('/login', {
                    templateUrl: 'views/extras-login.html',
                    hideMenus: true
                })
                .when('/Cities', {
                    templateUrl: 'views/Settings/Cities.html'
                })
                 .when('/Shelters', {
                     templateUrl: 'views/Settings/Shelters.html'
                 })
                 .when('/Categories', {
                     templateUrl: 'views/Settings/Categories.html'
                 })
                .when('/Settings', {
                    templateUrl: 'views/Settings/EditSetting.html'
                })
                 .when('/Users', {
                     templateUrl: 'views/users/Users.html'
                 })
                 .when('/Balance', {
                     templateUrl: 'views/users/ViewUserBalance.html'
                 })
                 .when('/Profile', {
                     templateUrl: 'views/users/ViewUserProfile.html'
                 })
                 .when('/Chat', {
                     templateUrl: 'views/users/ViewUserChat.html'
                 })
                .when('/:templateFile', {
                    templateUrl: function (param) {
                        return 'views/' + param.templateFile + '.html';
                    }
                })
                .when('#', {
                    templateUrl: 'views/index.html'
                })
                .otherwise({
                    redirectTo: 'views/extras-login'
                });
            $httpProvider.interceptors.push('authInterceptorService');
            //$httpProvider.interceptors.push('authInterceptor');
        }
    ])
    .run(['authService', function (authService) {
        authService.fillAuthData();
    }]);

/* .run(['$rootScope', '$location', '$cookieStore', '$http',
 function ($rootScope, $location, $cookieStore, $http) {

     $rootScope.globals2 = $cookieStore.get('token') || {};
     if ($rootScope.globals2.currentToken) {
         $http.defaults.headers.common['Authorization'] = 'bearer ' + $rootScope.globals2.currentToken.token;
     }

     $rootScope.$on('$locationChangeStart', function (event, next, current) {
         console.log("---Trace App-- In Run $locationChangeStart ");
         // redirect to login page if not logged in
         if ($location.path() !== '/login' && !$rootScope.globals2.currentToken) {
             $location.path('/login');
         }
         else
             if ($location.path() === '/login' && $rootScope.globals2.currentToken) {
                 $location.path('/');
             }
     });


 }]);*/
