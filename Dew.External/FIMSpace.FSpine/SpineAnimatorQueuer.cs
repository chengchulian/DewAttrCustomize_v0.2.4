using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FSpine;

[DefaultExecutionOrder(-12)]
[AddComponentMenu("FImpossible Creations/Spine Animator Utilities/Spine Animator Queuer")]
public class SpineAnimatorQueuer : MonoBehaviour
{
	[Tooltip("Can be used to fade out all spine animators")]
	[FPD_Suffix(0f, 1f, FPD_SuffixAttribute.SuffixMode.From0to100, "%", true, 0)]
	public float SpineAnimatorsAmount = 1f;

	[SerializeField]
	internal List<FSpineAnimator> updateOrder;

	private void Update()
	{
		for (int i = updateOrder.Count - 1; i >= 0; i--)
		{
			if (updateOrder[i] == null)
			{
				updateOrder.RemoveAt(i);
			}
			else
			{
				if (updateOrder[i].enabled)
				{
					updateOrder[i].enabled = false;
				}
				updateOrder[i].Update();
			}
		}
	}

	private void FixedUpdate()
	{
		for (int i = updateOrder.Count - 1; i >= 0; i--)
		{
			if (updateOrder[i] == null)
			{
				updateOrder.RemoveAt(i);
			}
			else
			{
				if (updateOrder[i].enabled)
				{
					updateOrder[i].enabled = false;
				}
				updateOrder[i].FixedUpdate();
			}
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < updateOrder.Count; i++)
		{
			if (SpineAnimatorsAmount < 1f)
			{
				updateOrder[i].SpineAnimatorAmount = SpineAnimatorsAmount;
			}
			updateOrder[i].LateUpdate();
		}
	}
}
