using UnityEngine;

namespace DEShaders.Utils;

public static class ShaderExtentions
{
	public static float GetGlobalFloat(this string property)
	{
		return Shader.GetGlobalFloat(property);
	}

	public static void SetGlobalFloat(this string property, float value)
	{
		Shader.SetGlobalFloat(property, value);
	}

	public static void SetGlobalInt(this string property, int value)
	{
		Shader.SetGlobalInt(property, value);
	}
}
