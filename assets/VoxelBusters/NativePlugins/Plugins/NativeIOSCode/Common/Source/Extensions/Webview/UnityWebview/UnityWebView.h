//
//  UnityWebView.h
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 21/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "CustomWebView.h"

@interface UnityWebView : CustomWebView

// Loading
- (NSString *)stringByEvaluatingJavaScriptFromString:(NSString *)script notifyUnity:(BOOL)needsToNotify;

@end
