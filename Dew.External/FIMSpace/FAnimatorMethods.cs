using UnityEngine;

namespace FIMSpace;

public static class FAnimatorMethods
{
	public static void LerpFloatValue(this Animator animator, string name = "RunWalk", float value = 0f, float deltaSpeed = 8f)
	{
		float newValue = animator.GetFloat(name);
		newValue = Mathf.Lerp(newValue, value, Time.deltaTime * deltaSpeed);
		animator.SetFloat(name, newValue);
	}

	public static bool CheckAnimationEnd(this Animator animator, int layer = 0, bool reverse = false, bool checkAnimLoop = true)
	{
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
		if (!animator.IsInTransition(layer))
		{
			if (checkAnimLoop)
			{
				if (!info.loop && !reverse)
				{
					if (info.normalizedTime > 0.98f)
					{
						return true;
					}
					if (info.normalizedTime < 0.02f)
					{
						return true;
					}
				}
			}
			else if (!reverse)
			{
				if (info.normalizedTime > 0.98f)
				{
					return true;
				}
				if (info.normalizedTime < 0.02f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void ResetLayersWeights(this Animator animator, float speed = 10f)
	{
		for (int i = 1; i < animator.layerCount; i++)
		{
			animator.SetLayerWeight(i, animator.GetLayerWeight(i).Lerp(0f, Time.deltaTime * speed));
		}
	}

	public static void LerpLayerWeight(this Animator animator, int layer = 0, float newValue = 1f, float speed = 8f)
	{
		float newWeight = animator.GetLayerWeight(layer);
		newWeight.Lerp(newValue, Time.deltaTime * speed);
		if (newValue == 1f && newWeight > 0.999f)
		{
			newWeight = 1f;
		}
		if (newValue == 0f && newWeight < 0.01f)
		{
			newWeight = 0f;
		}
		animator.SetLayerWeight(layer, newWeight);
	}

	public static bool StateExists(this Animator animator, string clipName, int layer = 0)
	{
		int animHash = Animator.StringToHash(clipName);
		return animator.HasState(layer, animHash);
	}
}
