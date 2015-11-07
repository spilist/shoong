using UnityEngine;
using System.Collections;

#if USES_NOTIFICATION_SERVICE && UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NotificationServiceAndroid : NotificationService
	{

		#region Parse Methods
		
		protected override void ParseAppLaunchInfo (string _launchData, out CrossPlatformNotification _launchLocalNotification, out CrossPlatformNotification _launchRemoteNotification)
		{
			Dictionary<string, object> 	_payloadDict		= JSONUtility.FromJSON(_launchData) as Dictionary<string, object>;
			
			object						_isRemoteNotification = "false";

			if(_payloadDict != null)
			{
				_payloadDict.TryGetValue(AndroidNotificationPayload.kIsRemoteNotification, out _isRemoteNotification);
	
				Debug.LogError("Launch Notification  is Remote Notification ? " + _isRemoteNotification);
				// Launched with local notification
				if (_isRemoteNotification.Equals("false"))
				{
					_launchLocalNotification	= new AndroidNotificationPayload((IDictionary)_payloadDict);
					_launchRemoteNotification	= null;
				}
				// Launched with remote notification
				else
				{
					_launchLocalNotification	= null;
					_launchRemoteNotification	= new AndroidNotificationPayload((IDictionary)_payloadDict);
				}
			}
			// Normal launch
			else
			{
				_launchLocalNotification	= null;
				_launchRemoteNotification	= null;
			}
		}

		#endregion
	}
}
#endif