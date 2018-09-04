
#import <Foundation/Foundation.h>
#import "TwitterConnect.h"
#import <Fabric/Fabric.h>
#import <TwitterKit/TwitterKit.h>

@implementation TwitterConnect

- (void)pluginInitialize
{
    
    NSString* consumerKey = [self.commandDelegate.settings objectForKey:[@"TwitterConsumerKey" lowercaseString]];
    NSString* consumerSecret = [self.commandDelegate.settings objectForKey:[@"TwitterConsumerSecret" lowercaseString]];
    [[Twitter sharedInstance] startWithConsumerKey:consumerKey consumerSecret:consumerSecret];
    [Fabric with:@[[Twitter sharedInstance]]];
    
    [Fabric with:@[TwitterKit]];
}


- (void)login:(CDVInvokedUrlCommand*)command
{
    [[Twitter sharedInstance] logInWithCompletion:^(TWTRSession *session, NSError *error) {
		CDVPluginResult* pluginResult = nil;
		if (session){
			NSLog(@"signed in as %@", [session userName]);
			NSDictionary *userSession = @{
										  @"userName": [session userName],
										  @"userId": [session userID],
										  @"secret": [session authTokenSecret],
										  @"token" : [session authToken]};
			pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:userSession];
		} else {
			NSLog(@"error: %@", [error localizedDescription]);
			pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:[error localizedDescription]];
		}
		[self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
	}];
}

- (void)logout:(CDVInvokedUrlCommand*)command
{
    [[Twitter sharedInstance] logOut];
	CDVPluginResult* pluginResult = pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
	[self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
}

- (void)showUser:(CDVInvokedUrlCommand*)command
{
	TWTRAPIClient *apiClient = [[Twitter sharedInstance] APIClient];

	NSDictionary *requestParameters = [NSDictionary dictionaryWithObjectsAndKeys:[[[Twitter sharedInstance] session] userID], @"user_id", nil];
	NSError *error = nil;
	NSURLRequest *apiRequest = [apiClient URLRequestWithMethod:@"GET"
														   URL:@"https://api.twitter.com/1.1/users/show.json"
													parameters:requestParameters
														 error:&error];
	[apiClient sendTwitterRequest:apiRequest
					   completion:^(NSURLResponse *response, NSData *data, NSError *error) {
						   NSHTTPURLResponse *httpResponse = (NSHTTPURLResponse *)response;
						   NSInteger _httpStatus = [httpResponse statusCode];

						   CDVPluginResult *pluginResult = nil;
						   NSLog(@"API Response :%@",response);
						   if (error != nil) {
							   pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:[error localizedDescription]];
						   } else if (_httpStatus == 200) {
							   NSDictionary *resultDict = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
							   pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:resultDict];
						   }
						   [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];

					   }];
}

@end
