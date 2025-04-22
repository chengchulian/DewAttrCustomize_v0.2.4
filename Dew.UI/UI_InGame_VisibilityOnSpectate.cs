using System;
using UnityEngine;

public class UI_InGame_VisibilityOnSpectate : MonoBehaviour
{
	public bool hideOnSpectate;

	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		CameraManager instance = ManagerBase<CameraManager>.instance;
		instance.onIsSpectatingChanged = (Action<bool>)Delegate.Combine(instance.onIsSpectatingChanged, new Action<bool>(OnIsSpectatingChanged));
		OnIsSpectatingChanged(ManagerBase<CameraManager>.instance.isSpectating);
	}

	private void OnIsSpectatingChanged(bool obj)
	{
		_cg.alpha = ((obj == !hideOnSpectate) ? 1 : 0);
	}
}
