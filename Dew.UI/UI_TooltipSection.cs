using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_TooltipSection : MonoBehaviour
{
	private float _lastUpdateTime;

	public IReadOnlyList<object> currentObjects => SingletonBehaviour<UI_TooltipManager>.instance.currentObjects;

	protected virtual void OnEnable()
	{
		SetupObjects();
	}

	private void Update()
	{
		if (Time.unscaledTime - _lastUpdateTime > 0.25f)
		{
			SetupObjects();
		}
		if (DewInput.GetButtonDown(DewSave.profile.controls.showDetails, checkGameAreaForMouse: false) || DewInput.GetButtonUp(DewSave.profile.controls.showDetails, checkGameAreaForMouse: false))
		{
			SetupObjects();
		}
	}

	private void SetupObjects()
	{
		_lastUpdateTime = Time.unscaledTime;
		UI_Tooltip_BaseObj[] componentsInChildren = GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
		foreach (UI_Tooltip_BaseObj o in componentsInChildren)
		{
			try
			{
				o.currentObjects = currentObjects;
				o.OnSetup();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, o);
			}
		}
	}
}
