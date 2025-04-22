using UnityEngine;
using UnityEngine.UI;

public class UI_HeroIcon : MonoBehaviour
{
	public Image icon;

	private Material _mat;

	private float _seed;

	public void Setup(string type)
	{
		if (_mat == null)
		{
			_mat = Object.Instantiate(icon.material);
			icon.material = _mat;
		}
		Hero h = DewResources.GetByShortTypeName<Hero>(type);
		icon.color = h.mainColor.WithS(Mathf.Clamp(h.mainColor.GetS(), 0f, 0.8f));
		icon.sprite = h.icon;
		_mat.SetColor("_OverlayColor", h.mainColor.WithS(0.5f).WithV(1f));
		_seed = Random.Range(0f, 1000f);
	}

	private void Update()
	{
		if (!(_mat == null))
		{
			_mat.SetVector("_OverlayTex_ST", new Vector4(1f, 1f, _seed, Time.time * -0.2f));
		}
	}

	private void OnDestroy()
	{
		if (_mat != null)
		{
			Object.Destroy(_mat);
			_mat = null;
		}
	}
}
