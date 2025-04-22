using System;
using UnityEngine;

namespace FIMSpace.FTools;

[Serializable]
public class FIK_CCDProcessor : FIK_ProcessorBase
{
	[Serializable]
	public class CCDIKBone : FIK_IKBoneBase
	{
		[Range(0f, 180f)]
		public float AngleLimit = 45f;

		[Range(0f, 180f)]
		public float TwistAngleLimit = 5f;

		public Vector3 ForwardOrientation;

		public float FrameWorldLength = 1f;

		public Vector2 HingeLimits = Vector2.zero;

		public Quaternion PreviousHingeRotation;

		public float PreviousHingeAngle;

		public Vector3 LastIKLocPosition;

		public Quaternion LastIKLocRotation;

		public CCDIKBone IKParent { get; private set; }

		public CCDIKBone IKChild { get; private set; }

		public CCDIKBone(Transform t)
			: base(t)
		{
		}

		public void Init(CCDIKBone child, CCDIKBone parent)
		{
			LastIKLocPosition = base.transform.localPosition;
			IKParent = parent;
			if (child != null)
			{
				SetChild(child);
			}
			IKChild = child;
		}

		public override void SetChild(FIK_IKBoneBase child)
		{
			base.SetChild(child);
		}

		public void AngleLimiting()
		{
			Quaternion localRotation = Quaternion.Inverse(LastKeyLocalRotation) * base.transform.localRotation;
			Quaternion limitedRotation = localRotation;
			if (FEngineering.VIsZero(HingeLimits))
			{
				if (AngleLimit < 180f)
				{
					limitedRotation = LimitSpherical(limitedRotation);
				}
				if (TwistAngleLimit < 180f)
				{
					limitedRotation = LimitZ(limitedRotation);
				}
			}
			else
			{
				limitedRotation = LimitHinge(limitedRotation);
			}
			if (!limitedRotation.QIsSame(localRotation))
			{
				base.transform.localRotation = LastKeyLocalRotation * limitedRotation;
			}
		}

		private Quaternion LimitSpherical(Quaternion rotation)
		{
			if (rotation.QIsZero())
			{
				return rotation;
			}
			Vector3 currentForward = rotation * ForwardOrientation;
			Quaternion limitAngle = Quaternion.RotateTowards(Quaternion.identity, Quaternion.FromToRotation(ForwardOrientation, currentForward), AngleLimit);
			return Quaternion.FromToRotation(currentForward, limitAngle * ForwardOrientation) * rotation;
		}

		private Quaternion LimitZ(Quaternion currentRotation)
		{
			Vector3 orthoOrientation = new Vector3(ForwardOrientation.y, ForwardOrientation.z, ForwardOrientation.x);
			Vector3 normal = currentRotation * ForwardOrientation;
			Vector3 tangent = orthoOrientation;
			Vector3.OrthoNormalize(ref normal, ref tangent);
			orthoOrientation = currentRotation * orthoOrientation;
			Vector3.OrthoNormalize(ref normal, ref orthoOrientation);
			Quaternion limitRot = Quaternion.FromToRotation(orthoOrientation, tangent) * currentRotation;
			if (TwistAngleLimit <= 0f)
			{
				return limitRot;
			}
			return Quaternion.RotateTowards(limitRot, currentRotation, TwistAngleLimit);
		}

		private Quaternion LimitHinge(Quaternion rotation)
		{
			Quaternion addRotation = Quaternion.FromToRotation(rotation * ForwardOrientation, ForwardOrientation) * rotation * Quaternion.Inverse(PreviousHingeRotation);
			float addAngle = Quaternion.Angle(Quaternion.identity, addRotation);
			Vector3 orthoOrientation = new Vector3(ForwardOrientation.z, ForwardOrientation.x, ForwardOrientation.y);
			Vector3 cross = Vector3.Cross(orthoOrientation, ForwardOrientation);
			if (Vector3.Dot(addRotation * orthoOrientation, cross) > 0f)
			{
				addAngle = 0f - addAngle;
			}
			PreviousHingeAngle = Mathf.Clamp(PreviousHingeAngle + addAngle, HingeLimits.x, HingeLimits.y);
			PreviousHingeRotation = Quaternion.AngleAxis(PreviousHingeAngle, ForwardOrientation);
			return PreviousHingeRotation;
		}
	}

	public CCDIKBone[] IKBones;

	public bool ContinousSolving = true;

	[Range(0f, 1f)]
	public float SyncWithAnimator = 1f;

	[Range(1f, 12f)]
	public int ReactionQuality = 2;

	[Range(0f, 1f)]
	public float Smoothing;

	[Range(0f, 1.5f)]
	public float StretchToTarget;

	public AnimationCurve StretchCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public bool Use2D;

	public CCDIKBone StartIKBone => IKBones[0];

	public CCDIKBone EndIKBone => IKBones[IKBones.Length - 1];

	public float ActiveLength { get; private set; }

	public FIK_CCDProcessor(Transform[] bonesChain)
	{
		IKBones = new CCDIKBone[bonesChain.Length];
		FIK_IKBoneBase[] bones = new CCDIKBone[IKBones.Length];
		base.Bones = bones;
		for (int i = 0; i < bonesChain.Length; i++)
		{
			IKBones[i] = new CCDIKBone(bonesChain[i]);
			base.Bones[i] = IKBones[i];
		}
		IKTargetPosition = base.EndBone.transform.position;
		IKTargetRotation = base.EndBone.transform.rotation;
	}

	public override void Init(Transform root)
	{
		if (base.Initialized)
		{
			return;
		}
		base.fullLength = 0f;
		for (int i = 0; i < base.Bones.Length; i++)
		{
			CCDIKBone b = IKBones[i];
			CCDIKBone child = null;
			CCDIKBone parent = null;
			if (i > 0)
			{
				parent = IKBones[i - 1];
			}
			if (i < base.Bones.Length - 1)
			{
				child = IKBones[i + 1];
			}
			if (i < base.Bones.Length - 1)
			{
				IKBones[i].Init(child, parent);
				base.fullLength += b.BoneLength;
				b.ForwardOrientation = Quaternion.Inverse(b.transform.rotation) * (IKBones[i + 1].transform.position - b.transform.position);
			}
			else
			{
				IKBones[i].Init(child, parent);
				b.ForwardOrientation = Quaternion.Inverse(b.transform.rotation) * (IKBones[IKBones.Length - 1].transform.position - IKBones[0].transform.position);
			}
		}
		base.Initialized = true;
	}

	public override void Update()
	{
		if (!base.Initialized || IKWeight <= 0f)
		{
			return;
		}
		CCDIKBone wb = IKBones[0];
		if (ContinousSolving)
		{
			while (wb != null)
			{
				wb.LastKeyLocalRotation = wb.transform.localRotation;
				wb.transform.localPosition = wb.LastIKLocPosition;
				wb.transform.localRotation = wb.LastIKLocRotation;
				wb = wb.IKChild;
			}
		}
		else if (SyncWithAnimator > 0f)
		{
			while (wb != null)
			{
				wb.LastKeyLocalRotation = wb.transform.localRotation;
				wb = wb.IKChild;
			}
		}
		if (ReactionQuality < 0)
		{
			ReactionQuality = 1;
		}
		Vector3 goalPivotOffset = Vector3.zero;
		if (ReactionQuality > 1)
		{
			goalPivotOffset = GetGoalPivotOffset();
		}
		for (int itr = 0; itr < ReactionQuality && (itr < 1 || goalPivotOffset.sqrMagnitude != 0f || !(Smoothing > 0f) || !(GetVelocityDifference() < Smoothing * Smoothing)); itr++)
		{
			LastLocalDirection = RefreshLocalDirection();
			Vector3 ikGoal = IKTargetPosition + goalPivotOffset;
			wb = IKBones[IKBones.Length - 2];
			if (!Use2D)
			{
				while (wb != null)
				{
					float weight = wb.MotionWeight * IKWeight;
					if (weight > 0f)
					{
						Quaternion targetRotation = Quaternion.FromToRotation(base.Bones[base.Bones.Length - 1].transform.position - wb.transform.position, ikGoal - wb.transform.position) * wb.transform.rotation;
						if (weight < 1f)
						{
							wb.transform.rotation = Quaternion.Lerp(wb.transform.rotation, targetRotation, weight);
						}
						else
						{
							wb.transform.rotation = targetRotation;
						}
					}
					wb.AngleLimiting();
					wb = wb.IKParent;
				}
				continue;
			}
			while (wb != null)
			{
				float weight2 = wb.MotionWeight * IKWeight;
				if (weight2 > 0f)
				{
					Vector3 fromThisToEndChildBone = base.Bones[base.Bones.Length - 1].transform.position - wb.transform.position;
					Vector3 fromThisToIKGoal = ikGoal - wb.transform.position;
					wb.transform.rotation = Quaternion.AngleAxis(Mathf.DeltaAngle(Mathf.Atan2(fromThisToEndChildBone.x, fromThisToEndChildBone.y) * 57.29578f, Mathf.Atan2(fromThisToIKGoal.x, fromThisToIKGoal.y) * 57.29578f) * weight2, Vector3.back) * wb.transform.rotation;
				}
				wb.AngleLimiting();
				wb = wb.IKParent;
			}
		}
		LastLocalDirection = RefreshLocalDirection();
		if (StretchToTarget > 0f)
		{
			float remainingDist = (IKTargetPosition - EndIKBone.transform.position).magnitude;
			ActiveLength = Mathf.Epsilon;
			wb = IKBones[0];
			int ind = 0;
			float boneMul = Mathf.Max(1f, StretchToTarget);
			while (wb.IKChild != null && !(remainingDist <= 0f))
			{
				Vector3 toTargetN = (IKTargetPosition - wb.transform.position).normalized;
				Vector3 prePos = wb.transform.position;
				Vector3 preChildPos = wb.IKChild.transform.position;
				Vector3 norm = (preChildPos - prePos).normalized;
				float dot = Vector3.Dot(norm, toTargetN);
				if (dot > 0f)
				{
					float moveBy = wb.BoneLength * boneMul * dot;
					if (moveBy > remainingDist)
					{
						moveBy = remainingDist;
					}
					Vector3 newChildPos = preChildPos + norm * moveBy;
					wb.IKChild.transform.position = Vector3.Lerp(preChildPos, newChildPos, StretchToTarget);
					wb.transform.rotation = wb.transform.rotation * Quaternion.FromToRotation(preChildPos - prePos, wb.Child.transform.position - wb.transform.position);
					remainingDist -= Vector3.Distance(preChildPos, newChildPos);
				}
				wb = wb.IKChild;
				ind++;
			}
		}
		for (wb = IKBones[0]; wb != null; wb = wb.IKChild)
		{
			wb.LastIKLocRotation = wb.transform.localRotation;
			wb.LastIKLocPosition = wb.transform.localPosition;
			Quaternion ikDiff = wb.LastIKLocRotation * Quaternion.Inverse(wb.InitialLocalRotation);
			wb.transform.localRotation = Quaternion.Lerp(wb.LastIKLocRotation, ikDiff * wb.LastKeyLocalRotation, SyncWithAnimator);
			if (IKWeight < 1f)
			{
				wb.transform.localRotation = Quaternion.Lerp(wb.LastKeyLocalRotation, wb.transform.localRotation, IKWeight);
			}
		}
	}

	protected Vector3 GetGoalPivotOffset()
	{
		if (!GoalPivotOffsetDetected())
		{
			return Vector3.zero;
		}
		Vector3 IKDirection = (IKTargetPosition - IKBones[0].transform.position).normalized;
		Vector3 secondaryDirection = new Vector3(IKDirection.y, IKDirection.z, IKDirection.x);
		if (IKBones[IKBones.Length - 2].AngleLimit < 180f || IKBones[IKBones.Length - 2].TwistAngleLimit < 180f)
		{
			secondaryDirection = IKBones[IKBones.Length - 2].transform.rotation * IKBones[IKBones.Length - 2].ForwardOrientation;
		}
		return Vector3.Cross(IKDirection, secondaryDirection) * IKBones[IKBones.Length - 2].BoneLength * 0.5f;
	}

	private bool GoalPivotOffsetDetected()
	{
		if (!base.Initialized)
		{
			return false;
		}
		Vector3 toLastDirection = base.Bones[base.Bones.Length - 1].transform.position - base.Bones[0].transform.position;
		Vector3 toGoalDirection = IKTargetPosition - base.Bones[0].transform.position;
		float toLastMagn = toLastDirection.magnitude;
		float toGoalMagn = toGoalDirection.magnitude;
		if (toGoalMagn == 0f)
		{
			return false;
		}
		if (toLastMagn == 0f)
		{
			return false;
		}
		if (toLastMagn < toGoalMagn)
		{
			return false;
		}
		if (toLastMagn < base.fullLength - base.Bones[base.Bones.Length - 2].BoneLength * 0.1f)
		{
			return false;
		}
		if (toGoalMagn > toLastMagn)
		{
			return false;
		}
		if (Vector3.Dot(toLastDirection / toLastMagn, toGoalDirection / toGoalMagn) < 0.999f)
		{
			return false;
		}
		return true;
	}

	private Vector3 RefreshLocalDirection()
	{
		LocalDirection = base.Bones[0].transform.InverseTransformDirection(base.Bones[base.Bones.Length - 1].transform.position - base.Bones[0].transform.position);
		return LocalDirection;
	}

	private float GetVelocityDifference()
	{
		return Vector3.SqrMagnitude(LocalDirection - LastLocalDirection);
	}

	public void AutoLimitAngle(float angleLimit = 60f, float twistAngleLimit = 50f)
	{
		if (IKBones != null)
		{
			float step = 1f / (float)IKBones.Length;
			for (int i = 0; i < IKBones.Length; i++)
			{
				IKBones[i].AngleLimit = angleLimit * Mathf.Min(1f, (float)(i + 1) * step * 3f);
				IKBones[i].TwistAngleLimit = twistAngleLimit * Mathf.Min(1f, (float)(i + 1) * step * 4.5f);
			}
		}
	}

	public void AutoWeightBones(float baseValue = 1f)
	{
		float step = baseValue / ((float)base.Bones.Length * 1.3f);
		for (int i = 0; i < base.Bones.Length; i++)
		{
			base.Bones[i].MotionWeight = baseValue - step * (float)i;
		}
	}

	public void AutoWeightBones(AnimationCurve weightCurve)
	{
		for (int i = 0; i < base.Bones.Length; i++)
		{
			base.Bones[i].MotionWeight = Mathf.Clamp(weightCurve.Evaluate((float)i / (float)base.Bones.Length), 0f, 1f);
		}
	}
}
