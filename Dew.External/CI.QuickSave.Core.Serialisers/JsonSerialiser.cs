using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CI.QuickSave.Core.Serialisers;

public static class JsonSerialiser
{
	private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
	{
		Converters = new List<JsonConverter>
		{
			new ColorConverter(),
			new QuaternionConverter(),
			new Matrix4x4Converter(),
			new Texture2DConverter(),
			new SpriteConverter(),
			new Vector2Converter(),
			new Vector3Converter(),
			new Vector4Converter()
		},
		ObjectCreationHandling = ObjectCreationHandling.Replace
	};

	private static readonly JsonSerializer _serialiser = JsonSerializer.Create(_settings);

	public static void RegisterConverter(JsonConverter converter)
	{
		if (!_settings.Converters.Any((JsonConverter x) => x.GetType() == converter.GetType()))
		{
			_settings.Converters.Add(converter);
		}
	}

	public static T DeserialiseKey<T>(string key, JObject data)
	{
		return data[key].ToObject<T>(_serialiser);
	}

	public static JToken SerialiseKey<T>(T data)
	{
		return JToken.FromObject(data, _serialiser);
	}

	public static string Serialise<T>(T value)
	{
		return JsonConvert.SerializeObject(value, _settings);
	}
}
