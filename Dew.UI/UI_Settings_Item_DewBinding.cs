using System;
using UnityEngine;

public class UI_Settings_Item_DewBinding : UI_Settings_Item
{
	public bool isGamepad;

	public GameObject keyboardIcon;

	public GameObject mouseIcon;

	public GameObject pcBindObject;

	public GameObject gamepadBindObject;

	protected override void OnSetup()
	{
		base.OnSetup();
		UI_Settings_Item_DewBinding_BindItem[] componentsInChildren = GetComponentsInChildren<UI_Settings_Item_DewBinding_BindItem>();
		foreach (UI_Settings_Item_DewBinding_BindItem obj in componentsInChildren)
		{
			obj.onValueChanged = (Action<object>)Delegate.Combine(obj.onValueChanged, (Action<object>)delegate(object k)
			{
				SetValue(k);
				RefreshBindItems((DewBinding)k);
			});
		}
		DewBinding val = (DewBinding)_field.GetValue(DewSave.profile.controls);
		pcBindObject.SetActive(!isGamepad);
		gamepadBindObject.SetActive(isGamepad);
		keyboardIcon.SetActive(!isGamepad && val.canAssignKeyboard);
		mouseIcon.SetActive(!isGamepad && val.canAssignMouse);
	}

	public override void OnLoad()
	{
		base.OnLoad();
		DewBinding val = (DewBinding)GetValue();
		RefreshBindItems(val);
	}

	public void ClearBinding()
	{
		DewBinding newBinding = (DewBinding)((DewBinding)GetValue()).Clone();
		if (isGamepad)
		{
			newBinding.gamepadBinds.Clear();
		}
		else
		{
			newBinding.pcBinds.Clear();
		}
		RefreshBindItems(newBinding);
		SetValue(newBinding);
	}

	private void RefreshBindItems(DewBinding newBinding)
	{
		UI_Settings_Item_DewBinding_BindItem[] componentsInChildren = GetComponentsInChildren<UI_Settings_Item_DewBinding_BindItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].UpdateValueDontNotify(newBinding);
		}
	}

	public override void LoadDefaults()
	{
		DewControlSettings defaultSettings = new DewControlSettings();
		DewBinding defaultValue = (DewBinding)_field.GetValue(defaultSettings);
		DewBinding newValue = (DewBinding)((DewBinding)GetValue()).Clone();
		if (isGamepad)
		{
			newValue.gamepadBinds = defaultValue.gamepadBinds;
		}
		else
		{
			newValue.pcBinds = defaultValue.pcBinds;
		}
		SetValue(newValue);
		OnLoad();
	}
}
