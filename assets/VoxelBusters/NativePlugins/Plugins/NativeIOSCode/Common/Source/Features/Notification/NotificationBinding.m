//
//  NotificationBinding.m
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NotificationBinding.h"
#import "NotificationHandler.h"

void initNotificationService (const char* keyForUserInfo)
{
	[[NotificationHandler Instance] initialize:ConvertToNSString(keyForUserInfo)];
}

void registerNotificationTypes (int notificationTypes)
{
	[[NotificationHandler Instance] registerUserNotificationTypes:notificationTypes];
}

void registerForRemoteNotifications ()
{
	[[NotificationHandler Instance] registerForRemoteNotifications];
}

void unregisterForRemoteNotifications ()
{
	[[NotificationHandler Instance] unregisterForRemoteNotifications];
}