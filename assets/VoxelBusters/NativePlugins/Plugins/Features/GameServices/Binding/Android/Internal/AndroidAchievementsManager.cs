using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;

#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	internal partial class AndroidAchievementsManager : MonoBehaviour 
	{
		#region Varibles

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

		#endregion


		#region Methods

		public void ReportProgress (AndroidAchievement _achievement, Action<bool> _onCompletion)
		{
			m_reportProgressCallbacks[_achievement.Identifier] = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.REPORT_PROGRESS, _achievement.Identifier , (float)_achievement.PercentageCompleted, _onCompletion != null);	
		}
		
		public void LoadAchievementDescriptions (Action<AchievementDescription[]> _onCompletion)
		{			
			OnLoadAchievementDescriptionsFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_ACHIEVEMENT_DESCRIPTIONS);			
		}
		
		public void LoadAchievements (Action<Achievement[]> _onCompletion)
		{
			// Load achievements
			OnLoadAchievementsFinished = _onCompletion;
			Plugin.Call(AndroidNativeInfo.Methods.LOAD_ACHIEVEMENTS);
		}

		public void ShowUI()
		{
			// Show Achievements
			Plugin.Call(AndroidNativeInfo.Methods.SHOW_ACHIEVEMENTS_UI);			
		}
		
		public IDictionary GetAchievementData (string _id)
		{
			string _dataStr	=	Plugin.Call<string>(AndroidNativeInfo.Methods.GET_ACHIEVEMENT_DATA, _id);	
			IDictionary _achievementsDict	= JSONUtility.FromJSON(_dataStr) as IDictionary; 

			return _achievementsDict;
		}
	
		#endregion

	}
}

#endif
