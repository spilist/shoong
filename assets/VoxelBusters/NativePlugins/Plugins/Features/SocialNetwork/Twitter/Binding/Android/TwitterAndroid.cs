using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterAndroid : Twitter 
	{
		#region Platform Native Info
		
		class NativeInfo
		{
			// Handler class name
			public class Class
			{
				public const string NAME									= "com.voxelbusters.nativeplugins.features.socialnetwork.twitter.TwitterHandler";
			}
			
			// For holding method names
			public class Methods
			{
				public const string INITIALIZE		 						= "initialize";
				public const string LOGIN		 							= "login";
				public const string LOGOUT		 							= "logout";
				public const string IS_LOGGED_IN		 					= "isLoggedIn";
				public const string GET_AUTH_TOKEN		 					= "getAuthToken";
				public const string GET_AUTH_TOKEN_SECRET		 			= "getAuthTokenSecret";
				public const string GET_USER_ID		 						= "getUserId";
				public const string GET_USER_NAME		 					= "getUserName";
				public const string SHOW_TWEET_COMPOSER						= "showTweetComposer";
				public const string REQUEST_ACCOUNT_DETAILS 				= "requestAccountDetails";
				public const string REQUEST_EMAIL_ACCESS 					= "requestEmailAccess";
				public const string URL_REQUEST 							= "urlRequest";
			}
		}
		
		#endregion
		
		#region  Required Variables
		
		private AndroidJavaObject 	m_plugin;
		private AndroidJavaObject  	Plugin
		{
			get 
			{ 
				if(m_plugin == null)
				{
					Console.LogError(Constants.kDebugTag, "[Twitter] Plugin class not intialized!");
				}
				return m_plugin; 
			}
			
			set
			{
				m_plugin = value;
			}
		}
		
		#endregion
		
		#region Constructors
		
		TwitterAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}
		
		#endregion

		#region Init API's

		public override bool Initialise ()
		{
			if (base.Initialise())
			{
				TwitterSettings _twitterSettings	= NPSettings.SocialNetworkSettings.TwitterSettings;

				Plugin.Call(NativeInfo.Methods.INITIALIZE, _twitterSettings.ConsumerKey, _twitterSettings.ConsumerSecret);
				return true;
			}

			return false;
		}

		#endregion

		#region Account API's
			
		public override void Login (TWTRLoginCompletion _onCompletion)
		{
			base.Login(_onCompletion);

			if (base.Initialise())
			{
				// Native method is called
				Plugin.Call(NativeInfo.Methods.LOGIN);
			}
		}
		
		public override void Logout ()
		{
			// Native method is called
			Plugin.Call(NativeInfo.Methods.LOGOUT);
		}
		
		public override bool IsLoggedIn ()
		{
			bool _isLoggedIn	= Plugin.Call<bool>(NativeInfo.Methods.IS_LOGGED_IN);
			Console.Log(Constants.kDebugTag, "[Twitter] IsLoggedIn=" + _isLoggedIn);
			
			return _isLoggedIn;
		}
		
		public override string GetAuthToken ()
		{
			string _authToken	= Plugin.Call<string>(NativeInfo.Methods.GET_AUTH_TOKEN);
			Console.Log(Constants.kDebugTag, "[Twitter] AuthToken=" + _authToken);
			
			return _authToken;
		}
		
		public override string GetAuthTokenSecret ()
		{
			string _authTokenSecret	= Plugin.Call<string>(NativeInfo.Methods.GET_AUTH_TOKEN_SECRET);
			Console.Log(Constants.kDebugTag, "[Twitter] AuthTokenSecret=" + _authTokenSecret);
			
			return _authTokenSecret;
		}
		
		public override string GetUserID ()
		{
			string _userID	= Plugin.Call<string>(NativeInfo.Methods.GET_USER_ID);
			Console.Log(Constants.kDebugTag, "[Twitter] UserID=" + _userID);
			
			return _userID;
		}
		
		public override string GetUserName ()
		{
			string _userName	= Plugin.Call<string>(NativeInfo.Methods.GET_USER_NAME);
			Console.Log(Constants.kDebugTag, "[Twitter] UserName=" + _userName);
			
			return _userName;
		}

		#endregion

		#region Tweet API's
		
		public override void ShowTweetComposer (string _message, string _URL, byte[] _imgByteArray, TWTRTweetCompletion _onCompletion)
		{
			base.ShowTweetComposer(_message, _URL, _imgByteArray, _onCompletion);

			// Get byte array length
			int _arrayLength	= (_imgByteArray == null) ? 0 : _imgByteArray.Length;
			
			// Native method is called
			Plugin.Call(NativeInfo.Methods.SHOW_TWEET_COMPOSER, _message, _URL, _imgByteArray, _arrayLength);
		}
		
		#endregion
		
		#region Request API's
		
		public override void RequestAccountDetails (TWTRAccountDetailsCompletion _onCompletion)
		{
			base.RequestAccountDetails(_onCompletion);

			// Native method is called
			Plugin.Call(NativeInfo.Methods.REQUEST_ACCOUNT_DETAILS);
		}
		
		public override void RequestEmailAccess (TWTREmailAccessCompletion _onCompletion)
		{
			base.RequestEmailAccess(_onCompletion);

			// Native method is called
			Plugin.Call(NativeInfo.Methods.REQUEST_EMAIL_ACCESS);
		}
		
		protected override void URLRequest (string _methodType, string _URL, IDictionary _parameters, TWTRResonse _onCompletion)
		{
			base.URLRequest(_methodType, _URL, _parameters, _onCompletion);

			// Native method is called
			Plugin.Call(NativeInfo.Methods.URL_REQUEST, _methodType, _URL, _parameters.ToJSON());
		}
		
		#endregion
	}
}
#endif