using System;
using UnityEngine;

public class UI_Reveries : LogicBehaviour, ISettingsChangedCallback, ILangaugeChangedCallback
{
	public GameObject specialReverieObject;

	public UI_Reveries_DailySlot[] slots;

	public void OnSettingsChanged()
	{
		FullRefresh();
	}

	private void Start()
	{
		FullRefresh();
	}

	public void OnLanguageChanged()
	{
		FullRefresh();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (DateTime.UtcNow > DewSave.profile.nextRerollReplenishTimestamp.ToDateTime())
		{
			DewSave.profile.remainingRerolls = 3;
			DewSave.profile.nextRerollReplenishTimestamp = new DateTimeOffset(DateTime.Today.AddDays(1.0).AddHours(6.0), TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)).ToUnixTimeSeconds();
		}
	}

	private void FullRefresh()
	{
		DewReverie.CheckSpecialReveries();
		specialReverieObject.SetActive(value: false);
		specialReverieObject.SetActive(!DewSave.profile.specialReverie.IsEmpty());
		UI_Reveries_DailySlot[] array = slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Refresh();
		}
	}
}
