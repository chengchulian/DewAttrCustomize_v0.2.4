using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TutorialDifficultyView : View
{
	public string[] namesByIndex;

	private UI_ToggleGroup _group;

	private bool _isInTransition;

	protected override void Awake()
	{
		base.Awake();
		if (Application.IsPlaying(this))
		{
			_group = GetComponentInChildren<UI_ToggleGroup>();
		}
	}

	protected override void Start()
	{
		base.Start();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		_group.onCurrentIndexChanged.AddListener(delegate(int index)
		{
			if (base.isShowing)
			{
				DewSave.profile.preferredDifficulty = namesByIndex[index];
			}
		});
	}

	protected override void OnShow()
	{
		base.OnShow();
		_group.currentIndex = 0;
	}

	public void StartGame()
	{
		PlayLobbyManager.isFirstTimePlayFlow = true;
		if (ManagerBase<TitleManager>.instance != null)
		{
			if (!ManagerBase<TitleManager>.instance.isInTransition)
			{
				DewNetworkManager.networkMode = DewNetworkManager.Mode.Singleplayer;
				DewSave.profile.preferredDifficulty = namesByIndex[_group.currentIndex];
				ManagerBase<TitleManager>.instance.TransitionToScene("PlayLobby");
			}
		}
		else if (!_isInTransition)
		{
			_isInTransition = true;
			StartFromPlayTutorial();
		}
	}

	private async UniTaskVoid StartFromPlayTutorial()
	{
		DewNetworkManager.networkMode = DewNetworkManager.Mode.Singleplayer;
		DewSave.profile.preferredDifficulty = namesByIndex[_group.currentIndex];
		ManagerBase<TransitionManager>.instance.FadeOut(showTips: false);
		if (ManagerBase<AchievementManager>.instance.isTrackingAchievements)
		{
			ManagerBase<AchievementManager>.instance.StopTrackingAchievements();
		}
		await UniTask.WaitForSeconds(1f, ignoreTimeScale: true);
		NetworkServer.Shutdown();
		NetworkClient.Shutdown();
		if (ManagerBase<GameLogicPackage>.instance != null)
		{
			Object.Destroy(ManagerBase<GameLogicPackage>.instance.gameObject);
		}
		Object.Destroy(ManagerBase<NetworkLogicPackage>.instance.gameObject);
		await UniTask.WaitForSeconds(0.25f, ignoreTimeScale: true);
		SceneManager.LoadScene("PlayLobby");
	}
}
