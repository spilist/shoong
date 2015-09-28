using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using System;
using System.Collections.Generic;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	public partial class AndroidUserProfilesManager : MonoBehaviour 
	{
		private event Action<User[]>		OnLoadUsersFinished;
		private event Action<AndroidUser> 	OnLocalUserAuthenticationFinished;
		private event Action<AndroidUser[]>	OnLoadLocalUserFriendsFinished;

		private Dictionary<string, DownloadTexture.Completion> m_profilePictureDownloadRequests = new Dictionary<string, DownloadTexture.Completion>();
		
		private void OnReceivingUserProfilesList(string _loadedUsersJsonStr) 
		{
			IList _usersJsonList				= JSONUtility.FromJSON(_loadedUsersJsonStr) as IList;
			
			// Send callback
			if (OnLoadUsersFinished != null)
			{
				OnLoadUsersFinished(AndroidUser.ConvertToUserList(_usersJsonList));
			}
		}

		private void OnReceivingLocalUserAuthenticationStatus(string _usersJsonStr) 
		{
			IDictionary _usersJson	= JSONUtility.FromJSON(_usersJsonStr) as IDictionary;
			AndroidUser _user 		= null;

			if(_usersJson != null)
			{
				_user = AndroidUser.ConvertToUser(_usersJson);
			}

			// Send internal callback
			if (OnLocalUserAuthenticationFinished != null)
			{
				OnLocalUserAuthenticationFinished(_user);
			}
		}

		private void OnReceivingLocalUsersFriendsList(string _friendsJsonStr) 
		{
			IList _friendsJsonList				= JSONUtility.FromJSON(_friendsJsonStr) as IList;
			
			// Send callback
			if (OnLoadLocalUserFriendsFinished != null)
			{
				OnLoadLocalUserFriendsFinished(AndroidUser.ConvertToUserList(_friendsJsonList));
			}
		}
		
		private void OnReceivingProfilePicture(string _responseStr)
		{
			string[] _components = _responseStr.Split(';');

			string _requestID 	= 	_components[0];
 			string _path		=	_components[1];
			OnReceivingProfilePicture(_requestID, _path);
		}

		private void OnReceivingProfilePicture(string _requestID, string _path)
		{
			//Remove from history
			DownloadTexture.Completion _callback = m_profilePictureDownloadRequests[_requestID];
			m_profilePictureDownloadRequests.Remove(_requestID);

			if(!string.IsNullOrEmpty(_path))
			{		
				URL _pathURL;
				bool _isFileURL = !_path.ToLower().StartsWith("http");

				if(!_isFileURL)
				{
					_pathURL = URL.URLWithString(_path);
				}
				else
				{
					_pathURL = URL.FileURLWithPath(_path);
				}
				
				// Download
				DownloadTexture _newDownload	= new DownloadTexture(_pathURL, true, true);
				_newDownload.OnCompletion		= (Texture2D _texture, string _error)=>{
					if (string.IsNullOrEmpty(_error))
					{
						if(_isFileURL)
						{
							FileOperations.Delete(_path);
						}

						//Call callback here with _texture
						if(_callback != null)
						{
							_callback(_texture, _error);
						}
					}
					else
					{
						VoxelBusters.DebugPRO.Console.LogError(Constants.kDebugTag, "[AndroidUserProfile] Texture download failed, URL=" + _pathURL.URLString);
						_callback(_texture, "Error loading texture!");
					}
				};
				
				_newDownload.StartRequest();
			}
			else
			{
				//Call callback here directly with null as texture
				_callback(null, "Error loading texture!");
			}
			
		} 
		
		
	}
}

#endif
