﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.AssetStoreProductUtility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[CustomEditor(typeof(NPSettings))]
	public class NPSettingsInspector : AssetStoreProductInspector
	{
		private enum eTabView
		{
			NONE,
			APPLICATION_SETTINGS,
			BILLING_SETTINGS,
			GAME_SERVICES_SETTINGS,
			MEDIA_LIBRARY_SETTINGS,
			NETWORK_CONNECTVITY_SETTINGS,
			NOTIFICATION_SERVICE_SETTINGS,
			SOCIAL_NETWORK_SETTINGS,
			UTILITY_SETTINGS,
		}

		#region Constants

		private		const 	string					kActiveView				= "np-active-view";

		// URL
		private		const 	string					kFullVersionProductURL	= "http://bit.ly/1Fnpb5j";
		private		const 	string					kLiteVersionProductURL	= "http://bit.ly/1KFEzdi";
		private		const 	string					kTutorialURL			= "http://bit.ly/1ZFadk4";
		private		const	string					kDocumentationURL		= "http://bit.ly/1cBFHDd";

		// Keys
		private		const	string					kUndoGroupApplicationSettings	= "application-settings";

		#endregion

		#region Properties

		// Toolbar tabs
		private 			eTabView				m_activeView;
		private				Dictionary<eTabView, SerializedProperty>	m_settingsCollection	= new Dictionary<eTabView, SerializedProperty>();
		private				Vector2					m_scrollPosition		= Vector2.zero;

		// GUI contents
#pragma warning disable
		private 			GUIContent				m_documentationText		= new GUIContent("Documentation", "Access complete API documentation for this plugin.");
		private 			GUIContent				m_saveChangesText		= new GUIContent("Save", "Save all your settings changes with the click of this button.");
		private 			GUIContent				m_supportText			= new GUIContent("Support", "Got a problem! Shoot a mail and let us know about it.");
		private 			GUIContent				m_tutotialsText			= new GUIContent("Tutorials", "Not sure how plugin works? No problem! Please have a look into our tutorial posts.");
		private 			GUIContent				m_writeReviewText		= new GUIContent("Write a review", "Review everything about our product from whats best to whats worse. Your reviews will help others to make a better decision.");
		private 			GUIContent				m_upgradeText			= new GUIContent("Upgrade", "For advanced features like IAP/Billing, Notifications, WebView, Game Servies etc you need to upgrade to our full version product.");
#pragma warning restore

		// GUI styles
		private		const	string					kButtonLeftStyle		= "ButtonLeft";
		private		const	string					kButtonMidStyle			= "ButtonMid";
		private		const	string					kButtonRightStyle		= "ButtonRight";

		#endregion

		#region Methods

		private void OnInspectorUpdate () 
		{
			// Call Repaint on OnInspectorUpdate as it repaints the windows
			// less times as if it was OnGUI/Update
			Repaint();
		}

		protected override void OnEnable ()
		{
			base.OnEnable();

			// Initialise 
			m_settingsCollection.Add(eTabView.APPLICATION_SETTINGS,			serializedObject.FindProperty("m_applicationSettings"));
			m_settingsCollection.Add(eTabView.BILLING_SETTINGS,				serializedObject.FindProperty("m_billingSettings"));
			m_settingsCollection.Add(eTabView.MEDIA_LIBRARY_SETTINGS,		serializedObject.FindProperty("m_mediaLibrarySettings"));
			m_settingsCollection.Add(eTabView.GAME_SERVICES_SETTINGS,		serializedObject.FindProperty("m_gameServicesSettings"));
			m_settingsCollection.Add(eTabView.NETWORK_CONNECTVITY_SETTINGS,	serializedObject.FindProperty("m_networkConnectivitySettings"));
			m_settingsCollection.Add(eTabView.NOTIFICATION_SERVICE_SETTINGS,serializedObject.FindProperty("m_notificationSettings"));
			m_settingsCollection.Add(eTabView.SOCIAL_NETWORK_SETTINGS,		serializedObject.FindProperty("m_socialNetworkSettings"));
			m_settingsCollection.Add(eTabView.UTILITY_SETTINGS,				serializedObject.FindProperty("m_utilitySettings"));

			// Restoring last selection
			m_activeView	= (eTabView)EditorPrefs.GetInt(kActiveView, 0);
		}

		protected override void OnDisable ()
		{
			base.OnDisable();

			// Save changes to settings
			EditorPrefs.SetInt(kActiveView, (int)m_activeView);	
		}

		protected override void OnGUIWindow ()
		{
			// Disable GUI when its compiling
			GUI.enabled			= !EditorApplication.isCompiling;

			// Drawing tabs
			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{	
				base.OnGUIWindow();

				UnityEditorUtility.DrawSplitter(new Color(0.35f, 0.35f, 0.35f), 1, 10);
				DrawTopBarButtons();

				GUILayout.Space(10f);
				GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
				{
					GUILayout.Space(2f);
					m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);
					{
						DrawTabViews();
					}
					GUILayout.EndScrollView();
					GUILayout.Space(2f);
				}
				GUILayout.EndVertical();

				GUILayout.Space(10f);
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();

					// Change button color, as a feedback to user activity
					Color _GUIColorOld 	= GUI.color;
					GUI.color			= EditorPrefs.GetBool(NPSettings.kPrefsKeyPropertyModified) ? Color.red : Color.green;

					if (GUILayout.Button(m_saveChangesText, GUILayout.MinWidth(120)))
						OnPressingSave();

					// Reset back to old state
					GUI.color 			= _GUIColorOld;

					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();

			// Reset GUI state
			GUI.enabled			= true;
		}

		#endregion

		#region Misc. Methods

		private void DrawTopBarButtons ()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(m_documentationText, kButtonMidStyle))
					Application.OpenURL(kDocumentationURL);
				
				if (GUILayout.Button(m_tutotialsText, kButtonMidStyle))
					Application.OpenURL(kTutorialURL);
				
				if (GUILayout.Button(m_supportText, kButtonMidStyle))
					OnPressingSupport();
				
#if NATIVE_PLUGINS_LITE_VERSION
				if (GUILayout.Button(m_upgradeText, kButtonMidStyle))
					Application.OpenURL(kFullVersionProductURL);
#endif
				
				if (GUILayout.Button(m_writeReviewText, kButtonRightStyle))
					OnPressingWriteReview();
				
				GUILayout.Space(10f);
			}
			GUILayout.EndHorizontal();
		}

		private void DrawTabViews ()
		{
			// Draw settings tab view
			Dictionary<eTabView, SerializedProperty>.Enumerator _enumerator	= m_settingsCollection.GetEnumerator();
			
			while (_enumerator.MoveNext())
			{
				eTabView			_curTabView		= _enumerator.Current.Key;
				SerializedProperty	_curProperty	= _enumerator.Current.Value;
				
				if (DrawSerializedProperty(_curProperty))
				{
					// Minimize old selection
					if (m_activeView != eTabView.NONE)
					{
						SerializedProperty _curActiveProperty	= m_settingsCollection[m_activeView];

						if (_curActiveProperty != null)
							_curActiveProperty.isExpanded		= false;
					}
					
					// Update current active view
					if (_curProperty.isExpanded)
						m_activeView	= _curTabView;
					else
						m_activeView	= eTabView.NONE;
				}
			}
		}

		private bool DrawSerializedProperty (SerializedProperty _property)
		{
			if (_property == null)
				return false;

			// Draw header
			bool	_isSelected		= UnityEditorUtility.DrawPropertyHeader(_property);

			// Draw immediate childrens
			if (_property.hasVisibleChildren && _property.isExpanded)
			{
				SerializedProperty	_propertyCopy	= _property.Copy();
				SerializedProperty 	_endProperty	= _property.GetEndProperty();

				// Move to immediate child property
				_propertyCopy.NextVisible(true);
				
				GUILayout.Space(-4f);
				GUILayout.BeginHorizontal("HelpBox");
				{
					GUILayout.Space(8f);
					GUILayout.BeginVertical();
					{
						do
						{
							if (SerializedProperty.EqualContents(_propertyCopy, _endProperty))
								break;

							// Lets make all properties expanded
							_propertyCopy.isExpanded	= true;
							
							EditorGUILayout.PropertyField(_propertyCopy, true);
						}while (_propertyCopy.NextVisible(false));
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
			}

			return _isSelected;
		}

		private void OnPressingSave ()
		{
			// Save changes
			(target as NPSettings).SaveConfigurationChanges();
		}

		private void OnPressingSupport ()
		{
			string	_mailToAddress	= WWW.EscapeURL("support@voxelbusters.com").Replace("+","%20");
			
			string	_mailToSubject	= null;

#if NATIVE_PLUGINS_LITE_VERSION
			_mailToSubject			= WWW.EscapeURL("[Cross Platform Native Plugins - LITE] : [Feature : ___________]").Replace("+","%20");;
#else
			_mailToSubject			= WWW.EscapeURL("[Cross Platform Native Plugins] : [Feature : ___________]").Replace("+","%20");;
#endif

			string	_mailBody		= WWW.EscapeURL("---------------------------------------------------------------------------- \n");

			_mailBody			   += WWW.EscapeURL(string.Format("Invoice Number	: \n"));
			_mailBody			   += WWW.EscapeURL(string.Format("Unity			: {0} \n", Application.unityVersion));
			_mailBody			   += WWW.EscapeURL(string.Format("Stripping: 		: {0} \n", UnityEditor.PlayerSettings.strippingLevel));
			_mailBody			   += WWW.EscapeURL(string.Format("API				: {0} \n", UnityEditor.PlayerSettings.apiCompatibilityLevel));
			_mailBody			   += WWW.EscapeURL(string.Format("Target			: {0} \n", EditorUserBuildSettings.activeBuildTarget));
			_mailBody			   += WWW.EscapeURL(string.Format("Platform			: {0} \n", Application.platform));
			
			_mailBody			   += WWW.EscapeURL("---------------------------------------------------------------------------- \n");
			_mailBody				= _mailBody.Replace("+","%20");

			string	_mailToString	= string.Format("mailto:{0}?subject={1}&body={2}", _mailToAddress, _mailToSubject, _mailBody);
			
			// Opens mail client
			Application.OpenURL(_mailToString);
		}

		private void OnPressingWriteReview ()
		{
			string	_assetPageURL	= null;

#if NATIVE_PLUGINS_LITE_VERSION
			_assetPageURL			= kLiteVersionProductURL;
#else
			_assetPageURL			= kFullVersionProductURL;
#endif

			Application.OpenURL(_assetPageURL);
		}
			   
		#endregion
	}
}