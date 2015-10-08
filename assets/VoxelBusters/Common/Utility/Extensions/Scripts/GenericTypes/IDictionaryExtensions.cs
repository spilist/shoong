﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace VoxelBusters.Utility
{
	public static class IDictionaryExtensions 
	{
		public static T GetIfAvailable<T>(this IDictionary _dictionary, string _key)
		{
			Type _type	= typeof(T);
			
			if(!string.IsNullOrEmpty(_key))
			{	
				if (_dictionary.Contains(_key))
				{
					if (_type.IsEnum)
					{
						return (T)Enum.ToObject(_type, _dictionary[_key]);
					}
					else
					{
						return (T)System.Convert.ChangeType(_dictionary[_key], _type);
					}
				}
			}
			
			return default(T);
		}

		public static T GetIfAvailable<T>(this IDictionary _sourceDictionary, string _key, string _path)
		{
			//Trim path at start
			if(_path != null)
			{
				//Trim start and end slash if exists.
				_path = _path.TrimStart('/').TrimEnd('/');
			}

			if(!string.IsNullOrEmpty(_key))
			{

				if(string.IsNullOrEmpty(_path))
				{
					return _sourceDictionary.GetIfAvailable<T>(_key);
				}
				else
				{
					string[] _pathComponents = _path.Split('/');

					IDictionary _currentDict = _sourceDictionary;

					//Here traverse to the path
					foreach(string _each in _pathComponents)
					{
						if(_currentDict.Contains(_each))
						{
							_currentDict = _currentDict[_each] as IDictionary;
						}
						else
						{
							Debug.LogError("Path not found " + _path);
							return default(T);
						}
					}
					
					return _currentDict.GetIfAvailable<T>(_key);
				}
			}
			else
			{
				return default(T);
			}
		}

		public static string GetKey<T>(this IDictionary _sourceDictionary, T _value)
		{
			string _key = null;

			if(_value != null)
			{
				ICollection _keys = _sourceDictionary.Keys;
				foreach (string _eachKey in _keys)	
				{
					object _eachValue = _sourceDictionary[_eachKey] as object;
					if (_eachValue != null && _eachValue.Equals(_value))
					{
						_key = _eachKey;
						break;
					}
				}
			}

			return _key;
		}
	}
}
