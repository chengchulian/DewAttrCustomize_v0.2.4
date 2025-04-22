using System;
using UnityEngine;

public class UI_InGame_VisibilityOnWorldView : MonoBehaviour
{
	public bool hideOnWorldView;

	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		InGameUIManager instance = InGameUIManager.instance;
		instance.onWorldDisplayedChanged = (Action<WorldDisplayStatus>)Delegate.Combine(instance.onWorldDisplayedChanged, new Action<WorldDisplayStatus>(OnWorldDisplayedChanged));
		OnWorldDisplayedChanged(InGameUIManager.instance.isWorldDisplayed);
	}

	private void OnWorldDisplayedChanged(WorldDisplayStatus obj)
	{
		_cg.alpha = ((obj != WorldDisplayStatus.None == !hideOnWorldView) ? 1 : 0);
	}
}
