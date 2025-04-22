using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Settings_Item_FPSDropdown : UI_Settings_Item
{
	private static List<int> DefaultFPSValues = new List<int> { 30, 60, 144, 165, 240 };

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

	public override void OnLoad()
	{
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (!(dropdown != null))
		{
			return;
		}
		int val = (int)GetValue();
		for (int i = 0; i < _dropdownValues.Count; i++)
		{
			if (_dropdownValues[i].Equals(val))
			{
				dropdown.value = i;
				break;
			}
		}
	}

	public override void OnLanguageChanged()
	{
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		int val = dropdown.value;
		_dropdownValues = new List<object>();
		List<int> fpsValues = new List<int>(DefaultFPSValues);
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution r = resolutions[i];
			if (!fpsValues.Contains(r.refreshRate))
			{
				fpsValues.Add(r.refreshRate);
			}
		}
		fpsValues.Sort();
		fpsValues.Add(-1);
		List<string> dropdownTexts = new List<string>();
		foreach (int fps in fpsValues)
		{
			dropdownTexts.Add((fps == -1) ? DewLocalization.GetUIValue("Settings_FPSUnlimited") : fps.ToString());
			_dropdownValues.Add(fps);
		}
		dropdown.ClearOptions();
		dropdown.AddOptions(dropdownTexts);
		dropdown.value = val;
	}
}
