using System;
using DG.Tweening;
using UnityEngine;

[LogicUpdatePriority(600)]
public class UI_InGame_SkillButton_GemGroup : LogicBehaviour
{
	public GameObject[] groups;

	public float expandedGemGroupScale;

	public float gemGroupAnimDuration;

	public bool enableFade;

	public bool interactableOnlyWhileEditing;

	[NonSerialized]
	public UI_InGame_GemSlot[] activeGemSlots;

	private CanvasGroup _cg;

	private float _gemGroupDefaultScale;

	private UI_InGame_SkillButton _button;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_cg.interactable = !interactableOnlyWhileEditing;
		_cg.blocksRaycasts = !interactableOnlyWhileEditing;
		_gemGroupDefaultScale = base.transform.localScale.x;
		_button = base.transform.parent.GetComponentInChildren<UI_InGame_SkillButton>();
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged, new Action<EditSkillManager.ModeType>(OnStateChanged));
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (DewPlayer.local == null || DewPlayer.local.hero == null)
		{
			return;
		}
		int gemCount = DewPlayer.local.hero.Skill.GetMaxGemCount(_button.skillType);
		for (int i = 0; i < groups.Length; i++)
		{
			bool shouldBeActive = i == gemCount - 1;
			if (shouldBeActive && !groups[i].activeSelf)
			{
				groups[i].SetActive(value: true);
				UI_InGame_GemSlot[] gemSlots = groups[i].GetComponentsInChildren<UI_InGame_GemSlot>();
				Array.Sort(gemSlots, (UI_InGame_GemSlot a, UI_InGame_GemSlot b) => a.slotIndex.CompareTo(b.slotIndex));
				activeGemSlots = gemSlots;
			}
			else if (!shouldBeActive && groups[i].activeSelf)
			{
				groups[i].SetActive(value: false);
			}
		}
	}

	private void OnStateChanged(EditSkillManager.ModeType mode)
	{
		if (mode == EditSkillManager.ModeType.None)
		{
			base.transform.DOScale(_gemGroupDefaultScale * Vector3.one, gemGroupAnimDuration).SetUpdate(isIndependentUpdate: true);
			if (enableFade)
			{
				_cg.DOFade(0f, gemGroupAnimDuration).SetUpdate(isIndependentUpdate: true);
			}
			_cg.interactable = !interactableOnlyWhileEditing;
			_cg.blocksRaycasts = !interactableOnlyWhileEditing;
		}
		else
		{
			base.transform.DOScale(expandedGemGroupScale * Vector3.one, gemGroupAnimDuration).SetUpdate(isIndependentUpdate: true);
			if (enableFade)
			{
				_cg.DOFade(1f, gemGroupAnimDuration).SetUpdate(isIndependentUpdate: true);
			}
			_cg.interactable = true;
			_cg.blocksRaycasts = true;
		}
	}
}
