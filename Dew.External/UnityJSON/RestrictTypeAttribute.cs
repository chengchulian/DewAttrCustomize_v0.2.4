using System;
using System.Collections.Generic;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class RestrictTypeAttribute : Attribute
{
	private ObjectTypes _types;

	private Type[] _customTypes;

	public ObjectTypes types => _types;

	public Type[] customTypes => _customTypes;

	public RestrictTypeAttribute(ObjectTypes types)
	{
		_types = types;
	}

	public RestrictTypeAttribute(ObjectTypes types, params Type[] customTypes)
	{
		if (customTypes == null)
		{
			throw new ArgumentNullException("customTypes");
		}
		_types = types;
		if (!_types.SupportsCustom())
		{
			throw new ArgumentException("Attribute does not support custom types.");
		}
		List<Type> typeList = new List<Type>();
		HashSet<Type> typeSet = new HashSet<Type>();
		foreach (Type type in customTypes)
		{
			if (type != null && !typeSet.Contains(type) && Util.IsCustomType(type))
			{
				typeSet.Add(type);
				typeList.Add(type);
			}
		}
		if (typeList.Count != 0)
		{
			_customTypes = typeList.ToArray();
		}
	}
}
