package com.manifestwebdesign.twitterconnect;

import android.app.Activity;
import android.content.Context;
import android.util.Log;

import com.twitter.sdk.android.core.*;
import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CordovaWebView;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Intent;

import com.twitter.sdk.android.Twitter;
import com.twitter.sdk.android.core.identity.TwitterLoginButton;

import io.fabric.sdk.android.Fabric;
import retrofit.client.Response;
import retrofit.http.GET;
import retrofit.http.Query;
import retrofit.mime.TypedByteArray;

public class TwitterConnect extends CordovaPlugin {

	private static final String LOG_TAG = "Twitter Connect";
	private String action;

	public void initialize(CordovaInterface cordova, CordovaWebView webView) {
		super.initialize(cordova, webView);
		Fabric.with(cordova.getActivity().getApplicationContext(), new Twitter(new TwitterAuthConfig(getTwitterKey(), getTwitterSecret())));
		Log.v(LOG_TAG, "Initialize TwitterConnect");
	}

	private String getTwitterKey() {
		return preferences.getString("TwitterConsumerKey", "");
	}

	private String getTwitterSecret() {
		return preferences.getString("TwitterConsumerSecret", "");
	}

	public boolean execute(final String action, JSONArray args, final CallbackContext callbackContext) throws JSONException {
		Log.v(LOG_TAG, "Received: " + action);
		this.action = action;
		final Activity activity = this.cordova.getActivity();
		final Context context = activity.getApplicationContext();
		cordova.setActivityResultCallback(this);
		if (action.equals("login")) {
			login(activity, callbackContext);
			return true;
		}
		if (action.equals("logout")) {
			logout(callbackContext);
			return true;
		}
		if (action.equals("showUser")) {
			showUser(callbackContext);
			return true;
		}
		return false;
	}

	private void login(final Activity activity, final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {
			@Override
			public void run() {
				Twitter.logIn(activity, new Callback<TwitterSession>() {
					@Override
					public void success(Result<TwitterSession> twitterSessionResult) {
						Log.v(LOG_TAG, "Successful login session!");
						callbackContext.success(handleResult(twitterSessionResult.data));

					}

					@Override
					public void failure(TwitterException e) {
						Log.v(LOG_TAG, "Failed login session");
						callbackContext.error("Failed login session");
					}
				});
			}
		});
	}

	private void logout(final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {
			@Override
			public void run() {
				Twitter.logOut();
				Log.v(LOG_TAG, "Logged out");
				callbackContext.success();
			}
		});
	}

	/**
	 * Extends TwitterApiClient adding our additional endpoints
	 * via the custom 'UserService'
	 */
	class UserServiceApi extends TwitterApiClient {
		public UserServiceApi(TwitterSession session) {
			super(session);
		}

		public UserService getCustomService() {
			return getService(UserService.class);
		}
	}

	interface UserService {
		@GET("/1.1/users/show.json")
		void show(@Query("user_id") long id, Callback<Response> cb);
	}

	private void showUser(final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {
			@Override
			public void run() {
				UserServiceApi twitterApiClient = new UserServiceApi(Twitter.getSessionManager().getActiveSession());
				UserService userService = twitterApiClient.getCustomService();
				userService.show(Twitter.getSessionManager().getActiveSession().getUserId(), new Callback<Response>() {
					@Override
					public void success(Result<Response> result) {
						try {
							callbackContext.success(new JSONObject(new String(((TypedByteArray) result.response.getBody()).getBytes())));
						} catch (JSONException e) {
							e.printStackTrace();
						}
					}
					@Override
					public void failure(TwitterException exception) {
						Log.v(LOG_TAG, "Twitter API Failed "+exception.getLocalizedMessage());
						callbackContext.error(exception.getLocalizedMessage());
					}
				});
			}
		});
	}
	
	private JSONObject handleResult(TwitterSession result) {
		JSONObject response = new JSONObject();
		try {
			response.put("userName", result.getUserName());
			response.put("userId", result.getUserId());
			response.put("secret", result.getAuthToken().secret);
			response.put("token", result.getAuthToken().token);
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return response;
	}

	private void handleLoginResult(int requestCode, int resultCode, Intent intent) {
		TwitterLoginButton twitterLoginButton = new TwitterLoginButton(cordova.getActivity());
		twitterLoginButton.onActivityResult(requestCode, resultCode, intent);
	}

	public void onActivityResult(int requestCode, int resultCode, Intent intent) {
		super.onActivityResult(requestCode, resultCode, intent);
		Log.v(LOG_TAG, "activity result: " + requestCode + ", code: " + resultCode);
		if (action.equals("login")) {
			handleLoginResult(requestCode, resultCode, intent);
		}
	}
}
