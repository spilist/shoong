using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;

#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	internal partial class AndroidLeaderboardsManager : MonoBehaviour 
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

		public void LoadTopScores (AndroidLeaderboard _leaderboard, int _maxResults, Leaderboard.LoadScoreCompletion _onCompletion)
		{
			OnLoadTopScoresFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_TOP_SCORES, _leaderboard.Identifier, GetTimeScopeString(_leaderboard.TimeScope), GetUserScopeString(_leaderboard.UserScope), _maxResults);			
		}

		public void LoadPlayerCenteredScores (AndroidLeaderboard _leaderboard, int _maxResults, Leaderboard.LoadScoreCompletion _onCompletion)
		{
			OnLoadPlayerCenteredScoresFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_PLAYER_CENTERED_SCORES, _leaderboard.Identifier, GetTimeScopeString(_leaderboard.TimeScope), GetUserScopeString(_leaderboard.UserScope), _maxResults);			
		}

		public void LoadMoreScores (AndroidLeaderboard _leaderboard, int _maxResults, eLeaderboardPageDirection _direction, Leaderboard.LoadScoreCompletion _onCompletion)
		{
			OnLoadMoreScoresFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_MORE_SCORES, _leaderboard.Identifier, (int)_direction, _maxResults);
		}
		
		public  void ReportScore (Score _score, Action<bool> _onCompletion)
		{			
			m_reportScoreCallbacks[_score.LeaderboardID] = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.REPORT_SCORE, _score.LeaderboardID, _score.Value, _onCompletion != null);
		}

		public void ShowUI(string _leaderboardID, eLeaderboardTimeScope _timeScope)
		{
			// Show leaderboard
			Plugin.Call(AndroidNativeInfo.Methods.SHOW_LEADERBOARD_UI, _leaderboardID, GetTimeScopeString(_timeScope));
			
		}


		#region Helpers
		
		private string GetTimeScopeString(eLeaderboardTimeScope _timeScope)
		{
			return  AndroidLeaderboard.kTimeScopeMap.GetKey<eLeaderboardTimeScope>(_timeScope);
		}

		private string GetUserScopeString(eLeaderboardUserScope _userScope)
		{
			return  AndroidLeaderboard.kUserScopeMap.GetKey<eLeaderboardUserScope>(_userScope);
		}

		#endregion
	}
}

#endif
