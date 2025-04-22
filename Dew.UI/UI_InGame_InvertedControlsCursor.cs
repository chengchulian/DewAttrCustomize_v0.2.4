using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UI_InGame_InvertedControlsCursor : LogicBehaviour
{
	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (DewInput.currentMode == InputMode.KeyboardAndMouse && ControlManager.AreControlsInverted() && ManagerBase<FloatingWindowManager>.instance.currentTarget == null && InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.None)
		{
			_cg.alpha = 1f;
		}
		else
		{
			_cg.alpha = 0f;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (!(_cg.alpha < 0.1f) && !(Time.timeScale < 0.0001f))
		{
			base.transform.position = ControlManager.GetMousePositionWithInversionInMind();
			base.transform.rotation = Quaternion.Euler(0f, 0f, base.transform.rotation.eulerAngles.z + 60f * Time.deltaTime);
		}
	}
}
