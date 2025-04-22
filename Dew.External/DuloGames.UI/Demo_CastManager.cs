using UnityEngine;

namespace DuloGames.UI;

public class Demo_CastManager : MonoBehaviour
{
	[SerializeField]
	private UICastBar m_CastBar;

	[SerializeField]
	private Transform[] m_SlotContainers;

	protected void Start()
	{
		if (m_SlotContainers == null || m_SlotContainers.Length == 0)
		{
			return;
		}
		Transform[] slotContainers = m_SlotContainers;
		for (int i = 0; i < slotContainers.Length; i++)
		{
			UISpellSlot[] componentsInChildren = slotContainers[i].GetComponentsInChildren<UISpellSlot>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].onClick.AddListener(OnSpellClick);
			}
		}
	}

	public void OnSpellClick(UISpellSlot slot)
	{
		if (m_CastBar == null || !slot.IsAssigned() || m_CastBar.IsCasting)
		{
			return;
		}
		UISpellInfo spellInfo = slot.GetSpellInfo();
		if (spellInfo == null || (spellInfo.Cooldown > 0f && slot.cooldownComponent != null && slot.cooldownComponent.IsOnCooldown))
		{
			return;
		}
		if (!spellInfo.Flags.Has(UISpellInfo_Flags.InstantCast))
		{
			m_CastBar.StartCasting(spellInfo, spellInfo.CastTime, Time.time + spellInfo.CastTime);
		}
		if (!(slot.cooldownComponent != null) || !(spellInfo.Cooldown > 0f))
		{
			return;
		}
		foreach (UISpellSlot s in UISpellSlot.GetSlots())
		{
			if (s.IsAssigned() && s.GetSpellInfo() != null && s.cooldownComponent != null && s.GetSpellInfo().ID == spellInfo.ID)
			{
				s.cooldownComponent.StartCooldown(spellInfo.ID, spellInfo.Cooldown);
			}
		}
	}
}
