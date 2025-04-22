using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomMod_EngulfedInFlame : RoomModifierBase
{
	public GameObject treeEffect;

	public float effectChance;

	public Color treeTint;

	private List<GameObject> _treeEffects = new List<GameObject>();

	private TerrainData _originalTerrainData;

	public override void OnStartServer()
	{
		base.OnStartServer();
		Entity[] array = NetworkedManagerBase<ActorManager>.instance.allEntities.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is Monster { campPosition: not null } monster)
			{
				monster.Destroy();
			}
		}
		NetworkedManagerBase<ActorManager>.instance.onEntityAdd += new Action<Entity>(OnEntityAdd);
		foreach (Entity allEntity in NetworkedManagerBase<ActorManager>.instance.allEntities)
		{
			OnEntityAdd(allEntity);
		}
	}

	public override void OnStart()
	{
		base.OnStart();
		Terrain terrain = global::UnityEngine.Object.FindObjectOfType<Terrain>();
		if (terrain == null)
		{
			return;
		}
		_originalTerrainData = terrain.terrainData;
		terrain.terrainData = global::UnityEngine.Object.Instantiate(_originalTerrainData);
		TreePrototype[] array = _originalTerrainData.treePrototypes.ToArray();
		bool[] array2 = new bool[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			TreePrototype treePrototype = array[i];
			array2[i] = treePrototype.prefab.TryGetComponent<Forest_FlammableTree>(out var _);
			GameObject gameObject = global::UnityEngine.Object.Instantiate(treePrototype.prefab);
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				Material[] array3 = renderer.sharedMaterials.ToArray();
				for (int k = 0; k < array3.Length; k++)
				{
					array3[k] = global::UnityEngine.Object.Instantiate(array3[k]);
					Color color = array3[k].GetColor("_BaseColor");
					array3[k].SetColor("_BaseColor", treeTint * color);
				}
				renderer.sharedMaterials = array3;
			}
			treePrototype.prefab = gameObject;
		}
		terrain.terrainData.treePrototypes = array;
		Vector3 size = _originalTerrainData.size;
		Vector3 vector = terrain.transform.position;
		TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
		for (int j = 0; j < treeInstances.Length; j++)
		{
			TreeInstance treeInstance = treeInstances[j];
			if (!(global::UnityEngine.Random.value > effectChance) && array2[treeInstance.prototypeIndex])
			{
				Vector3 vector2 = Vector3.Scale(treeInstance.position, size) + vector;
				Quaternion quaternion = Quaternion.AngleAxis(treeInstance.rotation * 57.29578f, Vector3.up);
				_treeEffects.Add(global::UnityEngine.Object.Instantiate(treeEffect, vector2, quaternion));
			}
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		foreach (GameObject treeEffect in _treeEffects)
		{
			if (treeEffect != null)
			{
				global::UnityEngine.Object.Destroy(treeEffect);
			}
		}
		_treeEffects.Clear();
		if (_originalTerrainData == null)
		{
			return;
		}
		Terrain terrain = global::UnityEngine.Object.FindObjectOfType<Terrain>();
		if (terrain == null)
		{
			return;
		}
		terrain.terrainData = _originalTerrainData;
		if (!base.isServer || NetworkedManagerBase<ActorManager>.instance == null)
		{
			return;
		}
		NetworkedManagerBase<ActorManager>.instance.onEntityAdd -= new Action<Entity>(OnEntityAdd);
		foreach (Entity allEntity in NetworkedManagerBase<ActorManager>.instance.allEntities)
		{
			if (allEntity.Status.TryGetStatusEffect<Se_EngulfedInFlame>(out var effect))
			{
				effect.Destroy();
			}
		}
	}

	private void OnEntityAdd(Entity obj)
	{
		if (obj is Monster monster)
		{
			monster.CreateStatusEffect<Se_EngulfedInFlame>(monster, new CastInfo(monster));
		}
	}

	private void MirrorProcessed()
	{
	}
}
