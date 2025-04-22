using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Flasher : MonoBehaviour
{
	public Color flashColor;

	public float flashDuration = 0.5f;

	private Image _image;

	private Color _originalColor;

	private void Awake()
	{
		_image = GetComponent<Image>();
		_originalColor = _image.color;
	}

	private void OnEnable()
	{
		Flash();
	}

	public void Flash()
	{
		_image.color = flashColor;
		DOTween.Kill(_image);
		_image.DOColor(_originalColor, flashDuration).SetUpdate(isIndependentUpdate: true);
	}
}
