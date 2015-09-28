using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;
using System.Collections.Generic;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidLeaderboard : Leaderboard 
	{
		#region Constants
		
		private const string	kIdentifier				= "identifier";
		private const string	kUserScope				= "user-scope";
		private const string	kTimeScope				= "time-scope";
		private const string	kTitle					= "title";
		private const string	kScores					= "scores";
		private const string	kLocalUserScore			= "local-user-score";

	
		private const string	kUserScopeGlobal		= "user-scope-gobal";
		private const string	kUserScopeFriendsOnly	= "user-scope-friends";

		private const string	kTimeScopeToday			= "time-scope-today";
		private const string	kTimeScopeWeek			= "time-scope-week";
		private const string	kTimeScopeAllTime		= "time-scope-all-time";
	
		
		#region Mapping For Parsing
		
		public static Dictionary<string, eLeaderboardUserScope> kUserScopeMap = new Dictionary<string, eLeaderboardUserScope>()
		{
			{ kUserScopeGlobal, eLeaderboardUserScope.GLOBAL},
			{ kUserScopeFriendsOnly, eLeaderboardUserScope.FRIENDS_ONLY}
		};
		
		public static Dictionary<string, eLeaderboardTimeScope> kTimeScopeMap = new Dictionary<string, eLeaderboardTimeScope>()
		{
			{ kTimeScopeToday, eLeaderboardTimeScope.TODAY},
			{ kTimeScopeWeek, eLeaderboardTimeScope.WEEK},
			{ kTimeScopeAllTime, eLeaderboardTimeScope.ALL_TIME},
		};
		
		#endregion

		#endregion
		
		#region Fields
		
		private				string					m_identifier;
		private				eLeaderboardUserScope	m_userScope;
		private 			eLeaderboardTimeScope	m_timeScope;
		private				bool					m_loading;
		private				string					m_title;
		private				AndroidScore[]			m_scores;
		private 			AndroidScore			m_localUserScore;

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
				m_identifier	= value;
			}
		}
		
		public override eLeaderboardUserScope UserScope
		{
			get
			{
				return m_userScope;
			}
			
			set
			{
				m_userScope	= value;
			}
		}
		
		public override eLeaderboardTimeScope TimeScope
		{
			get
			{
				return m_timeScope;
			}
			
			set
			{
				m_timeScope	= value;
			}
		}
		
		public override string Title
		{
			get
			{
				return m_title;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override Score[] Scores
		{
			get
			{
				return m_scores;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override Score LocalUserScore
		{
			get
			{
				return m_localUserScore;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}

		#endregion

		#region Constructors

		private AndroidLeaderboard ()
		{}

		internal AndroidLeaderboard (string _identifier)
		{
			Identifier				= _identifier;
			UserScope 				= eLeaderboardUserScope.GLOBAL;
			TimeScope 				= eLeaderboardTimeScope.ALL_TIME;
		}
		
		internal AndroidLeaderboard (IDictionary _leaderboardData)
		{
			m_identifier			= _leaderboardData.GetIfAvailable<string>(kIdentifier);	

			string _userScope		= _leaderboardData.GetIfAvailable<string>(kUserScope);
			m_userScope				= kUserScopeMap[_userScope];

			string _timeScope		= _leaderboardData.GetIfAvailable<string>(kTimeScope);
			m_timeScope				= kTimeScopeMap[_timeScope];

			m_title					= _leaderboardData.GetIfAvailable<string>(kTitle);
			
			IList _scoresList		= _leaderboardData.GetIfAvailable<List<object>>(kScores);			
			m_scores				= AndroidScore.ConvertScoreList(_scoresList);

			IDictionary _localScore	= _leaderboardData.GetIfAvailable<Dictionary<string, object>>(kLocalUserScore);			
			m_localUserScore		= AndroidScore.ConvertScore(_localScore);
		}
		
		#endregion

		#region Methods
		
		public override	void LoadTopScores (LoadScoreCompletion _onCompletion)
		{
			AndroidLeaderboardsManager _leaderboardsManager = GetLeaderboardManager();			
			_leaderboardsManager.LoadTopScores(this, MaxResults, _onCompletion);
		}
		
		public override	void LoadPlayerCenteredScores (LoadScoreCompletion _onCompletion)
		{
			AndroidLeaderboardsManager _leaderboardsManager = GetLeaderboardManager();			
			_leaderboardsManager.LoadPlayerCenteredScores(this, MaxResults, _onCompletion);
			 
		}
		
		public override	void LoadMoreScores (eLeaderboardPageDirection _pageDirection, LoadScoreCompletion _onCompletion)
		{
			AndroidLeaderboardsManager _leaderboardsManager = GetLeaderboardManager();
			_leaderboardsManager.LoadMoreScores(this, MaxResults, _pageDirection, _onCompletion);
		
		}

		#endregion

		#region Helpers

		private AndroidLeaderboardsManager GetLeaderboardManager()
		{
			return ((GameServicesAndroid)(NPBinding.GameServices)).LeaderboardsManager;
		}

		#endregion
	}
}
#endif