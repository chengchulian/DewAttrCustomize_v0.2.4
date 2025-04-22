using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Spell Slot", 12)]
public class UISpellSlot : UISlotBase, IUISpellSlot, IUISlotHasCooldown
{
	[Serializable]
	public class OnAssignEvent : UnityEvent<UISpellSlot>
	{
	}

	[Serializable]
	public class OnUnassignEvent : UnityEvent<UISpellSlot>
	{
	}

	[Serializable]
	public class OnClickEvent : UnityEvent<UISpellSlot>
	{
	}

	[SerializeField]
	[Tooltip("Placing the slot in a group will make the slot accessible via the static method GetSlot.")]
	private UISpellSlot_Group m_SlotGroup;

	[SerializeField]
	private int m_ID;

	public OnAssignEvent onAssign = new OnAssignEvent();

	public OnUnassignEvent onUnassign = new OnUnassignEvent();

	public OnClickEvent onClick = new OnClickEvent();

	private UISpellInfo m_SpellInfo;

	private UISlotCooldown m_Cooldown;

	public UISpellSlot_Group slotGroup
	{
		get
		{
			return m_SlotGroup;
		}
		set
		{
			m_SlotGroup = value;
		}
	}

	public int ID
	{
		get
		{
			return m_ID;
		}
		set
		{
			m_ID = value;
		}
	}

	public UISlotCooldown cooldownComponent => m_Cooldown;

	public UISpellInfo GetSpellInfo()
	{
		return m_SpellInfo;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	public override bool IsAssigned()
	{
		return m_SpellInfo != null;
	}

	public bool Assign(UISpellInfo spellInfo)
	{
		if (spellInfo == null)
		{
			return false;
		}
		Unassign();
		Assign(spellInfo.Icon);
		m_SpellInfo = spellInfo;
		if (onAssign != null)
		{
			onAssign.Invoke(this);
		}
		if (m_Cooldown != null)
		{
			m_Cooldown.OnAssignSpell();
		}
		return true;
	}

	public override bool Assign(global::UnityEngine.Object source)
	{
		if (source is IUISpellSlot && source is IUISpellSlot sourceSlot)
		{
			return Assign(sourceSlot.GetSpellInfo());
		}
		return false;
	}

	public override void Unassign()
	{
		base.Unassign();
		m_SpellInfo = null;
		if (onUnassign != null)
		{
			onUnassign.Invoke(this);
		}
		if (m_Cooldown != null)
		{
			m_Cooldown.OnUnassignSpell();
		}
	}

	public override bool CanSwapWith(global::UnityEngine.Object target)
	{
		return target is IUISpellSlot;
	}

	public override bool PerformSlotSwap(global::UnityEngine.Object sourceObject)
	{
		IUISpellSlot obj = sourceObject as IUISpellSlot;
		UISpellInfo sourceSpellInfo = obj.GetSpellInfo();
		bool num = obj.Assign(GetSpellInfo());
		bool assign2 = Assign(sourceSpellInfo);
		return num && assign2;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (IsAssigned() && onClick != null)
		{
			onClick.Invoke(this);
		}
	}

	public override void OnTooltip(bool show)
	{
		if (m_SpellInfo != null)
		{
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				PrepareTooltip(m_SpellInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
			}
			else
			{
				UITooltip.Hide();
			}
		}
	}

	public void SetCooldownComponent(UISlotCooldown cooldown)
	{
		m_Cooldown = cooldown;
	}

	[ContextMenu("Auto Assign ID")]
	public void AutoAssignID()
	{
		List<UISpellSlot> slots = GetSlotsInGroup(m_SlotGroup);
		if (slots.Count > 0)
		{
			slots.Reverse();
			m_ID = slots[0].ID + 1;
		}
		else
		{
			m_ID = 1;
		}
		slots.Clear();
	}

	public static void PrepareTooltip(UISpellInfo spellInfo)
	{
		if (spellInfo == null)
		{
			return;
		}
		if (UITooltipManager.Instance != null)
		{
			UITooltip.SetWidth(UITooltipManager.Instance.spellTooltipWidth);
		}
		UITooltip.AddLine(spellInfo.Name, "SpellTitle");
		UITooltip.AddSpacer();
		if (spellInfo.Flags.Has(UISpellInfo_Flags.Passive))
		{
			UITooltip.AddLine("Passive", "SpellAttribute");
		}
		else
		{
			if (spellInfo.PowerCost > 0f)
			{
				if (spellInfo.Flags.Has(UISpellInfo_Flags.PowerCostInPct))
				{
					UITooltip.AddLineColumn(spellInfo.PowerCost.ToString("0") + "% Energy", "SpellAttribute");
				}
				else
				{
					UITooltip.AddLineColumn(spellInfo.PowerCost.ToString("0") + " Energy", "SpellAttribute");
				}
			}
			if (spellInfo.Range > 0f)
			{
				if (spellInfo.Range == 1f)
				{
					UITooltip.AddLineColumn("Melee range", "SpellAttribute");
				}
				else
				{
					UITooltip.AddLineColumn(spellInfo.Range.ToString("0") + " yd range", "SpellAttribute");
				}
			}
			if (spellInfo.CastTime == 0f)
			{
				UITooltip.AddLineColumn("Instant", "SpellAttribute");
			}
			else
			{
				UITooltip.AddLineColumn(spellInfo.CastTime.ToString("0.0") + " sec cast", "SpellAttribute");
			}
			if (spellInfo.Cooldown > 0f)
			{
				UITooltip.AddLineColumn(spellInfo.Cooldown.ToString("0.0") + " sec cooldown", "SpellAttribute");
			}
		}
		if (!string.IsNullOrEmpty(spellInfo.Description))
		{
			UITooltip.AddSpacer();
			UITooltip.AddLine(spellInfo.Description, "SpellDescription");
		}
	}

	public static List<UISpellSlot> GetSlots()
	{
		List<UISpellSlot> slots = new List<UISpellSlot>();
		UISpellSlot[] array = Resources.FindObjectsOfTypeAll<UISpellSlot>();
		foreach (UISpellSlot s in array)
		{
			if (s.gameObject.activeInHierarchy)
			{
				slots.Add(s);
			}
		}
		return slots;
	}

	public static List<UISpellSlot> GetSlotsWithID(int ID)
	{
		List<UISpellSlot> slots = new List<UISpellSlot>();
		UISpellSlot[] array = Resources.FindObjectsOfTypeAll<UISpellSlot>();
		foreach (UISpellSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.ID == ID)
			{
				slots.Add(s);
			}
		}
		return slots;
	}

	public static List<UISpellSlot> GetSlotsInGroup(UISpellSlot_Group group)
	{
		List<UISpellSlot> slots = new List<UISpellSlot>();
		UISpellSlot[] array = Resources.FindObjectsOfTypeAll<UISpellSlot>();
		foreach (UISpellSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.slotGroup == group)
			{
				slots.Add(s);
			}
		}
		slots.Sort((UISpellSlot a, UISpellSlot b) => a.ID.CompareTo(b.ID));
		return slots;
	}

	public static UISpellSlot GetSlot(int ID, UISpellSlot_Group group)
	{
		UISpellSlot[] array = Resources.FindObjectsOfTypeAll<UISpellSlot>();
		foreach (UISpellSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.ID == ID && s.slotGroup == group)
			{
				return s;
			}
		}
		return null;
	}
}
