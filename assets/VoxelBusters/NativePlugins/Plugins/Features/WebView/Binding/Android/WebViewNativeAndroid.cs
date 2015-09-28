﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.DebugPRO;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	public partial class WebViewNativeAndroid : WebViewNative
	{
		#region Key Constants //The same keys are used  by Native code as well

		#endregion
		
		#region Platform Native Info
		
		private class NativeInfo
		{
			// Handler class name
			public class Class
			{
				public const string NAME									= "com.voxelbusters.nativeplugins.features.webview.WebViewHandler";
			}
			
			// For holding method names
			public class Methods
			{
				public const string CREATE_WEB_VIEW		 					= "createNativeWebView";
				public const string DESTROY_WEB_VIEW		 				= "destoryWebViewWithTag";
				public const string SHOW_WEB_VIEW		 					= "showWebViewWithTag";
				public const string HIDE_WEB_VIEW 							= "hideWebViewWithTag";
				public const string LOAD_REQUEST 							= "loadRequest";
				public const string LOAD_HTML_STRING 						= "loadHTMLString";
				public const string LOAD_DATA 								= "loadData";
				public const string EVALUATE_JS_FROM_STRING 				= "evaluateJavaScriptFromString";
				public const string RELOAD_WEB_VIEW 						= "reloadWebViewWithTag";
				public const string STOP_LOADING_WEB_VIEW 					= "stopLoadingWebViewWithTag";
				public const string SET_CAN_HIDE							= "setCanHide";
				public const string SET_CAN_BOUNCE							= "setCanBounce";
				public const string SET_CONTROL_TYPE						= "setControlType";
				public const string SET_SHOW_TOOLBAR 						= "setShowToolBar";
				public const string SET_SHOW_LOADING_SPINNER 				= "setShowLoadingSpinner";
				public const string SET_AUTO_SHOW_WHEN_LOAD_COMPLETE 		= "setAutoShowWhenLoadComplete";
				public const string SET_SCALES_PAGE_TO_FIT 					= "setScalesPageToFit";
				public const string SET_FRAME 								= "setFrame";
				public const string SET_BACKGROUND_COLOR 					= "setBackgroundColor";
				public const string SET_ALLOW_MEDIA_PLAYBACK 				= "setAllowMediaPlayback";
				public const string ADD_NEW_SCHEME 							= "addNewScheme";
				public const string CLEAR_CACHE 							= "clearCache";
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
					Console.LogError(Constants.kDebugTag, "[WebView] Plugin class not intialized!");
				}
				return m_plugin; 
			}
			
			set
			{
				m_plugin = value;
			}
		}
		
		private Dictionary<eWebviewControlType,string> m_controlTypes = null;

		private Dictionary<eWebviewControlType,string>  ControlTypes
		{
			get 
			{ 
				if(m_controlTypes == null)
				{
					m_controlTypes = new Dictionary<eWebviewControlType,string>();
					m_controlTypes.Add(eWebviewControlType.NO_CONTROLS, 		"NONE");
					m_controlTypes.Add(eWebviewControlType.CLOSE_BUTTON, 		"CLOSE_OPTION");
					m_controlTypes.Add(eWebviewControlType.TOOLBAR, 			"TOOLBAR_OPTION");
				}
				return m_controlTypes; 
			}
		}

		#endregion
		
		#region Constructors
		
		WebViewNativeAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}
		
		#endregion

		#region Lifecycle API's
		
		public override void Create (WebView _webview, Rect _frame)
		{
			base.Create(_webview, _frame);
			
			// Create native instance
			Plugin.Call(NativeInfo.Methods.CREATE_WEB_VIEW, _webview.UniqueID, _frame.x/Screen.width, _frame.y/Screen.height, _frame.width/Screen.width, _frame.height/Screen.height);
		}
		
		public override void Destroy (WebView _webview)
		{
			base.Destroy(_webview);

			// Native call
			Plugin.Call(NativeInfo.Methods.DESTROY_WEB_VIEW,_webview.UniqueID);
		}
		
		public override void Show (WebView _webview)
		{
			base.Show(_webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SHOW_WEB_VIEW,_webview.UniqueID);
		}
		
		public override void Hide (WebView _webview)
		{
			base.Hide(_webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.HIDE_WEB_VIEW,_webview.UniqueID);
		}
		
		#endregion
		
		#region Load API's
		
		public override void LoadRequest (string _URL, WebView _webview)
		{
			base.LoadRequest(_URL, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.LOAD_REQUEST, _URL, _webview.UniqueID);
		}
		
		public override void LoadHTMLString (string _HTMLString, string _baseURL, WebView _webview)
		{
			base.LoadHTMLString(_HTMLString, _baseURL, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.LOAD_HTML_STRING, _HTMLString, _baseURL, _webview.UniqueID);
		}
		
		public override void LoadData (byte[] _byteArray, string _MIMEType, string _textEncodingName, string _baseURL, WebView _webview)
		{
			base.LoadData(_byteArray, _MIMEType, _textEncodingName, _baseURL, _webview);
			
			// Native call
			if (_byteArray != null)
			{
				Plugin.Call(NativeInfo.Methods.LOAD_DATA, _byteArray, _byteArray.Length, _MIMEType, _textEncodingName, _baseURL,	_webview.UniqueID);
			}
		}
		
		public override void EvaluateJavaScriptFromString (string _javaScript, WebView _webview)
		{			
			base.EvaluateJavaScriptFromString (_javaScript, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.EVALUATE_JS_FROM_STRING, _javaScript, _webview.UniqueID);
		}
		
		public override void Reload (WebView _webview)
		{
			base.Reload(_webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.RELOAD_WEB_VIEW, _webview.UniqueID);
		}
		
		public override void StopLoading (WebView _webview)
		{
			base.StopLoading(_webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.STOP_LOADING_WEB_VIEW, _webview.UniqueID);
		}
		
		#endregion

		#region Property Access API's
		
		public override void SetCanHide (bool _canHide, WebView _webview)
		{
			base.SetCanHide(_canHide, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_CAN_HIDE, _canHide, _webview.UniqueID);
		}
		
		public override void SetCanBounce (bool _canBounce, WebView _webview)
		{
			base.SetCanBounce(_canBounce, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_CAN_BOUNCE, _canBounce, _webview.UniqueID);
		}
		
		public override void SetControlType (eWebviewControlType _type, WebView _webview) 
		{
			base.SetControlType(_type, _webview);

			// Native call
			Plugin.Call(NativeInfo.Methods.SET_CONTROL_TYPE, ControlTypes[_type], _webview.UniqueID);
		}
		
		public override void SetShowSpinnerOnLoad (bool _showSpinner, WebView _webview)
		{
			base.SetShowSpinnerOnLoad(_showSpinner, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_SHOW_LOADING_SPINNER, _showSpinner, _webview.UniqueID);
		}

		public override void SetAutoShowOnLoadFinish (bool _autoShow, WebView _webview)
		{
			base.SetAutoShowOnLoadFinish(_autoShow, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_AUTO_SHOW_WHEN_LOAD_COMPLETE, _autoShow, _webview.UniqueID);
		}
		
		public override void SetScalesPageToFit (bool _scaleToFit, WebView _webview)
		{			
			base.SetScalesPageToFit(_scaleToFit, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_SCALES_PAGE_TO_FIT, _scaleToFit, _webview.UniqueID);	
		}
		
		public override void SetFrame (Rect _frame, WebView _webview)
		{
			base.SetFrame(_frame, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_FRAME, _frame.x/Screen.width, _frame.y/Screen.height, _frame.width/Screen.width, _frame.height/Screen.height , _webview.UniqueID);
		}

		public override void SetBackgroundColor (Color _color, WebView _webview)
		{
			base.SetBackgroundColor(_color, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.SET_BACKGROUND_COLOR, _color.r, _color.g, _color.b, _color.a, _webview.UniqueID);
		}
		
		#endregion
		
		#region URL Scheme
		
		public override void AddNewURLSchemeName (string _newURLSchemeName, WebView _webview)
		{
			base.AddNewURLSchemeName(_newURLSchemeName, _webview);
			
			// Native call
			Plugin.Call(NativeInfo.Methods.ADD_NEW_SCHEME, _newURLSchemeName, _webview.UniqueID);
		}
		
		#endregion
		
		#region Cache Clearance API's
		
		public override void ClearCache ()
		{
			base.ClearCache();
			
			// Native call
			Plugin.Call(NativeInfo.Methods.CLEAR_CACHE);
		}
		
		#endregion
	}
}
#endif