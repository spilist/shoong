using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class MediaLibraryAndroid : MediaLibrary
	{
		#region Platform Native Info
		
		class NativeInfo
		{
			// Handler class name
			public class Class
			{
				public const string NAME						= "com.voxelbusters.nativeplugins.features.medialibrary.MediaLibraryHandler";
			}
			
			// For holding method names
			public class Methods
			{
				public const string INITIALIZE		 			= "initialize";
				public const string IS_CAMERA_SUPPORTED		 	= "isCameraSupported";
				public const string PICK_IMAGE				 	= "showImagePicker";
				public const string SAVE_IMAGE_TO_GALLERY	 	= "saveImageToAlbum";
				public const string PLAY_VIDEO_FROM_GALLERY		= "playVideoFromGallery";
				public const string PLAY_VIDEO_FROM_URL		 	= "playVideoFromURL";
				public const string PLAY_VIDEO_FROM_YOUTUBE		= "playVideoFromYoutube";
				public const string PLAY_VIDEO_FROM_WEBVIEW		= "playVideoFromWebView";
	
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
					Console.LogError(Constants.kDebugTag, "[MediaLibrary] Plugin class not intialized!");
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
		
		MediaLibraryAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);

			Plugin.Call(NativeInfo.Methods.INITIALIZE, NPSettings.MediaLibrary.Android.YoutubeAPIKey);		
		}

		#endregion

		#region overridden methods

		public override bool IsCameraSupported ()
		{
			bool _isSupported	= Plugin.Call<bool>(NativeInfo.Methods.IS_CAMERA_SUPPORTED);
			Console.Log(Constants.kDebugTag, "[MediaLibrary] IsCameraSupported=" + _isSupported);
			
			return _isSupported;
		}
		
		public override void PickImage (eImageSource _source, 	float _scaleFactor, 	
		                                PickImageCompletion _onCompletion)
		{
			base.PickImage(_source, _scaleFactor, _onCompletion);
			
			if (_scaleFactor > 0f)
				Plugin.Call(NativeInfo.Methods.PICK_IMAGE, (int)_source, _scaleFactor);
		}
		
		public override void SaveImageToGallery (byte[] _imageByteArray, SaveImageToGalleryCompletion _onCompletion)
		{			
			base.SaveImageToGallery(_imageByteArray, _onCompletion);
			
			if (_imageByteArray != null)
				Plugin.Call(NativeInfo.Methods.SAVE_IMAGE_TO_GALLERY, _imageByteArray, _imageByteArray.Length);
		}


		public override void PlayVideoFromGallery (PickVideoCompletion _onPickVideoCompletion,
		                                          PlayVideoCompletion _onPlayVideoCompletion)
		{
			base.PlayVideoFromGallery(_onPickVideoCompletion, _onPlayVideoCompletion);

			Plugin.Call(NativeInfo.Methods.PLAY_VIDEO_FROM_GALLERY);
		}

		public override void PlayVideoFromURL (URL _URL, 
		                                      PlayVideoCompletion _onCompletion)
		{
			base.PlayVideoFromURL(_URL, _onCompletion);
			
			if (!string.IsNullOrEmpty(_URL.URLString))
			{
				string _youtubeID = ExtractYoutubeVideoID(_URL.URLString);
				
				if(_youtubeID != null)
				{
					PlayYoutubeVideo(_youtubeID, _onCompletion);
				}
				else
				{
					Plugin.Call(NativeInfo.Methods.PLAY_VIDEO_FROM_URL, _URL.URLString);
				}
			}
		}

		public override void PlayYoutubeVideo (string _videoID, PlayVideoCompletion _onCompletion)
		{
			if (string.IsNullOrEmpty(NPSettings.MediaLibrary.Android.YoutubeAPIKey))
			{
				base.PlayYoutubeVideo(_videoID, _onCompletion);
			}
			else
			{
				Plugin.Call(NativeInfo.Methods.PLAY_VIDEO_FROM_YOUTUBE, _videoID);
			}
		}	

		public override void PlayEmbeddedVideo (string _embedHTMLString, PlayVideoCompletion _onCompletion)
		{
			base.PlayEmbeddedVideo(_embedHTMLString, _onCompletion);
			
			if (!string.IsNullOrEmpty(_embedHTMLString))
				Plugin.Call(NativeInfo.Methods.PLAY_VIDEO_FROM_WEBVIEW, _embedHTMLString);
		}

		#endregion
	}
}
#endif	