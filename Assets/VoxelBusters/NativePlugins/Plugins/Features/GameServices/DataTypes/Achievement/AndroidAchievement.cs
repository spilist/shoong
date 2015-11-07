using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidAchievement : Achievement 
	{
		#region Constants
		private	const 		string				kAchievementInfoKey		= "achievement-info";
		
		private const 		string				kIdentifier				= "identifier";
		private const 		string				kPointsScored			= "points-scored";
		private const 		string				kCompleted				= "is-completed";
		private const 		string				kLastReportDate			= "last-report-date";
		private const 		string				kDescription			= "description";
		
		#endregion

		#region Fields
		
		private 			int					m_pointsScored;

		#endregion

		#region Properties

		public override	string Identifier
		{
			get;
			protected set;
		}

		public override	int PointsScored
		{
			get
			{
				if (Description == null)
					return 0;

				return m_pointsScored;
			}

			set
			{
				if (Description == null)
				{
					m_pointsScored	= 0;
				}
				else
				{
					int		_maxPoints	= Description.MaximumPoints;
					m_pointsScored		= Mathf.Min(value, _maxPoints);
				}
			}
		}
		
		public override bool Completed
		{
			get;
			protected set;
		}
		
		public override	DateTime LastReportedDate
		{
			get;
			protected set;
		}
		
		#endregion
		
		#region Constructors

		private AndroidAchievement ()
		{}

		public AndroidAchievement (string _globalIdentifier, string _identifier, int _pointsScored = 0) 
			: base (_globalIdentifier, _identifier, _pointsScored)
		{}
		
		internal AndroidAchievement (IDictionary _achievementData)
		{
			SetDetails(_achievementData);
		}
		
		internal void SetDetails(IDictionary _achievementData)
		{
			Identifier				= _achievementData.GetIfAvailable<string>(kIdentifier);	
			PointsScored			= _achievementData.GetIfAvailable<int>(kPointsScored);
			Completed				= _achievementData.GetIfAvailable<bool>(kCompleted);
			
			long _timeInMillis		= _achievementData.GetIfAvailable<long>(kLastReportDate);
			LastReportedDate 		= _timeInMillis.ToDateTimeFromJavaTime();
			
			PointsScored			= m_pointsScored;
			
			GlobalIdentifier		= GameServicesIDHandler.GetAchievementGID(Identifier);
		}

		#endregion

		#region Static Methods

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

		public override void ReportProgress (Achievement.ReportProgressCompletion _onCompletion)
		{
			base.ReportProgress(_onCompletion);

			if (Description == null)
				return;

			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.REPORT_PROGRESS, GetInstanceID(), Identifier , PointsScored, _onCompletion != null);	
		}

		#endregion


		#region Event Callback Methods
		
		protected override void ReportProgressFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			IDictionary _infoDict	= _dataDict.GetIfAvailable<IDictionary>(kAchievementInfoKey);
			
			if (_infoDict != null)
			{
				// Update properties
				SetDetails(_infoDict);
			}
			
			ReportProgressFinished(_error == null, _error);
		}
		
		#endregion
	}
}
#endif