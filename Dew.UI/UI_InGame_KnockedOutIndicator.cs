using System;
using UnityEngine;

public class UI_InGame_KnockedOutIndicator : LogicBehaviour
{
	public GameObject disableOnBossBar;

	private CanvasGroup _cg;

	private Canvas _canvas;

	private UI_InGame_BossBar _bossBar;

	private void Awake()
	{
		_canvas = GetComponent<Canvas>();
		_cg = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		_bossBar = global::UnityEngine.Object.FindObjectOfType<UI_InGame_BossBar>();
		GameManager.CallOnReady(delegate
		{
			DewPlayer.local.hero.ClientHeroEvent_OnKnockedOut += new Action<EventInfoKill>(OnKnockedOut);
			DewPlayer.local.hero.ClientHeroEvent_OnRevive += new Action<Hero>(OnRevive);
		});
		_canvas.enabled = false;
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		disableOnBossBar.SetActive(!_bossBar.isShown);
	}

	private void OnKnockedOut(EventInfoKill obj)
	{
		_canvas.enabled = true;
	}

	private void OnRevive(Hero obj)
	{
		_canvas.enabled = false;
	}
}
