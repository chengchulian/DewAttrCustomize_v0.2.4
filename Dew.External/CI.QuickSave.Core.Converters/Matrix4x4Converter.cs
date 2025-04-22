using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters;

public class Matrix4x4Converter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Matrix4x4);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject val = JObject.Load(reader);
		Matrix4x4 matrix4x = default(Matrix4x4);
		matrix4x.m00 = ((float?)val["m00"]).GetValueOrDefault();
		matrix4x.m33 = ((float?)val["m33"]).GetValueOrDefault();
		matrix4x.m23 = ((float?)val["m23"]).GetValueOrDefault();
		matrix4x.m13 = ((float?)val["m13"]).GetValueOrDefault();
		matrix4x.m03 = ((float?)val["m03"]).GetValueOrDefault();
		matrix4x.m32 = ((float?)val["m32"]).GetValueOrDefault();
		matrix4x.m22 = ((float?)val["m22"]).GetValueOrDefault();
		matrix4x.m02 = ((float?)val["m02"]).GetValueOrDefault();
		matrix4x.m12 = ((float?)val["m12"]).GetValueOrDefault();
		matrix4x.m21 = ((float?)val["m21"]).GetValueOrDefault();
		matrix4x.m11 = ((float?)val["m11"]).GetValueOrDefault();
		matrix4x.m01 = ((float?)val["m01"]).GetValueOrDefault();
		matrix4x.m30 = ((float?)val["m30"]).GetValueOrDefault();
		matrix4x.m20 = ((float?)val["m20"]).GetValueOrDefault();
		matrix4x.m10 = ((float?)val["m10"]).GetValueOrDefault();
		matrix4x.m31 = ((float?)val["m31"]).GetValueOrDefault();
		return matrix4x;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Matrix4x4 val = (Matrix4x4)value;
		serializer.Serialize(writer, new
		{
			val.m00, val.m33, val.m23, val.m13, val.m03, val.m32, val.m22, val.m02, val.m12, val.m21,
			val.m11, val.m01, val.m30, val.m20, val.m10, val.m31
		});
	}
}
