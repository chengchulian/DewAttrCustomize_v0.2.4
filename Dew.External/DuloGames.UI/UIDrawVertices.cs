using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
public class UIDrawVertices : MonoBehaviour, IMeshModifier
{
	[SerializeField]
	private Color color = Color.green;

	private Mesh mesh;

	public void ModifyMesh(VertexHelper vertexHelper)
	{
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		vertexHelper.FillMesh(mesh);
	}

	public void ModifyMesh(Mesh mesh)
	{
	}

	public void OnDrawGizmos()
	{
		if (!(mesh == null))
		{
			Gizmos.color = color;
			Gizmos.DrawWireMesh(mesh, base.transform.position);
		}
	}
}
