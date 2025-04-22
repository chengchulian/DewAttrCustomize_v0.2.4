using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Item Slot", 12)]
public class UIItemSlot : UISlotBase, IUIItemSlot
{
	[Serializable]
	public class OnRightClickEvent : UnityEvent<UIItemSlot>
	{
	}

	[Serializable]
	public class OnDoubleClickEvent : UnityEvent<UIItemSlot>
	{
	}

	[Serializable]
	public class OnAssignEvent : UnityEvent<UIItemSlot>
	{
	}

	[Serializable]
	public class OnAssignWithSourceEvent : UnityEvent<UIItemSlot, global::UnityEngine.Object>
	{
	}

	[Serializable]
	public class OnUnassignEvent : UnityEvent<UIItemSlot>
	{
	}

	[SerializeField]
	private UIItemSlot_Group m_SlotGroup;

	[SerializeField]
	private int m_ID;

	private UIItemInfo m_ItemInfo;

	public OnRightClickEvent onRightClick = new OnRightClickEvent();

	public OnDoubleClickEvent onDoubleClick = new OnDoubleClickEvent();

	public OnAssignEvent onAssign = new OnAssignEvent();

	public OnAssignWithSourceEvent onAssignWithSource = new OnAssignWithSourceEvent();

	public OnUnassignEvent onUnassign = new OnUnassignEvent();

	public UIItemSlot_Group slotGroup
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

	public UIItemInfo GetItemInfo()
	{
		return m_ItemInfo;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	public override bool IsAssigned()
	{
		return m_ItemInfo != null;
	}

	public bool Assign(UIItemInfo itemInfo, global::UnityEngine.Object source)
	{
		if (itemInfo == null)
		{
			return false;
		}
		Unassign();
		Assign(itemInfo.Icon);
		m_ItemInfo = itemInfo;
		if (onAssign != null)
		{
			onAssign.Invoke(this);
		}
		if (onAssignWithSource != null)
		{
			onAssignWithSource.Invoke(this, source);
		}
		return true;
	}

	public bool Assign(UIItemInfo itemInfo)
	{
		return Assign(itemInfo, null);
	}

	public override bool Assign(global::UnityEngine.Object source)
	{
		if (source is IUIItemSlot && source is IUIItemSlot sourceSlot)
		{
			return Assign(sourceSlot.GetItemInfo(), source);
		}
		return false;
	}

	public override void Unassign()
	{
		base.Unassign();
		m_ItemInfo = null;
		if (onUnassign != null)
		{
			onUnassign.Invoke(this);
		}
	}

	public override bool CanSwapWith(global::UnityEngine.Object target)
	{
		if (target is IUIItemSlot)
		{
			if (target is UIEquipSlot)
			{
				return (target as UIEquipSlot).CheckEquipType(GetItemInfo());
			}
			return true;
		}
		return false;
	}

	public override bool PerformSlotSwap(global::UnityEngine.Object sourceObject)
	{
		IUIItemSlot obj = sourceObject as IUIItemSlot;
		UIItemInfo sourceItemInfo = obj.GetItemInfo();
		bool num = obj.Assign(GetItemInfo(), this);
		bool assign2 = Assign(sourceItemInfo, sourceObject);
		return num && assign2;
	}

	public override void OnTooltip(bool show)
	{
		if (m_ItemInfo != null)
		{
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				PrepareTooltip(m_ItemInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
			}
			else
			{
				UITooltip.Hide();
			}
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (!IsAssigned())
		{
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2)
		{
			eventData.clickCount = 0;
			if (onDoubleClick != null)
			{
				onDoubleClick.Invoke(this);
			}
		}
		if (eventData.button == PointerEventData.InputButton.Right && onRightClick != null)
		{
			onRightClick.Invoke(this);
		}
	}

	protected override void OnThrowAwayDenied()
	{
		if (!IsAssigned())
		{
			return;
		}
		if (UIModalBoxManager.Instance == null)
		{
			Debug.LogWarning("Could not load the modal box manager while creating a modal box.");
			return;
		}
		UIModalBox box = UIModalBoxManager.Instance.Create(base.gameObject);
		if (box != null)
		{
			box.SetText1("Do you really want to destroy \"" + m_ItemInfo.Name + "\"?");
			box.SetText2("You wont be able to reverse this operation and your item will be permamently removed.");
			box.SetConfirmButtonText("DESTROY");
			box.onConfirm.AddListener(Unassign);
			box.Show();
		}
	}

	[ContextMenu("Auto Assign ID")]
	public void AutoAssignID()
	{
		List<UIItemSlot> slots = GetSlotsInGroup(m_SlotGroup);
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

	public static List<UIItemSlot> GetSlots()
	{
		List<UIItemSlot> slots = new List<UIItemSlot>();
		UIItemSlot[] array = Resources.FindObjectsOfTypeAll<UIItemSlot>();
		foreach (UIItemSlot s in array)
		{
			if (s.gameObject.activeInHierarchy)
			{
				slots.Add(s);
			}
		}
		return slots;
	}

	public static List<UIItemSlot> GetSlotsWithID(int ID)
	{
		List<UIItemSlot> slots = new List<UIItemSlot>();
		UIItemSlot[] array = Resources.FindObjectsOfTypeAll<UIItemSlot>();
		foreach (UIItemSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.ID == ID)
			{
				slots.Add(s);
			}
		}
		return slots;
	}

	public static List<UIItemSlot> GetSlotsInGroup(UIItemSlot_Group group)
	{
		List<UIItemSlot> slots = new List<UIItemSlot>();
		UIItemSlot[] array = Resources.FindObjectsOfTypeAll<UIItemSlot>();
		foreach (UIItemSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.slotGroup == group)
			{
				slots.Add(s);
			}
		}
		slots.Sort((UIItemSlot a, UIItemSlot b) => a.ID.CompareTo(b.ID));
		return slots;
	}

	public static UIItemSlot GetSlot(int ID, UIItemSlot_Group group)
	{
		UIItemSlot[] array = Resources.FindObjectsOfTypeAll<UIItemSlot>();
		foreach (UIItemSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.ID == ID && s.slotGroup == group)
			{
				return s;
			}
		}
		return null;
	}

	public static void PrepareTooltip(UIItemInfo itemInfo)
	{
		if (itemInfo != null)
		{
			if (UITooltipManager.Instance != null)
			{
				UITooltip.SetWidth(UITooltipManager.Instance.itemTooltipWidth);
			}
			UITooltip.AddTitle("<color=#" + UIItemQualityColor.GetHexColor(itemInfo.Quality) + ">" + itemInfo.Name + "</color>");
			UITooltip.AddSpacer();
			UITooltip.AddLineColumn(itemInfo.Type, "ItemAttribute");
			UITooltip.AddLineColumn(itemInfo.Subtype, "ItemAttribute");
			if (itemInfo.ItemType == 1)
			{
				UITooltip.AddLineColumn(itemInfo.Damage + " Damage", "ItemAttribute");
				UITooltip.AddLineColumn(itemInfo.AttackSpeed.ToString("0.0") + " Attack speed", "ItemAttribute");
				UITooltip.AddLine("(" + ((float)itemInfo.Damage / itemInfo.AttackSpeed).ToString("0.0") + " damage per second)", "ItemAttribute");
			}
			else
			{
				UITooltip.AddLineColumn(itemInfo.Armor + " Armor", "ItemAttribute");
				UITooltip.AddLineColumn(itemInfo.Block + " Block", "ItemAttribute");
			}
			UITooltip.AddSpacer();
			UITooltip.AddLine("+" + itemInfo.Stamina + " Stamina", "ItemStat");
			UITooltip.AddLine("+" + itemInfo.Strength + " Strength", "ItemStat");
			UITooltip.AddSpacer();
			UITooltip.AddLine("Durability " + itemInfo.Durability + "/" + itemInfo.Durability, "ItemAttribute");
			if (itemInfo.RequiredLevel > 0)
			{
				UITooltip.AddLine("Requires Level " + itemInfo.RequiredLevel, "ItemAttribute");
			}
			if (!string.IsNullOrEmpty(itemInfo.Description))
			{
				UITooltip.AddSpacer();
				UITooltip.AddLine(itemInfo.Description, "ItemDescription");
			}
		}
	}
}
