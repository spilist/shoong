using UnityEngine;
using System.Collections;

#if UNITY_ANDROID

namespace VoxelBusters.NativePlugins.Internal
{
	#region Platform Native Info
	
	class AndroidNativeInfo
	{
		// Handler class name
		public class Class
		{
			public const string NAME			= "com.voxelbusters.nativeplugins.features.gameservices.GameServicesHandler";
		}
		
		// For holding method names
		public class Methods
		{
			public const string 	IS_SERVICE_AVAILABLE			=	"isServiceAvailable";
			
			//Achievements
			public const string 	REPORT_PROGRESS					=	"reportProgress";
			public const string 	LOAD_ACHIEVEMENT_DESCRIPTIONS	=	"loadAchievementDescriptions";
			public const string 	LOAD_ACHIEVEMENTS				=	"loadAchievements";
			public static string 	SHOW_ACHIEVEMENTS_UI 			=	"showAchievementsUi";
			public static string 	GET_ACHIEVEMENT_DATA 			=	"getAchievement";
			
			
			//Leaderboards
			public const string 	LOAD_TOP_SCORES					=	"loadTopScores";
			public const string 	LOAD_PLAYER_CENTERED_SCORES		=	"loadPlayerCenteredScores";
			public const string 	LOAD_MORE_SCORES				=	"loadMoreScores";
			public static string 	REPORT_SCORE 					=	"reportScore";
			public static string 	SHOW_LEADERBOARD_UI				=	"showLeaderboardsUi";

			//User Details
			public const string 	LOAD_USERS						=	"loadUsers";
			public const string 	LOAD_LOCAL_USER_FRIENDS			=	"loadLocalUserFriends";
			public const string 	AUTHENTICATE_LOCAL_USER			=	"authenticateLocalUser";

			public const string 	LOAD_PROFILE_PICTURE			=	"loadProfilePicture";
		}
	}
	
	#endregion
}

#endif
