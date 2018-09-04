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
    //.constant('appSettings', {
    //    cfrtsApiUrl: 'http://arousqatar.saned-projects.com/api/', //'http://localhost:8804/api/',// 
    //    apiServiceBaseUri: 'http://arousqatar.saned-projects.com/', // 'http://localhost:8804/',//
    //    clientId: 'ngAuthApp'
    //})
     .constant('appSettings', {
         // cfrtsApiUrl: 'http://localhost:8804/api/',// 
         // cfrtsApiUrl: 'http://ArousQatar.saned-projects.com/api/',// 
         cfrtsApiUrl: 'http://api.arousqatar.com/api/',// 


         //apiServiceBaseUri: 'http://localhost:8804/',//
         // apiServiceBaseUri: 'http://ArousQatar.saned-projects.com/',//
         apiServiceBaseUri: 'http://api.arousqatar.com/',//
         clientId: 'ngAuthApp'
     })
    .config([
        '$provide', '$routeProvider', '$httpProvider', function ($provide, $routeProvider, $httpProvider) {
            'use strict';
            $routeProvider
                .when('/', {
                    templateUrl: 'views/Settings/Advertisements.html',
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
                .when('/Categories', {
                    templateUrl: 'views/Settings/Categories.html'
                })
                .when('/Packages', {
                    templateUrl: 'views/Settings/Packages.html'
                })
                .when('/ContactUs', {
                    templateUrl: 'views/Settings/ContactUs.html'
                })
                .when('/Users', {
                    templateUrl: 'views/users/Users.html'
                })
                .when('/complaints', {
                    templateUrl: 'views/Settings/complaints.html'
                })
                 .when('/Messages', {
                     templateUrl: 'views/Settings/contactUsMessages.html'
                 })
                .when('/Advertisements', {
                    templateUrl: 'views/Settings/Advertisements.html'
                })
                .when('/Comments', {
                    templateUrl: 'views/Settings/Comments.html'
                })
                .when('/:templateFile', {
                    templateUrl: function (param) {
                        return 'views/' + param.templateFile + '.html';
                    }
                })
                .when('#', {
                    templateUrl: 'views/Settings/Advertisements.html'
                })
                .otherwise({
                    redirectTo: '/'
                });
            $httpProvider.interceptors.push('authInterceptorService');
            //$httpProvider.interceptors.push('authInterceptor');
        }
    ])
    .run(['authService', function (authService) {
        authService.fillAuthData();
    }]);

