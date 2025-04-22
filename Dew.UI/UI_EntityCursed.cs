using System;
using UnityEngine;

public class UI_EntityCursed : MonoBehaviour
{
	private CanvasGroup _cg;

	private UI_EntityProvider _provider;

	protected virtual void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
	}

	private void Start()
	{
		if (_provider.target != null)
		{
			_provider.target.ClientEntityEvent_OnStatusEffectAdded += new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectAdded);
			_provider.target.ClientEntityEvent_OnStatusEffectRemoved += new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectRemoved);
		}
		UI_EntityProvider provider = _provider;
		provider.onTargetChanged = (Action<Entity, Entity>)Delegate.Combine(provider.onTargetChanged, new Action<Entity, Entity>(OnTargetChanged));
		UpdateVisibility();
	}

	private void OnDestroy()
	{
		if (_provider != null && _provider.target != null)
		{
			_provider.target.ClientEntityEvent_OnStatusEffectAdded -= new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectAdded);
			_provider.target.ClientEntityEvent_OnStatusEffectRemoved -= new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectRemoved);
		}
	}

	private void OnTargetChanged(Entity oldVal, Entity newVal)
	{
		if (oldVal != null)
		{
			oldVal.ClientEntityEvent_OnStatusEffectAdded -= new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectAdded);
			oldVal.ClientEntityEvent_OnStatusEffectRemoved -= new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectRemoved);
		}
		if (newVal != null)
		{
			newVal.ClientEntityEvent_OnStatusEffectAdded += new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectAdded);
			newVal.ClientEntityEvent_OnStatusEffectRemoved += new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectRemoved);
		}
		UpdateVisibility();
	}

	private void ClientEntityEventOnStatusEffectAdded(EventInfoStatusEffect obj)
	{
		UpdateVisibility();
	}

	private void ClientEntityEventOnStatusEffectRemoved(EventInfoStatusEffect obj)
	{
		UpdateVisibility();
	}

	private void UpdateVisibility()
	{
		_cg.alpha = ((_provider.target != null && _provider.target.Status.HasStatusEffect<CurseStatusEffect>()) ? 1 : 0);
	}
}
