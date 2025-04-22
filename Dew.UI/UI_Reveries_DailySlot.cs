using System;
using TMPro;
using UnityEngine;

public class UI_Reveries_DailySlot : UI_Reveries_SlotBase
{
	public int index;

	public TextMeshProUGUI timeLeftForRefillText;

	public GameObject rerollButtonObject;

	public GameObject removeButtonObject;

	public override DewProfile.ReverieDataBase data => DewSave.profile.reverieSlots[index];

	protected override void Awake()
	{
		base.Awake();
		timeLeftForRefillText.text = "";
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		DewProfile.DailyReverieData d = (DewProfile.DailyReverieData)data;
		rerollButtonObject.SetActive(!d.isComplete && DewSave.profile.remainingRerolls > 0);
		removeButtonObject.SetActive(!d.isComplete && DewSave.profile.remainingRerolls <= 0);
		if (!string.IsNullOrEmpty(d.type))
		{
			return;
		}
		long now = DateTime.UtcNow.ToTimestamp();
		long longDiff = d.nextRefillTimestamp - now;
		if (longDiff > 0)
		{
			TimeSpan timespanDiff = longDiff.ToTimeSpan();
			timeLeftForRefillText.text = timespanDiff.ToString("hh\\:mm\\:ss");
		}
		else if (d.wasNeverFilled)
		{
			if (DewBuildProfile.current.buildType == BuildType.DemoLite)
			{
				if (index == 0)
				{
					DewReverie.SetDailyReverie<Rev_AddToWishlist>(index);
				}
				if (index == 1)
				{
					DewReverie.SetDailyReverie<Rev_JoinDiscord>(index);
				}
				if (index == 2)
				{
					DewReverie.SetDailyReverie<Rev_PlayThreeTimes>(index);
				}
			}
			else
			{
				if (index == 0)
				{
					DewReverie.SetDailyReverie<Rev_PlayThreeTimes>(index);
				}
				if (index == 1)
				{
					DewReverie.SetDailyReverie<Rev_JoinDiscord>(index);
				}
				if (index == 2)
				{
					DewReverie.SetDailyReverie<Rev_KillHeroicBosses>(index);
				}
			}
			HandleNewQuest();
		}
		else
		{
			DewReverie.SetRandomDailyReverie(index);
			HandleNewQuest();
		}
	}

	public void RerollOrRemove()
	{
		if (string.IsNullOrEmpty(DewSave.profile.reverieSlots[index].type))
		{
			return;
		}
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			rawContent = DewLocalization.GetUIValue((DewSave.profile.remainingRerolls > 0) ? "Reverie_Message_RerollUsingDice" : "Reverie_Message_AbandonThisReverie") + $"\n({DewSave.profile.remainingRerolls}/{3})",
			defaultButton = DewMessageSettings.ButtonType.Cancel,
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
			owner = this,
			destructiveConfirm = true,
			onClose = delegate(DewMessageSettings.ButtonType b)
			{
				if (b == DewMessageSettings.ButtonType.Yes)
				{
					DewEffect.Play(fxFlash);
					if (DewSave.profile.remainingRerolls > 0)
					{
						DewSave.profile.remainingRerolls--;
						DewReverie.SetRandomDailyReverie(index);
						HandleNewQuest();
					}
					else
					{
						DewEffect.PlayNew(fxRemove);
						DewReverie.ClearDailyReverie(index);
						Refresh();
					}
				}
			}
		});
	}

	public override void Refresh()
	{
		base.Refresh();
		if (instance != null)
		{
			instance.index = index;
		}
	}

	protected override void OnSuccessfullyClaimedReward()
	{
		base.OnSuccessfullyClaimedReward();
		DewReverie.ClearDailyReverie(index);
	}
}
