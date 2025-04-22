using System.Collections;
using UnityEngine;

public class Mon_Special_AmbushSpawner : Monster, IForceHeroicHealthbar
{
	public float range;

	public float rotSpeed;

	private EntityTransformModifier _entTransform;

	private float _angle;

	protected override void OnCreate()
	{
		base.OnCreate();
		_entTransform = base.Visual.GetNewTransformModifier();
		FxPlay(base.gameObject, this);
		if (base.isServer)
		{
			CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity);
		}
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (base.AI.Helper_CanBeCast<At_Mon_Special_AmbushSpawner_Spawn>() && !(NetworkedManagerBase<GameManager>.instance.spawnedPopulation > NetworkedManagerBase<GameManager>.instance.maxSpawnedPopulation * 1.5f))
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Special_AmbushSpawner_Spawn>();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_entTransform != null)
		{
			_entTransform.Stop();
			_entTransform = null;
		}
	}

	protected override void OnDeath(EventInfoKill info)
	{
		base.OnDeath(info);
		if (base.isServer)
		{
			NetworkedManagerBase<FlavourManager>.instance.FxPlayNewNetworked(NetworkedManagerBase<FlavourManager>.instance.hitStopDealDamage, this);
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			Vector3 pos = base.agentPosition;
			yield return new WaitForSeconds(1.5f);
			SingletonDewNetworkBehaviour<Room>.instance.rewards.DropChaosReward(pos, isHighQuality: false);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (_entTransform != null)
		{
			_angle += rotSpeed * dt;
			_entTransform.rotation = Quaternion.Euler(0f, _angle, 0f);
		}
	}

	private void MirrorProcessed()
	{
	}
}
