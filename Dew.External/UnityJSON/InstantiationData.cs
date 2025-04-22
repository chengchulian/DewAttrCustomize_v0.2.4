using System.Collections.Generic;

namespace UnityJSON;

public struct InstantiationData
{
	public static readonly InstantiationData Null;

	private object _instantiatedObject;

	private bool _needsDeserialization;

	private HashSet<string> _ignoredKeys;

	public object instantiatedObject
	{
		get
		{
			return _instantiatedObject;
		}
		set
		{
			_instantiatedObject = value;
		}
	}

	public bool needsDeserialization
	{
		get
		{
			if (_instantiatedObject != null)
			{
				return _needsDeserialization;
			}
			return false;
		}
		set
		{
			_needsDeserialization = value;
		}
	}

	public HashSet<string> ignoredKeys
	{
		get
		{
			if (_ignoredKeys == null)
			{
				_ignoredKeys = new HashSet<string>();
			}
			return _ignoredKeys;
		}
		set
		{
			_ignoredKeys = value;
		}
	}
}
