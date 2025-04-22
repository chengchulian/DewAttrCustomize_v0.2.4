using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(InputField))]
public class UIInputCaretOffset : MonoBehaviour
{
	[SerializeField]
	private Vector2 m_Offset = Vector2.zero;

	private Vector2 m_InitialPosition = Vector2.zero;

	protected void OnTransformChildrenChanged()
	{
		Invoke("ApplyOffset", 0.5f);
	}

	public void ApplyOffset()
	{
		foreach (Transform trans in base.transform)
		{
			if (trans.gameObject.name.ToLower().Contains("caret"))
			{
				RectTransform rect = trans as RectTransform;
				m_InitialPosition = rect.anchoredPosition;
				rect.anchoredPosition = m_InitialPosition + m_Offset;
				break;
			}
		}
	}
}
