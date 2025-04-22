using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Settings_Item_ResolutionDropdown : UI_Settings_Item
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
		List<(int, int)> resolutions = new List<(int, int)>();
		Resolution[] resolutions2 = Screen.resolutions;
		for (int i = 0; i < resolutions2.Length; i++)
		{
			Resolution r = resolutions2[i];
			if (r.height <= r.width && r.width >= 640 && r.height >= 480 && !resolutions.Contains((r.width, r.height)))
			{
				resolutions.Add((r.width, r.height));
			}
		}
		resolutions.Sort();
		List<string> dropdownTexts = new List<string>();
		foreach (var res in resolutions)
		{
			dropdownTexts.Add($"{res.Item1}x{res.Item2}");
			_dropdownValues.Add(res);
		}
		dropdown.ClearOptions();
		dropdown.AddOptions(dropdownTexts);
		(int, int) val = (base.parent.graphics.resolutionWidth, base.parent.graphics.resolutionHeight);
		for (int j = 0; j < _dropdownValues.Count; j++)
		{
			if (_dropdownValues[j].Equals(val))
			{
				dropdown.value = j;
				break;
			}
		}
	}

	protected override void SetValue(object value)
	{
		(int, int) val = ((int, int))value;
		if (base.parent.graphics.resolutionWidth != val.Item1 || base.parent.graphics.resolutionHeight != val.Item2)
		{
			base.parent.graphics.resolutionWidth = val.Item1;
			base.parent.graphics.resolutionHeight = val.Item2;
			base.parent.MarkAsDirty();
		}
	}

	public override void LoadDefaults()
	{
		SetValue((Screen.currentResolution.width, Screen.currentResolution.height));
		OnLoad();
	}
}
