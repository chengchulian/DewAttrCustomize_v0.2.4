using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame_TimerDisplay : MonoBehaviour
{
	public UI_InGame_TimerDisplay_Item itemPrefab;

	public Transform itemParent;

	internal List<UI_InGame_TimerDisplay_Item> _instances = new List<UI_InGame_TimerDisplay_Item>();

	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		CameraManager instance = ManagerBase<CameraManager>.instance;
		instance.onIsSpectatingChanged = (Action<bool>)Delegate.Combine(instance.onIsSpectatingChanged, new Action<bool>(OnIsSpectatingChanged));
		EditSkillManager instance2 = ManagerBase<EditSkillManager>.instance;
		instance2.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance2.onModeChanged, new Action<EditSkillManager.ModeType>(OnModeChanged));
		NetworkedManagerBase<ClientEventManager>.instance.OnShowOnScreenTimer += new Action<OnScreenTimerHandle>(OnShowOnScreenTimer);
		InGameUIManager instance3 = InGameUIManager.instance;
		instance3.onWorldDisplayedChanged = (Action<WorldDisplayStatus>)Delegate.Combine(instance3.onWorldDisplayedChanged, new Action<WorldDisplayStatus>(OnWorldDisplayedChanged));
	}

	private void OnShowOnScreenTimer(OnScreenTimerHandle obj)
	{
		try
		{
			foreach (UI_InGame_TimerDisplay_Item i in _instances)
			{
				if (i == null || i._targets.Count == 0)
				{
					continue;
				}
				OnScreenTimerHandle main = i._targets[0];
				if (!(obj.color != main.color))
				{
					string obj2 = ((obj.rawTextGetter == null) ? obj.rawText : obj.rawTextGetter());
					string mainText = ((main.rawTextGetter == null) ? main.rawText : main.rawTextGetter());
					if (!(obj2 != mainText))
					{
						i.AddTarget(obj);
						return;
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).Setup(obj, this);
	}

	private void OnWorldDisplayedChanged(WorldDisplayStatus obj)
	{
		UpdateAlpha();
	}

	private void OnModeChanged(EditSkillManager.ModeType obj)
	{
		UpdateAlpha();
	}

	private void OnIsSpectatingChanged(bool obj)
	{
		UpdateAlpha();
	}

	private void UpdateAlpha()
	{
		_cg.alpha = ((!ManagerBase<CameraManager>.instance.isSpectating && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None && InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.None) ? 1 : 0);
	}
}
