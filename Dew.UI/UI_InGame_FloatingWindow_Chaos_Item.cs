using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_FloatingWindow_Chaos_Item : MonoBehaviour, IPingableChoiceItem
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descText;

	public Image rarityImage;

	public PerRarityData<Color> textColor;

	private int _index;

	Object IPingableChoiceItem.shrine => ManagerBase<FloatingWindowManager>.instance.currentTarget as Shrine_Chaos;

	int IPingableChoiceItem.choiceIndex => _index;

	public void UpdateContent(ChaosReward d, int index)
	{
		titleText.text = DewLocalization.GetUIValue("InGame_Chaos_" + d.type);
		titleText.color = Dew.GetRarityColor(d.rarity);
		descText.text = string.Format(DewLocalization.GetUIValue("InGame_Chaos_" + d.type.ToString() + "_Description"), d.quantity.ToString("#,##0"));
		rarityImage.color = Dew.GetRarityColor(d.rarity);
		_index = index;
	}

	public void Click()
	{
		GetComponentInParent<UI_InGame_FloatingWindow_Chaos>().ClickChaosItem(_index);
	}
}
