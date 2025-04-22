using System;
using SimpleJSON;

namespace UnityJSON;

public static class JSON
{
	public static string Serialize(object obj, NodeOptions options = NodeOptions.Default, Serializer serializer = null)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (serializer == null)
		{
			serializer = Serializer.Default;
		}
		return serializer.Serialize(obj, options);
	}

	public static string Serialize(object obj, Serializer serializer)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (serializer == null)
		{
			throw new ArgumentNullException("serializer");
		}
		return serializer.Serialize(obj);
	}

	public static string ToJSONString(this object obj, NodeOptions options = NodeOptions.Default, Serializer serializer = null)
	{
		return Serialize(obj, options, serializer);
	}

	public static string ToJSONString(this object obj, Serializer serializer)
	{
		return Serialize(obj, serializer);
	}

	public static T Deserialize<T>(string jsonString, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		if (deserializer == null)
		{
			deserializer = Deserializer.Default;
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		if (node == null)
		{
			throw new ArgumentException("Argument is not a valid JSON string: " + jsonString);
		}
		return (T)deserializer.Deserialize(node, typeof(T), options);
	}

	public static T Deserialize<T>(string jsonString, Deserializer deserializer)
	{
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		if (deserializer == null)
		{
			throw new ArgumentNullException("deserializer");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		if (node == null)
		{
			throw new ArgumentException("Argument is not a valid JSON string: " + jsonString);
		}
		return (T)deserializer.Deserialize(node, typeof(T));
	}

	public static void DeserializeOn(object obj, string jsonString, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		if (deserializer == null)
		{
			deserializer = Deserializer.Default;
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		if (node == null)
		{
			throw new ArgumentException("Argument is not a valid JSON string: " + jsonString);
		}
		deserializer.DeserializeOn(obj, node, options);
	}

	public static void DeserializeOn(object obj, string jsonString, Deserializer deserializer)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (jsonString == null)
		{
			throw new ArgumentNullException("jsonString");
		}
		if (deserializer == null)
		{
			throw new ArgumentNullException("deserializer");
		}
		JSONNode node = global::SimpleJSON.JSON.Parse(jsonString);
		if (node == null)
		{
			throw new ArgumentException("Argument is not a valid JSON string: " + jsonString);
		}
		deserializer.DeserializeOn(obj, node);
	}

	public static void FeedJSON(this object obj, string jsonString, NodeOptions options = NodeOptions.Default, Deserializer deserializer = null)
	{
		DeserializeOn(obj, jsonString, options, deserializer);
	}

	public static void FeedJSON(this object obj, string jsonString, Deserializer deserializer)
	{
		DeserializeOn(obj, jsonString, deserializer);
	}
}
