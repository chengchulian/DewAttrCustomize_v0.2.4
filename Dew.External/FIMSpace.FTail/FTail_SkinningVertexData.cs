using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FTail;

[Serializable]
public class FTail_SkinningVertexData
{
	public Vector3 position;

	public int[] bonesIndexes;

	public int allMeshBonesCount;

	public float[] weights;

	public float[] debugDists;

	public float[] debugDistWeights;

	public float[] debugWeights;

	public FTail_SkinningVertexData(Vector3 pos)
	{
		position = pos;
	}

	public float DistanceToLine(Vector3 pos, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 dirVector1 = pos - lineStart;
		Vector3 dirVector2 = (lineEnd - lineStart).normalized;
		float distance = Vector3.Distance(lineStart, lineEnd);
		float dot = Vector3.Dot(dirVector2, dirVector1);
		if (dot <= 0f)
		{
			return Vector3.Distance(pos, lineStart);
		}
		if (dot >= distance)
		{
			return Vector3.Distance(pos, lineEnd);
		}
		Vector3 dotVector = dirVector2 * dot;
		Vector3 closestPoint = lineStart + dotVector;
		return Vector3.Distance(pos, closestPoint);
	}

	public void CalculateVertexParameters(Vector3[] bonesPos, Quaternion[] bonesRot, Vector3[] boneAreas, int maxWeightedBones, float spread, Vector3 spreadOffset, float spreadPower = 1f)
	{
		allMeshBonesCount = bonesPos.Length;
		List<Vector2> calculatedDistances = new List<Vector2>();
		for (int i = 0; i < bonesPos.Length; i++)
		{
			Vector3 boneEnd = ((i == bonesPos.Length - 1) ? Vector3.Lerp(bonesPos[i], bonesPos[i] + (bonesPos[i] - bonesPos[i - 1]), 0.9f) : Vector3.Lerp(bonesPos[i], bonesPos[i + 1], 0.9f));
			boneEnd += bonesRot[i] * spreadOffset;
			float distance = DistanceToLine(position, bonesPos[i], boneEnd);
			calculatedDistances.Add(new Vector2(i, distance));
		}
		calculatedDistances.Sort((Vector2 a, Vector2 b) => a.y.CompareTo(b.y));
		int maxBones = Mathf.Min(maxWeightedBones, bonesPos.Length);
		bonesIndexes = new int[maxBones];
		float[] nearestDistances = new float[maxBones];
		for (int j = 0; j < maxBones; j++)
		{
			bonesIndexes[j] = (int)calculatedDistances[j].x;
			nearestDistances[j] = calculatedDistances[j].y;
		}
		float[] boneWeightsForVertex = new float[maxBones];
		AutoSetBoneWeights(boneWeightsForVertex, nearestDistances, spread, spreadPower, boneAreas);
		float weightLeft = 1f;
		weights = new float[maxBones];
		for (int k = 0; k < maxBones && (spread != 0f || k <= 0); k++)
		{
			if (weightLeft <= 0f)
			{
				weights[k] = 0f;
				continue;
			}
			float targetWeight = boneWeightsForVertex[k];
			weightLeft -= targetWeight;
			if (weightLeft <= 0f)
			{
				targetWeight += weightLeft;
			}
			else if (k == maxBones - 1)
			{
				targetWeight += weightLeft;
			}
			weights[k] = targetWeight;
		}
	}

	public void AutoSetBoneWeights(float[] weightForBone, float[] distToBone, float spread, float spreadPower, Vector3[] boneAreas)
	{
		int bonesC = weightForBone.Length;
		float[] boneLengths = new float[bonesC];
		for (int i = 0; i < boneLengths.Length; i++)
		{
			boneLengths[i] = boneAreas[i].magnitude;
		}
		float[] normalizedDistanceWeights = new float[bonesC];
		for (int j = 0; j < weightForBone.Length; j++)
		{
			weightForBone[j] = 0f;
		}
		float normalizeDistance = 0f;
		for (int k = 0; k < bonesC; k++)
		{
			normalizeDistance += distToBone[k];
		}
		for (int l = 0; l < bonesC; l++)
		{
			normalizedDistanceWeights[l] = 1f - distToBone[l] / normalizeDistance;
		}
		debugDists = distToBone;
		if (bonesC == 1 || spread == 0f)
		{
			weightForBone[0] = 1f;
			return;
		}
		if (bonesC == 2)
		{
			float normalizer = 1f;
			weightForBone[0] = 1f;
			float distRange = Mathf.InverseLerp(distToBone[0] + boneLengths[0] / 1.25f * spread, distToBone[0], distToBone[1]);
			debugDists[0] = distRange;
			normalizer += (weightForBone[1] = DistributionIn(Mathf.Lerp(0f, 1f, distRange), Mathf.Lerp(1.5f, 16f, spreadPower)));
			debugDistWeights = new float[weightForBone.Length];
			weightForBone.CopyTo(debugDistWeights, 0);
			for (int m = 0; m < bonesC; m++)
			{
				weightForBone[m] /= normalizer;
			}
			debugWeights = weightForBone;
			return;
		}
		float reffVal = boneLengths[0] / 10f;
		float refLength = boneLengths[0] / 2f;
		float normalizer2 = 0f;
		for (int n = 0; n < bonesC; n++)
		{
			float weight = Mathf.InverseLerp(0f, reffVal + refLength * spread, distToBone[n]);
			float value = Mathf.Lerp(1f, 0f, weight);
			if (n == 0 && value == 0f)
			{
				value = 1f;
			}
			weightForBone[n] = value;
			normalizer2 += value;
		}
		debugDistWeights = new float[weightForBone.Length];
		weightForBone.CopyTo(debugDistWeights, 0);
		for (int num = 0; num < bonesC; num++)
		{
			weightForBone[num] /= normalizer2;
		}
		debugWeights = weightForBone;
	}

	public static float DistributionIn(float k, float power)
	{
		return Mathf.Pow(k, power + 1f);
	}

	public static Color GetBoneIndicatorColor(int boneIndex, int bonesCount, float s = 0.9f, float v = 0.9f)
	{
		return Color.HSVToRGB(((float)boneIndex * 1.125f / (float)bonesCount + 0.125f * (float)boneIndex + 0.3f) % 1f, s, v);
	}

	public Color GetWeightColor()
	{
		Color lerped = GetBoneIndicatorColor(bonesIndexes[0], allMeshBonesCount, 1f, 1f);
		for (int i = 1; i < bonesIndexes.Length; i++)
		{
			lerped = Color.Lerp(lerped, GetBoneIndicatorColor(bonesIndexes[i], allMeshBonesCount, 1f, 1f), weights[i]);
		}
		return lerped;
	}
}
