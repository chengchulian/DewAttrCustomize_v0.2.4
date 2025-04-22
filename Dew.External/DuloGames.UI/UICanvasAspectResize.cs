using UnityEngine;

namespace DuloGames.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UICanvasAspectResize : MonoBehaviour
{
	[SerializeField]
	private Camera m_Camera;

	private RectTransform m_RectTransform;

	protected void Awake()
	{
		m_RectTransform = base.transform as RectTransform;
	}

	private void Update()
	{
		if (!(m_Camera == null))
		{
			m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Camera.aspect * m_RectTransform.rect.size.y);
		}
	}
}
