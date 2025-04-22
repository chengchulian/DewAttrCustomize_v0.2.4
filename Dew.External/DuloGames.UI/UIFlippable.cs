using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Graphic))]
[DisallowMultipleComponent]
[AddComponentMenu("UI/Flippable", 8)]
public class UIFlippable : MonoBehaviour, IMeshModifier
{
	[SerializeField]
	private bool m_Horizontal;

	[SerializeField]
	private bool m_Veritical;

	public bool horizontal
	{
		get
		{
			return m_Horizontal;
		}
		set
		{
			m_Horizontal = value;
			GetComponent<Graphic>().SetVerticesDirty();
		}
	}

	public bool vertical
	{
		get
		{
			return m_Veritical;
		}
		set
		{
			m_Veritical = value;
			GetComponent<Graphic>().SetVerticesDirty();
		}
	}

	public void ModifyMesh(VertexHelper vertexHelper)
	{
		if (base.enabled)
		{
			List<UIVertex> list = new List<UIVertex>();
			vertexHelper.GetUIVertexStream(list);
			ModifyVertices(list);
			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(list);
		}
	}

	public void ModifyMesh(Mesh mesh)
	{
		if (!base.enabled)
		{
			return;
		}
		List<UIVertex> list = new List<UIVertex>();
		using (VertexHelper vertexHelper = new VertexHelper(mesh))
		{
			vertexHelper.GetUIVertexStream(list);
		}
		ModifyVertices(list);
		using VertexHelper vertexHelper2 = new VertexHelper();
		vertexHelper2.AddUIVertexTriangleStream(list);
		vertexHelper2.FillMesh(mesh);
	}

	public void ModifyVertices(List<UIVertex> verts)
	{
		if (base.enabled)
		{
			RectTransform rt = base.transform as RectTransform;
			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex v = verts[i];
				v.position = new Vector3(m_Horizontal ? (v.position.x + (rt.rect.center.x - v.position.x) * 2f) : v.position.x, m_Veritical ? (v.position.y + (rt.rect.center.y - v.position.y) * 2f) : v.position.y, v.position.z);
				verts[i] = v;
			}
		}
	}
}
