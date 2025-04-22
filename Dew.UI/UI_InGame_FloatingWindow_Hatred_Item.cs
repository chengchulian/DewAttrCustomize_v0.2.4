using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_FloatingWindow_Hatred_Item : MonoBehaviour, IPingableChoiceItem
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI subtitleText;

	public Image[] backdropImages;

	public Color[] colorsByStrength;

	private Color[] _imageOriginalColors;

	private Color _titleTextOriginalColor;

	private Color _subtitleTextOriginalColor;

	private int _index;

	public HatredStrengthType type { get; private set; }

	Object IPingableChoiceItem.shrine => ManagerBase<FloatingWindowManager>.instance.currentTarget as Shrine_Hatred;

	int IPingableChoiceItem.choiceIndex => _index;

	public void UpdateContent(HatredStrengthType t, int index)
	{
		if (_imageOriginalColors == null)
		{
			_imageOriginalColors = new Color[backdropImages.Length];
			for (int i = 0; i < backdropImages.Length; i++)
			{
				_imageOriginalColors[i] = backdropImages[i].color;
			}
			_titleTextOriginalColor = titleText.color;
			_subtitleTextOriginalColor = subtitleText.color;
		}
		type = t;
		_index = index;
		Color color = colorsByStrength.GetValue(t);
		for (int j = 0; j < backdropImages.Length; j++)
		{
			backdropImages[j].color = _imageOriginalColors[j] * color;
		}
		titleText.color = _titleTextOriginalColor * color;
		subtitleText.color = Color.Lerp(_subtitleTextOriginalColor * color, new Color(1f, 1f, 1f, _subtitleTextOriginalColor.a), 0.5f);
		titleText.text = DewLocalization.GetUIValue($"InGame_Hatred_Type_{type}Hatred");
		subtitleText.text = DewLocalization.GetUIValue($"InGame_Hatred_Type_{type}Hatred_Description");
	}

	public void Click()
	{
		GetComponentInParent<UI_InGame_FloatingWindow_Hatred>().ClickChoice(_index);
	}
}
