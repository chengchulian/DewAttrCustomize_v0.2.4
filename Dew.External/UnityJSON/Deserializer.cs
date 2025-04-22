using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

namespace UnityJSON;

public class Deserializer
{
	private static Deserializer _default = new Deserializer();

	public static readonly Deserializer Simple = new Deserializer();

	private Instantiater _instantiater = Instantiater.Default;

	public static Deserializer Default
	{
		get
		{
			return _default;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("default deserializer");
			}
			_default = value;
		}
	}

	public Instantiater instantiater
	{
		get
		{
			return _instantiater;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("instantiater");
			}
			_instantiater = value;
		}
	}

	protected Deserializer()
	{
	}

	protected virtual bool TryDeserializeOn(object obj, JSONNode node, NodeOptions options, HashSet<string> ignoredKeys)
	{
		return false;
	}

	public void DeserializeOn(object obj, string jsonString, NodeOptions options = NodeOptions.Default, HashSet<string> ignoredKeys = null)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		DeserializeOn(obj, node, options, ignoredKeys);
	}

	public void DeserializeOn(object obj, JSONNode node, NodeOptions options = NodeOptions.Default, HashSet<string> ignoredKeys = null)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		Type type = obj.GetType();
		if (type.IsEnum)
		{
			throw new ArgumentException("Cannot deserialize on enums.");
		}
		if (type.IsPrimitive)
		{
			throw new ArgumentException("Cannot deserialize on primitive types.");
		}
		if (ignoredKeys == null)
		{
			ignoredKeys = new HashSet<string>();
		}
		_DeserializeOn(obj, node, options, ignoredKeys);
	}

	public void DeserializeByParts(object obj, string jsonString, NodeOptions options = NodeOptions.Default, HashSet<string> ignoredKeys = null)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		DeserializeByParts(obj, node, options, ignoredKeys);
	}

	public void DeserializeByParts(object obj, JSONNode node, NodeOptions options = NodeOptions.Default, HashSet<string> ignoredKeys = null)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		Type type = obj.GetType();
		if (type.IsEnum)
		{
			throw new ArgumentException("Cannot deserialize on enums.");
		}
		if (type.IsPrimitive)
		{
			throw new ArgumentException("Cannot deserialize on primitive types.");
		}
		if (ignoredKeys == null)
		{
			ignoredKeys = new HashSet<string>();
		}
		_DeserializeByParts(obj, node, options, ignoredKeys);
	}

	public object Deserialize(string jsonString, Type type, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return Deserialize(node, type, options);
	}

	public object Deserialize(JSONNode node, Type type, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return Deserialize(node, type, options, ObjectTypes.JSON, null);
	}

	public object DeserializeToObject(string jsonString, ObjectTypes restrictedTypes = ObjectTypes.JSON, Type[] customTypes = null, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToObject(jsonString, restrictedTypes, customTypes, options);
	}

	public object DeserializeToObject(JSONNode node, ObjectTypes restrictedTypes = ObjectTypes.JSON, Type[] customTypes = null, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (customTypes != null)
		{
			if (!restrictedTypes.SupportsCustom())
			{
				throw new ArgumentException("Restrictions do not allow custom types.");
			}
			foreach (Type type in customTypes)
			{
				if (!Util.IsCustomType(type))
				{
					throw new ArgumentException("Unsupported custom type: " + type);
				}
			}
		}
		return _DeserializeToObject(node, options, restrictedTypes, customTypes);
	}

	public T? DeserializeToNullable<T>(string jsonString, NodeOptions options = NodeOptions.Default) where T : struct
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToNullable<T>(node, options);
	}

	public T? DeserializeToNullable<T>(JSONNode node, NodeOptions options = NodeOptions.Default) where T : struct
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		return (T?)Deserialize(node, typeof(T), options);
	}

	public int DeserializeToInt(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToInt(node, options);
	}

	public int DeserializeToInt(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (int)_DeserializeToInt(node, options);
	}

	public uint DeserializeToUInt(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToUInt(node, options);
	}

	public uint DeserializeToUInt(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (uint)_DeserializeToUInt(node, options);
	}

	public byte DeserializeToByte(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToByte(node, options);
	}

	public byte DeserializeToByte(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (byte)_DeserializeToByte(node, options);
	}

	public bool DeserializeToBool(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToBool(node, options);
	}

	public bool DeserializeToBool(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (bool)_DeserializeToBool(node, options);
	}

	public float DeserializeToFloat(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToFloat(node, options);
	}

	public float DeserializeToFloat(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (float)_DeserializeToFloat(node, options);
	}

	public double DeserializeToDouble(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToDouble(node, options);
	}

	public double DeserializeToDouble(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (double)_DeserializeToDouble(node, options);
	}

	public long DeserializeToLong(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToLong(node, options);
	}

	public long DeserializeToLong(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		return (long)_DeserializeToLong(node, options);
	}

	public string DeserializeToString(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToString(node, options);
	}

	public string DeserializeToString(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		return _DeserializeToString(node, options);
	}

	public T DeserializeToEnum<T>(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToEnum<T>(node, options);
	}

	public T DeserializeToEnum<T>(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("Generic type is not an enum.");
		}
		return (T)_DeserializeToEnum(node, typeof(T), options);
	}

	public List<T> DeserializeToList<T>(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToList<T>(node, options);
	}

	public List<T> DeserializeToList<T>(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		List<T> list = new List<T>();
		_FeedList(list, node, typeof(T), options);
		return list;
	}

	public List<object> DeserializeToObjectList(string jsonString, ObjectTypes restrictedTypes = ObjectTypes.JSON, Type[] customTypes = null, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToObjectList(node, restrictedTypes, customTypes, options);
	}

	public List<object> DeserializeToObjectList(JSONNode node, ObjectTypes restrictedTypes = ObjectTypes.JSON, Type[] customTypes = null, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		List<object> list = new List<object>();
		_FeedList(list, node, typeof(object), options, restrictedTypes, customTypes);
		return list;
	}

	public Dictionary<K, V> DeserializeToDictionary<K, V>(string jsonString, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToDictionary<K, V>(node, options);
	}

	public Dictionary<K, V> DeserializeToDictionary<K, V>(JSONNode node, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		Dictionary<K, V> dictionary = new Dictionary<K, V>();
		_FeedDictionary(dictionary, node, typeof(K), typeof(V), options);
		return dictionary;
	}

	public Dictionary<K, object> DeserializeToObjectDictionary<K>(string jsonString, ObjectTypes restrictedTypes = ObjectTypes.JSON, Type[] customTypes = null, NodeOptions options = NodeOptions.Default)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		return DeserializeToObjectDictionary<K>(node, restrictedTypes, customTypes, options);
	}

	public Dictionary<K, object> DeserializeToObjectDictionary<K>(JSONNode node, ObjectTypes restrictedTypes = ObjectTypes.JSON, Type[] customTypes = null, NodeOptions options = NodeOptions.Default)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		Dictionary<K, object> dictionary = new Dictionary<K, object>();
		_FeedDictionary(dictionary, node, typeof(K), typeof(object), options, restrictedTypes, customTypes);
		return dictionary;
	}

	internal object Deserialize(JSONNode node, Type targetType, NodeOptions options, ObjectTypes restrictedTypes, Type[] customTypes)
	{
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			return null;
		}
		if (targetType == typeof(object))
		{
			return _DeserializeToObject(node, options, restrictedTypes, customTypes);
		}
		if (targetType.IsValueType)
		{
			if (targetType.IsEnum)
			{
				return _DeserializeToEnum(node, targetType, options);
			}
			if (targetType.IsPrimitive)
			{
				return _DeserializeToPrimitive(node, targetType, options);
			}
		}
		else
		{
			if (targetType == typeof(string))
			{
				return _DeserializeToString(node, options);
			}
			if (Nullable.GetUnderlyingType(targetType) != null)
			{
				return _DeserializeToNullable(node, targetType, options);
			}
			if (typeof(IList).IsAssignableFrom(targetType))
			{
				return _DeserializeToIList(node, targetType, options, restrictedTypes, customTypes);
			}
			if (Util.IsDictionary(targetType))
			{
				return _DeserializeToIDictionary(node, targetType, options, restrictedTypes, customTypes);
			}
		}
		return _DeserializeToCustom(node, targetType, options);
	}

	private object _Deserialize(JSONNode node, Type type, NodeOptions options, MemberInfo memberInfo)
	{
		RestrictTypeAttribute typeAttribute = ((memberInfo == null) ? null : Util.GetAttribute<RestrictTypeAttribute>(memberInfo));
		ObjectTypes types = typeAttribute?.types ?? ObjectTypes.JSON;
		Type[] customTypes = typeAttribute?.customTypes;
		return Deserialize(node, type, options, types, customTypes);
	}

	private object _DeserializeToObject(JSONNode node, NodeOptions options, ObjectTypes restrictedTypes, Type[] customTypes)
	{
		if (node.IsArray)
		{
			if (!restrictedTypes.SupportsArray())
			{
				return _HandleMismatch(options, "Arrays are not supported for object.");
			}
			return _DeserializeToArray(node, typeof(object), options, restrictedTypes, customTypes);
		}
		if (node.IsBoolean)
		{
			if (!restrictedTypes.SupportsBool())
			{
				return _HandleMismatch(options, "Bools are not supported for object.");
			}
			return node.AsBool;
		}
		if (node.IsNumber)
		{
			if (!restrictedTypes.SupportsNumber())
			{
				return _HandleMismatch(options, "Numbers are not supported for object.");
			}
			return node.AsDouble;
		}
		if (node.IsObject)
		{
			if (restrictedTypes.SupportsCustom() && customTypes != null)
			{
				foreach (Type customType in customTypes)
				{
					try
					{
						object obj = Deserialize(node, customType);
						if (obj != null)
						{
							return obj;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			if (!restrictedTypes.SupportsDictionary())
			{
				return _HandleMismatch(options, "Dictionaries are not supported for object.");
			}
			return _DeserializeToGenericDictionary(node, typeof(string), typeof(object), options, restrictedTypes, customTypes);
		}
		if (node.IsString)
		{
			if (!restrictedTypes.SupportsString())
			{
				return _HandleMismatch(options, "Strings are not supported for object.");
			}
			return _DeserializeToString(node, options);
		}
		return null;
	}

	private object _DeserializeToNullable(JSONNode node, Type nullableType, NodeOptions options)
	{
		Type underlyingType = Nullable.GetUnderlyingType(nullableType);
		return Deserialize(node, underlyingType, options);
	}

	private object _DeserializeToPrimitive(JSONNode node, Type type, NodeOptions options)
	{
		if (type == typeof(int))
		{
			return _DeserializeToInt(node, options);
		}
		if (type == typeof(byte))
		{
			return _DeserializeToByte(node, options);
		}
		if (type == typeof(long))
		{
			return _DeserializeToLong(node, options);
		}
		if (type == typeof(uint))
		{
			return _DeserializeToUInt(node, options);
		}
		if (type == typeof(bool))
		{
			return _DeserializeToBool(node, options);
		}
		if (type == typeof(float))
		{
			return _DeserializeToFloat(node, options);
		}
		if (type == typeof(double))
		{
			return _DeserializeToDouble(node, options);
		}
		return _HandleUnknown(options, "Unknown primitive type " + type);
	}

	private string _DeserializeToString(JSONNode node, NodeOptions options)
	{
		if (!node.IsString)
		{
			return _HandleMismatch(options, "Expected string, found: " + node) as string;
		}
		return node.Value;
	}

	private object _DeserializeToInt(JSONNode node, NodeOptions options)
	{
		if (node.IsNumber && int.TryParse(node.Value, out var value))
		{
			return value;
		}
		return _HandleMismatch(options, "Expected integer, found " + node);
	}

	private object _DeserializeToUInt(JSONNode node, NodeOptions options)
	{
		if (node.IsNumber && uint.TryParse(node.Value, out var value))
		{
			return value;
		}
		return _HandleMismatch(options, "Expected unsigned integer, found " + node);
	}

	private object _DeserializeToByte(JSONNode node, NodeOptions options)
	{
		if (node.IsNumber && byte.TryParse(node.Value, out var value))
		{
			return value;
		}
		return _HandleMismatch(options, "Expected byte, found " + node);
	}

	private object _DeserializeToLong(JSONNode node, NodeOptions options)
	{
		if (node.IsNumber && long.TryParse(node.Value, out var value))
		{
			return value;
		}
		return _HandleMismatch(options, "Expected long, found " + node);
	}

	private object _DeserializeToFloat(JSONNode node, NodeOptions options)
	{
		if (node.IsNumber)
		{
			return node.AsFloat;
		}
		return _HandleMismatch(options, "Expected float, found " + node);
	}

	private object _DeserializeToDouble(JSONNode node, NodeOptions options)
	{
		if (node.IsNumber)
		{
			return node.AsDouble;
		}
		return _HandleMismatch(options, "Expected double, found " + node);
	}

	private object _DeserializeToBool(JSONNode node, NodeOptions options)
	{
		if (node.IsBoolean)
		{
			return node.AsBool;
		}
		return _HandleMismatch(options, "Expected integer, found " + node);
	}

	private object _DeserializeToEnum(JSONNode node, Type enumType, NodeOptions options)
	{
		Func<object> handleError = () => _HandleMismatch(options, "Expected enum of type " + enumType?.ToString() + ", found: " + node);
		JSONEnumAttribute enumAttribute = Util.GetAttribute<JSONEnumAttribute>(enumType);
		if (enumAttribute != null && enumAttribute.useIntegers && node.IsNumber)
		{
			try
			{
				return Enum.ToObject(enumType, _DeserializeToInt(node, options));
			}
			catch (Exception)
			{
			}
		}
		else if (node.IsString)
		{
			string value = node.Value;
			if (enumAttribute != null)
			{
				if (enumAttribute.prefix != null)
				{
					if (!value.StartsWith(enumAttribute.prefix))
					{
						return handleError();
					}
					value = value.Substring(enumAttribute.prefix.Length);
				}
				if (enumAttribute.suffix != null)
				{
					if (!value.EndsWith(enumAttribute.suffix))
					{
						return handleError();
					}
					value = value.Substring(0, value.Length - enumAttribute.suffix.Length);
				}
			}
			try
			{
				return Enum.Parse(enumType, value, ignoreCase: true);
			}
			catch (Exception)
			{
			}
		}
		return handleError();
	}

	private IDictionary _DeserializeToIDictionary(JSONNode node, Type dictionaryType, NodeOptions options, ObjectTypes types, Type[] customTypes)
	{
		Type genericType = ((!dictionaryType.IsGenericType) ? null : (dictionaryType.IsGenericTypeDefinition ? dictionaryType : dictionaryType.GetGenericTypeDefinition()));
		if (dictionaryType == typeof(IDictionary))
		{
			return _DeserializeToGenericDictionary(node, typeof(string), typeof(object), options, types, customTypes);
		}
		if (genericType == typeof(IDictionary<, >) || genericType == typeof(Dictionary<, >))
		{
			Type[] args = dictionaryType.GetGenericArguments();
			return _DeserializeToGenericDictionary(node, args[0], args[1], options, types, customTypes);
		}
		return _HandleUnknown(options, "Unknown dictionary type " + dictionaryType) as IDictionary;
	}

	private IList _DeserializeToIList(JSONNode node, Type listType, NodeOptions options, ObjectTypes types, Type[] customTypes)
	{
		Type genericType = ((!listType.IsGenericType) ? null : (listType.IsGenericTypeDefinition ? listType : listType.GetGenericTypeDefinition()));
		if (listType == typeof(Array))
		{
			return _DeserializeToArray(node, typeof(object), options, types, customTypes);
		}
		if (listType.IsArray)
		{
			return _DeserializeToArray(node, listType.GetElementType(), options, types, customTypes);
		}
		if (listType == typeof(IList))
		{
			return _DeserializeToGenericList(node, typeof(object), options, types, customTypes);
		}
		if (genericType == typeof(IList<>) || genericType == typeof(List<>))
		{
			return _DeserializeToGenericList(node, listType.GetGenericArguments()[0], options, types, customTypes);
		}
		return _HandleUnknown(options, "Unknown list type " + listType) as IList;
	}

	private Array _DeserializeToArray(JSONNode node, Type elementType, NodeOptions options, ObjectTypes types, Type[] customTypes)
	{
		IList list = _DeserializeToGenericList(node, elementType, options, types, customTypes);
		Array array = Array.CreateInstance(elementType, list.Count);
		list.CopyTo(array, 0);
		return array;
	}

	private IList _DeserializeToGenericList(JSONNode node, Type genericArgument, NodeOptions options, ObjectTypes types = ObjectTypes.JSON, Type[] customTypes = null)
	{
		IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArgument));
		_FeedList(list, node, genericArgument, options, types, customTypes);
		return list;
	}

	private void _FeedList(IList list, JSONNode node, Type genericArgument, NodeOptions options, ObjectTypes types = ObjectTypes.JSON, Type[] customTypes = null)
	{
		if (node.IsArray)
		{
			IEnumerator enumerator = (node as JSONArray).GetEnumerator();
			while (enumerator.MoveNext())
			{
				JSONNode child = (JSONNode)enumerator.Current;
				list.Add(Deserialize(child, genericArgument, options & (NodeOptions)(-65), types, customTypes));
			}
		}
		else
		{
			_HandleMismatch(options, "Expected an array, found " + node);
		}
	}

	private IDictionary _DeserializeToGenericDictionary(JSONNode node, Type keyType, Type valueType, NodeOptions options, ObjectTypes types = ObjectTypes.JSON, Type[] customTypes = null)
	{
		IDictionary dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(keyType, valueType));
		_FeedDictionary(dictionary, node, keyType, valueType, options, types, customTypes);
		return dictionary;
	}

	private void _FeedDictionary(IDictionary dictionary, JSONNode node, Type keyType, Type valueType, NodeOptions options, ObjectTypes types = ObjectTypes.JSON, Type[] customTypes = null)
	{
		if (node.IsObject)
		{
			IEnumerator enumerator = (node as JSONObject).GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, JSONNode> pair = (KeyValuePair<string, JSONNode>)enumerator.Current;
				object key = Deserialize(new JSONString(pair.Key), keyType, NodeOptions.Default, ObjectTypes.JSON, null);
				object value = Deserialize(pair.Value, valueType, options & (NodeOptions)(-65), types, customTypes);
				dictionary.Add(key, value);
			}
		}
		else
		{
			_HandleMismatch(options, "Expected a dictionary, found " + node);
		}
	}

	private object _DeserializeToCustom(JSONNode node, Type targetType, NodeOptions options)
	{
		InstantiationData instantiationData = instantiater.Instantiate(node, targetType, null, options, this);
		if (!instantiationData.needsDeserialization)
		{
			return instantiationData.instantiatedObject;
		}
		_DeserializeOn(instantiationData.instantiatedObject, node, options, instantiationData.ignoredKeys);
		return instantiationData.instantiatedObject;
	}

	private void _DeserializeOn(object obj, JSONNode node, NodeOptions options, HashSet<string> ignoredKeys)
	{
		if (!TryDeserializeOn(obj, node, options, ignoredKeys))
		{
			if (obj is IDeserializable)
			{
				(obj as IDeserializable).Deserialize(node, this);
			}
			else
			{
				_DeserializeByParts(obj, node, options, ignoredKeys);
			}
		}
	}

	private void _DeserializeByParts(object obj, JSONNode node, NodeOptions options, HashSet<string> ignoredKeys)
	{
		IDeserializationListener listener = obj as IDeserializationListener;
		listener?.OnDeserializationWillBegin(this);
		if (node.IsObject)
		{
			try
			{
				Type type = obj.GetType();
				MemberInfo extrasMember = null;
				JSONExtrasAttribute extrasAttribute = null;
				Dictionary<string, object> extras = new Dictionary<string, object>();
				Dictionary<string, List<MemberInfo>> members = _GetDeserializedClassMembers(type, out extrasMember, out extrasAttribute);
				IEnumerator enumerator = (node as JSONObject).GetEnumerator();
				RestrictTypeAttribute extrasTypeAttribute = ((extrasMember == null) ? null : Util.GetAttribute<RestrictTypeAttribute>(extrasMember));
				ObjectTypes extrasTypes = extrasTypeAttribute?.types ?? ObjectTypes.JSON;
				Type[] extrasCustomTypes = extrasTypeAttribute?.customTypes;
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> pair = (KeyValuePair<string, JSONNode>)enumerator.Current;
					if (!ignoredKeys.Contains(pair.Key))
					{
						if (members.ContainsKey(pair.Key))
						{
							_DeserializeClassMember(obj, members[pair.Key], pair.Value);
						}
						else if (extrasMember != null)
						{
							extras.Add(pair.Key, _DeserializeToObject(pair.Value, extrasAttribute.options, extrasTypes, extrasCustomTypes));
						}
						else
						{
							JSONObjectAttribute objectAttribute = Util.GetAttribute<JSONObjectAttribute>(type);
							if (objectAttribute == null || objectAttribute.options.ShouldThrowAtUnknownKey())
							{
								throw new DeserializationException("The key " + pair.Key + " does not exist in class " + type);
							}
						}
					}
				}
				if (extrasMember != null && (extras.Count != 0 || extrasAttribute.options.ShouldAssignNull()))
				{
					Util.SetMemberValue(extrasMember, obj, extras);
				}
				listener?.OnDeserializationSucceeded(this);
				return;
			}
			catch (Exception ex)
			{
				listener?.OnDeserializationFailed(this);
				throw ex;
			}
		}
		if (node.IsNull || node.Tag == JSONNodeType.None)
		{
			listener?.OnDeserializationSucceeded(this);
			return;
		}
		listener?.OnDeserializationFailed(this);
		_HandleMismatch(options, "Expected a JSON object, found " + node);
	}

	private void _DeserializeClassMember(object filledObject, List<MemberInfo> memberInfos, JSONNode node)
	{
		for (int i = 0; i < memberInfos.Count; i++)
		{
			MemberInfo memberInfo = memberInfos[i];
			NodeOptions options = Util.GetAttribute<JSONNodeAttribute>(memberInfo)?.options ?? NodeOptions.Default;
			try
			{
				Type type = Util.GetMemberType(memberInfo);
				if (node.IsObject && !type.IsValueType && !typeof(IDictionary).IsAssignableFrom(type) && !options.ShouldReplaceWithDeserialized())
				{
					object value = Util.GetMemberValue(memberInfo, filledObject);
					if (value != null)
					{
						_DeserializeOn(value, node, options, new HashSet<string>());
						break;
					}
				}
				object deserialized = _Deserialize(node, type, options, memberInfo);
				if (deserialized != null || options.ShouldAssignNull())
				{
					Util.SetMemberValue(memberInfo, filledObject, deserialized);
					break;
				}
			}
			catch (Exception ex)
			{
				if (i == memberInfos.Count - 1)
				{
					throw ex;
				}
			}
		}
	}

	private Dictionary<string, List<MemberInfo>> _GetDeserializedClassMembers(Type classType, out MemberInfo extrasMember, out JSONExtrasAttribute extrasAttribute)
	{
		JSONObjectAttribute objectAttribute = Util.GetAttribute<JSONObjectAttribute>(classType);
		Dictionary<string, List<MemberInfo>> members = new Dictionary<string, List<MemberInfo>>();
		BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		if (objectAttribute != null)
		{
			if (!objectAttribute.options.ShouldIgnoreStatic())
			{
				flags |= BindingFlags.Static;
			}
			if (objectAttribute.options.ShouldUseTupleFormat())
			{
				throw new ArgumentException("Cannot deserialize on a tuple formatted object.");
			}
		}
		extrasMember = null;
		extrasAttribute = null;
		FieldInfo[] fields = classType.GetFields(flags);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (extrasMember == null && Util.IsJSONExtrasMember(fieldInfo, out extrasAttribute))
			{
				extrasMember = fieldInfo;
				continue;
			}
			JSONNodeAttribute fieldAttribute = Util.GetAttribute<JSONNodeAttribute>(fieldInfo);
			if ((fieldAttribute == null || fieldAttribute.options.IsDeserialized()) && !fieldInfo.IsLiteral && (fieldInfo.IsPublic || fieldAttribute != null))
			{
				string key = ((fieldAttribute != null && fieldAttribute.key != null) ? fieldAttribute.key : fieldInfo.Name);
				if (!members.ContainsKey(key))
				{
					members[key] = new List<MemberInfo>();
				}
				members[key].Add(fieldInfo);
			}
		}
		if (objectAttribute == null || !objectAttribute.options.ShouldIgnoreProperties())
		{
			PropertyInfo[] properties = classType.GetProperties(flags);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (extrasMember == null && Util.IsJSONExtrasMember(propertyInfo, out extrasAttribute))
				{
					extrasMember = propertyInfo;
					continue;
				}
				JSONNodeAttribute fieldAttribute2 = Util.GetAttribute<JSONNodeAttribute>(propertyInfo);
				if ((fieldAttribute2 == null || fieldAttribute2.options.IsDeserialized()) && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanWrite && (fieldAttribute2 != null || propertyInfo.GetSetMethod(nonPublic: false) != null))
				{
					string key2 = ((fieldAttribute2 != null && fieldAttribute2.key != null) ? fieldAttribute2.key : propertyInfo.Name);
					if (!members.ContainsKey(key2))
					{
						members[key2] = new List<MemberInfo>();
					}
					members[key2].Add(propertyInfo);
				}
			}
		}
		return members;
	}

	private object _HandleMismatch(NodeOptions options, string message)
	{
		if (!options.ShouldIgnoreTypeMismatch())
		{
			throw new DeserializationException(message);
		}
		return null;
	}

	private object _HandleUnknown(NodeOptions options, string message)
	{
		if (!options.ShouldIgnoreUnknownType())
		{
			throw new DeserializationException(message);
		}
		return null;
	}
}
