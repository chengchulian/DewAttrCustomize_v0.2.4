using System;
using UnityEngine;

public class Se_FatalHitProtection_Invulnerable : StatusEffect
{
	public DewAnimationClip invulAnim;

	public float dazeDuration;

	public bool doKnockbackAndStun;

	public DewCollider range;

	public float stunDuration;

	public Knockback knockback;

	public GameObject hitEffect;

	internal float duration;

	internal string timerCustomNameKey;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		SetTimer(duration);
		ShowOnScreenTimer(timerCustomNameKey);
		DoInvulnerable();
		base.victim.Control.CancelOngoingDisplacement();
		base.victim.Control.CancelOngoingChannels();
		base.victim.Animation.PlayAbilityAnimation(invulAnim);
		base.victim.Control.StartDaze(dazeDuration);
		if (doKnockbackAndStun)
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				CreateBasicEffect(entity, new StunEffect(), stunDuration, "fatalhit_stun");
				knockback.ApplyWithOrigin(base.victim.position, entity);
				FxPlayNewNetworked(hitEffect, entity);
			}
			handle.Return();
		}
	}

	private void MirrorProcessed()
	{
	}
}
