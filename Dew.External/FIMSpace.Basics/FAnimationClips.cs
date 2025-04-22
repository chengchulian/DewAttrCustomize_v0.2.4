using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.Basics;

public class FAnimationClips : Dictionary<string, int>
{
	public readonly Animator Animator;

	public int Layer;

	public string CurrentAnimation { get; private set; }

	public string PreviousAnimation { get; private set; }

	public FAnimationClips(Animator animator)
	{
		Animator = animator;
		CurrentAnimation = "";
		PreviousAnimation = "";
	}

	public void Add(string clipName, bool exactClipName = false)
	{
		AddClip(clipName, exactClipName);
	}

	public void AddClip(string clipName, bool exactClipName = false)
	{
		AddClip(Animator, clipName, exactClipName);
	}

	public void AddClip(Animator animator, string clipName, bool exactClipName = false)
	{
		if (!animator)
		{
			Debug.LogError("No animator!");
			return;
		}
		string existing = "";
		if (!exactClipName)
		{
			if (animator.StateExists(clipName, Layer))
			{
				existing = clipName;
			}
			else if (animator.StateExists(clipName.CapitalizeFirstLetter()))
			{
				existing = clipName.CapitalizeFirstLetter();
			}
			else if (animator.StateExists(clipName.ToLower(), Layer))
			{
				existing = clipName.ToLower();
			}
			else if (animator.StateExists(clipName.ToUpper(), Layer))
			{
				existing = clipName.ToUpper();
			}
		}
		else if (animator.StateExists(clipName, Layer))
		{
			existing = clipName;
		}
		if (existing == "")
		{
			Debug.LogWarning("Clip with name " + clipName + " not exists in animator from game object " + animator.gameObject.name);
		}
		else if (!ContainsKey(clipName))
		{
			Add(clipName, Animator.StringToHash(existing));
		}
	}

	public void CrossFadeInFixedTime(string clip, float transitionTime = 0.25f, float timeOffset = 0f, bool startOver = false)
	{
		if (ContainsKey(clip))
		{
			RefreshClipMemory(clip);
			if (startOver)
			{
				Animator.CrossFadeInFixedTime(base[clip], transitionTime, Layer, timeOffset);
			}
			else if (!IsPlaying(clip))
			{
				Animator.CrossFadeInFixedTime(base[clip], transitionTime, Layer, timeOffset);
			}
		}
	}

	public void CrossFade(string clip, float transitionTime = 0.25f, float timeOffset = 0f, bool startOver = false)
	{
		if (ContainsKey(clip))
		{
			RefreshClipMemory(clip);
			if (startOver)
			{
				Animator.CrossFade(base[clip], transitionTime, Layer, timeOffset);
			}
			else if (!IsPlaying(clip))
			{
				Animator.CrossFade(base[clip], transitionTime, Layer, timeOffset);
			}
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
			if (Animator.GetNextAnimatorStateInfo(Layer).shortNameHash == base[clip])
			{
				return true;
			}
		}
		else if (Animator.GetCurrentAnimatorStateInfo(Layer).shortNameHash == base[clip])
		{
			return true;
		}
		return false;
	}
}
