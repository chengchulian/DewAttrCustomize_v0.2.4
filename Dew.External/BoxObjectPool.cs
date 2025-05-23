using System.Collections.Generic;
using UnityEngine;

public class BoxObjectPool : MonoBehaviour
{
	public static BoxObjectPool current;

	[Tooltip("Assign the box prefab.")]
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
			Indicator box = Object.Instantiate(pooledObject);
			box.transform.SetParent(base.transform, worldPositionStays: false);
			box.Activate(value: false);
			pooledObjects.Add(box);
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
			Indicator box = Object.Instantiate(pooledObject);
			box.transform.SetParent(base.transform, worldPositionStays: false);
			box.Activate(value: false);
			pooledObjects.Add(box);
			return box;
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
