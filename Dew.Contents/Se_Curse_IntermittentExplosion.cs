using System.Linq;
using UnityEngine;

public class Se_Curse_IntermittentExplosion : CurseStatusEffect
{
	public float[] interval;

	private float _nextActivateTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_nextActivateTime = Time.time + GetValue(interval);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (ShouldBeSuppressed(base.victim))
		{
			_nextActivateTime = Time.time + GetValue(interval) * 0.35f;
		}
		else if (!(Time.time < _nextActivateTime))
		{
			_nextActivateTime = Time.time + GetValue(interval);
			CreateAbilityInstance<Ai_Curse_IntermittentExplosion_Explosion>(base.victim.agentPosition, null, new CastInfo(base.victim));
		}
	}

	public static bool ShouldBeDestroyed()
	{
		if (!(NetworkedManagerBase<ZoneManager>.instance != null) || !NetworkedManagerBase<ZoneManager>.instance.isInAnyTransition)
		{
			if (ManagerBase<CameraManager>.instance != null)
			{
				return ManagerBase<CameraManager>.instance.isPlayingCutscene;
			}
			return false;
		}
		return true;
	}

	public static bool ShouldBeSuppressed(Entity victim)
	{
		if (!ShouldBeDestroyed())
		{
			if (victim is Hero hero)
			{
				return !hero.isInCombat;
			}
			return false;
		}
		return true;
	}

	public override bool IsViable(Entity target)
	{
		return DewPlayer.humanPlayers.Count((DewPlayer h) => !h.hero.IsNullInactiveDeadOrKnockedOut()) > 1;
	}

	private void MirrorProcessed()
	{
	}
}
