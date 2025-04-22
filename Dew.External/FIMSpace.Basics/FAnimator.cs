using UnityEngine;

namespace FIMSpace.Basics;

public class FAnimator
{
	public readonly Animator Animator;

	public string CurrentAnimation { get; private set; }

	public string PreviousAnimation { get; private set; }

	public int Layer { get; private set; }

	public FAnimator(Animator animator, int layer = 0)
	{
		Animator = animator;
		CurrentAnimation = "";
		PreviousAnimation = "";
		Layer = layer;
	}

	public bool ContainsClip(string clipName, bool exactClipName = false)
	{
		if (!Animator)
		{
			Debug.LogError("No animator!");
			return false;
		}
		string existing = "";
		if (!exactClipName)
		{
			if (Animator.StateExists(clipName, Layer))
			{
				existing = clipName;
			}
			else if (Animator.StateExists(clipName.CapitalizeFirstLetter()))
			{
				existing = clipName.CapitalizeFirstLetter();
			}
			else if (Animator.StateExists(clipName.ToLower(), Layer))
			{
				existing = clipName.ToLower();
			}
			else if (Animator.StateExists(clipName.ToUpper(), Layer))
			{
				existing = clipName.ToUpper();
			}
		}
		else if (Animator.StateExists(clipName, Layer))
		{
			existing = clipName;
		}
		if (existing == "")
		{
			Debug.LogWarning("Clip with name " + clipName + " not exists in animator from game object " + Animator.gameObject.name);
			return false;
		}
		return true;
	}

	public void CrossFadeInFixedTime(string clip, float transitionTime = 0.25f, float timeOffset = 0f, bool startOver = false)
	{
		RefreshClipMemory(clip);
		if (startOver)
		{
			Animator.CrossFadeInFixedTime(clip, transitionTime, Layer, timeOffset);
		}
		else if (!IsPlaying(clip))
		{
			Animator.CrossFadeInFixedTime(clip, transitionTime, Layer, timeOffset);
		}
	}

	public void CrossFade(string clip, float transitionTime = 0.25f, float timeOffset = 0f, bool startOver = false)
	{
		RefreshClipMemory(clip);
		if (startOver)
		{
			Animator.CrossFade(clip, transitionTime, Layer, timeOffset);
		}
		else if (!IsPlaying(clip))
		{
			Animator.CrossFade(clip, transitionTime, Layer, timeOffset);
		}
	}

	private void RefreshClipMemory(string name)
	{
		if (name != CurrentAnimation)
		{
			PreviousAnimation = CurrentAnimation;
			CurrentAnimation = name;
		}
	}

	public void SetFloat(string parameter, float value = 0f, float deltaSpeed = 60f)
	{
		float newValue = Animator.GetFloat(parameter);
		newValue = ((!(deltaSpeed >= 60f)) ? FLogicMethods.FLerp(newValue, value, Time.deltaTime * deltaSpeed) : value);
		Animator.SetFloat(parameter, newValue);
	}

	public void SetFloatUnscaledDelta(string parameter, float value = 0f, float deltaSpeed = 60f)
	{
		float newValue = Animator.GetFloat(parameter);
		newValue = ((!(deltaSpeed >= 60f)) ? FLogicMethods.FLerp(newValue, value, Time.unscaledDeltaTime * deltaSpeed) : value);
		Animator.SetFloat(parameter, newValue);
	}

	internal bool IsPlaying(string clip)
	{
		if (Animator.IsInTransition(Layer))
		{
			if (Animator.GetNextAnimatorStateInfo(Layer).shortNameHash == Animator.StringToHash(clip))
			{
				return true;
			}
		}
		else if (Animator.GetCurrentAnimatorStateInfo(Layer).shortNameHash == Animator.StringToHash(clip))
		{
			return true;
		}
		return false;
	}
}
