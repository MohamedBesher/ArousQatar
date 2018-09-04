var exec = require('cordova/exec');

var TwitterConnect = {
	login: function (successCallback, errorCallback) {
		exec(successCallback, errorCallback, 'TwitterConnect', 'login', []);
	},
	logout: function (successCallback, errorCallback) {
		exec(successCallback, errorCallback, 'TwitterConnect', 'logout', []);
	},
	showUser: function (successCallback, errorCallback) {
		exec(successCallback, errorCallback, 'TwitterConnect', 'showUser', []);
	}
};

module.exports = TwitterConnect;
