using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters;

public class QuaternionConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Quaternion);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject val = JObject.Load(reader);
		return new Quaternion(((float?)val["x"]).GetValueOrDefault(), ((float?)val["y"]).GetValueOrDefault(), ((float?)val["z"]).GetValueOrDefault(), ((float?)val["w"]).GetValueOrDefault());
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Quaternion val = (Quaternion)value;
		serializer.Serialize(writer, new { val.x, val.y, val.z, val.w });
	}
}
