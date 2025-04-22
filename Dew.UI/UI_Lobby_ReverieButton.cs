using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Lobby_ReverieButton : LogicBehaviour
{
	public GameObject blockerObject;

	public GameObject isCompleteObject;

	public CanvasGroup reverieWindowGroup;

	public GameObject fxShow;

	public GameObject fxHide;

	public TextMeshProUGUI text;

	private void Start()
	{
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 1000, delegate
		{
			if (blockerObject.activeSelf)
			{
				Hide();
				return true;
			}
			return false;
		});
		reverieWindowGroup.SetActivationState(value: false);
		blockerObject.SetActive(value: false);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		int ongoingCount = 0;
		bool isComplete = false;
		foreach (DewProfile.DailyReverieData r in DewSave.profile.reverieSlots)
		{
			if (!r.IsEmpty())
			{
				ongoingCount++;
				if (r.isComplete)
				{
					isComplete = true;
				}
			}
		}
		if (!DewSave.profile.specialReverie.IsEmpty())
		{
			ongoingCount++;
			if (DewSave.profile.specialReverie.isComplete)
			{
				isComplete = true;
			}
		}
		text.text = string.Format("{0} ({1})", DewLocalization.GetUIValue("Reverie_Reverie"), ongoingCount);
		isCompleteObject.SetActive(isComplete);
	}

	public void Show()
	{
		reverieWindowGroup.SetActivationState(value: true);
		reverieWindowGroup.transform.localScale = Vector3.one * 0.9f;
		reverieWindowGroup.transform.DOKill(complete: true);
		reverieWindowGroup.transform.DOScale(Vector3.one, 0.2f);
		DewEffect.PlayNew(fxShow);
		blockerObject.SetActive(value: true);
	}

	public void Hide()
	{
		reverieWindowGroup.SetActivationState(value: false);
		blockerObject.SetActive(value: false);
		reverieWindowGroup.transform.DOKill(complete: true);
		DewEffect.PlayNew(fxHide);
	}
}
