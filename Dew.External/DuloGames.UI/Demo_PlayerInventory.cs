using UnityEngine;

namespace DuloGames.UI;

public class Demo_PlayerInventory : MonoBehaviour
{
	[SerializeField]
	private Transform m_SlotsContainer;

	private void Start()
	{
		if (!(m_SlotsContainer != null))
		{
			return;
		}
		UIItemSlot[] componentsInChildren = m_SlotsContainer.GetComponentsInChildren<UIItemSlot>();
		foreach (UIItemSlot slot in componentsInChildren)
		{
			int assignedID = PlayerPrefs.GetInt("InventorySlot" + slot.ID, 0);
			if (assignedID > 0)
			{
				slot.Assign(UIItemDatabase.Instance.GetByID(assignedID));
			}
		}
	}

	private void OnEnable()
	{
		if (m_SlotsContainer != null)
		{
			UIItemSlot[] componentsInChildren = m_SlotsContainer.GetComponentsInChildren<UIItemSlot>();
			foreach (UIItemSlot obj in componentsInChildren)
			{
				obj.onAssign.AddListener(OnSlotAssigned);
				obj.onUnassign.AddListener(OnSlotUnassigned);
			}
		}
	}

	private void OnDisable()
	{
		if (m_SlotsContainer != null)
		{
			UIItemSlot[] componentsInChildren = m_SlotsContainer.GetComponentsInChildren<UIItemSlot>();
			foreach (UIItemSlot obj in componentsInChildren)
			{
				obj.onAssign.RemoveListener(OnSlotAssigned);
				obj.onUnassign.RemoveListener(OnSlotUnassigned);
			}
		}
	}

	private void OnSlotAssigned(UIItemSlot slot)
	{
		PlayerPrefs.SetInt("InventorySlot" + slot.ID, slot.GetItemInfo().ID);
	}

	private void OnSlotUnassigned(UIItemSlot slot)
	{
		PlayerPrefs.SetInt("InventorySlot" + slot.ID, 0);
	}
}
