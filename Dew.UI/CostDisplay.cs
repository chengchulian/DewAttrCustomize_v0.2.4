using TMPro;
using UnityEngine;

public class CostDisplay : MonoBehaviour
{
	public GameObject goldObject;

	public TextMeshProUGUI goldCostText;

	public GameObject dreamDustObject;

	public TextMeshProUGUI dreamDustCostText;

	public GameObject stardustObject;

	public TextMeshProUGUI stardustCostText;

	public GameObject healthObject;

	public TextMeshProUGUI healthCostText;

	public GameObject freeObject;

	private Color _goldCostColor;

	private Color _dreamDustCostColor;

	private Color _starDustCostColor;

	private void Awake()
	{
		if (_goldCostColor == default(Color))
		{
			_goldCostColor = goldCostText.color;
			_dreamDustCostColor = dreamDustCostText.color;
			_starDustCostColor = stardustCostText.color;
		}
	}

	public void SetupDreamDust(int amount, bool showCantAfford = true, bool showPlusSign = false)
	{
		Setup(Cost.DreamDust(amount), showCantAfford, showPlusSign);
	}

	public void SetupGold(int amount, bool showCantAfford = true, bool showPlusSign = false)
	{
		Setup(Cost.Gold(amount), showCantAfford, showPlusSign);
	}

	public void Setup(Cost cost, bool showCantAfford = true, bool showPlusSign = false)
	{
		if (_goldCostColor == default(Color))
		{
			_goldCostColor = goldCostText.color;
			_dreamDustCostColor = dreamDustCostText.color;
			_starDustCostColor = stardustCostText.color;
		}
		goldObject.SetActive(cost.gold != 0);
		dreamDustObject.SetActive(cost.dreamDust != 0);
		stardustObject.SetActive(cost.stardust != 0);
		healthObject.SetActive(cost.healthPercentage != 0);
		if (cost.gold == 0 && cost.dreamDust == 0 && cost.stardust == 0 && cost.healthPercentage == 0)
		{
			freeObject.SetActive(value: true);
			return;
		}
		freeObject.SetActive(value: false);
		string numberFormat = GetNumberFormat(showPlusSign);
		goldCostText.text = cost.gold.ToString(numberFormat);
		dreamDustCostText.text = cost.dreamDust.ToString(numberFormat);
		stardustCostText.text = cost.stardust.ToString(numberFormat);
		healthCostText.text = string.Format(DewLocalization.GetUIValue("InGame_Interact_HealthCost"), cost.healthPercentage.ToString(numberFormat));
		if (showCantAfford)
		{
			Color cantAffordColor = new Color(1f, 0.55f, 0.55f);
			goldCostText.color = ((DewPlayer.local == null || DewPlayer.local.gold >= Mathf.Abs(cost.gold)) ? _goldCostColor : cantAffordColor);
			dreamDustCostText.color = ((DewPlayer.local == null || DewPlayer.local.dreamDust >= Mathf.Abs(cost.dreamDust)) ? _dreamDustCostColor : cantAffordColor);
			stardustCostText.color = ((DewSave.profile.stardust >= Mathf.Abs(cost.stardust)) ? _starDustCostColor : cantAffordColor);
		}
		else
		{
			goldCostText.color = _goldCostColor;
			dreamDustCostText.color = _dreamDustCostColor;
			stardustCostText.color = _starDustCostColor;
		}
	}

	private string GetNumberFormat(bool showPlusSign)
	{
		if (showPlusSign)
		{
			return "+#,##0;-#,##0";
		}
		return "#,##0";
	}
}
