using System;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class JSONObjectAttribute : Attribute
{
	private ObjectOptions _options;

	public ObjectOptions options => _options;

	public JSONObjectAttribute(ObjectOptions options)
	{
		_options = options;
	}

	public JSONObjectAttribute()
	{
		_options = ObjectOptions.Default;
	}
}
