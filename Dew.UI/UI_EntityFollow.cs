using System;
using UnityEngine;

public class UI_EntityFollow : LogicBehaviour
{
	public bool shakeOnDamage;

	public AnimationCurve shakeCurve;

	public float shakeMagnitude = 10f;

	public float shakeDecreaseSpeed = 3f;

	public float shakeInterval = 0.05f;

	public float shakeOnDeath;

	private float _shakeValue;

	private Vector2 _shakeOffset;

	private float _lastShakeTime;

	private float _elapsedAfterKill;

	private UI_EntityProvider _provider;

	private Vector3 _lastHealthbarPosition;

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
	}

	private void Start()
	{
		if (!(target == null))
		{
			if (shakeOnDamage)
			{
				target.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(EntityEventOnTakeDamage);
			}
			if (shakeOnDeath > 0f)
			{
				target.EntityEvent_OnDeath += new Action<EventInfoKill>(EntityEventOnDeath);
			}
		}
	}

	private void OnDestroy()
	{
		if (shakeOnDamage && target != null)
		{
			target.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(EntityEventOnTakeDamage);
		}
		if (shakeOnDeath > 0f && target != null)
		{
			target.EntityEvent_OnDeath -= new Action<EventInfoKill>(EntityEventOnDeath);
		}
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
		float normalized = obj.damage.amount / (target.maxHealth + target.Status.currentShield);
		_shakeValue += shakeCurve.Evaluate(normalized);
	}

	private void EntityEventOnDeath(EventInfoKill obj)
	{
		_shakeValue = Mathf.Max(_shakeValue, shakeOnDeath);
	}

	private void LateUpdate()
	{
		RefreshPosition();
	}

	private void RefreshPosition()
	{
		if (target != null && target.Status.isAlive)
		{
			Vector3 realPos = target.position;
			_lastHealthbarPosition = ((target.Visual.healthBarPosition != null) ? target.Visual.healthBarPosition.position : target.Visual.GetAbovePosition());
			_lastHealthbarPosition.x = realPos.x;
			_lastHealthbarPosition.z = realPos.z;
		}
		Vector3 pos = Dew.mainCamera.WorldToScreenPoint(_lastHealthbarPosition) + (Vector3)_shakeOffset;
		pos.Quantitize();
		base.transform.position = pos;
		if (Time.time - _lastShakeTime > shakeInterval)
		{
			_lastShakeTime = Time.time;
			_shakeOffset = global::UnityEngine.Random.insideUnitCircle * shakeMagnitude * _shakeValue;
		}
		_shakeValue = Mathf.MoveTowards(_shakeValue, 0f, shakeDecreaseSpeed * Time.deltaTime);
	}
}
