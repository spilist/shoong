using UnityEngine;
using System.Collections;

#if USES_WEBVIEW && UNITY_ANDROID
namespace VoxelBusters.NativePlugins.Internal
{
	public partial class WebViewNativeAndroid : WebViewNative
	{
		#region Parse Constants

		private const string	kTag			= "tag";
		private const string	kError			= "error";
		private const string	kResult			= "result";
		private const string	kMessageData	= "message-data";
					
		#endregion

		#region Parse Methods
		
		protected override void ParseLoadErrorData (IDictionary _dataDict, out string _tag, out string _error)
		{
			_tag	= _dataDict[kTag] as string;
			_error	= _dataDict[kError] as string;
		}
		
		protected override void ParseEvalJSData (IDictionary _resultData, out string _tag, out string _result)
		{
			_tag	= _resultData[kTag] as string;
			_result	= _resultData[kResult] as string;
		}

		protected override void ParseMessageData (IDictionary _dataDict, out string _tag, out WebViewMessage _message)
		{
			_tag		= _dataDict[kTag] as string;
			_message	= new AndroidWebViewMessage(_dataDict[kMessageData] as IDictionary);
		}
	
		#endregion
	}
}
#endif