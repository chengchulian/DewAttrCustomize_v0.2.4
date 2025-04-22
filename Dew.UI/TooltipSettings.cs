using System;
using UnityEngine;

public struct TooltipSettings
{
	public TooltipPositionMode mode;

	public Vector2 position;

	public Func<Vector2> getter;

	public float? customDelay;

	public Vector2? pivot;

	public static implicit operator TooltipSettings(Vector2 v)
	{
		TooltipSettings result = default(TooltipSettings);
		result.mode = TooltipPositionMode.RawValue;
		result.position = v;
		return result;
	}

	public static implicit operator TooltipSettings(Vector3 v)
	{
		TooltipSettings result = default(TooltipSettings);
		result.mode = TooltipPositionMode.RawValue;
		result.position = v;
		return result;
	}

	public static implicit operator TooltipSettings(Func<Vector2> v)
	{
		TooltipSettings result = default(TooltipSettings);
		result.mode = TooltipPositionMode.Getter;
		result.getter = v;
		return result;
	}
}
