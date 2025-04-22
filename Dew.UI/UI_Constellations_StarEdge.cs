using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class UI_Constellations_StarEdge : MonoBehaviour
{
	public UILineRenderer lineRenderer;

	public Material matNormal;

	public Material matHover;

	public Material matConnected;

	private UI_Constellations_StarItem _a;

	private UI_Constellations_StarItem _b;

	private UI_Constellations_CategoryGroup _parent;

	private Vector2[] _points = new Vector2[2];

	public void Setup(UI_Constellations_StarItem a, UI_Constellations_StarItem b, UI_Constellations_CategoryGroup parent)
	{
		RectTransform obj = (RectTransform)base.transform;
		obj.anchorMin = Vector2.zero;
		obj.anchorMax = Vector2.zero;
		obj.anchoredPosition = Vector2.zero;
		base.transform.SetSiblingIndex(0);
		_a = a;
		_b = b;
		_parent = parent;
		lineRenderer.Points = _points;
		UI_Constellations instance = SingletonBehaviour<UI_Constellations>.instance;
		instance.onSelectedGroupChanged = (Action<UI_Constellations_CategoryGroup>)Delegate.Combine(instance.onSelectedGroupChanged, new Action<UI_Constellations_CategoryGroup>(OnSelectedGroupChanged));
		UI_Constellations instance2 = SingletonBehaviour<UI_Constellations>.instance;
		instance2.onStateChanged = (Action)Delegate.Combine(instance2.onStateChanged, new Action(UpdateStatus));
		OnSelectedGroupChanged(null);
		UpdatePosition();
		UpdateStatus();
	}

	private void OnSelectedGroupChanged(UI_Constellations_CategoryGroup obj)
	{
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		lineRenderer.material = ((_a.data.level > 0 && _b.data.level > 0) ? matConnected : matNormal);
		float thickness = ((_a.data.level > 0 && _b.data.level > 0) ? 60f : 30f);
		lineRenderer.LineThickness = ((SingletonBehaviour<UI_Constellations>.instance.selectedGroup == _parent) ? thickness : (thickness * 1.5f));
	}

	private void Update()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		if (!(_a == null) && !(_b == null))
		{
			_points[0] = base.transform.InverseTransformPoint(_a.transform.position);
			_points[1] = base.transform.InverseTransformPoint(_b.transform.position);
			lineRenderer.SetVerticesDirty();
		}
	}
}
