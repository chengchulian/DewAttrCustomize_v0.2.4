using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UnityJSON;

internal static class Util
{
	internal static T GetAttribute<T>(MemberInfo info) where T : Attribute
	{
		object[] attributes = info.GetCustomAttributes(typeof(T), inherit: true);
		if (attributes != null && attributes.Length != 0)
		{
			return attributes[0] as T;
		}
		return null;
	}

	internal static T GetAttribute<T>(ParameterInfo info) where T : Attribute
	{
		object[] attributes = info.GetCustomAttributes(typeof(T), inherit: true);
		if (attributes != null && attributes.Length != 0)
		{
			return attributes[0] as T;
		}
		return null;
	}

	internal static Type GetMemberType(MemberInfo memberInfo)
	{
		if (!(memberInfo is FieldInfo))
		{
			return (memberInfo as PropertyInfo).PropertyType;
		}
		return (memberInfo as FieldInfo).FieldType;
	}

	internal static object GetMemberValue(MemberInfo memberInfo, object obj)
	{
		if (memberInfo is FieldInfo)
		{
			return (memberInfo as FieldInfo).GetValue(obj);
		}
		return (memberInfo as PropertyInfo).GetValue(obj, null);
	}

	internal static void SetMemberValue(MemberInfo memberInfo, object obj, object value)
	{
		if (memberInfo is FieldInfo)
		{
			(memberInfo as FieldInfo).SetValue(obj, value);
		}
		else
		{
			(memberInfo as PropertyInfo).SetValue(obj, value, null);
		}
	}

	internal static bool IsJSONExtrasMember(MemberInfo memberInfo, out JSONExtrasAttribute attribute)
	{
		if (GetMemberType(memberInfo) != typeof(Dictionary<string, object>))
		{
			attribute = null;
			return false;
		}
		attribute = GetAttribute<JSONExtrasAttribute>(memberInfo);
		return attribute != null;
	}

	internal static bool IsCustomType(Type type)
	{
		if (!type.IsEnum && !type.IsPrimitive)
		{
			if (!type.IsValueType)
			{
				if (!typeof(IEnumerable).IsAssignableFrom(type) && Nullable.GetUnderlyingType(type) == null)
				{
					return type != typeof(object);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	internal static bool IsDictionary(Type type)
	{
		if (!typeof(IDictionary).IsAssignableFrom(type))
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(IDictionary<, >);
			}
			return false;
		}
		return true;
	}
}
