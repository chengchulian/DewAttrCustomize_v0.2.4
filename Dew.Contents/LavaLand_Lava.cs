using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LavaLand_Lava : Actor, IBanRoomNodesOnTop, INotPlayableOnTop
{
	public class Ad_Lava
	{
		public bool isImmune;

		public int currentStack;

		public float lastDamageTime;
	}

	public static LavaLand_Lava instance;

	public GameObject hitEffect;

	public GameObject hitStartEffect;

	public float tickInterval;

	public float fireChance;

	public float damageMaxHpRatio = 0.03f;

	public float damageMultiplierPerTick;

	public bool enableTranslation;

	public float translationInterval;

	public Vector2 translationRange;

	private float _lastTickTime;

	private List<Entity> _ents = new List<Entity>();

	public override void OnStartServer()
	{
		base.OnStartServer();
		instance = this;
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (enableTranslation)
		{
			Vector3 localPosition = base.transform.localPosition;
			float t = Mathf.Sin((float)NetworkTime.time * MathF.PI / translationInterval * 2f) * 0.5f + 0.5f;
			localPosition.y = Mathf.Lerp(translationRange.x, translationRange.y, t);
			base.transform.localPosition = localPosition;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || NetworkedManagerBase<ZoneManager>.instance.isInRoomTransition || Time.time - _lastTickTime < tickInterval)
		{
			return;
		}
		_lastTickTime = Time.time;
		_ents.Clear();
		_ents.AddRange(NetworkedManagerBase<ActorManager>.instance.allEntities);
		foreach (Entity ent in _ents)
		{
			if (ent.Visual.isSpawning || ent.Control.isDisplacing || ent.IsNullInactiveDeadOrKnockedOut() || ent.Status.hasUncollidable || ent.Status.hasInvulnerable)
			{
				continue;
			}
			Ad_Lava ad_Lava = ent.GetData<Ad_Lava>();
			if ((ad_Lava == null || !ad_Lava.isImmune) && IsEntityOnLava(ent))
			{
				if (ad_Lava == null)
				{
					ad_Lava = new Ad_Lava();
					ent.AddData(ad_Lava);
				}
				if (Time.time - ad_Lava.lastDamageTime > tickInterval * 2f)
				{
					ad_Lava.currentStack = 0;
					FxPlayNewNetworked(hitStartEffect, ent);
				}
				else
				{
					ad_Lava.currentStack++;
				}
				ad_Lava.lastDamageTime = Time.time;
				DamageData damageData = DefaultDamage(ent.maxHealth * damageMaxHpRatio * (1f + (damageMultiplierPerTick - 1f) * (float)ad_Lava.currentStack));
				if (global::UnityEngine.Random.value < fireChance)
				{
					damageData.SetElemental(ElementalType.Fire);
				}
				damageData.Dispatch(ent);
				FxPlayNewNetworked(hitEffect, ent);
			}
		}
	}

	public bool IsEntityOnLava(Entity e)
	{
		if (!Physics.Raycast(e.position + Vector3.up * 5f, Vector3.down, out var hitInfo, 10f, LayerMasks.Ground))
		{
			return false;
		}
		if (hitInfo.transform != base.transform)
		{
			return false;
		}
		return true;
	}

	public bool IsPositionOnLava(Vector3 pos)
	{
		if (!Physics.Raycast(pos + Vector3.up * 20f, Vector3.down, out var hitInfo, 40f, LayerMasks.Ground))
		{
			return false;
		}
		if (hitInfo.transform != base.transform)
		{
			return false;
		}
		return true;
	}

	private void MirrorProcessed()
	{
	}
}
