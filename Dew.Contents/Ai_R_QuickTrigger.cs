using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_R_QuickTrigger : AbilityInstance
{
	public int fireCount = 3;

	public float fireInterval = 0.15f;

	public float[] deviateAngles = new float[3] { 5f, 0f, -5f };

	public float postDelay = 0.1f;

	private List<int> _angleIndices = new List<int>();

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		for (int i = 0; i < deviateAngles.Length; i++)
		{
			_angleIndices.Add(i);
		}
		base.info.caster.Control.StartChannel(new Channel
		{
			blockedActions = (Channel.BlockedAction.Ability | Channel.BlockedAction.Attack),
			duration = (float)fireCount * fireInterval / base.info.caster.Status.attackSpeedMultiplier + postDelay,
			onCancel = base.Destroy
		});
		for (int j = 0; j < fireCount; j++)
		{
			if (_angleIndices.Count == 0)
			{
				for (int k = 0; k < deviateAngles.Length; k++)
				{
					_angleIndices.Add(k);
				}
			}
			int index = Random.Range(0, _angleIndices.Count);
			CreateAbilityInstance<Ai_R_QuickTrigger_Projectile>(base.position, base.info.rotation, new CastInfo(base.info.caster, base.info.angle + deviateAngles[_angleIndices[index]]));
			_angleIndices.RemoveAt(index);
			yield return new SI.WaitForSeconds(fireInterval / base.info.caster.Status.attackSpeedMultiplier);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
