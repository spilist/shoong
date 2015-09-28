using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NotificationServiceAndroid : NotificationService
	{
		#region Platform Native Info
		
		class NativeInfo
		{
			// Handler class name
			public class Class
			{
				public const string NAME								= "com.voxelbusters.nativeplugins.features.notification.NotificationHandler";
			}
			
			// For holding method names
			public class Methods
			{
				public const string INITIALIZE		 					= "initialize";
				public const string SET_NOTIFICATION_TYPE		 		= "setNotificationType";
				public const string CANCEL_LOCAL_NOTIFICATION 			= "cancelLocalNotification";
				public const string CANCEL_ALL_LOCAL_NOTIFICATIONS		= "cancelAllLocalNotifications";
				public const string SCHEDULE_LOCAL_NOTIFICATION			= "scheduleLocalNotification";
				public const string CLEAR_ALL_NOTIFICATIONS				= "clearAllNotifications";
				
				public const string REGISTER_REMOTE_NOTIFICATIONS 		= "registerRemoteNotifications";
				public const string UNREGISTER_REMOTE_NOTIFICATIONS 	= "unregisterRemoteNotifications";
			}
		}
		
		#endregion

		#region Constants
		
		private const string kUserInfo 			= "user-info";
		private const string kTickerText 		= "ticker-text";
		private const string kContentTitle 		= "content-title";
		private const string kContentText 		= "content-text";
		private const string kTag			 	= "notification-tag";

		#endregion
		
		#region  Required Variables
		
		private AndroidJavaObject 	m_plugin;
		private AndroidJavaObject  	Plugin
		{
			get 
			{ 
				if(m_plugin == null)
				{
					Console.LogError(Constants.kDebugTag, "[NotificationService] Plugin class not intialized!");
				}
				return m_plugin; 
			}
			
			set
			{
				m_plugin = value;
			}
		}
		
		#endregion
		
		#region Constructors
		
		NotificationServiceAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}
		
		#endregion

		#region Initialise
		
		protected override void Initialise (NotificationServiceSettings _settings)
		{
			Dictionary<string, string> customKeys = getCustomKeysForNotfication(_settings);
			
			//Pass sender id list and customkeys to Native platform
			SendConfigInfoToNative(_settings.Android.SenderIDList, customKeys, _settings.Android.NeedsBigStyle, _settings.Android.WhiteSmallIcon);
		
			// Get notifications received at launch
			getLaunchNotifications();
		}
		
		#endregion
		
		#region Misc

		void getLaunchNotifications()
		{
			//TODO will do by saving to prefs on native side after receiving notification - if app is not running.
			//Not implemented yet for Android as we don't override the root activity for other benefits which helps in easy integration withother plugins.
		}

		Dictionary<string, string> getCustomKeysForNotfication(NotificationServiceSettings _settings)
		{
			Dictionary<string, string> _data =  new Dictionary<string, string>();
			
			_data.Add(kUserInfo, AndroidNotificationPayload.UserInfoKey);
			_data.Add(kTickerText, AndroidNotificationPayload.TickerTextKey);
			_data.Add(kContentTitle, AndroidNotificationPayload.ContentTitleKey);
			_data.Add(kContentText, AndroidNotificationPayload.ContentTextKey);
			_data.Add(kTag, AndroidNotificationPayload.TagKey);
			
			//Add collapse key and extra stuff as needed //TODO
			
			return _data;
			
		}

		#endregion

		#region Overriden Local Notification API'S

		public override void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			base.RegisterNotificationTypes(_notificationTypes);
			
			Plugin.Call(NativeInfo.Methods.SET_NOTIFICATION_TYPE, (int)_notificationTypes);
		}

		public override string ScheduleLocalNotification (CrossPlatformNotification _notification)
		{
			string _notificationID		= _notification.GenerateNotificationID();

			// Create meta info and pass to native
			IDictionary _payLoadInfo	= AndroidNotificationPayload.CreateNotificationPayload(_notification);

			// Scheduling notification
			Plugin.Call(NativeInfo.Methods.SCHEDULE_LOCAL_NOTIFICATION, _payLoadInfo.ToJSON());
			
			return _notificationID;
		}
		
		public override void CancelLocalNotification (string _notificationID)
		{
			Plugin.Call(NativeInfo.Methods.CANCEL_LOCAL_NOTIFICATION, _notificationID);
		}
		
		public override void CancelAllLocalNotification ()
		{
			Plugin.Call(NativeInfo.Methods.CANCEL_ALL_LOCAL_NOTIFICATIONS);
		}

		public override void ClearNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.CLEAR_ALL_NOTIFICATIONS);
		}
		
		#endregion
		
		#region Overriden Remote Notification API's
		
		public override void RegisterForRemoteNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.REGISTER_REMOTE_NOTIFICATIONS);
		}
		
		public override void UnregisterForRemoteNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.UNREGISTER_REMOTE_NOTIFICATIONS);
		}

		#endregion

		protected void SendConfigInfoToNative(string[] _senderIDs, Dictionary<string,string> _customKeysInfo, bool _needsBigStyle, Texture2D _whiteSmallNotificationIcon)
		{
			if (_senderIDs.Length == 0)
			{
				Console.LogError(Constants.kDebugTag, "Add senderid list for notifications to work");
			}

			List<string> list =  new List<string>(_senderIDs);
			
			//Pass this to android
			Plugin.Call(NativeInfo.Methods.INITIALIZE,list.ToJSON(),_customKeysInfo.ToJSON(), _needsBigStyle, _whiteSmallNotificationIcon == null ? false : true);
		}
	}
}
#endif