using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_SkillNotifications : LogicBehaviour
{
	public GameObject skillObjects;

	public GameObject gemObjects;

	public GameObject cancelObjects;

	public UI_InGame_Flasher flasher;

	public Image rarityImage;

	public Image skillImage;

	public Image gemImage;

	public float sustainTime = 3f;

	public float decayTime = 0.5f;

	public float mouseOverAlpha = 0.3f;

	public float defaultAlpha = 1f;

	public float mouseOverChangeSpeed = 4f;

	private CanvasGroup _canvasGroup;

	private float _currentElapsedTime = float.PositiveInfinity;

	private SkillTrigger _lastEditingSkill;

	public float currentAlpha => _canvasGroup.alpha;

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0f;
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged, new Action<EditSkillManager.ModeType>(OnStateChanged));
		NetworkedManagerBase<ActorManager>.instance.onLocalHeroAdd += (Action<Hero>)delegate(Hero h)
		{
			h.Skill.ClientHeroEvent_OnSkillPickup += new Action<SkillTrigger>(HandleSkillPickup);
			h.Skill.ClientHeroEvent_OnGemQualityChanged += new Action<Gem, int, int>(ShowGemUpgrade);
			h.Skill.ClientHeroEvent_OnSkillLevelChanged += new Action<SkillTrigger, int, int>(ShowSkillUpgrade);
		};
		NetworkedManagerBase<ActorManager>.instance.onLocalHeroRemove += (Action<Hero>)delegate(Hero h)
		{
			h.Skill.ClientHeroEvent_OnSkillPickup -= new Action<SkillTrigger>(HandleSkillPickup);
			h.Skill.ClientHeroEvent_OnGemQualityChanged -= new Action<Gem, int, int>(ShowGemUpgrade);
			h.Skill.ClientHeroEvent_OnSkillLevelChanged -= new Action<SkillTrigger, int, int>(ShowSkillUpgrade);
		};
	}

	private void ShowSkillUpgrade(SkillTrigger target, int arg2, int arg3)
	{
		if (arg2 < arg3)
		{
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_EditSkill_UpgradeSkill");
		}
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		if (mode == EditSkillManager.ModeType.None || mode == EditSkillManager.ModeType.Regular || mode == EditSkillManager.ModeType.UpgradeSkill || mode == EditSkillManager.ModeType.Upgrade || mode == EditSkillManager.ModeType.Cleanse)
		{
			gemObjects.gameObject.SetActive(value: false);
			skillObjects.gameObject.SetActive(value: true);
			flasher.Flash();
			_canvasGroup.alpha = defaultAlpha;
			_currentElapsedTime = 0f;
			UI_Tooltip_BaseObj[] objs = skillObjects.GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
			object[] data = new object[3] { target, arg2, arg3 };
			UI_Tooltip_BaseObj[] array = objs;
			foreach (UI_Tooltip_BaseObj obj in array)
			{
				obj.currentObjects = data;
				obj.OnSetup();
			}
			skillImage.gameObject.SetActive(value: true);
			gemImage.gameObject.SetActive(value: false);
			skillImage.sprite = target.configs[0].triggerIcon;
			rarityImage.color = Dew.GetRarityColor(target.rarity);
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
			if (ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.Upgrade && ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.UpgradeSkill && ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.Cleanse && arg2 <= arg3)
			{
				string msg = DewLocalization.GetUIValue("Chat_SkillUpgraded");
				string typeName = target.GetType().Name;
				msg = string.Format(msg, ChatManager.GetColoredSkillName(typeName, arg2) + " <sprite=0> " + ChatManager.GetColoredSkillName(typeName, arg3));
				msg = "<color=#a7bfc4>" + msg + "</color>";
				NetworkedManagerBase<ChatManager>.instance.ShowMessageLocally(new ChatManager.Message
				{
					type = ChatManager.MessageType.Raw,
					content = msg
				});
			}
		}
	}

	private void ShowGemUpgrade(Gem target, int arg2, int arg3)
	{
		if (arg2 < arg3)
		{
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_EditSkill_UpgradeGem");
		}
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		if (mode == EditSkillManager.ModeType.None || mode == EditSkillManager.ModeType.Regular || mode == EditSkillManager.ModeType.UpgradeGem || mode == EditSkillManager.ModeType.Upgrade || mode == EditSkillManager.ModeType.Cleanse)
		{
			skillObjects.gameObject.SetActive(value: false);
			gemObjects.gameObject.SetActive(value: true);
			flasher.Flash();
			_canvasGroup.alpha = defaultAlpha;
			_currentElapsedTime = 0f;
			UI_Tooltip_BaseObj[] tooltipObjects = gemObjects.GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
			object[] objs = new object[3] { target, arg2, arg3 };
			UI_Tooltip_BaseObj[] array = tooltipObjects;
			foreach (UI_Tooltip_BaseObj obj in array)
			{
				obj.currentObjects = objs;
				obj.OnSetup();
			}
			skillImage.gameObject.SetActive(value: false);
			gemImage.gameObject.SetActive(value: true);
			gemImage.sprite = target.icon;
			rarityImage.color = Dew.GetRarityColor(target.rarity);
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
			if (ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.Upgrade && ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.UpgradeGem && ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.Cleanse && arg2 <= arg3)
			{
				string msg = DewLocalization.GetUIValue("Chat_GemUpgraded");
				string typeName = target.GetType().Name;
				msg = string.Format(msg, ChatManager.GetColoredGemName(typeName, arg2) + " <sprite=0> " + ChatManager.GetColoredGemName(typeName, arg3));
				msg = "<color=#a7bfc4>" + msg + "</color>";
				NetworkedManagerBase<ChatManager>.instance.ShowMessageLocally(new ChatManager.Message
				{
					type = ChatManager.MessageType.Raw,
					content = msg
				});
			}
		}
	}

	private void OnStateChanged(EditSkillManager.ModeType mode)
	{
		switch (mode)
		{
		case EditSkillManager.ModeType.EquipGem:
			ShowGemNotification((Gem)DewPlayer.local.hero.Skill.holdingObject);
			break;
		case EditSkillManager.ModeType.EquipSkill:
			ShowSkillNotification((SkillTrigger)DewPlayer.local.hero.Skill.holdingObject, isFromPickup: false);
			_lastEditingSkill = (SkillTrigger)DewPlayer.local.hero.Skill.holdingObject;
			break;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		cancelObjects.gameObject.SetActive(ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.EquipGem);
		GameObject target = null;
		if (skillObjects.activeSelf)
		{
			target = skillObjects;
		}
		else if (gemObjects.activeSelf)
		{
			target = gemObjects;
		}
		if (_currentElapsedTime <= sustainTime)
		{
			Rect rect = ((RectTransform)base.transform).GetScreenSpaceRect();
			float expandedX = rect.width * 0.2f;
			float expandedY = rect.height * 0.4f;
			rect.xMin -= expandedX;
			rect.xMax += expandedX;
			rect.yMin -= expandedY;
			rect.yMax += expandedY;
			float targetAlpha = ((rect.Contains(Input.mousePosition) && DewPlayer.local != null && !DewPlayer.local.hero.IsNullInactiveDeadOrKnockedOut() && DewPlayer.local.hero.isInCombat && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None) ? mouseOverAlpha : defaultAlpha);
			_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, targetAlpha, Time.deltaTime * mouseOverChangeSpeed);
		}
		if (!(target != null))
		{
			return;
		}
		_currentElapsedTime += Time.deltaTime;
		if (ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.EquipGem && _currentElapsedTime > sustainTime)
		{
			_currentElapsedTime = sustainTime;
		}
		if (ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.EquipSkill && _currentElapsedTime > sustainTime)
		{
			_currentElapsedTime = sustainTime;
		}
		if (_currentElapsedTime > sustainTime)
		{
			_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0f, Time.deltaTime / decayTime);
			if (_canvasGroup.alpha < 0.001f)
			{
				_canvasGroup.alpha = 0f;
				target.SetActive(value: false);
			}
		}
	}

	private void HandleSkillPickup(SkillTrigger newSkill)
	{
		ShowSkillNotification(newSkill, isFromPickup: true);
	}

	private void ShowSkillNotification(SkillTrigger newSkill, bool isFromPickup)
	{
		if (isFromPickup)
		{
			if (newSkill == _lastEditingSkill)
			{
				_lastEditingSkill = null;
				return;
			}
			EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
			if (mode != 0 && mode != EditSkillManager.ModeType.Regular)
			{
				return;
			}
		}
		gemObjects.gameObject.SetActive(value: false);
		skillObjects.gameObject.SetActive(value: true);
		flasher.Flash();
		_canvasGroup.alpha = 1f;
		_currentElapsedTime = 0f;
		UI_Tooltip_BaseObj[] componentsInChildren = skillObjects.GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
		foreach (UI_Tooltip_BaseObj o in componentsInChildren)
		{
			o.currentObjects = new object[1] { newSkill };
			o.OnSetup();
		}
		skillImage.gameObject.SetActive(value: true);
		gemImage.gameObject.SetActive(value: false);
		skillImage.sprite = newSkill.configs[0].triggerIcon;
		rarityImage.color = Dew.GetRarityColor(newSkill.rarity);
	}

	private void ShowGemNotification(Gem gem)
	{
		skillObjects.gameObject.SetActive(value: false);
		gemObjects.gameObject.SetActive(value: true);
		flasher.Flash();
		_canvasGroup.alpha = 1f;
		_currentElapsedTime = 0f;
		UI_Tooltip_BaseObj[] componentsInChildren = gemObjects.GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
		foreach (UI_Tooltip_BaseObj o in componentsInChildren)
		{
			o.currentObjects = new object[1] { gem };
			o.OnSetup();
		}
		skillImage.gameObject.SetActive(value: false);
		gemImage.gameObject.SetActive(value: true);
		gemImage.sprite = gem.icon;
		rarityImage.color = Dew.GetRarityColor(gem.rarity);
	}
}
