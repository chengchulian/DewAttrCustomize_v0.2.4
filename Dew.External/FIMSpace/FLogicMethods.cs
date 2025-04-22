using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace;

public static class FLogicMethods
{
	public static float Lerp(this float from, float to, float value)
	{
		if (to != from)
		{
			return Mathf.Clamp((value - from) / (to - from), -1f, 1f);
		}
		return 0f;
	}

	public static float InverseLerp(float from, float to, float value)
	{
		if (to != from)
		{
			return Mathf.Clamp((value - from) / (to - from), -1f, 1f);
		}
		return 0f;
	}

	public static float InverseLerpUnclamped(float xx, float yy, float value)
	{
		if (yy - xx == 0f)
		{
			return 0f;
		}
		return (value - xx) / (yy - xx);
	}

	public static float FLerp(float a, float b, float t, float factor = 0.01f)
	{
		float preB = b;
		b = ((!(preB > a)) ? (b - factor) : (b + factor));
		float val = Mathf.LerpUnclamped(a, b, t);
		if (preB > a)
		{
			if (val >= preB)
			{
				return preB;
			}
		}
		else if (val <= preB)
		{
			return preB;
		}
		return val;
	}

	public static int IntLerp(int a, int b, float t)
	{
		int lerp = 0;
		IntLerp(ref lerp, a, b, t);
		return lerp;
	}

	public static void IntLerp(ref int source, int a, int b, float t)
	{
		source = Mathf.RoundToInt((float)a * (1f - t)) + Mathf.RoundToInt((float)b * t);
	}

	public static void IntLerp(ref int source, int b, float t)
	{
		IntLerp(ref source, source, b, t);
	}

	public static float FAbs(this float value)
	{
		if (value < 0f)
		{
			value = 0f - value;
		}
		return value;
	}

	public static float HyperCurve(this float value)
	{
		return 0f - 1f / (3.2f * value - 4f) - 0.25f;
	}

	public static float TopDownDistanceManhattan(this Vector3 a, Vector3 b)
	{
		return 0f + (a.x - b.x).FAbs() + (a.z - b.z).FAbs();
	}

	public static float TopDownDistance(this Vector3 a, Vector3 b)
	{
		a.y = a.z;
		b.y = b.z;
		return Vector2.Distance(a, b);
	}

	public static float DistanceManhattan(this Vector3 a, Vector3 b)
	{
		return 0f + (a.x - b.x).FAbs() + (a.y - b.y).FAbs() + (a.z - b.z).FAbs();
	}

	public static float WrapAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f)
		{
			return angle - 360f;
		}
		return angle;
	}

	public static Vector3 WrapVector(Vector3 angles)
	{
		return new Vector3(WrapAngle(angles.x), WrapAngle(angles.y), WrapAngle(angles.z));
	}

	public static float UnwrapAngle(float angle)
	{
		if (angle >= 0f)
		{
			return angle;
		}
		angle = (0f - angle) % 360f;
		return 360f - angle;
	}

	public static Vector3 UnwrapVector(Vector3 angles)
	{
		return new Vector3(UnwrapAngle(angles.x), UnwrapAngle(angles.y), UnwrapAngle(angles.z));
	}

	public static bool IsAlmostEqual(float val, float to, int afterComma = 2, float addRange = 0f)
	{
		float commaVal = 1f / Mathf.Pow(10f, afterComma) + addRange;
		if ((val > to - commaVal && val < to + commaVal) || val == to)
		{
			return true;
		}
		return false;
	}

	public static Quaternion TopDownAngle(Vector3 from, Vector3 to)
	{
		from.y = 0f;
		to.y = 0f;
		return Quaternion.LookRotation(to - from);
	}

	public static Quaternion TopDownAnglePosition2D(Vector2 from, Vector2 to, float offset = 0f)
	{
		Vector2 dir = to - from;
		return Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * 57.29578f + offset, Vector3.forward);
	}

	public static bool ContainsIndex<T>(this List<T> list, int i, bool falseIfNull = true) where T : class
	{
		if (list == null)
		{
			return false;
		}
		if (i < 0)
		{
			return false;
		}
		if (i >= list.Count)
		{
			return false;
		}
		if (falseIfNull && list[i] == null)
		{
			return false;
		}
		return true;
	}

	public static bool ContainsIndex<T>(this List<T> list, int i) where T : struct
	{
		if (list == null)
		{
			return false;
		}
		if (i < 0)
		{
			return false;
		}
		if (i >= list.Count)
		{
			return false;
		}
		return true;
	}

	public static bool ContainsIndex<T>(this T[] list, int i, bool falseIfNull) where T : class
	{
		if (list == null)
		{
			return false;
		}
		if (i < 0)
		{
			return false;
		}
		if (i >= list.Length)
		{
			return false;
		}
		if (falseIfNull && list[i] == null)
		{
			return false;
		}
		return true;
	}
}
