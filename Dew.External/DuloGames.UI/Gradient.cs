using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseMeshEffect
{
	[SerializeField]
	private Color topColor = Color.white;

	[SerializeField]
	private Color bottomColor = Color.black;

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

	public void ModifyVertices(List<UIVertex> vertexList)
	{
		if (!IsActive() || vertexList.Count == 0)
		{
			return;
		}
		int count = vertexList.Count;
		float bottomY = vertexList[0].position.y;
		float topY = vertexList[0].position.y;
		for (int i = 1; i < count; i++)
		{
			float y = vertexList[i].position.y;
			if (y > topY)
			{
				topY = y;
			}
			else if (y < bottomY)
			{
				bottomY = y;
			}
		}
		float uiElementHeight = topY - bottomY;
		for (int j = 0; j < count; j++)
		{
			UIVertex uiVertex = vertexList[j];
			uiVertex.color *= Color.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
			vertexList[j] = uiVertex;
		}
	}
}
