using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[LogicUpdatePriority(1390)]
public class UI_InGame_StatusEffectIcons_Item : LogicBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Transform tooltipPivot;

	public TextMeshProUGUI stackText;

	public Image[] icons;

	public Image durationFillIcon;

	private char[] _stackDisplay;

	public StatusEffect target { get; private set; }

	public int iconOrder
	{
		get
		{
			if (!(target == null))
			{
				return target.iconOrder;
			}
			return 0;
		}
	}

	public void Setup(StatusEffect t)
	{
		durationFillIcon.fillAmount = 1f;
		if (!t.isActive)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (!(this == null) && !(base.transform.parent == null))
		{
			target = t;
			Image[] array = icons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = target.icon;
			}
			SetSiblingIndex();
			bool showStack = target is StackedStatusEffect s && s.maxStack > 1;
			stackText.gameObject.SetActive(showStack);
			if (showStack)
			{
				_stackDisplay = new char[3];
				UpdateStacks();
			}
			NetworkedManagerBase<ClientEventManager>.instance.OnHideStatusEffectIcon += new Action<StatusEffect>(HideStatusEffectIcon);
			base.transform.DOPunchScale(Vector3.one, 0.3f);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (stackText.gameObject.activeInHierarchy)
		{
			UpdateStacks();
		}
		if (!target.IsNullOrInactive())
		{
			float? value = null;
			if (target.normalizedDuration.HasValue)
			{
				value = target.normalizedDuration.Value;
			}
			else if (target is StackedStatusEffect { autoDecay: not false } se)
			{
				value = se.remainingDecayTime / se.decayTime;
			}
			durationFillIcon.fillAmount = value ?? 1f;
		}
	}

	private void UpdateStacks()
	{
		if (_stackDisplay != null)
		{
			int s = ((StackedStatusEffect)target).stack;
			if (s > 999)
			{
				s = 999;
			}
			if (s < 10)
			{
				_stackDisplay[0] = (char)(s % 10 + 48);
				_stackDisplay[1] = '\0';
			}
			else if (s < 100)
			{
				_stackDisplay[0] = (char)(s / 10 + 48);
				_stackDisplay[1] = (char)(s % 10 + 48);
				_stackDisplay[2] = '\0';
			}
			else
			{
				_stackDisplay[0] = (char)(s / 100 + 48);
				_stackDisplay[1] = (char)(s / 10 % 10 + 48);
				_stackDisplay[2] = (char)(s % 10 + 48);
			}
			stackText.SetText(_stackDisplay);
		}
	}

	private void HideStatusEffectIcon(StatusEffect obj)
	{
		if (this != null && obj == target)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ClientEventManager>.instance != null)
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnHideStatusEffectIcon -= new Action<StatusEffect>(HideStatusEffectIcon);
		}
	}

	private void SetSiblingIndex()
	{
		List<UI_InGame_StatusEffectIcons_Item> siblings = new List<UI_InGame_StatusEffectIcons_Item>(base.transform.parent.GetComponentsInChildren<UI_InGame_StatusEffectIcons_Item>());
		siblings.Remove(this);
		for (int i = siblings.Count - 1; i >= 0; i--)
		{
			if (siblings[i] == null)
			{
				siblings.RemoveAt(i);
			}
		}
		siblings.Sort((UI_InGame_StatusEffectIcons_Item x, UI_InGame_StatusEffectIcons_Item y) => x.iconOrder.CompareTo(y.iconOrder));
		int index = siblings.FindIndex((UI_InGame_StatusEffectIcons_Item x) => x.iconOrder > iconOrder);
		if (index == -1)
		{
			base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
		}
		else
		{
			base.transform.SetSiblingIndex(index);
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (target is CurseStatusEffect c)
		{
			tooltip.ShowQuestTooltip(tooltipPivot.position, c.quest);
		}
		else
		{
			tooltip.ShowStatusEffectTooltip(tooltipPivot.position, target);
		}
	}
}
