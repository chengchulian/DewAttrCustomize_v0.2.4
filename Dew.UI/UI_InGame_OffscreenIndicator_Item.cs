using System;
using PixelPlay.OffScreenIndicator;
using UnityEngine;

public abstract class UI_InGame_OffscreenIndicator_Item : LogicBehaviour
{
	public Transform rotationTransform;

	[NonSerialized]
	public Vector2 screenMargin;

	private CanvasGroup _cg;

	protected virtual void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (ShouldBeDestroyed())
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Vector3 worldPos = GetTargetPosition();
		Vector3 screenPos = Dew.mainCamera.WorldToScreenPoint(worldPos);
		_cg.alpha = ((!ShouldBeHidden() && !OffScreenIndicatorCore.IsTargetVisible(screenPos)) ? 1 : 0);
		if (!(_cg.alpha < 0.1f))
		{
			float angleInRad = 0f;
			Vector3 screenCentre = new Vector3(Screen.width, Screen.height, 0f) / 2f;
			Vector3 screenBounds = screenCentre - new Vector3(screenMargin.x, screenMargin.y, 0f);
			OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPos, ref angleInRad, screenCentre, screenBounds);
			base.transform.position = screenPos;
			rotationTransform.rotation = Quaternion.Euler(0f, 0f, angleInRad * 57.29578f - 90f);
		}
	}

	protected abstract Vector3 GetTargetPosition();

	protected virtual bool ShouldBeDestroyed()
	{
		return false;
	}

	protected virtual bool ShouldBeHidden()
	{
		return false;
	}
}
