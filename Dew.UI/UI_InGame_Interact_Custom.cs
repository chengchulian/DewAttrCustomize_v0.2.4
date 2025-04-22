using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Interact_Custom : UI_InGame_Interact_Base
{
	public GameObject nameObject;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI interactText;

	public TextMeshProUGUI interactAltText;

	public GameObject interactAltObject;

	public CostDisplay costDisplay;

	public GameObject altProgressObject;

	public Image altProgressFill;

	private float _fillCv;

	public new ICustomInteractable interactable => (ICustomInteractable)base.interactable;

	public override Type GetSupportedType()
	{
		return typeof(ICustomInteractable);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		altProgressFill.fillAmount = 0f;
		_fillCv = 0f;
		UpdateStatus();
		string nameRaw = interactable.nameRawText;
		if (string.IsNullOrEmpty(nameRaw))
		{
			nameObject.SetActive(value: false);
			return;
		}
		nameText.text = nameRaw;
		nameObject.SetActive(value: true);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		worldOffset = interactable.worldOffset;
		interactText.text = interactable.interactActionRawText;
		interactAltObject.SetActive(interactable.canAltInteract);
		interactAltText.text = interactable.interactAltActionRawText;
		Cost cost = interactable.cost;
		if (cost.dreamDust == 0 && cost.stardust == 0 && cost.healthPercentage == 0 && cost.gold == 0)
		{
			costDisplay.gameObject.SetActive(value: false);
		}
		else
		{
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.Setup(interactable.cost);
		}
		float? progress = interactable.altInteractProgress;
		altProgressObject.SetActive(progress.HasValue);
		if (progress.HasValue)
		{
			altProgressFill.fillAmount = Mathf.SmoothDamp(altProgressFill.fillAmount, progress.Value / 0.8f, ref _fillCv, 0.1f, float.PositiveInfinity, 1f / 30f);
		}
	}
}
