using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;
using System.Globalization;
using System.Collections.Generic;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidScore : Score 
	{
		#region Constants
		
		private const string	kIdentifier			= "identifier";
		private const string	kUser				= "user";
		private const string	kValue				= "value";
		private const string	kDate				= "date";
		private const string	kFormattedValue		= "formatted-value";
		private const string	kRank				= "rank";
		
		#endregion
		
		#region Fields

		private				string					m_leaderboardID;
		private				User					m_user;
		private 			long					m_value;
		private 			DateTime				m_date;
		private				string					m_formattedValue;
		private				int 					m_rank;

		#endregion

		#region Properties

		public override string LeaderboardID
		{
			get
			{
				return m_leaderboardID;
			}

			protected set
			{
				m_leaderboardID	= value;
			}
		}
		

		public override User User
		{
			get
			{
				return m_user;
			}

			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override long Value
		{
			get
			{
				return m_value;
			}

			set
			{
				m_value			= value;
			}
		}
		
		public override DateTime Date
		{
			get
			{
				return m_date;
			}

			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override string FormattedValue
		{
			get
			{
				return m_formattedValue;
			}
		}
		
		public override int Rank
		{
			get
			{
				return m_rank;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}

		#endregion

		#region Constructors

		private AndroidScore ()
		{}

		internal AndroidScore (IDictionary _scoreData)
		{
			LeaderboardID			= _scoreData.GetIfAvailable<string>(kIdentifier);	
			m_user					= new AndroidUser(_scoreData.GetIfAvailable<Dictionary<string, object>>(kUser));
			m_value					= _scoreData.GetIfAvailable<long>(kValue);

			long _timeInMillis		= _scoreData.GetIfAvailable<long>(kDate);
			m_date 					= _timeInMillis.ToDateTimeFromJavaTime();

			m_formattedValue		= _scoreData.GetIfAvailable<string>(kFormattedValue);
			m_rank					= _scoreData.GetIfAvailable<int>(kRank);
		}

		internal AndroidScore (string _leaderBoardID, long _value)
		{
			LeaderboardID			= _leaderBoardID;
			m_value					= _value;
			m_date					= DateTime.Now;
			m_formattedValue		= _value.ToString("0,0", CultureInfo.InvariantCulture);
			m_rank					= -1;
		}

		#endregion

		#region Static Methods

		internal static AndroidScore ConvertScore (IDictionary _score)
		{
			if (_score == null)
				return null;

			return new AndroidScore(_score);
		}

		internal static AndroidScore[] ConvertScoreList (IList _scoreList)
		{
			if (_scoreList == null)
				return null;
			
			int					_count				= _scoreList.Count;
			AndroidScore[]		_androidScoreList	= new AndroidScore[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_androidScoreList[_iter]			= new AndroidScore(_scoreList[_iter] as IDictionary);
			
			return _androidScoreList;
		}

		#endregion

		#region Methods

		public override void ReportScore (Action<bool> _onCompletion)
		{
			AndroidLeaderboardsManager _leaderboardsManager = ((GameServicesAndroid)(NPBinding.GameServices)).LeaderboardsManager;

			_leaderboardsManager.ReportScore(this, _onCompletion);
		}

		#endregion
	}
}
#endif