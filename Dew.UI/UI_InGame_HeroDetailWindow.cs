using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

[LogicUpdatePriority(600)]
public class UI_InGame_HeroDetailWindow : ManagerBase<UI_InGame_HeroDetailWindow>
{
	public Transform topLeftGroup;

	public TextMeshProUGUI healthText;

	public TextMeshProUGUI adText;

	public TextMeshProUGUI apText;

	public TextMeshProUGUI skillHasteText;

	public TextMeshProUGUI attackSpeedText;

	public TextMeshProUGUI critChanceText;

	public TextMeshProUGUI armorText;

	public TextMeshProUGUI fireAmpText;

	public TextMeshProUGUI movementSpeedText;

	public UI_Toggle alwaysShowToggle;

	public Vector3 hideOffset;

	public float transitionDuration;

	public Action<bool> onVisibilityChanged;

	private bool _lastIsShown;

	private RectTransform _rt;

	private Vector3 _originalPos;

	private CanvasGroup _cg;

	public bool isShown { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		_rt = (RectTransform)base.transform;
		_cg = GetComponent<CanvasGroup>();
		_originalPos = _rt.anchoredPosition;
		_rt.anchoredPosition = _originalPos + hideOffset;
		alwaysShowToggle.onIsCheckedChanged.AddListener(delegate
		{
			UpdateVisibility();
		});
	}

	private void Start()
	{
		EditSkillManager editSkillManager = ManagerBase<EditSkillManager>.instance;
		editSkillManager.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(editSkillManager.onModeChanged, new Action<EditSkillManager.ModeType>(OnModeChanged));
		GlobalUIManager globalUIManager = ManagerBase<GlobalUIManager>.instance;
		globalUIManager.onFocusedChanged = (Action<IGamepadFocusable, IGamepadFocusable>)Delegate.Combine(globalUIManager.onFocusedChanged, new Action<IGamepadFocusable, IGamepadFocusable>(OnFocusedChanged));
		_cg.blocksRaycasts = false;
		_cg.alpha = 0f;
	}

	private void OnDestroy()
	{
		if (ManagerBase<GlobalUIManager>.instance != null)
		{
			GlobalUIManager globalUIManager = ManagerBase<GlobalUIManager>.instance;
			globalUIManager.onFocusedChanged = (Action<IGamepadFocusable, IGamepadFocusable>)Delegate.Remove(globalUIManager.onFocusedChanged, new Action<IGamepadFocusable, IGamepadFocusable>(OnFocusedChanged));
		}
	}

	private void OnFocusedChanged(IGamepadFocusable arg1, IGamepadFocusable arg2)
	{
		UpdateVisibility();
	}

	private void OnModeChanged(EditSkillManager.ModeType obj)
	{
		UpdateVisibility();
	}

	private void UpdateVisibility()
	{
		IGamepadFocusable focused = ManagerBase<GlobalUIManager>.instance.focused;
		bool newShown = ManagerBase<EditSkillManager>.instance.mode != 0 || alwaysShowToggle.isChecked || focused is UI_InGame_CurrentArtifact || (focused != null && (focused.GetTransform().IsSelfOrDescendantOf(topLeftGroup) || focused.GetTransform().IsSelfOrDescendantOf(base.transform)));
		if (isShown != newShown)
		{
			isShown = newShown;
			try
			{
				onVisibilityChanged?.Invoke(isShown);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (!(DewPlayer.local == null) && !(DewPlayer.local.hero == null))
		{
			if (isShown && !_lastIsShown)
			{
				_rt.DOKill();
				_rt.DOAnchorPos(_originalPos, transitionDuration).SetUpdate(isIndependentUpdate: true);
				_cg.DOKill();
				_cg.DOFade(1f, transitionDuration).SetUpdate(isIndependentUpdate: true);
				_cg.blocksRaycasts = true;
				_lastIsShown = isShown;
				UpdateText();
			}
			if (!isShown && _lastIsShown)
			{
				_rt.DOKill();
				_rt.DOAnchorPos(_originalPos + hideOffset, transitionDuration).SetUpdate(isIndependentUpdate: true);
				_cg.DOKill();
				_cg.DOFade(0f, transitionDuration).SetUpdate(isIndependentUpdate: true);
				_cg.blocksRaycasts = false;
				_lastIsShown = isShown;
			}
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(DewPlayer.local == null) && !(DewPlayer.local.hero == null) && isShown)
		{
			alwaysShowToggle.gameObject.SetActive(DewInput.currentMode == InputMode.KeyboardAndMouse);
			if (alwaysShowToggle.isChecked && DewInput.currentMode == InputMode.Gamepad)
			{
				alwaysShowToggle.isChecked = false;
			}
			UpdateText();
		}
	}

	private void UpdateText()
	{
		Hero h = DewPlayer.local.hero;
		EntityStatus s = h.Status;
		float atkSpd = 1f / h.Ability.attackAbility.configs[0].cooldownTime * s.attackSpeedMultiplier;
		if (h.Status.isHealthHidden)
		{
			healthText.text = $"???/{s.maxHealth:#,##0}";
		}
		else
		{
			healthText.text = $"{s.currentHealth:#,##0}/{s.maxHealth:#,##0}";
		}
		adText.text = $"{Mathf.RoundToInt(s.attackDamage):#,##0}";
		apText.text = $"{Mathf.RoundToInt(s.abilityPower):#,##0}";
		skillHasteText.text = $"{Mathf.RoundToInt(s.abilityHaste):#,##0}";
		attackSpeedText.text = $"{atkSpd:#,##0.00}";
		armorText.text = $"{Mathf.RoundToInt(h.Status.totalArmor):#,##0}";
		critChanceText.text = $"{h.Status.critChance:P0}";
		fireAmpText.text = $"{h.Status.fireEffectAmp + 1f:P0}";
		movementSpeedText.text = $"{h.Status.movementSpeedMultiplier * h.Control.baseAgentSpeed * 100f:#,##0}";
	}
}
