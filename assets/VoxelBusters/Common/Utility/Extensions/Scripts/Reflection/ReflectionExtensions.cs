﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.Utility
{
	public static class ReflectionExtensions 
	{
		#region Field Extensions

		public static object GetStaticValue (this Type _objectType, string _name)
		{
			BindingFlags 	_bindingAttr	= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo		_fieldInfo		= _objectType.GetField(_name, _bindingAttr);
			
			if (_fieldInfo != null)
			{
				return _fieldInfo.GetValue(null);
			}
			else if (_objectType.BaseType != null)
			{
				return GetStaticValue(_objectType.BaseType, _name);
			}

			throw new MissingFieldException(string.Format("[RS] Field {0} not found.", _name));
		}

		public static void SetStaticValue (this Type _objectType, string _name, object _value)
		{
			BindingFlags 	_bindingAttr	= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo		_fieldInfo		= _objectType.GetField(_name, _bindingAttr);

			if (_fieldInfo != null)
			{
				_fieldInfo.SetValue(null, _value);
			}
			else if (_objectType.BaseType != null)
			{
				SetStaticValue(_objectType.BaseType, _name, _value);
			}
		}

		#endregion

		#region Method Extensions

		public static void InvokeMethod (this object _object, string _method, object _value = null)
		{
			if (_object == null)
				throw new NullReferenceException("Target Object is null.");

			Type			_objectType		= _object.GetType();
			object[] 		_argValueList	= null;
			Type[]			_argTypeList	= null;

			if (_value != null)
			{
				_argValueList				= new object[] { _value };
				_argTypeList				= new Type[] { _value.GetType() };
			}

			// Invoke method
			InvokeMethod(_object, _objectType, _method, _argValueList, _argTypeList);
		}

		public static void InvokeMethod (this object _object, string _method, object[] _argValues, Type[] _argTypes)
		{
			InvokeMethod(_object, _object.GetType(), _method, _argValues, _argTypes);
		}

		private static void InvokeMethod (object _object, Type _objectType, string _method, object[] _argValueList, Type[] _argTypeList)
		{
			BindingFlags 	_bindingAttr	= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.OptionalParamBinding;
			MethodInfo 		_methodInfo		= null;

			// Find method info based on method name, parameter
			if (_argValueList != null)
				_methodInfo 				= _objectType.GetMethod(_method, _bindingAttr, null, _argTypeList, null);
			else
				_methodInfo					= _objectType.GetMethod(_method, _bindingAttr);

			// Invoke the method
			if (_methodInfo != null)
			{
				_methodInfo.Invoke(_object, _argValueList);
			}
			// Failed to find a matching method, so search for it in base class
			else if (_objectType.BaseType != null)
			{
				InvokeMethod(_object, _objectType.BaseType, _method, _argValueList, _argTypeList);
			}
			// Object doesnt have this method
			else
			{
				throw new MissingMethodException();
			}
		}

		#endregion

		public static T GetAttribute <T> (this System.Type _type, bool _inherit) where T : System.Attribute
		{
			object[] _attributes	= _type.GetCustomAttributes(_inherit);
			
			for (int _iter = 0; _iter < _attributes.Length; _iter++)
			{
				if ((_attributes[_iter] as T) != null)
					return (T)_attributes[_iter];
			}
			
			return null;
		}

		public static T GetAttribute <T> (this FieldInfo _fieldInfo, bool _inherit) where T : System.Attribute
		{
			object[] _attributes	= _fieldInfo.GetCustomAttributes(_inherit);
			
			for (int _iter = 0; _iter < _attributes.Length; _iter++)
			{
				if ((_attributes[_iter] as T) != null)
					return (T)_attributes[_iter];
			}
			
			return null;
		}

		public static FieldInfo GetFieldWithName (this System.Type _type, string _name, bool _isPublic)
		{
			BindingFlags _bindingAttr	= BindingFlags.Instance;

			if (_isPublic)
				_bindingAttr	|= BindingFlags.Public;
			else
				_bindingAttr	|= BindingFlags.NonPublic;

			FieldInfo _field	= _type.GetField(_name, _bindingAttr);

			// Lets search in base class
			if (_field == null)
			{
				System.Type _baseType	= _type.BaseType;

				if (_baseType != null)
					return _baseType.GetFieldWithName(_name, _isPublic);
			}

			return _field;
		}

#if UNITY_EDITOR

		public static T GetAttribute <T> (this SerializedProperty _property) where T : System.Attribute
		{
			return _property.propertyType.GetType().GetAttribute<T>(true);
		}

		public static bool IsArrayElement (this SerializedProperty _serializableProperty)
		{
			return (_serializableProperty.propertyPath.IndexOf(".Array.data") != -1);
		}

		public static string GetDisplayName (this SerializedProperty _originalProperty)
		{
			if (!_originalProperty.hasVisibleChildren)
				return ObjectNames.NicifyVariableName(_originalProperty.name);

			string _displayName		= null;

#if UNITY_4_6 || UNITY_5 || UNITY_6 || UNITY_7

			_displayName	= _originalProperty.displayName;

#else
			if (_originalProperty.IsArrayElement())
			{
				SerializedProperty _propertyCopy	= _originalProperty.Copy(); 
				string _parentPropertyPath			= _originalProperty.propertyPath;

				// Move to next visible child property, which isnt expandable
				bool _failedToGetDisplayName	= false;

				while (true)
				{
					bool _movedToNextProperty	=	_propertyCopy.NextVisible(true);

					if (!_movedToNextProperty)
					{
						_failedToGetDisplayName	= true;
						break;
					}

					// We should make sure iteration doesnt move to next property which is outside parent property
					if (!_propertyCopy.propertyPath.Contains(_parentPropertyPath))
					{
						_failedToGetDisplayName	= true;
						break;
					}

					// We will consider first visible string property as display name
					if (_propertyCopy.propertyType == SerializedPropertyType.String)
					{
						break;
					}
				}

				// We failed to find proper display name, so lets use original name
				if (_failedToGetDisplayName)
				{
					_displayName			= _originalProperty.name; 
				}
				else
				{
					string _newDisplayName	= _propertyCopy.stringValue; 

					if (string.IsNullOrEmpty(_newDisplayName))
						_displayName	= "Element " + Regex.Match(_originalProperty.propertyPath, "(?<=data\\[)\\d+").Value;
					else
						_displayName	= _newDisplayName;
				}
			}
			else
			{
				_displayName	= _originalProperty.name; 
			}
#endif

			return ObjectNames.NicifyVariableName(_displayName);
		}

		public static FieldInfo[] GetFields (this SerializedObject _serializedObject)
		{
			object	_targetObject	= _serializedObject.targetObject;
			Type	_objectType		= _targetObject.GetType();

			return _objectType.GetAllFields();
		}

		public static FieldInfo[] GetFields (this SerializedProperty _serializableProperty)
		{
			object _serObject		= _serializableProperty.serializedObject.targetObject;
			string _propertyPath	= _serializableProperty.propertyPath;
			int _arrayDataIndex		= _propertyPath.IndexOf(".Array.data");

			// Its an array element, for each element of array "propertyPath" has index information, need to remove it
			if (_arrayDataIndex != -1)
			{
				_propertyPath		= _propertyPath.Substring(0, _arrayDataIndex);
			}

			// Path is split into components
			string[] _propertyPathComponents	= _propertyPath.Split('.');
			BindingFlags _bindingAttribute		= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

			// We are interating though property path, to get typeof property
			int _componentCount					= _propertyPathComponents.Length;
			System.Type _containerObjectType	= _serObject.GetType();

			for (int _iter = 0; _iter < _componentCount; _iter++)
			{
				// Each component of property path is a variable of parent object
				string _fieldName		= _propertyPathComponents[_iter];
				FieldInfo _targetField	= _containerObjectType.GetField(_fieldName, _bindingAttribute);

				// As we go deeper, update container object type
				_containerObjectType	= _targetField.FieldType;
			}

			// If its an array element, our parent is currently pointing at array rather than element as we chucked index information
			if (_containerObjectType.IsArray) 
			{
				_containerObjectType	= _containerObjectType.GetElementType();	
			}
			else if (_containerObjectType.IsGenericType && _containerObjectType.GetGenericTypeDefinition() == typeof(List<>))
			{
				_containerObjectType	= _containerObjectType.GetGenericArguments()[0];
			}

			return _containerObjectType.GetAllFields();
		}

		public static List<SerializedProperty> GetSerializableProperties (this SerializedObject _serializedObject)
		{
			List<SerializedProperty> _serializableProperties	= new List<SerializedProperty>();
			FieldInfo[] _definedFields							= _serializedObject.GetFields();
			int _definedFieldsCount								= _definedFields.Length;
			
			for (int _iter = 0; _iter < _definedFieldsCount; _iter++)
			{
				SerializedProperty _serializableProperty	= _serializedObject.FindProperty(_definedFields[_iter].Name);
				
				if (_serializableProperty != null)
					_serializableProperties.Add(_serializableProperty);
			}
			
			return _serializableProperties;
		} 

		public static List<SerializedProperty> GetSerializableChildProperties (this SerializedProperty _serializableProperty)
		{
			List<SerializedProperty> _serializableProperties	= new List<SerializedProperty>();
			FieldInfo[] _definedFields							= _serializableProperty.GetFields();
			int _definedFieldsCount								= _definedFields.Length;

			for (int _iter = 0; _iter < _definedFieldsCount; _iter++)
			{
				SerializedProperty _serializableChildProperty	= _serializableProperty.FindPropertyRelative(_definedFields[_iter].Name);

				if (_serializableChildProperty != null)
					_serializableProperties.Add(_serializableChildProperty);
			}

			return _serializableProperties;
		}

		public static object GetDirectParent(this SerializedProperty _property)
		{
			string 		_propertyPath 				= 	_property.propertyPath.Replace(".Array.data",""); // ex: targetVariable.internalVariable.{...}.propertyVariable
			string[] 	_propertyPathComponents 	= 	_propertyPath.Split('.');
			
			object 		_rootInstance				=	_property.serializedObject.targetObject;
			
			//Now iterate through the path variables starting from _rootInstance variables.
			object _targetObject = _rootInstance;
			
			BindingFlags _bindingFlags		= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			
			//Iterate through the path and update targetObject
			for(int _index = 0 ; _index < (_propertyPathComponents.Length - 1) ; _index++)
			{
				
				string _currentPathComponent = _propertyPathComponents[_index];
			
				FieldInfo _requiredFieldInfo;

				//If its indexed field
				if(_currentPathComponent.Contains("["))
				{
					int _arrayIndex = int.Parse(_currentPathComponent.StringBetween("[","]",true));
					
					System.Array _array = (System.Array)_targetObject;
					_targetObject = _array.GetValue(_arrayIndex);
				}
				else
				{
					_requiredFieldInfo	= _targetObject.GetType().GetField(_currentPathComponent, _bindingFlags);
					
					//Save back the field in current _targetObject to _targetObject.
					_targetObject = _requiredFieldInfo.GetValue(_targetObject);
				}
				
			}
			
			return _targetObject;
		}

		private static FieldInfo[] GetAllFields (this Type _objectType)
		{
			BindingFlags 	_bindingAttribute	= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			List<FieldInfo>	_objectFields		= new List<FieldInfo>();
			
			for (Type _curType = _objectType; _curType != null; _curType =  _curType.BaseType)
			{
				_objectFields.AddRange(_curType.GetFields(_bindingAttribute));
			}
			
			return _objectFields.ToArray();
		}
#endif
	}
}