using System;
using UnityEngine;

public class UI_EntityDestroyOnDeath : LogicBehaviour
{
	public float delay;

	public bool destroyOnSleep = true;

	private float _elapsedAfterKill;

	private UI_EntityProvider _provider;

	private CanvasGroup _cg;

	public Entity target
	{
		get
		{
			if (!(_provider != null))
			{
				return null;
			}
			return _provider.target;
		}
	}

	protected virtual void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		_cg = GetComponent<CanvasGroup>();
		NetworkedManagerBase<ClientEventManager>.instance.OnRefreshEntityHealthbar += new Action<Entity>(OnRefreshEntityHealthbar);
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ClientEventManager>.instance != null)
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnRefreshEntityHealthbar -= new Action<Entity>(OnRefreshEntityHealthbar);
		}
	}

	private void OnRefreshEntityHealthbar(Entity obj)
	{
		if (_provider.target == obj)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (destroyOnSleep && target != null && target.isSleeping)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (target == null || target.Status.isDead)
		{
			if (_cg != null)
			{
				_cg.alpha = (delay - _elapsedAfterKill) / delay;
			}
			_elapsedAfterKill += dt;
			if (_elapsedAfterKill > delay)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
