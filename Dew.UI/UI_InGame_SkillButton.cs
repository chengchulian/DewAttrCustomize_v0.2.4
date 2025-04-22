using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[LogicUpdatePriority(600)]
public class UI_InGame_SkillButton : LogicBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPingableUIElement, IShowTooltip, ISettingsChangedCallback
{
	private static int instances;

	public HeroSkillLocation skillType;

	public Image icon;

	public GameObject iconPlaceHolder;

	public GameObject disabledIndicator;

	public Image cooldownFill;

	public GameObject noAbilityFrame;

	public GameObject activeAbilityFrame;

	public GameObject passiveAbilityFrame;

	public TextMeshProUGUI cooldownText;

	public GameObject multipleCharges;

	public TextMeshProUGUI chargesText;

	public Image chargesFill;

	public GameObject skillActivationKeyObject;

	public ButtonDisplay skillActivationKey;

	public Transform tooltipPivot;

	public Transform editTooltipPivot;

	public Image abilityFill;

	public GameObject ultIndicator;

	public GameObject hoverObject;

	public Material[] buttonSharedMaterials;

	public GameObject lockedObject;

	public Image specialOverlay;

	public Component[] abilityBorders;

	public Image rarityBackground;

	private Color[] _abilityBorderOriginalColors;

	private Material[] _materials;

	private CanvasGroup _canvasGroup;

	private string _lastWaitText;

	private SkillTrigger _target;

	private UI_InGame_SkillButton_GemGroup _gemGroup;

	global::UnityEngine.Object IPingableUIElement.pingTarget => _target;

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		instances = 0;
	}

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_materials = new Material[buttonSharedMaterials.Length];
		for (int i = 0; i < buttonSharedMaterials.Length; i++)
		{
			_materials[i] = global::UnityEngine.Object.Instantiate(buttonSharedMaterials[i]);
		}
		Image[] componentsInChildren = GetComponentsInChildren<Image>();
		foreach (Image img in componentsInChildren)
		{
			for (int k = 0; k < buttonSharedMaterials.Length; k++)
			{
				if (!(img.material != buttonSharedMaterials[k]))
				{
					img.material = _materials[k];
					break;
				}
			}
		}
		_abilityBorderOriginalColors = new Color[abilityBorders.Length];
		for (int l = 0; l < abilityBorders.Length; l++)
		{
			Component c = abilityBorders[l];
			if (c is Image img2)
			{
				_abilityBorderOriginalColors[l] = img2.color;
			}
			else if (c is TextMeshProUGUI ugui)
			{
				_abilityBorderOriginalColors[l] = ugui.color;
			}
		}
		instances++;
		Material mat = global::UnityEngine.Object.Instantiate(rarityBackground.material);
		rarityBackground.material = mat;
		mat.SetTextureOffset("_DistortTex", new Vector2(global::UnityEngine.Random.value, (float)instances * 0.12371f));
		rarityBackground.color = Color.clear;
		_gemGroup = base.transform.parent.GetComponentInChildren<UI_InGame_SkillButton_GemGroup>();
	}

	public UI_InGame_GemSlot GetGemSlot(int index)
	{
		return _gemGroup.activeGemSlots[index];
	}

	private void OnDestroy()
	{
		Material[] materials = _materials;
		for (int i = 0; i < materials.Length; i++)
		{
			global::UnityEngine.Object.Destroy(materials[i]);
		}
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.OnSkillSlotClientState = (Action<HeroSkillLocation, SkillTrigger>)Delegate.Combine(instance.OnSkillSlotClientState, new Action<HeroSkillLocation, SkillTrigger>(OnSkillSlotClientState));
		EditSkillManager instance2 = ManagerBase<EditSkillManager>.instance;
		instance2.onSelectedSlotChanged = (Action)Delegate.Combine(instance2.onSelectedSlotChanged, new Action(OnSelectedSlotChanged));
		NetworkedManagerBase<ClientEventManager>.instance.OnLocalHeroAbilityChanged += new Action<Hero, HeroSkillLocation>(OnLocalHeroAbilityChanged);
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		UpdateSkillActivationKey();
	}

	private void OnSelectedSlotChanged()
	{
		Dew.CallDelayed(delegate
		{
			if (ManagerBase<GlobalUIManager>.instance.focused == ManagerBase<UI_InGame_SkillButtons>.instance && ManagerBase<EditSkillManager>.instance.selectedSkillSlot == skillType && SingletonBehaviour<UI_TooltipManager>.instance != null)
			{
				ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
			}
		}, 2);
	}

	private void OnSkillSlotClientState(HeroSkillLocation type, SkillTrigger skill)
	{
		if (type == skillType)
		{
			SetTarget(skill);
		}
	}

	private void OnLocalHeroAbilityChanged(Hero hero, HeroSkillLocation type)
	{
		if (type == skillType)
		{
			SetTarget(hero.Skill.GetSkill(type));
		}
	}

	private void SetTarget(SkillTrigger newSkill)
	{
		if (_target == newSkill)
		{
			return;
		}
		if (_target != null)
		{
			_target.ClientTriggerEvent_OnCurrentConfigCharged -= new Action(FlashWeak);
			_target.ClientTriggerEvent_OnCurrentConfigCooldownReduced -= new Action(FlashVeryWeak);
			_target.ClientSkillEvent_OnLevelChange -= new Action<int, int>(FlashStrong);
		}
		if (newSkill != null)
		{
			FlashWeak();
			newSkill.ClientTriggerEvent_OnCurrentConfigCharged += new Action(FlashWeak);
			newSkill.ClientTriggerEvent_OnCurrentConfigCooldownReduced += new Action(FlashVeryWeak);
			newSkill.ClientSkillEvent_OnLevelChange += new Action<int, int>(FlashStrong);
			Color rarityColor = Dew.GetRarityColor(newSkill.rarity);
			for (int i = 0; i < abilityBorders.Length; i++)
			{
				Component c = abilityBorders[i];
				if (c is Image img)
				{
					img.color = Color.Lerp(_abilityBorderOriginalColors[i], _abilityBorderOriginalColors[i] * rarityColor, 0.35f);
				}
				else if (c is TextMeshProUGUI ugui)
				{
					ugui.color = Color.Lerp(_abilityBorderOriginalColors[i], _abilityBorderOriginalColors[i] * rarityColor, 0.25f);
				}
			}
			cooldownFill.color = rarityColor * 0.8f;
			if (newSkill.rarity == Rarity.Character || newSkill.rarity == Rarity.Identity)
			{
				rarityBackground.color = Color.clear;
			}
			else
			{
				rarityBackground.color = rarityColor;
			}
			Material[] materials = _materials;
			for (int j = 0; j < materials.Length; j++)
			{
				materials[j].SetColor("_GlowColor", Color.Lerp(Color.white, Dew.GetRarityColor(newSkill.rarity), 0.85f));
			}
		}
		else
		{
			rarityBackground.color = Color.clear;
		}
		_target = newSkill;
	}

	private string GetReadableRepresentationOfNumber(float num)
	{
		if (num > 10f)
		{
			return ((int)num).ToString();
		}
		return num.ToString("0.0");
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (_target.IsNullOrInactive() || _target == ManagerBase<EditSkillManager>.instance.draggingObject)
		{
			ShowEmpty();
			return;
		}
		bool canBeReserved = _target.CanBeReserved();
		bool isUltimate = _target.type == SkillType.Ultimate;
		float waitFillAmount = 0f;
		string waitText = "";
		if (_target.currentConfigCurrentMinimumDelay > 0f)
		{
			waitFillAmount = _target.currentConfigCurrentMinimumDelay / _target.currentConfig.minimumDelay;
			waitText = GetReadableRepresentationOfNumber(_target.currentConfigCooldownTime);
		}
		else if (_target.currentConfigCooldownTime > 0f)
		{
			if (isUltimate && _target.currentConfigIndex == 0)
			{
				waitFillAmount = 1f - _target.currentConfigCooldownTime / _target.currentConfigMaxCooldownTime;
				waitText = $"{Mathf.FloorToInt(waitFillAmount * 100f)}%";
			}
			else
			{
				waitFillAmount = _target.currentConfigCooldownTime / _target.currentConfigMaxCooldownTime;
				waitText = GetReadableRepresentationOfNumber(_target.currentConfigCooldownTime);
			}
		}
		if (_lastWaitText != "" && waitText == "")
		{
			FlashWeak();
		}
		_lastWaitText = waitText;
		bool shouldShowMultipleCharges = _target.currentConfig.maxCharges > 1 && _target.currentConfigCurrentCharge > 0;
		bool isActive = _target.currentConfig.isActive;
		bool shouldBlockRaycast = ManagerBase<ControlManager>.instance.state.type == ControlManager.ControlStateType.None;
		if (iconPlaceHolder.activeSelf)
		{
			FlashWeak();
		}
		_canvasGroup.blocksRaycasts = shouldBlockRaycast;
		icon.enabled = true;
		icon.sprite = _target.currentConfig.triggerIcon;
		iconPlaceHolder.SetActive(value: false);
		disabledIndicator.SetActive(!canBeReserved);
		if (!canBeReserved)
		{
			hoverObject.SetActive(value: false);
		}
		noAbilityFrame.SetActive(value: false);
		activeAbilityFrame.SetActive(isActive);
		passiveAbilityFrame.SetActive(!isActive);
		multipleCharges.SetActive(shouldShowMultipleCharges && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None);
		abilityFill.fillAmount = _target.fillAmount;
		specialOverlay.color = _target.specialOverlayColor;
		bool shouldShowSkillActivationKey = isActive;
		if (!shouldShowSkillActivationKey && skillActivationKeyObject.activeSelf)
		{
			skillActivationKeyObject.SetActive(value: false);
		}
		else if (shouldShowSkillActivationKey && !skillActivationKeyObject.activeSelf)
		{
			skillActivationKeyObject.SetActive(value: true);
			UpdateSkillActivationKey();
		}
		if (shouldShowMultipleCharges)
		{
			string currentChargeString = _target.currentConfigCurrentCharge.ToString();
			chargesText.text = currentChargeString;
			chargesFill.fillAmount = waitFillAmount;
			cooldownFill.fillAmount = 0f;
			cooldownText.text = "";
		}
		else
		{
			cooldownFill.fillAmount = waitFillAmount;
			cooldownText.text = waitText;
		}
		if (ultIndicator.activeSelf != isUltimate)
		{
			ultIndicator.SetActive(isUltimate);
		}
		bool isLocked = _target.owner.Ability.ShouldShowAbilityLockIcon((int)skillType);
		if (isLocked != lockedObject.activeSelf)
		{
			lockedObject.SetActive(isLocked);
			FlashWeak();
		}
	}

	public void OnSettingsChanged()
	{
		UpdateSkillActivationKey();
	}

	private void UpdateSkillActivationKey()
	{
		skillActivationKey.binding = ManagerBase<ControlManager>.instance.GetSkillBinding(skillType);
		skillActivationKey.UpdateButtonDisplay();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != 0 || ManagerBase<EditSkillManager>.instance.mode != 0 || disabledIndicator.activeSelf || !activeAbilityFrame.activeSelf)
		{
			return;
		}
		Hero hero = ManagerBase<ControlManager>.instance.controllingEntity as Hero;
		if (hero.IsNullInactiveDeadOrKnockedOut() || !hero.Skill.TryGetSkill(skillType, out var skill))
		{
			return;
		}
		if (skill.currentConfig.alwaysCastImmediately && skill.currentConfig.castMethod.type != CastMethodType.Target)
		{
			CastInfo info;
			switch (skill.currentConfig.castMethod.type)
			{
			default:
				return;
			case CastMethodType.None:
				info = new CastInfo(hero);
				break;
			case CastMethodType.Cone:
			case CastMethodType.Arrow:
				info = new CastInfo(hero, CastInfo.GetAngle(hero.transform.forward));
				break;
			case CastMethodType.Point:
				info = new CastInfo(hero, hero.position);
				break;
			case CastMethodType.Target:
				return;
			}
			_ = skill.currentConfig.postponeBasicCommand;
			ManagerBase<ControlManager>.instance.SetCastByKeyFlag(skill, value: false, DewInputTrigger.MockTrigger);
			ManagerBase<ControlManager>.instance.CastAbility(skill, info, shouldMoveToCast: false);
		}
		else
		{
			ManagerBase<ControlManager>.instance.state = new ControlManager.ControlState
			{
				type = ControlManager.ControlStateType.Cast,
				castKey = ManagerBase<ControlManager>.instance.GetSkillBinding(skillType),
				configIndex = skill.currentConfigIndex,
				trigger = skill,
				castType = CastConfirmType.Normal,
				isCastInDirectionOfMovement = ManagerBase<ControlManager>.instance.ShouldCastInDirectionOfMovement(skill)
			};
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None && !(SingletonBehaviour<UI_TooltipManager>.instance == null))
		{
			if (activeAbilityFrame.activeSelf && !disabledIndicator.activeSelf)
			{
				hoverObject.SetActive(value: true);
			}
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hoverObject.SetActive(value: false);
		if (!(SingletonBehaviour<UI_TooltipManager>.instance == null))
		{
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (DewPlayer.local == null || DewPlayer.local.hero == null)
		{
			return;
		}
		SkillTrigger currentSkill = DewPlayer.local.hero.Skill.GetSkill(skillType);
		global::UnityEngine.Object dragging = ManagerBase<EditSkillManager>.instance.draggingObject;
		if (dragging is Gem gem)
		{
			if (DewPlayer.local.hero.Skill.GetMaxGemCount(skillType) > 0)
			{
				tooltip.ShowGemEquipTooltip((Func<Vector2>)(() => editTooltipPivot.position), currentSkill, null, gem);
			}
			return;
		}
		if (dragging is SkillTrigger skill)
		{
			if (DewPlayer.local.hero.Skill.CanReplaceSkill(skillType))
			{
				tooltip.ShowSkillEquipTooltip((Func<Vector2>)(() => editTooltipPivot.position), currentSkill, skill);
			}
			return;
		}
		if (currentSkill != null)
		{
			EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
			if (mode == EditSkillManager.ModeType.Upgrade || mode == EditSkillManager.ModeType.UpgradeSkill)
			{
				if (ManagerBase<EditSkillManager>.instance.currentProvider is IUpgradeSkillProvider prov)
				{
					tooltip.ShowSkillTooltip((Func<Vector2>)(() => editTooltipPivot.position), currentSkill, currentSkill.level, currentSkill.level + prov.GetAddedLevel());
				}
				else
				{
					tooltip.ShowSkillTooltip((Func<Vector2>)(() => editTooltipPivot.position), currentSkill, currentSkill.level, currentSkill.level + 1);
				}
				return;
			}
		}
		if (currentSkill != null && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.Cleanse && currentSkill.level > NetworkedManagerBase<GameManager>.instance.GetCleanseSkillMinLevel())
		{
			tooltip.ShowSkillTooltip((Func<Vector2>)(() => editTooltipPivot.position), currentSkill, currentSkill.level, NetworkedManagerBase<GameManager>.instance.GetCleanseSkillMinLevel());
		}
		else
		{
			tooltip.ShowSkillTooltip((Func<Vector2>)(() => editTooltipPivot.position), currentSkill);
		}
	}

	protected virtual void ShowEmpty()
	{
		_canvasGroup.blocksRaycasts = true;
		icon.enabled = false;
		iconPlaceHolder.SetActive(value: true);
		disabledIndicator.SetActive(value: false);
		cooldownFill.fillAmount = 0f;
		noAbilityFrame.SetActive(value: true);
		activeAbilityFrame.SetActive(value: false);
		passiveAbilityFrame.SetActive(value: false);
		cooldownText.text = "";
		multipleCharges.SetActive(value: false);
		skillActivationKeyObject.SetActive(value: false);
		abilityFill.fillAmount = 0f;
		ultIndicator.SetActive(value: false);
		lockedObject.SetActive(value: false);
		specialOverlay.color = Color.clear;
	}

	public void FlashStrong(int _, int __)
	{
		FlashStrong();
	}

	public void FlashStrong()
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			obj.SetFloat("_Glow", 80f);
			obj.SetFloat("_ShineLocation", 1f);
			obj.SetFloat("_ShineGlow", 1f);
			DOTween.Kill(obj);
			obj.DOFloat(0f, "_Glow", 0.85f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, "_ShineLocation", 0.4f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, "_ShineGlow", 0.4f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void FlashWeak()
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			obj.SetFloat("_Glow", 20f);
			obj.SetFloat("_ShineLocation", 0f);
			obj.SetFloat("_ShineGlow", 1f);
			DOTween.Kill(obj);
			obj.DOFloat(0f, "_Glow", 0.45f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(1f, "_ShineLocation", 0.35f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, "_ShineGlow", 0.35f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void FlashVeryWeak()
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			obj.SetFloat("_Glow", 10f);
			obj.SetFloat("_ShineLocation", 0f);
			obj.SetFloat("_ShineGlow", 0.75f);
			DOTween.Kill(obj);
			obj.DOFloat(0f, "_Glow", 0.35f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(1f, "_ShineLocation", 0.25f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, "_ShineGlow", 0.25f).SetUpdate(isIndependentUpdate: true);
		}
	}
}
