using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using System;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;


#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	public partial class AndroidUserProfilesManager : MonoBehaviour 
	{
		private AndroidJavaObject 	m_plugin;
		private AndroidJavaObject  	Plugin
		{
			get 
			{ 
				return m_plugin; 
			}
		}
		
		public void SetPluginInstance(AndroidJavaObject _plugin)
		{
			m_plugin = _plugin;
		}

		public void LoadUsers(string[] _userIDs, Action<User[]> _onCompletion)
		{
			OnLoadUsersFinished = _onCompletion;

			string _usersListJSON	   = _userIDs.ToJSON();			
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_USERS,_usersListJSON);
		}

		public void AuthenticateLocalUser(Action<AndroidUser> _onCompletion)
		{
			OnLocalUserAuthenticationFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.AUTHENTICATE_LOCAL_USER);
		}

		public void LoadLocalUserFriends(Action<AndroidUser[]> _onCompletion)
		{
			OnLoadLocalUserFriendsFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_LOCAL_USER_FRIENDS, false);
		}

		public void LoadProfilePicture(string _path, DownloadTexture.Completion _onCompletion)
		{
			string _newRequestID = System.Guid.NewGuid().ToString();
			m_profilePictureDownloadRequests.Add(_newRequestID, _onCompletion);

			if(_path.ToLower().StartsWith("http"))
			{
				OnReceivingProfilePicture(_newRequestID, _path);
			}
			else
			{
				Plugin.Call(AndroidNativeInfo.Methods.LOAD_PROFILE_PICTURE, _newRequestID, _path);
			}
		}
				
	}
}

#endif
