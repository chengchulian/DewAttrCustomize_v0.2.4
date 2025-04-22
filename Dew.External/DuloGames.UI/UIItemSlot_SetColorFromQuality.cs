using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(UIItemSlot))]
public class UIItemSlot_SetColorFromQuality : MonoBehaviour
{
	[Serializable]
	public struct QualityToColor
	{
		public UIItemQuality quality;

		public Color color;
	}

	[SerializeField]
	private Graphic m_Target;

	private UIItemSlot m_Slot;

	protected void Awake()
	{
		m_Slot = base.gameObject.GetComponent<UIItemSlot>();
	}

	protected void OnEnable()
	{
		m_Slot.onAssign.AddListener(OnSlotAssign);
		m_Slot.onUnassign.AddListener(OnSlotUnassign);
	}

	protected void OnDisable()
	{
		m_Slot.onAssign.RemoveListener(OnSlotAssign);
		m_Slot.onUnassign.RemoveListener(OnSlotUnassign);
	}

	public void OnSlotAssign(UIItemSlot slot)
	{
		if (!(m_Target == null) || slot.GetItemInfo() == null)
		{
			m_Target.color = UIItemQualityColor.GetColor(slot.GetItemInfo().Quality);
		}
	}

	public void OnSlotUnassign(UIItemSlot slot)
	{
		if (!(m_Target == null))
		{
			m_Target.color = Color.clear;
		}
	}
}
