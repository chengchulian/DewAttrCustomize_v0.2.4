using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

namespace UnityJSON;

public class Instantiater
{
	public static readonly Instantiater Default = new Instantiater();

	protected Instantiater()
	{
	}

	public InstantiationData Instantiate(JSONNode node, Type targetType, Type referingType = null, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (targetType == null)
		{
			throw new ArgumentNullException("targetType");
		}
		if (deserializer == null)
		{
			deserializer = Deserializer.Default;
		}
		return _Instantiate(node, targetType, referingType, options, deserializer);
	}

	public InstantiationData Instantiate(string jsonString, Type targetType, Type referingType = null, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return Instantiate(node, targetType, referingType, options, deserializer);
	}

	public InstantiationData InstantiateWithConstructor(string jsonString, Type targetType, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return InstantiateWithConstructor(node, targetType, options, deserializer);
	}

	public InstantiationData InstantiateWithConstructor(JSONNode node, Type targetType, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (targetType == null)
		{
			throw new ArgumentNullException("targetType");
		}
		if (deserializer == null)
		{
			deserializer = Deserializer.Default;
		}
		return _InstantiateWithConstructor(node, targetType, options, deserializer);
	}

	protected virtual bool TryInstantiate(JSONNode node, Type targetType, Type referingType, NodeOptions options, Deserializer deserializer, out InstantiationData instantiationData)
	{
		instantiationData = InstantiationData.Null;
		return false;
	}

	private InstantiationData _Instantiate(JSONNode node, Type targetType, Type referingType, NodeOptions options, Deserializer deserializer)
	{
		if (TryInstantiate(node, targetType, referingType, options, deserializer, out var instantiationData))
		{
			return instantiationData;
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return InstantiationData.Null;
		}
		if (referingType != targetType)
		{
			if (node.IsObject)
			{
				object[] customAttributes = targetType.GetCustomAttributes(typeof(ConditionalInstantiationAttribute), inherit: false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					ConditionalInstantiationAttribute condition = customAttributes[i] as ConditionalInstantiationAttribute;
					if (object.Equals(node[condition.key].Value, condition.value.ToString()))
					{
						instantiationData = _Instantiate(node, condition.referenceType, targetType, options, deserializer);
						if (condition.ignoreConditionKey)
						{
							instantiationData.ignoredKeys = new HashSet<string> { condition.key };
						}
						return instantiationData;
					}
				}
			}
			DefaultInstantiationAttribute defaultAttribute = Util.GetAttribute<DefaultInstantiationAttribute>(targetType);
			if (defaultAttribute != null)
			{
				return _Instantiate(node, defaultAttribute.referenceType, targetType, options, deserializer);
			}
		}
		return _InstantiateWithConstructor(node, targetType, options, deserializer);
	}

	private InstantiationData _InstantiateWithConstructor(JSONNode node, Type targetType, NodeOptions options, Deserializer deserializer)
	{
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return InstantiationData.Null;
		}
		bool useTupleFormat = Util.GetAttribute<JSONObjectAttribute>(targetType)?.options.ShouldUseTupleFormat() ?? false;
		if (useTupleFormat && !node.IsArray)
		{
			throw new InstantiationException("Expected JSON array, found " + node.Tag);
		}
		if (!useTupleFormat && !node.IsObject)
		{
			throw new InstantiationException("Expected JSON object, found " + node.Tag);
		}
		ConstructorInfo[] constructors = targetType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (ConstructorInfo constructorInfo in constructors)
		{
			if (Util.GetAttribute<JSONConstructorAttribute>(constructorInfo) != null)
			{
				return _InstantiateWithConstructor(node, constructorInfo, deserializer, useTupleFormat);
			}
		}
		try
		{
			InstantiationData instantiationData = default(InstantiationData);
			instantiationData.instantiatedObject = Activator.CreateInstance(targetType);
			instantiationData.needsDeserialization = node.Count != 0;
			return instantiationData;
		}
		catch (Exception)
		{
			return _HandleError(options, "Type " + targetType?.ToString() + " does not have a suitable constructor.");
		}
	}

	private InstantiationData _InstantiateWithConstructor(JSONNode node, ConstructorInfo constructorInfo, Deserializer deserializer, bool useTupleFormat)
	{
		ParameterInfo[] parameters = constructorInfo.GetParameters();
		object[] parameterValues = new object[parameters.Length];
		HashSet<string> ignoredKeys = new HashSet<string>();
		for (int i = 0; i < parameterValues.Length; i++)
		{
			JSONNodeAttribute nodeAttribute = Util.GetAttribute<JSONNodeAttribute>(parameters[i]);
			RestrictTypeAttribute restrictAttribute = Util.GetAttribute<RestrictTypeAttribute>(parameters[i]);
			string key = ((nodeAttribute != null && nodeAttribute.key != null) ? nodeAttribute.key : parameters[i].Name);
			JSONNode parameterNode = (useTupleFormat ? node[i] : node[key]);
			ObjectTypes restrictedTypes = restrictAttribute?.types ?? ObjectTypes.JSON;
			Type[] customTypes = restrictAttribute?.customTypes;
			parameterValues[i] = deserializer.Deserialize(parameterNode, parameters[i].ParameterType, nodeAttribute?.options ?? NodeOptions.Default, restrictedTypes, customTypes);
			if (!useTupleFormat)
			{
				ignoredKeys.Add(key);
			}
		}
		InstantiationData instantiationData = default(InstantiationData);
		instantiationData.instantiatedObject = constructorInfo.Invoke(parameterValues);
		instantiationData.needsDeserialization = !useTupleFormat && ignoredKeys.Count != node.Count;
		instantiationData.ignoredKeys = ignoredKeys;
		return instantiationData;
	}

	private InstantiationData _HandleError(NodeOptions options, string message)
	{
		if (options.ShouldIgnoreUnknownType())
		{
			return InstantiationData.Null;
		}
		throw new InstantiationException(message);
	}
}
