using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("DE Shaders/Preview Manager")]
public class DE_PreviewManager : MonoBehaviour
{
	[HideInInspector]
	public Material[] materials;

	public void OnDestroy()
	{
		LoadDefaultShaders();
	}

	public void LoadDefaultShaders()
	{
		MeshRenderer r = GetComponent<MeshRenderer>();
		if ((bool)r)
		{
			r.sharedMaterials = materials;
		}
	}
}
