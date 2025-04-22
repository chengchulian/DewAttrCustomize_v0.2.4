using System;
using System.Collections.Generic;
using UnityEngine;

namespace HighlightPlus;

[ExecuteInEditMode]
public class HighlightSeeThroughOccluder : MonoBehaviour
{
	public OccluderMode mode;

	public DetectionMethod detectionMethod;

	[NonSerialized]
	public MeshData[] meshData;

	private List<Renderer> rr;

	private void OnEnable()
	{
		if (base.gameObject.activeInHierarchy)
		{
			Init();
		}
	}

	private void Init()
	{
		if (mode == OccluderMode.BlocksSeeThrough && detectionMethod == DetectionMethod.RayCast)
		{
			HighlightEffect.RegisterOccluder(this);
			return;
		}
		if (rr == null)
		{
			rr = new List<Renderer>();
		}
		else
		{
			rr.Clear();
		}
		GetComponentsInChildren(rr);
		int rrCount = rr.Count;
		meshData = new MeshData[rrCount];
		for (int k = 0; k < rrCount; k++)
		{
			meshData[k].renderer = rr[k];
			meshData[k].subMeshCount = 1;
			if (rr[k] is MeshRenderer)
			{
				MeshFilter mf = rr[k].GetComponent<MeshFilter>();
				if (mf != null && mf.sharedMesh != null)
				{
					meshData[k].subMeshCount = mf.sharedMesh.subMeshCount;
				}
			}
			else if (rr[k] is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer smr = (SkinnedMeshRenderer)rr[k];
				meshData[k].subMeshCount = smr.sharedMesh.subMeshCount;
			}
		}
		if (rrCount > 0)
		{
			HighlightEffect.RegisterOccluder(this);
		}
	}

	private void OnDisable()
	{
		HighlightEffect.UnregisterOccluder(this);
	}
}
