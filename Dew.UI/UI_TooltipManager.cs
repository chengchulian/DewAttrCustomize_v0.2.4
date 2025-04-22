using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TooltipManager : SingletonBehaviour<UI_TooltipManager>
{
	public readonly List<object> currentObjects = new List<object>(3);

	public GameObject skillTooltip;

	public GameObject gemTooltip;

	public GameObject skillEquipTooltip;

	public GameObject gemEquipTooltip;

	public GameObject statusEffectTooltip;

	public GameObject worldNodeTooltip;

	public GameObject rawTextTooltip;

	public GameObject titleDescTooltip;

	public GameObject collectableTooltip;

	public GameObject questTooltip;

	public GameObject itemsTooltip;

	public GameObject treasureTooltip;

	public Vector2 followCursorOffset = new Vector2(0f, 25f);

	public float tooltipDelay = 0.01f;

	public float fadeInSpeed = 0.05f;

	public RectTransform safeZone;

	private CanvasGroup _cg;

	private int _tooltipUpdateRequestFrame = int.MaxValue;

	private readonly List<RaycastResult> _results = new List<RaycastResult>(64);

	private TooltipSettings _tooltipPos;

	private readonly Vector3[] _safeZoneCorners = new Vector3[4];

	private readonly Vector3[] _tooltipCorners = new Vector3[4];

	public bool isShowing { get; private set; }

	public IShowTooltip lastTarget { get; private set; }

	public void UpdateTooltip()
	{
		_tooltipUpdateRequestFrame = Time.frameCount;
	}

	private void LateUpdate()
	{
		if (ManagerBase<MessageManager>.instance.isShowingMessage && _cg.alpha > 0.5f)
		{
			Hide();
		}
		if (Time.frameCount < _tooltipUpdateRequestFrame)
		{
			return;
		}
		_tooltipUpdateRequestFrame = int.MaxValue;
		Hide();
		if (DewInput.currentMode == InputMode.Gamepad && ManagerBase<GlobalUIManager>.instance.focused != null)
		{
			if (ManagerBase<GlobalUIManager>.instance.focused is IGamepadFocusableOverrideTooltip ovt && ovt.OnUpdateTooltip())
			{
				return;
			}
			Dew.RaycastAllUIElementsBelowScreenPoint(ManagerBase<GlobalUIManager>.instance.focused.GetTransform().GetScreenSpaceRect().center, _results);
		}
		else
		{
			Dew.RaycastAllUIElementsBelowCursor(_results);
		}
		foreach (RaycastResult result in _results)
		{
			IShowTooltip t = result.gameObject.GetComponentInParent<IShowTooltip>();
			if (t != null && !(t as global::UnityEngine.Object == null))
			{
				t.ShowTooltip(this);
				if (isShowing)
				{
					lastTarget = t;
					break;
				}
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
		Hide();
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		Hide();
	}

	private void OnDestroy()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void Start()
	{
		TransitionManager transitionManager = ManagerBase<TransitionManager>.instance;
		transitionManager.onFadeIn = (Action)Delegate.Combine(transitionManager.onFadeIn, new Action(UpdateTooltip));
		TransitionManager transitionManager2 = ManagerBase<TransitionManager>.instance;
		transitionManager2.onFadeOut = (Action)Delegate.Combine(transitionManager2.onFadeOut, new Action(UpdateTooltip));
	}

	private void Update()
	{
		TooltipPositionMode mode = _tooltipPos.mode;
		if (mode != 0 && mode == TooltipPositionMode.Getter)
		{
			try
			{
				base.transform.position = ((_tooltipPos.getter != null) ? _tooltipPos.getter() : ((ManagerBase<GlobalUIManager>.instance.focused != null) ? ((Vector2)ManagerBase<GlobalUIManager>.instance.focused.GetTransform().position + followCursorOffset) : ((Vector2)Input.mousePosition + followCursorOffset))).Quantitized();
			}
			catch (Exception)
			{
			}
			ClampToSafeZone(delayed: false);
		}
	}

	private void FadeIn(float? customDelay)
	{
		_cg.DOKill();
		DOTween.Sequence().SetId(_cg).AppendInterval((customDelay ?? tooltipDelay) + 0.05f)
			.Append(_cg.DOFade(1f, fadeInSpeed))
			.SetUpdate(isIndependentUpdate: true);
	}

	private void ClampToSafeZone(bool delayed = true)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			if (delayed)
			{
				yield return null;
			}
			((RectTransform)base.transform).GetWorldCorners(_tooltipCorners);
			safeZone.GetWorldCorners(_safeZoneCorners);
			Vector3 safeLB = _safeZoneCorners[0];
			Vector3 safeRT = _safeZoneCorners[2];
			Vector3 tooltipLB = _tooltipCorners[0];
			Vector3 tooltipRT = _tooltipCorners[2];
			if (tooltipLB.x < safeLB.x)
			{
				base.transform.position += Vector3.right * (safeLB.x - tooltipLB.x);
			}
			else if (tooltipRT.x > safeRT.x)
			{
				base.transform.position += Vector3.left * (tooltipRT.x - safeRT.x);
			}
			if (tooltipLB.y < safeLB.y)
			{
				base.transform.position += Vector3.up * (safeLB.y - tooltipLB.y);
			}
			else if (tooltipRT.y > safeRT.y)
			{
				base.transform.position += Vector3.down * (tooltipRT.y - safeRT.y);
			}
		}
	}

	public void Hide()
	{
		isShowing = false;
		skillTooltip.SetActive(value: false);
		gemTooltip.SetActive(value: false);
		skillEquipTooltip.SetActive(value: false);
		gemEquipTooltip.SetActive(value: false);
		statusEffectTooltip.SetActive(value: false);
		worldNodeTooltip.SetActive(value: false);
		rawTextTooltip.SetActive(value: false);
		titleDescTooltip.SetActive(value: false);
		collectableTooltip.SetActive(value: false);
		questTooltip.SetActive(value: false);
		itemsTooltip.SetActive(value: false);
		treasureTooltip.SetActive(value: false);
		currentObjects.Clear();
		_tooltipPos = default(TooltipSettings);
		_cg.alpha = 0f;
		_cg.DOKill();
	}

	protected void Show(TooltipSettings settings)
	{
		isShowing = true;
		_tooltipPos = settings;
		FadeIn(settings.customDelay);
		RectTransform rt = (RectTransform)base.transform;
		rt.pivot = settings.pivot ?? new Vector2(0.5f, 0f);
		switch (settings.mode)
		{
		case TooltipPositionMode.RawValue:
			rt.position = settings.position;
			break;
		case TooltipPositionMode.Getter:
			rt.position = ((settings.getter == null) ? ((Vector2)Input.mousePosition + followCursorOffset) : settings.getter());
			break;
		}
		ClampToSafeZone();
	}

	public void RegisterInGameCallbacks()
	{
		EditSkillManager editSkillManager = ManagerBase<EditSkillManager>.instance;
		editSkillManager.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(editSkillManager.onModeChanged, new Action<EditSkillManager.ModeType>(OnEditSkillStateChanged));
		EditSkillManager editSkillManager2 = ManagerBase<EditSkillManager>.instance;
		editSkillManager2.onDraggingObjectChanged = (Action<global::UnityEngine.Object>)Delegate.Combine(editSkillManager2.onDraggingObjectChanged, new Action<global::UnityEngine.Object>(OnDraggingObjectChanged));
		NetworkedManagerBase<ActorManager>.instance.onLocalHeroAdd += (Action<Hero>)delegate(Hero h)
		{
			h.Skill.ClientHeroEvent_OnSkillEquip += new Action<SkillTrigger>(HandleRefreshTooltip);
			h.Skill.ClientHeroEvent_OnSkillUnequip += new Action<SkillTrigger>(HandleRefreshTooltip);
			h.Skill.ClientHeroEvent_OnGemEquip += new Action<Gem>(HandleRefreshTooltip);
			h.Skill.ClientHeroEvent_OnGemUnequip += new Action<Gem>(HandleRefreshTooltip);
		};
		NetworkedManagerBase<ActorManager>.instance.onLocalHeroRemove += (Action<Hero>)delegate(Hero h)
		{
			h.Skill.ClientHeroEvent_OnSkillEquip -= new Action<SkillTrigger>(HandleRefreshTooltip);
			h.Skill.ClientHeroEvent_OnSkillUnequip -= new Action<SkillTrigger>(HandleRefreshTooltip);
			h.Skill.ClientHeroEvent_OnGemEquip -= new Action<Gem>(HandleRefreshTooltip);
			h.Skill.ClientHeroEvent_OnGemUnequip -= new Action<Gem>(HandleRefreshTooltip);
		};
		FloatingWindowManager floatingWindowManager = ManagerBase<FloatingWindowManager>.instance;
		floatingWindowManager.onTargetChanged = (Action<MonoBehaviour>)Delegate.Combine(floatingWindowManager.onTargetChanged, (Action<MonoBehaviour>)delegate
		{
			StartCoroutine(Routine());
		});
		InGameUIManager inGameUIManager = InGameUIManager.instance;
		inGameUIManager.onStateChanged = (Action<string, string>)Delegate.Combine(inGameUIManager.onStateChanged, new Action<string, string>(OnUIStateChanged));
		ControlManager controlManager = ManagerBase<ControlManager>.instance;
		controlManager.onFocusedInteractableChanged = (Action<IInteractable>)Delegate.Combine(controlManager.onFocusedInteractableChanged, new Action<IInteractable>(HandleRefreshTooltip));
		IEnumerator Routine()
		{
			yield return null;
			UpdateTooltip();
		}
	}

	private void HandleRefreshTooltip(SkillTrigger _)
	{
		UpdateTooltip();
	}

	private void HandleRefreshTooltip(Gem _)
	{
		UpdateTooltip();
	}

	private void HandleRefreshTooltip(IInteractable _)
	{
		UpdateTooltip();
	}

	private void OnDraggingObjectChanged(global::UnityEngine.Object obj)
	{
		UpdateTooltip();
	}

	private void OnEditSkillStateChanged(EditSkillManager.ModeType mode)
	{
		if (mode == EditSkillManager.ModeType.None)
		{
			Hide();
		}
		else
		{
			UpdateTooltip();
		}
	}

	private void OnUIStateChanged(string arg1, string arg2)
	{
		if (isShowing && !InGameUIManager.instance.IsState("Playing") && !InGameUIManager.instance.IsState("Result"))
		{
			Hide();
		}
		else
		{
			UpdateTooltip();
		}
	}

	public void ShowRawTextTooltip(TooltipSettings settings, string rawText)
	{
		Hide();
		currentObjects.Add(rawText);
		rawTextTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowTitleDescTooltip(TooltipSettings settings, string titleRaw, string descRaw)
	{
		Hide();
		currentObjects.Add(titleRaw);
		currentObjects.Add(descRaw);
		titleDescTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSkillTooltip(TooltipSettings settings, SkillTrigger skill, int fromLevel, int toLevel)
	{
		Hide();
		currentObjects.Add(skill);
		currentObjects.Add(fromLevel);
		currentObjects.Add(toLevel);
		skillTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSkillTooltip(TooltipSettings settings, SkillTrigger skill, int level = -1)
	{
		Hide();
		currentObjects.Add(skill);
		if (level > 0)
		{
			currentObjects.Add(level);
		}
		skillTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSkillTooltipRaw(TooltipSettings settings, SkillTrigger skill, int level)
	{
		Hide();
		currentObjects.Add(skill);
		currentObjects.Add(level);
		currentObjects.Add("RAW");
		skillTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSkillTooltip(TooltipSettings settings, SkillTrigger skill, Hero owner)
	{
		Hide();
		currentObjects.Add(skill);
		currentObjects.Add(owner);
		skillTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSkillTooltip(TooltipSettings settings, DewGameResult result, int playerIndex, HeroSkillLocation skillType)
	{
		Hide();
		currentObjects.Add(result);
		currentObjects.Add(playerIndex);
		currentObjects.Add(skillType);
		skillTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowStatusEffectTooltip(TooltipSettings settings, StatusEffect effect)
	{
		Hide();
		currentObjects.Add(effect);
		statusEffectTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemTooltip(TooltipSettings settings, Gem gem, int fromLevel, int toLevel)
	{
		Hide();
		currentObjects.Add(gem);
		currentObjects.Add(fromLevel);
		currentObjects.Add(toLevel);
		gemTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemTooltip(TooltipSettings settings, Gem gem, int level = -1)
	{
		Hide();
		currentObjects.Add(gem);
		if (level > 0)
		{
			currentObjects.Add(level);
		}
		gemTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemTooltip(TooltipSettings settings, Gem gem, Hero owner)
	{
		Hide();
		currentObjects.Add(gem);
		currentObjects.Add(owner);
		gemTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemTooltipRaw(TooltipSettings settings, Gem gem, int level)
	{
		Hide();
		currentObjects.Add(gem);
		currentObjects.Add(level);
		currentObjects.Add("RAW");
		gemTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemTooltip(TooltipSettings settings, DewGameResult result, int playerIndex, GemLocation gemLocation)
	{
		Hide();
		currentObjects.Add(result);
		currentObjects.Add(playerIndex);
		currentObjects.Add(gemLocation);
		gemTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSkillEquipTooltip(TooltipSettings settings, SkillTrigger prevSkill, SkillTrigger newSkill)
	{
		Hide();
		currentObjects.Add(prevSkill);
		currentObjects.Add(newSkill);
		skillEquipTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemEquipTooltip(TooltipSettings settings, SkillTrigger skill, Gem prevGem, Gem newGem)
	{
		Hide();
		currentObjects.Add(skill);
		currentObjects.Add(prevGem);
		currentObjects.Add(newGem);
		gemEquipTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowWorldNodeTooltip(TooltipSettings settings, int nodeIndex)
	{
		Hide();
		currentObjects.Add(nodeIndex);
		worldNodeTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowArtifactTooltip(TooltipSettings settings, Artifact a)
	{
		ShowRawTextTooltip(settings, "<color=" + Dew.GetHex(a.mainColor) + ">" + DewLocalization.GetArtifactName(DewLocalization.GetArtifactKey(a.GetType())) + "</color>");
	}

	public void ShowNotDiscoveredObjectTooltip(TooltipSettings settings)
	{
		ShowRawTextTooltip(settings, "???\n" + DewLocalization.GetUIValue("Collectables_YouHaveNotDiscoveredThisYet"));
	}

	public void ShowCollectableTooltip(TooltipSettings settings, Type targetObjectType, CollectableTooltipSettings collectable = default(CollectableTooltipSettings))
	{
		Hide();
		currentObjects.Add(targetObjectType);
		currentObjects.Add(collectable);
		collectableTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowQuestTooltip(TooltipSettings settings, DewQuest quest)
	{
		Hide();
		currentObjects.Add(quest);
		questTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowItemsTooltip(TooltipSettings settings, string[] items)
	{
		Hide();
		currentObjects.Add(items);
		itemsTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowTreasureTooltip(TooltipSettings settings, Treasure trea, int price, string customData)
	{
		trea.price = price;
		trea.hero = DewPlayer.local.hero;
		trea.player = DewPlayer.local;
		trea.customData = customData;
		Hide();
		currentObjects.Add(trea);
		treasureTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowSouvenirTooltip(TooltipSettings settings, string accName)
	{
		ShowRawTextTooltip(settings, DewLocalization.GetUIValue(accName + "_Name"));
	}

	public void ShowSkillTooltipForDejavu(TooltipSettings settings, SkillTrigger skill)
	{
		Hide();
		currentObjects.Add(skill);
		currentObjects.Add("Dejavu");
		skillTooltip.SetActive(value: true);
		Show(settings);
	}

	public void ShowGemTooltipForDejavu(TooltipSettings settings, Gem gem)
	{
		Hide();
		currentObjects.Add(gem);
		currentObjects.Add("Dejavu");
		gemTooltip.SetActive(value: true);
		Show(settings);
	}
}
