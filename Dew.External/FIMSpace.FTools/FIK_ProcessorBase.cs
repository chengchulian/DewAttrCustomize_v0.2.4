using System;
using UnityEngine;

namespace FIMSpace.FTools;

[Serializable]
public abstract class FIK_ProcessorBase
{
	[Range(0f, 1f)]
	public float IKWeight = 1f;

	public Vector3 IKTargetPosition;

	public Quaternion IKTargetRotation;

	public Vector3 LastLocalDirection;

	public Vector3 LocalDirection;

	public float fullLength { get; protected set; }

	public bool Initialized { get; protected set; }

	public FIK_IKBoneBase[] Bones { get; protected set; }

	public FIK_IKBoneBase StartBone => Bones[0];

	public FIK_IKBoneBase EndBone => Bones[Bones.Length - 1];

	public Quaternion StartBoneRotationOffset { get; set; }

	public virtual void Init(Transform root)
	{
		StartBoneRotationOffset = Quaternion.identity;
	}

	public virtual void PreCalibrate()
	{
		FIK_IKBoneBase child = Bones[0];
		while (child.Child != null)
		{
			child.transform.localRotation = child.InitialLocalRotation;
			child = child.Child;
		}
	}

	public virtual void Update()
	{
	}

	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}
}
