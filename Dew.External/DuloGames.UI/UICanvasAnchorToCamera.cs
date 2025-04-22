using UnityEngine;

namespace DuloGames.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UICanvasAnchorToCamera : MonoBehaviour
{
	[SerializeField]
	private Camera m_Camera;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_Vertical;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_Horizontal;

	private RectTransform m_RectTransform;

	protected void Awake()
	{
		m_RectTransform = base.transform as RectTransform;
	}

	private void Update()
	{
		if (!(m_Camera == null))
		{
			Vector3 newPos = m_Camera.ViewportToWorldPoint(new Vector3(m_Horizontal, m_Vertical, m_Camera.farClipPlane));
			newPos.z = m_RectTransform.position.z;
			m_RectTransform.position = newPos;
		}
	}
}
