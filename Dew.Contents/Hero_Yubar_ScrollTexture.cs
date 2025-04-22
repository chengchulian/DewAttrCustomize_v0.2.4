using System.Collections.Generic;
using UnityEngine;

public class Hero_Yubar_ScrollTexture : MonoBehaviour
{
	private SkinnedMeshRenderer _renderer;

	private Material _mat;

	private void Awake()
	{
		_renderer = GetComponent<SkinnedMeshRenderer>();
	}

	private void Update()
	{
		if (_mat == null)
		{
			ListReturnHandle<Material> handle;
			List<Material> list = DewPool.GetList(out handle);
			_renderer.GetSharedMaterials(list);
			if (list.Count == 3 && list[2] != null && list[2].name.EndsWith("(Clone)"))
			{
				_mat = list[2];
			}
			handle.Return();
		}
		else
		{
			_mat.SetTextureOffset("_BaseMap", new Vector2(Time.time * 0.05f, 0f));
			_mat.SetTextureOffset("_EmissionMap", new Vector2(Time.time * 0.05f, 0f));
		}
	}
}
