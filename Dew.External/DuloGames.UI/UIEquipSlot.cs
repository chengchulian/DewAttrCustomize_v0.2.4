using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Equip Slot", 12)]
public class UIEquipSlot : UISlotBase, IUIItemSlot
{
	[Serializable]
	public class OnAssignEvent : UnityEvent<UIEquipSlot>
	{
	}

	[Serializable]
	public class OnAssignWithSourceEvent : UnityEvent<UIEquipSlot, global::UnityEngine.Object>
	{
	}

	[Serializable]
	public class OnUnassignEvent : UnityEvent<UIEquipSlot>
	{
	}

	[SerializeField]
	private UIEquipmentType m_EquipType;

	private UIItemInfo m_ItemInfo;

	public OnAssignEvent onAssign = new OnAssignEvent();

	public OnAssignWithSourceEvent onAssignWithSource = new OnAssignWithSourceEvent();

	public OnUnassignEvent onUnassign = new OnUnassignEvent();

	public UIEquipmentType equipType
	{
		get
		{
			return m_EquipType;
		}
		set
		{
			m_EquipType = value;
		}
	}

	public UIItemInfo GetItemInfo()
	{
		return m_ItemInfo;
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
		if (!CheckEquipType(itemInfo))
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
			if (!CheckEquipType(sourceSlot.GetItemInfo()))
			{
				return false;
			}
			return Assign(sourceSlot.GetItemInfo(), source);
		}
		return false;
	}

	public virtual bool CheckEquipType(UIItemInfo info)
	{
		if (info == null)
		{
			return false;
		}
		if (info.EquipType != equipType)
		{
			return false;
		}
		return true;
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
		UITooltip.InstantiateIfNecessary(base.gameObject);
		if (!IsAssigned())
		{
			if (show)
			{
				UITooltip.AddTitle(EquipTypeToString(m_EquipType));
				UITooltip.SetHorizontalFitMode(ContentSizeFitter.FitMode.PreferredSize);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
			}
			else
			{
				UITooltip.Hide();
			}
		}
		else if (m_ItemInfo != null)
		{
			if (show)
			{
				UIItemSlot.PrepareTooltip(m_ItemInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
			}
			else
			{
				UITooltip.Hide();
			}
		}
	}

	protected override void OnThrowAwayDenied()
	{
		if (!IsAssigned())
		{
			return;
		}
		List<UIItemSlot> itemSlots = UIItemSlot.GetSlotsInGroup(UIItemSlot_Group.Inventory);
		if (itemSlots.Count <= 0)
		{
			return;
		}
		foreach (UIItemSlot slot in itemSlots)
		{
			if (!slot.IsAssigned())
			{
				slot.Assign(this);
				Unassign();
				break;
			}
		}
	}

	public static string EquipTypeToString(UIEquipmentType type)
	{
		string str = "Undefined";
		switch (type)
		{
		case UIEquipmentType.Head:
			str = "Head";
			break;
		case UIEquipmentType.Necklace:
			str = "Necklace";
			break;
		case UIEquipmentType.Shoulders:
			str = "Shoulders";
			break;
		case UIEquipmentType.Chest:
			str = "Chest";
			break;
		case UIEquipmentType.Back:
			str = "Back";
			break;
		case UIEquipmentType.Bracers:
			str = "Bracers";
			break;
		case UIEquipmentType.Gloves:
			str = "Gloves";
			break;
		case UIEquipmentType.Belt:
			str = "Belt";
			break;
		case UIEquipmentType.Pants:
			str = "Pants";
			break;
		case UIEquipmentType.Boots:
			str = "Boots";
			break;
		case UIEquipmentType.Finger:
			str = "Ring";
			break;
		case UIEquipmentType.Trinket:
			str = "Trinket";
			break;
		case UIEquipmentType.Weapon_MainHand:
			str = "Main Hand";
			break;
		case UIEquipmentType.Weapon_OffHand:
			str = "Off Hand";
			break;
		}
		return str;
	}

	public static List<UIEquipSlot> GetSlots()
	{
		List<UIEquipSlot> slots = new List<UIEquipSlot>();
		UIEquipSlot[] array = Resources.FindObjectsOfTypeAll<UIEquipSlot>();
		foreach (UIEquipSlot s in array)
		{
			if (s.gameObject.activeInHierarchy)
			{
				slots.Add(s);
			}
		}
		return slots;
	}

	public static UIEquipSlot GetSlotWithType(UIEquipmentType type)
	{
		UIEquipSlot[] array = Resources.FindObjectsOfTypeAll<UIEquipSlot>();
		foreach (UIEquipSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.equipType == type)
			{
				return s;
			}
		}
		return null;
	}

	public static List<UIEquipSlot> GetSlotsWithType(UIEquipmentType type)
	{
		List<UIEquipSlot> slots = new List<UIEquipSlot>();
		UIEquipSlot[] array = Resources.FindObjectsOfTypeAll<UIEquipSlot>();
		foreach (UIEquipSlot s in array)
		{
			if (s.gameObject.activeInHierarchy && s.equipType == type)
			{
				slots.Add(s);
			}
		}
		return slots;
	}
}
