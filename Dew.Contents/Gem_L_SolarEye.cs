using System.Collections;
using UnityEngine;

public class Gem_L_SolarEye : Gem
{
	public DewCollider range;

	public GameObject fxCast;

	public GameObject fxStart;

	public float delay;

	public int tickCount;

	public ScalingValue totalStack;

	public int calTotalStack => Mathf.CeilToInt(GetValue(totalStack));

	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		if (IsReady() && base.isValid)
		{
			FxPlayNewNetworked(fxCast, base.owner);
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			NotifyUse();
			StartCooldown();
			AbilityInstance source = info.instance;
			source.LockDestroy();
			yield return new WaitForSeconds(delay);
			source.UnlockDestroy();
			bool checkValidation = false;
			range.transform.position = base.owner.position;
			ArrayReturnHandle<Entity> handle;
			Entity[] ents = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets).ToArray();
			for (int i = 0; i < ents.Length; i++)
			{
				Entity entity = ents[i];
				if (entity is Monster && entity.Status.HasElemental(ElementalType.Fire))
				{
					CreateAbilityInstanceWithSource(source, base.owner.position, null, new CastInfo(base.owner, entity), delegate(Ai_Gem_L_SolarEye b)
					{
						b.totalStack = calTotalStack;
						b.maxTickCount = tickCount;
					});
					checkValidation = true;
					if (i < 2)
					{
						FxPlayNewNetworked(fxStart, entity);
					}
					yield return new WaitForSeconds(0.03f);
				}
			}
			handle.Return();
			if (!checkValidation)
			{
				ResetCooldown();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
