angular
  .module('theme.core.services', [])
  .factory('progressLoader', function () {
      'use strict';
      return {
          start: function () {
              angular.element.skylo('start');
          },
          set: function (position) {
              angular.element.skylo('set', position);
          },
          end: function () {
              angular.element.skylo('end');
          },
          get: function () {
              return angular.element.skylo('get');
          },
          inch: function (amount) {
              angular.element.skylo('show', function () {
                  angular.element(document).skylo('inch', amount);
              });
          }
      };
  })
  .factory('EnquireService', ['$window', function ($window) {
      'use strict';
      return $window.enquire;
  }])
  .factory('pinesNotifications', ['$window', function ($window) {
      'use strict';
      return {
          notify: function (args) {
              args.mouse_reset = false;
              var notification = new $window.PNotify(args);
              notification.notify = notification.update;
              return notification;
          },
      };
  }])
  .factory('$bootbox', ['$modal', '$window', function ($modal, $window) {
      'use strict';
      // NOTE: this is a workaround to make BootboxJS somewhat compatible with
      // Angular UI Bootstrap in the absence of regular bootstrap.js
      if (angular.element.fn.modal === undefined) {
          angular.element.fn.modal = function (directive) {
              var that = this;
              if (directive === 'hide') {
                  if (this.data('bs.modal')) {
                      this.data('bs.modal').close();
                      angular.element(that).remove();
                  }
                  return;
              } else if (directive === 'show') {
                  return;
              }

              var modalInstance = $modal.open({
                  template: angular.element(this).find('.modal-content').html()
              });
              this.data('bs.modal', modalInstance);
              setTimeout(function () {
                  angular.element('.modal.ng-isolate-scope').remove();
                  angular.element(that).css({
                      opacity: 1,
                      display: 'block'
                  }).addClass('in');
              }, 100);
          };
      }

      return $window.bootbox;
  }])
  .service('lazyLoad', ['$q', '$timeout', function ($q, $t) {
      'use strict';
      var deferred = $q.defer();
      var promise = deferred.promise;
      this.load = function (files) {
          angular.forEach(files, function (file) {
              if (file.indexOf('.js') > -1) { // script
                  (function (d, script) {
                      var fDeferred = $q.defer();
                      script = d.createElement('script');
                      script.type = 'text/javascript';
                      script.async = true;
                      script.onload = function () {
                          $t(function () {
                              fDeferred.resolve();
                          });
                      };
                      script.onerror = function () {
                          $t(function () {
                              fDeferred.reject();
                          });
                      };

                      promise = promise.then(function () {
                          script.src = file;
                          d.getElementsByTagName('head')[0].appendChild(script);
                          return fDeferred.promise;
                      });
                  }(document));
              }
          });

          deferred.resolve();

          return promise;
      };
  }])
  .filter('safe_html', ['$sce', function ($sce) {
      'use strict';
      return function (val) {
          return $sce.trustAsHtml(val);
      };
  }])
  .factory('AuthenticationService',
    ['Base64', '$http', '$cookieStore', '$rootScope', '$timeout', '$location',
    function (base64, $http, $cookieStore, $rootScope, $timeout) {
        var service = {}

        service.Loginbck = function (username, password, callback) {

            var settings = {
                "async": false,
                "crossDomain": true,
                "url": "http://localhost:24439/oauth2/token",
                "method": "POST",
                "headers": {
                    "accept": "application/json",
                    "accept-language": "en-gb",
                    "audience": "Any",
                    "cache-control": "no-cache",
                    "content-type": "application/x-www-form-urlencoded"
                },
                "data": {
                    "username": username,
                    "password": password,
                    "grant_type": "password"
                }
            }

            //$http(settings)
            //    .then(function (response) {
            //        callback(response);
            //    }, function (error) {
            //        callback(error);
            //        console.log(error);
            //    });

            $.ajax(settings).done(function (response) {
                console.log("---Trace Service-- In submit response'" + response + "' ");
                callback(response);
            }).error(function (response) {
                callback(response);
            });

        };
        service.Login = function (username, password, callback) {

            var settings = {
                "async": false,
                "crossDomain": true,
                "url": "http://localhost:24439/oauth2/token",
                "method": "POST",
                "headers": {
                    "accept": "application/json",
                    "accept-language": "en-gb",
                    "audience": "Any",
                    "cache-control": "no-cache",
                    "content-type": "application/x-www-form-urlencoded"
                },
                "data": {
                    "username": username,
                    "password": password,
                    "grant_type": "password"
                }
            }
            $.ajax(settings).done(function (response) {
                console.log("---Trace Service-- In submit response'" + response + "' ");
                callback(response);
            }).error(function (response) {

                callback(response);
            });
        };
        service.SetRememberMe = function (username, password) {
            console.log("---Trace Service-- In SetRememberMe UserName Is '" + username + "' Password Is " + password);
            //  var authdata = base64.encode(username + ':' + password);
            var authdata = password;

            $rootScope.globals = {
                currentUser: {
                    username: username,
                    authdata: authdata
                }
            };
            console.log("---Trace Service-- In SetRememberMe $rootScope.globals Is '" + $rootScope.globals + "' ");

            // $http.defaults.headers.common['Authorization'] = 'Basic ' + authdata; // jshint ignore:line
            $cookieStore.put('globals', $rootScope.globals);
            console.log("---Trace App-- In SetRememberMe cookieStore Is '" + $cookieStore.get('globals') + "' ");
        };
        service.Relogin = function () {
            var isLogin = 0;
            console.log("---Trace Service-- In Relogin");
            var cookieStoreData = $cookieStore.get('globals') || {};

            if (cookieStoreData.currentUser) {
                service.Login(cookieStoreData.currentUser.username, cookieStoreData.currentUser.authdata, function (response) {

                    if (response.access_token) {
                        service.SetCredentials(response.access_token, response.token_type, response.expires_in);
                        isLogin = 1;
                    } else {
                        isLogin = 0;
                        console.log("---Trace Service-- In Relogin ERROR " + response.message);
                    }

                });

            } else {
                service.ClearCredentials();
                isLogin = 0;

            }
            return isLogin;
        };
        service.SetCredentials = function (token, tokentype, expirein) {
            // debugger;
            console.log("---Trace Service-- In SetCredentials Token Is '" + token + "' tokentype Is " + tokentype);
            var authdata = base64.encode(tokentype + ':' + expirein);

            $rootScope.globals2 = {
                currentToken: {
                    token: token,
                    tokentype: tokentype,
                    expirein: expirein
                }
            };
            console.log("---Trace Service-- In SetCredentials $rootScope.globals2 Is '" + $rootScope.globals2 + "' ");

            $http.defaults.headers.common['Authorization'] = 'bearer ' + $rootScope.globals2.currentToken.token;
            $cookieStore.put('token', $rootScope.globals2);
            console.log("---Trace App-- In SetCredentials 'bearer ' + $rootScope.globals2.currentToken.token; Is '" + 'bearer ' + $rootScope.globals2.currentToken.token + "' ");
            return $rootScope.globals2.currentToken.token;
        };
        service.ClearCredentials = function () {

            $rootScope.globals = {};
            $rootScope.globals2 = {};
            $cookieStore.remove('globals');
            $cookieStore.remove('token');
            $http.defaults.headers.common.Authorization = 'Basic ';
        };

        return service;
    }])

  .factory('Base64', function () {
      /* jshint ignore:start */

      var keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';

      return {
          encode: function (input) {
              var output = "";
              var chr1, chr2, chr3 = "";
              var enc1, enc2, enc3, enc4 = "";
              var i = 0;

              do {
                  chr1 = input.charCodeAt(i++);
                  chr2 = input.charCodeAt(i++);
                  chr3 = input.charCodeAt(i++);

                  enc1 = chr1 >> 2;
                  enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                  enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                  enc4 = chr3 & 63;

                  if (isNaN(chr2)) {
                      enc3 = enc4 = 64;
                  } else if (isNaN(chr3)) {
                      enc4 = 64;
                  }

                  output = output +
                      keyStr.charAt(enc1) +
                      keyStr.charAt(enc2) +
                      keyStr.charAt(enc3) +
                      keyStr.charAt(enc4);
                  chr1 = chr2 = chr3 = "";
                  enc1 = enc2 = enc3 = enc4 = "";
              } while (i < input.length);

              return output;
          },

          decode: function (input) {
              var output = "";
              var chr1, chr2, chr3 = "";
              var enc1, enc2, enc3, enc4 = "";
              var i = 0;

              // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
              var base64test = /[^A-Za-z0-9\+\/\=]/g;
              if (base64test.exec(input)) {
                  window.alert("There were invalid base64 characters in the input text.\n" +
                      "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                      "Expect errors in decoding.");
              }
              input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

              do {
                  enc1 = keyStr.indexOf(input.charAt(i++));
                  enc2 = keyStr.indexOf(input.charAt(i++));
                  enc3 = keyStr.indexOf(input.charAt(i++));
                  enc4 = keyStr.indexOf(input.charAt(i++));

                  chr1 = (enc1 << 2) | (enc2 >> 4);
                  chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                  chr3 = ((enc3 & 3) << 6) | enc4;

                  output = output + String.fromCharCode(chr1);

                  if (enc3 != 64) {
                      output = output + String.fromCharCode(chr2);
                  }
                  if (enc4 != 64) {
                      output = output + String.fromCharCode(chr3);
                  }

                  chr1 = chr2 = chr3 = "";
                  enc1 = enc2 = enc3 = enc4 = "";

              } while (i < input.length);

              return output;
          }
      };

      /* jshint ignore:end */
  })
  .factory('authInterceptor', function ($injector, $rootScope, $cookieStore, $q, $window) {
      var retries = 0,
         waitBetweenErrors = 1000,
         maxRetries = 1;
        var $state;
      function onResponseError(httpConfig) {

          var $http = $injector.get('$http');
         
          console.log("---In onResponseError---");

          var authenticationService = $injector.get('AuthenticationService');

          console.log("---In authenticationService---" + authenticationService);

          setTimeout(function () {

              debugger;
              var isLogin = authenticationService.Relogin();
              if (isLogin === 0) {
                  $window.location = '#/login';
                  return;
              }

              $http(httpConfig).success(function (response) {
                  $rootScope.$broadcast('onResponseError', response);
              }
              ).error(function (response) {
                  console.log("---In error reponse---" + response);

              });

              //authenticationService.Relogin();then(function (reponse) {
              //    debugger;
              //    console.log("---In reponse---" + reponse);
              //    console.log("---In httpConfig---" + httpConfig);

              //    $http(httpConfig).success(function (response) {

              //        $rootScope.$broadcast('onResponseError', response);
              //    }).error(function (response) {


              //    });


              //}, deferred.reject);


              //return deferred.promise.then(function () {
              //    console.log("---In httpConfig---" + httpConfig);
              //    return $http(httpConfig);
              //});
          }, waitBetweenErrors);

      }
      function setState(stateName) {
          //if (!$state) {
          //    $injector.get('$state'); //Or just get $state from $injector always it is anyways the dependency container and service are singletons
          //}
          $state.go(stateName);
      }
      return {
          request: function (config) {
              config.headers = config.headers || {};
              $rootScope.globals2 = $cookieStore.get('token') || {};

              if ($rootScope.globals2.currentToken) {
                  console.log("---Trace Service-- In authInterceptor $rootScope.globals2.currentToken.token) Is '" + $rootScope.globals2.currentToken.token + "' ");
                  config.headers.Authorization = 'bearer ' + $rootScope.globals2.currentToken.token;
              }
              return config;
          },
          response: function (response) {

              if (response.status === 401) {

                  // handle the case where the user is not authenticated
              }
              return response || $q.when(response);
          },
          responseError: function (response) {

              if (response.status === 401 && retries < maxRetries) {
                  retries++;
                  onResponseError(response.config);
              }
              retries = 0;
              return $q.reject(response);
          }
      };
  })

