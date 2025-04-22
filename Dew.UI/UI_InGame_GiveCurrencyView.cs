using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_GiveCurrencyView : View
{
	public TextMeshProUGUI goldText;

	public TextMeshProUGUI dreamDustText;

	public TextMeshProUGUI summaryText;

	public Button confirmButton;

	public Transform goldButtonsParent;

	public Transform dreamDustButtonsParent;

	[NonSerialized]
	public DewPlayer giveTarget;

	private int _goldAmount;

	private int _dreamDustAmount;

	protected override void Start()
	{
		base.Start();
		UI_HoldToRapidClickButton[] componentsInChildren = goldButtonsParent.GetComponentsInChildren<UI_HoldToRapidClickButton>(includeInactive: true);
		foreach (UI_HoldToRapidClickButton b in componentsInChildren)
		{
			if (!int.TryParse(b.name, out var amount))
			{
				return;
			}
			b.onClick.AddListener(delegate
			{
				SetValue(_goldAmount + amount, _dreamDustAmount);
			});
		}
		componentsInChildren = dreamDustButtonsParent.GetComponentsInChildren<UI_HoldToRapidClickButton>(includeInactive: true);
		foreach (UI_HoldToRapidClickButton b2 in componentsInChildren)
		{
			if (!int.TryParse(b2.name, out var amount2))
			{
				return;
			}
			b2.onClick.AddListener(delegate
			{
				SetValue(_goldAmount, _dreamDustAmount + amount2);
			});
		}
		confirmButton.onClick.AddListener(Confirm);
	}

	protected override void OnShow()
	{
		base.OnShow();
		SetValue(0, 0);
	}

	private void Confirm()
	{
		Hide();
		if (!(giveTarget == null) && !(giveTarget.hero == null) && !(DewPlayer.local == null))
		{
			DewPlayer.local.CmdGiveCurrency(_goldAmount, _dreamDustAmount, giveTarget);
		}
	}

	private void SetValue(int gold, int dreamDust)
	{
		confirmButton.interactable = false;
		if (giveTarget == null || giveTarget.hero == null || DewPlayer.local == null)
		{
			return;
		}
		_goldAmount = Mathf.Clamp(gold, 0, DewPlayer.local.gold);
		_dreamDustAmount = Mathf.Clamp(dreamDust, 0, DewPlayer.local.dreamDust);
		goldText.text = $"{_goldAmount:#,##0} <alpha=151><size=70%>/ {DewPlayer.local.gold:#,##0}";
		dreamDustText.text = $"{_dreamDustAmount:#,##0} <alpha=151><size=70%>/ {DewPlayer.local.dreamDust:#,##0}";
		string pName = ChatManager.GetColoredDescribedPlayerName(giveTarget);
		if (_goldAmount <= 0 && _dreamDustAmount <= 0)
		{
			summaryText.text = string.Format(DewLocalization.GetUIValue("InGame_GiveCurrency_GiveToPlayer_Nothing"), pName);
		}
		else if (_goldAmount > 0 && _dreamDustAmount <= 0)
		{
			summaryText.text = string.Format(DewLocalization.GetUIValue("InGame_GiveCurrency_GiveToPlayer_Gold"), pName, _goldAmount.ToString("#,##0"));
		}
		else if (_goldAmount <= 0 && _dreamDustAmount > 0)
		{
			summaryText.text = string.Format(DewLocalization.GetUIValue("InGame_GiveCurrency_GiveToPlayer_DreamDust"), pName, _dreamDustAmount.ToString("#,##0"));
		}
		else
		{
			summaryText.text = string.Format(DewLocalization.GetUIValue("InGame_GiveCurrency_GiveToPlayer_GoldAndDreamDust"), pName, _goldAmount.ToString("#,##0"), _dreamDustAmount.ToString("#,##0"));
		}
		confirmButton.interactable = _goldAmount > 0 || _dreamDustAmount > 0;
		Button[] componentsInChildren = goldButtonsParent.GetComponentsInChildren<Button>(includeInactive: true);
		foreach (Button b in componentsInChildren)
		{
			if (!int.TryParse(b.name, out var amount))
			{
				return;
			}
			if (amount < 0)
			{
				b.interactable = _goldAmount > 0;
			}
			else
			{
				b.interactable = _goldAmount < DewPlayer.local.gold;
			}
		}
		componentsInChildren = dreamDustButtonsParent.GetComponentsInChildren<Button>(includeInactive: true);
		foreach (Button b2 in componentsInChildren)
		{
			if (!int.TryParse(b2.name, out var amount2))
			{
				break;
			}
			if (amount2 < 0)
			{
				b2.interactable = _dreamDustAmount > 0;
			}
			else
			{
				b2.interactable = _dreamDustAmount < DewPlayer.local.dreamDust;
			}
		}
	}
}
