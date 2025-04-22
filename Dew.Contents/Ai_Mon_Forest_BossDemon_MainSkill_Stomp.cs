using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Forest_BossDemon_MainSkill_Stomp : AbilityInstance
{
	public int waves;

	public float treeInterval;

	public int perWaveTreeCount;

	public float waveInterval;

	public float knockupAmount;

	public GameObject hitEffect;

	public DewCollider range;

	public ScalingValue damage;

	public Knockback knockback;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			DefaultDamage(damage).SetOriginPosition(base.transform.position).Dispatch(entity);
			knockback.ApplyWithOrigin(base.transform.position, entity);
			entity.Visual.KnockUp(knockupAmount, isFriendly: false);
			FxPlayNewNetworked(hitEffect, entity);
		}
		handle.Return();
		float treeDelay = DewResources.GetByType<Ai_Mon_Forest_BossDemon_MainSkill_Stomp_Tree>().damageDelay;
		RoomSection section = base.info.caster.section;
		if (section == null)
		{
			section = SingletonDewNetworkBehaviour<Room>.instance.GetFinalSection();
		}
		if (section == null)
		{
			Destroy();
			yield break;
		}
		for (int j = 0; j < waves; j++)
		{
			for (int k = 0; k < perWaveTreeCount; k++)
			{
				Vector3 vector = ((k < DewPlayer.humanPlayers.Count && !DewPlayer.humanPlayers[k].hero.isKnockedOut) ? DewPlayer.humanPlayers[k].hero.position : ((k < perWaveTreeCount - DewPlayer.humanPlayers.Count || DewPlayer.humanPlayers[k - perWaveTreeCount + DewPlayer.humanPlayers.Count].hero.isKnockedOut) ? (section.GetAnyRandomNode() + global::UnityEngine.Random.insideUnitSphere.Flattened() * 1.5f) : AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.Range(0.7f, 1f), DewPlayer.humanPlayers[k - perWaveTreeCount + DewPlayer.humanPlayers.Count].hero, treeDelay)));
				vector = Dew.GetPositionOnGround(vector);
				CreateAbilityInstance<Ai_Mon_Forest_BossDemon_MainSkill_Stomp_Tree>(vector, null, new CastInfo(base.info.caster, vector));
				yield return new SI.WaitForSeconds(treeInterval);
			}
			yield return new SI.WaitForSeconds(waveInterval);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
