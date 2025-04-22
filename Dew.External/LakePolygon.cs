using System;
using System.Collections.Generic;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Topology;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
public class LakePolygon : MonoBehaviour
{
	public int toolbarInt;

	public LakePolygonProfile currentProfile;

	public LakePolygonProfile oldProfile;

	public List<Vector3> points = new List<Vector3>();

	public List<Vector3> splinePoints = new List<Vector3>();

	public AnimationCurve terrainCarve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(10f, -2f));

	public float distSmooth = 5f;

	public float terrainSmoothMultiplier = 5f;

	public bool overrideLakeRender;

	public float uvScale = 1f;

	public bool receiveShadows;

	public ShadowCastingMode shadowCastingMode;

	public AnimationCurve terrainPaintCarve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public int currentSplatMap = 1;

	public float distanceClearFoliage = 1f;

	public float distanceClearFoliageTrees = 1f;

	public bool mixTwoSplatMaps;

	public int secondSplatMap = 1;

	public bool addCliffSplatMap;

	public int cliffSplatMap = 1;

	public float cliffAngle = 25f;

	public float cliffBlend = 1f;

	public int cliffSplatMapOutside = 1;

	public float cliffAngleOutside = 45f;

	public float cliffBlendOutside = 1f;

	public bool noiseCarve;

	public float noiseMultiplierInside = 1f;

	public float noiseMultiplierOutside = 0.25f;

	public float noiseSizeX = 0.2f;

	public float noiseSizeZ = 0.2f;

	public bool noisePaint;

	public float noiseMultiplierInsidePaint = 1f;

	public float noiseMultiplierOutsidePaint = 0.5f;

	public float noiseSizeXPaint = 0.2f;

	public float noiseSizeZPaint = 0.2f;

	public float maximumTriangleSize = 50f;

	public float traingleDensity = 0.2f;

	public float height;

	public bool lockHeight = true;

	public float yOffset;

	public float trianglesGenerated;

	public float vertsGenerated;

	public global::UnityEngine.Mesh currentMesh;

	public MeshFilter meshfilter;

	public bool showVertexColors;

	public bool showFlowMap;

	public bool overrideFlowMap;

	public float automaticFlowMapScale = 0.2f;

	public bool noiseflowMap;

	public float noiseMultiplierflowMap = 1f;

	public float noiseSizeXflowMap = 0.2f;

	public float noiseSizeZflowMap = 0.2f;

	public bool drawOnMesh;

	public bool drawOnMeshFlowMap;

	public Color drawColor = Color.black;

	public bool drawColorR = true;

	public bool drawColorG = true;

	public bool drawColorB = true;

	public bool drawColorA = true;

	public bool drawOnMultiple;

	public float opacity = 0.1f;

	public float drawSize = 1f;

	public Material oldMaterial;

	public Color[] colors;

	public List<Vector2> colorsFlowMap = new List<Vector2>();

	public float floatSpeed = 10f;

	public float flowSpeed = 1f;

	public float flowDirection;

	public float closeDistanceSimulation = 5f;

	public int angleSimulation = 5;

	public float checkDistanceSimulation = 50f;

	public bool removeFirstPointSimulation = true;

	public bool normalFromRaycast;

	public bool snapToTerrain;

	public LayerMask snapMask = 1;

	public LakePolygonCarveData lakePolygonCarveData;

	public LakePolygonCarveData lakePolygonPaintData;

	public LakePolygonCarveData lakePolygonClearData;

	public List<GameObject> meshGOs = new List<GameObject>();

	public void AddPoint(Vector3 position)
	{
		points.Add(position);
	}

	public void AddPointAfter(int i)
	{
		Vector3 position = points[i];
		if (i < points.Count - 1 && points.Count > i + 1)
		{
			Vector3 positionSecond = points[i + 1];
			if (Vector3.Distance(positionSecond, position) > 0f)
			{
				position = (position + positionSecond) * 0.5f;
			}
			else
			{
				position.x += 1f;
			}
		}
		else if (points.Count > 1 && i == points.Count - 1)
		{
			Vector3 positionSecond2 = points[i - 1];
			if (Vector3.Distance(positionSecond2, position) > 0f)
			{
				position += position - positionSecond2;
			}
			else
			{
				position.x += 1f;
			}
		}
		else
		{
			position.x += 1f;
		}
		points.Insert(i + 1, position);
	}

	public void ChangePointPosition(int i, Vector3 position)
	{
		points[i] = position;
	}

	public void RemovePoint(int i)
	{
		if (i < points.Count)
		{
			points.RemoveAt(i);
		}
	}

	public void RemovePoints(int fromID = -1)
	{
		for (int i = points.Count - 1; i > fromID; i--)
		{
			RemovePoint(i);
		}
	}

	private void CenterPivot()
	{
		_ = base.transform.position;
		Vector3 center = Vector3.zero;
		for (int i = 0; i < points.Count; i++)
		{
			center += points[i];
		}
		center /= (float)points.Count;
		for (int j = 0; j < points.Count; j++)
		{
			Vector3 vec = points[j];
			vec.x -= center.x;
			vec.y -= center.y;
			vec.z -= center.z;
			points[j] = vec;
		}
		base.transform.position += center;
	}

	public void GeneratePolygon(bool quick = false)
	{
		MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer != null)
		{
			meshRenderer.receiveShadows = receiveShadows;
			meshRenderer.shadowCastingMode = shadowCastingMode;
		}
		if (lockHeight)
		{
			for (int i = 1; i < points.Count; i++)
			{
				Vector3 vec = points[i];
				vec.y = points[0].y;
				points[i] = vec;
			}
		}
		if (points.Count < 3)
		{
			return;
		}
		CenterPivot();
		splinePoints.Clear();
		for (int j = 0; j < points.Count; j++)
		{
			CalculateCatmullRomSpline(j);
		}
		List<Vector3> verticesList = new List<Vector3>();
		List<Vector3> verts = new List<Vector3>();
		List<int> indices = new List<int>();
		verticesList.AddRange(splinePoints.ToArray());
		Polygon polygon = new Polygon();
		List<Vertex> vertexs = new List<Vertex>();
		for (int k = 0; k < verticesList.Count; k++)
		{
			Vertex vert = new Vertex(verticesList[k].x, verticesList[k].z);
			vert.z = verticesList[k].y;
			vertexs.Add(vert);
		}
		polygon.Add(new Contour(vertexs));
		ConstraintOptions options = new ConstraintOptions
		{
			ConformingDelaunay = true
		};
		QualityOptions quality = new QualityOptions
		{
			MinimumAngle = 90.0,
			MaximumArea = maximumTriangleSize
		};
		global::TriangleNet.Mesh mesh = (global::TriangleNet.Mesh)polygon.Triangulate(options, quality);
		polygon.Triangulate(options, quality);
		indices.Clear();
		foreach (Triangle triangle in mesh.triangles)
		{
			Vertex vertex = mesh.vertices[triangle.vertices[2].id];
			Vector3 v0 = new Vector3((float)vertex.x, (float)vertex.z, (float)vertex.y);
			vertex = mesh.vertices[triangle.vertices[1].id];
			Vector3 v1 = new Vector3((float)vertex.x, (float)vertex.z, (float)vertex.y);
			vertex = mesh.vertices[triangle.vertices[0].id];
			Vector3 v2 = new Vector3((float)vertex.x, (float)vertex.z, (float)vertex.y);
			indices.Add(verts.Count);
			indices.Add(verts.Count + 1);
			indices.Add(verts.Count + 2);
			verts.Add(v0);
			verts.Add(v1);
			verts.Add(v2);
		}
		Vector3[] vertices = verts.ToArray();
		int vertCount = vertices.Length;
		Vector3[] normals = new Vector3[vertCount];
		Vector2[] uvs = new Vector2[vertCount];
		colors = new Color[vertCount];
		for (int l = 0; l < vertCount; l++)
		{
			if (Physics.Raycast(vertices[l] + base.transform.position + Vector3.up * 10f, Vector3.down, out var hit, 1000f, snapMask.value) && snapToTerrain)
			{
				vertices[l] = hit.point - base.transform.position + new Vector3(0f, 0.1f, 0f);
			}
			vertices[l].y += yOffset;
			if (normalFromRaycast)
			{
				normals[l] = hit.normal;
			}
			else
			{
				normals[l] = Vector3.up;
			}
			uvs[l] = new Vector2(vertices[l].x, vertices[l].z) * 0.01f * uvScale;
			colors[l] = Color.black;
		}
		if (overrideFlowMap || quick)
		{
			while (colorsFlowMap.Count < vertCount)
			{
				colorsFlowMap.Add(new Vector2(0f, 1f));
			}
			while (colorsFlowMap.Count > vertCount)
			{
				colorsFlowMap.RemoveAt(colorsFlowMap.Count - 1);
			}
		}
		else
		{
			List<Vector2> lines = new List<Vector2>();
			List<Vector2> vert2 = new List<Vector2>();
			for (int m = 0; m < splinePoints.Count; m++)
			{
				lines.Add(new Vector2(splinePoints[m].x, splinePoints[m].z));
			}
			for (int n = 0; n < vertices.Length; n++)
			{
				vert2.Add(new Vector2(vertices[n].x, vertices[n].z));
			}
			colorsFlowMap.Clear();
			Vector2 flow = Vector2.zero;
			for (int num = 0; num < vertCount; num++)
			{
				float minDist = float.MaxValue;
				Vector2 minPoint = vert2[num];
				for (int num2 = 0; num2 < splinePoints.Count; num2++)
				{
					int idOne = num2;
					int idTwo = (num2 + 1) % lines.Count;
					Vector2 point;
					float dist = DistancePointLine(vert2[num], lines[idOne], lines[idTwo], out point);
					if (minDist > dist)
					{
						minDist = dist;
						minPoint = point;
					}
				}
				flow = -(minPoint - vert2[num]).normalized * (automaticFlowMapScale + (noiseflowMap ? (Mathf.PerlinNoise(vert2[num].x * noiseSizeXflowMap, vert2[num].y * noiseSizeZflowMap) * noiseMultiplierflowMap - noiseMultiplierflowMap * 0.5f) : 0f));
				colorsFlowMap.Add(flow);
			}
		}
		currentMesh = new global::UnityEngine.Mesh();
		vertsGenerated = vertCount;
		if (vertCount > 65000)
		{
			currentMesh.indexFormat = IndexFormat.UInt32;
		}
		currentMesh.vertices = vertices;
		currentMesh.subMeshCount = 1;
		currentMesh.SetTriangles(indices, 0);
		currentMesh.uv = uvs;
		currentMesh.uv4 = colorsFlowMap.ToArray();
		currentMesh.normals = normals;
		currentMesh.colors = colors;
		currentMesh.RecalculateTangents();
		currentMesh.RecalculateBounds();
		currentMesh.RecalculateTangents();
		currentMesh.RecalculateBounds();
		trianglesGenerated = indices.Count / 3;
		meshfilter = GetComponent<MeshFilter>();
		meshfilter.sharedMesh = currentMesh;
		MeshCollider meshCollider = GetComponent<MeshCollider>();
		if (meshCollider != null)
		{
			meshCollider.sharedMesh = currentMesh;
		}
	}

	public static LakePolygon CreatePolygon(Material material, List<Vector3> positions = null)
	{
		GameObject obj = new GameObject("Lake Polygon")
		{
			layer = LayerMask.NameToLayer("Water")
		};
		LakePolygon polygon = obj.AddComponent<LakePolygon>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.receiveShadows = false;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		if (material != null)
		{
			meshRenderer.sharedMaterial = material;
		}
		if (positions != null)
		{
			for (int i = 0; i < positions.Count; i++)
			{
				polygon.AddPoint(positions[i]);
			}
		}
		return polygon;
	}

	private void CalculateCatmullRomSpline(int pos)
	{
		Vector3 p0 = points[ClampListPos(pos - 1)];
		Vector3 p1 = points[pos];
		Vector3 p2 = points[ClampListPos(pos + 1)];
		Vector3 p3 = points[ClampListPos(pos + 2)];
		int loops = Mathf.FloorToInt(1f / traingleDensity);
		for (int i = 1; i <= loops; i++)
		{
			float t = (float)i * traingleDensity;
			splinePoints.Add(GetCatmullRomPosition(t, p0, p1, p2, p3));
		}
	}

	public int ClampListPos(int pos)
	{
		if (pos < 0)
		{
			pos = points.Count - 1;
		}
		if (pos > points.Count)
		{
			pos = 1;
		}
		else if (pos > points.Count - 1)
		{
			pos = 0;
		}
		return pos;
	}

	private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;
		return 0.5f * (a + b * t + c * t * t + d * t * t * t);
	}

	public float DistancePointLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd, out Vector2 pointProject)
	{
		pointProject = ProjectPointLine(point, lineStart, lineEnd);
		return Vector2.Distance(pointProject, point);
	}

	public Vector2 ProjectPointLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
	{
		Vector2 rhs = point - lineStart;
		Vector2 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector2 lhs = vector2;
		if (magnitude > 1E-06f)
		{
			lhs /= magnitude;
		}
		float num2 = Mathf.Clamp(Vector2.Dot(lhs, rhs), 0f, magnitude);
		return lineStart + lhs * num2;
	}

	public void TerrainCarve(bool terrainShow = false)
	{
		Terrain[] terrains = Terrain.activeTerrains;
		Physics.autoSyncTransforms = false;
		if (meshGOs != null && meshGOs.Count > 0)
		{
			foreach (GameObject meshGO2 in meshGOs)
			{
				global::UnityEngine.Object.DestroyImmediate(meshGO2);
			}
			meshGOs.Clear();
		}
		Terrain[] array = terrains;
		foreach (Terrain terrain in array)
		{
			TerrainData terrainData = terrain.terrainData;
			float polygonHeight = base.transform.position.y;
			float posY = terrain.transform.position.y;
			float sizeX = terrain.terrainData.size.x;
			float sizeY = terrain.terrainData.size.y;
			float sizeZ = terrain.terrainData.size.z;
			float[,] heightmapData;
			if (lakePolygonCarveData == null || distSmooth != lakePolygonCarveData.distSmooth)
			{
				float minX = float.MaxValue;
				float maxX = float.MinValue;
				float minZ = float.MaxValue;
				float maxZ = float.MinValue;
				for (int j = 0; j < splinePoints.Count; j++)
				{
					Vector3 point = base.transform.TransformPoint(splinePoints[j]);
					if (minX > point.x)
					{
						minX = point.x;
					}
					if (maxX < point.x)
					{
						maxX = point.x;
					}
					if (minZ > point.z)
					{
						minZ = point.z;
					}
					if (maxZ < point.z)
					{
						maxZ = point.z;
					}
				}
				float terrainTowidth = 1f / sizeZ * (float)(terrainData.heightmapResolution - 1);
				float terrainToheight = 1f / sizeX * (float)(terrainData.heightmapResolution - 1);
				minX -= terrain.transform.position.x + distSmooth;
				maxX -= terrain.transform.position.x - distSmooth;
				minZ -= terrain.transform.position.z + distSmooth;
				maxZ -= terrain.transform.position.z - distSmooth;
				minX *= terrainToheight;
				maxX *= terrainToheight;
				minZ *= terrainTowidth;
				maxZ *= terrainTowidth;
				minX = (int)Mathf.Clamp(minX, 0f, terrainData.heightmapResolution);
				maxX = (int)Mathf.Clamp(maxX, 0f, terrainData.heightmapResolution);
				minZ = (int)Mathf.Clamp(minZ, 0f, terrainData.heightmapResolution);
				maxZ = (int)Mathf.Clamp(maxZ, 0f, terrainData.heightmapResolution);
				heightmapData = terrainData.GetHeights((int)minX, (int)minZ, (int)(maxX - minX), (int)(maxZ - minZ));
				Vector4[,] distances = new Vector4[heightmapData.GetLength(0), heightmapData.GetLength(1)];
				MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
				Transform transformTerrain = terrain.transform;
				Vector3 position = Vector3.zero;
				position.y = polygonHeight;
				for (int x = 0; x < heightmapData.GetLength(0); x++)
				{
					for (int z = 0; z < heightmapData.GetLength(1); z++)
					{
						position.x = ((float)z + minX) / terrainToheight + transformTerrain.position.x;
						position.z = ((float)x + minZ) / terrainTowidth + transformTerrain.position.z;
						Ray ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
						if (meshCollider.Raycast(ray, out var hit, 10000f))
						{
							float minDist = float.MaxValue;
							for (int k = 0; k < splinePoints.Count; k++)
							{
								int idOne = k;
								int idTwo = (k + 1) % splinePoints.Count;
								float dist = DistancePointLine(hit.point, base.transform.TransformPoint(splinePoints[idOne]), base.transform.TransformPoint(splinePoints[idTwo]));
								if (minDist > dist)
								{
									minDist = dist;
								}
							}
							distances[x, z] = new Vector3(hit.point.x, minDist, hit.point.z);
							continue;
						}
						float minDist2 = float.MaxValue;
						for (int l = 0; l < splinePoints.Count; l++)
						{
							int idOne2 = l;
							int idTwo2 = (l + 1) % splinePoints.Count;
							float dist2 = DistancePointLine(position, base.transform.TransformPoint(splinePoints[idOne2]), base.transform.TransformPoint(splinePoints[idTwo2]));
							if (minDist2 > dist2)
							{
								minDist2 = dist2;
							}
						}
						distances[x, z] = new Vector3(position.x, 0f - minDist2, position.z);
					}
				}
				global::UnityEngine.Object.DestroyImmediate(meshCollider);
				lakePolygonCarveData = new LakePolygonCarveData();
				lakePolygonCarveData.minX = minX;
				lakePolygonCarveData.maxX = maxX;
				lakePolygonCarveData.minZ = minZ;
				lakePolygonCarveData.maxZ = maxZ;
				lakePolygonCarveData.distances = distances;
			}
			heightmapData = terrainData.GetHeights((int)lakePolygonCarveData.minX, (int)lakePolygonCarveData.minZ, (int)(lakePolygonCarveData.maxX - lakePolygonCarveData.minX), (int)(lakePolygonCarveData.maxZ - lakePolygonCarveData.minZ));
			float noise = 0f;
			List<List<Vector4>> positionArray = new List<List<Vector4>>();
			for (int m = 0; m < heightmapData.GetLength(0); m++)
			{
				List<Vector4> positionArrayRow = new List<Vector4>();
				for (int n = 0; n < heightmapData.GetLength(1); n++)
				{
					Vector3 distance = lakePolygonCarveData.distances[m, n];
					if (distance.y > 0f)
					{
						noise = ((!noiseCarve) ? 0f : (Mathf.PerlinNoise((float)m * noiseSizeX, (float)n * noiseSizeZ) * noiseMultiplierInside - noiseMultiplierInside * 0.5f));
						float minDist3 = distance.y;
						heightmapData[m, n] = (noise + polygonHeight + terrainCarve.Evaluate(minDist3) - posY) / sizeY;
						positionArrayRow.Add(new Vector4(distance.x, heightmapData[m, n] * sizeY + posY, distance.z, 1f));
					}
					else if (0f - distance.y <= distSmooth)
					{
						noise = ((!noiseCarve) ? 0f : (Mathf.PerlinNoise((float)m * noiseSizeX, (float)n * noiseSizeZ) * noiseMultiplierOutside - noiseMultiplierOutside * 0.5f));
						float y = heightmapData[m, n] * sizeY + posY;
						float height = polygonHeight + terrainCarve.Evaluate(distance.y);
						float smoothValue = (0f - distance.y) / distSmooth;
						smoothValue = Mathf.Pow(smoothValue, terrainSmoothMultiplier);
						height = noise + Mathf.Lerp(height, y, smoothValue) - posY;
						heightmapData[m, n] = height / sizeY;
						positionArrayRow.Add(new Vector4(distance.x, heightmapData[m, n] * sizeY + posY, distance.z, Mathf.Pow(1f + distance.y / distSmooth, 0.5f)));
					}
					else
					{
						positionArrayRow.Add(new Vector4(distance.x, heightmapData[m, n] * sizeY + posY, distance.z, 0f));
					}
				}
				positionArray.Add(positionArrayRow);
			}
			if (terrainShow)
			{
				global::UnityEngine.Mesh meshTerrain = new global::UnityEngine.Mesh();
				meshTerrain.indexFormat = IndexFormat.UInt32;
				List<Vector3> vertices = new List<Vector3>();
				List<int> triangles = new List<int>();
				List<Color> colors = new List<Color>();
				foreach (List<Vector4> item in positionArray)
				{
					foreach (Vector4 vert in item)
					{
						vertices.Add(vert);
						colors.Add(new Color(vert.w, vert.w, vert.w, vert.w));
					}
				}
				for (int num = 0; num < positionArray.Count - 1; num++)
				{
					List<Vector4> rowPosition = positionArray[num];
					for (int num2 = 0; num2 < rowPosition.Count - 1; num2++)
					{
						triangles.Add(num2 + num * rowPosition.Count);
						triangles.Add(num2 + (num + 1) * rowPosition.Count);
						triangles.Add(num2 + 1 + num * rowPosition.Count);
						triangles.Add(num2 + 1 + num * rowPosition.Count);
						triangles.Add(num2 + (num + 1) * rowPosition.Count);
						triangles.Add(num2 + 1 + (num + 1) * rowPosition.Count);
					}
				}
				meshTerrain.SetVertices(vertices);
				meshTerrain.SetTriangles(triangles, 0);
				meshTerrain.SetColors(colors);
				meshTerrain.RecalculateNormals();
				meshTerrain.RecalculateTangents();
				meshTerrain.RecalculateBounds();
				GameObject meshGO = new GameObject("TerrainMesh");
				meshGO.transform.parent = base.transform;
				meshGO.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = meshGO.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterial = new Material(Shader.Find("Debug Terrain Carve"));
				meshRenderer.sharedMaterial.color = new Color(0f, 0.5f, 0f);
				meshGO.transform.position = Vector3.zero;
				meshGO.GetComponent<MeshFilter>().sharedMesh = meshTerrain;
				if (overrideLakeRender)
				{
					meshGO.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 5000;
				}
				else
				{
					meshGO.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 2980;
				}
				meshGOs.Add(meshGO);
				continue;
			}
			if (meshGOs != null && meshGOs.Count > 0)
			{
				foreach (GameObject meshGO3 in meshGOs)
				{
					global::UnityEngine.Object.DestroyImmediate(meshGO3);
				}
				meshGOs.Clear();
			}
			terrainData.SetHeights((int)lakePolygonCarveData.minX, (int)lakePolygonCarveData.minZ, heightmapData);
			terrain.Flush();
			lakePolygonCarveData = null;
		}
		Physics.autoSyncTransforms = true;
	}

	public void TerrainPaint(bool terrainShow = false)
	{
		Terrain[] terrains = Terrain.activeTerrains;
		Physics.autoSyncTransforms = false;
		if (meshGOs != null && meshGOs.Count > 0)
		{
			foreach (GameObject meshGO in meshGOs)
			{
				global::UnityEngine.Object.DestroyImmediate(meshGO);
			}
			meshGOs.Clear();
		}
		float distSmooth = this.distSmooth;
		float minKey = float.MaxValue;
		Keyframe[] keys = terrainPaintCarve.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe key = keys[i];
			if (key.time < minKey)
			{
				minKey = key.time;
			}
		}
		if (minKey < 0f)
		{
			distSmooth = 0f - minKey;
		}
		Terrain[] array = terrains;
		foreach (Terrain terrain in array)
		{
			TerrainData terrainData = terrain.terrainData;
			float polygonHeight = base.transform.position.y;
			float sizeX = terrain.terrainData.size.x;
			float sizeZ = terrain.terrainData.size.z;
			float[,,] alphamapData;
			if (lakePolygonPaintData == null || distSmooth != lakePolygonPaintData.distSmooth)
			{
				float minX = float.MaxValue;
				float maxX = float.MinValue;
				float minZ = float.MaxValue;
				float maxZ = float.MinValue;
				for (int j = 0; j < splinePoints.Count; j++)
				{
					Vector3 point = base.transform.TransformPoint(splinePoints[j]);
					if (minX > point.x)
					{
						minX = point.x;
					}
					if (maxX < point.x)
					{
						maxX = point.x;
					}
					if (minZ > point.z)
					{
						minZ = point.z;
					}
					if (maxZ < point.z)
					{
						maxZ = point.z;
					}
				}
				float terrainTowidth = 1f / sizeZ * (float)(terrainData.alphamapWidth - 1);
				float terrainToheight = 1f / sizeX * (float)(terrainData.alphamapHeight - 1);
				Debug.Log(terrainTowidth + " " + terrainToheight);
				minX -= terrain.transform.position.x + distSmooth;
				maxX -= terrain.transform.position.x - distSmooth;
				minZ -= terrain.transform.position.z + distSmooth;
				maxZ -= terrain.transform.position.z - distSmooth;
				minX *= terrainToheight;
				maxX *= terrainToheight;
				minZ *= terrainTowidth;
				maxZ *= terrainTowidth;
				minX = (int)Mathf.Clamp(minX, 0f, terrainData.alphamapWidth);
				maxX = (int)Mathf.Clamp(maxX, 0f, terrainData.alphamapWidth);
				minZ = (int)Mathf.Clamp(minZ, 0f, terrainData.alphamapHeight);
				maxZ = (int)Mathf.Clamp(maxZ, 0f, terrainData.alphamapHeight);
				alphamapData = terrainData.GetAlphamaps((int)minX, (int)minZ, (int)(maxX - minX), (int)(maxZ - minZ));
				Vector4[,] distances = new Vector4[alphamapData.GetLength(0), alphamapData.GetLength(1)];
				MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
				Transform transformTerrain = terrain.transform;
				Vector3 position = Vector3.zero;
				position.y = polygonHeight;
				for (int x = 0; x < alphamapData.GetLength(0); x++)
				{
					for (int z = 0; z < alphamapData.GetLength(1); z++)
					{
						position.x = ((float)z + minX) / terrainToheight + transformTerrain.position.x;
						position.z = ((float)x + minZ) / terrainTowidth + transformTerrain.position.z;
						Ray ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
						if (meshCollider.Raycast(ray, out var hit, 10000f))
						{
							float minDist = float.MaxValue;
							for (int k = 0; k < splinePoints.Count; k++)
							{
								int idOne = k;
								int idTwo = (k + 1) % splinePoints.Count;
								float dist = DistancePointLine(hit.point, base.transform.TransformPoint(splinePoints[idOne]), base.transform.TransformPoint(splinePoints[idTwo]));
								if (minDist > dist)
								{
									minDist = dist;
								}
							}
							float angle = 0f;
							if (addCliffSplatMap)
							{
								ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
								if (terrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 10000f))
								{
									angle = Vector3.Angle(hit.normal, Vector3.up);
								}
							}
							distances[x, z] = new Vector4(hit.point.x, minDist, hit.point.z, angle);
							continue;
						}
						float minDist2 = float.MaxValue;
						for (int l = 0; l < splinePoints.Count; l++)
						{
							int idOne2 = l;
							int idTwo2 = (l + 1) % splinePoints.Count;
							float dist2 = DistancePointLine(position, base.transform.TransformPoint(splinePoints[idOne2]), base.transform.TransformPoint(splinePoints[idTwo2]));
							if (minDist2 > dist2)
							{
								minDist2 = dist2;
							}
						}
						float angle2 = 0f;
						if (addCliffSplatMap)
						{
							ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
							if (terrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 10000f))
							{
								angle2 = Vector3.Angle(hit.normal, Vector3.up);
							}
						}
						distances[x, z] = new Vector4(position.x, 0f - minDist2, position.z, angle2);
					}
				}
				global::UnityEngine.Object.DestroyImmediate(meshCollider);
				lakePolygonPaintData = new LakePolygonCarveData();
				lakePolygonPaintData.minX = minX;
				lakePolygonPaintData.maxX = maxX;
				lakePolygonPaintData.minZ = minZ;
				lakePolygonPaintData.maxZ = maxZ;
				lakePolygonPaintData.distances = distances;
			}
			alphamapData = terrainData.GetAlphamaps((int)lakePolygonPaintData.minX, (int)lakePolygonPaintData.minZ, (int)(lakePolygonPaintData.maxX - lakePolygonPaintData.minX), (int)(lakePolygonPaintData.maxZ - lakePolygonPaintData.minZ));
			float noise = 0f;
			new List<List<Vector4>>();
			for (int m = 0; m < alphamapData.GetLength(0); m++)
			{
				new List<Vector4>();
				for (int n = 0; n < alphamapData.GetLength(1); n++)
				{
					Vector4 distance = lakePolygonPaintData.distances[m, n];
					if (!(0f - distance.y <= distSmooth) && !(distance.y > 0f))
					{
						continue;
					}
					if (!mixTwoSplatMaps)
					{
						noise = ((!noisePaint) ? 0f : ((!(distance.y > 0f)) ? (Mathf.PerlinNoise((float)m * noiseSizeXPaint, (float)n * noiseSizeZPaint) * noiseMultiplierOutsidePaint - noiseMultiplierOutsidePaint * 0.5f) : (Mathf.PerlinNoise((float)m * noiseSizeXPaint, (float)n * noiseSizeZPaint) * noiseMultiplierInsidePaint - noiseMultiplierInsidePaint * 0.5f)));
						float oldValue = alphamapData[m, n, currentSplatMap];
						alphamapData[m, n, currentSplatMap] = Mathf.Clamp01(Mathf.Lerp(alphamapData[m, n, currentSplatMap], 1f, terrainPaintCarve.Evaluate(distance.y) + noise * terrainPaintCarve.Evaluate(distance.y)));
						for (int num = 0; num < terrainData.terrainLayers.Length; num++)
						{
							if (num != currentSplatMap)
							{
								alphamapData[m, n, num] = ((oldValue == 1f) ? 0f : Mathf.Clamp01(alphamapData[m, n, num] * ((1f - alphamapData[m, n, currentSplatMap]) / (1f - oldValue))));
							}
						}
					}
					else
					{
						noise = ((!(distance.y > 0f)) ? (Mathf.PerlinNoise((float)m * noiseSizeXPaint, (float)n * noiseSizeZPaint) * noiseMultiplierOutsidePaint - noiseMultiplierOutsidePaint * 0.5f) : (Mathf.PerlinNoise((float)m * noiseSizeXPaint, (float)n * noiseSizeZPaint) * noiseMultiplierInsidePaint - noiseMultiplierInsidePaint * 0.5f));
						float oldValue2 = alphamapData[m, n, currentSplatMap];
						alphamapData[m, n, currentSplatMap] = Mathf.Clamp01(Mathf.Lerp(alphamapData[m, n, currentSplatMap], 1f, terrainPaintCarve.Evaluate(distance.y)));
						for (int num2 = 0; num2 < terrainData.terrainLayers.Length; num2++)
						{
							if (num2 != currentSplatMap)
							{
								alphamapData[m, n, num2] = ((oldValue2 == 1f) ? 0f : Mathf.Clamp01(alphamapData[m, n, num2] * ((1f - alphamapData[m, n, currentSplatMap]) / (1f - oldValue2))));
							}
						}
						if (noise > 0f)
						{
							oldValue2 = alphamapData[m, n, secondSplatMap];
							alphamapData[m, n, secondSplatMap] = Mathf.Clamp01(Mathf.Lerp(alphamapData[m, n, secondSplatMap], 1f, noise));
							for (int num3 = 0; num3 < terrainData.terrainLayers.Length; num3++)
							{
								if (num3 != secondSplatMap)
								{
									alphamapData[m, n, num3] = ((oldValue2 == 1f) ? 0f : Mathf.Clamp01(alphamapData[m, n, num3] * ((1f - alphamapData[m, n, secondSplatMap]) / (1f - oldValue2))));
								}
							}
						}
					}
					if (!addCliffSplatMap)
					{
						continue;
					}
					float oldValue3 = alphamapData[m, n, cliffSplatMap];
					if (distance.y > 0f)
					{
						if (!(distance.w > cliffAngle))
						{
							continue;
						}
						alphamapData[m, n, cliffSplatMap] = cliffBlend;
						for (int num4 = 0; num4 < terrainData.terrainLayers.Length; num4++)
						{
							if (num4 != cliffSplatMap)
							{
								alphamapData[m, n, num4] = ((oldValue3 == 1f) ? 0f : Mathf.Clamp01(alphamapData[m, n, num4] * ((1f - alphamapData[m, n, cliffSplatMap]) / (1f - oldValue3))));
							}
						}
					}
					else
					{
						if (!(distance.w > cliffAngleOutside))
						{
							continue;
						}
						alphamapData[m, n, cliffSplatMapOutside] = cliffBlendOutside;
						for (int num5 = 0; num5 < terrainData.terrainLayers.Length; num5++)
						{
							if (num5 != cliffSplatMapOutside)
							{
								alphamapData[m, n, num5] = ((oldValue3 == 1f) ? 0f : Mathf.Clamp01(alphamapData[m, n, num5] * ((1f - alphamapData[m, n, cliffSplatMapOutside]) / (1f - oldValue3))));
							}
						}
					}
				}
			}
			if (meshGOs != null && meshGOs.Count > 0)
			{
				foreach (GameObject meshGO2 in meshGOs)
				{
					global::UnityEngine.Object.DestroyImmediate(meshGO2);
				}
				meshGOs.Clear();
			}
			terrainData.SetAlphamaps((int)lakePolygonPaintData.minX, (int)lakePolygonPaintData.minZ, alphamapData);
			terrain.Flush();
			lakePolygonPaintData = null;
		}
		Physics.autoSyncTransforms = true;
	}

	public void TerrainClearTrees(bool details = true)
	{
		Terrain[] terrains = Terrain.activeTerrains;
		Physics.autoSyncTransforms = false;
		if (meshGOs != null && meshGOs.Count > 0)
		{
			foreach (GameObject meshGO in meshGOs)
			{
				global::UnityEngine.Object.DestroyImmediate(meshGO);
			}
			meshGOs.Clear();
		}
		Terrain[] array = terrains;
		foreach (Terrain terrain in array)
		{
			TerrainData terrainData = terrain.terrainData;
			Transform transformTerrain = terrain.transform;
			float polygonHeight = base.transform.position.y;
			_ = terrain.transform.position;
			float sizeX = terrain.terrainData.size.x;
			_ = terrain.terrainData.size;
			float sizeZ = terrain.terrainData.size.z;
			float minX = float.MaxValue;
			float maxX = float.MinValue;
			float minZ = float.MaxValue;
			float maxZ = float.MinValue;
			for (int j = 0; j < splinePoints.Count; j++)
			{
				Vector3 point = base.transform.TransformPoint(splinePoints[j]);
				if (minX > point.x)
				{
					minX = point.x;
				}
				if (maxX < point.x)
				{
					maxX = point.x;
				}
				if (minZ > point.z)
				{
					minZ = point.z;
				}
				if (maxZ < point.z)
				{
					maxZ = point.z;
				}
			}
			float terrainTowidth = 1f / sizeZ * (float)(terrainData.detailWidth - 1);
			float terrainToheight = 1f / sizeX * (float)(terrainData.detailHeight - 1);
			minX -= terrain.transform.position.x + distanceClearFoliage;
			maxX -= terrain.transform.position.x - distanceClearFoliage;
			minZ -= terrain.transform.position.z + distanceClearFoliage;
			maxZ -= terrain.transform.position.z - distanceClearFoliage;
			minX *= terrainToheight;
			maxX *= terrainToheight;
			minZ *= terrainTowidth;
			maxZ *= terrainTowidth;
			minX = (int)Mathf.Clamp(minX, 0f, terrainData.detailWidth);
			maxX = (int)Mathf.Clamp(maxX, 0f, terrainData.detailWidth);
			minZ = (int)Mathf.Clamp(minZ, 0f, terrainData.detailHeight);
			maxZ = (int)Mathf.Clamp(maxZ, 0f, terrainData.detailHeight);
			int[,] detailLayer = terrainData.GetDetailLayer((int)minX, (int)minZ, (int)(maxX - minX), (int)(maxZ - minZ), 0);
			Vector4[,] distances = new Vector4[detailLayer.GetLength(0), detailLayer.GetLength(1)];
			MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
			Vector3 position = Vector3.zero;
			position.y = polygonHeight;
			for (int x = 0; x < detailLayer.GetLength(0); x++)
			{
				for (int z = 0; z < detailLayer.GetLength(1); z++)
				{
					position.x = ((float)z + minX) / terrainToheight + transformTerrain.position.x;
					position.z = ((float)x + minZ) / terrainTowidth + transformTerrain.position.z;
					Ray ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
					if (meshCollider.Raycast(ray, out var hit, 10000f))
					{
						float minDist = float.MaxValue;
						for (int k = 0; k < splinePoints.Count; k++)
						{
							int idOne = k;
							int idTwo = (k + 1) % splinePoints.Count;
							float dist = DistancePointLine(hit.point, base.transform.TransformPoint(splinePoints[idOne]), base.transform.TransformPoint(splinePoints[idTwo]));
							if (minDist > dist)
							{
								minDist = dist;
							}
						}
						float angle = 0f;
						distances[x, z] = new Vector4(hit.point.x, minDist, hit.point.z, angle);
						continue;
					}
					float minDist2 = float.MaxValue;
					for (int l = 0; l < splinePoints.Count; l++)
					{
						int idOne2 = l;
						int idTwo2 = (l + 1) % splinePoints.Count;
						float dist2 = DistancePointLine(position, base.transform.TransformPoint(splinePoints[idOne2]), base.transform.TransformPoint(splinePoints[idTwo2]));
						if (minDist2 > dist2)
						{
							minDist2 = dist2;
						}
					}
					float angle2 = 0f;
					distances[x, z] = new Vector4(position.x, 0f - minDist2, position.z, angle2);
				}
			}
			if (!details)
			{
				List<TreeInstance> newTrees = new List<TreeInstance>();
				TreeInstance[] treeInstances = terrainData.treeInstances;
				position.y = polygonHeight;
				TreeInstance[] array2 = treeInstances;
				for (int m = 0; m < array2.Length; m++)
				{
					TreeInstance tree = array2[m];
					position.x = tree.position.x * sizeX + transformTerrain.position.x;
					position.z = tree.position.z * sizeZ + transformTerrain.position.z;
					Ray ray2 = new Ray(position + Vector3.up * 1000f, Vector3.down);
					if (meshCollider.Raycast(ray2, out var _, 10000f))
					{
						continue;
					}
					float minDist3 = float.MaxValue;
					for (int n = 0; n < splinePoints.Count; n++)
					{
						int idOne3 = n;
						int idTwo3 = (n + 1) % splinePoints.Count;
						float dist3 = DistancePointLine(position, base.transform.TransformPoint(splinePoints[idOne3]), base.transform.TransformPoint(splinePoints[idTwo3]));
						if (minDist3 > dist3)
						{
							minDist3 = dist3;
						}
					}
					if (minDist3 > distanceClearFoliageTrees)
					{
						newTrees.Add(tree);
					}
				}
				terrainData.treeInstances = newTrees.ToArray();
				global::UnityEngine.Object.DestroyImmediate(meshCollider);
			}
			lakePolygonClearData = new LakePolygonCarveData();
			lakePolygonClearData.minX = minX;
			lakePolygonClearData.maxX = maxX;
			lakePolygonClearData.minZ = minZ;
			lakePolygonClearData.maxZ = maxZ;
			lakePolygonClearData.distances = distances;
			if (details)
			{
				for (int num = 0; num < terrainData.detailPrototypes.Length; num++)
				{
					detailLayer = terrainData.GetDetailLayer((int)lakePolygonClearData.minX, (int)lakePolygonClearData.minZ, (int)(lakePolygonClearData.maxX - lakePolygonClearData.minX), (int)(lakePolygonClearData.maxZ - lakePolygonClearData.minZ), num);
					new List<List<Vector4>>();
					for (int num2 = 0; num2 < detailLayer.GetLength(0); num2++)
					{
						new List<Vector4>();
						for (int num3 = 0; num3 < detailLayer.GetLength(1); num3++)
						{
							Vector4 distance = lakePolygonClearData.distances[num2, num3];
							if (0f - distance.y <= distanceClearFoliage || distance.y > 0f)
							{
								_ = detailLayer[num2, num3];
								detailLayer[num2, num3] = 0;
							}
						}
					}
					terrainData.SetDetailLayer((int)lakePolygonClearData.minX, (int)lakePolygonClearData.minZ, num, detailLayer);
				}
			}
			if (meshGOs != null && meshGOs.Count > 0)
			{
				foreach (GameObject meshGO2 in meshGOs)
				{
					global::UnityEngine.Object.DestroyImmediate(meshGO2);
				}
				meshGOs.Clear();
			}
			terrain.Flush();
			lakePolygonClearData = null;
		}
		Physics.autoSyncTransforms = true;
	}

	public void Simulation()
	{
		List<Vector3> vectorPoints = new List<Vector3>();
		vectorPoints.Add(base.transform.TransformPoint(points[0]));
		int iterations = 1;
		for (int i = 0; i < iterations; i++)
		{
			List<Vector3> newPoints = new List<Vector3>();
			foreach (Vector3 vec in vectorPoints)
			{
				for (int angle = 0; angle <= 360; angle += angleSimulation)
				{
					Ray ray = new Ray(vec, new Vector3(Mathf.Cos((float)angle * (MathF.PI / 180f)), 0f, Mathf.Sin((float)angle * (MathF.PI / 180f))).normalized);
					if (Physics.Raycast(ray, out var hit, checkDistanceSimulation))
					{
						bool tooClose = false;
						Vector3 point = hit.point;
						foreach (Vector3 item in vectorPoints)
						{
							if (Vector3.Distance(point, item) < closeDistanceSimulation)
							{
								tooClose = true;
								break;
							}
						}
						foreach (Vector3 item2 in newPoints)
						{
							if (Vector3.Distance(point, item2) < closeDistanceSimulation)
							{
								tooClose = true;
								break;
							}
						}
						if (!tooClose)
						{
							newPoints.Add(point + ray.direction * 0.3f);
						}
						continue;
					}
					bool tooClose2 = false;
					Vector3 point2 = ray.origin + ray.direction * 50f;
					foreach (Vector3 item3 in vectorPoints)
					{
						if (Vector3.Distance(point2, item3) < closeDistanceSimulation)
						{
							tooClose2 = true;
							break;
						}
					}
					foreach (Vector3 item4 in newPoints)
					{
						if (Vector3.Distance(point2, item4) < closeDistanceSimulation)
						{
							tooClose2 = true;
							break;
						}
					}
					if (!tooClose2)
					{
						newPoints.Add(point2);
					}
				}
			}
			if (i == 0)
			{
				vectorPoints.AddRange(newPoints);
			}
			else
			{
				for (int k = 0; k < newPoints.Count; k++)
				{
					float min = float.MaxValue;
					int idMin = -1;
					Vector3 point3 = newPoints[k];
					for (int p = 0; p < vectorPoints.Count; p++)
					{
						Vector3 posOne = vectorPoints[p];
						Vector3 posTwo = vectorPoints[(p + 1) % vectorPoints.Count];
						bool intersects = false;
						for (int f = 0; f < vectorPoints.Count; f++)
						{
							if (p != f)
							{
								Vector3 posCheckOne = vectorPoints[f];
								Vector3 posCheckTwo = vectorPoints[(f + 1) % vectorPoints.Count];
								if (AreLinesIntersecting(posOne, point3, posCheckOne, posCheckTwo) || AreLinesIntersecting(point3, posTwo, posCheckOne, posCheckTwo))
								{
									intersects = true;
									break;
								}
							}
						}
						if (!intersects)
						{
							float dist = Vector3.Distance(point3, posTwo);
							if (min > dist)
							{
								min = dist;
								idMin = (p + 1) % vectorPoints.Count;
							}
						}
					}
					if (idMin > -1)
					{
						vectorPoints.Insert(idMin, point3);
					}
				}
			}
			if (i == 0 && removeFirstPointSimulation)
			{
				vectorPoints.RemoveAt(0);
			}
		}
		points.Clear();
		foreach (Vector3 vec2 in vectorPoints)
		{
			points.Add(base.transform.InverseTransformPoint(vec2));
		}
		GeneratePolygon();
	}

	public static bool AreLinesIntersecting(Vector3 l1_p1, Vector3 l1_p2, Vector3 l2_p1, Vector3 l2_p2, bool shouldIncludeEndPoints = true)
	{
		float epsilon = 1E-05f;
		bool isIntersecting = false;
		float denominator = (l2_p2.z - l2_p1.z) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.z - l1_p1.z);
		if (denominator != 0f)
		{
			float u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.z - l2_p1.z) - (l2_p2.z - l2_p1.z) * (l1_p1.x - l2_p1.x)) / denominator;
			float u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.z - l2_p1.z) - (l1_p2.z - l1_p1.z) * (l1_p1.x - l2_p1.x)) / denominator;
			if (shouldIncludeEndPoints)
			{
				if (u_a >= 0f + epsilon && u_a <= 1f - epsilon && u_b >= 0f + epsilon && u_b <= 1f - epsilon)
				{
					isIntersecting = true;
				}
			}
			else if (u_a > 0f + epsilon && u_a < 1f - epsilon && u_b > 0f + epsilon && u_b < 1f - epsilon)
			{
				isIntersecting = true;
			}
		}
		return isIntersecting;
	}

	public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		return Vector3.Distance(ProjectPointLine(point, lineStart, lineEnd), point);
	}

	public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 rhs = point - lineStart;
		Vector3 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector3 lhs = vector2;
		if (magnitude > 1E-06f)
		{
			lhs /= magnitude;
		}
		float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
		return lineStart + lhs * num2;
	}
}
