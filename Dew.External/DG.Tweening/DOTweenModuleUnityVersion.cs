using System.Threading.Tasks;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening;

public static class DOTweenModuleUnityVersion
{
	public static Sequence DOGradientColor(this Material target, Gradient gradient, float duration)
	{
		Sequence s = DOTween.Sequence();
		GradientColorKey[] colors = gradient.colorKeys;
		int len = colors.Length;
		for (int i = 0; i < len; i++)
		{
			GradientColorKey c = colors[i];
			if (i == 0 && c.time <= 0f)
			{
				target.color = c.color;
				continue;
			}
			float colorDuration = ((i == len - 1) ? (duration - s.Duration(includeLoops: false)) : (duration * ((i == 0) ? c.time : (c.time - colors[i - 1].time))));
			s.Append(target.DOColor(c.color, colorDuration).SetEase(Ease.Linear));
		}
		s.SetTarget(target);
		return s;
	}

	public static Sequence DOGradientColor(this Material target, Gradient gradient, string property, float duration)
	{
		Sequence s = DOTween.Sequence();
		GradientColorKey[] colors = gradient.colorKeys;
		int len = colors.Length;
		for (int i = 0; i < len; i++)
		{
			GradientColorKey c = colors[i];
			if (i == 0 && c.time <= 0f)
			{
				target.SetColor(property, c.color);
				continue;
			}
			float colorDuration = ((i == len - 1) ? (duration - s.Duration(includeLoops: false)) : (duration * ((i == 0) ? c.time : (c.time - colors[i - 1].time))));
			s.Append(target.DOColor(c.color, property, colorDuration).SetEase(Ease.Linear));
		}
		s.SetTarget(target);
		return s;
	}

	public static CustomYieldInstruction WaitForCompletion(this Tween t, bool returnCustomYieldInstruction)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
			return null;
		}
		return new DOTweenCYInstruction.WaitForCompletion(t);
	}

	public static CustomYieldInstruction WaitForRewind(this Tween t, bool returnCustomYieldInstruction)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
			return null;
		}
		return new DOTweenCYInstruction.WaitForRewind(t);
	}

	public static CustomYieldInstruction WaitForKill(this Tween t, bool returnCustomYieldInstruction)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
			return null;
		}
		return new DOTweenCYInstruction.WaitForKill(t);
	}

	public static CustomYieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops, bool returnCustomYieldInstruction)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
			return null;
		}
		return new DOTweenCYInstruction.WaitForElapsedLoops(t, elapsedLoops);
	}

	public static CustomYieldInstruction WaitForPosition(this Tween t, float position, bool returnCustomYieldInstruction)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
			return null;
		}
		return new DOTweenCYInstruction.WaitForPosition(t, position);
	}

	public static CustomYieldInstruction WaitForStart(this Tween t, bool returnCustomYieldInstruction)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
			return null;
		}
		return new DOTweenCYInstruction.WaitForStart(t);
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureOffset(propertyID), delegate(Vector2 x)
		{
			target.SetTextureOffset(propertyID, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureScale(propertyID), delegate(Vector2 x)
		{
			target.SetTextureScale(propertyID, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static async Task AsyncWaitForCompletion(this Tween t)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
		}
		else
		{
			while (t.active && !t.IsComplete())
			{
				await Task.Yield();
			}
		}
	}

	public static async Task AsyncWaitForRewind(this Tween t)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
		}
		else
		{
			while (t.active && (!t.playedOnce || t.position * (float)(t.CompletedLoops() + 1) > 0f))
			{
				await Task.Yield();
			}
		}
	}

	public static async Task AsyncWaitForKill(this Tween t)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
		}
		else
		{
			while (t.active)
			{
				await Task.Yield();
			}
		}
	}

	public static async Task AsyncWaitForElapsedLoops(this Tween t, int elapsedLoops)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
		}
		else
		{
			while (t.active && t.CompletedLoops() < elapsedLoops)
			{
				await Task.Yield();
			}
		}
	}

	public static async Task AsyncWaitForPosition(this Tween t, float position)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
		}
		else
		{
			while (t.active && t.position * (float)(t.CompletedLoops() + 1) < position)
			{
				await Task.Yield();
			}
		}
	}

	public static async Task AsyncWaitForStart(this Tween t)
	{
		if (!t.active)
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogInvalidTween(t);
			}
		}
		else
		{
			while (t.active && !t.playedOnce)
			{
				await Task.Yield();
			}
		}
	}
}
