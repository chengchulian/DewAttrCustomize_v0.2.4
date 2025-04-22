using System;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JSONExtrasAttribute : Attribute
{
	private NodeOptions _options;

	public NodeOptions options => _options;

	public JSONExtrasAttribute(NodeOptions options)
	{
		_options = options;
	}

	public JSONExtrasAttribute()
	{
		_options = NodeOptions.Default;
	}
}
