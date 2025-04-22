using System;
using UnityEngine;

[LogicUpdatePriority(400)]
public abstract class UI_InGame_FloatingWindow_Base : LogicBehaviour
{
	public GameObject fxActivate;

	public GameObject fxDeactivate;

	private UI_InGame_FollowAndFaceHero_Transform _ff;

	public MonoBehaviour target { get; internal set; }

	public abstract Type GetSupportedType();

	protected virtual void Awake()
	{
		GetComponent(out _ff);
	}

	protected virtual void Start()
	{
		FloatingWindowManager instance = ManagerBase<FloatingWindowManager>.instance;
		instance.onTargetChanged = (Action<MonoBehaviour>)Delegate.Combine(instance.onTargetChanged, new Action<MonoBehaviour>(OnTargetChanged));
		if (!IsSupported(target))
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnTargetChanged(MonoBehaviour obj)
	{
		if (IsSupported(obj))
		{
			if (base.gameObject.activeSelf)
			{
				OnDeactivate();
			}
			target = obj;
			OnActivate();
		}
		else if (base.gameObject.activeSelf)
		{
			OnDeactivate();
			target = null;
		}
	}

	public virtual void OnDeactivate()
	{
		DewEffect.PlayNew(fxDeactivate);
		base.gameObject.SetActive(value: false);
	}

	public virtual void OnActivate()
	{
		base.gameObject.SetActive(value: true);
		DewEffect.PlayNew(fxActivate);
		if (_ff != null)
		{
			_ff.target = target.transform;
		}
		Dew.CallDelayed(delegate
		{
			ManagerBase<GlobalUIManager>.instance.SetFocusOnFirstFocusable(base.gameObject);
		});
	}

	public virtual bool IsSupported(MonoBehaviour t)
	{
		return GetSupportedType().IsInstanceOfType(t);
	}
}
