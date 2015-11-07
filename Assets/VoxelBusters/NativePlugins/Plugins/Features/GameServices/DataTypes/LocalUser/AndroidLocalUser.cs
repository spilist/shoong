using UnityEngine;
using System.Collections;


#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins.Internal;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidLocalUser : LocalUser 
	{
		#region Constants

		private 	const 	string 		kLocalUserFriendsKey	= "local-user-friends";
		private 	const 	string 		kLocalUserInfoKey		= "local-user-info";

		#endregion


		#region Fields
		
		private AndroidUser 	m_user;
		private	bool 			m_isInitialised;
		private	IDictionary		m_authResponseData;
		
		#endregion

		#region Properties
		
		public override string Identifier
		{
			get
			{
				return m_user.Identifier;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override string Name
		{
			get
			{
				return m_user.Name;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}

		public override	bool IsAuthenticated
		{
			get
			{
				AndroidJavaObject _plugin = GameServicesAndroid.Plugin;
				bool _isAuthFinished;

				if(_plugin	== null)
				{
					_isAuthFinished = false;
				}
				else
				{
					_isAuthFinished = GameServicesAndroid.Plugin.Call<bool>(GameServicesAndroid.Native.Methods.IS_LOCAL_USER_AUTHENTICATED);
				}
				
				return _isAuthFinished;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override User[] Friends 
		{
			get;
			protected set;
		}
		
		#endregion
		
		#region Methods
		
		public override void Authenticate (AuthenticationCompletion _onCompletion)
		{
			base.Authenticate(_onCompletion);
			
			// Request authentication
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.AUTHENTICATE_LOCAL_USER);
		}

		public override void SignOut (SignOutCompletion _onCompletion)
		{
			base.SignOut (_onCompletion);
			
			// Request signout
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.SIGN_OUT_LOCAL_USER);
		}
		
		public override void LoadFriends (LoadFriendsCompletion _onCompletion)
		{
			base.LoadFriends (_onCompletion);

			// Verify user
			if (!IsAuthenticated)
				return;

			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.LOAD_LOCAL_USER_FRIENDS, false);
		}
		
		public override void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			if (m_user == null)
			{
				if (_onCompletion != null)
					_onCompletion(null, Constants.kGameServicesUserAuthMissingError);
				
				return;
			}
			
			m_user.GetImageAsync(_onCompletion);
		}
		
		#endregion

		#region Event Callback Methods
		
		protected override void AuthenticationFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			
			if (_error == null)
			{
				m_authResponseData	= _dataDict;
			}
			else
			{
				// Update properties
				Friends				= null;
				m_user				= null;
			}
			
			AuthenticationFinished(_error);
		}

		protected override void SignOutFinished (IDictionary _dataDict)
		{
			string		_error		= 	_dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);

			SignOutFinished(_error == null, _error);			
		}
		
		protected override void LoadFriendsFinished (IDictionary _dataDict)
		{
			string		_error			= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			IList		_friendJSONList	= _dataDict.GetIfAvailable<List<object>>(kLocalUserFriendsKey);
			
			if (_friendJSONList != null)
			{
				// Update property
				Friends					= AndroidUser.ConvertToUserList(_friendJSONList);
			}
			
			LoadFriendsFinished(Friends, _error);
		}
		
		protected override void OnInitSuccess ()
		{
			IDictionary _infoDict	= m_authResponseData.GetIfAvailable<IDictionary>(kLocalUserInfoKey);
			
			// Update properties
			m_user				= new AndroidUser(_infoDict);
			
			// Reset needless data
			m_authResponseData		= null;
			
			base.OnInitSuccess ();
		}
		
		protected override void OnInitFail ()
		{
			// Update properties
			m_user					= null;
			
			// Reset needless data
			m_authResponseData		= null;
			
			base.OnInitFail ();
		}
		
		#endregion
	}
}
#endif