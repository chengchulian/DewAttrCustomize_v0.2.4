using System;
using TMPro;
using UnityEngine;

public class UI_InGame_ResultView : View
{
	public GameObject[] gameOverObjects;

	public GameObject[] demoFinishObjects;

	public TextMeshProUGUI gameOverSubtitle;

	public TextMeshProUGUI totalScoreText;

	public TextMeshProUGUI readyStatusText;

	public UI_Toggle readyToggle;

	public GameObject multiplePlayersObject;

	private int _playerIndex;

	public DewGameResult current => NetworkedManagerBase<GameResultManager>.instance.current;

	protected override void Start()
	{
		base.Start();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		GameManager.CallOnReady(delegate
		{
			readyToggle.onClick.AddListener(delegate
			{
				if (!(DewPlayer.local == null))
				{
					DewPlayer.local.CmdSetIsReady(!DewPlayer.local.isReady);
				}
			});
			readyToggle.doNotToggleOnClick = true;
			readyToggle.isChecked = DewPlayer.local.isReady;
			DewPlayer.local.ClientEvent_OnIsReadyChanged += new Action<bool>(IsReadyChanged);
			readyStatusText.text = "";
		});
	}

	private new void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnIsReadyChanged -= new Action<bool>(IsReadyChanged);
		}
	}

	private void IsReadyChanged(bool obj)
	{
		readyToggle.isChecked = obj;
	}

	protected override void OnShow()
	{
		base.OnShow();
		if (current != null)
		{
			if (current.result == DewGameResult.ResultType.GameOver)
			{
				ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Result_GameOver");
			}
			gameOverObjects.SetActiveAll(current.result == DewGameResult.ResultType.GameOver);
			demoFinishObjects.SetActiveAll(current.result == DewGameResult.ResultType.DemoFinish);
			if (current.result == DewGameResult.ResultType.GameOver)
			{
				gameOverSubtitle.text = GetGameOverText();
			}
			_playerIndex = current.players.FindIndex((DewGameResult.PlayerData p) => p.playerNetId == DewPlayer.local.netId);
			multiplePlayersObject.SetActive(current.players.Count > 1);
			if (_playerIndex < 0)
			{
				_playerIndex = 0;
			}
			Refresh();
		}
	}

	public void NextPlayer()
	{
		_playerIndex++;
		if (_playerIndex >= current.players.Count)
		{
			_playerIndex = 0;
		}
		Refresh();
	}

	public void PrevPlayer()
	{
		_playerIndex--;
		if (_playerIndex < 0)
		{
			_playerIndex = current.players.Count - 1;
		}
		Refresh();
	}

	private void FixedUpdate()
	{
		if (Application.IsPlaying(this) && base.isShowing)
		{
			RefreshReadyStatus();
		}
	}

	private void Refresh()
	{
		double totalScore = 0.0;
		float scoreMultiplier = DewResources.GetByName<DewDifficultySettings>(NetworkedManagerBase<GameResultManager>.instance.current.difficulty).scoreMultiplier;
		IGameResultStatItem[] componentsInChildren = GetComponentsInChildren<IGameResultStatItem>(includeInactive: true);
		foreach (IGameResultStatItem c in componentsInChildren)
		{
			try
			{
				totalScore += c.UpdateAndGetScore(NetworkedManagerBase<GameResultManager>.instance.current, _playerIndex, scoreMultiplier);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
		totalScoreText.text = string.Format(DewLocalization.GetUIValue("InGame_Result_ScoreFormat"), totalScore.ToString("#,##0"));
	}

	private void RefreshReadyStatus()
	{
		if (DewPlayer.humanPlayers.Count <= 1)
		{
			readyStatusText.text = "";
			return;
		}
		int readyPlayers = 0;
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			if (humanPlayer.isReady)
			{
				readyPlayers++;
			}
		}
		if (readyPlayers == DewPlayer.humanPlayers.Count)
		{
			readyStatusText.text = "";
		}
		else
		{
			readyStatusText.text = string.Format(DewLocalization.GetUIValue("Lobby_ReadyStatus"), readyPlayers, DewPlayer.humanPlayers.Count);
		}
	}

	private string GetGameOverText()
	{
		DewGameResult.PlayerData local = current.players.Find((DewGameResult.PlayerData p) => p.playerNetId == DewPlayer.local.netId);
		if (local == null)
		{
			return "";
		}
		if (local.causeOfDeathActor.Contains("Fire") && global::UnityEngine.Random.value < 0.1f)
		{
			return DewLocalization.GetUIValue("InGame_Result_Subtitle_GameOver_Fire_" + global::UnityEngine.Random.Range(0, 3));
		}
		if (local.causeOfDeathActor.Contains("Ice") && global::UnityEngine.Random.value < 0.1f)
		{
			return DewLocalization.GetUIValue("InGame_Result_Subtitle_GameOver_Ice_" + global::UnityEngine.Random.Range(0, 3));
		}
		if (local.causeOfDeathActor.Contains("Dark") && global::UnityEngine.Random.value < 0.1f)
		{
			return DewLocalization.GetUIValue("InGame_Result_Subtitle_GameOver_Dark_" + global::UnityEngine.Random.Range(0, 3));
		}
		if (local.causeOfDeathActor.Contains("Light") && global::UnityEngine.Random.value < 0.1f)
		{
			return DewLocalization.GetUIValue("InGame_Result_Subtitle_GameOver_Light_" + global::UnityEngine.Random.Range(0, 3));
		}
		if (local.causeOfDeathActor.Contains("Spider") && global::UnityEngine.Random.value < 0.1f)
		{
			return DewLocalization.GetUIValue("InGame_Result_Subtitle_GameOver_Spider_" + global::UnityEngine.Random.Range(0, 3));
		}
		if (global::UnityEngine.Random.value < 0.1f)
		{
			return DewLocalization.GetUIValue($"InGame_Result_Subtitle_GameOver_{local.heroType}_{global::UnityEngine.Random.Range(0, 3)}");
		}
		return DewLocalization.GetUIValue($"InGame_Result_Subtitle_GameOver_{global::UnityEngine.Random.Range(0, 10)}");
	}
}
