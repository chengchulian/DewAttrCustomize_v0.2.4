using UnityEngine;

public class Se_Gem_E_Fever_LivingBomb : StatusEffect
{
	public Transform[] scaledTransforms;

	public float duration = 1.5f;

	public float radiusAmpPerStack = 0.3f;

	public float dmgAmpPerStack = 0.1f;

	public ParticleSystem[] colorAdjustedParticles;

	public Gradient gradient;

	protected override void OnCreate()
	{
		base.OnCreate();
		UpdateColor(0f);
		if (base.isServer)
		{
			SetTimer(duration);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		float explosionRadius = GetExplosionRadius();
		Transform[] array = scaledTransforms;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
		}
		if (base.normalizedDuration.HasValue)
		{
			UpdateColor(1f - base.normalizedDuration.Value);
		}
	}

	private void UpdateColor(float normalized)
	{
		ParticleSystem[] array = colorAdjustedParticles;
		for (int i = 0; i < array.Length; i++)
		{
			ParticleSystem.MainModule main = array[i].main;
			main.startColor = gradient.Evaluate(normalized);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !base.gem.IsNullOrInactive() && base.gem.isValid && !(base.victim == null))
		{
			CreateAbilityInstance(Dew.GetPositionOnGround(base.victim.agentPosition), null, base.info, delegate(Ai_Gem_E_Fever_Explosion ai)
			{
				ai.chain = chain;
				ai.NetworkexplosionRadius = GetExplosionRadius();
				ai.NetworkdamageAmp = GetDamageAmp();
			});
		}
	}

	public float GetExplosionRadius()
	{
		return 1.5f * (1f + (float)Mathf.Clamp(base.victim.Status.fireStack, 0, 25) * radiusAmpPerStack);
	}

	public float GetDamageAmp()
	{
		return (float)base.victim.Status.fireStack * dmgAmpPerStack;
	}

	private void MirrorProcessed()
	{
	}
}
