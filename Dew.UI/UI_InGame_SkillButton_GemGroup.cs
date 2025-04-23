using System;
using System.Collections;
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

    [NonSerialized] public UI_InGame_GemSlot[] activeGemSlots;

    private CanvasGroup _cg;

    private float _gemGroupDefaultScale;

    private UI_InGame_SkillButton _button;

    private bool _isAnimating;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        _cg.interactable = !interactableOnlyWhileEditing;
        _cg.blocksRaycasts = !interactableOnlyWhileEditing;
        _gemGroupDefaultScale = base.transform.localScale.x;
        _button = base.transform.parent.GetComponentInChildren<UI_InGame_SkillButton>();
    }

    private void UpdateActiveSlots(GameObject activeGroup)
    {
        UI_InGame_GemSlot[] componentsInChildren = activeGroup.GetComponentsInChildren<UI_InGame_GemSlot>();
        Array.Sort(componentsInChildren,
            (UI_InGame_GemSlot a, UI_InGame_GemSlot b) => a.slotIndex.CompareTo(b.slotIndex));
        activeGemSlots = componentsInChildren;
    }

    private string GetEnglishByNum(int number)
    {
        return number switch
        {
            1 => "One",
            2 => "Two",
            3 => "Three",
            4 => "Four",
            5 => "Five",
            6 => "Six",
            7 => "Seven",
            8 => "Eight",
            9 => "Nine",
            10 => "Ten",
            11 => "Eleven",
            12 => "Twelve",
            13 => "Thirteen",
            14 => "Fourteen",
            15 => "Fifteen",
            16 => "Sixteen",
            17 => "Seventeen",
            18 => "Eighteen",
            19 => "Nineteen",
            20 => "Twenty",
            _ => number.ToString() ?? "",
        };
    }

    private void Start()
    {
        EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
        instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged,
            new Action<EditSkillManager.ModeType>(OnStateChanged));
        StartCoroutine(waitHeroLoadEndCoroutine());
    }

    private IEnumerator waitHeroLoadEndCoroutine()
    {
        int maxGemCount = -1;

        while (maxGemCount < 0)
        {
            yield return new WaitForSeconds(0.5f);
            try
            {
                maxGemCount = DewPlayer.local.hero.Skill.GetMaxGemCount(_button.skillType);
            }
            catch (Exception)
            {
                maxGemCount = -1;
            }
        }
        

        if (groups.Length < maxGemCount)
        {
            Array.Resize(ref groups, maxGemCount);
            for (int i = 4; i < maxGemCount; i++)
            {
                groups[i] = global::UnityEngine.Object.Instantiate(groups[3], base.transform);
                groups[i].name = string.Format(GetEnglishByNum(i + 1));
                Transform group = groups[i].transform;
                int num = i + 1;
                while (group.childCount < num)
                {
                    GameObject obj =
                        global::UnityEngine.Object.Instantiate(groups[0].transform.GetChild(0).gameObject, group,
                            false);
                    obj.GetComponent<UI_InGame_GemSlot>().slotIndex = group.childCount - 1;
                }

                SetupDualLineLayout(group, num);
            }

            var rectTransform = this.transform as RectTransform;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = new Vector2(0, 0);
            rectTransform.offsetMin = new Vector2(0, 0);
        }

        for (int j = 0; j < groups.Length; j++)
        {
            bool flag = j == maxGemCount - 1;
            groups[j].SetActive(flag);
            if (flag)
            {
                UpdateActiveSlots(groups[j]);
            }
        }
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
            base.transform.DOScale(_gemGroupDefaultScale * Vector3.one, gemGroupAnimDuration)
                .SetUpdate(isIndependentUpdate: true);
            if (enableFade)
            {
                _cg.DOFade(0f, gemGroupAnimDuration).SetUpdate(isIndependentUpdate: true);
            }

            _cg.interactable = !interactableOnlyWhileEditing;
            _cg.blocksRaycasts = !interactableOnlyWhileEditing;
            if (!_isAnimating)
            {
                return;
            }
            if (groups.Length > 4)
            {
                SmoothIncreaseOfSpacing(groups[^1]);
            }
            _isAnimating = false;
        }
        else
        {
            base.transform.DOScale(expandedGemGroupScale * Vector3.one, gemGroupAnimDuration)
                .SetUpdate(isIndependentUpdate: true);
            if (enableFade)
            {
                _cg.DOFade(1f, gemGroupAnimDuration).SetUpdate(isIndependentUpdate: true);
            }

            _cg.interactable = true;
            _cg.blocksRaycasts = true;
            if (_isAnimating)
            {
                return;
            }

            if (groups.Length > 4)
            {
                SmoothReductionOfSpacing(groups[^1]);
            }
            _isAnimating = true;
        }
    }

    private void SmoothIncreaseOfSpacing(GameObject group)
    {
        int totalSlots = group.transform.childCount;
        int num = Mathf.CeilToInt((float)totalSlots / 2f);
        int slotCount = totalSlots - num;
        SmoothMove(group, 0, num, -20);
        SmoothMove(group, num, totalSlots, 20);

    }

    private void SmoothReductionOfSpacing(GameObject group)
    {
        int totalSlots = group.transform.childCount;
        int num = Mathf.CeilToInt((float)totalSlots / 2f);
        int slotCount = totalSlots - num;
        SmoothMove(group, 0, num, 20);
        SmoothMove(group, num, totalSlots, -20);

    }

    private void SmoothMove(GameObject group, int startIndex, int endIndex, int value)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            var child = group.transform.GetChild(i) as RectTransform;
            child.DOKill(complete: true);
            child.DOAnchorPosY(child.anchoredPosition.y + value, gemGroupAnimDuration)
                .SetUpdate(isIndependentUpdate: true);
        }
    }

    private void SetupDualLineLayout(Transform group, int totalSlots)
    {
        int num = Mathf.CeilToInt((float)totalSlots / 2f);
        int slotCount = totalSlots - num;
        ArrangeLine(group, num, slotCount, 100f, 50f * (1f - (float)totalSlots * 0.02f));
        ArrangeLine(group, 0, num, -100f, 50f * (1f - (float)totalSlots * 0.02f));
    }

    private void ArrangeLine(Transform group, int startIndex, int slotCount, float yPos, float spacing)
    {
        if (slotCount <= 0)
        {
            return;
        }

        float num = (0f - (float)(slotCount - 1)) * spacing / 2f;
        for (int i = 0; i < slotCount; i++)
        {
            int num2 = startIndex + i;
            if (num2 < group.childCount)
            {
                group.GetChild(num2).localPosition = new Vector3(num + (float)i * spacing, yPos, 0f);
                continue;
            }

            break;
        }
    }
}