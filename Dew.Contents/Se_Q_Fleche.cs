using System;
using System.Collections.Generic;
using UnityEngine;

public class Se_Q_Fleche : StackedStatusEffect
{
	public ScalingValue startStack;

	public ScalingValue maxRefundCount;

	public float duration;

	public float refundGraceTime;

	public bool resetDurationOnActivate;

	public Vector2 distanceRange;

	public Transform stackItemParent;

	public GameObject stackDisplayPrefabOn;

	public GameObject stackDisplayPrefabRefundable;

	public GameObject stackDisplayPrefabOff;

	public float stackItemSpacing;

	public GameObject refundEffect;

	[NonSerialized]
	public Dictionary<Entity, float> refundList;

	private List<(GameObject, GameObject, GameObject)> _stackDisplayItems;

	private int _maxRefundCount;

	private int _currentRefundCount;

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	protected override void ActiveFrameUpdate()
	{
	}

	protected override void OnStackChange(int oldStack, int newStack)
	{
	}

	private void MirrorProcessed()
	{
	}
}
