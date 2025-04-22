public class Ai_Q_EtherealInfluence_Projectile_Base : StandardProjectile
{
	public ScalingValue damage;

	public float procCoefficient = 1f;

	protected Ai_Q_EtherealInfluence _parent;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_parent = FindFirstAncestorOfType<Ai_Q_EtherealInfluence>();
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (_parent != null && _parent.isActive)
		{
			_parent._projectilePos = base.position;
		}
	}

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		if (hit.entity == base.info.caster)
		{
			Destroy();
		}
		else
		{
			Damage(damage, procCoefficient).SetElemental(ElementalType.Light).SetDirection(base.rotation).Dispatch(hit.entity);
		}
	}

	private void MirrorProcessed()
	{
	}
}
