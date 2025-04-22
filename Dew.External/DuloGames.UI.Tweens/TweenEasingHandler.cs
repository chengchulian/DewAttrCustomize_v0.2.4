using System;
using UnityEngine;

namespace DuloGames.UI.Tweens;

internal class TweenEasingHandler
{
	public static float Apply(TweenEasing e, float t, float b, float c, float d)
	{
		switch (e)
		{
		case TweenEasing.Swing:
			return (0f - c) * (t /= d) * (t - 2f) + b;
		case TweenEasing.InQuad:
			return c * (t /= d) * t + b;
		case TweenEasing.OutQuad:
			return (0f - c) * (t /= d) * (t - 2f) + b;
		case TweenEasing.InOutQuad:
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return (0f - c) / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		case TweenEasing.InCubic:
			return c * (t /= d) * t * t + b;
		case TweenEasing.OutCubic:
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		case TweenEasing.InOutCubic:
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		case TweenEasing.InQuart:
			return c * (t /= d) * t * t * t + b;
		case TweenEasing.OutQuart:
			return (0f - c) * ((t = t / d - 1f) * t * t * t - 1f) + b;
		case TweenEasing.InOutQuart:
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return (0f - c) / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		case TweenEasing.InQuint:
			return c * (t /= d) * t * t * t * t + b;
		case TweenEasing.OutQuint:
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		case TweenEasing.InOutQuint:
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		case TweenEasing.InSine:
			return (0f - c) * Mathf.Cos(t / d * (MathF.PI / 2f)) + c + b;
		case TweenEasing.OutSine:
			return c * Mathf.Sin(t / d * (MathF.PI / 2f)) + b;
		case TweenEasing.InOutSine:
			return (0f - c) / 2f * (Mathf.Cos(MathF.PI * t / d) - 1f) + b;
		case TweenEasing.InExpo:
			if (t != 0f)
			{
				return c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b;
			}
			return b;
		case TweenEasing.OutExpo:
			if (t != d)
			{
				return c * (0f - Mathf.Pow(2f, -10f * t / d) + 1f) + b;
			}
			return b + c;
		case TweenEasing.InOutExpo:
			if (t == 0f)
			{
				return b;
			}
			if (t == d)
			{
				return b + c;
			}
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
			}
			return c / 2f * (0f - Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
		case TweenEasing.InCirc:
			return (0f - c) * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
		case TweenEasing.OutCirc:
			return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
		case TweenEasing.InOutCirc:
			if ((t /= d / 2f) < 1f)
			{
				return (0f - c) / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
			}
			return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
		case TweenEasing.InBack:
		{
			float s5 = 1.70158f;
			return c * (t /= d) * t * ((s5 + 1f) * t - s5) + b;
		}
		case TweenEasing.OutBack:
		{
			float s4 = 1.70158f;
			return c * ((t = t / d - 1f) * t * ((s4 + 1f) * t + s4) + 1f) + b;
		}
		case TweenEasing.InOutBack:
		{
			float s3 = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((s3 *= 1.525f) + 1f) * t - s3)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((s3 *= 1.525f) + 1f) * t + s3) + 2f) + b;
		}
		case TweenEasing.InBounce:
			return c - Apply(TweenEasing.OutBounce, d - t, 0f, c, d) + b;
		case TweenEasing.OutBounce:
			if ((t /= d) < 0.36363637f)
			{
				return c * (7.5625f * t * t) + b;
			}
			if (t < 0.72727275f)
			{
				return c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
			}
			if (t < 0.90909094f)
			{
				return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
			}
			return c * (7.5625f * (t -= 21f / 22f) * t + 63f / 64f) + b;
		case TweenEasing.InOutBounce:
			if (t < d / 2f)
			{
				return Apply(TweenEasing.InBounce, t * 2f, 0f, c, d) * 0.5f + b;
			}
			return Apply(TweenEasing.OutBounce, t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		case TweenEasing.InElastic:
		{
			float s6 = 1.70158f;
			float p3 = 0f;
			float a3 = c;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			if (p3 == 0f)
			{
				p3 = d * 0.3f;
			}
			if (a3 < Mathf.Abs(c))
			{
				a3 = c;
				s6 = p3 / 4f;
			}
			else
			{
				s6 = p3 / (MathF.PI * 2f) * Mathf.Asin(c / a3);
			}
			if (float.IsNaN(s6))
			{
				s6 = 0f;
			}
			return 0f - a3 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - s6) * (MathF.PI * 2f) / p3) + b;
		}
		case TweenEasing.OutElastic:
		{
			float s2 = 1.70158f;
			float p2 = 0f;
			float a2 = c;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			if (p2 == 0f)
			{
				p2 = d * 0.3f;
			}
			if (a2 < Mathf.Abs(c))
			{
				a2 = c;
				s2 = p2 / 4f;
			}
			else
			{
				s2 = p2 / (MathF.PI * 2f) * Mathf.Asin(c / a2);
			}
			if (float.IsNaN(s2))
			{
				s2 = 0f;
			}
			return a2 * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - s2) * (MathF.PI * 2f) / p2) + c + b;
		}
		case TweenEasing.InOutElastic:
		{
			float s = 1.70158f;
			float p = 0f;
			float a = c;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d / 2f) == 2f)
			{
				return b + c;
			}
			if (p == 0f)
			{
				p = d * 0.45000002f;
			}
			if (a < Mathf.Abs(c))
			{
				a = c;
				s = p / 4f;
			}
			else
			{
				s = p / (MathF.PI * 2f) * Mathf.Asin(c / a);
			}
			if (float.IsNaN(s))
			{
				s = 0f;
			}
			if (t < 1f)
			{
				return -0.5f * (a * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (MathF.PI * 2f) / p)) + b;
			}
			return a * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (MathF.PI * 2f) / p) * 0.5f + c + b;
		}
		default:
			return c * t / d + b;
		}
	}
}
