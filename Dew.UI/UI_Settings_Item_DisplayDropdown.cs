using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Settings_Item_DisplayDropdown : UI_Settings_Item
{
	protected override void OnSetup()
	{
	}

	public override void OnLoad()
	{
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (!(dropdown != null))
		{
			return;
		}
		_dropdownValues = new List<object>();
		dropdown.onValueChanged.AddListener(delegate(int b)
		{
			SetValue(_dropdownValues[b]);
		});
		List<string> displays = new List<string>();
		List<DisplayInfo> infos = new List<DisplayInfo>();
		Screen.GetDisplayLayout(infos);
		for (int i = 0; i < infos.Count; i++)
		{
			DisplayInfo r = infos[i];
			if (!displays.Contains($"{i}: {r.name}"))
			{
				displays.Add($"{i}: {r.name}");
			}
		}
		List<string> dropdownTexts = new List<string>();
		for (int j = 0; j < displays.Count; j++)
		{
			string d = displays[j];
			dropdownTexts.Add(d);
			_dropdownValues.Add(j);
		}
		dropdown.ClearOptions();
		dropdown.AddOptions(dropdownTexts);
		int val = base.parent.graphics.displayIndex;
		for (int k = 0; k < _dropdownValues.Count; k++)
		{
			if (k.Equals(val))
			{
				dropdown.value = k;
				break;
			}
		}
	}
}
