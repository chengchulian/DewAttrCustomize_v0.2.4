using System;
using UnityEngine;

namespace FIMSpace.FTools;

[Serializable]
public abstract class FIK_IKBoneBase
{
	public float sqrMagn = 0.1f;

	public float BoneLength = 0.1f;

	public float MotionWeight = 1f;

	public Vector3 InitialLocalPosition;

	public Quaternion InitialLocalRotation;

	public Quaternion LastKeyLocalRotation;

	public FIK_IKBoneBase Child { get; private set; }

	public Transform transform { get; protected set; }

	public FIK_IKBoneBase(Transform t)
	{
		transform = t;
		if ((bool)transform)
		{
			InitialLocalPosition = transform.localPosition;
			InitialLocalRotation = transform.localRotation;
			LastKeyLocalRotation = t.localRotation;
		}
	}

	public virtual void SetChild(FIK_IKBoneBase child)
	{
		if (child != null)
		{
			Child = child;
			sqrMagn = (child.transform.position - transform.position).sqrMagnitude;
			BoneLength = (child.transform.position - transform.position).sqrMagnitude;
		}
	}
}
