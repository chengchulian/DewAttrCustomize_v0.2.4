using System.Collections.Generic;
using UnityEngine;

public class MeshColoringRam : MonoBehaviour
{
	public float height = 0.5f;

	public float threshold = 0.5f;

	public bool autoColor = true;

	public bool newMesh = true;

	public Vector3 oldPosition = Vector3.zero;

	public bool colorMeshLive;

	public LayerMask layer;

	private MeshFilter[] meshFilters;

	private bool colored;

	private static RamSpline[] ramSplines;

	private static LakePolygon[] lakePolygons;

	private void Start()
	{
		if (colorMeshLive)
		{
			if (ramSplines == null)
			{
				ramSplines = Object.FindObjectsOfType<RamSpline>();
			}
			if (lakePolygons == null)
			{
				lakePolygons = Object.FindObjectsOfType<LakePolygon>();
			}
			colored = false;
			meshFilters = base.gameObject.GetComponentsInChildren<MeshFilter>();
		}
	}

	private void Update()
	{
		if (colorMeshLive)
		{
			ColorMeshLive();
		}
	}

	public void ColorMeshLive()
	{
		colored = true;
		Ray ray = default(Ray);
		ray.direction = Vector3.up;
		Vector3 upVector = -Vector3.up * (height + threshold);
		Color white = Color.white;
		List<MeshCollider> meshColliders = new List<MeshCollider>();
		RamSpline[] array = ramSplines;
		foreach (RamSpline item in array)
		{
			meshColliders.Add(item.gameObject.AddComponent<MeshCollider>());
		}
		LakePolygon[] array2 = lakePolygons;
		foreach (LakePolygon item2 in array2)
		{
			meshColliders.Add(item2.gameObject.AddComponent<MeshCollider>());
		}
		bool backFace = Physics.queriesHitBackfaces;
		Physics.queriesHitBackfaces = true;
		MeshFilter[] array3 = meshFilters;
		foreach (MeshFilter meshFilter in array3)
		{
			Mesh mesh = meshFilter.sharedMesh;
			if (!(meshFilter.sharedMesh != null))
			{
				continue;
			}
			if (!colored)
			{
				mesh = (meshFilter.sharedMesh = Object.Instantiate(meshFilter.sharedMesh));
				colored = true;
			}
			int vertLength = mesh.vertices.Length;
			Vector3[] vertices = mesh.vertices;
			Color[] colors = mesh.colors;
			Transform transform = meshFilter.transform;
			float minY = float.MaxValue;
			Vector3 lowestPoint = vertices[0];
			for (int j = 0; j < vertLength; j++)
			{
				vertices[j] = transform.TransformPoint(vertices[j]) + upVector;
				if (vertices[j].y < minY)
				{
					minY = vertices[j].y;
					lowestPoint = vertices[j];
				}
			}
			if (colors.Length == 0)
			{
				colors = new Color[vertLength];
				for (int k = 0; k < colors.Length; k++)
				{
					colors[k] = white;
				}
			}
			ray.origin = lowestPoint;
			minY = float.MinValue;
			if (Physics.Raycast(ray, out var hit, 100f, layer))
			{
				minY = hit.point.y;
			}
			for (int l = 0; l < vertLength; l++)
			{
				if (vertices[l].y < minY)
				{
					float dist = Mathf.Abs(vertices[l].y - minY);
					if (dist > threshold)
					{
						colors[l].r = 0f;
					}
					else
					{
						colors[l].r = Mathf.Lerp(1f, 0f, dist / threshold);
					}
				}
				else
				{
					colors[l] = white;
				}
			}
			mesh.colors = colors;
		}
		foreach (MeshCollider item3 in meshColliders)
		{
			Object.Destroy(item3);
		}
		Physics.queriesHitBackfaces = backFace;
	}
}
