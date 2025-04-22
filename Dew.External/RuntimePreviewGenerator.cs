using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class RuntimePreviewGenerator
{
	private class CameraSetup
	{
		private Vector3 position;

		private Quaternion rotation;

		private Color backgroundColor;

		private bool orthographic;

		private float orthographicSize;

		private float nearClipPlane;

		private float farClipPlane;

		private float aspect;

		private int cullingMask;

		private CameraClearFlags clearFlags;

		private RenderTexture targetTexture;

		public void GetSetup(Camera camera)
		{
			position = camera.transform.position;
			rotation = camera.transform.rotation;
			backgroundColor = camera.backgroundColor;
			orthographic = camera.orthographic;
			orthographicSize = camera.orthographicSize;
			nearClipPlane = camera.nearClipPlane;
			farClipPlane = camera.farClipPlane;
			aspect = camera.aspect;
			cullingMask = camera.cullingMask;
			clearFlags = camera.clearFlags;
			targetTexture = camera.targetTexture;
		}

		public void ApplySetup(Camera camera)
		{
			camera.transform.position = position;
			camera.transform.rotation = rotation;
			camera.backgroundColor = backgroundColor;
			camera.orthographic = orthographic;
			camera.orthographicSize = orthographicSize;
			camera.aspect = aspect;
			camera.cullingMask = cullingMask;
			camera.clearFlags = clearFlags;
			if (nearClipPlane < camera.farClipPlane)
			{
				camera.nearClipPlane = nearClipPlane;
				camera.farClipPlane = farClipPlane;
			}
			else
			{
				camera.farClipPlane = farClipPlane;
				camera.nearClipPlane = nearClipPlane;
			}
			camera.targetTexture = targetTexture;
			targetTexture = null;
		}
	}

	private const int PREVIEW_LAYER = 22;

	private static Vector3 PREVIEW_POSITION = new Vector3(-250f, -250f, -250f);

	private static Camera renderCamera;

	private static readonly CameraSetup cameraSetup = new CameraSetup();

	private static readonly Vector3[] boundingBoxPoints = new Vector3[8];

	private static readonly Vector3[] localBoundsMinMax = new Vector3[2];

	private static readonly List<Renderer> renderersList = new List<Renderer>(64);

	private static readonly List<int> layersList = new List<int>(64);

	private static Camera m_internalCamera = null;

	private static Camera m_previewRenderCamera;

	private static Vector3 m_previewDirection = new Vector3(-0.57735f, -0.57735f, -0.57735f);

	private static float m_padding;

	private static Color m_backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);

	private static bool m_orthographicMode = false;

	private static bool m_useLocalBounds = false;

	private static float m_renderSupersampling = 1f;

	private static bool m_markTextureNonReadable = true;

	private static Camera InternalCamera
	{
		get
		{
			if (m_internalCamera == null)
			{
				m_internalCamera = new GameObject("ModelPreviewGeneratorCamera").AddComponent<Camera>();
				m_internalCamera.enabled = false;
				m_internalCamera.nearClipPlane = 0.01f;
				m_internalCamera.cullingMask = 4194304;
				m_internalCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_internalCamera;
		}
	}

	public static Camera PreviewRenderCamera
	{
		get
		{
			return m_previewRenderCamera;
		}
		set
		{
			m_previewRenderCamera = value;
		}
	}

	public static Vector3 PreviewDirection
	{
		get
		{
			return m_previewDirection;
		}
		set
		{
			m_previewDirection = value.normalized;
		}
	}

	public static float Padding
	{
		get
		{
			return m_padding;
		}
		set
		{
			m_padding = Mathf.Clamp(value, -0.25f, 0.25f);
		}
	}

	public static Color BackgroundColor
	{
		get
		{
			return m_backgroundColor;
		}
		set
		{
			m_backgroundColor = value;
		}
	}

	public static bool OrthographicMode
	{
		get
		{
			return m_orthographicMode;
		}
		set
		{
			m_orthographicMode = value;
		}
	}

	public static bool UseLocalBounds
	{
		get
		{
			return m_useLocalBounds;
		}
		set
		{
			m_useLocalBounds = value;
		}
	}

	public static float RenderSupersampling
	{
		get
		{
			return m_renderSupersampling;
		}
		set
		{
			m_renderSupersampling = Mathf.Max(value, 0.1f);
		}
	}

	public static bool MarkTextureNonReadable
	{
		get
		{
			return m_markTextureNonReadable;
		}
		set
		{
			m_markTextureNonReadable = value;
		}
	}

	public static Texture2D GenerateMaterialPreview(Material material, PrimitiveType previewPrimitive, int width = 64, int height = 64)
	{
		return GenerateMaterialPreviewInternal(material, previewPrimitive, null, null, width, height);
	}

	public static Texture2D GenerateMaterialPreviewWithShader(Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width = 64, int height = 64)
	{
		return GenerateMaterialPreviewInternal(material, previewPrimitive, shader, replacementTag, width, height);
	}

	public static void GenerateMaterialPreviewAsync(Action<Texture2D> callback, Material material, PrimitiveType previewPrimitive, int width = 64, int height = 64)
	{
		GenerateMaterialPreviewInternal(material, previewPrimitive, null, null, width, height, callback);
	}

	public static void GenerateMaterialPreviewWithShaderAsync(Action<Texture2D> callback, Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width = 64, int height = 64)
	{
		GenerateMaterialPreviewInternal(material, previewPrimitive, shader, replacementTag, width, height, callback);
	}

	private static Texture2D GenerateMaterialPreviewInternal(Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width, int height, Action<Texture2D> asyncCallback = null)
	{
		GameObject previewModel = GameObject.CreatePrimitive(previewPrimitive);
		previewModel.gameObject.hideFlags = HideFlags.HideAndDontSave;
		previewModel.GetComponent<Renderer>().sharedMaterial = material;
		try
		{
			return GenerateModelPreviewInternal(previewModel.transform, shader, replacementTag, width, height, shouldCloneModel: false, shouldIgnoreParticleSystems: true, asyncCallback);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
			global::UnityEngine.Object.DestroyImmediate(previewModel);
		}
		return null;
	}

	public static Texture2D GenerateModelPreview(Transform model, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		return GenerateModelPreviewInternal(model, null, null, width, height, shouldCloneModel, shouldIgnoreParticleSystems);
	}

	public static Texture2D GenerateModelPreviewWithShader(Transform model, Shader shader, string replacementTag, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		return GenerateModelPreviewInternal(model, shader, replacementTag, width, height, shouldCloneModel, shouldIgnoreParticleSystems);
	}

	public static void GenerateModelPreviewAsync(Action<Texture2D> callback, Transform model, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		GenerateModelPreviewInternal(model, null, null, width, height, shouldCloneModel, shouldIgnoreParticleSystems, callback);
	}

	public static void GenerateModelPreviewWithShaderAsync(Action<Texture2D> callback, Transform model, Shader shader, string replacementTag, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		GenerateModelPreviewInternal(model, shader, replacementTag, width, height, shouldCloneModel, shouldIgnoreParticleSystems, callback);
	}

	private static Texture2D GenerateModelPreviewInternal(Transform model, Shader shader, string replacementTag, int width, int height, bool shouldCloneModel, bool shouldIgnoreParticleSystems, Action<Texture2D> asyncCallback = null)
	{
		if (!model)
		{
			if (asyncCallback != null)
			{
				asyncCallback(null);
			}
			return null;
		}
		Texture2D result = null;
		if (!model.gameObject.scene.IsValid() || !model.gameObject.scene.isLoaded)
		{
			shouldCloneModel = true;
		}
		Transform previewObject;
		if (shouldCloneModel)
		{
			previewObject = global::UnityEngine.Object.Instantiate(model, null, worldPositionStays: false);
			previewObject.gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
		else
		{
			previewObject = model;
			layersList.Clear();
			GetLayerRecursively(previewObject);
		}
		bool isStatic = IsStatic(model);
		bool wasActive = previewObject.gameObject.activeSelf;
		Vector3 prevPos = previewObject.position;
		Quaternion prevRot = previewObject.rotation;
		bool asyncOperationStarted = false;
		try
		{
			SetupCamera();
			SetLayerRecursively(previewObject);
			if (!isStatic)
			{
				previewObject.position = PREVIEW_POSITION;
				previewObject.rotation = Quaternion.identity;
			}
			if (!wasActive)
			{
				previewObject.gameObject.SetActive(value: true);
			}
			Quaternion cameraRotation = Quaternion.LookRotation(previewObject.rotation * m_previewDirection, previewObject.up);
			Bounds previewBounds = default(Bounds);
			if (!CalculateBounds(previewObject, shouldIgnoreParticleSystems, cameraRotation, out previewBounds))
			{
				if (asyncCallback != null)
				{
					asyncCallback(null);
				}
				return null;
			}
			renderCamera.aspect = (float)width / (float)height;
			renderCamera.transform.rotation = cameraRotation;
			CalculateCameraPosition(renderCamera, previewBounds, m_padding);
			renderCamera.farClipPlane = (renderCamera.transform.position - previewBounds.center).magnitude + (m_useLocalBounds ? (previewBounds.extents.z * 1.01f) : previewBounds.size.magnitude);
			RenderTexture activeRT = RenderTexture.active;
			RenderTexture renderTexture = null;
			try
			{
				int supersampledWidth = Mathf.RoundToInt((float)width * m_renderSupersampling);
				int supersampledHeight = Mathf.RoundToInt((float)height * m_renderSupersampling);
				renderTexture = RenderTexture.GetTemporary(supersampledWidth, supersampledHeight, 16);
				RenderTexture.active = renderTexture;
				if (m_backgroundColor.a < 1f)
				{
					GL.Clear(clearDepth: true, clearColor: true, m_backgroundColor);
				}
				renderCamera.targetTexture = renderTexture;
				if (!shader)
				{
					renderCamera.Render();
				}
				else
				{
					renderCamera.RenderWithShader(shader, replacementTag ?? string.Empty);
				}
				renderCamera.targetTexture = null;
				if (supersampledWidth != width || supersampledHeight != height)
				{
					RenderTexture _renderTexture = null;
					try
					{
						_renderTexture = (RenderTexture.active = RenderTexture.GetTemporary(width, height, 16));
						if (m_backgroundColor.a < 1f)
						{
							GL.Clear(clearDepth: true, clearColor: true, m_backgroundColor);
						}
						Graphics.Blit(renderTexture, _renderTexture);
					}
					finally
					{
						if ((bool)_renderTexture)
						{
							RenderTexture.ReleaseTemporary(renderTexture);
							renderTexture = _renderTexture;
						}
					}
				}
				if (asyncCallback != null)
				{
					AsyncGPUReadback.Request(renderTexture, 0, (m_backgroundColor.a < 1f) ? TextureFormat.RGBA32 : TextureFormat.RGB24, delegate(AsyncGPUReadbackRequest asyncResult)
					{
						try
						{
							result = new Texture2D(width, height, (m_backgroundColor.a < 1f) ? TextureFormat.RGBA32 : TextureFormat.RGB24, mipChain: false);
							if (!asyncResult.hasError)
							{
								result.LoadRawTextureData(asyncResult.GetData<byte>());
							}
							else
							{
								Debug.LogWarning("Async thumbnail request failed, falling back to conventional method");
								RenderTexture active = RenderTexture.active;
								try
								{
									RenderTexture.active = renderTexture;
									result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
								}
								finally
								{
									RenderTexture.active = active;
								}
							}
							result.Apply(updateMipmaps: false, m_markTextureNonReadable);
							asyncCallback(result);
						}
						finally
						{
							if ((bool)renderTexture)
							{
								RenderTexture.ReleaseTemporary(renderTexture);
							}
						}
					});
					asyncOperationStarted = true;
				}
				else
				{
					result = new Texture2D(width, height, (m_backgroundColor.a < 1f) ? TextureFormat.RGBA32 : TextureFormat.RGB24, mipChain: false);
					result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
					result.Apply(updateMipmaps: false, m_markTextureNonReadable);
				}
			}
			finally
			{
				RenderTexture.active = activeRT;
				if ((bool)renderTexture && !asyncOperationStarted)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
			if (shouldCloneModel)
			{
				global::UnityEngine.Object.DestroyImmediate(previewObject.gameObject);
			}
			else
			{
				if (!wasActive)
				{
					previewObject.gameObject.SetActive(value: false);
				}
				if (!isStatic)
				{
					previewObject.position = prevPos;
					previewObject.rotation = prevRot;
				}
				int index = 0;
				SetLayerRecursively(previewObject, ref index);
			}
			if (renderCamera == m_previewRenderCamera)
			{
				cameraSetup.ApplySetup(renderCamera);
			}
		}
		if (!asyncOperationStarted && asyncCallback != null)
		{
			asyncCallback(null);
		}
		return result;
	}

	public static bool CalculateBounds(Transform target, bool shouldIgnoreParticleSystems, Quaternion cameraRotation, out Bounds bounds)
	{
		renderersList.Clear();
		target.GetComponentsInChildren(renderersList);
		Quaternion inverseCameraRotation = Quaternion.Inverse(cameraRotation);
		Vector3 localBoundsMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 localBoundsMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		bounds = default(Bounds);
		bool hasBounds = false;
		for (int i = 0; i < renderersList.Count; i++)
		{
			if (!renderersList[i].enabled || (shouldIgnoreParticleSystems && renderersList[i] is ParticleSystemRenderer))
			{
				continue;
			}
			if (m_useLocalBounds)
			{
				Bounds localBounds = renderersList[i].localBounds;
				Transform transform = renderersList[i].transform;
				localBoundsMinMax[0] = localBounds.min;
				localBoundsMinMax[1] = localBounds.max;
				for (int x = 0; x < 2; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						for (int z = 0; z < 2; z++)
						{
							Vector3 point = inverseCameraRotation * transform.TransformPoint(new Vector3(localBoundsMinMax[x].x, localBoundsMinMax[y].y, localBoundsMinMax[z].z));
							localBoundsMin = Vector3.Min(localBoundsMin, point);
							localBoundsMax = Vector3.Max(localBoundsMax, point);
						}
					}
				}
				hasBounds = true;
			}
			else if (!hasBounds)
			{
				bounds = renderersList[i].bounds;
				hasBounds = true;
			}
			else
			{
				bounds.Encapsulate(renderersList[i].bounds);
			}
		}
		if (m_useLocalBounds && hasBounds)
		{
			bounds = new Bounds(cameraRotation * ((localBoundsMin + localBoundsMax) * 0.5f), localBoundsMax - localBoundsMin);
		}
		return hasBounds;
	}

	public static void CalculateCameraPosition(Camera camera, Bounds bounds, float padding = 0f)
	{
		Transform cameraTR = camera.transform;
		Vector3 cameraDirection = cameraTR.forward;
		float aspect = camera.aspect;
		if (padding != 0f)
		{
			bounds.size *= 1f + padding * 2f;
		}
		Vector3 boundsCenter = bounds.center;
		Vector3 boundsExtents = bounds.extents;
		Vector3 boundsSize = 2f * boundsExtents;
		if (m_useLocalBounds)
		{
			Matrix4x4 localBoundsMatrix = Matrix4x4.TRS(boundsCenter, camera.transform.rotation, Vector3.one);
			Vector3 point = boundsExtents;
			boundingBoxPoints[0] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.x -= boundsSize.x;
			boundingBoxPoints[1] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.y -= boundsSize.y;
			boundingBoxPoints[2] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.x += boundsSize.x;
			boundingBoxPoints[3] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.z -= boundsSize.z;
			boundingBoxPoints[4] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.x -= boundsSize.x;
			boundingBoxPoints[5] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.y += boundsSize.y;
			boundingBoxPoints[6] = localBoundsMatrix.MultiplyPoint3x4(point);
			point.x += boundsSize.x;
			boundingBoxPoints[7] = localBoundsMatrix.MultiplyPoint3x4(point);
		}
		else
		{
			Vector3 point2 = boundsCenter + boundsExtents;
			boundingBoxPoints[0] = point2;
			point2.x -= boundsSize.x;
			boundingBoxPoints[1] = point2;
			point2.y -= boundsSize.y;
			boundingBoxPoints[2] = point2;
			point2.x += boundsSize.x;
			boundingBoxPoints[3] = point2;
			point2.z -= boundsSize.z;
			boundingBoxPoints[4] = point2;
			point2.x -= boundsSize.x;
			boundingBoxPoints[5] = point2;
			point2.y += boundsSize.y;
			boundingBoxPoints[6] = point2;
			point2.x += boundsSize.x;
			boundingBoxPoints[7] = point2;
		}
		if (camera.orthographic)
		{
			cameraTR.position = boundsCenter;
			float minX = float.PositiveInfinity;
			float minY = float.PositiveInfinity;
			float maxX = float.NegativeInfinity;
			float maxY = float.NegativeInfinity;
			for (int i = 0; i < boundingBoxPoints.Length; i++)
			{
				Vector3 localPoint = cameraTR.InverseTransformPoint(boundingBoxPoints[i]);
				if (localPoint.x < minX)
				{
					minX = localPoint.x;
				}
				if (localPoint.x > maxX)
				{
					maxX = localPoint.x;
				}
				if (localPoint.y < minY)
				{
					minY = localPoint.y;
				}
				if (localPoint.y > maxY)
				{
					maxY = localPoint.y;
				}
			}
			float distance = boundsExtents.magnitude + 1f;
			camera.orthographicSize = Mathf.Max(maxY - minY, (maxX - minX) / aspect) * 0.5f;
			cameraTR.position = boundsCenter - cameraDirection * distance;
			return;
		}
		Vector3 cameraUp = cameraTR.up;
		Vector3 cameraRight = cameraTR.right;
		float verticalFOV = camera.fieldOfView * 0.5f;
		float horizontalFOV = Mathf.Atan(Mathf.Tan(verticalFOV * (MathF.PI / 180f)) * aspect) * 57.29578f;
		Vector3 topFrustumPlaneNormal = Quaternion.AngleAxis(90f + verticalFOV, -cameraRight) * cameraDirection;
		Vector3 bottomFrustumPlaneNormal = Quaternion.AngleAxis(90f + verticalFOV, cameraRight) * cameraDirection;
		Vector3 rightFrustumPlaneNormal = Quaternion.AngleAxis(90f + horizontalFOV, cameraUp) * cameraDirection;
		Vector3 leftFrustumPlaneNormal = Quaternion.AngleAxis(90f + horizontalFOV, -cameraUp) * cameraDirection;
		int leftmostPoint = -1;
		int rightmostPoint = -1;
		int topmostPoint = -1;
		int bottommostPoint = -1;
		for (int j = 0; j < boundingBoxPoints.Length; j++)
		{
			if (leftmostPoint < 0 && IsOutermostPointInDirection(j, leftFrustumPlaneNormal))
			{
				leftmostPoint = j;
			}
			if (rightmostPoint < 0 && IsOutermostPointInDirection(j, rightFrustumPlaneNormal))
			{
				rightmostPoint = j;
			}
			if (topmostPoint < 0 && IsOutermostPointInDirection(j, topFrustumPlaneNormal))
			{
				topmostPoint = j;
			}
			if (bottommostPoint < 0 && IsOutermostPointInDirection(j, bottomFrustumPlaneNormal))
			{
				bottommostPoint = j;
			}
		}
		Ray planesIntersection = GetPlanesIntersection(new Plane(leftFrustumPlaneNormal, boundingBoxPoints[leftmostPoint]), new Plane(rightFrustumPlaneNormal, boundingBoxPoints[rightmostPoint]));
		Ray verticalIntersection = GetPlanesIntersection(new Plane(topFrustumPlaneNormal, boundingBoxPoints[topmostPoint]), new Plane(bottomFrustumPlaneNormal, boundingBoxPoints[bottommostPoint]));
		FindClosestPointsOnTwoLines(planesIntersection, verticalIntersection, out var closestPoint1, out var closestPoint2);
		cameraTR.position = ((Vector3.Dot(closestPoint1 - closestPoint2, cameraDirection) < 0f) ? closestPoint1 : closestPoint2);
	}

	private static bool IsOutermostPointInDirection(int pointIndex, Vector3 direction)
	{
		Vector3 point = boundingBoxPoints[pointIndex];
		for (int i = 0; i < boundingBoxPoints.Length; i++)
		{
			if (i != pointIndex && Vector3.Dot(direction, boundingBoxPoints[i] - point) > 0f)
			{
				return false;
			}
		}
		return true;
	}

	private static Ray GetPlanesIntersection(Plane p1, Plane p2)
	{
		Vector3 p3Normal = Vector3.Cross(p1.normal, p2.normal);
		float det = p3Normal.sqrMagnitude;
		return new Ray((Vector3.Cross(p3Normal, p2.normal) * p1.distance + Vector3.Cross(p1.normal, p3Normal) * p2.distance) / det, p3Normal);
	}

	private static void FindClosestPointsOnTwoLines(Ray line1, Ray line2, out Vector3 closestPointLine1, out Vector3 closestPointLine2)
	{
		Vector3 line1Direction = line1.direction;
		Vector3 line2Direction = line2.direction;
		float num = Vector3.Dot(line1Direction, line1Direction);
		float b = Vector3.Dot(line1Direction, line2Direction);
		float e = Vector3.Dot(line2Direction, line2Direction);
		float d = num * e - b * b;
		Vector3 r = line1.origin - line2.origin;
		float c = Vector3.Dot(line1Direction, r);
		float f = Vector3.Dot(line2Direction, r);
		float s = (b * f - c * e) / d;
		float t = (num * f - c * b) / d;
		closestPointLine1 = line1.origin + line1Direction * s;
		closestPointLine2 = line2.origin + line2Direction * t;
	}

	private static void SetupCamera()
	{
		if ((bool)m_previewRenderCamera)
		{
			cameraSetup.GetSetup(m_previewRenderCamera);
			renderCamera = m_previewRenderCamera;
			renderCamera.nearClipPlane = 0.01f;
			renderCamera.cullingMask = 4194304;
		}
		else
		{
			renderCamera = InternalCamera;
		}
		renderCamera.backgroundColor = m_backgroundColor;
		renderCamera.orthographic = m_orthographicMode;
		renderCamera.clearFlags = ((m_backgroundColor.a < 1f) ? CameraClearFlags.Depth : CameraClearFlags.Color);
	}

	private static bool IsStatic(Transform obj)
	{
		if (obj.gameObject.isStatic)
		{
			return true;
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			if (IsStatic(obj.GetChild(i)))
			{
				return true;
			}
		}
		return false;
	}

	private static void SetLayerRecursively(Transform obj)
	{
		obj.gameObject.layer = 22;
		for (int i = 0; i < obj.childCount; i++)
		{
			SetLayerRecursively(obj.GetChild(i));
		}
	}

	private static void GetLayerRecursively(Transform obj)
	{
		layersList.Add(obj.gameObject.layer);
		for (int i = 0; i < obj.childCount; i++)
		{
			GetLayerRecursively(obj.GetChild(i));
		}
	}

	private static void SetLayerRecursively(Transform obj, ref int index)
	{
		obj.gameObject.layer = layersList[index++];
		for (int i = 0; i < obj.childCount; i++)
		{
			SetLayerRecursively(obj.GetChild(i), ref index);
		}
	}
}
