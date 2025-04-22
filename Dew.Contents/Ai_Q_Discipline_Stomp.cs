public class Ai_Q_Discipline_Stomp : InstantDamageInstance
{
	protected override void OnBeforeDispatchDamage(ref DamageData dmg, Entity target)
	{
		base.OnBeforeDispatchDamage(ref dmg, target);
		if (strengthMultiplier > 1.01f)
		{
			dmg.SetAttr(DamageAttribute.IsCrit);
		}
	}

	private void MirrorProcessed()
	{
	}
}
