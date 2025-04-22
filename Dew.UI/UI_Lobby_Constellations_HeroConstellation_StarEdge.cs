using UnityEngine;
using UnityEngine.UI.Extensions;

public class UI_Lobby_Constellations_HeroConstellation_StarEdge : MonoBehaviour
{
	private Transform _a;

	private Transform _b;

	private UILineRenderer[] _lineRenderers;

	public void Setup(StarType type, Transform a, Transform b)
	{
		_a = a;
		_b = b;
		_lineRenderers = GetComponentsInChildren<UILineRenderer>();
		UILineRenderer[] lineRenderers = _lineRenderers;
		for (int i = 0; i < lineRenderers.Length; i++)
		{
			lineRenderers[i].color *= Color.Lerp(Dew.GetStarCategoryColor(type), Color.white, 0f);
		}
	}

	private void UpdatePosition()
	{
		base.transform.position = Vector3.zero;
		Vector2[] arr = _lineRenderers[0].Points ?? new Vector2[2];
		arr[0] = _a.position / base.transform.lossyScale.x;
		arr[1] = _b.position / base.transform.lossyScale.x;
		UILineRenderer[] lineRenderers = _lineRenderers;
		foreach (UILineRenderer obj in lineRenderers)
		{
			obj.Points = arr;
			obj.SetAllDirty();
		}
	}

	private void Update()
	{
		UpdatePosition();
	}
}
