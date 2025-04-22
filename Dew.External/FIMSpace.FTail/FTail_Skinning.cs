using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FTail;

public static class FTail_Skinning
{
	public static FTail_SkinningVertexData[] CalculateVertexWeightingData(Mesh baseMesh, Transform[] bonesCoords, Vector3 spreadOffset, int weightBoneLimit = 2, float spreadValue = 0.8f, float spreadPower = 0.185f)
	{
		Vector3[] pos = new Vector3[bonesCoords.Length];
		Quaternion[] rot = new Quaternion[bonesCoords.Length];
		for (int i = 0; i < bonesCoords.Length; i++)
		{
			pos[i] = bonesCoords[0].parent.InverseTransformPoint(bonesCoords[i].position);
			rot[i] = bonesCoords[0].parent.rotation.QToLocal(bonesCoords[i].rotation);
		}
		return CalculateVertexWeightingData(baseMesh, pos, rot, spreadOffset, weightBoneLimit, spreadValue, spreadPower);
	}

	public static FTail_SkinningVertexData[] CalculateVertexWeightingData(Mesh baseMesh, Vector3[] bonesPos, Quaternion[] bonesRot, Vector3 spreadOffset, int weightBoneLimit = 2, float spreadValue = 0.8f, float spreadPower = 0.185f)
	{
		if (weightBoneLimit < 1)
		{
			weightBoneLimit = 1;
		}
		if (weightBoneLimit > 2)
		{
			weightBoneLimit = 2;
		}
		int vertCount = baseMesh.vertexCount;
		FTail_SkinningVertexData[] vertexDatas = new FTail_SkinningVertexData[vertCount];
		Vector3[] boneAreas = new Vector3[bonesPos.Length];
		for (int i = 0; i < bonesPos.Length - 1; i++)
		{
			boneAreas[i] = bonesPos[i + 1] - bonesPos[i];
		}
		if (boneAreas.Length > 1)
		{
			boneAreas[^1] = boneAreas[^2];
		}
		for (int j = 0; j < vertCount; j++)
		{
			vertexDatas[j] = new FTail_SkinningVertexData(baseMesh.vertices[j]);
			vertexDatas[j].CalculateVertexParameters(bonesPos, bonesRot, boneAreas, weightBoneLimit, spreadValue, spreadOffset, spreadPower);
		}
		return vertexDatas;
	}

	public static SkinnedMeshRenderer SkinMesh(Mesh baseMesh, Transform skinParent, Transform[] bonesStructure, FTail_SkinningVertexData[] vertData)
	{
		Vector3[] pos = new Vector3[bonesStructure.Length];
		Quaternion[] rot = new Quaternion[bonesStructure.Length];
		for (int i = 0; i < bonesStructure.Length; i++)
		{
			pos[i] = skinParent.InverseTransformPoint(bonesStructure[i].position);
			rot[i] = skinParent.rotation.QToLocal(bonesStructure[i].rotation);
		}
		return SkinMesh(baseMesh, pos, rot, vertData);
	}

	public static SkinnedMeshRenderer SkinMesh(Mesh baseMesh, Vector3[] bonesPositions, Quaternion[] bonesRotations, FTail_SkinningVertexData[] vertData)
	{
		if (bonesPositions == null)
		{
			return null;
		}
		if (bonesRotations == null)
		{
			return null;
		}
		if (baseMesh == null)
		{
			return null;
		}
		if (vertData == null)
		{
			return null;
		}
		Mesh newMesh = Object.Instantiate(baseMesh);
		newMesh.name = baseMesh.name + " [FSKINNED]";
		Transform newParent = new GameObject(baseMesh.name + " [FSKINNED]").transform;
		SkinnedMeshRenderer skin = newParent.gameObject.AddComponent<SkinnedMeshRenderer>();
		Transform[] bones = new Transform[bonesPositions.Length];
		Matrix4x4[] bindPoses = new Matrix4x4[bonesPositions.Length];
		string nameString = ((baseMesh.name.Length >= 6) ? baseMesh.name.Substring(0, 5) : baseMesh.name);
		for (int i = 0; i < bonesPositions.Length; i++)
		{
			bones[i] = new GameObject("BoneF-" + nameString + "[" + i + "]").transform;
			if (i == 0)
			{
				bones[i].SetParent(newParent, worldPositionStays: true);
			}
			else
			{
				bones[i].SetParent(bones[i - 1], worldPositionStays: true);
			}
			bones[i].transform.position = bonesPositions[i];
			bones[i].transform.rotation = bonesRotations[i];
			bindPoses[i] = bones[i].worldToLocalMatrix * newParent.localToWorldMatrix;
		}
		BoneWeight[] weights = new BoneWeight[newMesh.vertexCount];
		for (int v = 0; v < weights.Length; v++)
		{
			weights[v] = default(BoneWeight);
		}
		for (int j = 0; j < vertData.Length; j++)
		{
			for (int w = 0; w < vertData[j].weights.Length; w++)
			{
				weights[j] = SetWeightIndex(weights[j], w, vertData[j].bonesIndexes[w]);
				weights[j] = SetWeightToBone(weights[j], w, vertData[j].weights[w]);
			}
		}
		newMesh.bindposes = bindPoses;
		newMesh.boneWeights = weights;
		List<Vector3> normals = new List<Vector3>();
		List<Vector4> tangents = new List<Vector4>();
		baseMesh.GetNormals(normals);
		baseMesh.GetTangents(tangents);
		newMesh.SetNormals(normals);
		newMesh.SetTangents(tangents);
		newMesh.bounds = baseMesh.bounds;
		skin.sharedMesh = newMesh;
		skin.rootBone = bones[0];
		skin.bones = bones;
		return skin;
	}

	public static BoneWeight SetWeightIndex(BoneWeight weight, int bone = 0, int index = 0)
	{
		switch (bone)
		{
		case 1:
			weight.boneIndex1 = index;
			break;
		case 2:
			weight.boneIndex2 = index;
			break;
		case 3:
			weight.boneIndex3 = index;
			break;
		default:
			weight.boneIndex0 = index;
			break;
		}
		return weight;
	}

	public static BoneWeight SetWeightToBone(BoneWeight weight, int bone = 0, float value = 1f)
	{
		switch (bone)
		{
		case 1:
			weight.weight1 = value;
			break;
		case 2:
			weight.weight2 = value;
			break;
		case 3:
			weight.weight3 = value;
			break;
		default:
			weight.weight0 = value;
			break;
		}
		return weight;
	}
}
