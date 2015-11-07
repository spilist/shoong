using UnityEngine;
using System.Collections;

#if USES_NOTIFICATION_SERVICE && UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NotificationServiceAndroid : NotificationService
	{

		#region Constants
		
		private const string kUserInfo 			= "user-info";
		private const string kTickerText 		= "ticker-text";
		private const string kContentTitle 		= "content-title";
		private const string kContentText 		= "content-text";
		private const string kTag			 	= "notification-tag";
		private const string kCustomSoundKey	= "custom-sound";
		

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
			Dictionary<string, string> customKeys = GetCustomKeysForNotfication(_settings);
			
			//Pass sender id list and customkeys to Native platform
			SendConfigInfoToNative(_settings.Android.SenderIDList, customKeys, _settings.Android.NeedsBigStyle, _settings.Android.WhiteSmallIcon, _settings.Android.AllowVibration);
		}

		public override void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			base.RegisterNotificationTypes(_notificationTypes);
			
			Plugin.Call(NativeInfo.Methods.SET_NOTIFICATION_TYPES, (int)_notificationTypes);
		}
		
		#endregion

		#region Overriden Local Notification API'S

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


		#region Overriden Common Local & Notification Notification API'S
		
		public override void ClearNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.CLEAR_ALL_NOTIFICATIONS);
		}
		
		#endregion


		#region Helpers

		private Dictionary<string, string> GetCustomKeysForNotfication(NotificationServiceSettings _settings)
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

		protected void SendConfigInfoToNative(string[] _senderIDs, Dictionary<string,string> _customKeysInfo, bool _needsBigStyle, Texture2D _whiteSmallNotificationIcon, bool _allowVibration)
		{
			if (_senderIDs.Length == 0)
			{
				Console.LogError(Constants.kDebugTag, "Add senderid list for notifications to work");
			}

			List<string> list =  new List<string>(_senderIDs);
			
			//Pass this to android
			Plugin.Call(NativeInfo.Methods.INITIALIZE,list.ToJSON(),_customKeysInfo.ToJSON(), _needsBigStyle, _whiteSmallNotificationIcon == null ? false : true, _allowVibration);
		}
		
		#endregion
	}
}
#endif