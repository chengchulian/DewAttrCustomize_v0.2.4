using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_TurretMode_Projectile : StandardProjectile
{
	public float startHeight;

	public float startFrontDis;

	public float startPosDeviation;

	public float secondAtkDelay;

	public DewCollider range;

	public ScalingValue damage;

	public Knockback Knockback;

	public GameObject fxExplode;

	public GameObject fxHit;

	public GameObject fxTelegrpah;

	public GameObject fxGround;

	private Vector3 _startPos;

	protected override void OnPrepare()
	{
		base.OnPrepare();
		Vector2 insideUnitCircle = global::UnityEngine.Random.insideUnitCircle;
		Vector3 vector = new Vector3(0f, insideUnitCircle.x, insideUnitCircle.y);
		_startPos = base.info.caster.position + Vector3.up * startHeight + base.info.forward * startFrontDis + vector * startPosDeviation;
		SetCustomStartPosition(_startPos);
	}

	protected override void OnComplete()
	{
		base.OnComplete();
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			Vector3 normalized = (base.info.point - _startPos).normalized;
			FxPlayNetworked(fxTelegrpah, base.info.point, null);
			FxPlayNetworked(fxGround, base.info.point, Quaternion.LookRotation(normalized));
			yield return new WaitForSeconds(secondAtkDelay);
			FxPlayNetworked(fxExplode, base.info.point, null);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				Damage(damage).SetOriginPosition(base.position).Dispatch(entity);
				FxPlayNewNetworked(fxHit, entity);
			}
			handle.Return();
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
