using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using VoxelBusters.NativePlugins.Internal;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidLocalUser : LocalUser 
	{
		#region Fields
		
		private AndroidUser m_user;
		private	bool m_isAuthenticated;
		private	User[] m_friends;

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
				return m_isAuthenticated;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override User[] Friends 
		{
			get
			{
				return m_friends;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		#endregion
		
		#region Constructors
		
		public AndroidLocalUser ()
		{
				
		}
		
		#endregion
		
		#region Methods
		
		public override void Authenticate (Action<bool> _onCompletion)
		{
			base.Authenticate(_onCompletion);
			
			// Request authentication
			AndroidUserProfilesManager _profilesManager = GetUserProfilesManager();
			_profilesManager.AuthenticateLocalUser((AndroidUser _user) => {

				// Check auth status
				bool 	_authSuccess	= _user != null;

				// Update authentication callback
				m_authCallback	= (bool _success)=>{
	
					m_isAuthenticated 	= _success;

					// Set properties
					if (_success)
						m_user			= _user;
					else
						m_user			= null;

					if(_onCompletion != null)
					{
						_onCompletion(_success);
					}
				};
				
				OnAuthenticationFinish(_authSuccess);
			});
		}
		
		public override void LoadFriends (Action<User[]> _onCompletion)
		{
			AndroidUserProfilesManager _profilesManager = GetUserProfilesManager();
			_profilesManager.LoadLocalUserFriends((AndroidUser[] _friendsJSONList)=>{
				
				if (_onCompletion != null)
				{
					if (_friendsJSONList != null)
					{
						m_friends = AndroidUser.ConvertToUserList(_friendsJSONList);
						_onCompletion(m_friends);
					}
					else
					{
						_onCompletion(null);
					}
				}
				
			});
		}
		
		public override void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			if (_onCompletion != null)
			{
				if(m_user != null)
				{
					m_user.GetImageAsync(_onCompletion);
				}
				else
				{
					_onCompletion(null, "Local user not found. Authenticate first.");
				}
			}
		}
		
		#endregion

		#region Helpers
	
		private AndroidUserProfilesManager GetUserProfilesManager()
		{
			return ((GameServicesAndroid)(NPBinding.GameServices)).UserProfilesManager;
		}

		#endregion
	}
}
#endif