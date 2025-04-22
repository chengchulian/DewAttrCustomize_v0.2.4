using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_Reveries_SlotBase : LogicBehaviour
{
	public GameObject nonEmptyObject;

	public GameObject emptyObject;

	public GameObject[] completeObjects;

	public GameObject[] notCompleteObjects;

	public TextMeshProUGUI descriptionText;

	public GameObject progressObject;

	public Image progressFill;

	public TextMeshProUGUI progressText;

	public GameObject giftObject;

	public GameObject multipleGiftsObject;

	public TextMeshProUGUI multipleGiftsCountText;

	public GameObject stardustObject;

	public TextMeshProUGUI stardustAmountText;

	public GameObject specialButtonObject;

	public TextMeshProUGUI specialButtonText;

	[Space(30f)]
	public GameObject fxNewQuest;

	public GameObject fxClaimReward;

	public GameObject fxFlash;

	public GameObject fxRemove;

	public DewReverieItem instance;

	public abstract DewProfile.ReverieDataBase data { get; }

	protected virtual void Awake()
	{
		nonEmptyObject.SetActive(value: false);
		emptyObject.SetActive(value: true);
	}

	public virtual void Refresh()
	{
		if (string.IsNullOrEmpty(data.type))
		{
			nonEmptyObject.SetActive(value: false);
			emptyObject.SetActive(value: true);
			return;
		}
		instance = (DewReverieItem)Activator.CreateInstance(Dew.reveriesByName[data.type].GetType());
		instance.LoadState(data.persistentVariables);
		descriptionText.text = DewLocalization.ProcessGenericBacktickedString(DewLocalization.GetUIValue(data.type + "_QuestDescription"), instance);
		stardustObject.SetActive(data.grantedStardust > 0);
		stardustAmountText.text = data.grantedStardust.ToString("###,0");
		string[] items = data.grantedItems;
		giftObject.SetActive(items != null && items.Length != 0);
		multipleGiftsObject.SetActive(items != null && items.Length > 1);
		multipleGiftsCountText.text = ((items == null) ? "" : items.Length.ToString("###,0"));
		nonEmptyObject.SetActive(value: true);
		emptyObject.SetActive(value: false);
		specialButtonObject.SetActive(value: false);
		RefreshEveryTick();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		RefreshEveryTick();
	}

	private void RefreshEveryTick()
	{
		if (data != null && !string.IsNullOrEmpty(data.type))
		{
			progressText.text = (data.isComplete ? "" : $"{data.currentProgress:###,0} / {data.maxProgress:###,0}");
			progressFill.fillAmount = (data.isComplete ? 1f : ((float)data.currentProgress / (float)data.maxProgress));
			completeObjects.SetActiveAll(data.isComplete);
			notCompleteObjects.SetActiveAll(!data.isComplete);
			string bt = instance.reverieListButtonText;
			if (!data.isComplete && !string.IsNullOrEmpty(bt))
			{
				progressObject.SetActive(value: false);
				specialButtonObject.SetActive(value: true);
				specialButtonText.text = bt;
			}
			else
			{
				progressObject.SetActive(value: true);
				specialButtonObject.SetActive(value: false);
			}
		}
	}

	public async void ClaimReward()
	{
		bool hadItems = data.grantedItems != null && data.grantedItems.Length != 0;
		if (!(await DewReverie.ReceiveRewardOfReverie(data)))
		{
			return;
		}
		OnSuccessfullyClaimedReward();
		if (this == null)
		{
			return;
		}
		DewEffect.Play(fxClaimReward);
		DewEffect.Play(fxFlash);
		DewEffect.Play(fxRemove);
		Refresh();
		if (hadItems)
		{
			if (SingletonBehaviour<UI_NametagList>.instance != null)
			{
				SingletonBehaviour<UI_NametagList>.instance.RefreshList();
			}
			if (SingletonBehaviour<UI_AccList>.instance != null)
			{
				SingletonBehaviour<UI_AccList>.instance.RefreshList();
			}
			if (SingletonBehaviour<UI_EmoteList>.instance != null)
			{
				SingletonBehaviour<UI_EmoteList>.instance.RefreshList();
			}
		}
	}

	public void ClickButton()
	{
		try
		{
			instance.OnReverieListButtonClick();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	protected void HandleNewQuest()
	{
		DewEffect.Play(fxNewQuest);
		DewEffect.Play(fxFlash);
		Refresh();
	}

	protected virtual void OnSuccessfullyClaimedReward()
	{
	}
}
