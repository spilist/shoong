using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using VoxelBusters.Utility;

#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	internal partial class AndroidAchievementsManager : MonoBehaviour 
	{
		private event Action<AchievementDescription[]> 	OnLoadAchievementDescriptionsFinished;
		private event Action<Achievement[]> 			OnLoadAchievementsFinished;
		private Dictionary<string, Action<bool>> m_reportProgressCallbacks = new Dictionary<string, Action<bool>>();
		


		private void OnReceivingAchievementDescriptions(string _achievementDescriptionsJsonStr)
		{

			IList _achievementDescriptionsJsonList				= JSONUtility.FromJSON(_achievementDescriptionsJsonStr) as IList;

			//Parse data here and report callbacks if any.
			AchievementDescription[] _descriptions = AndroidAchievementDescription.ConvertAchievementDescriptionList(_achievementDescriptionsJsonList);
			
			if(OnLoadAchievementDescriptionsFinished != null)
			{
				OnLoadAchievementDescriptionsFinished(_descriptions);
			}
		}
		
		private void OnReceivingAchievements(string _achievementsJsonStr)
		{
			IList _achievementsJsonList				= JSONUtility.FromJSON(_achievementsJsonStr) as IList; 
			
			// Send callback
			if (OnLoadAchievementsFinished != null)
			{
				OnLoadAchievementsFinished(AndroidAchievement.ConvertAchievementList(_achievementsJsonList));
			}
		}

		private void OnReceivingReportProgress(string _statusStr) 
		{			
			string[] _components = _statusStr.Split(';');

			string _achievementID 	= _components[0];
			string _error 			= _components[1];

			bool _reportStatus = (string.IsNullOrEmpty(_error)) ? true : false;

			if(!_reportStatus)
			{
				Debug.LogError("Error Reporting Score : " + _error);
			}

			Action<bool> _callback = m_reportProgressCallbacks[_achievementID];
			m_reportProgressCallbacks.Remove(_achievementID);

			// Send callback
			if (_callback != null)
			{
				_callback(_reportStatus);
			}
		}
		
	}
}

#endif
