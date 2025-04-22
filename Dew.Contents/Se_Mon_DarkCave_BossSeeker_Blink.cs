using System;
using UnityEngine;

public class Se_Mon_DarkCave_BossSeeker_Blink : StatusEffect
{
	public bool resetAtkCooldown;

	public float duration;

	public DewEase ease;

	[NonSerialized]
	public Vector3? customDestination;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}
}
