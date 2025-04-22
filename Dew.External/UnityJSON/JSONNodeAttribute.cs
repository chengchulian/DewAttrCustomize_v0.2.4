using System;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class JSONNodeAttribute : Attribute
{
	private NodeOptions _options;

	private string _key;

	public string key
	{
		get
		{
			return _key;
		}
		set
		{
			_key = ((value == "") ? null : value);
		}
	}

	public NodeOptions options => _options;

	public JSONNodeAttribute(NodeOptions options)
	{
		_options = options;
	}

	public JSONNodeAttribute()
	{
		_options = NodeOptions.Default;
	}
}
