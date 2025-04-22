using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(3000)]
public class UI_InGame_TimerDisplay_Item : LogicBehaviour
{
	public TextMeshProUGUI nameText;

	public TextMeshProUGUI multipleTargetText;

	public Image fillImage;

	public RectTransform otherTargetTickPrefab;

	public RectTransform otherTargetTickParent;

	internal List<OnScreenTimerHandle> _targets = new List<OnScreenTimerHandle>();

	internal List<RectTransform> _otherTargetTicks = new List<RectTransform>();

	private UI_InGame_TimerDisplay _parent;

	public void Setup(OnScreenTimerHandle target, UI_InGame_TimerDisplay parent)
	{
		otherTargetTickPrefab.gameObject.SetActive(value: false);
		NetworkedManagerBase<ClientEventManager>.instance.OnHideOnScreenTimer += new Action<OnScreenTimerHandle>(OnHideTimer);
		_targets.Clear();
		_targets.Add(target);
		_parent = parent;
		nameText.text = ((target.rawTextGetter != null) ? target.rawTextGetter() : target.rawText);
		multipleTargetText.gameObject.SetActive(value: false);
		if (target.color != default(Color))
		{
			fillImage.color = Color.Lerp(target.color.WithV(1f), Color.black, 0.35f);
		}
		UpdateTimer();
		if (_parent != null)
		{
			_parent._instances.Add(this);
		}
	}

	public void AddTarget(OnScreenTimerHandle target)
	{
		_targets.Add(target);
	}

	private void OnHideTimer(OnScreenTimerHandle obj)
	{
		if (!(this == null) && _targets.Contains(obj))
		{
			_targets.Remove(obj);
			if (_targets.Count == 0)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ClientEventManager>.instance != null)
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnHideOnScreenTimer -= new Action<OnScreenTimerHandle>(OnHideTimer);
		}
		if (_parent != null)
		{
			_parent._instances.Remove(this);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateTimer();
	}

	private void UpdateTimer()
	{
		try
		{
			int neededTicks = Mathf.Max(0, _targets.Count - 1);
			while (_otherTargetTicks.Count > neededTicks)
			{
				global::UnityEngine.Object.Destroy(_otherTargetTicks[0].gameObject);
				_otherTargetTicks.RemoveAt(0);
			}
			while (_otherTargetTicks.Count < neededTicks)
			{
				RectTransform otherTargetTick = global::UnityEngine.Object.Instantiate(otherTargetTickPrefab, otherTargetTickParent);
				otherTargetTick.gameObject.SetActive(value: true);
				_otherTargetTicks.Add(otherTargetTick);
			}
			if (_targets.Count == 0)
			{
				return;
			}
			float biggestValue = float.NegativeInfinity;
			int biggestIndex = -1;
			for (int i = 0; i < _targets.Count; i++)
			{
				float v = _targets[i].fillAmountGetter();
				if (v > biggestValue)
				{
					biggestIndex = i;
					biggestValue = v;
				}
			}
			for (int j = 0; j < _otherTargetTicks.Count; j++)
			{
				int targetIndex = j;
				if (j >= biggestIndex)
				{
					targetIndex++;
				}
				float v2 = _targets[targetIndex].fillAmountGetter();
				_otherTargetTicks[j].anchorMin = _otherTargetTicks[j].anchorMin.WithX(v2);
				_otherTargetTicks[j].anchorMax = _otherTargetTicks[j].anchorMax.WithX(v2);
				_otherTargetTicks[j].anchoredPosition = _otherTargetTicks[j].anchoredPosition.WithX(0f);
			}
			fillImage.fillAmount = biggestValue;
			if (_targets[0].rawTextGetter != null)
			{
				nameText.text = _targets[0].rawTextGetter();
			}
			if (_targets.Count > 1)
			{
				multipleTargetText.gameObject.SetActive(value: true);
				multipleTargetText.text = $"x{_targets.Count:###,0}";
			}
			else
			{
				multipleTargetText.gameObject.SetActive(value: false);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}
}
