using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[LogicUpdatePriority(600)]
public class UI_InGame_GemSlot : LogicBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IShowTooltip, IPingableUIElement
{
	private static readonly int HsvSaturation = Shader.PropertyToID("_HsvSaturation");

	private static readonly int HsvBright = Shader.PropertyToID("_HsvBright");

	private static readonly int Glow = Shader.PropertyToID("_Glow");

	private static readonly int ShineLocation = Shader.PropertyToID("_ShineLocation");

	private static readonly int ShineGlow = Shader.PropertyToID("_ShineGlow");

	public int slotIndex;

	public float frameAlphaSpeed = 4f;

	public CanvasGroup normalFrame;

	public CanvasGroup editingFrame;

	public Image gemIconImage;

	public UI_InGame_SkillButton button;

	public Transform tooltipPivot;

	public Image cooldownFill;

	public float emptyAlpha = 0.3f;

	public float emptyEditingAlpha = 0.8f;

	public float iconScale = 1.25f;

	public float editingIconScale = 1f;

	public float iconScaleSpeed = 4f;

	public TextMeshProUGUI numberDisplay;

	public GameObject hoverObject;

	public Material[] buttonSharedMaterials;

	private Material[] _materials;

	private Gem _target;

	private CanvasGroup _cg;

	public GemLocation thisSlotLocation => new GemLocation(button.skillType, slotIndex);

	public float desiredAlpha { get; private set; }

	global::UnityEngine.Object IPingableUIElement.pingTarget => _target;

	private void Awake()
	{
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
		_cg = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.OnGemSlotClientState = (Action<GemLocation, Gem>)Delegate.Combine(instance.OnGemSlotClientState, new Action<GemLocation, Gem>(OnGemSlotClientState));
		EditSkillManager instance2 = ManagerBase<EditSkillManager>.instance;
		instance2.onSelectedSlotChanged = (Action)Delegate.Combine(instance2.onSelectedSlotChanged, new Action(OnSelectedSlotChanged));
		NetworkedManagerBase<ClientEventManager>.instance.OnLocalHeroGemChanged += new Action<Hero, GemLocation>(OnLocalHeroGemChanged);
	}

	private void OnSelectedSlotChanged()
	{
		GemLocation? selectedGemSlot = ManagerBase<EditSkillManager>.instance.selectedGemSlot;
		GemLocation gemLocation = thisSlotLocation;
		if (selectedGemSlot.HasValue && (!selectedGemSlot.HasValue || selectedGemSlot.GetValueOrDefault() == gemLocation) && SingletonBehaviour<UI_TooltipManager>.instance != null)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			yield return null;
			ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
		}
	}

	private void OnLocalHeroGemChanged(Hero hero, GemLocation loc)
	{
		if (loc.skill == button.skillType && loc.index == slotIndex)
		{
			hero.Skill.gems.TryGetValue(loc, out var newTarget);
			SetTarget(newTarget);
		}
	}

	private void OnGemSlotClientState(GemLocation loc, Gem gem)
	{
		if (loc.skill == button.skillType && loc.index == slotIndex)
		{
			SetTarget(gem);
		}
	}

	private void SetTarget(Gem newGem)
	{
		if (!(_target == newGem))
		{
			if (_target != null)
			{
				_target.ClientGemEvent_OnCooldownReady -= new Action(FlashWeak);
				_target.ClientGemEvent_OnCooldownReduced -= new Action<float>(FlashVeryWeak);
				_target.ClientGemEvent_OnCooldownReducedByRatio -= new Action<float>(FlashVeryWeak);
				_target.ClientGemEvent_OnFlash -= new Action(FlashWeak);
				_target.ClientGemEvent_OnQualityChanged -= new Action<int, int>(FlashStrong);
			}
			if (newGem != null)
			{
				FlashWeak();
				newGem.ClientGemEvent_OnCooldownReady += new Action(FlashWeak);
				newGem.ClientGemEvent_OnCooldownReduced += new Action<float>(FlashVeryWeak);
				newGem.ClientGemEvent_OnCooldownReducedByRatio += new Action<float>(FlashVeryWeak);
				newGem.ClientGemEvent_OnFlash += new Action(FlashWeak);
				newGem.ClientGemEvent_OnQualityChanged += new Action<int, int>(FlashStrong);
			}
			_target = newGem;
		}
	}

	private void OnDestroy()
	{
		Material[] materials = _materials;
		for (int i = 0; i < materials.Length; i++)
		{
			global::UnityEngine.Object.Destroy(materials[i]);
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		bool isEditing = ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.None;
		normalFrame.alpha = Mathf.MoveTowards(normalFrame.alpha, (!isEditing) ? 1 : 0, frameAlphaSpeed * Time.unscaledDeltaTime);
		editingFrame.alpha = Mathf.MoveTowards(editingFrame.alpha, isEditing ? 1 : 0, frameAlphaSpeed * Time.unscaledDeltaTime);
		gemIconImage.transform.localScale = Mathf.MoveTowards(gemIconImage.transform.localScale.x, isEditing ? editingIconScale : iconScale, Time.unscaledDeltaTime * iconScaleSpeed) * Vector3.one;
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_target == null || _target == ManagerBase<EditSkillManager>.instance.draggingObject)
		{
			ShowEmpty();
			return;
		}
		desiredAlpha = 1f;
		SetReady(_target.IsReady());
		cooldownFill.fillAmount = (_target.isCooldownEnabled ? (_target.currentCooldown / _target.maxCooldown) : 0f);
		gemIconImage.gameObject.SetActive(value: true);
		gemIconImage.sprite = _target.icon;
		numberDisplay.enabled = _target.numberDisplay.HasValue;
		if (_target.numberDisplay.HasValue)
		{
			numberDisplay.text = Dew.FormatBigNumbers(_target.numberDisplay.Value, 1000f, "#,##0.#");
		}
	}

	private void ShowEmpty()
	{
		gemIconImage.gameObject.SetActive(value: false);
		numberDisplay.enabled = false;
		cooldownFill.fillAmount = 0f;
		SetReady(isReady: true);
		desiredAlpha = ((ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None) ? emptyAlpha : emptyEditingAlpha);
		bool isEditing = ManagerBase<EditSkillManager>.instance.mode != EditSkillManager.ModeType.None;
		normalFrame.alpha = Mathf.MoveTowards(normalFrame.alpha, (!isEditing) ? 1 : 0, frameAlphaSpeed * (1f / 30f));
		editingFrame.alpha = Mathf.MoveTowards(editingFrame.alpha, isEditing ? 1 : 0, frameAlphaSpeed * (1f / 30f));
	}

	public void SetReady(bool isReady)
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			obj.SetFloat(HsvSaturation, isReady ? 1f : 0.25f);
			obj.SetFloat(HsvBright, isReady ? 1f : 0.5f);
		}
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
			obj.SetFloat(Glow, 80f);
			obj.SetFloat(ShineLocation, 1f);
			obj.SetFloat(ShineGlow, 1f);
			DOTween.Kill(obj);
			obj.DOFloat(0f, Glow, 0.65f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, ShineLocation, 0.4f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, ShineGlow, 0.4f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void FlashWeak()
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			obj.SetFloat(Glow, 20f);
			obj.SetFloat(ShineLocation, 0f);
			obj.SetFloat(ShineGlow, 1f);
			DOTween.Kill(obj);
			obj.DOFloat(0f, Glow, 0.35f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(1f, ShineLocation, 0.35f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, ShineGlow, 0.35f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void FlashVeryWeak(float _)
	{
		FlashVeryWeak();
	}

	public void FlashVeryWeak()
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			obj.SetFloat(Glow, 10f);
			obj.SetFloat(ShineLocation, 0f);
			obj.SetFloat(ShineGlow, 0.75f);
			DOTween.Kill(obj);
			obj.DOFloat(0f, Glow, 0.25f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(1f, ShineLocation, 0.25f).SetUpdate(isIndependentUpdate: true);
			obj.DOFloat(0f, ShineGlow, 0.25f).SetUpdate(isIndependentUpdate: true);
		}
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

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (DewPlayer.local == null || DewPlayer.local.hero == null)
		{
			return;
		}
		global::UnityEngine.Object draggingObject = ManagerBase<EditSkillManager>.instance.draggingObject;
		DewPlayer.local.hero.Skill.gems.TryGetValue(new GemLocation(button.skillType, slotIndex), out var prevGem);
		if (draggingObject is Gem gem)
		{
			SkillTrigger prevSkill = DewPlayer.local.hero.Skill.GetSkill(button.skillType);
			tooltip.ShowGemEquipTooltip((Func<Vector2>)(() => tooltipPivot.position), prevSkill, prevGem, gem);
			return;
		}
		if (prevGem != null)
		{
			EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
			if (mode == EditSkillManager.ModeType.Upgrade || mode == EditSkillManager.ModeType.UpgradeGem)
			{
				if (ManagerBase<EditSkillManager>.instance.currentProvider is IUpgradeGemProvider prov)
				{
					tooltip.ShowGemTooltip((Func<Vector2>)(() => tooltipPivot.position), prevGem, prevGem.quality, prevGem.quality + prov.GetAddedQuality());
				}
				else
				{
					tooltip.ShowGemTooltip((Func<Vector2>)(() => tooltipPivot.position), prevGem, prevGem.quality, prevGem.quality + NetworkedManagerBase<GameManager>.instance.GetGemUpgradeAddedQuality());
				}
				return;
			}
		}
		if (prevGem != null && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.Cleanse && prevGem.quality > NetworkedManagerBase<GameManager>.instance.GetCleanseGemMinQuality())
		{
			tooltip.ShowGemTooltip((Func<Vector2>)(() => tooltipPivot.position), prevGem, prevGem.quality, NetworkedManagerBase<GameManager>.instance.GetCleanseGemMinQuality());
		}
		else
		{
			tooltip.ShowGemTooltip((Func<Vector2>)(() => tooltipPivot.position), prevGem);
		}
	}
}
