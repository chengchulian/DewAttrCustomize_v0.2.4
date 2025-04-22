using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_EntityGenericStackIndicator : LogicBehaviour
{
	public GameObject bgTemplate;

	public Transform bgParent;

	public CanvasGroup fillTemplate;

	public Transform fillParent;

	private List<(GameObject, CanvasGroup)> _items = new List<(GameObject, CanvasGroup)>();

	private UI_EntityProvider _provider;

	private CanvasGroup _cg;

	public Entity target
	{
		get
		{
			if (!(_provider != null))
			{
				return null;
			}
			return _provider.target;
		}
	}

	private void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		bgTemplate.SetActive(value: false);
		fillTemplate.gameObject.SetActive(value: false);
		_cg = GetComponent<CanvasGroup>();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (target == null)
		{
			return;
		}
		_cg.alpha = ((target.Visual.genericStackIndicatorMax > 0) ? 1 : 0);
		while (_items.Count < target.Visual.genericStackIndicatorMax)
		{
			(GameObject, CanvasGroup) newItem = (Object.Instantiate(bgTemplate, bgParent), Object.Instantiate(fillTemplate, fillParent));
			_items.Add(newItem);
			newItem.Item2.alpha = 0.35f;
			Image component = newItem.Item2.transform.GetChild(0).GetComponent<Image>();
			component.material = Object.Instantiate(component.material);
			component.material.SetTextureOffset("_DistortTex", new Vector2((float)_items.Count * 0.127f, (float)_items.Count * -0.61f));
			newItem.Item1.SetActive(value: true);
			newItem.Item2.gameObject.SetActive(value: true);
		}
		while (_items.Count > target.Visual.genericStackIndicatorMax)
		{
			(GameObject, CanvasGroup) tuple = _items[_items.Count - 1];
			Object.Destroy(tuple.Item1);
			Object.Destroy(tuple.Item2.transform.GetChild(0).GetComponent<Image>().material);
			Object.Destroy(tuple.Item2.gameObject);
			_items.RemoveAt(_items.Count - 1);
		}
		int stack = target.Visual.genericStackIndicatorValue;
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].Item2.alpha < 0.2f && i < stack)
			{
				_items[i].Item2.alpha = 1f;
				_items[i].Item2.DOKill(complete: true);
				_items[i].Item2.DOFade(0.3f, 0.45f);
			}
			if (_items[i].Item2.alpha > 0.2f && i >= stack)
			{
				_items[i].Item2.alpha = 0f;
				_items[i].Item2.DOKill(complete: true);
			}
		}
	}
}
