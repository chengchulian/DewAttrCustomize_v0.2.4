using System.Globalization;
using UnityEngine;

namespace FIMSpace;

public static class FColorMethods
{
	public static Color ChangeColorAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static Color ToGammaSpace(Color hdrColor)
	{
		float getMax = hdrColor.r;
		if (hdrColor.g > getMax)
		{
			getMax = hdrColor.g;
		}
		if (hdrColor.b > getMax)
		{
			getMax = hdrColor.b;
		}
		if (hdrColor.a > getMax)
		{
			getMax = hdrColor.a;
		}
		if (getMax <= 0f)
		{
			return Color.clear;
		}
		return hdrColor / getMax;
	}

	public static Color ChangeColorsValue(this Color color, float brightenOrDarken = 0f)
	{
		return new Color(color.r + brightenOrDarken, color.g + brightenOrDarken, color.b + brightenOrDarken, color.a);
	}

	public static Color32 HexToColor(this string hex)
	{
		if (string.IsNullOrEmpty(hex))
		{
			Debug.Log("<color=red>Trying convert from hex to color empty string!</color>");
			return Color.white;
		}
		uint rgba = 255u;
		hex = hex.Replace("#", "");
		hex = hex.Replace("0x", "");
		if (!uint.TryParse(hex, NumberStyles.HexNumber, null, out rgba))
		{
			Debug.Log("Error during converting hex string.");
			return Color.white;
		}
		return new Color32((byte)((rgba & -16777216) >> 24), (byte)((rgba & 0xFF0000) >> 16), (byte)((rgba & 0xFF00) >> 8), (byte)(rgba & 0xFF));
	}

	public static string ColorToHex(this Color32 color, bool addHash = true)
	{
		string hex = "";
		if (addHash)
		{
			hex = "#";
		}
		return hex + string.Format("{0}{1}{2}{3}", (color.r.ToString("X").Length == 1) ? string.Format("0{0}", color.r.ToString("X")) : color.r.ToString("X"), (color.g.ToString("X").Length == 1) ? string.Format("0{0}", color.g.ToString("X")) : color.g.ToString("X"), (color.b.ToString("X").Length == 1) ? string.Format("0{0}", color.b.ToString("X")) : color.b.ToString("X"), (color.a.ToString("X").Length == 1) ? string.Format("0{0}", color.a.ToString("X")) : color.a.ToString("X"));
	}

	public static string ColorToHex(this Color color, bool addHash = true)
	{
		return new Color32((byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), (byte)(color.a * 255f)).ColorToHex(addHash);
	}

	public static void LerpMaterialColor(this Material mat, string property, Color targetColor, float deltaMultiplier = 8f)
	{
		if (!(mat == null))
		{
			if (!mat.HasProperty(property))
			{
				Debug.LogError("Material " + mat.name + " don't have property '" + property + "'  in shader " + mat.shader.name);
			}
			else
			{
				Color currentColor = mat.GetColor(property);
				mat.SetColor(property, Color.Lerp(currentColor, targetColor, Time.deltaTime * deltaMultiplier));
			}
		}
	}
}
