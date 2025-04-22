using System;
using UnityEngine;

public static class FEasing
{
	public enum EFease
	{
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		EaseInOutElastic,
		EaseInElastic,
		EaseOutElastic,
		EaseInExpo,
		EaseOutExpo,
		EaseInOutExpo,
		Linear
	}

	public delegate float Function(float s, float e, float v, float extraParameter = 1f);

	public static float EaseInCubic(float start, float end, float value, float ignore = 1f)
	{
		end -= start;
		return end * value * value * value + start;
	}

	public static float EaseOutCubic(float start, float end, float value, float ignore = 1f)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	public static float EaseInOutCubic(float start, float end, float value, float ignore = 1f)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	public static float EaseOutElastic(float start, float end, float value, float rangeMul = 1f)
	{
		end -= start;
		float d = 1f;
		float p = d * 0.3f * rangeMul;
		float a = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= d) == 1f)
		{
			return start + end;
		}
		float s;
		if (a == 0f || a < Mathf.Abs(end))
		{
			a = end;
			s = p * 0.25f * rangeMul;
		}
		else
		{
			s = p / (MathF.PI * 2f) * Mathf.Asin(end / a);
		}
		return a * Mathf.Pow(2f, -10f * value * rangeMul) * Mathf.Sin((value * d - s) * (MathF.PI * 2f) / p) + end + start;
	}

	public static float EaseInElastic(float start, float end, float value, float rangeMul = 1f)
	{
		end -= start;
		float d = 1f;
		float p = d * 0.3f * rangeMul;
		float a = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= d) == 1f)
		{
			return start + end;
		}
		float s;
		if (a == 0f || a < Mathf.Abs(end))
		{
			a = end;
			s = p / 4f * rangeMul;
		}
		else
		{
			s = p / (MathF.PI * 2f) * Mathf.Asin(end / a);
		}
		return 0f - a * Mathf.Pow(2f, 10f * rangeMul * (value -= 1f)) * Mathf.Sin((value * d - s) * (MathF.PI * 2f) / p) + start;
	}

	public static float EaseInOutElastic(float start, float end, float value, float rangeMul = 1f)
	{
		end -= start;
		float d = 1f;
		float p = d * 0.3f * rangeMul;
		float a = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= d * 0.5f) == 2f)
		{
			return start + end;
		}
		float s;
		if (a == 0f || a < Mathf.Abs(end))
		{
			a = end;
			s = p / 4f * rangeMul;
		}
		else
		{
			s = p / (MathF.PI * 2f) * Mathf.Asin(end / a);
		}
		if (value < 1f)
		{
			return -0.5f * (a * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * d - s) * (MathF.PI * 2f) / p)) + start;
		}
		return a * Mathf.Pow(2f, -10f * rangeMul * (value -= 1f)) * Mathf.Sin((value * d - s) * (MathF.PI * 2f) / p) * 0.5f + end + start;
	}

	public static float EaseInExpo(float start, float end, float value, float ignore = 1f)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	public static float EaseOutExpo(float start, float end, float value, float ignore = 1f)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	public static float EaseInOutExpo(float start, float end, float value, float ignore = 1f)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	public static float Linear(float start, float end, float value, float ignore = 1f)
	{
		return Mathf.Lerp(start, end, value);
	}

	public static Function GetEasingFunction(EFease easingFunction)
	{
		return easingFunction switch
		{
			EFease.EaseInCubic => EaseInCubic, 
			EFease.EaseOutCubic => EaseOutCubic, 
			EFease.EaseInOutCubic => EaseInOutCubic, 
			EFease.EaseInElastic => EaseInElastic, 
			EFease.EaseOutElastic => EaseOutElastic, 
			EFease.EaseInOutElastic => EaseInOutElastic, 
			EFease.EaseInExpo => EaseInExpo, 
			EFease.EaseOutExpo => EaseOutExpo, 
			EFease.EaseInOutExpo => EaseInOutExpo, 
			EFease.Linear => Linear, 
			_ => null, 
		};
	}
}
