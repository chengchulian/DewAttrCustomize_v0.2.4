using System.Collections;
using UnityEngine;

public class Gem_R_Glaciate : Gem
{
	public float delay;

	public float waitForDashMaxTime;

	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			info.instance.LockDestroy();
			yield return new WaitForSeconds(delay);
			if (!base.isValid)
			{
				info.instance.UnlockDestroy();
			}
			else
			{
				float startTime = Time.time;
				while (base.owner != null && base.owner.Control.isDashing && Time.time - startTime < waitForDashMaxTime)
				{
					yield return null;
				}
				info.instance.UnlockDestroy();
				if (base.isValid)
				{
					CreateAbilityInstanceWithSource<Ai_Gem_R_Glaciate>(info.instance, base.owner.position, Quaternion.identity, new CastInfo(base.owner));
					NotifyUse();
				}
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
