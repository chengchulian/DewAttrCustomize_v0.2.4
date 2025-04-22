using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[LogicUpdatePriority(600)]
public class UI_InGame_GemSlot_EditSkill : LogicBehaviour, IPointerClickHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IEditSkillDropTargetGemSlot
{
	private static readonly int ShineLocation = Shader.PropertyToID("_ShineLocation");

	[FormerlySerializedAs("upgradeCostDisplay")]
	public CostDisplay costDisplay;

	public CostDisplay dreamDustCostDisplay;

	public float fadeAlpha = 0.6f;

	public float fadeSpeed = 2f;

	public GameObject canBeClickedObject;

	public GameObject cantBeClickedObject;

	public GameObject gamepadSelectedObject;

	private CanvasGroup _cg;

	private UI_InGame_GemSlot _slot;

	private Material _matCanBeClicked;

	public GemLocation location => new GemLocation(_button.skillType, _slot.slotIndex);

	private UI_InGame_SkillButton _button => _slot.button;

	private Gem _gem => DewPlayer.local.hero.Skill.GetGem(location);

	private void Awake()
	{
		GetComponent(out _slot);
		GetComponent(out _cg);
		Image img = canBeClickedObject.GetComponent<Image>();
		_matCanBeClicked = global::UnityEngine.Object.Instantiate(img.material);
		img.material = _matCanBeClicked;
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged, new Action<EditSkillManager.ModeType>(OnModeChanged));
	}

	private void OnModeChanged(EditSkillManager.ModeType obj)
	{
		UpdateCostDisplayStatus();
	}

	private void UpdateCostDisplayStatus()
	{
		EditSkillManager.ModeType obj = ManagerBase<EditSkillManager>.instance.mode;
		if ((obj == EditSkillManager.ModeType.UpgradeGem || obj == EditSkillManager.ModeType.Upgrade) && _gem != null && ManagerBase<EditSkillManager>.instance.currentProvider is IUpgradeGemProvider prov)
		{
			int cost = prov.GetDreamDustCost(_gem);
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
		else if (obj == EditSkillManager.ModeType.Sell && _gem != null)
		{
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.SetupGold(_gem.GetSellGold(DewPlayer.local), showCantAfford: false, showPlusSign: true);
			dreamDustCostDisplay.gameObject.SetActive(value: false);
		}
		else if (obj == EditSkillManager.ModeType.Cleanse && _gem != null && _gem.quality > NetworkedManagerBase<GameManager>.instance.GetCleanseGemMinQuality())
		{
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.SetupGold(-NetworkedManagerBase<GameManager>.instance.GetCleanseGoldCost(_gem));
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
		float targetAlpha = ((ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.UpgradeSkill) ? fadeAlpha : 1f);
		_cg.alpha = Mathf.MoveTowards(_cg.alpha, targetAlpha * _slot.desiredAlpha, fadeSpeed * Time.deltaTime);
		if (canBeClickedObject.activeSelf)
		{
			_matCanBeClicked.SetFloat(ShineLocation, Mathf.Repeat(Time.time * 3f, 8f));
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateCostDisplayStatus();
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		canBeClickedObject.SetActive((mode != EditSkillManager.ModeType.Regular || ManagerBase<EditSkillManager>.instance.draggingObject != null) && ManagerBase<EditSkillManager>.instance.IsSlotHighlighted(location));
		cantBeClickedObject.gameObject.SetActive(mode != 0 && mode != EditSkillManager.ModeType.Regular && !canBeClickedObject.gameObject.activeSelf);
		gamepadSelectedObject.SetActive(DewInput.currentMode == InputMode.Gamepad && ManagerBase<EditSkillManager>.instance.selectedGemSlot == location);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ManagerBase<EditSkillManager>.instance.DoClickOnGemSlot(location);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && !ManagerBase<ControlManager>.instance.isEditSkillDisabled && (DewSave.profile.gameplay.unlockSkillsOutsideEditMode || ManagerBase<EditSkillManager>.instance.mode != 0) && _gem != null)
		{
			ManagerBase<EditSkillManager>.instance.StartDrag(_gem);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		ManagerBase<EditSkillManager>.instance.EndDrag(isCancel: false);
	}
}
