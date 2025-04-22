using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Talent Slot", 12)]
public class UITalentSlot : UISlotBase
{
	[SerializeField]
	private Text m_PointsText;

	[SerializeField]
	private Color m_pointsMinColor = Color.white;

	[SerializeField]
	private Color m_pointsMaxColor = Color.white;

	[SerializeField]
	private Color m_pointsActiveColor = Color.white;

	private UITalentInfo m_TalentInfo;

	private UISpellInfo m_SpellInfo;

	private int m_CurrentPoints;

	protected override void Start()
	{
		base.Start();
		base.dragAndDropEnabled = false;
	}

	public UISpellInfo GetSpellInfo()
	{
		return m_SpellInfo;
	}

	public UITalentInfo GetTalentInfo()
	{
		return m_TalentInfo;
	}

	public override bool IsAssigned()
	{
		return m_TalentInfo != null;
	}

	public bool Assign(UITalentInfo talentInfo, UISpellInfo spellInfo)
	{
		if (talentInfo == null || spellInfo == null)
		{
			return false;
		}
		Assign(spellInfo.Icon);
		m_TalentInfo = talentInfo;
		m_SpellInfo = spellInfo;
		UpdatePointsLabel();
		return true;
	}

	public void UpdatePointsLabel()
	{
		if (!(m_PointsText == null))
		{
			m_PointsText.text = "";
			if (m_CurrentPoints == 0)
			{
				Text pointsText = m_PointsText;
				pointsText.text = pointsText.text + "<color=#" + CommonColorBuffer.ColorToString(m_pointsMinColor) + ">" + m_CurrentPoints + "</color>";
				pointsText = m_PointsText;
				pointsText.text = pointsText.text + "<color=#" + CommonColorBuffer.ColorToString(m_pointsMaxColor) + ">/" + m_TalentInfo.maxPoints + "</color>";
			}
			else if (m_CurrentPoints > 0 && m_CurrentPoints < m_TalentInfo.maxPoints)
			{
				Text pointsText = m_PointsText;
				pointsText.text = pointsText.text + "<color=#" + CommonColorBuffer.ColorToString(m_pointsMinColor) + ">" + m_CurrentPoints + "</color>";
				pointsText = m_PointsText;
				pointsText.text = pointsText.text + "<color=#" + CommonColorBuffer.ColorToString(m_pointsMaxColor) + ">/" + m_TalentInfo.maxPoints + "</color>";
			}
			else
			{
				Text pointsText = m_PointsText;
				pointsText.text = pointsText.text + "<color=#" + CommonColorBuffer.ColorToString(m_pointsActiveColor) + ">" + m_CurrentPoints + "/" + m_TalentInfo.maxPoints + "</color>";
			}
		}
	}

	public override void Unassign()
	{
		base.Unassign();
		m_TalentInfo = null;
		m_SpellInfo = null;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (IsAssigned())
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				OnRightPointerClick(eventData);
			}
			else if (m_CurrentPoints < m_TalentInfo.maxPoints)
			{
				m_CurrentPoints++;
				UpdatePointsLabel();
			}
		}
	}

	public virtual void OnRightPointerClick(PointerEventData eventData)
	{
		if (m_CurrentPoints != 0)
		{
			m_CurrentPoints--;
			UpdatePointsLabel();
		}
	}

	public void AddPoints(int points)
	{
		if (IsAssigned() && points != 0)
		{
			m_CurrentPoints += points;
			if (m_CurrentPoints < 0)
			{
				m_CurrentPoints = 0;
			}
			if (m_CurrentPoints > m_TalentInfo.maxPoints)
			{
				m_CurrentPoints = m_TalentInfo.maxPoints;
			}
			UpdatePointsLabel();
		}
	}

	public override void OnTooltip(bool show)
	{
		if (m_SpellInfo != null)
		{
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				UISpellSlot.PrepareTooltip(m_SpellInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
			}
			else
			{
				UITooltip.Hide();
			}
		}
	}
}
