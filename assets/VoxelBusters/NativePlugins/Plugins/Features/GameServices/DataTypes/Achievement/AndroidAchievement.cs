using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidAchievement : Achievement 
	{
		#region Constants
		
		private const 		string				kIdentifier				= "identifier";
		private const 		string				kPointsScored			= "points-scored";
		private const 		string				kCompleted				= "is-completed";
		private const 		string				kLastReportDate			= "last-report-date";
		private const 		string				kDescription			= "description";
		
		#endregion

		#region Fields
		
		private 			string 				m_identifier;
		private 			int					m_pointsScored;
		private 			float 				m_percentCompleted;
		private 			bool 				m_isCompleted;
		private				System.DateTime		m_lastReportedDate;

		#endregion

		#region Properties

		public override	string Identifier
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

		public override	int PointsScored
		{
			get
			{
				if (Description == null)
					return 0;

				return (int)(m_percentCompleted * Description.MaximumPoints);
			}

			set
			{
				if (Description == null)
					m_percentCompleted	= 0;
				else
					m_percentCompleted	= ((float)value / (float)Description.MaximumPoints);
			}
		}
		
		public override bool Completed
		{
			get
			{
				return m_isCompleted;
			}

			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override	DateTime LastReportedDate
		{
			get
			{
				return m_lastReportedDate;
			}

			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		#endregion
		
		#region Constructors

		private AndroidAchievement ()
		{}

		internal AndroidAchievement (string _identifier)
		{
			// Initialize properties
			m_identifier		= _identifier;
		}
		
		internal AndroidAchievement (IDictionary _achievementData)
		{

			m_identifier			= _achievementData.GetIfAvailable<string>(kIdentifier);	
			m_pointsScored			= _achievementData.GetIfAvailable<int>(kPointsScored);
			m_isCompleted			= _achievementData.GetIfAvailable<bool>(kCompleted);

			long _timeInMillis		= _achievementData.GetIfAvailable<long>(kLastReportDate);
			m_lastReportedDate 		= _timeInMillis.ToDateTimeFromJavaTime();

			PointsScored			= m_pointsScored;
		}
		
		#endregion

		#region Static Methods

		internal static AndroidAchievement Create(string _achievementId)
		{
			AndroidAchievementsManager _achievementManager = ((GameServicesAndroid)(NPBinding.GameServices)).AchievementsManager;
			IDictionary _achievementData = _achievementManager.GetAchievementData(_achievementId);

			if(_achievementData != null)
			{
				return new AndroidAchievement(_achievementData);
			}
			else
			{
				return new AndroidAchievement(_achievementId);
			}
		}

		internal static AndroidAchievement[] ConvertAchievementList (IList _achievementList)
		{
			if (_achievementList == null)
				return null;

			int 					_count					= _achievementList.Count;
			AndroidAchievement[]	_androidAchievementList	= new AndroidAchievement[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
			{
				_androidAchievementList[_iter]				= new AndroidAchievement(_achievementList[_iter] as IDictionary);
			}

			return _androidAchievementList;
		}

		#endregion

		#region Methods

		public override void ReportProgress (Action<bool> _onCompletion)
		{
			base.ReportProgress(_onCompletion);

			if (Description == null)
				return;

			AndroidAchievementsManager _achievementManager = ((GameServicesAndroid)(NPBinding.GameServices)).AchievementsManager;
			_achievementManager.ReportProgress(this, OnReportProgressFinished);
		}

		#endregion
	}
}
#endif