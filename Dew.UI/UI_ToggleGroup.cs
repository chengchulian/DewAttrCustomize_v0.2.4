using System;
using UnityEngine;
using UnityEngine.Events;

public class UI_ToggleGroup : MonoBehaviour
{
	public UnityEvent<int> onCurrentIndexChanged;

	[SerializeField]
	private int _currentIndex;

	public int currentIndex
	{
		get
		{
			return _currentIndex;
		}
		set
		{
			if (_currentIndex == value)
			{
				return;
			}
			_currentIndex = value;
			UI_Toggle[] componentsInChildren = GetComponentsInChildren<UI_Toggle>();
			foreach (UI_Toggle c in componentsInChildren)
			{
				bool val = _currentIndex == c.index;
				if (c.isChecked != val)
				{
					c.isChecked = val;
				}
			}
			try
			{
				onCurrentIndexChanged?.Invoke(value);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	private void Start()
	{
		UI_Toggle[] componentsInChildren = GetComponentsInChildren<UI_Toggle>();
		foreach (UI_Toggle c in componentsInChildren)
		{
			bool val = _currentIndex == c.index;
			if (c.isChecked != val)
			{
				c.isChecked = val;
			}
		}
	}
}
