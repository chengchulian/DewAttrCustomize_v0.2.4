using System;
using TMPro;
using UnityEngine;

public class UI_InGame_SpectateIndicator : LogicBehaviour
{
	public TextMeshProUGUI spectatingText;

	private CanvasGroup _cg;

	private void Awake()
	{
		GetComponent(out _cg);
	}

	private void Start()
	{
		CameraManager instance = ManagerBase<CameraManager>.instance;
		instance.onIsSpectatingChanged = (Action<bool>)Delegate.Combine(instance.onIsSpectatingChanged, new Action<bool>(OnIsSpectatingChanged));
		InGameUIManager instance2 = InGameUIManager.instance;
		instance2.onWorldDisplayedChanged = (Action<WorldDisplayStatus>)Delegate.Combine(instance2.onWorldDisplayedChanged, new Action<WorldDisplayStatus>(OnWorldDisplayedChanged));
		UpdateAlpha();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(_cg.alpha <= 0.1f))
		{
			if (ManagerBase<CameraManager>.instance.focusedEntity == null)
			{
				spectatingText.text = null;
				return;
			}
			string targetName = ((!(ManagerBase<CameraManager>.instance.focusedEntity.owner != null)) ? DewLocalization.GetUIValue(ManagerBase<CameraManager>.instance.focusedEntity.GetType().Name + "_Name") : ManagerBase<CameraManager>.instance.focusedEntity.owner.playerName);
			spectatingText.text = string.Format(DewLocalization.GetUIValue("InGame_Spectate_Spectating"), targetName);
		}
	}

	private void OnIsSpectatingChanged(bool obj)
	{
		UpdateAlpha();
	}

	private void OnWorldDisplayedChanged(WorldDisplayStatus obj)
	{
		UpdateAlpha();
	}

	private void UpdateAlpha()
	{
		_cg.alpha = ((ManagerBase<CameraManager>.instance.isSpectating && InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.None) ? 1f : 0f);
	}
}
