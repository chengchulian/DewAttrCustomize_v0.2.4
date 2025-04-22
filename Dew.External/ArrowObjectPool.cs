using System.Collections.Generic;
using UnityEngine;

internal class ArrowObjectPool : MonoBehaviour
{
	public static ArrowObjectPool current;

	[Tooltip("Assign the arrow prefab.")]
	public Indicator pooledObject;

	[Tooltip("Initial pooled amount.")]
	public int pooledAmount = 1;

	[Tooltip("Should the pooled amount increase.")]
	public bool willGrow = true;

	private List<Indicator> pooledObjects;

	private void Awake()
	{
		current = this;
	}

	private void Start()
	{
		pooledObjects = new List<Indicator>();
		for (int i = 0; i < pooledAmount; i++)
		{
			Indicator arrow = Object.Instantiate(pooledObject);
			arrow.transform.SetParent(base.transform, worldPositionStays: false);
			arrow.Activate(value: false);
			pooledObjects.Add(arrow);
		}
	}

	public Indicator GetPooledObject()
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].Active)
			{
				return pooledObjects[i];
			}
		}
		if (willGrow)
		{
			Indicator arrow = Object.Instantiate(pooledObject);
			arrow.transform.SetParent(base.transform, worldPositionStays: false);
			arrow.Activate(value: false);
			pooledObjects.Add(arrow);
			return arrow;
		}
		return null;
	}

	public void DeactivateAllPooledObjects()
	{
		foreach (Indicator pooledObject in pooledObjects)
		{
			pooledObject.Activate(value: false);
		}
	}
}
