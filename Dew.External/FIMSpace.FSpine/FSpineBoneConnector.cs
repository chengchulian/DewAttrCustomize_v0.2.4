using UnityEngine;
using UnityEngine.EventSystems;

namespace FIMSpace.FSpine;

[AddComponentMenu("FImpossible Creations/Spine Animator Utilities/FSpine Bone Connector")]
public class FSpineBoneConnector : MonoBehaviour, IDropHandler, IEventSystemHandler, IFHierarchyIcon
{
	[Space(5f)]
	public FSpineAnimator TargetSpineAnimator;

	public Transform TargetBone;

	[Space(3f)]
	[FPD_Width(130)]
	public bool PositionAnimated;

	[FPD_Width(130)]
	public bool RotationAnimated = true;

	[Space(3f)]
	public Vector3 RotationCorrection;

	public bool Mirror;

	private Vector3 animatorStatePosition;

	private Quaternion animatorStateRotation;

	private Quaternion targetBoneStateRotation;

	public string EditorIconPath => "Spine Animator/Spine Bone Connector Icon";

	public void OnDrop(PointerEventData data)
	{
	}

	private void Start()
	{
		if (!TargetBone)
		{
			Debug.LogError("No target bone in " + base.name + " for Spine Bone Connector Component!");
			Object.Destroy(this);
		}
		if ((bool)TargetSpineAnimator)
		{
			TargetSpineAnimator.AddConnector(this);
		}
		else
		{
			Debug.LogError("No target SpineAnimator in " + base.name + " for Spine Bone Connector Component!");
			Object.Destroy(this);
		}
		if (!PositionAnimated)
		{
			animatorStatePosition = TargetBone.InverseTransformVector(base.transform.position - TargetBone.position);
		}
		if (!RotationAnimated)
		{
			animatorStateRotation = base.transform.localRotation;
		}
		targetBoneStateRotation = TargetBone.localRotation;
	}

	internal void RememberAnimatorState()
	{
		if (PositionAnimated)
		{
			animatorStatePosition = TargetBone.InverseTransformVector(base.transform.position - TargetBone.position);
		}
		if (RotationAnimated)
		{
			animatorStateRotation = base.transform.localRotation;
		}
	}

	internal void RefreshAnimatorState()
	{
		if (base.enabled)
		{
			base.transform.position = TargetBone.position + TargetBone.TransformVector(animatorStatePosition);
			base.transform.rotation = TargetBone.rotation * (animatorStateRotation * (Mirror ? targetBoneStateRotation : Quaternion.Inverse(targetBoneStateRotation))) * Quaternion.Euler(RotationCorrection);
		}
	}
}
