using System;
using System.Collections;
using System.Collections.Generic;
using DewInternal;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Constellations_StarDetails : SingletonBehaviour<UI_Constellations_StarDetails>
{
	public Image imgOutline;

	public Image imgFill;

	public TextMeshProUGUI progressText;

	public TextMeshProUGUI starName;

	public TextMeshProUGUI starDescription;

	public TextMeshProUGUI adjacentRequirementsText;

	public GameObject maxLevelObject;

	public CanvasGroup costGroup;

	public GameObject costObject;

	public TextMeshProUGUI costText;

	public Color canAffordColor;

	public Color insufficientColor;

	private UI_Constellations_StarItem _target;

	private void Start()
	{
		base.gameObject.SetActive(value: false);
		UI_Constellations uI_Constellations = SingletonBehaviour<UI_Constellations>.instance;
		uI_Constellations.onStateChanged = (Action)Delegate.Combine(uI_Constellations.onStateChanged, new Action(UpdateStatus));
	}

	public void Setup(UI_Constellations_StarItem item)
	{
		_target = item;
		base.transform.position = item.transform.position;
		base.gameObject.SetActive(value: true);
		UpdateStatus();
		CanvasGroup cg = GetComponent<CanvasGroup>();
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			StopAllCoroutines();
			StartCoroutine(Routine());
		}
		else
		{
			cg.alpha = 1f;
		}
		IEnumerator Routine()
		{
			cg.DOKill();
			cg.alpha = 0f;
			yield return new WaitForSecondsRealtime(0.15f);
			cg.DOFade(1f, 0.125f);
		}
	}

	private void UpdateStatus()
	{
		if (_target == null || !base.gameObject.activeInHierarchy || !SingletonBehaviour<UI_Constellations>.instance.state.stars.TryGetValue(_target.name, out var save))
		{
			return;
		}
		if (_target.maxLevel > 1)
		{
			starName.text = DewLocalization.GetStarName(_target.name) + " " + Dew.IntToRoman(Mathf.Max(1, _target.data.level));
		}
		else
		{
			starName.text = DewLocalization.GetStarName(_target.name) ?? "";
		}
		starName.color = Color.white * 0.2f + _target.color * 0.8f;
		List<LocaleNode> nodes = DewLocalization.GetStarDescription(_target.name);
		DewLocalization.DescriptionSettings settings = default(DewLocalization.DescriptionSettings);
		if (save.level == 0)
		{
			settings.currentLevel = 1;
		}
		else if (save.level < _target.maxLevel)
		{
			settings.previousLevel = save.level;
			settings.currentLevel = save.level + 1;
		}
		else
		{
			settings.currentLevel = _target.maxLevel;
		}
		starDescription.text = DewLocalization.ConvertDescriptionNodesToText(nodes, settings);
		imgOutline.sprite = _target.iconOutline.sprite;
		imgOutline.color = _target.iconOutline.color;
		imgFill.sprite = _target.iconFill.sprite;
		imgFill.color = _target.iconFill.color;
		imgFill.fillAmount = _target.iconFill.fillAmount;
		imgFill.gameObject.SetActive(_target.iconFill.gameObject.activeSelf);
		progressText.text = _target.progressText.text;
		costObject.SetActive(_target.data.level < _target.maxLevel);
		adjacentRequirementsText.gameObject.SetActive(_target.isLocked);
		if (adjacentRequirementsText.gameObject.activeSelf)
		{
			if (!Dew.IsStarIncludedInGame(_target.name))
			{
				adjacentRequirementsText.text = DewLocalization.GetUIValue("Constellations_UnavailableInDemo");
			}
			else
			{
				adjacentRequirementsText.text = string.Format(DewLocalization.GetUIValue("Constellations_AdjacentRequirements"), _target.requiredAdjacentLevels, _target.currentAdjacentLevels);
			}
		}
		if (costObject.activeSelf)
		{
			costText.text = $"{SingletonBehaviour<UI_Constellations>.instance.state.stardust}/{_target.requiredStardust:#,##0}";
			costText.color = ((_target.requiredStardust <= SingletonBehaviour<UI_Constellations>.instance.state.stardust) ? canAffordColor : insufficientColor);
			costGroup.alpha = (adjacentRequirementsText.gameObject.activeSelf ? 0.5f : 1f);
		}
		maxLevelObject.SetActive(_target.data.level >= _target.maxLevel);
	}
}
