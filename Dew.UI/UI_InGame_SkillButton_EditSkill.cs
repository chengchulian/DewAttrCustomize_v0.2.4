using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[LogicUpdatePriority(600)]
public class UI_InGame_SkillButton_EditSkill : LogicBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IEditSkillDropTargetSkillButton
{
	private static readonly int ShineLocation = Shader.PropertyToID("_ShineLocation");

	[FormerlySerializedAs("upgradeCostDisplay")]
	public CostDisplay costDisplay;

	public CostDisplay dreamDustCostDisplay;

	public GameObject canBeClickedObject;

	public GameObject cantBeClickedObject;

	public GameObject hoverObject;

	public GameObject equipSkillActionObject;

	public TextMeshProUGUI equipSkillActionText;

	public ButtonDisplay equipSkillActionButtonDisplay;

	public GameObject equipGemActionObject;

	public ActionDisplay equipGemActionDisplay;

	public TextMeshProUGUI equipGemActionText;

	public ButtonDisplay equipGemActionButtonDisplay;

	public GameObject gamepadSelectedObject;

	public GameObject draggingHitboxObject;

	public float fadeAlpha = 0.6f;

	public float fadeSpeed = 2f;

	private UI_InGame_SkillButton _button;

	private CanvasGroup _cg;

	private Material _matCanBeClicked;

	public HeroSkillLocation skillType => _button.skillType;

	public SkillTrigger skill => DewPlayer.local.hero.Skill.GetSkill(skillType);

	private void Awake()
	{
		GetComponent(out _cg);
		GetComponent(out _button);
		equipGemActionObject.SetActive(value: false);
		equipSkillActionObject.SetActive(value: false);
		Image img = canBeClickedObject.GetComponent<Image>();
		_matCanBeClicked = global::UnityEngine.Object.Instantiate(img.material);
		img.material = _matCanBeClicked;
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged, (Action<EditSkillManager.ModeType>)delegate
		{
			UpdateStatus();
		});
		NetworkedManagerBase<ActorManager>.instance.onLocalHeroAdd += (Action<Hero>)delegate(Hero h)
		{
			h.Skill.ClientHeroEvent_OnSkillPickup += new Action<SkillTrigger>(OnSkillPickup);
			h.Skill.ClientHeroEvent_OnGemEquip += new Action<Gem>(OnGem);
			h.Skill.ClientHeroEvent_OnGemUnequip += new Action<Gem>(OnGem);
		};
		NetworkedManagerBase<ActorManager>.instance.onLocalHeroRemove += (Action<Hero>)delegate(Hero h)
		{
			h.Skill.ClientHeroEvent_OnSkillPickup -= new Action<SkillTrigger>(OnSkillPickup);
			h.Skill.ClientHeroEvent_OnGemEquip -= new Action<Gem>(OnGem);
			h.Skill.ClientHeroEvent_OnGemUnequip -= new Action<Gem>(OnGem);
		};
	}

	private void OnGem(Gem obj)
	{
		UpdateStatus();
	}

	private void OnSkillLevelChanged(SkillTrigger arg1, int arg2, int arg3)
	{
		UpdateStatus();
	}

	private void OnSkillPickup(SkillTrigger obj)
	{
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		if (mode == EditSkillManager.ModeType.EquipGem && DewPlayer.local.hero.Skill.GetMaxGemCount(skillType) > 0)
		{
			bool isFull = DewPlayer.local.hero.Skill.GetEmptyGemSlot(skillType) < 0;
			equipGemActionDisplay.isDisabled = isFull;
			equipGemActionText.text = DewLocalization.GetUIValue(isFull ? "Interact_Gem_EquipSlotFull" : "Interact_Gem_Equip");
			equipGemActionButtonDisplay.settingsKey = "skill" + skillType.ToString() + "Edit";
			equipGemActionObject.GetComponent<Animator>().enabled = !isFull;
			equipGemActionDisplay.transform.localPosition = Vector3.zero;
			equipGemActionObject.SetActive(DewInput.currentMode == InputMode.KeyboardAndMouse);
		}
		else
		{
			equipGemActionObject.SetActive(value: false);
		}
		if (mode == EditSkillManager.ModeType.EquipSkill && DewPlayer.local.hero.Skill.CanReplaceSkill(skillType))
		{
			SkillTrigger hasSkill = DewPlayer.local.hero.Skill.GetSkill(skillType);
			equipSkillActionText.text = DewLocalization.GetUIValue(hasSkill ? "Interact_Skill_Replace" : "Interact_Skill_Equip");
			equipSkillActionButtonDisplay.settingsKey = "skill" + skillType.ToString() + "Edit";
			equipSkillActionObject.SetActive(DewInput.currentMode == InputMode.KeyboardAndMouse);
		}
		else
		{
			equipSkillActionObject.SetActive(value: false);
		}
		UpdateCostDisplayStatus();
	}

	private void UpdateCostDisplayStatus()
	{
		EditSkillManager.ModeType obj = ManagerBase<EditSkillManager>.instance.mode;
		if ((obj == EditSkillManager.ModeType.UpgradeSkill || obj == EditSkillManager.ModeType.Upgrade) && skill != null && skill.isLevelUpEnabled && ManagerBase<EditSkillManager>.instance.currentProvider is IUpgradeSkillProvider prov)
		{
			int cost = prov.GetDreamDustCost(skill);
			if (cost > 0)
			{
				costDisplay.gameObject.SetActive(value: true);
				costDisplay.SetupDreamDust(-cost);
			}
			else
			{
				costDisplay.gameObject.SetActive(value: false);
			}
			dreamDustCostDisplay.gameObject.SetActive(value: false);
		}
		else if (obj == EditSkillManager.ModeType.Sell && skill != null && DewPlayer.local.hero.Skill.CanReplaceSkill(skillType))
		{
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.SetupGold(skill.GetSellGold(DewPlayer.local), showCantAfford: false, showPlusSign: true);
			dreamDustCostDisplay.gameObject.SetActive(value: false);
		}
		else if (obj == EditSkillManager.ModeType.Cleanse && skill != null && skill.level > NetworkedManagerBase<GameManager>.instance.GetCleanseSkillMinLevel())
		{
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.SetupGold(-NetworkedManagerBase<GameManager>.instance.GetCleanseGoldCost(skill));
			dreamDustCostDisplay.gameObject.SetActive(value: false);
		}
		else
		{
			costDisplay.gameObject.SetActive(value: false);
			dreamDustCostDisplay.gameObject.SetActive(value: false);
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (!ManagerBase<ControlManager>.instance.isCharacterControlEnabled)
		{
			return;
		}
		DewBinding binding = ManagerBase<ControlManager>.instance.GetSkillBinding(skillType);
		bool isKeyDown = DewInput.GetButtonDown(binding, checkGameAreaForMouse: false);
		float targetAlpha = ((ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.UpgradeGem) ? fadeAlpha : 1f);
		_cg.alpha = Mathf.MoveTowards(_cg.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
		if (DewInput.GetButton(DewSave.profile.controls.editSkillHold, checkGameAreaForMouse: true) && isKeyDown && ManagerBase<ControlManager>.instance.shouldProcessCharacterInput)
		{
			if (ManagerBase<ControlManager>.instance.isEditSkillDisabled || (DewInput.currentMode == InputMode.KeyboardAndMouse && binding.pcBinds.Count > 0 && binding.pcBinds[0].mouse != 0))
			{
				return;
			}
			Hero hero = DewPlayer.local.hero;
			SkillTrigger skill = hero.Skill.GetSkill(skillType);
			if (skill != null && (ManagerBase<ControlManager>.instance.dropConstraint == null || ManagerBase<ControlManager>.instance.dropConstraint(skill)))
			{
				if (hero.Skill.CanReplaceSkill(skillType))
				{
					DewPlayer.local.hero.Skill.CmdUnequipSkill(skillType, DewPlayer.local.hero.position + global::UnityEngine.Random.insideUnitSphere.Flattened() * 3f);
					ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_EditSkill_SkillUnequip");
				}
				else
				{
					InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, (skillType == HeroSkillLocation.Identity) ? "InGame_Message_CantReplaceCharacterSkill" : "InGame_Message_CantReplaceLockedSkill");
				}
			}
		}
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		if ((mode == EditSkillManager.ModeType.EquipGem || mode == EditSkillManager.ModeType.EquipSkill) && DewInput.GetButtonDown(DewSave.profile.controls.GetSkillEditBinding(skillType), checkGameAreaForMouse: true))
		{
			OnPointerClick(new PointerEventData(EventSystem.current));
		}
		if (canBeClickedObject.activeSelf)
		{
			_matCanBeClicked.SetFloat(ShineLocation, Mathf.Repeat(Time.time * 3f, 8f));
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (equipSkillActionObject.activeSelf)
		{
			SkillTrigger hasSkill = DewPlayer.local.hero.Skill.GetSkill(skillType);
			equipSkillActionText.text = DewLocalization.GetUIValue((hasSkill != null) ? "Interact_Skill_Replace" : "Interact_Skill_Equip");
		}
		UpdateCostDisplayStatus();
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		canBeClickedObject.SetActive(ManagerBase<EditSkillManager>.instance.IsSlotHighlighted(skillType));
		cantBeClickedObject.gameObject.SetActive(mode != 0 && mode != EditSkillManager.ModeType.Regular && !canBeClickedObject.gameObject.activeSelf);
		gamepadSelectedObject.SetActive(DewInput.currentMode == InputMode.Gamepad && ManagerBase<EditSkillManager>.instance.selectedSkillSlot == _button.skillType);
		draggingHitboxObject.gameObject.SetActive(DewInput.currentMode == InputMode.KeyboardAndMouse && ManagerBase<EditSkillManager>.instance.draggingObject != null);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		hoverObject.SetActive(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		hoverObject.SetActive(value: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ManagerBase<EditSkillManager>.instance.DoClickOnSkillButton(skillType);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button != 0 || ManagerBase<ControlManager>.instance.isEditSkillDisabled || (!DewSave.profile.gameplay.unlockSkillsOutsideEditMode && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None))
		{
			return;
		}
		Hero hero = DewPlayer.local.hero;
		if (!(hero == null))
		{
			hero.Skill.TryGetSkill(skillType, out var skill);
			if (!(skill == null))
			{
				ManagerBase<EditSkillManager>.instance.StartDrag(skill);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		ManagerBase<EditSkillManager>.instance.EndDrag(isCancel: false);
	}
}
