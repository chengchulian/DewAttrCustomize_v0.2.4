using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[LogicUpdatePriority(3000)]
public class UI_InGame_Scoreboard_PlayerItem_Skill : LogicBehaviour, IShowTooltip, IPointerEnterHandler,
    IEventSystemHandler, IPointerExitHandler, IPingableUIElement
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

    private UI_InGame_ScoreboardView _view;

    private UI_InGame_Scoreboard_PlayerItem _item;

    public Hero hero => _item.hero;

    Object IPingableUIElement.pingTarget
    {
        get
        {
            Hero h = _item.hero;
            if (h == null || !h.isActive)
            {
                return null;
            }

            return h.Skill.GetSkill(type);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _item = GetComponentInParent<UI_InGame_Scoreboard_PlayerItem>();
        _view = GetComponentInParent<UI_InGame_ScoreboardView>();
        UpdateInfo();
    }

    private void Start()
    {
        _view.onShow.AddListener(UpdateInfo);
    }


    private void SetupDualLineLayout(Transform group, int totalSlots)
    {
        int num = Mathf.CeilToInt((float)totalSlots / 2f);
        int slotCount = totalSlots - num;
        ArrangeLine(group, num, slotCount, 100f, 30f * (1f - (float)totalSlots * 0.02f));
        ArrangeLine(group, 0, num, -20f, 30f * (1f - (float)totalSlots * 0.02f));
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

    public override void LogicUpdate(float dt)
    {
        base.LogicUpdate(dt);
        if (_view.isShowing)
        {
            UpdateInfo();
        }
    }

    private void UpdateInfo()
    {
        Hero h = _item.hero;
        if (!(h == null) && h.isActive)
        {
            var maxGemCount = h.Skill.GetMaxGemCount(type);

            if (gemObjects234.Length < maxGemCount - 1)
            {
                Array.Resize(ref gemObjects234, maxGemCount - 1);
                for (int i = 3; i < maxGemCount - 1; i++)
                {
                    int quantity = i + 2;
                    gemObjects234[i] =
                        global::UnityEngine.Object.Instantiate(gemObjects234[2], transform);
                    gemObjects234[i].name = $"{quantity} Gems";
                    var group = gemObjects234[i].transform;

                    while (group.childCount < quantity)
                    {
                        GameObject obj =
                            global::UnityEngine.Object.Instantiate(gemObjects234[2].transform.GetChild(0).gameObject,
                                group,
                                false);
                        obj.GetComponent<UI_InGame_Scoreboard_PlayerItem_Skill_Gem>().index = group.childCount - 1;
                    }

                    SetupDualLineLayout(group, quantity);
                }
            }


            for (int i = 0; i < gemObjects234.Length; i++)
            {
                gemObjects234[i].SetActive(maxGemCount == i + 2);
            }

            SkillTrigger skill = h.Skill.GetSkill(type);
            hasSkillObject.SetActive(skill != null);
            noSkillObject.SetActive(skill == null);
            if (skill != null)
            {
                skillIcon.sprite = skill.configs[0].triggerIcon;
                multipleChargesObject.SetActive(skill.configs[0].maxCharges > 1);
                chargeCountText.text = skill.configs[0].maxCharges.ToString();
            }

            activationKeyText.text =
                DewInput.GetReadableTextForCurrentMode(ManagerBase<ControlManager>.instance.GetSkillBinding(type));
        }
    }

    public void ShowTooltip(UI_TooltipManager tooltip)
    {
        SkillTrigger skill = hero.Skill.GetSkill(type);
        tooltip.ShowSkillTooltip(tooltipPivot.position, skill, hero);
    }
}