using System;
using UnityEngine;

namespace DuloGames.UI;

[ExecuteInEditMode]
public class UIWorldCanvasScaleByCamera : MonoBehaviour
{
	[SerializeField]
	private Camera m_Camera;

	[SerializeField]
	private Canvas m_Canvas;

	protected void Update()
	{
		if (!(m_Camera == null) && !(m_Canvas == null))
		{
			float distanceToMain = Vector3.Distance(m_Camera.transform.position, m_Canvas.transform.position);
			float camHeight = ((!m_Camera.orthographic) ? (2f * distanceToMain * Mathf.Tan(m_Camera.fieldOfView * 0.5f * (MathF.PI / 180f))) : (m_Camera.orthographicSize * 2f));
			float scaleFactor = (float)Screen.height / (m_Canvas.transform as RectTransform).rect.height;
			float scale = camHeight / (float)Screen.height * scaleFactor;
			m_Canvas.transform.localScale = new Vector3(scale, scale, 1f);
		}
	}
}
