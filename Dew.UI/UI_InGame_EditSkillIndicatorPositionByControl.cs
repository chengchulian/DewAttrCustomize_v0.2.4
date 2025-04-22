using System;
using UnityEngine;

public class UI_InGame_EditSkillIndicatorPositionByControl : MonoBehaviour
{
	public Transform keyboardAndMouse;

	public Transform gamepad;

	private void Start()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged, new Action<EditSkillManager.ModeType>(OnModeChanged));
	}

	private void OnDestroy()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnModeChanged(EditSkillManager.ModeType obj)
	{
		UpdatePosition();
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		base.transform.position = ((DewInput.currentMode == InputMode.Gamepad) ? gamepad.position : keyboardAndMouse.position);
	}
}
