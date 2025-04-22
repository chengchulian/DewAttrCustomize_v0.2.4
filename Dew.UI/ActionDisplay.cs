using TMPro;
using UnityEngine;

public class ActionDisplay : MonoBehaviour
{
	[SerializeField]
	private bool _isDisabled;

	public TextMeshProUGUI text;

	private Color _originalColor;

	public bool isDisabled
	{
		get
		{
			return _isDisabled;
		}
		set
		{
			if (_isDisabled != value)
			{
				_isDisabled = value;
				UpdateDisabledStatus();
			}
		}
	}

	private void Awake()
	{
		UpdateDisabledStatus();
	}

	private void UpdateDisabledStatus()
	{
		if (_originalColor == default(Color))
		{
			_originalColor = text.color;
		}
		GetComponentInChildren<ButtonDisplay>(includeInactive: true).isDisabled = isDisabled;
		text.color = (isDisabled ? (Color.gray * _originalColor) : _originalColor);
	}
}
