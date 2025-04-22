using System;
using UnityEngine;

public abstract class UI_InGame_Interact_Base : LogicBehaviour
{
	public Vector3 worldOffset;

	private Quaternion _cv;

	public IInteractable interactable { get; internal set; }

	public virtual void OnDeactivate()
	{
		base.gameObject.SetActive(value: false);
	}

	public virtual void OnActivate()
	{
		base.gameObject.SetActive(value: true);
		base.transform.rotation = GetTargetRotation();
	}

	public abstract Type GetSupportedType();

	public virtual bool CanActivate(IInteractable candidate)
	{
		return GetSupportedType().IsInstanceOfType(candidate);
	}

	protected void LateUpdate()
	{
		if (interactable.IsUnityNull())
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		Quaternion targetRot = GetTargetRotation();
		base.transform.rotation = QuaternionUtil.SmoothDamp(base.transform.rotation, targetRot, ref _cv, 0.2f);
	}

	private Quaternion GetTargetRotation()
	{
		if (interactable.IsUnityNull())
		{
			return base.transform.rotation;
		}
		if (DewPlayer.local == null || DewPlayer.local.hero == null)
		{
			return base.transform.rotation;
		}
		Transform cam = Dew.mainCamera.transform;
		base.transform.position = Dew.mainCamera.WorldToScreenPoint(interactable.interactPivot.position + worldOffset);
		Vector3 delta = interactable.interactPivot.position - DewPlayer.local.hero.position;
		delta.y = 0f;
		Plane camPlane = new Plane(cam.forward.Flattened(), Vector3.zero);
		if (!camPlane.GetSide(delta))
		{
			float dist = camPlane.GetDistanceToPoint(delta);
			delta = camPlane.ClosestPointOnPlane(delta) + camPlane.normal * (dist * -1f);
		}
		if (delta.sqrMagnitude < 0.001f)
		{
			delta = Dew.mainCamera.transform.forward;
		}
		Quaternion targetRot = Quaternion.Slerp(Quaternion.identity, Quaternion.Inverse(cam.rotation) * Quaternion.LookRotation(delta, Vector3.up), 0.3f);
		return Quaternion.RotateTowards(Quaternion.identity, targetRot, 20f);
	}
}
