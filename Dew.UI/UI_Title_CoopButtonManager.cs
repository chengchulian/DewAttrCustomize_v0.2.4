using UnityEngine;

public class UI_Title_CoopButtonManager : LogicBehaviour
{
	public GameObject textObject;

	public GameObject unavailableObject;

	public GameObject loadingObject;

	public UI_Title_TitleView titleView;

	private float _startUnscaledTime;

	private void Awake()
	{
		_startUnscaledTime = Time.unscaledTime;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		UpdateStatus();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		bool isUsingSteam = DewBuildProfile.current.platform == PlatformType.STEAM && DewBuildProfile.current.useSteamLobbyAndRelay;
		if ((isUsingSteam && SteamManagerBase.Initialized) || (!isUsingSteam && ManagerBase<EOSManager>.instance.status == ServiceStatus.Ready))
		{
			textObject.SetActive(value: true);
			unavailableObject.SetActive(value: false);
			loadingObject.SetActive(value: false);
			return;
		}
		if (((isUsingSteam && !SteamManagerBase.Initialized) || (!isUsingSteam && ManagerBase<EOSManager>.instance.status == ServiceStatus.Loading)) && Time.unscaledTime - _startUnscaledTime < 8f)
		{
			textObject.SetActive(value: false);
			unavailableObject.SetActive(value: false);
			loadingObject.SetActive(value: true);
			return;
		}
		textObject.SetActive(value: true);
		unavailableObject.SetActive(value: true);
		loadingObject.SetActive(value: false);
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void Click()
	{
		if (!loadingObject.activeSelf)
		{
			if (unavailableObject.activeSelf)
			{
				ManagerBase<MessageManager>.instance.ShowMessageLocalized("Title_Message_OnlineServicesNotAvailable");
			}
			else
			{
				titleView.EnterCoopOptions();
			}
		}
	}
}
