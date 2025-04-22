using System;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class DefaultInstantiationAttribute : Attribute
{
	private Type _reference;

	public Type referenceType => _reference;

	public DefaultInstantiationAttribute(Type reference)
	{
		if (reference == null)
		{
			throw new ArgumentNullException("reference");
		}
		_reference = reference;
	}
}
