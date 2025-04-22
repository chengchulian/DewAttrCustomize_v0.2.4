using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame_PickupNumbers : MonoBehaviour
{
	public enum PickupType
	{
		GainGold,
		LoseGold,
		GainDreamDust,
		LoseDreamDust,
		GainStardust,
		LoseStardust
	}

	[Serializable]
	public class NumberConfig
	{
		public UI_InGame_PickupNumbers_Number prefab;

		[NonSerialized]
		public List<UI_InGame_PickupNumbers_Number> mergeableInstances = new List<UI_InGame_PickupNumbers_Number>();
	}

	public NumberConfig gainGold;

	public NumberConfig loseGold;

	public NumberConfig gainDreamDust;

	public NumberConfig loseDreamDust;

	public NumberConfig gainStardust;

	private void Start()
	{
		GameManager.CallOnReady(delegate
		{
			DewPlayer.local.ClientEvent_OnGoldChanged += new Action<int, int>(ClientPlayerEventOnGoldChanged);
			DewPlayer.local.ClientEvent_OnDreamDustChanged += new Action<int, int>(ClientPlayerEventOnDreamDustChanged);
			DewPlayer.local.ClientEvent_OnEarnStardust += new Action<int>(ClientEventOnEarnStardust);
		});
	}

	private void ClientEventOnEarnStardust(int obj)
	{
		HandlePickup(PickupType.GainStardust, obj);
	}

	private void HandlePickup(PickupType type, int delta)
	{
		NumberConfig config = type switch
		{
			PickupType.GainGold => gainGold, 
			PickupType.LoseGold => loseGold, 
			PickupType.GainDreamDust => gainDreamDust, 
			PickupType.LoseDreamDust => loseDreamDust, 
			PickupType.GainStardust => gainStardust, 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (config.mergeableInstances.Count > 0)
		{
			config.mergeableInstances[0].Merge(delta);
			return;
		}
		UI_InGame_PickupNumbers_Number ins = global::UnityEngine.Object.Instantiate(config.prefab, base.transform);
		config.mergeableInstances.Add(ins);
		ins.Setup(delta, config.mergeableInstances);
	}

	private void ClientPlayerEventOnDreamDustChanged(int arg1, int arg2)
	{
		int delta = arg2 - arg1;
		if (delta > 0)
		{
			HandlePickup(PickupType.GainDreamDust, delta);
		}
		else if (delta < 0)
		{
			HandlePickup(PickupType.LoseDreamDust, delta);
		}
	}

	private void ClientPlayerEventOnGoldChanged(int arg1, int arg2)
	{
		int delta = arg2 - arg1;
		if (delta > 0)
		{
			HandlePickup(PickupType.GainGold, delta);
		}
		else if (delta < 0)
		{
			HandlePickup(PickupType.LoseGold, delta);
		}
	}
}
