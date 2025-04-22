using System;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Enum)]
public class JSONEnumAttribute : Attribute
{
	private bool _useIntegers;

	private JSONEnumMemberFormating _format;

	private string _prefix;

	private string _suffix;

	public bool useIntegers
	{
		get
		{
			return _useIntegers;
		}
		set
		{
			_useIntegers = value;
		}
	}

	public JSONEnumMemberFormating format
	{
		get
		{
			return _format;
		}
		set
		{
			_format = value;
		}
	}

	public string prefix
	{
		get
		{
			return _prefix;
		}
		set
		{
			_prefix = ((value == "") ? null : value);
		}
	}

	public string suffix
	{
		get
		{
			return _suffix;
		}
		set
		{
			_suffix = ((value == "") ? null : value);
		}
	}
}
