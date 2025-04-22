using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Effects/Letter Spacing", 14)]
[RequireComponent(typeof(Text))]
public class LetterSpacing : BaseMeshEffect, ILayoutElement
{
	[SerializeField]
	private float m_spacing;

	public float spacing
	{
		get
		{
			return m_spacing;
		}
		set
		{
			if (m_spacing != value)
			{
				m_spacing = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
				LayoutRebuilder.MarkLayoutForRebuild((RectTransform)base.transform);
			}
		}
	}

	private Text text => base.gameObject.GetComponent<Text>();

	public float minWidth => text.minWidth;

	public float preferredWidth => spacing * (float)text.fontSize / 100f * (float)(text.text.Length - 1) + text.preferredWidth;

	public float flexibleWidth => text.flexibleWidth;

	public float minHeight => text.minHeight;

	public float preferredHeight => text.preferredHeight;

	public float flexibleHeight => text.flexibleHeight;

	public int layoutPriority => text.layoutPriority;

	protected LetterSpacing()
	{
	}

	public void CalculateLayoutInputHorizontal()
	{
	}

	public void CalculateLayoutInputVertical()
	{
	}

	private string[] GetLines()
	{
		IList<UILineInfo> lineInfos = text.cachedTextGenerator.lines;
		string[] lines = new string[lineInfos.Count];
		for (int i = 0; i < lineInfos.Count; i++)
		{
			if (i < lineInfos.Count - 1)
			{
				UILineInfo uILineInfo = lineInfos[i + 1];
				int lineStart = lineInfos[i].startCharIdx;
				int length = uILineInfo.startCharIdx - 1 - lineStart;
				lines[i] = text.text.Substring(lineInfos[i].startCharIdx, length);
			}
			else
			{
				lines[i] = text.text.Substring(lineInfos[i].startCharIdx);
			}
		}
		return lines;
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
		if (!IsActive() || verts.Count == 0)
		{
			return;
		}
		string[] lines = GetLines();
		float letterOffset = spacing * (float)text.fontSize / 100f;
		float alignmentFactor = 0f;
		int glyphIdx = 0;
		switch (text.alignment)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.MiddleLeft:
		case TextAnchor.LowerLeft:
			alignmentFactor = 0f;
			break;
		case TextAnchor.UpperCenter:
		case TextAnchor.MiddleCenter:
		case TextAnchor.LowerCenter:
			alignmentFactor = 0.5f;
			break;
		case TextAnchor.UpperRight:
		case TextAnchor.MiddleRight:
		case TextAnchor.LowerRight:
			alignmentFactor = 1f;
			break;
		}
		foreach (string line in lines)
		{
			float lineOffset = (float)(line.Length - 1) * letterOffset * alignmentFactor;
			for (int charIdx = 0; charIdx < line.Length; charIdx++)
			{
				int idx1 = glyphIdx * 6;
				int idx2 = glyphIdx * 6 + 1;
				int idx3 = glyphIdx * 6 + 2;
				int idx4 = glyphIdx * 6 + 3;
				int idx5 = glyphIdx * 6 + 4;
				int idx6 = glyphIdx * 6 + 5;
				if (idx6 > verts.Count - 1)
				{
					return;
				}
				UIVertex vert1 = verts[idx1];
				UIVertex vert2 = verts[idx2];
				UIVertex vert3 = verts[idx3];
				UIVertex vert4 = verts[idx4];
				UIVertex vert5 = verts[idx5];
				UIVertex vert6 = verts[idx6];
				Vector3 pos = Vector3.right * (letterOffset * (float)charIdx - lineOffset);
				vert1.position += pos;
				vert2.position += pos;
				vert3.position += pos;
				vert4.position += pos;
				vert5.position += pos;
				vert6.position += pos;
				verts[idx1] = vert1;
				verts[idx2] = vert2;
				verts[idx3] = vert3;
				verts[idx4] = vert4;
				verts[idx5] = vert5;
				verts[idx6] = vert6;
				glyphIdx++;
			}
			glyphIdx++;
		}
	}
}
