using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Toggle : MonoBehaviour
{
	public int index;

	public GameObject onObject;

	public GameObject offObject;

	public UnityEvent<bool> onIsCheckedChanged;

	public UnityEvent onClick;

	public bool doNotToggleOnClick;

	[SerializeField]
	private bool _isChecked;

	private Button _button;

	public bool interactable
	{
		get
		{
			return _button.interactable;
		}
		set
		{
			_button.interactable = value;
		}
	}

	public bool isChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			UI_ToggleGroup parent = GetComponentInParent<UI_ToggleGroup>();
			if (_isChecked == value || (parent != null && parent.currentIndex == index && !value))
			{
				return;
			}
			_isChecked = value;
			if (onObject != null)
			{
				onObject.SetActive(_isChecked);
			}
			if (offObject != null)
			{
				offObject.SetActive(!_isChecked);
			}
			if (_isChecked && parent != null)
			{
				parent.currentIndex = index;
			}
			try
			{
				onIsCheckedChanged?.Invoke(value);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	private void Start()
	{
		UI_ToggleGroup parent = GetComponentInParent<UI_ToggleGroup>();
		if (parent != null && isChecked != (parent.currentIndex == index))
		{
			isChecked = parent.currentIndex == index;
		}
		if (onObject != null)
		{
			onObject.SetActive(_isChecked);
		}
		if (offObject != null)
		{
			offObject.SetActive(!_isChecked);
		}
		_button.onClick.AddListener(delegate
		{
			if (!doNotToggleOnClick)
			{
				isChecked = !isChecked;
			}
			try
			{
				onClick?.Invoke();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		});
	}
}
