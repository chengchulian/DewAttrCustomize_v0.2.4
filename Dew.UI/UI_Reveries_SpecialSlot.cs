using System;
using TMPro;

public class UI_Reveries_SpecialSlot : UI_Reveries_SlotBase
{
	public TextMeshProUGUI remainingTimeText;

	public TextMeshProUGUI emptySlotText;

	public override DewProfile.ReverieDataBase data => DewSave.profile.specialReverie;

	public DewProfile.SpecialReverieData sData => (DewProfile.SpecialReverieData)data;

	protected override void OnEnable()
	{
		base.OnEnable();
		Refresh();
	}

	protected override void OnSuccessfullyClaimedReward()
	{
		base.OnSuccessfullyClaimedReward();
		DewReverie.StartNextSpecialReverieOrClear();
		emptySlotText.text = DewLocalization.GetUIValue("Reverie_Special_AllSpecialReveriesCompleted");
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!sData.IsEmpty())
		{
			DateTime dueDate = sData.timeLimitTimestamp.ToDateTime();
			if (DateTime.UtcNow > dueDate)
			{
				DewReverie.ClearSpecialReverie();
				DewEffect.Play(fxFlash);
				DewEffect.Play(fxRemove);
				emptySlotText.text = DewLocalization.GetUIValue("Reverie_Special_NoSpecialReverie");
				Refresh();
			}
			else
			{
				TimeSpan remaining = dueDate - DateTime.UtcNow;
				remainingTimeText.text = Dew.GetReadableTimespan(remaining);
			}
		}
	}
}
