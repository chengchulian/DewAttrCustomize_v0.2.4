using System;
using System.Collections;
using UnityEngine;

public class Gem_R_Dusk : Gem
{
	public float empowerDuration = 4f;

	public int shootCount = 3;

	public float shootInterval = 0.15f;

	public float radius = 2f;

	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		AttackEmpowerEffect ae = new AttackEmpowerEffect();
		ae.onAttackEffect = delegate(EventInfoAttackEffect effect, int i)
		{
			if (base.isValid)
			{
				effect.actor.StartCoroutine(Routine());
			}
			IEnumerator Routine()
			{
				for (int j = 0; j < shootCount; j++)
				{
					if (!base.isValid)
					{
						break;
					}
					ArrayReturnHandle<Entity> handle;
					ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, effect.victim.position, radius, tvDefaultHarmfulEffectTargets);
					if (readOnlySpan.Length > 0)
					{
						CreateAbilityInstanceWithSource(ae.parent, base.owner.position, Quaternion.identity, new CastInfo(base.owner, readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)]), delegate(Ai_Gem_R_Dusk_Projectile ai)
						{
							ai.chain = effect.chain.New(this);
							ai._strength = effect.strength;
						});
					}
					handle.Return();
					NotifyUse();
					yield return new WaitForSeconds(shootInterval);
				}
			}
		};
		CreateBasicEffectWithSource(info.instance, base.owner, ae, empowerDuration, "dusk_empower");
	}

	private void MirrorProcessed()
	{
	}
}
