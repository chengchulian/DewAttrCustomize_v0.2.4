using System;
using System.Collections;
using UnityEngine;

public class Ai_Shrine_Hatred_CurrencyReward : AbilityInstance
{
	public GameObject fxExplode;

	public float animDuration = 1f;

	public DewEase ease;

	public Transform shakeTransform;

	[NonSerialized]
	public int gold;

	[NonSerialized]
	public int dreamDust;

	private Vector3 _startPos;

	private float _yPos;

	private Vector3 _originalPos;

	private float _nextShakeTime;

	private float _nextPosChangeTime;

	protected override IEnumerator OnCreateSequenced()
	{
		_originalPos = shakeTransform.localPosition;
		shakeTransform.localRotation = Quaternion.Euler(global::UnityEngine.Random.Range(0, 360), global::UnityEngine.Random.Range(0, 360), global::UnityEngine.Random.Range(0, 360));
		_yPos = Dew.GetPositionOnGround(base.transform.position).y;
		_startPos = base.transform.position;
		if (base.isServer)
		{
			yield return new SI.WaitForSeconds(animDuration);
			if (gold > 0)
			{
				NetworkedManagerBase<PickupManager>.instance.DropGold(isKillGold: false, isGivenByOtherPlayer: false, gold, base.transform.position, base.info.caster as Hero);
			}
			if (dreamDust > 0)
			{
				NetworkedManagerBase<PickupManager>.instance.DropDreamDust(isGivenByOtherPlayer: false, dreamDust, base.transform.position, base.info.caster as Hero);
			}
			FxPlayNewNetworked(fxExplode);
			Destroy();
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = Vector3.Lerp(_startPos, base.info.point, ease.Get(Mathf.Clamp01((Time.time - base.creationTime) / animDuration))).WithY(_yPos);
		if (Time.time > _nextShakeTime)
		{
			_nextShakeTime = Time.time + 1f / 60f;
			shakeTransform.localPosition = _originalPos + global::UnityEngine.Random.onUnitSphere * 0.02f;
		}
	}

	private void MirrorProcessed()
	{
	}
}
