using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.SocialPlatforms;
using VoxelBusters.DebugPRO;
using VoxelBusters.NativePlugins.Internal;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public sealed partial class GameServicesAndroid : GameServices 
	{
		#region  Variables
		
		private AndroidJavaObject 	m_plugin;
		private AndroidJavaObject  	Plugin
		{
			get 
			{ 
				if(m_plugin == null)
				{
					VoxelBusters.DebugPRO.Console.LogError(Constants.kDebugTag, "[GameServices] Plugin class not intialized!");
				}
				return m_plugin; 
			}
			
			set
			{
				m_plugin = value;
			}
		}

		private AndroidAchievementsManager m_achievementsManager;
		internal AndroidAchievementsManager  	AchievementsManager
		{
			get 
			{ 
				return m_achievementsManager; 
			}
		}

		private AndroidLeaderboardsManager m_leaderboardsManager;
		internal AndroidLeaderboardsManager LeaderboardsManager
		{
			get 
			{ 
				return m_leaderboardsManager; 
			}
		}

		private AndroidUserProfilesManager m_userProfilesManager;
		internal AndroidUserProfilesManager UserProfilesManager
		{
			get 
			{ 
				return m_userProfilesManager; 
			}
		}

		private AndroidLocalUser m_localUser;
		public override LocalUser LocalUser 
		{
			get 
			{
				return m_localUser;
			}
			
			protected set 
			{
				m_localUser = value as AndroidLocalUser;
			}
		}
			
		#endregion
		
		#region Constructors
		
		GameServicesAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(AndroidNativeInfo.Class.NAME);

			m_achievementsManager = gameObject.AddComponent<AndroidAchievementsManager>();
			m_achievementsManager.SetPluginInstance(Plugin);

			m_leaderboardsManager = gameObject.AddComponent<AndroidLeaderboardsManager>();
			m_leaderboardsManager.SetPluginInstance(Plugin);

			m_userProfilesManager = gameObject.AddComponent<AndroidUserProfilesManager>();
			m_userProfilesManager.SetPluginInstance(Plugin);	
		}
		
		#endregion

		#region Unity Methods

		protected override void Awake ()
		{
			base.Awake ();

			// Initialize
			LocalUser	= new AndroidLocalUser();
		}

		#endregion

		#region Methods

		public override bool IsAvailable ()
		{
			return Plugin.Call<bool>(AndroidNativeInfo.Methods.IS_SERVICE_AVAILABLE);
		}

		public override Leaderboard CreateLeaderboard (string _leaderboardID)
		{
			// Verify user authentication state before proceeding
			if (VerifyUser())
				return new AndroidLeaderboard(_leaderboardID);
			else
				return null;
		}
		
		public override Achievement CreateAchievement (string _achievementID)
		{
			// Verify user authentication state before proceeding
			if (VerifyUser())
				return AndroidAchievement.Create(_achievementID);
			else
				return null;
		}

		protected override void LoadAchievementDescriptions (bool _needsVerification, Action<AchievementDescription[]> _onCompletion)
		{
			base.LoadAchievementDescriptions(_needsVerification, _onCompletion);
			
			// Verify user authentication state before proceeding
			if (_needsVerification && !VerifyUser())
			{
				if (_onCompletion != null)
					_onCompletion(null);
				
				return;
			}
			
			AchievementsManager.LoadAchievementDescriptions(OnLoadAchievementDescriptionsFinished);
		}
		
		public override void LoadAchievements (Action<Achievement[]> _onCompletion)
		{
			base.LoadAchievements(_onCompletion);

			// Verify user authentication state before proceeding
			if (!VerifyUser())
			{
				if (_onCompletion != null)
					_onCompletion(null);
				
				return;
			}

			AchievementsManager.LoadAchievements(_onCompletion);
		}
		
		public override void LoadUsers (string[] _userIDs, Action<User[]> _onCompletion)
		{
			base.LoadUsers(_userIDs, _onCompletion);

			if (_userIDs == null)
				return;

			if (!VerifyUser())
			{
				if (_onCompletion != null)
					_onCompletion(null);
				
				return;
			}
			
			// Load users
			UserProfilesManager.LoadUsers(_userIDs, _onCompletion);
		}
		
		public override void ReportProgress (string _achievementID, int _pointsScored, Action<bool> _onCompletion)
		{
			base.ReportProgress(_achievementID, _pointsScored, _onCompletion);

			if (string.IsNullOrEmpty(_achievementID))
				return;
			
			// Verify user authentication state before proceeding
			if (!VerifyUser())
			{
				if (_onCompletion != null)
					_onCompletion(false);
				
				return;
			}

			// Create new instance
			AndroidAchievement	_achievement	= new AndroidAchievement(_achievementID);

			// Update percentage completed
			_achievement.PointsScored		= _pointsScored;

			// Report progress
			_achievement.ReportProgress(_onCompletion);
		}
		
		public override void ReportScore (string _leaderboardID, long _score, Action<bool> _onCompletion)
		{
			base.ReportScore(_leaderboardID, _score, _onCompletion);
			
			if (string.IsNullOrEmpty(_leaderboardID))
				return;

			// Verify user authentication state before proceeding
			if (!VerifyUser())
			{
				if (_onCompletion != null)
					_onCompletion(false);
				
				return;
			}

			// Report score
			AndroidScore		_newScore		= new AndroidScore(_leaderboardID, _score);

			_newScore.ReportScore(_onCompletion);
		}

		public override void ShowAchievementsUI (GameServiceViewClosed _onCompletion)
		{
			base.ShowAchievementsUI(_onCompletion);
			
			// Check if valid account
			if (!VerifyUser())
			{
				return;
			}

			AchievementsManager.ShowUI();
		}
		
		public override void ShowLeaderboardUI (string _leaderboardID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			base.ShowLeaderboardUI(_leaderboardID, _timeScope, _onCompletion);
			
			if (string.IsNullOrEmpty(_leaderboardID))
				return;

			// Check if valid account
			if (!VerifyUser())
			{
				return;
			}
			
			LeaderboardsManager.ShowUI(_leaderboardID, _timeScope);
		}

		#endregion
	}
}
#endif