using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidUser : User 
	{

		#region Constants
		
		internal const string	kIdentifier			= "identifier";
		internal const string	kName				= "name";
		internal const string	kHighResImageURL	= "high-res-image-url";
		internal const string	kIconImageURL		= "icon-image-url";
		internal const string	kTimeStamp			= "timestamp";

		#endregion

		#region Fields
		
		private	string		m_identifier;
		private	string		m_name;
		private string 		m_imagePath;
		private Texture2D 	m_imageTexture; 

		#endregion

		#region Properties

		public override string Identifier
		{
			get
			{
				return m_identifier;
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
				return m_name;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		#endregion
		
		#region Constructors
		
		internal AndroidUser (IDictionary _userProfileData)
		{
			if(_userProfileData != null)
			{
				m_identifier		= _userProfileData.GetIfAvailable<string>(kIdentifier);
				m_name				= _userProfileData.GetIfAvailable<string>(kName);
				m_imagePath			= _userProfileData.GetIfAvailable<string>(kHighResImageURL);
			}
			
		}
		
		#endregion
		
		#region Static Methods
		
		internal static AndroidUser ConvertToUser (IDictionary _user)
		{
			if (_user == null)
				return null;
			
			return new AndroidUser(_user);
		}
		
		internal static AndroidUser[] ConvertToUserList (IList _userList)
		{
			if (_userList == null)
				return null;
			
			int					_count				= _userList.Count;
			AndroidUser[]		_androidUsersList	= new AndroidUser[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_androidUsersList[_iter]			= new AndroidUser(_userList[_iter] as IDictionary);
			
			return _androidUsersList;
		}
		
		#endregion

		#region Methods
		
		public override void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			if (_onCompletion != null)
			{
				if (string.IsNullOrEmpty(m_imagePath))
				{
					VoxelBusters.DebugPRO.Console.LogError(Constants.kDebugTag, "[GameServices] No profile image for " + Name);					
					_onCompletion(null, "Texture not found.");
				}
				else if(m_imageTexture != null) //If already cached
				{
					_onCompletion(m_imageTexture, null);
				}
				else
				{
					AndroidUserProfilesManager _userManager = ((GameServicesAndroid)(NPBinding.GameServices)).UserProfilesManager;

					_userManager.LoadProfilePicture(m_imagePath, (Texture2D _texture, string _error)=>{
						m_imageTexture = _texture;
						if(_onCompletion != null  && _onCompletion.Target != null)
						{
							_onCompletion(_texture, _error);
						}
					});	

				}
			}
		}
		
		#endregion
	}
}
#endif