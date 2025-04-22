using System;

namespace UnityJSON;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class ConditionalInstantiationAttribute : Attribute
{
	private Type _reference;

	private string _key;

	private object _value;

	public Type referenceType => _reference;

	public string key => _key;

	public object value => _value;

	public bool ignoreConditionKey { get; set; }

	public ConditionalInstantiationAttribute(Type reference, string key, object value)
	{
		if (reference == null)
		{
			throw new ArgumentNullException("reference");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		_reference = reference;
		_key = key;
		_value = value;
	}
}
