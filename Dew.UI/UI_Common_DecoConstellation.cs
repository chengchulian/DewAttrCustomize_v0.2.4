using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_Common_DecoConstellation : MonoBehaviour
{
	[Serializable]
	public struct Edge
	{
		public int a;

		public int b;
	}

	public Color color = new Color(0f, 0.5f, 1f);

	public bool disableAnimation;

	public int starCount;

	public List<Vector2> starPositions;

	public List<Edge> edges;

	public GameObject starTemplate;

	public Transform starParent;

	public UILineRenderer lineRenderer;

	public CanvasRenderer alphaRefRenderer;

	public Vector2 fluctuateMagnitude;

	public float fluctuateSmoothTime;

	public float starPositionChangeTime;

	public float timeScale = 1f;

	[NonSerialized]
	public RectTransform[] stars;

	private Vector2[] _targetPositions;

	private Vector2[] _currentPositions;

	private Vector2[] _currentVelocities;

	private Vector2[] _points;

	private float _lastStarPositionChangeTime = float.NegativeInfinity;

	private Material _mat;

	private RectTransform _rt => (RectTransform)base.transform;

	private void Awake()
	{
		_mat = global::UnityEngine.Object.Instantiate(lineRenderer.material);
		lineRenderer.material = _mat;
		Image[] componentsInChildren = starTemplate.GetComponentsInChildren<Image>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material = _mat;
		}
		starTemplate.SetActive(value: false);
		CreateStars();
	}

	private void Start()
	{
		UpdateHue();
	}

	private void UpdateHue()
	{
		color.ToHSV(out var h, out var _, out var _);
		_mat.SetFloat("_HsvShift", Mathf.Repeat((0f - h) * 360f + 210f, 360f));
	}

	private void OnDestroy()
	{
		if (_mat != null)
		{
			global::UnityEngine.Object.Destroy(_mat);
			_mat = null;
		}
	}

	private void CreateStars()
	{
		if (starPositions == null)
		{
			starPositions = new List<Vector2>();
		}
		while (starPositions.Count > starCount)
		{
			starPositions.RemoveAt(starPositions.Count - 1);
		}
		while (starPositions.Count < starCount)
		{
			starPositions.Add((starPositions.Count > 0) ? starPositions[^1] : default(Vector2));
		}
		if (edges == null)
		{
			edges = new List<Edge>();
		}
		for (int i = starParent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.DestroyImmediate(starParent.GetChild(i).gameObject);
		}
		stars = new RectTransform[starCount];
		for (int j = 0; j < starCount; j++)
		{
			GameObject newStar = global::UnityEngine.Object.Instantiate(starTemplate, starParent);
			newStar.SetActive(value: true);
			newStar.hideFlags = HideFlags.HideAndDontSave;
			stars[j] = (RectTransform)newStar.transform;
		}
		UpdatePositions();
	}

	private void UpdatePositions()
	{
		Rect rect = _rt.rect;
		float w = rect.width;
		float h = rect.height;
		if (_currentPositions == null || _currentPositions.Length != starCount)
		{
			_currentPositions = new Vector2[starCount];
			for (int i = 0; i < starCount; i++)
			{
				_currentPositions[i] = starPositions[i];
			}
		}
		if (_targetPositions == null || _targetPositions.Length != starCount)
		{
			_targetPositions = new Vector2[starCount];
		}
		if (_currentVelocities == null || _currentVelocities.Length != starCount)
		{
			_currentVelocities = new Vector2[starCount];
		}
		if (_points == null || _points.Length != edges.Count * 2)
		{
			_points = new Vector2[edges.Count * 2];
		}
		if (!disableAnimation)
		{
			if ((Time.unscaledTime - _lastStarPositionChangeTime) * timeScale > starPositionChangeTime)
			{
				_lastStarPositionChangeTime = Time.unscaledTime;
				for (int j = 0; j < starCount; j++)
				{
					_targetPositions[j] = starPositions[j] + new Vector2(fluctuateMagnitude.x * global::UnityEngine.Random.Range(-1f, 1f), fluctuateMagnitude.y * global::UnityEngine.Random.Range(-1f, 1f));
				}
			}
			for (int k = 0; k < starCount; k++)
			{
				if (Application.IsPlaying(this))
				{
					_currentPositions[k] = Vector2.SmoothDamp(_currentPositions[k], _targetPositions[k], ref _currentVelocities[k], fluctuateSmoothTime, float.PositiveInfinity, Time.unscaledDeltaTime * timeScale);
				}
				else
				{
					_currentPositions[k] = starPositions[k];
				}
			}
		}
		for (int l = 0; l < starCount; l++)
		{
			Vector2 starPos = _currentPositions[l];
			starPos.x *= w;
			starPos.y *= h;
			stars[l].anchoredPosition = starPos;
		}
		for (int m = 0; m < edges.Count; m++)
		{
			_points[m * 2] = _currentPositions[edges[m].a];
			_points[m * 2 + 1] = _currentPositions[edges[m].b];
		}
		lineRenderer.Points = _points;
		lineRenderer.SetVerticesDirty();
	}

	private void Update()
	{
		if (alphaRefRenderer.GetInheritedAlpha() > 0.0001f)
		{
			UpdatePositions();
		}
	}
}
