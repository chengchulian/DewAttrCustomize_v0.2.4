using System.Collections.Generic;
using UnityEngine;

[LogicUpdatePriority(100)]
public class UI_TickedBar : LogicBehaviour
{
	public RectTransform smallTickTemplate;

	public RectTransform bigTickTemplate;

	private float _lastMax;

	private List<RectTransform> _bigTicks = new List<RectTransform>();

	private List<RectTransform> _smallTicks = new List<RectTransform>();

	private Vector2 _screen;

	private void Awake()
	{
		smallTickTemplate.gameObject.SetActive(value: false);
		bigTickTemplate.gameObject.SetActive(value: false);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_screen != new Vector2(Screen.width, Screen.height))
		{
			_screen = new Vector2(Screen.width, Screen.height);
			SetMaxValue(_lastMax, force: true);
		}
	}

	public void SetMaxValue(float value, bool force = false)
	{
		if (value < 0f)
		{
			value = 0f;
		}
		force = true;
		if (force || _lastMax != value)
		{
			_lastMax = value;
			int bigTickSepValue = 500;
			int smallTickSepValue = 50;
			int smallTickPixelWidth = 1;
			int bigTickPixelWidth = 1;
			int num = (int)value;
			int bigTicks = num / bigTickSepValue;
			int smallTicks = num / smallTickSepValue - bigTicks;
			if (smallTicks > 40)
			{
				smallTicks = 0;
			}
			while (bigTicks > 40)
			{
				bigTicks /= 10;
				bigTickSepValue *= 10;
				bigTickPixelWidth++;
			}
			while (_bigTicks.Count > bigTicks)
			{
				Object.Destroy(_bigTicks[_bigTicks.Count - 1].gameObject);
				_bigTicks.RemoveAt(_bigTicks.Count - 1);
			}
			while (_bigTicks.Count < bigTicks)
			{
				RectTransform item = Object.Instantiate(bigTickTemplate, base.transform);
				item.gameObject.SetActive(value: true);
				_bigTicks.Add(item);
			}
			while (_smallTicks.Count > smallTicks)
			{
				Object.Destroy(_smallTicks[_smallTicks.Count - 1].gameObject);
				_smallTicks.RemoveAt(_smallTicks.Count - 1);
			}
			while (_smallTicks.Count < smallTicks)
			{
				RectTransform item2 = Object.Instantiate(smallTickTemplate, base.transform);
				item2.gameObject.SetActive(value: true);
				_smallTicks.Add(item2);
			}
			float scale = base.transform.lossyScale.x;
			for (int i = 0; i < bigTicks; i++)
			{
				float normalizedPos = (float)((i + 1) * bigTickSepValue) / value;
				_bigTicks[i].anchorMin = _bigTicks[i].anchorMin.WithX(normalizedPos);
				_bigTicks[i].anchorMax = _bigTicks[i].anchorMax.WithX(normalizedPos);
				_bigTicks[i].anchoredPosition = _bigTicks[i].anchoredPosition.WithX(0f);
				_bigTicks[i].sizeDelta = _bigTicks[i].sizeDelta.WithX((float)bigTickPixelWidth / scale);
			}
			for (int j = 0; j < smallTicks; j++)
			{
				float normalizedPos2 = (float)((j + 1 + (j + 1) / (bigTickSepValue / smallTickSepValue)) * smallTickSepValue) / value;
				_smallTicks[j].anchorMin = _smallTicks[j].anchorMin.WithX(normalizedPos2);
				_smallTicks[j].anchorMax = _smallTicks[j].anchorMax.WithX(normalizedPos2);
				_smallTicks[j].anchoredPosition = _smallTicks[j].anchoredPosition.WithX(0f);
				_smallTicks[j].sizeDelta = _smallTicks[j].sizeDelta.WithX((float)smallTickPixelWidth / scale);
			}
		}
	}
}
