using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_Result_HeroSkillItem : MonoBehaviour, IGameResultStatItem, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public HeroSkillLocation type;

	public GameObject[] gemObjects234;

	public Image skillIcon;

	public GameObject hasSkillObject;

	public GameObject noSkillObject;

	public GameObject multipleChargesObject;

	public TextMeshProUGUI chargeCountText;

	public TextMeshProUGUI activationKeyText;

	public Transform tooltipPivot;

	[NonSerialized]
	public DewGameResult currentData;

	[NonSerialized]
	public int currentPlayerIndex;

	public double UpdateAndGetScore(DewGameResult data, int playerIndex, float scoreMultiplier)
	{
		currentData = data;
		currentPlayerIndex = playerIndex;
		DewGameResult.PlayerData p = data.players[playerIndex];
		int maxGemCount = p.maxGemCounts[(int)type];
		for (int i = 0; i < gemObjects234.Length; i++)
		{
			gemObjects234[i].SetActive(maxGemCount == i + 2);
		}
		DewGameResult.SkillData skill;
		bool hasSkill = p.TryGetSkillData(type, out skill);
		hasSkillObject.SetActive(hasSkill);
		noSkillObject.SetActive(!hasSkill);
		if (hasSkill)
		{
			skillIcon.sprite = skill.GetSkillTrigger().configs[0].triggerIcon;
			multipleChargesObject.SetActive(skill.maxCharges > 1);
			chargeCountText.text = skill.maxCharges.ToString();
		}
		activationKeyText.text = DewInput.GetReadableTextForCurrentMode(ManagerBase<ControlManager>.instance.GetSkillBinding(type));
		return 0.0;
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowSkillTooltip(tooltipPivot.position, currentData, currentPlayerIndex, type);
	}
}
