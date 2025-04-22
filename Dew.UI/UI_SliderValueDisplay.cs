using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SliderValueDisplay : MonoBehaviour
{
	public string format;

	private Slider _slider;

	private TextMeshProUGUI _text;

	private void Awake()
	{
		_slider = GetComponentInParent<Slider>();
		_text = GetComponent<TextMeshProUGUI>();
		_slider.onValueChanged.AddListener(delegate
		{
			UpdateValue();
		});
		UpdateValue();
	}

	private void UpdateValue()
	{
		_text.text = _slider.value.ToString(format);
	}
}
