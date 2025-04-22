using UnityEngine;
using UnityEngine.UI;

public class UI_SliderValueDisplay_Hue : MonoBehaviour
{
	public float offset;

	private Slider _slider;

	private Image _image;

	private void Awake()
	{
		_slider = GetComponentInParent<Slider>();
		_image = GetComponent<Image>();
		_slider.onValueChanged.AddListener(delegate
		{
			UpdateValue();
		});
		UpdateValue();
	}

	private void UpdateValue()
	{
		_image.color = Color.HSVToRGB(Mathf.Repeat(offset + _slider.value, 1f), 0.7f, 1f);
	}
}
