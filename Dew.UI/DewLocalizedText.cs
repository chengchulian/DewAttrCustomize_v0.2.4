using TMPro;
using UnityEngine;

public class DewLocalizedText : MonoBehaviour, ILangaugeChangedCallback
{
	public string key;

	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		UpdateText();
	}

	private void OnValidate()
	{
		UpdateText();
	}

	public void UpdateText()
	{
		if (_text == null)
		{
			_text = GetComponent<TextMeshProUGUI>();
		}
		_text.text = DewLocalization.GetUIValue(key);
	}

	public void OnLanguageChanged()
	{
		UpdateText();
	}
}
