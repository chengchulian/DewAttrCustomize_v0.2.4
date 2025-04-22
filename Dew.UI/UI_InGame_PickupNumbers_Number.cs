using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_InGame_PickupNumbers_Number : LogicBehaviour
{
	public string localeKey;

	public Vector3 worldOffset;

	public Vector3 floatSpeed;

	public float startTime;

	public float sustainTime;

	public float decayTime;

	public float mergePunch;

	public float mergePunchDuration;

	private TextMeshProUGUI _text;

	private List<UI_InGame_PickupNumbers_Number> _mergeable;

	private int _amount;

	public float elapsedTime { get; private set; }

	private void Awake()
	{
		GetComponent(out _text);
	}

	private void Start()
	{
		DOTween.Kill(base.transform);
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(Vector3.one, startTime);
	}

	private void OnDestroy()
	{
		_mergeable.Remove(this);
	}

	public void Merge(int added)
	{
		Setup(_amount + added, _mergeable);
		elapsedTime = startTime;
		DOTween.Kill(base.transform);
		base.transform.localScale = Vector3.one;
		base.transform.DOPunchScale(Vector3.one * mergePunch, mergePunchDuration);
	}

	public void Setup(int amount, List<UI_InGame_PickupNumbers_Number> mergeable)
	{
		_amount = amount;
		_mergeable = mergeable;
		if (amount >= 0)
		{
			_text.text = $"+{amount:#,##0} {DewLocalization.GetUIValue(localeKey)}";
		}
		else
		{
			_text.text = $"{amount:#,##0} {DewLocalization.GetUIValue(localeKey)}";
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		base.transform.position += floatSpeed * Time.deltaTime;
		if (elapsedTime < startTime + sustainTime && elapsedTime + Time.deltaTime > startTime + sustainTime)
		{
			_mergeable.Remove(this);
			DOTween.Kill(base.transform);
			base.transform.DOScale(0f, decayTime);
			Object.Destroy(base.gameObject, decayTime);
		}
		elapsedTime += Time.deltaTime;
	}

	private void LateUpdate()
	{
		RefreshPosition();
	}

	private void RefreshPosition()
	{
		if (!(DewPlayer.local == null) && !(DewPlayer.local.hero == null))
		{
			Hero h = DewPlayer.local.hero;
			Vector3 pos = Dew.mainCamera.WorldToScreenPoint(h.position + worldOffset + floatSpeed * elapsedTime);
			pos.Quantitize();
			base.transform.position = pos;
		}
	}
}
