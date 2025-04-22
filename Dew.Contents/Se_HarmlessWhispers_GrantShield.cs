using System.Collections;
using UnityEngine;

public class Se_HarmlessWhispers_GrantShield : StatusEffect
{
	public float delay;

	public GameObject fxAfterDelay;

	public Transform moveDownwardsTransform;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			yield return new SI.WaitForSeconds(delay);
			FxPlayNetworked(fxAfterDelay, base.victim);
			float ratio = 0.14f + NetworkedManagerBase<GameManager>.instance.GetMultiplayerDifficultyFactor(reduceWhenDead: true) * 0.02f;
			base.victim.Status.SetHealth(Mathf.Clamp(base.victim.Status.currentHealth - base.victim.Status.maxHealth * ratio, base.victim.Status.maxHealth * 0.05f, base.victim.Status.maxHealth));
			CreateStatusEffect(base.victim, delegate(Se_MirageSkin_Armor m)
			{
				m.customAmount = base.victim.Status.maxHealth * ratio;
			});
			Destroy();
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		float num = DewEase.EaseInQuart.Get((Time.time - base.creationTime) / delay);
		moveDownwardsTransform.localPosition = ((1f - num) * 5f - 0.5f) * Vector3.up;
	}

	private void MirrorProcessed()
	{
	}
}
