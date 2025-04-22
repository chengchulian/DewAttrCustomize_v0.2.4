using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityJSON;

public class Serializer
{
	private const string _kUndefined = "undefined";

	private const string _kNull = "null";

	private const string _kTrue = "true";

	private const string _kFalse = "false";

	private static Serializer _default = new Serializer();

	private static readonly Serializer Simple = new Serializer();

	public bool useUndefinedForNull;

	public static Serializer Default
	{
		get
		{
			return _default;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("default serializer");
			}
			_default = value;
		}
	}

	protected Serializer()
	{
	}

	protected virtual bool TrySerialize(object obj, NodeOptions options, out string serialized)
	{
		serialized = null;
		return false;
	}

	public string Serialize(object obj, NodeOptions options = NodeOptions.Default)
	{
		if (obj == null)
		{
			return SerializeNull(options);
		}
		string result = null;
		if (TrySerialize(obj, options, out result))
		{
			return result;
		}
		if (obj is ISerializable serializable)
		{
			return serializable.Serialize(this);
		}
		if (obj != null)
		{
			Type type = obj.GetType();
			if (type.IsValueType)
			{
				result = (type.IsEnum ? _SerializeEnum((Enum)obj) : (type.IsPrimitive ? ((!(obj is bool)) ? obj.ToString() : SerializeBool((bool)obj)) : ((obj is DictionaryEntry) ? _SerializeDictionaryEntry((DictionaryEntry)obj, options) : ((obj is Vector2) ? SerializeVector2((Vector2)obj) : ((obj is Vector3) ? SerializeVector3((Vector3)obj) : ((obj is Vector4) ? SerializeVector4((Vector4)obj) : ((obj is Quaternion) ? SerializeQuaternion((Quaternion)obj) : ((obj is Color) ? SerializeColor((Color)obj) : ((obj is Rect) ? SerializeRect((Rect)obj) : ((!(obj is Bounds)) ? _SerializeCustom(obj, options) : SerializeBounds((Bounds)obj)))))))))));
			}
			else if (obj is string)
			{
				result = _SerializeString(obj as string);
			}
			else if (Nullable.GetUnderlyingType(type) != null)
			{
				result = _SerializeNullable(obj, options);
			}
			else if (obj is IEnumerable)
			{
				string enumerableJSON = _SerializeEnumarable(obj as IEnumerable, options);
				result = ((!(obj is IDictionary)) ? ("[" + enumerableJSON + "]") : ("{" + enumerableJSON + "}"));
			}
			else
			{
				result = _SerializeCustom(obj, options);
			}
		}
		if (result == null)
		{
			return SerializeNull(options);
		}
		return result;
	}

	public string SerializeNull(NodeOptions options)
	{
		if (!options.ShouldSerializeNull())
		{
			return null;
		}
		if (!useUndefinedForNull)
		{
			return "null";
		}
		return "undefined";
	}

	public string SerializeString(string stringValue, NodeOptions options = NodeOptions.Default)
	{
		if (stringValue == null)
		{
			return SerializeNull(options);
		}
		return _SerializeString(stringValue);
	}

	public string SerializeEnum(Enum enumValue)
	{
		if (enumValue == null)
		{
			throw new ArgumentNullException("enumValue");
		}
		return _SerializeEnum(enumValue);
	}

	public string SerializeNullable(object nullable, NodeOptions options = NodeOptions.Default)
	{
		if (nullable == null)
		{
			return SerializeNull(options);
		}
		if (Nullable.GetUnderlyingType(nullable.GetType()) == null)
		{
			throw new ArgumentException("Argument is not a nullable object.");
		}
		return _SerializeNullable(nullable, options);
	}

	public string SerializeBool(bool boolValue)
	{
		if (!boolValue)
		{
			return "false";
		}
		return "true";
	}

	public string SerializeVector2(Vector2 vector)
	{
		return "{\"x\":" + vector.x + ",\"y\":" + vector.y + "}";
	}

	public string SerializeVector3(Vector3 vector)
	{
		return "{\"x\":" + vector.x + ",\"y\":" + vector.y + ",\"z\":" + vector.z + "}";
	}

	public string SerializeVector4(Vector4 vector)
	{
		return "{\"x\":" + vector.x + ",\"y\":" + vector.y + ",\"z\":" + vector.z + ",\"w\":" + vector.w + "}";
	}

	public string SerializeQuaternion(Quaternion quaternion)
	{
		return "{\"x\":" + quaternion.x + ",\"y\":" + quaternion.y + ",\"z\":" + quaternion.z + ",\"w\":" + quaternion.w + "}";
	}

	public string SerializeColor(Color color)
	{
		return "{\"r\":" + color.r + ",\"g\":" + color.g + ",\"b\":" + color.b + ",\"a\":" + color.a + "}";
	}

	public string SerializeRect(Rect rect)
	{
		return "{\"x\":" + rect.x + ",\"y\":" + rect.y + ",\"width\":" + rect.width + ",\"height\":" + rect.height + "}";
	}

	public string SerializeBounds(Bounds bounds)
	{
		return "{\"center\":" + SerializeVector3(bounds.center) + ",\"extents\":" + SerializeVector3(bounds.extents) + "}";
	}

	public string SerializeByParts(object obj, NodeOptions options = NodeOptions.Default)
	{
		if (obj == null)
		{
			return SerializeNull(options);
		}
		Type type = obj.GetType();
		if (type.IsPrimitive || type.IsEnum)
		{
			throw new ArgumentException("Cannot serialize non-struct value types by parts.");
		}
		return _SerializeCustom(obj, options);
	}

	private string _SerializeString(string stringValue)
	{
		return "\"" + stringValue.Replace("\"", "\\\"") + "\"";
	}

	private string _SerializeEnum(Enum obj)
	{
		JSONEnumAttribute enumAttribute = Util.GetAttribute<JSONEnumAttribute>(obj.GetType());
		if (enumAttribute != null)
		{
			if (enumAttribute.useIntegers)
			{
				return Convert.ToInt32(obj).ToString();
			}
			string formatted = _FormatEnumMember(obj.ToString(), enumAttribute.format);
			if (enumAttribute.prefix != null)
			{
				formatted = enumAttribute.prefix + formatted;
			}
			if (enumAttribute.suffix != null)
			{
				formatted += enumAttribute.suffix;
			}
			return _SerializeString(formatted);
		}
		return _SerializeString(obj.ToString());
	}

	private string _SerializeNullable(object nullable, NodeOptions options)
	{
		Type type = nullable.GetType();
		if (!(bool)type.GetProperty("HasValue").GetValue(nullable, null))
		{
			return SerializeNull(options);
		}
		return Serialize(type.GetProperty("Value").GetValue(nullable, null), options);
	}

	private string _SerializeEnumarable(IEnumerable enumerable, NodeOptions options)
	{
		if (enumerable is IList)
		{
			options |= NodeOptions.SerializeNull;
		}
		string joined = _Join(enumerable, (object obj) => Serialize(obj, options));
		if (joined == null)
		{
			return SerializeNull(options);
		}
		return joined;
	}

	private string _SerializeDictionaryEntry(DictionaryEntry entry, NodeOptions options)
	{
		NodeOptions keyOptions = options & (NodeOptions)(-5);
		string serializedKey = Serialize(entry.Key, keyOptions);
		if (serializedKey == null)
		{
			return SerializeNull(options);
		}
		string valueKey = Serialize(entry.Value, options);
		if (valueKey == null)
		{
			return null;
		}
		return ((entry.Key is string) ? serializedKey : _SerializeString(serializedKey)) + ":" + valueKey;
	}

	private string _SerializeCustom(object obj, NodeOptions options)
	{
		ISerializationListener listener = obj as ISerializationListener;
		listener?.OnSerializationWillBegin(this);
		try
		{
			IEnumerable<string> enumerable = new string[0];
			MemberInfo extrasMember = null;
			JSONExtrasAttribute extrasAttribute = null;
			Func<MemberInfo, bool> isNotExtras = delegate(MemberInfo m)
			{
				if (extrasMember == null && Util.IsJSONExtrasMember(m, out extrasAttribute))
				{
					extrasMember = m;
					return false;
				}
				return true;
			};
			Type type = obj.GetType();
			JSONObjectAttribute objectAttribute = Util.GetAttribute<JSONObjectAttribute>(type);
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			bool useTupleFormat = false;
			if (objectAttribute != null)
			{
				if (!objectAttribute.options.ShouldIgnoreStatic())
				{
					flags |= BindingFlags.Static;
				}
				if (objectAttribute.options.ShouldUseTupleFormat())
				{
					useTupleFormat = true;
				}
			}
			enumerable = enumerable.Concat(from f in type.GetFields(flags)
				where isNotExtras(f) && _IsValidFieldInfo(f)
				select _SerializeCustomField(obj, f, useTupleFormat));
			if (objectAttribute == null || !objectAttribute.options.ShouldIgnoreProperties())
			{
				enumerable = enumerable.Concat(from p in type.GetProperties(flags)
					where isNotExtras(p) && _IsValidPropertyInfo(p)
					select _SerializeCustomProperty(obj, p, useTupleFormat));
			}
			string result = _Join(enumerable, (object o) => o as string);
			if (!useTupleFormat && extrasMember != null && Util.GetMemberValue(extrasMember, obj) is IEnumerable extras)
			{
				result = result + ((result == "") ? "" : ",") + _SerializeEnumarable(extras, extrasAttribute.options);
			}
			listener?.OnSerializationSucceeded(this);
			if (useTupleFormat)
			{
				return "[" + result + "]";
			}
			return "{" + result + "}";
		}
		catch (Exception ex)
		{
			listener?.OnSerializationFailed(this);
			throw ex;
		}
	}

	private bool _IsValidFieldInfo(FieldInfo fieldInfo)
	{
		if (!fieldInfo.IsPublic)
		{
			return Attribute.IsDefined(fieldInfo, typeof(JSONNodeAttribute), inherit: true);
		}
		return true;
	}

	private bool _IsValidPropertyInfo(PropertyInfo propertyInfo)
	{
		if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)
		{
			if (!(propertyInfo.GetGetMethod(nonPublic: false) != null))
			{
				return Attribute.IsDefined(propertyInfo, typeof(JSONNodeAttribute), inherit: true);
			}
			return true;
		}
		return false;
	}

	private string _FormatEnumMember(string member, JSONEnumMemberFormating format)
	{
		return format switch
		{
			JSONEnumMemberFormating.Lowercased => member.ToLower(), 
			JSONEnumMemberFormating.Uppercased => member.ToUpper(), 
			JSONEnumMemberFormating.Capitalized => char.ToUpper(member[0]) + member.Substring(1).ToLower(), 
			_ => member, 
		};
	}

	private string _Join(IEnumerable enumerable, Func<object, string> serializer)
	{
		string result = "";
		bool firstAdded = false;
		IEnumerator enumerator;
		if (!(enumerable is IDictionary))
		{
			enumerator = enumerable.GetEnumerator();
		}
		else
		{
			IEnumerator enumerator2 = (enumerable as IDictionary).GetEnumerator();
			enumerator = enumerator2;
		}
		IEnumerator enumerator3 = enumerator;
		while (enumerator3.MoveNext())
		{
			string serialized = serializer(enumerator3.Current);
			if (serialized != null)
			{
				string prefix = (firstAdded ? "," : "");
				firstAdded = true;
				result = result + prefix + serialized;
			}
		}
		return result;
	}

	private string _SerializeCustomField(object obj, FieldInfo fieldInfo, bool useTupleFormat)
	{
		return _SerializeCustomMember(fieldInfo, fieldInfo.GetValue(obj), useTupleFormat);
	}

	private string _SerializeCustomProperty(object obj, PropertyInfo propertyInfo, bool useTupleFormat)
	{
		return _SerializeCustomMember(propertyInfo, propertyInfo.GetValue(obj, null), useTupleFormat);
	}

	private string _SerializeCustomMember(MemberInfo keyMemberInfo, object value, bool useTupleFormat)
	{
		JSONNodeAttribute attribute = Util.GetAttribute<JSONNodeAttribute>(keyMemberInfo);
		NodeOptions options = attribute?.options ?? NodeOptions.Default;
		if (!options.IsSerialized())
		{
			return null;
		}
		string valueString = Serialize(value, options);
		if (valueString != null || options.ShouldSerializeNull())
		{
			if (useTupleFormat)
			{
				if (valueString != null)
				{
					return valueString;
				}
				return "undefined";
			}
			string key = ((attribute != null && attribute.key != null) ? attribute.key : keyMemberInfo.Name);
			return _SerializeString(key) + ":" + ((valueString == null) ? "undefined" : valueString);
		}
		return null;
	}
}
