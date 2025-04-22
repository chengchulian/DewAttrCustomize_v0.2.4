using UnityEngine;

public class Ai_Q_CruelSun : AbilityInstance
{
	public ChargingChannelData channel;

	public float fullChargeLengthMult;

	public float fullChargeWidthMult;

	public float fullGraceThreshold;

	private ChargingChannel _channel;

	private float _initLength;

	private float _initWidth;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			if (base.info.caster.Status.TryGetStatusEffect<Se_Q_CruelSun_Armored>(out var effect))
			{
				effect.Destroy();
			}
			channel.chargeFullDuration /= Mathf.Max(1f, base.info.caster.Status.attackSpeedMultiplier);
			_channel = channel.Get(base.netIdentity).SetInitialInfo(base.info).OnCast(delegate
			{
				Complete();
			})
				.OnCancel(delegate
				{
					Complete();
				})
				.OnComplete(delegate
				{
					Complete();
				})
				.Dispatch(base.info.caster, base.firstTrigger);
			_initLength = channel.castMethod._length;
			_initWidth = channel.castMethod._width;
			CreateStatusEffect<Se_Q_CruelSun_Armored>(base.info.caster, new CastInfo(base.info.caster));
			base.info.caster.Ability.attackAbility.ResetCooldown();
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			_channel.castMethod._length = _initLength * Mathf.Lerp(1f, fullChargeLengthMult, _channel.chargeAmount);
			_channel.castMethod._width = _initWidth * Mathf.Lerp(1f, fullChargeWidthMult, _channel.chargeAmount);
			_channel.UpdateCastMethod();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.info.caster != null && base.info.caster.Status.TryGetStatusEffect<Se_Q_CruelSun_Armored>(out var effect))
		{
			effect.Destroy();
		}
	}

	private void Complete()
	{
		CreateAbilityInstance(base.info.caster.position, _channel.castInfo.rotation, new CastInfo(base.info.caster, _channel.castInfo.angle), delegate(Ai_Q_CruelSun_Shockwave p)
		{
			float num = _channel.chargeAmount;
			if (num > fullGraceThreshold)
			{
				num = 1f;
			}
			p.Network_length = _initLength * Mathf.Lerp(1f, fullChargeLengthMult, num);
			p.Network_width = _initWidth * Mathf.Lerp(1f, fullChargeWidthMult, num);
			p.Network_chargeAmount = num;
		});
		base.info.caster.Control.Rotate(_channel.castInfo.rotation, immediately: true, 1f);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
