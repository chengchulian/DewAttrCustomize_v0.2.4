using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Pagination", 82)]
public class UIPagination : MonoBehaviour
{
	[SerializeField]
	private Transform m_PagesContainer;

	[SerializeField]
	private Button m_ButtonPrev;

	[SerializeField]
	private Button m_ButtonNext;

	[SerializeField]
	private Text m_LabelText;

	[SerializeField]
	private Color m_LabelActiveColor = Color.white;

	private int activePage;

	private void Start()
	{
		if (m_ButtonPrev != null)
		{
			m_ButtonPrev.onClick.AddListener(OnPrevClick);
		}
		if (m_ButtonNext != null)
		{
			m_ButtonNext.onClick.AddListener(OnNextClick);
		}
		if (m_PagesContainer != null && m_PagesContainer.childCount > 0)
		{
			for (int i = 0; i < m_PagesContainer.childCount; i++)
			{
				if (m_PagesContainer.GetChild(i).gameObject.activeSelf)
				{
					activePage = i;
					break;
				}
			}
		}
		UpdatePagesVisibility();
	}

	private void UpdatePagesVisibility()
	{
		if (m_PagesContainer == null)
		{
			return;
		}
		if (m_PagesContainer.childCount > 0)
		{
			for (int i = 0; i < m_PagesContainer.childCount; i++)
			{
				m_PagesContainer.GetChild(i).gameObject.SetActive((i == activePage) ? true : false);
			}
		}
		if (m_LabelText != null)
		{
			m_LabelText.text = "<color=#" + CommonColorBuffer.ColorToString(m_LabelActiveColor) + ">" + (activePage + 1) + "</color> / " + m_PagesContainer.childCount;
		}
	}

	private void OnPrevClick()
	{
		if (base.isActiveAndEnabled && !(m_PagesContainer == null))
		{
			if (activePage == 0)
			{
				activePage = m_PagesContainer.childCount - 1;
			}
			else
			{
				activePage--;
			}
			UpdatePagesVisibility();
		}
	}

	private void OnNextClick()
	{
		if (base.isActiveAndEnabled && !(m_PagesContainer == null))
		{
			if (activePage == m_PagesContainer.childCount - 1)
			{
				activePage = 0;
			}
			else
			{
				activePage++;
			}
			UpdatePagesVisibility();
		}
	}
}
