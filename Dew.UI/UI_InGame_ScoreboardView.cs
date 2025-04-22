using System;
using System.Collections;
using UnityEngine;

public class UI_InGame_ScoreboardView : View
{
	public UI_InGame_Scoreboard_PlayerItem itemPrefab;

	public Transform mineParent;

	public Transform othersParent;

	public GameObject separator;

	public GameObject gamepadBlocker;

	private bool _disablingInGamePlayingInput;

	protected override void Start()
	{
		base.Start();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		NetworkedManagerBase<ActorManager>.instance.onHeroAdd += new Action<Hero>(HandleNewHero);
		NetworkedManagerBase<ActorManager>.instance.onHeroRemove += new Action<Hero>(OnHeroRemove);
		InGameUIManager instance = InGameUIManager.instance;
		instance.onScoreboardDisplayedChanged = (Action<bool>)Delegate.Combine(instance.onScoreboardDisplayedChanged, new Action<bool>(OnScoreboardDisplayedChanged));
		foreach (Hero h in NetworkedManagerBase<ActorManager>.instance.allHeroes)
		{
			HandleNewHeroNoDelay(h);
		}
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	protected override void OnDestroy()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		gamepadBlocker.SetActive(arg2 == InputMode.Gamepad);
	}

	protected override void OnShow()
	{
		base.OnShow();
		if (!_disablingInGamePlayingInput && InGameUIManager.instance != null && DewInput.currentMode == InputMode.Gamepad)
		{
			_disablingInGamePlayingInput = true;
			InGameUIManager.instance.DisablePlayingInput();
		}
		OnCurrentModeChanged(InputMode.KeyboardAndMouse, DewInput.currentMode);
	}

	protected override void OnHide()
	{
		base.OnHide();
		if (_disablingInGamePlayingInput && InGameUIManager.instance != null)
		{
			_disablingInGamePlayingInput = false;
			InGameUIManager.instance.EnablePlayingInput();
		}
	}

	private void OnScoreboardDisplayedChanged(bool obj)
	{
		if (obj)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void OnHeroRemove(Hero obj)
	{
		NetworkedManagerBase<GameManager>.instance.StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSecondsRealtime(0.1f);
			UpdateLayout();
		}
	}

	private void HandleNewHero(Hero h)
	{
		NetworkedManagerBase<GameManager>.instance.StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSecondsRealtime(0.1f);
			HandleNewHeroNoDelay(h);
		}
	}

	private void HandleNewHeroNoDelay(Hero h)
	{
		if (!h.IsNullOrInactive())
		{
			UI_InGame_Scoreboard_PlayerItem uI_InGame_Scoreboard_PlayerItem = global::UnityEngine.Object.Instantiate(itemPrefab, h.isOwned ? mineParent : othersParent);
			uI_InGame_Scoreboard_PlayerItem.GetComponent<UI_EntityProvider>().target = h;
			uI_InGame_Scoreboard_PlayerItem.Setup(h);
			UpdateLayout();
		}
	}

	private void UpdateLayout()
	{
		int cc = othersParent.childCount;
		othersParent.gameObject.SetActive(cc > 0);
		separator.gameObject.SetActive(cc > 0);
	}
}
