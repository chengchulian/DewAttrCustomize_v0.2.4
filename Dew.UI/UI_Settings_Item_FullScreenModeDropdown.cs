using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Settings_Item_FullScreenModeDropdown : UI_Settings_Item, ILangaugeChangedCallback
{
	protected override void OnSetup()
	{
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (dropdown != null)
		{
			dropdown.onValueChanged.AddListener(delegate(int b)
			{
				SetValue(_dropdownValues[b]);
			});
			OnLanguageChanged();
		}
	}

	public override void OnLanguageChanged()
	{
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		int val = dropdown.value;
		dropdown.ClearOptions();
		_dropdownValues = new List<object>();
		if (_field != null && _field.FieldType.IsEnum)
		{
			Array values = Enum.GetValues(_field.FieldType);
			List<string> list = new List<string>();
			foreach (object v in values)
			{
				if (!v.Equals(FullScreenMode.MaximizedWindow))
				{
					list.Add(DewLocalization.GetUIValue("Settings_" + _field.FieldType.Name + "_" + Enum.GetName(_field.FieldType, v)));
					_dropdownValues.Add(v);
				}
			}
			dropdown.AddOptions(list);
		}
		dropdown.value = val;
	}
}
