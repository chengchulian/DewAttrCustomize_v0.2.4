using UnityEngine;

namespace FIMSpace;

public static class FVectorMethods
{
	public static Vector3 RandomVector(float rangeA, float rangeB)
	{
		return new Vector3(Random.Range(rangeA, rangeB), Random.Range(rangeA, rangeB), Random.Range(rangeA, rangeB));
	}

	public static float VectorSum(Vector3 vector)
	{
		return vector.x + vector.y + vector.z;
	}

	public static Vector3 RandomVectorNoY(float rangeA, float rangeB)
	{
		return new Vector3(Random.Range(rangeA, rangeB), 0f, Random.Range(rangeA, rangeB));
	}

	public static Vector3 RandomVectorMinMax(float min, float max)
	{
		float mul1 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			mul1 = -1f;
		}
		float mul2 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			mul2 = -1f;
		}
		float mul3 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			mul3 = -1f;
		}
		return new Vector3(Random.Range(min, max) * mul1, Random.Range(min, max) * mul2, Random.Range(min, max) * mul3);
	}

	public static Vector3 RandomVectorNoYMinMax(float min, float max)
	{
		float mul1 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			mul1 = -1f;
		}
		float mul2 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			mul2 = -1f;
		}
		return new Vector3(Random.Range(min, max) * mul1, 0f, Random.Range(min, max) * mul2);
	}

	public static Vector3 GetUIPositionFromWorldPosition(Vector3 position, Camera camera, RectTransform canvas)
	{
		Vector3 uiPosition = camera.WorldToViewportPoint(position);
		uiPosition.x *= canvas.sizeDelta.x;
		uiPosition.y *= canvas.sizeDelta.y;
		uiPosition.x -= canvas.sizeDelta.x * canvas.pivot.x;
		uiPosition.y -= canvas.sizeDelta.y * canvas.pivot.y;
		return uiPosition;
	}

	public static Vector2 XOZ(this Vector3 toBeFlattened)
	{
		return new Vector2(toBeFlattened.x, toBeFlattened.z);
	}

	public static Vector3 XOZ(this Vector3 toBeFlattened, float yValue = 0f)
	{
		return new Vector3(toBeFlattened.x, yValue, toBeFlattened.z);
	}

	public static float DistanceTopDown(Vector3 from, Vector3 to)
	{
		return Vector2.Distance(from.XOZ(), to.XOZ());
	}

	public static float DistanceTopDownManhattan(Vector3 from, Vector3 to)
	{
		return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.z - to.z);
	}

	public static float BoundsSizeOnAxis(this Bounds bounds, Vector3 normalized)
	{
		return Vector3.Scale(bounds.size, normalized).magnitude;
	}

	public static Vector3 ChooseDominantAxis(Vector3 axis)
	{
		Vector3 abs = new Vector3(Mathf.Abs(axis.x), Mathf.Abs(axis.y), Mathf.Abs(axis.z));
		if (abs.x > abs.y)
		{
			if (abs.z > abs.x)
			{
				return new Vector3(0f, 0f, (axis.z > 0f) ? 1f : (-1f));
			}
			return new Vector3((axis.x > 0f) ? 1f : (-1f), 0f, 0f);
		}
		if (abs.z > abs.y)
		{
			return new Vector3(0f, 0f, (axis.z > 0f) ? 1f : (-1f));
		}
		return new Vector3(0f, (axis.y > 0f) ? 1f : (-1f), 0f);
	}

	public static Vector3 GetRounded(Vector3 dir)
	{
		return new Vector3(Mathf.Round(dir.x), Mathf.Round(dir.y), Mathf.Round(dir.z));
	}

	public static Vector3 GetCounterAxis(Vector3 axis)
	{
		return new Vector3(axis.z, axis.x, axis.y);
	}

	public static Color GetAxisColor(Vector3 axis, float alpha = 0.75f)
	{
		return new Color(axis.z, axis.x, axis.y, alpha);
	}

	public static Vector3 FlattenVector(Vector3 v, float to = 90f)
	{
		v.x = Mathf.Round(v.x / to) * to;
		v.y = Mathf.Round(v.y / to) * to;
		v.z = Mathf.Round(v.z / to) * to;
		return v;
	}

	public static Vector3 FlattenVectorFlr(Vector3 v, float to = 90f)
	{
		v.x = Mathf.Floor(v.x / to) * to;
		v.y = Mathf.Floor(v.y / to) * to;
		v.z = Mathf.Floor(v.z / to) * to;
		return v;
	}

	public static Vector3 FlattenVectorCeil(Vector3 v, float to = 90f)
	{
		v.x = Mathf.Ceil(v.x / to) * to;
		v.y = Mathf.Ceil(v.y / to) * to;
		v.z = Mathf.Ceil(v.z / to) * to;
		return v;
	}

	public static Vector3 FlattenVector(Vector3 v, Vector3 to)
	{
		v.x = Mathf.Round(v.x / to.x) * to.x;
		v.y = Mathf.Round(v.y / to.y) * to.y;
		v.z = Mathf.Round(v.z / to.z) * to.z;
		return v;
	}

	public static Vector3Int V3toV3Int(Vector3 v)
	{
		return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
	}

	public static Vector3 FlattenNormal(Quaternion orientation, Vector3? forward = null, float to = 90f)
	{
		Vector3 f = ((!forward.HasValue) ? Vector3.forward : forward.Value);
		return Quaternion.Euler(FlattenVector(orientation.eulerAngles, to)) * f;
	}

	public static Vector3 EqualVector(float valueAll)
	{
		return new Vector3(valueAll, valueAll, valueAll);
	}

	public static Quaternion FlattenRotation(Quaternion orientation, float to = 90f)
	{
		return Quaternion.Euler(FlattenVector(orientation.eulerAngles, to));
	}
}
