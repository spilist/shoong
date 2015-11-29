using UnityEngine;
using System.Collections;

#if USES_TWITTER && UNITY_ANDROID
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterAndroid : Twitter 
	{	
		#region Constructors
		
		TwitterAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region Init API's

		public override bool Initialise ()
		{
			if (base.Initialise())
			{
				TwitterSettings _twitterSettings	= NPSettings.SocialNetworkSettings.TwitterSettings;

				Plugin.Call(Native.Methods.INITIALIZE, _twitterSettings.ConsumerKey, _twitterSettings.ConsumerSecret);
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
				Plugin.Call(Native.Methods.LOGIN);
			}
		}
		
		public override void Logout ()
		{
			// Native method is called
			Plugin.Call(Native.Methods.LOGOUT);
		}
		
		public override bool IsLoggedIn ()
		{
			bool _isLoggedIn	= Plugin.Call<bool>(Native.Methods.IS_LOGGED_IN);
			Console.Log(Constants.kDebugTag, "[Twitter] IsLoggedIn=" + _isLoggedIn);
			
			return _isLoggedIn;
		}
		
		public override string GetAuthToken ()
		{
			string _authToken	= Plugin.Call<string>(Native.Methods.GET_AUTH_TOKEN);
			Console.Log(Constants.kDebugTag, "[Twitter] AuthToken=" + _authToken);
			
			return _authToken;
		}
		
		public override string GetAuthTokenSecret ()
		{
			string _authTokenSecret	= Plugin.Call<string>(Native.Methods.GET_AUTH_TOKEN_SECRET);
			Console.Log(Constants.kDebugTag, "[Twitter] AuthTokenSecret=" + _authTokenSecret);
			
			return _authTokenSecret;
		}
		
		public override string GetUserID ()
		{
			string _userID	= Plugin.Call<string>(Native.Methods.GET_USER_ID);
			Console.Log(Constants.kDebugTag, "[Twitter] UserID=" + _userID);
			
			return _userID;
		}
		
		public override string GetUserName ()
		{
			string _userName	= Plugin.Call<string>(Native.Methods.GET_USER_NAME);
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
			Plugin.Call(Native.Methods.SHOW_TWEET_COMPOSER, _message, _URL, _imgByteArray, _arrayLength);
		}
		
		#endregion
		
		#region Request API's
		
		public override void RequestAccountDetails (TWTRAccountDetailsCompletion _onCompletion)
		{
			base.RequestAccountDetails(_onCompletion);

			// Native method is called
			Plugin.Call(Native.Methods.REQUEST_ACCOUNT_DETAILS);
		}
		
		public override void RequestEmailAccess (TWTREmailAccessCompletion _onCompletion)
		{
			base.RequestEmailAccess(_onCompletion);

			// Native method is called
			Plugin.Call(Native.Methods.REQUEST_EMAIL_ACCESS);
		}
		
		protected override void URLRequest (string _methodType, string _URL, IDictionary _parameters, TWTRResonse _onCompletion)
		{
			base.URLRequest(_methodType, _URL, _parameters, _onCompletion);

			// Native method is called
			Plugin.Call(Native.Methods.URL_REQUEST, _methodType, _URL, _parameters.ToJSON());
		}
		
		#endregion
	}
}
#endif