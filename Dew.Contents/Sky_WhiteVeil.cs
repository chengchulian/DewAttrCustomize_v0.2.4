using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Sky_WhiteVeil : MonoBehaviour
{
	public GameObject particleSystemPrefab;

	private Mesh _mesh;

	private void Start()
	{
		CreateVeil();
	}

	private void CreateVeil()
	{
		if (_mesh != null)
		{
			global::UnityEngine.Object.DestroyImmediate(_mesh);
			_mesh = null;
		}
		Cells2D<MapCellType> cells = SingletonDewNetworkBehaviour<Room>.instance.GetComponent<RoomMap>().mapData.cells;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		for (int i = 0; i < cells.dataWidth; i++)
		{
			for (int j = 0; j < cells.dataHeight; j++)
			{
				if (cells.Get((i, j)) == MapCellType.Wall)
				{
					list.Add(base.transform.InverseTransformPoint(Dew.GetPositionOnGround(cells.GetWorldPos((i, j)).ToXZ())));
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		list2.Add(list[0]);
		list.RemoveAt(0);
		while (list.Count > 0)
		{
			Vector3 a = list2[^1];
			float num = float.PositiveInfinity;
			int index = -1;
			for (int k = 0; k < list.Count; k++)
			{
				float num2 = Vector3.Distance(a, list[k]);
				if (num2 < num)
				{
					num = num2;
					index = k;
				}
			}
			if (num < 2f)
			{
				list2.Add(list[index]);
			}
			list.RemoveAt(index);
		}
		for (int l = 0; l < list2.Count; l++)
		{
			list2[l] += new Vector3(Rand(-0.5f, 0.5f, list2[l].WithY(0f).GetHashCode()), Rand(-0.5f, 0.5f, list2[l].WithY(0f).GetHashCode() + 1), Rand(-0.5f, 0.5f, list2[l].WithY(0f).GetHashCode() + 2));
		}
		for (int num3 = base.transform.childCount - 1; num3 >= 0; num3--)
		{
			Transform child = base.transform.GetChild(num3);
			if (!(child.gameObject == particleSystemPrefab))
			{
				global::UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
		if (particleSystemPrefab != null)
		{
			for (int m = 0; m < list2.Count; m++)
			{
				global::UnityEngine.Object.Instantiate(particleSystemPrefab, base.transform.TransformPoint(list2[m]), Quaternion.Euler(0f, global::UnityEngine.Random.Range(0, 360), 0f), base.transform).SetActive(value: true);
			}
		}
		CreateVeilMeshFromPoints(list2);
	}

	private void CreateVeilMeshFromPoints(List<Vector3> basePoints)
	{
		_mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		int num = 8;
		int num2 = 20;
		int num3 = 12;
		float num4 = 5f / (float)num2;
		for (int i = 0; i <= num2; i++)
		{
			float num5 = (float)i * num4;
			for (int j = 0; j < basePoints.Count; j++)
			{
				Vector3 p4 = basePoints[(j - 1 + basePoints.Count) % basePoints.Count];
				Vector3 p5 = basePoints[j];
				Vector3 p6 = basePoints[(j + 1) % basePoints.Count];
				Vector3 p7 = basePoints[(j + 2) % basePoints.Count];
				for (int k = 0; k <= num3; k++)
				{
					if (k != num3 || j != basePoints.Count - 1)
					{
						float t2 = (float)k / (float)num3;
						Vector3 vector = CatmullRom(p4, p5, p6, p7, t2);
						list.Add(new Vector3(vector.x, vector.y + num5, vector.z));
					}
				}
			}
		}
		List<int> list2 = new List<int>();
		int num6 = basePoints.Count * (num3 + 1) - 1;
		for (int l = 0; l < num2; l++)
		{
			for (int m = 0; m < num6; m++)
			{
				int num7 = (m + 1) % num6;
				int num8 = l * num6;
				int num9 = (l + 1) * num6;
				if ((m + 1) % (num3 + 1) != 0)
				{
					list2.Add(num8 + m);
					list2.Add(num9 + m);
					list2.Add(num8 + num7);
					list2.Add(num8 + num7);
					list2.Add(num9 + m);
					list2.Add(num9 + num7);
				}
			}
		}
		Vector2[] array = new Vector2[list.Count];
		for (int n = 0; n <= num2; n++)
		{
			for (int num10 = 0; num10 < basePoints.Count; num10++)
			{
				for (int num11 = 0; num11 <= num3; num11++)
				{
					if (num11 != num3 || num10 != basePoints.Count - 1)
					{
						float num12 = (float)num10 / (float)num;
						float num13 = (float)num11 / (float)num3 / (float)num;
						float x = num12 + num13;
						float y = (float)n / (float)num2;
						int num14 = n * num6 + num10 * (num3 + 1) + num11;
						array[num14] = new Vector2(x, y);
					}
				}
			}
		}
		_mesh.vertices = list.ToArray();
		_mesh.triangles = list2.ToArray();
		_mesh.uv = array;
		_mesh.RecalculateNormals();
		_mesh.RecalculateBounds();
		GetComponent<MeshFilter>().mesh = _mesh;
		static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float num15 = t * t;
			float num16 = num15 * t;
			return 0.5f * ((0f - num16 + 2f * num15 - t) * p0 + (3f * num16 - 5f * num15 + 2f) * p1 + (-3f * num16 + 4f * num15 + t) * p2 + (num16 - num15) * p3);
		}
	}

	public static float Rand(float min, float max, int seed)
	{
		double num = new global::System.Random(seed).NextDouble();
		return (float)((double)min + num * (double)(max - min));
	}
}
