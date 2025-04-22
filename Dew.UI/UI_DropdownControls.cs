using TMPro;
using UnityEngine;

[LogicUpdatePriority(100)]
public class UI_DropdownControls : LogicBehaviour
{
	private TMP_Dropdown _dropdown;

	private int _lastChildCount;

	private IGamepadFocusable _lastFocusable;

	private void Awake()
	{
		_dropdown = GetComponent<TMP_Dropdown>();
	}

	private void Start()
	{
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 100, delegate
		{
			if (IsOpen())
			{
				_dropdown.Hide();
				return true;
			}
			return false;
		});
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		int childCount = _dropdown.transform.childCount;
		if (_lastChildCount == childCount)
		{
			return;
		}
		_lastChildCount = childCount;
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			if (IsOpen())
			{
				_lastFocusable = null;
				Transform itemsParent = _dropdown.transform.GetChild(childCount - 1).GetChild(0).GetChild(0);
				_lastFocusable = ManagerBase<GlobalUIManager>.instance.focused;
				itemsParent.gameObject.AddComponent<UI_GamepadNavigationHint>();
				ManagerBase<GlobalUIManager>.instance.SetFocus(itemsParent.GetChild(_dropdown.value + 1).GetComponent<IGamepadFocusable>());
			}
			else if (_lastFocusable != null)
			{
				IGamepadFocusable f = _lastFocusable;
				_lastFocusable = null;
				ManagerBase<GlobalUIManager>.instance.SetFocus(f);
			}
		}
	}

	private bool IsOpen()
	{
		int childCount = _dropdown.transform.childCount;
		return _dropdown.transform.GetChild(childCount - 1).gameObject.activeSelf;
	}
}
