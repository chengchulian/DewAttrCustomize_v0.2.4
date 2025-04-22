using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Effects/Nicer Outline")]
public class NicerOutline : BaseMeshEffect
{
	[SerializeField]
	private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

	[SerializeField]
	private Vector2 m_EffectDistance = new Vector2(1f, -1f);

	[SerializeField]
	private bool m_UseGraphicAlpha = true;

	public Color effectColor
	{
		get
		{
			return m_EffectColor;
		}
		set
		{
			m_EffectColor = value;
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			return m_EffectDistance;
		}
		set
		{
			if (value.x > 600f)
			{
				value.x = 600f;
			}
			if (value.x < -600f)
			{
				value.x = -600f;
			}
			if (value.y > 600f)
			{
				value.y = 600f;
			}
			if (value.y < -600f)
			{
				value.y = -600f;
			}
			if (!(m_EffectDistance == value))
			{
				m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}
	}

	public bool useGraphicAlpha
	{
		get
		{
			return m_UseGraphicAlpha;
		}
		set
		{
			m_UseGraphicAlpha = value;
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}
	}

	protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
	{
		int num = verts.Count * 2;
		if (verts.Capacity < num)
		{
			verts.Capacity = num;
		}
		for (int i = start; i < end; i++)
		{
			UIVertex uIVertex = verts[i];
			verts.Add(uIVertex);
			Vector3 position = uIVertex.position;
			position.x += x;
			position.y += y;
			uIVertex.position = position;
			Color32 color2 = color;
			if (m_UseGraphicAlpha)
			{
				color2.a = (byte)(color2.a * verts[i].color.a / 255);
			}
			uIVertex.color = color2;
			verts[i] = uIVertex;
		}
	}

	public override void ModifyMesh(VertexHelper vertexHelper)
	{
		if (IsActive())
		{
			List<UIVertex> list = new List<UIVertex>();
			vertexHelper.GetUIVertexStream(list);
			ModifyVertices(list);
			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(list);
		}
	}

	public void ModifyVertices(List<UIVertex> verts)
	{
		if (IsActive() && verts.Count != 0)
		{
			Text foundtext = GetComponent<Text>();
			float best_fit_adjustment = 1f;
			if ((bool)foundtext && foundtext.resizeTextForBestFit)
			{
				best_fit_adjustment = (float)foundtext.cachedTextGenerator.fontSizeUsedForBestFit / (float)(foundtext.resizeTextMaxSize - 1);
			}
			float distanceX = effectDistance.x * best_fit_adjustment;
			float distanceY = effectDistance.y * best_fit_adjustment;
			int start = 0;
			int count = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, distanceX, distanceY);
			start = count;
			int count2 = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, distanceX, 0f - distanceY);
			start = count2;
			int count3 = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, 0f - distanceX, distanceY);
			start = count3;
			int count4 = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, 0f - distanceX, 0f - distanceY);
			start = count4;
			int count5 = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, distanceX, 0f);
			start = count5;
			int count6 = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, 0f - distanceX, 0f);
			start = count6;
			int count7 = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, 0f, distanceY);
			start = count7;
			_ = verts.Count;
			ApplyShadow(verts, effectColor, start, verts.Count, 0f, 0f - distanceY);
		}
	}
}
