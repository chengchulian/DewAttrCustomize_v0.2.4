using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Equip Receiver", 46)]
[ExecuteInEditMode]
public class UIEquipReceiver : UIBehaviour, IEventSystemHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public enum HintState
	{
		Shown,
		Hidden
	}

	[SerializeField]
	private Transform m_SlotsContainer;

	[SerializeField]
	private Text m_HintText;

	[SerializeField]
	private bool m_Fading = true;

	[SerializeField]
	private float m_FadeDuration = 0.15f;

	[SerializeField]
	private HintState m_HintState = HintState.Hidden;

	protected override void Start()
	{
		if (Application.isPlaying && m_SlotsContainer == null)
		{
			m_SlotsContainer = base.transform;
		}
		if (m_HintText != null)
		{
			m_HintText.canvasRenderer.SetAlpha((m_HintState == HintState.Hidden) ? 0f : 1f);
		}
	}

	public UIEquipSlot GetSlotByType(UIEquipmentType type)
	{
		UIEquipSlot[] componentsInChildren = m_SlotsContainer.GetComponentsInChildren<UIEquipSlot>();
		foreach (UIEquipSlot slot in componentsInChildren)
		{
			if (slot.enabled && slot.gameObject.activeSelf && slot.equipType == type)
			{
				return slot;
			}
		}
		return null;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!(m_HintText == null) && eventData.dragging && ExtractSlot(eventData) != null)
		{
			if (m_Fading)
			{
				m_HintText.CrossFadeAlpha(1f, m_FadeDuration, ignoreTimeScale: true);
			}
			else
			{
				m_HintText.canvasRenderer.SetAlpha(1f);
			}
			m_HintState = HintState.Shown;
		}
	}

	private void HideHint()
	{
		if (m_HintState != HintState.Hidden)
		{
			if (m_Fading)
			{
				m_HintText.CrossFadeAlpha(0f, m_FadeDuration, ignoreTimeScale: true);
			}
			else
			{
				m_HintText.canvasRenderer.SetAlpha(0f);
			}
			m_HintState = HintState.Hidden;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!(m_HintText == null))
		{
			HideHint();
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerPress == null)
		{
			return;
		}
		UISlotBase slotBase = ExtractSlot(eventData);
		if (slotBase == null || !(slotBase is UIItemSlot))
		{
			return;
		}
		UIItemSlot itemSlot = slotBase as UIItemSlot;
		if (itemSlot != null && itemSlot.IsAssigned())
		{
			UIEquipSlot equipSlot = GetSlotByType(itemSlot.GetItemInfo().EquipType);
			if (equipSlot != null)
			{
				equipSlot.OnDrop(eventData);
				HideHint();
			}
		}
	}

	private UISlotBase ExtractSlot(PointerEventData eventData)
	{
		if (eventData.pointerPress == null)
		{
			return null;
		}
		UISlotBase slotBase = eventData.pointerPress.GetComponent<UISlotBase>();
		if (slotBase == null)
		{
			slotBase = eventData.pointerPress.GetComponentInChildren<UISlotBase>();
		}
		return slotBase;
	}
}
