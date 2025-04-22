using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
public class RamSpline : MonoBehaviour
{
	public SplineProfile currentProfile;

	public SplineProfile oldProfile;

	public List<RamSpline> beginnigChildSplines = new List<RamSpline>();

	public List<RamSpline> endingChildSplines = new List<RamSpline>();

	public RamSpline beginningSpline;

	public RamSpline endingSpline;

	public int beginningConnectionID;

	public int endingConnectionID;

	public float beginningMinWidth = 0.5f;

	public float beginningMaxWidth = 1f;

	public float endingMinWidth = 0.5f;

	public float endingMaxWidth = 1f;

	public int toolbarInt;

	public bool invertUVDirection;

	public bool uvRotation = true;

	public MeshFilter meshfilter;

	public List<Vector4> controlPoints = new List<Vector4>();

	public List<Quaternion> controlPointsRotations = new List<Quaternion>();

	public List<Quaternion> controlPointsOrientation = new List<Quaternion>();

	public List<Vector3> controlPointsUp = new List<Vector3>();

	public List<Vector3> controlPointsDown = new List<Vector3>();

	public List<float> controlPointsSnap = new List<float>();

	public AnimationCurve meshCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

	public List<AnimationCurve> controlPointsMeshCurves = new List<AnimationCurve>();

	public bool normalFromRaycast;

	public bool snapToTerrain;

	public LayerMask snapMask = 1;

	public List<Vector3> points = new List<Vector3>();

	public List<Vector3> pointsUp = new List<Vector3>();

	public List<Vector3> pointsDown = new List<Vector3>();

	public List<Vector3> carvePointsUp = new List<Vector3>();

	public List<Vector3> carvePointsDown = new List<Vector3>();

	public List<Vector3> points2 = new List<Vector3>();

	public List<Vector3> verticesBeginning = new List<Vector3>();

	public List<Vector3> verticesEnding = new List<Vector3>();

	public List<Vector3> normalsBeginning = new List<Vector3>();

	public List<Vector3> normalsEnding = new List<Vector3>();

	public List<float> widths = new List<float>();

	public List<float> snaps = new List<float>();

	public List<float> lerpValues = new List<float>();

	public List<Quaternion> orientations = new List<Quaternion>();

	public List<Vector3> tangents = new List<Vector3>();

	public List<Vector3> normalsList = new List<Vector3>();

	public Color[] colors;

	public List<Vector2> colorsFlowMap = new List<Vector2>();

	public List<Vector3> verticeDirection = new List<Vector3>();

	public float floatSpeed = 10f;

	public bool generateOnStart;

	public float minVal = 0.5f;

	public float maxVal = 0.5f;

	public float width = 4f;

	public int vertsInShape = 3;

	public float traingleDensity = 0.2f;

	public float uvScale = 3f;

	public Material oldMaterial;

	public bool showVertexColors;

	public bool showFlowMap;

	public bool overrideFlowMap;

	public bool drawOnMesh;

	public bool drawOnMeshFlowMap;

	public bool uvScaleOverride;

	public bool debug;

	public bool debugNormals;

	public bool debugTangents;

	public bool debugBitangent;

	public bool debugFlowmap;

	public bool debugPoints;

	public bool debugPointsConnect;

	public bool debugMesh = true;

	public float distanceToDebug = 5f;

	public Color drawColor = Color.black;

	public bool drawColorR = true;

	public bool drawColorG = true;

	public bool drawColorB = true;

	public bool drawColorA = true;

	public bool drawOnMultiple;

	public float flowSpeed = 1f;

	public float flowDirection;

	public AnimationCurve flowFlat = new AnimationCurve(new Keyframe(0f, 0.025f), new Keyframe(0.5f, 0.05f), new Keyframe(1f, 0.025f));

	public AnimationCurve flowWaterfall = new AnimationCurve(new Keyframe(0f, 0.25f), new Keyframe(1f, 0.25f));

	public bool noiseflowMap;

	public float noiseMultiplierflowMap = 0.1f;

	public float noiseSizeXflowMap = 2f;

	public float noiseSizeZflowMap = 2f;

	public float opacity = 0.1f;

	public float drawSize = 1f;

	public float length;

	public float fulllength;

	public float uv3length;

	public float minMaxWidth;

	public float uvWidth;

	public float uvBeginning;

	public bool receiveShadows;

	public ShadowCastingMode shadowCastingMode;

	public bool generateMeshParts;

	public int meshPartsCount = 3;

	public List<Transform> meshesPartTransforms = new List<Transform>();

	public float simulatedRiverLength = 100f;

	public int simulatedRiverPoints = 10;

	public float simulatedMinStepSize = 1f;

	public bool simulatedNoUp;

	public bool simulatedBreakOnUp = true;

	public float terrainAdditionalWidth = 2f;

	public float terrainMeshSmoothX = 2f;

	public float terrainMeshSmoothZ = 10f;

	public float terrainSmoothMultiplier = 5f;

	public bool overrideRiverRender;

	public bool noiseWidth;

	public float noiseMultiplierWidth = 4f;

	public float noiseSizeWidth = 0.5f;

	public bool noiseCarve;

	public float noiseMultiplierInside = 1f;

	public float noiseMultiplierOutside = 0.25f;

	public float noiseSizeX = 0.2f;

	public float noiseSizeZ = 0.2f;

	public bool noisePaint;

	public float noiseMultiplierInsidePaint = 0.25f;

	public float noiseMultiplierOutsidePaint = 0.25f;

	public float noiseSizeXPaint = 0.2f;

	public float noiseSizeZPaint = 0.2f;

	public Terrain workTerrain;

	public List<Terrain> terrainsUnder = new List<Terrain>();

	public int currentWorkTerrain;

	public LayerMask maskCarve = 1;

	public AnimationCurve terrainCarve = new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(10f, -4f));

	public float distSmooth = 5f;

	public float distSmoothStart = 1f;

	public AnimationCurve terrainPaintCarve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public int currentSplatMap = 1;

	public bool mixTwoSplatMaps;

	public int secondSplatMap = 1;

	public bool addCliffSplatMap;

	public int cliffSplatMap = 1;

	public float cliffAngle = 45f;

	public float cliffBlend = 1f;

	public int cliffSplatMapOutside = 1;

	public float cliffAngleOutside = 45f;

	public float cliffBlendOutside = 1f;

	public float distanceClearFoliage = 1f;

	public float distanceClearFoliageTrees = 1f;

	public GameObject meshGO;

	public void Start()
	{
		if (generateOnStart)
		{
			GenerateSpline();
		}
	}

	public static RamSpline CreateSpline(Material splineMaterial = null, List<Vector4> positions = null, string name = "RamSpline")
	{
		GameObject obj = new GameObject(name)
		{
			layer = LayerMask.NameToLayer("Water")
		};
		RamSpline spline = obj.AddComponent<RamSpline>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.receiveShadows = false;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		if (splineMaterial != null)
		{
			meshRenderer.sharedMaterial = splineMaterial;
		}
		if (positions != null)
		{
			for (int i = 0; i < positions.Count; i++)
			{
				spline.AddPoint(positions[i]);
			}
		}
		return spline;
	}

	public void AddPoint(Vector4 position)
	{
		if (position.w == 0f)
		{
			if (controlPoints.Count > 0)
			{
				position.w = controlPoints[controlPoints.Count - 1].w;
			}
			else
			{
				position.w = width;
			}
		}
		controlPointsRotations.Add(Quaternion.identity);
		controlPoints.Add(position);
		controlPointsSnap.Add(0f);
		controlPointsMeshCurves.Add(new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f)));
	}

	public void AddPointAfter(int i)
	{
		Vector4 position = ((i != -1) ? controlPoints[i] : controlPoints[0]);
		if (i < controlPoints.Count - 1 && controlPoints.Count > i + 1)
		{
			Vector4 positionSecond = controlPoints[i + 1];
			if (Vector3.Distance(positionSecond, position) > 0f)
			{
				position = (position + positionSecond) * 0.5f;
			}
			else
			{
				position.x += 1f;
			}
		}
		else if (controlPoints.Count > 1 && i == controlPoints.Count - 1)
		{
			Vector4 positionSecond2 = controlPoints[i - 1];
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
		controlPoints.Insert(i + 1, position);
		controlPointsRotations.Insert(i + 1, Quaternion.identity);
		controlPointsSnap.Insert(i + 1, 0f);
		controlPointsMeshCurves.Insert(i + 1, new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f)));
	}

	public void ChangePointPosition(int i, Vector3 position)
	{
		ChangePointPosition(i, new Vector4(position.x, position.y, position.z, 0f));
	}

	public void ChangePointPosition(int i, Vector4 position)
	{
		Vector4 oldPos = controlPoints[i];
		if (position.w == 0f)
		{
			position.w = oldPos.w;
		}
		controlPoints[i] = position;
	}

	public void RemovePoint(int i)
	{
		if (i < controlPoints.Count)
		{
			controlPoints.RemoveAt(i);
			controlPointsRotations.RemoveAt(i);
			controlPointsMeshCurves.RemoveAt(i);
			controlPointsSnap.RemoveAt(i);
		}
	}

	public void RemovePoints(int fromID = -1)
	{
		for (int i = controlPoints.Count - 1; i > fromID; i--)
		{
			RemovePoint(i);
		}
	}

	public void GenerateBeginningParentBased()
	{
		vertsInShape = (int)Mathf.Round((float)(beginningSpline.vertsInShape - 1) * (beginningMaxWidth - beginningMinWidth) + 1f);
		if (vertsInShape < 1)
		{
			vertsInShape = 1;
		}
		beginningConnectionID = beginningSpline.points.Count - 1;
		float width = beginningSpline.controlPoints[beginningSpline.controlPoints.Count - 1].w;
		width *= beginningMaxWidth - beginningMinWidth;
		Vector4 pos = Vector3.Lerp(beginningSpline.pointsDown[beginningConnectionID], beginningSpline.pointsUp[beginningConnectionID], beginningMinWidth + (beginningMaxWidth - beginningMinWidth) * 0.5f) + beginningSpline.transform.position - base.transform.position;
		pos.w = width;
		controlPoints[0] = pos;
		if (!uvScaleOverride)
		{
			uvScale = beginningSpline.uvScale;
		}
	}

	public void GenerateEndingParentBased()
	{
		if (beginningSpline == null)
		{
			vertsInShape = (int)Mathf.Round((float)(endingSpline.vertsInShape - 1) * (endingMaxWidth - endingMinWidth) + 1f);
			if (vertsInShape < 1)
			{
				vertsInShape = 1;
			}
		}
		endingConnectionID = 0;
		float width = endingSpline.controlPoints[0].w;
		width *= endingMaxWidth - endingMinWidth;
		Vector4 pos = Vector3.Lerp(endingSpline.pointsDown[endingConnectionID], endingSpline.pointsUp[endingConnectionID], endingMinWidth + (endingMaxWidth - endingMinWidth) * 0.5f) + endingSpline.transform.position - base.transform.position;
		pos.w = width;
		controlPoints[controlPoints.Count - 1] = pos;
	}

	public void GenerateSpline(List<RamSpline> generatedSplines = null)
	{
		if (generatedSplines == null)
		{
			generatedSplines = new List<RamSpline>();
		}
		if (beginningSpline != null && beginningSpline.endingSpline != null)
		{
			Debug.LogError("River can't be ending spline and have beginning spline");
			return;
		}
		if (endingSpline != null && endingSpline.beginningSpline != null)
		{
			Debug.LogError("River can't be begining spline and have ending spline");
			return;
		}
		if ((bool)beginningSpline)
		{
			GenerateBeginningParentBased();
		}
		if ((bool)endingSpline)
		{
			GenerateEndingParentBased();
		}
		List<Vector4> pointsChecked = new List<Vector4>();
		for (int i = 0; i < controlPoints.Count; i++)
		{
			if (i > 0)
			{
				if (Vector3.Distance(controlPoints[i], controlPoints[i - 1]) > 0f)
				{
					pointsChecked.Add(controlPoints[i]);
				}
			}
			else
			{
				pointsChecked.Add(controlPoints[i]);
			}
		}
		Mesh mesh = new Mesh();
		meshfilter = GetComponent<MeshFilter>();
		if (pointsChecked.Count < 2)
		{
			mesh.Clear();
			meshfilter.mesh = mesh;
			return;
		}
		controlPointsOrientation = new List<Quaternion>();
		lerpValues.Clear();
		snaps.Clear();
		points.Clear();
		pointsUp.Clear();
		pointsDown.Clear();
		carvePointsUp.Clear();
		carvePointsDown.Clear();
		orientations.Clear();
		tangents.Clear();
		normalsList.Clear();
		widths.Clear();
		controlPointsUp.Clear();
		controlPointsDown.Clear();
		verticesBeginning.Clear();
		verticesEnding.Clear();
		normalsBeginning.Clear();
		normalsEnding.Clear();
		if (beginningSpline != null && beginningSpline.controlPointsRotations.Count > 0)
		{
			controlPointsRotations[0] = Quaternion.identity;
		}
		if (endingSpline != null && endingSpline.controlPointsRotations.Count > 0)
		{
			controlPointsRotations[controlPointsRotations.Count - 1] = Quaternion.identity;
		}
		for (int j = 0; j < pointsChecked.Count; j++)
		{
			if (j <= pointsChecked.Count - 2)
			{
				CalculateCatmullRomSideSplines(pointsChecked, j);
			}
		}
		if (beginningSpline != null && beginningSpline.controlPointsRotations.Count > 0)
		{
			controlPointsRotations[0] = Quaternion.Inverse(controlPointsOrientation[0]) * beginningSpline.controlPointsOrientation[beginningSpline.controlPointsOrientation.Count - 1];
		}
		if (endingSpline != null && endingSpline.controlPointsRotations.Count > 0)
		{
			controlPointsRotations[controlPointsRotations.Count - 1] = Quaternion.Inverse(controlPointsOrientation[controlPointsOrientation.Count - 1]) * endingSpline.controlPointsOrientation[0];
		}
		controlPointsOrientation = new List<Quaternion>();
		controlPointsUp.Clear();
		controlPointsDown.Clear();
		for (int k = 0; k < pointsChecked.Count; k++)
		{
			if (k <= pointsChecked.Count - 2)
			{
				CalculateCatmullRomSideSplines(pointsChecked, k);
			}
		}
		for (int l = 0; l < pointsChecked.Count; l++)
		{
			if (l <= pointsChecked.Count - 2)
			{
				CalculateCatmullRomSplineParameters(pointsChecked, l);
			}
		}
		for (int m = 0; m < controlPointsUp.Count; m++)
		{
			if (m <= controlPointsUp.Count - 2)
			{
				CalculateCatmullRomSpline(controlPointsUp, m, ref pointsUp);
			}
		}
		for (int n = 0; n < controlPointsDown.Count; n++)
		{
			if (n <= controlPointsDown.Count - 2)
			{
				CalculateCatmullRomSpline(controlPointsDown, n, ref pointsDown);
			}
		}
		traingleDensity /= terrainMeshSmoothX;
		for (int num = 0; num < controlPointsUp.Count; num++)
		{
			if (num <= controlPointsUp.Count - 2)
			{
				CalculateCatmullRomSpline(controlPointsUp, num, ref carvePointsUp);
			}
		}
		for (int num2 = 0; num2 < controlPointsDown.Count; num2++)
		{
			if (num2 <= controlPointsDown.Count - 2)
			{
				CalculateCatmullRomSpline(controlPointsDown, num2, ref carvePointsDown);
			}
		}
		traingleDensity *= terrainMeshSmoothX;
		GenerateMesh(ref mesh);
		if (generatedSplines == null)
		{
			return;
		}
		generatedSplines.Add(this);
		foreach (RamSpline item in beginnigChildSplines)
		{
			if (item != null && !generatedSplines.Contains(item) && (item.beginningSpline == this || item.endingSpline == this))
			{
				item.GenerateSpline(generatedSplines);
			}
		}
		foreach (RamSpline item2 in endingChildSplines)
		{
			if (item2 != null && !generatedSplines.Contains(item2) && (item2.beginningSpline == this || item2.endingSpline == this))
			{
				item2.GenerateSpline(generatedSplines);
			}
		}
	}

	private void CalculateCatmullRomSideSplines(List<Vector4> controlPoints, int pos)
	{
		Vector3 p0 = controlPoints[pos];
		Vector3 p1 = controlPoints[pos];
		Vector3 p2 = controlPoints[ClampListPos(pos + 1)];
		Vector3 p3 = controlPoints[ClampListPos(pos + 1)];
		if (pos > 0)
		{
			p0 = controlPoints[ClampListPos(pos - 1)];
		}
		if (pos < controlPoints.Count - 2)
		{
			p3 = controlPoints[ClampListPos(pos + 2)];
		}
		int tValueMax = 0;
		if (pos == controlPoints.Count - 2)
		{
			tValueMax = 1;
		}
		for (int tValue = 0; tValue <= tValueMax; tValue++)
		{
			Vector3 catmullRomPosition = GetCatmullRomPosition(tValue, p0, p1, p2, p3);
			Vector3 tangent = GetCatmullRomTangent(tValue, p0, p1, p2, p3).normalized;
			Vector3 normal = CalculateNormal(tangent, Vector3.up).normalized;
			Quaternion orientation = ((!(normal == tangent) || !(normal == Vector3.zero)) ? Quaternion.LookRotation(tangent, normal) : Quaternion.identity);
			orientation *= Quaternion.Lerp(controlPointsRotations[pos], controlPointsRotations[ClampListPos(pos + 1)], tValue);
			controlPointsOrientation.Add(orientation);
			Vector3 posUp = catmullRomPosition + orientation * (0.5f * controlPoints[pos + tValue].w * Vector3.right);
			Vector3 posDown = catmullRomPosition + orientation * (0.5f * controlPoints[pos + tValue].w * Vector3.left);
			controlPointsUp.Add(posUp);
			controlPointsDown.Add(posDown);
		}
	}

	private void CalculateCatmullRomSplineParameters(List<Vector4> controlPoints, int pos, bool initialPoints = false)
	{
		Vector3 p0 = controlPoints[pos];
		Vector3 p1 = controlPoints[pos];
		Vector3 p2 = controlPoints[ClampListPos(pos + 1)];
		Vector3 p3 = controlPoints[ClampListPos(pos + 1)];
		if (pos > 0)
		{
			p0 = controlPoints[ClampListPos(pos - 1)];
		}
		if (pos < controlPoints.Count - 2)
		{
			p3 = controlPoints[ClampListPos(pos + 2)];
		}
		int loops = Mathf.FloorToInt(1f / traingleDensity);
		float i = 1f;
		float start = 0f;
		if (pos > 0)
		{
			start = 1f;
		}
		for (i = start; i <= (float)loops; i += 1f)
		{
			float t = i * traingleDensity;
			CalculatePointParameters(controlPoints, pos, p0, p1, p2, p3, t);
		}
		if (i < (float)loops)
		{
			i = loops;
			float t2 = i * traingleDensity;
			CalculatePointParameters(controlPoints, pos, p0, p1, p2, p3, t2);
		}
	}

	private void CalculateCatmullRomSpline(List<Vector3> controlPoints, int pos, ref List<Vector3> points)
	{
		Vector3 p0 = controlPoints[pos];
		Vector3 p1 = controlPoints[pos];
		Vector3 p2 = controlPoints[ClampListPos(pos + 1)];
		Vector3 p3 = controlPoints[ClampListPos(pos + 1)];
		if (pos > 0)
		{
			p0 = controlPoints[ClampListPos(pos - 1)];
		}
		if (pos < controlPoints.Count - 2)
		{
			p3 = controlPoints[ClampListPos(pos + 2)];
		}
		int loops = Mathf.FloorToInt(1f / traingleDensity);
		float i = 1f;
		float start = 0f;
		if (pos > 0)
		{
			start = 1f;
		}
		for (i = start; i <= (float)loops; i += 1f)
		{
			float t = i * traingleDensity;
			CalculatePointPosition(controlPoints, pos, p0, p1, p2, p3, t, ref points);
		}
		if (i < (float)loops)
		{
			i = loops;
			float t2 = i * traingleDensity;
			CalculatePointPosition(controlPoints, pos, p0, p1, p2, p3, t2, ref points);
		}
	}

	private void CalculatePointPosition(List<Vector3> controlPoints, int pos, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, ref List<Vector3> points)
	{
		Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
		points.Add(newPos);
		Vector3 tangent = GetCatmullRomTangent(t, p0, p1, p2, p3).normalized;
		_ = CalculateNormal(tangent, Vector3.up).normalized;
	}

	private void CalculatePointParameters(List<Vector4> controlPoints, int pos, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
		widths.Add(Mathf.Lerp(controlPoints[pos].w, controlPoints[ClampListPos(pos + 1)].w, t));
		if (controlPointsSnap.Count > pos + 1)
		{
			snaps.Add(Mathf.Lerp(controlPointsSnap[pos], controlPointsSnap[ClampListPos(pos + 1)], t));
		}
		else
		{
			snaps.Add(0f);
		}
		lerpValues.Add((float)pos + t);
		points.Add(newPos);
		Vector3 tangent = GetCatmullRomTangent(t, p0, p1, p2, p3).normalized;
		Vector3 normal = CalculateNormal(tangent, Vector3.up).normalized;
		Quaternion orientation = ((!(normal == tangent) || !(normal == Vector3.zero)) ? Quaternion.LookRotation(tangent, normal) : Quaternion.identity);
		orientation *= Quaternion.Lerp(controlPointsRotations[pos], controlPointsRotations[ClampListPos(pos + 1)], t);
		orientations.Add(orientation);
		tangents.Add(tangent);
		if (normalsList.Count > 0 && Vector3.Angle(normalsList[normalsList.Count - 1], normal) > 90f)
		{
			normal *= -1f;
		}
		normalsList.Add(normal);
	}

	private int ClampListPos(int pos)
	{
		if (pos < 0)
		{
			pos = controlPoints.Count - 1;
		}
		if (pos > controlPoints.Count)
		{
			pos = 1;
		}
		else if (pos > controlPoints.Count - 1)
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

	private Vector3 GetCatmullRomTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		return 0.5f * (-p0 + p2 + 2f * (2f * p0 - 5f * p1 + 4f * p2 - p3) * t + 3f * (-p0 + 3f * p1 - 3f * p2 + p3) * t * t);
	}

	private Vector3 CalculateNormal(Vector3 tangent, Vector3 up)
	{
		Vector3 binormal = Vector3.Cross(up, tangent);
		return Vector3.Cross(tangent, binormal);
	}

	private void GenerateMesh(ref Mesh mesh)
	{
		terrainsUnder.Clear();
		MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer != null)
		{
			meshRenderer.receiveShadows = receiveShadows;
			meshRenderer.shadowCastingMode = shadowCastingMode;
		}
		foreach (Transform item in meshesPartTransforms)
		{
			if (item != null)
			{
				if (Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(item.gameObject);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(item.gameObject);
				}
				global::UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		int segments = points.Count - 1;
		int edgeLoops = points.Count;
		int vertCount = vertsInShape * edgeLoops;
		List<int> triangleIndices = new List<int>();
		Vector3[] vertices = new Vector3[vertCount];
		Vector3[] normals = new Vector3[vertCount];
		Vector2[] uvs = new Vector2[vertCount];
		Vector2[] uvs3 = new Vector2[vertCount];
		Vector2[] uvs4 = new Vector2[vertCount];
		if (colors == null || colors.Length != vertCount)
		{
			colors = new Color[vertCount];
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = Color.black;
			}
		}
		if (colorsFlowMap.Count != vertCount)
		{
			colorsFlowMap.Clear();
		}
		length = 0f;
		fulllength = 0f;
		if (beginningSpline != null)
		{
			length = beginningSpline.length;
		}
		minMaxWidth = 1f;
		uvWidth = 1f;
		uvBeginning = 0f;
		if (beginningSpline != null)
		{
			minMaxWidth = beginningMaxWidth - beginningMinWidth;
			uvWidth = minMaxWidth * beginningSpline.uvWidth;
			uvBeginning = beginningSpline.uvWidth * beginningMinWidth + beginningSpline.uvBeginning;
		}
		else if (endingSpline != null)
		{
			minMaxWidth = endingMaxWidth - endingMinWidth;
			uvWidth = minMaxWidth * endingSpline.uvWidth;
			uvBeginning = endingSpline.uvWidth * endingMinWidth + endingSpline.uvBeginning;
		}
		for (int j = 0; j < pointsDown.Count; j++)
		{
			float width = widths[j];
			if (j > 0)
			{
				fulllength += uvWidth * Vector3.Distance(pointsDown[j], pointsDown[j - 1]) / (uvScale * width);
			}
		}
		Terrain checkTerrain = null;
		float roundEnding = Mathf.Round(fulllength);
		for (int k = 0; k < pointsDown.Count; k++)
		{
			float width2 = widths[k];
			int offset = k * vertsInShape;
			if (k > 0)
			{
				length += uvWidth * Vector3.Distance(pointsDown[k], pointsDown[k - 1]) / (uvScale * width2) / fulllength * roundEnding;
			}
			float u = 0f;
			float u3 = 0f;
			for (int l = 0; l < vertsInShape; l++)
			{
				int id = offset + l;
				float pos = (float)l / (float)(vertsInShape - 1);
				pos = ((!(pos < 0.5f)) ? (((pos - 0.5f) * (1f - maxVal) + 0.5f * maxVal) * 2f) : (pos * (minVal * 2f)));
				if (k == 0 && beginningSpline != null && beginningSpline.verticesEnding != null && beginningSpline.normalsEnding != null)
				{
					int pos2 = (int)((float)beginningSpline.vertsInShape * beginningMinWidth);
					vertices[id] = beginningSpline.verticesEnding[Mathf.Clamp(l + pos2, 0, beginningSpline.verticesEnding.Count - 1)] + beginningSpline.transform.position - base.transform.position;
				}
				else if (k == pointsDown.Count - 1 && endingSpline != null && endingSpline.verticesBeginning != null && endingSpline.verticesBeginning.Count > 0 && endingSpline.normalsBeginning != null)
				{
					int pos3 = (int)((float)endingSpline.vertsInShape * endingMinWidth);
					vertices[id] = endingSpline.verticesBeginning[Mathf.Clamp(l + pos3, 0, endingSpline.verticesBeginning.Count - 1)] + endingSpline.transform.position - base.transform.position;
				}
				else
				{
					vertices[id] = Vector3.Lerp(pointsDown[k], pointsUp[k], pos);
					if (Physics.Raycast(vertices[id] + base.transform.position + Vector3.up * 5f, Vector3.down, out var hit, 1000f, snapMask.value))
					{
						checkTerrain = hit.collider.gameObject.GetComponent<Terrain>();
						if ((bool)checkTerrain && !terrainsUnder.Contains(checkTerrain))
						{
							terrainsUnder.Add(checkTerrain);
						}
						vertices[id] = Vector3.Lerp(vertices[id], hit.point - base.transform.position + new Vector3(0f, 0.1f, 0f), (Mathf.Sin(MathF.PI * snaps[k] - MathF.PI / 2f) + 1f) * 0.5f);
					}
					if (normalFromRaycast && Physics.Raycast(points[k] + base.transform.position + Vector3.up * 5f, Vector3.down, out var hit2, 1000f, snapMask.value))
					{
						normals[id] = hit2.normal;
					}
					vertices[id] += orientations[k] * Vector3.up * Mathf.Lerp(controlPointsMeshCurves[Mathf.FloorToInt(lerpValues[k])].Evaluate(pos), controlPointsMeshCurves[Mathf.CeilToInt(lerpValues[k])].Evaluate(pos), lerpValues[k] - Mathf.Floor(lerpValues[k]));
				}
				if (k > 0 && k < 5 && beginningSpline != null && beginningSpline.verticesEnding != null)
				{
					vertices[id].y = (vertices[id].y + vertices[id - vertsInShape].y) * 0.5f;
				}
				if (k == pointsDown.Count - 1 && endingSpline != null && endingSpline.verticesBeginning != null)
				{
					for (int m = 1; m < 5; m++)
					{
						vertices[id - vertsInShape * m].y = (vertices[id - vertsInShape * (m - 1)].y + vertices[id - vertsInShape * m].y) * 0.5f;
					}
				}
				if (k == 0)
				{
					verticesBeginning.Add(vertices[id]);
				}
				if (k == pointsDown.Count - 1)
				{
					verticesEnding.Add(vertices[id]);
				}
				if (!normalFromRaycast)
				{
					normals[id] = orientations[k] * Vector3.up;
				}
				if (k == 0)
				{
					normalsBeginning.Add(normals[id]);
				}
				if (k == pointsDown.Count - 1)
				{
					normalsEnding.Add(normals[id]);
				}
				if (l > 0)
				{
					u = pos * uvWidth;
					u3 = pos;
				}
				if (beginningSpline != null || endingSpline != null)
				{
					u += uvBeginning;
				}
				u /= uvScale;
				float uv4u = FlowCalculate(u3, normals[id].y, vertices[id]);
				int lerpDistance = 10;
				if (beginnigChildSplines.Count > 0 && k <= lerpDistance)
				{
					float lerpUv4u = 0f;
					foreach (RamSpline item2 in beginnigChildSplines)
					{
						if (!(item2 == null) && Mathf.CeilToInt(item2.endingMaxWidth * (float)(vertsInShape - 1)) >= l && l >= Mathf.CeilToInt(item2.endingMinWidth * (float)(vertsInShape - 1)))
						{
							lerpUv4u = (float)(l - Mathf.CeilToInt(item2.endingMinWidth * (float)(vertsInShape - 1))) / (float)(Mathf.CeilToInt(item2.endingMaxWidth * (float)(vertsInShape - 1)) - Mathf.CeilToInt(item2.endingMinWidth * (float)(vertsInShape - 1)));
							lerpUv4u = FlowCalculate(lerpUv4u, normals[id].y, vertices[id]);
						}
					}
					uv4u = ((k <= 0) ? lerpUv4u : Mathf.Lerp(uv4u, lerpUv4u, 1f - (float)k / (float)lerpDistance));
				}
				if (k >= pointsDown.Count - lerpDistance - 1 && endingChildSplines.Count > 0)
				{
					float lerpUv4u2 = 0f;
					foreach (RamSpline item3 in endingChildSplines)
					{
						if (!(item3 == null) && Mathf.CeilToInt(item3.beginningMaxWidth * (float)(vertsInShape - 1)) >= l && l >= Mathf.CeilToInt(item3.beginningMinWidth * (float)(vertsInShape - 1)))
						{
							lerpUv4u2 = (float)(l - Mathf.CeilToInt(item3.beginningMinWidth * (float)(vertsInShape - 1))) / (float)(Mathf.CeilToInt(item3.beginningMaxWidth * (float)(vertsInShape - 1)) - Mathf.CeilToInt(item3.beginningMinWidth * (float)(vertsInShape - 1)));
							lerpUv4u2 = FlowCalculate(lerpUv4u2, normals[id].y, vertices[id]);
						}
					}
					uv4u = ((k >= pointsDown.Count - 1) ? lerpUv4u2 : Mathf.Lerp(uv4u, lerpUv4u2, (float)(k - (pointsDown.Count - lerpDistance - 1)) / (float)lerpDistance));
				}
				float uv4v = (0f - (u3 - 0.5f)) * 0.01f;
				uv3length = length / fulllength;
				if (beginningSpline != null)
				{
					uv3length = (length - beginningSpline.length) / fulllength + beginningSpline.uv3length;
				}
				foreach (RamSpline beginningSplines in beginnigChildSplines)
				{
					if (!(beginningSplines == null))
					{
						uv3length = length / fulllength + beginningSplines.uv3length;
						break;
					}
				}
				if (uvRotation)
				{
					if (!invertUVDirection)
					{
						uvs[id] = new Vector2(1f - length, u);
						uvs3[id] = new Vector2(1f - uv3length, u3);
						uvs4[id] = new Vector2(uv4u, uv4v);
					}
					else
					{
						uvs[id] = new Vector2(1f + length, u);
						uvs3[id] = new Vector2(1f + uv3length, u3);
						uvs4[id] = new Vector2(uv4u, uv4v);
					}
				}
				else if (!invertUVDirection)
				{
					uvs[id] = new Vector2(u, 1f - length);
					uvs3[id] = new Vector2(u3, 1f - uv3length);
					uvs4[id] = new Vector2(uv4v, uv4u);
				}
				else
				{
					uvs[id] = new Vector2(u, 1f + length);
					uvs3[id] = new Vector2(u3, 1f + uv3length);
					uvs4[id] = new Vector2(uv4v, uv4u);
				}
				float tempRound = (int)(uvs4[id].x * 100f);
				uvs4[id].x = tempRound * 0.01f;
				tempRound = (int)(uvs4[id].y * 100f);
				uvs4[id].y = tempRound * 0.01f;
				if (colorsFlowMap.Count <= id)
				{
					colorsFlowMap.Add(uvs4[id]);
				}
				else if (!overrideFlowMap)
				{
					colorsFlowMap[id] = uvs4[id];
				}
			}
		}
		for (int n = 0; n < segments; n++)
		{
			int offset2 = n * vertsInShape;
			for (int num = 0; num < vertsInShape - 1; num++)
			{
				int a = offset2 + num;
				int b = offset2 + num + vertsInShape;
				int c = offset2 + num + 1 + vertsInShape;
				int d = offset2 + num + 1;
				triangleIndices.Add(a);
				triangleIndices.Add(b);
				triangleIndices.Add(c);
				triangleIndices.Add(c);
				triangleIndices.Add(d);
				triangleIndices.Add(a);
			}
		}
		verticeDirection.Clear();
		for (int num2 = 0; num2 < vertices.Length - vertsInShape; num2++)
		{
			Vector3 dir = (vertices[num2 + vertsInShape] - vertices[num2]).normalized;
			if (uvRotation)
			{
				dir = new Vector3(dir.z, 0f, 0f - dir.x);
			}
			verticeDirection.Add(dir);
		}
		for (int num3 = vertices.Length - vertsInShape; num3 < vertices.Length; num3++)
		{
			Vector3 dir2 = (vertices[num3] - vertices[num3 - vertsInShape]).normalized;
			if (uvRotation)
			{
				dir2 = new Vector3(dir2.z, 0f, 0f - dir2.x);
			}
			verticeDirection.Add(dir2);
		}
		mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.uv3 = uvs3;
		mesh.uv4 = colorsFlowMap.ToArray();
		mesh.triangles = triangleIndices.ToArray();
		mesh.colors = colors;
		mesh.RecalculateTangents();
		meshfilter.mesh = mesh;
		GetComponent<MeshRenderer>().enabled = true;
		if (generateMeshParts)
		{
			GenerateMeshParts(mesh);
		}
	}

	public void GenerateMeshParts(Mesh baseMesh)
	{
		foreach (Transform item in meshesPartTransforms)
		{
			if (item != null)
			{
				global::UnityEngine.Object.DestroyImmediate(item.gameObject);
			}
		}
		Vector3[] vertices = baseMesh.vertices;
		Vector3[] normals = baseMesh.normals;
		Vector2[] uvs = baseMesh.uv;
		Vector2[] uvs3 = baseMesh.uv3;
		GetComponent<MeshRenderer>().enabled = false;
		int verticesInPart = Mathf.RoundToInt((float)(vertices.Length / vertsInShape) / (float)meshPartsCount) * vertsInShape;
		for (int i = 0; i < meshPartsCount; i++)
		{
			GameObject go = new GameObject(base.gameObject.name + "- Mesh part " + i);
			go.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			meshesPartTransforms.Add(go.transform);
			MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
			meshRenderer.receiveShadows = receiveShadows;
			meshRenderer.shadowCastingMode = shadowCastingMode;
			MeshFilter mf = go.AddComponent<MeshFilter>();
			Mesh meshPart = new Mesh();
			meshPart.Clear();
			List<Vector3> verticesPart = new List<Vector3>();
			List<Vector3> normalsPart = new List<Vector3>();
			List<Vector2> uvPart = new List<Vector2>();
			List<Vector2> uv3Part = new List<Vector2>();
			List<Vector2> uv4Part = new List<Vector2>();
			List<Color> colorsPart = new List<Color>();
			List<int> trianglesPart = new List<int>();
			for (int j = verticesInPart * i + ((i > 0) ? (-vertsInShape) : 0); (j < verticesInPart * (i + 1) && j < vertices.Length) || (i == meshPartsCount - 1 && j < vertices.Length); j++)
			{
				verticesPart.Add(vertices[j]);
				normalsPart.Add(normals[j]);
				uvPart.Add(uvs[j]);
				uv3Part.Add(uvs3[j]);
				uv4Part.Add(colorsFlowMap[j]);
				colorsPart.Add(colors[j]);
			}
			if (verticesPart.Count <= 0)
			{
				continue;
			}
			Vector3 pivotChange = verticesPart[0];
			for (int k = 0; k < verticesPart.Count; k++)
			{
				verticesPart[k] -= pivotChange;
			}
			for (int l = 0; l < verticesPart.Count / vertsInShape - 1; l++)
			{
				int offset = l * vertsInShape;
				for (int m = 0; m < vertsInShape - 1; m++)
				{
					int a = offset + m;
					int b = offset + m + vertsInShape;
					int c = offset + m + 1 + vertsInShape;
					int d = offset + m + 1;
					trianglesPart.Add(a);
					trianglesPart.Add(b);
					trianglesPart.Add(c);
					trianglesPart.Add(c);
					trianglesPart.Add(d);
					trianglesPart.Add(a);
				}
			}
			go.transform.position += pivotChange;
			meshPart.vertices = verticesPart.ToArray();
			meshPart.triangles = trianglesPart.ToArray();
			meshPart.normals = normalsPart.ToArray();
			meshPart.uv = uvPart.ToArray();
			meshPart.uv3 = uv3Part.ToArray();
			meshPart.uv4 = uv4Part.ToArray();
			meshPart.colors = colorsPart.ToArray();
			meshPart.RecalculateTangents();
			mf.mesh = meshPart;
		}
	}

	public void AddNoiseToWidths()
	{
		for (int i = 0; i < controlPoints.Count; i++)
		{
			Vector4 controlPoint = controlPoints[i];
			controlPoint.w += (noiseWidth ? (noiseMultiplierWidth * (Mathf.PerlinNoise(noiseSizeWidth * (float)i, 0f) - 0.5f)) : 0f);
			if (controlPoint.w < 0f)
			{
				controlPoint.w = 0f;
			}
			controlPoints[i] = controlPoint;
		}
	}

	public void SimulateRiver(bool generate = true)
	{
		if (meshGO != null)
		{
			if (Application.isEditor)
			{
				global::UnityEngine.Object.DestroyImmediate(meshGO);
			}
			else
			{
				global::UnityEngine.Object.Destroy(meshGO);
			}
		}
		if (controlPoints.Count == 0)
		{
			Debug.Log("Add one point to start Simulating River");
			return;
		}
		Ray ray = default(Ray);
		Vector3 lastPosition = base.transform.TransformPoint(controlPoints[controlPoints.Count - 1]);
		List<Vector3> positionsGenerated = new List<Vector3>();
		if (controlPoints.Count > 1)
		{
			positionsGenerated.Add(base.transform.TransformPoint(controlPoints[controlPoints.Count - 2]));
			positionsGenerated.Add(lastPosition);
		}
		List<Vector3> samplePositionsGenerated = new List<Vector3>();
		samplePositionsGenerated.Add(lastPosition);
		float length = 0f;
		int i = -1;
		int added = 0;
		bool end = false;
		float widthNew = 0f;
		widthNew = ((controlPoints.Count <= 0) ? width : controlPoints[controlPoints.Count - 1].w);
		do
		{
			i++;
			if (i <= 0)
			{
				continue;
			}
			Vector3 maxPosition = Vector3.zero;
			float max = float.MinValue;
			bool foundNextPositon = false;
			for (float j = simulatedMinStepSize; j < 10f; j += 0.1f)
			{
				for (int angle = 0; angle < 36; angle++)
				{
					float x = j * Mathf.Cos(angle);
					float z = j * Mathf.Sin(angle);
					ray.origin = lastPosition + new Vector3(0f, 1000f, 0f) + new Vector3(x, 0f, z);
					ray.direction = Vector3.down;
					if (!Physics.Raycast(ray, out var hit, 10000f) || !(hit.distance > max))
					{
						continue;
					}
					bool goodPoint = true;
					foreach (Vector3 item in positionsGenerated)
					{
						if (Vector3.Distance(item, lastPosition) > Vector3.Distance(item, hit.point) + 0.5f)
						{
							goodPoint = false;
							break;
						}
					}
					if (goodPoint)
					{
						foundNextPositon = true;
						max = hit.distance;
						maxPosition = hit.point;
					}
				}
				if (foundNextPositon)
				{
					break;
				}
			}
			if (!foundNextPositon)
			{
				break;
			}
			if (maxPosition.y > lastPosition.y)
			{
				if (simulatedNoUp)
				{
					maxPosition.y = lastPosition.y;
				}
				if (simulatedBreakOnUp)
				{
					end = true;
				}
			}
			length += Vector3.Distance(maxPosition, lastPosition);
			if (i % simulatedRiverPoints == 0 || simulatedRiverLength <= length || end)
			{
				samplePositionsGenerated.Add(maxPosition);
				if (generate)
				{
					added++;
					Vector4 newPosition = maxPosition - base.transform.position;
					newPosition.w = widthNew + (noiseWidth ? (noiseMultiplierWidth * (Mathf.PerlinNoise(noiseSizeWidth * (float)added, 0f) - 0.5f)) : 0f);
					controlPointsRotations.Add(Quaternion.identity);
					controlPoints.Add(newPosition);
					controlPointsSnap.Add(0f);
					controlPointsMeshCurves.Add(new AnimationCurve(meshCurve.keys));
				}
			}
			positionsGenerated.Add(lastPosition);
			lastPosition = maxPosition;
		}
		while (simulatedRiverLength > length && !end);
		if (generate)
		{
			return;
		}
		widthNew = ((controlPoints.Count <= 0) ? width : controlPoints[controlPoints.Count - 1].w);
		float widthNoise = 0f;
		List<List<Vector4>> positionArray = new List<List<Vector4>>();
		Vector3 v1 = default(Vector3);
		for (i = 0; i < samplePositionsGenerated.Count - 1; i++)
		{
			widthNoise = widthNew + (noiseWidth ? (noiseMultiplierWidth * (Mathf.PerlinNoise(noiseSizeWidth * (float)i, 0f) - 0.5f)) : 0f);
			v1 = Vector3.Cross(samplePositionsGenerated[i + 1] - samplePositionsGenerated[i], Vector3.up).normalized;
			if (i > 0)
			{
				Vector3 v2 = Vector3.Cross(samplePositionsGenerated[i] - samplePositionsGenerated[i - 1], Vector3.up).normalized;
				v1 = (v1 + v2).normalized;
			}
			List<Vector4> positionRow = new List<Vector4>();
			positionRow.Add(samplePositionsGenerated[i] + v1 * widthNoise * 0.5f);
			positionRow.Add(samplePositionsGenerated[i] - v1 * widthNoise * 0.5f);
			positionArray.Add(positionRow);
		}
		widthNoise = widthNew + (noiseWidth ? (noiseMultiplierWidth * (Mathf.PerlinNoise(noiseSizeWidth * (float)i, 0f) - 0.5f)) : 0f);
		List<Vector4> positionRowLast = new List<Vector4>();
		positionRowLast.Add(samplePositionsGenerated[i] + v1 * widthNoise * 0.5f);
		positionRowLast.Add(samplePositionsGenerated[i] - v1 * widthNoise * 0.5f);
		positionArray.Add(positionRowLast);
		Mesh meshTerrain = new Mesh();
		meshTerrain.indexFormat = IndexFormat.UInt32;
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		foreach (List<Vector4> item2 in positionArray)
		{
			foreach (Vector4 vert in item2)
			{
				vertices.Add(vert);
			}
		}
		for (i = 0; i < positionArray.Count - 1; i++)
		{
			int count = positionArray[i].Count;
			for (int k = 0; k < count - 1; k++)
			{
				triangles.Add(k + i * count);
				triangles.Add(k + (i + 1) * count);
				triangles.Add(k + 1 + i * count);
				triangles.Add(k + 1 + i * count);
				triangles.Add(k + (i + 1) * count);
				triangles.Add(k + 1 + (i + 1) * count);
			}
		}
		meshTerrain.SetVertices(vertices);
		meshTerrain.SetTriangles(triangles, 0);
		meshTerrain.RecalculateNormals();
		meshTerrain.RecalculateTangents();
		meshTerrain.RecalculateBounds();
		meshGO = new GameObject("TerrainMesh");
		meshGO.hideFlags = HideFlags.HideAndDontSave;
		meshGO.AddComponent<MeshFilter>();
		meshGO.transform.parent = base.transform;
		MeshRenderer meshRenderer = meshGO.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Debug Terrain Carve"));
		meshRenderer.sharedMaterial.color = new Color(0f, 0.5f, 0f);
		meshGO.transform.position = Vector3.zero;
		meshGO.GetComponent<MeshFilter>().sharedMesh = meshTerrain;
	}

	public void ShowTerrainCarve(float differentSize = 0f)
	{
		if (Application.isEditor && meshGO == null)
		{
			Transform meshGoTrans = base.transform.Find("TerrainMesh");
			if (meshGoTrans != null)
			{
				meshGO = meshGoTrans.gameObject;
			}
		}
		if (meshGO != null)
		{
			if (Application.isEditor)
			{
				global::UnityEngine.Object.DestroyImmediate(meshGO);
			}
			else
			{
				global::UnityEngine.Object.Destroy(meshGO);
			}
		}
		_ = meshfilter.sharedMesh;
		int detailTerrain = (int)terrainMeshSmoothZ;
		if (differentSize == 0f)
		{
			terrainAdditionalWidth = distSmooth + distSmoothStart;
		}
		else
		{
			terrainAdditionalWidth = differentSize;
		}
		List<List<Vector4>> positionArray = new List<List<Vector4>>();
		float noise = 0f;
		for (int i = 0; i < carvePointsDown.Count - 1; i++)
		{
			List<Vector4> positionArrayRow = new List<Vector4>();
			Vector3 rayPointDown = carvePointsDown[i];
			Vector3 rayPointUp = carvePointsUp[i];
			Vector3 diff = rayPointDown - rayPointUp;
			float diffMagintude = diff.magnitude;
			rayPointDown += diff * 0.05f;
			rayPointUp -= diff * 0.05f;
			diff.Normalize();
			Vector3 rayPointDownNew = rayPointDown + diff * terrainAdditionalWidth * 0.5f;
			Vector3 rayPointUpNew = rayPointUp - diff * terrainAdditionalWidth * 0.5f;
			RaycastHit hit;
			if (terrainAdditionalWidth > 0f)
			{
				for (int t = 0; t < detailTerrain; t++)
				{
					Vector3 point = Vector3.Lerp(rayPointDownNew, rayPointDown, (float)t / (float)detailTerrain) + base.transform.position;
					if (Physics.Raycast(point + Vector3.up * 500f, Vector3.down, out hit, 10000f, maskCarve.value))
					{
						noise = ((!noiseCarve) ? 0f : (Mathf.PerlinNoise(point.x * noiseSizeX, point.z * noiseSizeZ) * noiseMultiplierOutside - noiseMultiplierOutside * 0.5f));
						float evaluate = 1f - (float)t / (float)detailTerrain;
						evaluate *= terrainAdditionalWidth;
						float height = point.y + terrainCarve.Evaluate(0f - evaluate) + terrainCarve.Evaluate(0f - evaluate) * noise;
						float smoothValue = (float)t / (float)detailTerrain;
						smoothValue = Mathf.Pow(smoothValue, terrainSmoothMultiplier);
						height = Mathf.Lerp(hit.point.y, height, smoothValue);
						Vector4 newPos = new Vector4(hit.point.x, height, hit.point.z, 0f - evaluate);
						positionArrayRow.Add(newPos);
					}
					else
					{
						positionArrayRow.Add(point);
					}
				}
			}
			for (int j = 0; j <= detailTerrain; j++)
			{
				Vector3 point = Vector3.Lerp(rayPointDown, rayPointUp, (float)j / (float)detailTerrain) + base.transform.position;
				if (Physics.Raycast(point + Vector3.up * 500f, Vector3.down, out hit, 10000f, maskCarve.value))
				{
					noise = ((!noiseCarve) ? 0f : (Mathf.PerlinNoise(point.x * noiseSizeX, point.z * noiseSizeZ) * noiseMultiplierInside - noiseMultiplierInside * 0.5f));
					float evaluate2 = diffMagintude * (0.5f - Mathf.Abs(0.5f - (float)j / (float)detailTerrain));
					float height2 = point.y + terrainCarve.Evaluate(evaluate2) + terrainCarve.Evaluate(evaluate2) * noise;
					Mathf.Pow(1f - 2f * Mathf.Abs((float)j / (float)detailTerrain - 0.5f), terrainSmoothMultiplier);
					height2 = Mathf.Lerp(hit.point.y, height2, 1f);
					Vector4 newPos2 = new Vector4(hit.point.x, height2, hit.point.z, evaluate2);
					positionArrayRow.Add(newPos2);
				}
				else
				{
					positionArrayRow.Add(point);
				}
			}
			if (terrainAdditionalWidth > 0f)
			{
				for (int k = 1; k <= detailTerrain; k++)
				{
					Vector3 point = Vector3.Lerp(rayPointUp, rayPointUpNew, (float)k / (float)detailTerrain) + base.transform.position;
					if (Physics.Raycast(point + Vector3.up * 50f, Vector3.down, out hit, 10000f, maskCarve.value))
					{
						noise = ((!noiseCarve) ? 0f : (Mathf.PerlinNoise(point.x * noiseSizeX, point.z * noiseSizeZ) * noiseMultiplierOutside - noiseMultiplierOutside * 0.5f));
						float evaluate3 = (float)k / (float)detailTerrain;
						evaluate3 *= terrainAdditionalWidth;
						float height3 = point.y + terrainCarve.Evaluate(0f - evaluate3) + terrainCarve.Evaluate(0f - evaluate3) * noise;
						float smoothValue2 = 1f - (float)k / (float)detailTerrain;
						smoothValue2 = Mathf.Pow(smoothValue2, terrainSmoothMultiplier);
						height3 = Mathf.Lerp(hit.point.y, height3, smoothValue2);
						Vector4 newPos3 = new Vector4(hit.point.x, height3, hit.point.z, 0f - evaluate3);
						positionArrayRow.Add(newPos3);
					}
					else
					{
						positionArrayRow.Add(point);
					}
				}
			}
			positionArray.Add(positionArrayRow);
		}
		Mesh meshTerrain = new Mesh();
		meshTerrain.indexFormat = IndexFormat.UInt32;
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uv = new List<Vector2>();
		foreach (List<Vector4> item in positionArray)
		{
			foreach (Vector4 vert in item)
			{
				vertices.Add(vert);
			}
		}
		for (int l = 0; l < positionArray.Count - 1; l++)
		{
			int count = positionArray[l].Count;
			for (int m = 0; m < count - 1; m++)
			{
				triangles.Add(m + l * count);
				triangles.Add(m + (l + 1) * count);
				triangles.Add(m + 1 + l * count);
				triangles.Add(m + 1 + l * count);
				triangles.Add(m + (l + 1) * count);
				triangles.Add(m + 1 + (l + 1) * count);
			}
		}
		foreach (List<Vector4> item2 in positionArray)
		{
			foreach (Vector4 item3 in item2)
			{
				uv.Add(new Vector2(item3.w, 0f));
			}
		}
		meshTerrain.SetVertices(vertices);
		meshTerrain.SetTriangles(triangles, 0);
		meshTerrain.SetUVs(0, uv);
		meshTerrain.RecalculateNormals();
		meshTerrain.RecalculateTangents();
		meshTerrain.RecalculateBounds();
		meshGO = new GameObject("TerrainMesh");
		meshGO.transform.parent = base.transform;
		meshGO.hideFlags = HideFlags.HideAndDontSave;
		meshGO.AddComponent<MeshFilter>();
		meshGO.transform.parent = base.transform;
		MeshRenderer meshRenderer = meshGO.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Debug Terrain Carve"));
		meshRenderer.sharedMaterial.color = new Color(0f, 0.5f, 0f);
		meshGO.transform.position = Vector3.zero;
		meshGO.GetComponent<MeshFilter>().sharedMesh = meshTerrain;
		if (overrideRiverRender)
		{
			meshGO.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 5000;
		}
		else
		{
			meshGO.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 2980;
		}
	}

	public void TerrainCarve(Terrain singleTerrain = null)
	{
		bool debugLines = false;
		bool savedAutoSyncTransforms = Physics.autoSyncTransforms;
		Physics.autoSyncTransforms = false;
		Terrain[] activeTerrains = Terrain.activeTerrains;
		foreach (Terrain terrain in activeTerrains)
		{
			if (singleTerrain != null && terrain != singleTerrain)
			{
				continue;
			}
			TerrainData terrainData = terrain.terrainData;
			float posY = terrain.transform.position.y;
			float sizeX = terrain.terrainData.size.x;
			float sizeY = terrain.terrainData.size.y;
			float sizeZ = terrain.terrainData.size.z;
			float terrainTowidth = 1f / sizeZ * (float)(terrainData.heightmapResolution - 1);
			float terrainToheight = 1f / sizeX * (float)(terrainData.heightmapResolution - 1);
			MeshCollider meshCollider = meshGO.gameObject.AddComponent<MeshCollider>();
			List<Vector3> transformPointUp = new List<Vector3>();
			List<Vector3> transformPointDown = new List<Vector3>();
			int pointsCount = 5;
			int pointsStart = 0;
			_ = Vector3.zero;
			_ = Vector3.zero;
			for (pointsStart = 0; pointsStart < pointsUp.Count; pointsStart = Mathf.Clamp(pointsStart + pointsCount - 1, 0, pointsUp.Count))
			{
				int end = Mathf.Min(pointsStart + pointsCount, pointsUp.Count);
				transformPointUp.Clear();
				transformPointDown.Clear();
				for (int j = pointsStart; j < end; j++)
				{
					transformPointUp.Add(base.transform.TransformPoint(pointsUp[j]));
					transformPointDown.Add(base.transform.TransformPoint(pointsDown[j]));
				}
				float minX = float.MaxValue;
				float maxX = float.MinValue;
				float minZ = float.MaxValue;
				float maxZ = float.MinValue;
				for (int k = 0; k < transformPointUp.Count; k++)
				{
					Vector3 point = transformPointUp[k];
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
				for (int l = 0; l < transformPointDown.Count; l++)
				{
					Vector3 point2 = transformPointDown[l];
					if (minX > point2.x)
					{
						minX = point2.x;
					}
					if (maxX < point2.x)
					{
						maxX = point2.x;
					}
					if (minZ > point2.z)
					{
						minZ = point2.z;
					}
					if (maxZ < point2.z)
					{
						maxZ = point2.z;
					}
				}
				minX -= terrain.transform.position.x + distSmooth;
				maxX -= terrain.transform.position.x - distSmooth;
				minZ -= terrain.transform.position.z + distSmooth;
				maxZ -= terrain.transform.position.z - distSmooth;
				minX *= terrainToheight;
				maxX *= terrainToheight;
				minZ *= terrainTowidth;
				maxZ *= terrainTowidth;
				maxX = Mathf.Ceil(Mathf.Clamp(maxX + 1f, 0f, terrainData.heightmapResolution));
				minZ = Mathf.Floor(Mathf.Clamp(minZ, 0f, terrainData.heightmapResolution));
				maxZ = Mathf.Ceil(Mathf.Clamp(maxZ + 1f, 0f, terrainData.heightmapResolution));
				minX = Mathf.Floor(Mathf.Clamp(minX, 0f, terrainData.heightmapResolution));
				float[,] heightmapData = terrainData.GetHeights((int)minX, (int)minZ, (int)(maxX - minX), (int)(maxZ - minZ));
				Vector3 position = Vector3.zero;
				_ = Vector3.zero;
				for (int x = 0; x < heightmapData.GetLength(0); x++)
				{
					for (int z = 0; z < heightmapData.GetLength(1); z++)
					{
						position.x = ((float)z + minX) / terrainToheight + terrain.transform.position.x;
						position.z = ((float)x + minZ) / terrainTowidth + terrain.transform.position.z;
						Ray ray = new Ray(position + Vector3.up * 3000f, Vector3.down);
						if (meshCollider.Raycast(ray, out var hit, 10000f))
						{
							float height = hit.point.y - posY;
							heightmapData[x, z] = height / sizeY;
							if (debugLines)
							{
								Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.1f, Color.magenta, 10f);
							}
						}
					}
				}
				terrainData.SetHeightsDelayLOD((int)minX, (int)minZ, heightmapData);
			}
			global::UnityEngine.Object.DestroyImmediate(meshCollider);
			terrainData.SyncHeightmap();
			terrain.Flush();
		}
		Physics.autoSyncTransforms = savedAutoSyncTransforms;
		if (meshGO != null)
		{
			global::UnityEngine.Object.DestroyImmediate(meshGO);
		}
	}

	public void TerrainPaintMeshBased(Terrain singleTerrain = null)
	{
		bool savedAutoSyncTransforms = Physics.autoSyncTransforms;
		Physics.autoSyncTransforms = false;
		Terrain[] activeTerrains = Terrain.activeTerrains;
		foreach (Terrain terrain in activeTerrains)
		{
			if (singleTerrain != null && terrain != singleTerrain)
			{
				continue;
			}
			TerrainData terrainData = terrain.terrainData;
			float sizeX = terrain.terrainData.size.x;
			_ = terrain.terrainData.size;
			float sizeZ = terrain.terrainData.size.z;
			float terrainTowidth = 1f / sizeZ * (float)(terrainData.alphamapWidth - 1);
			float terrainToheight = 1f / sizeX * (float)(terrainData.alphamapHeight - 1);
			MeshCollider meshCollider = meshGO.gameObject.AddComponent<MeshCollider>();
			List<Vector3> transformPointUp = new List<Vector3>();
			List<Vector3> transformPointDown = new List<Vector3>();
			int pointsCount = 5;
			int pointsStart = 0;
			_ = Vector3.zero;
			_ = Vector3.zero;
			for (pointsStart = 0; pointsStart < pointsUp.Count; pointsStart = Mathf.Clamp(pointsStart + pointsCount - 1, 0, pointsUp.Count))
			{
				int end = Mathf.Min(pointsStart + pointsCount, pointsUp.Count);
				transformPointUp.Clear();
				transformPointDown.Clear();
				for (int j = pointsStart; j < end; j++)
				{
					transformPointUp.Add(base.transform.TransformPoint(pointsUp[j]));
					transformPointDown.Add(base.transform.TransformPoint(pointsDown[j]));
				}
				float minX = float.MaxValue;
				float maxX = float.MinValue;
				float minZ = float.MaxValue;
				float maxZ = float.MinValue;
				for (int k = 0; k < transformPointUp.Count; k++)
				{
					Vector3 point = transformPointUp[k];
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
				for (int l = 0; l < transformPointDown.Count; l++)
				{
					Vector3 point2 = transformPointDown[l];
					if (minX > point2.x)
					{
						minX = point2.x;
					}
					if (maxX < point2.x)
					{
						maxX = point2.x;
					}
					if (minZ > point2.z)
					{
						minZ = point2.z;
					}
					if (maxZ < point2.z)
					{
						maxZ = point2.z;
					}
				}
				minX -= terrain.transform.position.x + distSmooth;
				maxX -= terrain.transform.position.x - distSmooth;
				minZ -= terrain.transform.position.z + distSmooth;
				maxZ -= terrain.transform.position.z - distSmooth;
				minX *= terrainToheight;
				maxX *= terrainToheight;
				minZ *= terrainTowidth;
				maxZ *= terrainTowidth;
				minX = Mathf.Floor(Mathf.Clamp(minX, 0f, terrainData.alphamapWidth));
				maxX = Mathf.Ceil(Mathf.Clamp(maxX + 1f, 0f, terrainData.alphamapWidth));
				minZ = Mathf.Floor(Mathf.Clamp(minZ, 0f, terrainData.alphamapHeight));
				maxZ = Mathf.Ceil(Mathf.Clamp(maxZ + 1f, 0f, terrainData.alphamapHeight));
				float[,,] alphamapData = terrainData.GetAlphamaps((int)minX, (int)minZ, (int)(maxX - minX), (int)(maxZ - minZ));
				if (alphamapData.GetLength(2) <= currentSplatMap)
				{
					Debug.LogWarning("RAM: Terrain \"" + terrain.name + "\" doesn't have layer: " + currentSplatMap);
					break;
				}
				Vector3 position = Vector3.zero;
				_ = Vector3.zero;
				float noise = 0f;
				for (int x = 0; x < alphamapData.GetLength(0); x++)
				{
					for (int z = 0; z < alphamapData.GetLength(1); z++)
					{
						position.x = ((float)z + minX) / terrainToheight + terrain.transform.position.x;
						position.z = ((float)x + minZ) / terrainTowidth + terrain.transform.position.z;
						Ray ray = new Ray(position + Vector3.up * 3000f, Vector3.down);
						if (!meshCollider.Raycast(ray, out var hit, 10000f))
						{
							continue;
						}
						float minDist = hit.textureCoord.x;
						if (!mixTwoSplatMaps)
						{
							noise = ((!noisePaint) ? 0f : ((!(minDist >= 0f)) ? (Mathf.PerlinNoise(hit.point.x * noiseSizeXPaint, hit.point.z * noiseSizeZPaint) * noiseMultiplierOutsidePaint - noiseMultiplierOutsidePaint * 0.5f) : (Mathf.PerlinNoise(hit.point.x * noiseSizeXPaint, hit.point.z * noiseSizeZPaint) * noiseMultiplierInsidePaint - noiseMultiplierInsidePaint * 0.5f)));
							float oldValue = alphamapData[x, z, currentSplatMap];
							alphamapData[x, z, currentSplatMap] = Mathf.Clamp01(Mathf.Lerp(alphamapData[x, z, currentSplatMap], 1f, terrainPaintCarve.Evaluate(minDist) + terrainPaintCarve.Evaluate(minDist) * noise));
							for (int m = 0; m < terrainData.terrainLayers.Length; m++)
							{
								if (m != currentSplatMap)
								{
									alphamapData[x, z, m] = ((oldValue == 1f) ? 0f : Mathf.Clamp01(alphamapData[x, z, m] * ((1f - alphamapData[x, z, currentSplatMap]) / (1f - oldValue))));
								}
							}
						}
						else
						{
							noise = ((!(minDist >= 0f)) ? (Mathf.PerlinNoise(hit.point.x * noiseSizeXPaint, hit.point.z * noiseSizeZPaint) * noiseMultiplierOutsidePaint - noiseMultiplierOutsidePaint * 0.5f) : (Mathf.PerlinNoise(hit.point.x * noiseSizeXPaint, hit.point.z * noiseSizeZPaint) * noiseMultiplierInsidePaint - noiseMultiplierInsidePaint * 0.5f));
							float oldValue2 = alphamapData[x, z, currentSplatMap];
							alphamapData[x, z, currentSplatMap] = Mathf.Clamp01(Mathf.Lerp(alphamapData[x, z, currentSplatMap], 1f, terrainPaintCarve.Evaluate(minDist)));
							for (int n = 0; n < terrainData.terrainLayers.Length; n++)
							{
								if (n != currentSplatMap)
								{
									alphamapData[x, z, n] = ((oldValue2 == 1f) ? 0f : Mathf.Clamp01(alphamapData[x, z, n] * ((1f - alphamapData[x, z, currentSplatMap]) / (1f - oldValue2))));
								}
							}
							if (noise > 0f)
							{
								oldValue2 = alphamapData[x, z, secondSplatMap];
								alphamapData[x, z, secondSplatMap] = Mathf.Clamp01(Mathf.Lerp(alphamapData[x, z, secondSplatMap], 1f, noise));
								for (int num = 0; num < terrainData.terrainLayers.Length; num++)
								{
									if (num != secondSplatMap)
									{
										alphamapData[x, z, num] = ((oldValue2 == 1f) ? 0f : Mathf.Clamp01(alphamapData[x, z, num] * ((1f - alphamapData[x, z, secondSplatMap]) / (1f - oldValue2))));
									}
								}
							}
						}
						if (!addCliffSplatMap)
						{
							continue;
						}
						if (minDist >= 0f)
						{
							if (!(Vector3.Angle(hit.normal, Vector3.up) > cliffAngle))
							{
								continue;
							}
							float oldValue3 = alphamapData[x, z, cliffSplatMap];
							alphamapData[x, z, cliffSplatMap] = cliffBlend;
							for (int num2 = 0; num2 < terrainData.terrainLayers.Length; num2++)
							{
								if (num2 != cliffSplatMap)
								{
									alphamapData[x, z, num2] = ((oldValue3 == 1f) ? 0f : Mathf.Clamp01(alphamapData[x, z, num2] * ((1f - alphamapData[x, z, cliffSplatMap]) / (1f - oldValue3))));
								}
							}
						}
						else
						{
							if (!(Vector3.Angle(hit.normal, Vector3.up) > cliffAngleOutside))
							{
								continue;
							}
							float oldValue4 = alphamapData[x, z, cliffSplatMapOutside];
							alphamapData[x, z, cliffSplatMapOutside] = cliffBlendOutside;
							for (int num3 = 0; num3 < terrainData.terrainLayers.Length; num3++)
							{
								if (num3 != cliffSplatMapOutside)
								{
									alphamapData[x, z, num3] = ((oldValue4 == 1f) ? 0f : Mathf.Clamp01(alphamapData[x, z, num3] * ((1f - alphamapData[x, z, cliffSplatMapOutside]) / (1f - oldValue4))));
								}
							}
						}
					}
				}
				terrainData.SetAlphamaps((int)minX, (int)minZ, alphamapData);
			}
			global::UnityEngine.Object.DestroyImmediate(meshCollider);
			terrain.Flush();
		}
		Physics.autoSyncTransforms = savedAutoSyncTransforms;
		if (meshGO != null)
		{
			global::UnityEngine.Object.DestroyImmediate(meshGO);
		}
	}

	public void TerrainClearFoliage(bool details = true)
	{
		bool savedAutoSyncTransforms = Physics.autoSyncTransforms;
		Physics.autoSyncTransforms = false;
		Terrain[] activeTerrains = Terrain.activeTerrains;
		foreach (Terrain terrain in activeTerrains)
		{
			TerrainData terrainData = terrain.terrainData;
			Transform transformTerrain = terrain.transform;
			_ = terrain.transform.position;
			float sizeX = terrain.terrainData.size.x;
			_ = terrain.terrainData.size;
			float sizeZ = terrain.terrainData.size.z;
			float terrainTowidth = 1f / sizeZ * (float)terrainData.detailWidth;
			float terrainToheight = 1f / sizeX * (float)terrainData.detailHeight;
			MeshCollider meshCollider = meshGO.gameObject.AddComponent<MeshCollider>();
			List<Vector3> transformPointUp = new List<Vector3>();
			List<Vector3> transformPointDown = new List<Vector3>();
			int pointsCount = 5;
			int pointsStart = 0;
			_ = Vector3.zero;
			_ = Vector3.zero;
			Vector3 position = Vector3.zero;
			if (details)
			{
				for (pointsStart = 0; pointsStart < pointsUp.Count; pointsStart = Mathf.Clamp(pointsStart + pointsCount - 1, 0, pointsUp.Count))
				{
					int end = Mathf.Min(pointsStart + pointsCount, pointsUp.Count);
					Mathf.Min(pointsCount, pointsUp.Count - pointsStart);
					transformPointUp.Clear();
					transformPointDown.Clear();
					for (int j = pointsStart; j < end; j++)
					{
						transformPointUp.Add(base.transform.TransformPoint(pointsUp[j]));
						transformPointDown.Add(base.transform.TransformPoint(pointsDown[j]));
					}
					float minX = float.MaxValue;
					float maxX = float.MinValue;
					float minZ = float.MaxValue;
					float maxZ = float.MinValue;
					for (int k = 0; k < transformPointUp.Count; k++)
					{
						Vector3 point = transformPointUp[k];
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
					for (int l = 0; l < transformPointDown.Count; l++)
					{
						Vector3 point2 = transformPointDown[l];
						if (minX > point2.x)
						{
							minX = point2.x;
						}
						if (maxX < point2.x)
						{
							maxX = point2.x;
						}
						if (minZ > point2.z)
						{
							minZ = point2.z;
						}
						if (maxZ < point2.z)
						{
							maxZ = point2.z;
						}
					}
					minX -= transformTerrain.position.x + distanceClearFoliage;
					maxX -= transformTerrain.position.x - distanceClearFoliage;
					minZ -= transformTerrain.position.z + distanceClearFoliage;
					maxZ -= transformTerrain.position.z - distanceClearFoliage;
					minX *= terrainToheight;
					maxX *= terrainToheight;
					minZ *= terrainTowidth;
					maxZ *= terrainTowidth;
					minX = Mathf.Floor(Mathf.Clamp(minX, 0f, terrainData.detailWidth));
					maxX = Mathf.Ceil(Mathf.Clamp(maxX + 1f, 0f, terrainData.detailWidth));
					minZ = Mathf.Floor(Mathf.Clamp(minZ, 0f, terrainData.detailHeight));
					maxZ = Mathf.Ceil(Mathf.Clamp(maxZ + 1f, 0f, terrainData.detailHeight));
					if (maxX - minX > 0f && maxZ - minZ > 0f)
					{
						for (int m = 0; m < terrainData.detailPrototypes.Length; m++)
						{
							int[,] detailLayer = terrainData.GetDetailLayer((int)minX, (int)minZ, (int)(maxX - minX), (int)(maxZ - minZ), m);
							for (int x = 0; x < detailLayer.GetLength(0); x++)
							{
								for (int z = 0; z < detailLayer.GetLength(1); z++)
								{
									position.x = ((float)z + minX) / terrainToheight + terrain.transform.position.x;
									position.z = ((float)x + minZ) / terrainTowidth + terrain.transform.position.z;
									Ray ray = new Ray(position + Vector3.up * 3000f, Vector3.down);
									if (meshCollider.Raycast(ray, out var _, 10000f))
									{
										detailLayer[x, z] = 0;
									}
								}
							}
							terrainData.SetDetailLayer((int)minX, (int)minZ, m, detailLayer);
						}
					}
				}
			}
			else
			{
				List<TreeInstance> newTrees = new List<TreeInstance>();
				TreeInstance[] treeInstances = terrainData.treeInstances;
				for (int n = 0; n < treeInstances.Length; n++)
				{
					TreeInstance tree = treeInstances[n];
					position.x = tree.position.x * sizeX + transformTerrain.position.x;
					position.z = tree.position.z * sizeZ + transformTerrain.position.z;
					Ray ray2 = new Ray(position + Vector3.up * 3000f, Vector3.down);
					if (!meshCollider.Raycast(ray2, out var _, 10000f))
					{
						newTrees.Add(tree);
					}
				}
				terrainData.treeInstances = newTrees.ToArray();
			}
			global::UnityEngine.Object.DestroyImmediate(meshCollider);
			terrain.Flush();
		}
		Physics.autoSyncTransforms = savedAutoSyncTransforms;
		if (meshGO != null)
		{
			global::UnityEngine.Object.DestroyImmediate(meshGO);
		}
	}

	private float FlowCalculate(float u, float normalY, Vector3 vertice)
	{
		float noise = (noiseflowMap ? (Mathf.PerlinNoise(vertice.x * noiseSizeXflowMap, vertice.z * noiseSizeZflowMap) * noiseMultiplierflowMap - noiseMultiplierflowMap * 0.5f) : 0f) * Mathf.Pow(Mathf.Clamp(normalY, 0f, 1f), 5f);
		return Mathf.Lerp(flowWaterfall.Evaluate(u), flowFlat.Evaluate(u) + noise, Mathf.Clamp(normalY, 0f, 1f));
	}
}
