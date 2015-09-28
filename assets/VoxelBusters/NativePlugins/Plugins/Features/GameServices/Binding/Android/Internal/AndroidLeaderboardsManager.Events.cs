using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	internal partial class AndroidLeaderboardsManager : MonoBehaviour 
	{
		private event Leaderboard.LoadScoreCompletion 					OnLoadTopScoresFinished;
		private event Leaderboard.LoadScoreCompletion 					OnLoadPlayerCenteredScoresFinished;
		private event Leaderboard.LoadScoreCompletion 					OnLoadMoreScoresFinished;

		private Dictionary<string, Action<bool>> m_reportScoreCallbacks = new Dictionary<string, Action<bool>>();
		

		private void OnReceivingTopScores(string _scoresJsonStr)
		{
			OnReceivingScores(_scoresJsonStr, OnLoadTopScoresFinished);
		}
	
		private void OnReceivingPlayerCenteredScores(string _scoresJsonStr)
		{
			OnReceivingScores(_scoresJsonStr, OnLoadPlayerCenteredScoresFinished);
		}

		private void OnReceivingMoreScores(string _scoresJsonStr)
		{
			OnReceivingScores(_scoresJsonStr, OnLoadMoreScoresFinished);
		}

		private void OnReceivingScores(string _loadedScoresJsonStr, Leaderboard.LoadScoreCompletion _callback)
		{
			
			IList _scoresJsonList				= JSONUtility.FromJSON(_loadedScoresJsonStr) as IList;

			Score[] _scores = null;
			Score	_localUserScore = null;

			if(_scoresJsonList != null)
			{
				int _count = _scoresJsonList.Count;
				if(_count > 0)
				{
					_localUserScore = AndroidScore.ConvertScore(_scoresJsonList[_count - 1] as IDictionary);

					//Now remove the last element. As we stored user score in the last entry.
					_scoresJsonList.RemoveAt(_count - 1);

					_scores = AndroidScore.ConvertScoreList(_scoresJsonList);


					if(_localUserScore.User == null)//Empty entry
					{
						_localUserScore = null;
					}
				}
			}

			// Send callback
			if (_callback != null)
			{
				_callback(_scores, _localUserScore);
			}

		}


		private void OnReceivingReportScore(string _statusStr) 
		{			
			string[] _components = _statusStr.Split(';');
			
			string _leaderboardID 	= _components[0];
			string _error 			= _components[1];

			bool _reportStatus = string.IsNullOrEmpty(_error) ? true : false;

			if(!_reportStatus)
			{
				Debug.LogError("Error Reporting Score : " + _error + " LeaderboardID " + _leaderboardID);
			}

			Action<bool> _callback = m_reportScoreCallbacks[_leaderboardID];
			m_reportScoreCallbacks.Remove(_leaderboardID);

			// Send callback
			if (_callback != null)
			{
				_callback(_reportStatus);
			}
		}
		
	}
}

#endif
