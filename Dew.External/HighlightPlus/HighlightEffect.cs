using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

namespace HighlightPlus;

[ExecuteAlways]
[HelpURL("https://kronnect.com/guides/highlight-plus-introduction/")]
public class HighlightEffect : MonoBehaviour
{
	private struct ModelMaterials
	{
		public bool render;

		public Transform transform;

		public bool renderWasVisibleDuringSetup;

		public Mesh mesh;

		public Mesh originalMesh;

		public Mesh bakedSkinnedMesh;

		public Renderer renderer;

		public bool isSkinnedMesh;

		public Material[] fxMatMask;

		public Material[] fxMatOutline;

		public Material[] fxMatGlow;

		public Material[] fxMatSolidColor;

		public Material[] fxMatSeeThroughInner;

		public Material[] fxMatSeeThroughBorder;

		public Material[] fxMatOverlay;

		public Material[] fxMatInnerGlow;

		public NormalsOption normalsOption;

		public Matrix4x4 renderingMatrix;

		public bool isCombined;

		public bool preserveOriginalMesh
		{
			get
			{
				if (!isCombined)
				{
					return normalsOption == NormalsOption.PreserveOriginal;
				}
				return false;
			}
		}

		public void Init()
		{
			render = false;
			transform = null;
			mesh = (originalMesh = null);
			if (bakedSkinnedMesh != null)
			{
				global::UnityEngine.Object.DestroyImmediate(bakedSkinnedMesh);
			}
			renderer = null;
			isSkinnedMesh = false;
			normalsOption = NormalsOption.Smooth;
			isCombined = false;
		}
	}

	public enum FadingState
	{
		FadingOut = -1,
		NoFading,
		FadingIn
	}

	private class PerCameraOcclusionData
	{
		public float checkLastTime = -10000f;

		public int occlusionRenderFrame;

		public bool lastOcclusionTestResult;

		public readonly List<Renderer> cachedOccluders = new List<Renderer>();
	}

	[Tooltip("The current profile (optional). A profile let you store Highlight Plus settings and apply those settings easily to many objects. You can also load a profile and apply its settings at runtime, using the ProfileLoad() method of the Highlight Effect component.")]
	public HighlightProfile profile;

	[Tooltip("If enabled, settings from the profile will be applied to this component automatically when game starts or when any profile setting is updated.")]
	public bool profileSync;

	[Tooltip("Which cameras can render the effect.")]
	public LayerMask camerasLayerMask = -1;

	[Tooltip("Different options to specify which objects are affected by this Highlight Effect component.")]
	public TargetOptions effectGroup;

	[Tooltip("The layer that contains the affected objects by this effect when effectGroup is set to LayerMask.")]
	public LayerMask effectGroupLayer = -1;

	[Tooltip("Only include objects whose names contains this text.")]
	public string effectNameFilter;

	[Tooltip("Use RegEx to determine if an object name matches the effectNameFilter.")]
	public bool effectNameUseRegEx;

	[Tooltip("Combine meshes of all objects in this group affected by Highlight Effect reducing draw calls.")]
	public bool combineMeshes;

	[Tooltip("The alpha threshold for transparent cutout objects. Pixels with alpha below this value will be discarded.")]
	[Range(0f, 1f)]
	public float alphaCutOff;

	[Tooltip("If back facing triangles are ignored.Backfaces triangles are not visible but you may set this property to false to force highlight effects to act on those triangles as well.")]
	public bool cullBackFaces = true;

	[Tooltip("Show highlight effects even if the object is not visible. If this object or its children use GPU Instancing tools, the MeshRenderer can be disabled although the object is visible. In this case, this option is useful to enable highlighting.")]
	public bool ignoreObjectVisibility;

	[Tooltip("Support reflection probes. Enable only if you want the effects to be visible in reflections.")]
	public bool reflectionProbes;

	[Tooltip("Enables GPU instancing. Reduces draw calls in outline and outer glow effects on platforms that support GPU instancing. Should be enabled by default.")]
	public bool GPUInstancing = true;

	[Tooltip("Bakes skinned mesh to leverage GPU instancing when using outline/outer glow with mesh-based rendering. Reduces draw calls significantly on skinned meshes.")]
	public bool optimizeSkinnedMesh = true;

	[Tooltip("Enables depth buffer clipping. Only applies to outline or outer glow in High Quality mode.")]
	public bool depthClip;

	[Tooltip("Fades out effects based on distance to camera")]
	public bool cameraDistanceFade;

	[Tooltip("The closest distance particles can get to the camera before they fade from the camera’s view.")]
	public float cameraDistanceFadeNear;

	[Tooltip("The farthest distance particles can get away from the camera before they fade from the camera’s view.")]
	public float cameraDistanceFadeFar = 1000f;

	[Tooltip("Normals handling option:\nPreserve original: use original mesh normals.\nSmooth: average normals to produce a smoother outline/glow mesh based effect.\nReorient: recomputes normals based on vertex direction to centroid.")]
	public NormalsOption normalsOption;

	[Tooltip("Ignore highlighting on this object.")]
	public bool ignore;

	[SerializeField]
	private bool _highlighted;

	public float fadeInDuration;

	public float fadeOutDuration;

	public bool flipY;

	[Tooltip("Keeps the outline/glow size unaffected by object distance.")]
	public bool constantWidth = true;

	[Tooltip("Mask to include or exclude certain submeshes. By default, all submeshes are included.")]
	public int subMeshMask = -1;

	[Range(0f, 1f)]
	[Tooltip("Intensity of the overlay effect. A value of 0 disables the overlay completely.")]
	public float overlay;

	public OverlayMode overlayMode;

	[ColorUsage(true, true)]
	public Color overlayColor = Color.yellow;

	public float overlayAnimationSpeed = 1f;

	[Range(0f, 1f)]
	public float overlayMinIntensity = 0.5f;

	[Range(0f, 1f)]
	[Tooltip("Controls the blending or mix of the overlay color with the natural colors of the object.")]
	public float overlayBlending = 1f;

	[Tooltip("Optional overlay texture.")]
	public Texture2D overlayTexture;

	public TextureUVSpace overlayTextureUVSpace;

	public float overlayTextureScale = 1f;

	public Vector2 overlayTextureScrolling;

	public Visibility overlayVisibility;

	[Range(0f, 1f)]
	[Tooltip("Intensity of the outline. A value of 0 disables the outline completely.")]
	public float outline = 1f;

	[ColorUsage(true, true)]
	public Color outlineColor = Color.black;

	public ColorStyle outlineColorStyle;

	[GradientUsage(true, ColorSpace.Linear)]
	public Gradient outlineGradient;

	public bool outlineGradientInLocalSpace;

	public float outlineWidth = 0.45f;

	[Range(1f, 3f)]
	public int outlineBlurPasses = 2;

	public QualityLevel outlineQuality = QualityLevel.Medium;

	public OutlineEdgeMode outlineEdgeMode;

	public float outlineEdgeThreshold = 0.995f;

	public float outlineSharpness = 1f;

	[Range(1f, 8f)]
	[Tooltip("Reduces the quality of the outline but improves performance a bit.")]
	public int outlineDownsampling = 1;

	public Visibility outlineVisibility;

	public GlowBlendMode glowBlendMode;

	public bool outlineBlitDebug;

	[Tooltip("If enabled, this object won't combine the outline with other objects.")]
	public bool outlineIndependent;

	public ContourStyle outlineContourStyle;

	[Tooltip("Select the mask mode used with this effect.")]
	public MaskMode outlineMaskMode;

	[Range(0f, 5f)]
	[Tooltip("The intensity of the outer glow effect. A value of 0 disables the glow completely.")]
	public float glow;

	public float glowWidth = 0.4f;

	public QualityLevel glowQuality = QualityLevel.Medium;

	public BlurMethod glowBlurMethod;

	[Range(1f, 8f)]
	[Tooltip("Reduces the quality of the glow but improves performance a bit.")]
	public int glowDownsampling = 2;

	[ColorUsage(true, true)]
	public Color glowHQColor = new Color(0.64f, 1f, 0f, 1f);

	[Tooltip("When enabled, outer glow renders with dithering. When disabled, glow appears as a solid color.")]
	[Range(0f, 1f)]
	public float glowDithering = 1f;

	public GlowDitheringStyle glowDitheringStyle;

	[Tooltip("Seed for the dithering effect")]
	public float glowMagicNumber1 = 0.75f;

	[Tooltip("Another seed for the dithering effect that combines with first seed to create different patterns")]
	public float glowMagicNumber2 = 0.5f;

	public float glowAnimationSpeed = 1f;

	public Visibility glowVisibility;

	public bool glowBlitDebug;

	[Tooltip("Blends glow passes one after another. If this option is disabled, glow passes won't overlap (in this case, make sure the glow pass 1 has a smaller offset than pass 2, etc.)")]
	public bool glowBlendPasses = true;

	[NonReorderable]
	public GlowPassData[] glowPasses;

	[Tooltip("Select the mask mode used with this effect.")]
	public MaskMode glowMaskMode;

	[Range(0f, 5f)]
	[Tooltip("The intensity of the inner glow effect. A value of 0 disables the glow completely.")]
	public float innerGlow;

	[Range(0f, 2f)]
	public float innerGlowWidth = 1f;

	[ColorUsage(true, true)]
	public Color innerGlowColor = Color.white;

	public InnerGlowBlendMode innerGlowBlendMode;

	public Visibility innerGlowVisibility;

	[Tooltip("Enables the targetFX effect. This effect draws an animated sprite over the object.")]
	public bool targetFX;

	public Texture2D targetFXTexture;

	[ColorUsage(true, true)]
	public Color targetFXColor = Color.white;

	public Transform targetFXCenter;

	public float targetFXRotationSpeed = 50f;

	public float targetFXInitialScale = 4f;

	public float targetFXEndScale = 1.5f;

	[Tooltip("Makes target scale relative to object renderer bounds")]
	public bool targetFXScaleToRenderBounds = true;

	[Tooltip("Enable to render a single target FX effect at the center of the enclosing bounds")]
	public bool targetFXUseEnclosingBounds;

	[Tooltip("Places target FX sprite at the bottom of the highlighted object.")]
	public bool targetFXAlignToGround;

	[Tooltip("Optional worlds space offset for the position of the targetFX effect")]
	public Vector3 targetFXOffset;

	[Tooltip("Fade out effect with altitude")]
	public float targetFXFadePower = 32f;

	public float targetFXGroundMaxDistance = 10f;

	public LayerMask targetFXGroundLayerMask = -1;

	public float targetFXTransitionDuration = 0.5f;

	[Tooltip("The duration of the effect. A value of 0 will keep the target sprite on screen while object is highlighted.")]
	public float targetFXStayDuration = 1.5f;

	public Visibility targetFXVisibility = Visibility.AlwaysOnTop;

	[Tooltip("See-through mode for this Highlight Effect component.")]
	public SeeThroughMode seeThrough = SeeThroughMode.Never;

	[Tooltip("This mask setting let you specify which objects will be considered as occluders and cause the see-through effect for this Highlight Effect component. For example, you assign your walls to a different layer and specify that layer here, so only walls and not other objects, like ground or ceiling, will trigger the see-through effect.")]
	public LayerMask seeThroughOccluderMask = -1;

	[Tooltip("A multiplier for the occluder volume size which can be used to reduce the actual size of occluders when Highlight Effect checks if they're occluding this object.")]
	[Range(0.01f, 0.6f)]
	public float seeThroughOccluderThreshold = 0.3f;

	[Tooltip("Uses stencil buffers to ensure pixel-accurate occlusion test. If this option is disabled, only physics raycasting is used to test for occlusion.")]
	public bool seeThroughOccluderMaskAccurate;

	[Tooltip("The interval of time between occlusion tests.")]
	public float seeThroughOccluderCheckInterval = 1f;

	[Tooltip("If enabled, occlusion test is performed for each children element. If disabled, the bounds of all children is combined and a single occlusion test is performed for the combined bounds.")]
	public bool seeThroughOccluderCheckIndividualObjects;

	[Tooltip("Shows the see-through effect only if the occluder if at this 'offset' distance from the object.")]
	public float seeThroughDepthOffset;

	[Tooltip("Hides the see-through effect if the occluder is further than this distance from the object (0 = infinite)")]
	public float seeThroughMaxDepth;

	[Range(0f, 5f)]
	public float seeThroughIntensity = 0.8f;

	[Range(0f, 1f)]
	public float seeThroughTintAlpha = 0.5f;

	[ColorUsage(true, true)]
	public Color seeThroughTintColor = Color.red;

	[Range(0f, 1f)]
	public float seeThroughNoise = 1f;

	[Range(0f, 1f)]
	public float seeThroughBorder;

	public Color seeThroughBorderColor = Color.black;

	[Tooltip("Only display the border instead of the full see-through effect.")]
	public bool seeThroughBorderOnly;

	public float seeThroughBorderWidth = 0.45f;

	[Tooltip("This option clears the stencil buffer after rendering the see-through effect which results in correct rendering order and supports other stencil-based effects that render afterwards.")]
	public bool seeThroughOrdered;

	[Tooltip("Optional see-through mask effect texture.")]
	public Texture2D seeThroughTexture;

	public TextureUVSpace seeThroughTextureUVSpace;

	public float seeThroughTextureScale = 1f;

	[Tooltip("The order by which children objects are rendered by the see-through effect")]
	public SeeThroughSortingMode seeThroughChildrenSortingMode;

	[SerializeField]
	[HideInInspector]
	private ModelMaterials[] rms;

	[SerializeField]
	[HideInInspector]
	private int rmsCount;

	[NonSerialized]
	public bool isVisible;

	[NonSerialized]
	public Transform target;

	[NonSerialized]
	public float highlightStartTime;

	[NonSerialized]
	public float targetFxStartTime;

	private bool _isSelected;

	[NonSerialized]
	public bool spriteMode;

	[NonSerialized]
	public HighlightProfile previousSettings;

	private const float TAU = 0.70711f;

	private static Material fxMatMask;

	private static Material fxMatSolidColor;

	private static Material fxMatSeeThrough;

	private static Material fxMatSeeThroughBorder;

	private static Material fxMatOverlay;

	private static Material fxMatClearStencil;

	private static Material fxMatSeeThroughMask;

	private Material fxMatGlowTemplate;

	private Material fxMatInnerGlow;

	private Material fxMatOutlineTemplate;

	private Material fxMatTarget;

	private Material fxMatComposeGlow;

	private Material fxMatComposeOutline;

	private Material fxMatBlurGlow;

	private Material fxMatBlurOutline;

	private static Vector4[] offsets;

	private float fadeStartTime;

	[NonSerialized]
	public FadingState fading;

	private CommandBuffer cbHighlight;

	private bool cbHighlightEmpty;

	private int[] mipGlowBuffers;

	private int[] mipOutlineBuffers;

	private static Mesh quadMesh;

	private static Mesh cubeMesh;

	private int sourceRT;

	private Matrix4x4 quadGlowMatrix;

	private Matrix4x4 quadOutlineMatrix;

	private Vector4[] corners;

	private RenderTextureDescriptor sourceDesc;

	private Color debugColor;

	private Color blackColor;

	private Visibility lastOutlineVisibility;

	private bool requireUpdateMaterial;

	[NonSerialized]
	public static List<HighlightEffect> effects = new List<HighlightEffect>();

	public static bool customSorting;

	[NonSerialized]
	public float sortingOffset;

	private bool useSmoothGlow;

	private bool useSmoothOutline;

	private bool useSmoothBlend;

	private bool useGPUInstancing;

	private bool usesReversedZBuffer;

	private bool usesSeeThrough;

	private readonly Dictionary<Camera, PerCameraOcclusionData> perCameraOcclusionData = new Dictionary<Camera, PerCameraOcclusionData>();

	private MaterialPropertyBlock glowPropertyBlock;

	private MaterialPropertyBlock outlinePropertyBlock;

	private static readonly List<Vector4> matDataDirection = new List<Vector4>();

	private static readonly List<Vector4> matDataGlow = new List<Vector4>();

	private static readonly List<Vector4> matDataColor = new List<Vector4>();

	private static Matrix4x4[] matrices;

	private int outlineOffsetsMin;

	private int outlineOffsetsMax;

	private int glowOffsetsMin;

	private int glowOffsetsMax;

	private static CombineInstance[] combineInstances;

	private bool maskRequired;

	private Texture2D outlineGradientTex;

	private Color[] outlineGradientColors;

	public static bool isVREnabled;

	private bool shouldBakeSkinnedMesh;

	public static HighlightEffect lastHighlighted;

	public static HighlightEffect lastSelected;

	[NonSerialized]
	public string lastRegExError;

	private bool isInitialized;

	private RenderTargetIdentifier colorAttachmentBuffer;

	private RenderTargetIdentifier depthAttachmentBuffer;

	private readonly List<Renderer> tempRR = new List<Renderer>();

	private static List<Vector3> vertices;

	private static List<Vector3> normals;

	private static Vector3[] newNormals;

	private static int[] matches;

	private static readonly Dictionary<Vector3, int> vv = new Dictionary<Vector3, int>();

	private static readonly List<Material> rendererSharedMaterials = new List<Material>();

	private static readonly Dictionary<int, Mesh> smoothMeshes = new Dictionary<int, Mesh>();

	private static readonly Dictionary<int, Mesh> reorientedMeshes = new Dictionary<int, Mesh>();

	private static readonly Dictionary<int, Mesh> combinedMeshes = new Dictionary<int, Mesh>();

	private int combinedMeshesHashId;

	private static readonly Dictionary<Mesh, int> sharedMeshUsage = new Dictionary<Mesh, int>();

	private readonly List<Mesh> instancedMeshes = new List<Mesh>();

	private const int MAX_VERTEX_COUNT = 65535;

	public static bool useUnscaledTime;

	[Range(0f, 1f)]
	public float hitFxInitialIntensity;

	public HitFxMode hitFxMode;

	public float hitFxFadeOutDuration = 0.25f;

	[ColorUsage(true, true)]
	public Color hitFxColor = Color.white;

	public float hitFxRadius = 0.5f;

	private float hitInitialIntensity;

	private float hitStartTime;

	private float hitFadeOutDuration;

	private Color hitColor;

	private bool hitActive;

	private Vector3 hitPosition;

	private float hitRadius;

	private static readonly List<HighlightSeeThroughOccluder> occluders = new List<HighlightSeeThroughOccluder>();

	private static readonly Dictionary<Camera, int> occludersFrameCount = new Dictionary<Camera, int>();

	private static Material fxMatSeeThroughOccluder;

	private static Material fxMatDepthWrite;

	private static RaycastHit[] hits;

	private static Collider[] colliders;

	private const int MAX_OCCLUDER_HITS = 50;

	private static RaycastHit[] occluderHits;

	public bool highlighted
	{
		get
		{
			return _highlighted;
		}
		set
		{
			SetHighlighted(value);
		}
	}

	public int includedObjectsCount => rmsCount;

	public bool isSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			if (_isSelected != value)
			{
				_isSelected = value;
				if (_isSelected)
				{
					lastSelected = this;
				}
			}
		}
	}

	public event OnObjectHighlightEvent OnObjectHighlightStart;

	public event OnObjectHighlightEvent OnObjectHighlightEnd;

	public event OnObjectHighlightStateEvent OnObjectHighlightStateChange;

	public event OnRendererHighlightEvent OnRendererHighlightStart;

	public event OnTargetAnimatesEvent OnTargetAnimates;

	public void RestorePreviousHighlightEffectSettings()
	{
		previousSettings.Load(this);
	}

	[RuntimeInitializeOnLoadMethod]
	private static void DomainReloadDisabledSupport()
	{
		lastHighlighted = (lastSelected = null);
		effects.RemoveAll((HighlightEffect i) => i == null);
	}

	private void OnEnable()
	{
		InitIfNeeded();
	}

	private void InitIfNeeded()
	{
		if (rms == null || !isInitialized)
		{
			Init();
		}
		if (!effects.Contains(this))
		{
			effects.Add(this);
		}
		UpdateVisibilityState();
	}

	private void Init()
	{
		lastOutlineVisibility = outlineVisibility;
		debugColor = new Color(1f, 0f, 0f, 0.5f);
		blackColor = new Color(0f, 0f, 0f, 0f);
		if (offsets == null || offsets.Length != 8)
		{
			offsets = new Vector4[8]
			{
				new Vector4(0f, 1f),
				new Vector4(1f, 0f),
				new Vector4(0f, -1f),
				new Vector4(-1f, 0f),
				new Vector4(-0.70711f, 0.70711f),
				new Vector4(0.70711f, 0.70711f),
				new Vector4(0.70711f, -0.70711f),
				new Vector4(-0.70711f, -0.70711f)
			};
		}
		if (corners == null || corners.Length != 8)
		{
			corners = new Vector4[8];
		}
		InitCommandBuffer();
		if (quadMesh == null)
		{
			BuildQuad();
		}
		if (cubeMesh == null)
		{
			BuildCube();
		}
		if (target == null)
		{
			target = base.transform;
		}
		if (glowPasses == null || glowPasses.Length == 0)
		{
			glowPasses = new GlowPassData[4];
			glowPasses[0] = new GlowPassData
			{
				offset = 4f,
				alpha = 0.1f,
				color = new Color(0.64f, 1f, 0f, 1f)
			};
			glowPasses[1] = new GlowPassData
			{
				offset = 3f,
				alpha = 0.2f,
				color = new Color(0.64f, 1f, 0f, 1f)
			};
			glowPasses[2] = new GlowPassData
			{
				offset = 2f,
				alpha = 0.3f,
				color = new Color(0.64f, 1f, 0f, 1f)
			};
			glowPasses[3] = new GlowPassData
			{
				offset = 1f,
				alpha = 0.4f,
				color = new Color(0.64f, 1f, 0f, 1f)
			};
		}
		sourceRT = Shader.PropertyToID("_HPSourceRT");
		useGPUInstancing = GPUInstancing && SystemInfo.supportsInstancing;
		usesReversedZBuffer = SystemInfo.usesReversedZBuffer;
		if (useGPUInstancing)
		{
			if (glowPropertyBlock == null)
			{
				glowPropertyBlock = new MaterialPropertyBlock();
			}
			if (outlinePropertyBlock == null)
			{
				outlinePropertyBlock = new MaterialPropertyBlock();
			}
		}
		CheckGeometrySupportDependencies();
		if (profileSync && profile != null)
		{
			profile.Load(this);
		}
		isInitialized = true;
	}

	private void Start()
	{
		SetupMaterial();
	}

	public void OnDidApplyAnimationProperties()
	{
		UpdateMaterialProperties();
	}

	private void OnDisable()
	{
		UpdateMaterialProperties();
		if (effects != null)
		{
			int k = effects.IndexOf(this);
			if (k >= 0)
			{
				effects.RemoveAt(k);
			}
		}
	}

	private void Reset()
	{
		SetupMaterial();
	}

	private void DestroyMaterial(Material mat)
	{
		if (mat != null)
		{
			global::UnityEngine.Object.DestroyImmediate(mat);
		}
	}

	private void DestroyMaterialArray(Material[] mm)
	{
		if (mm != null)
		{
			for (int k = 0; k < mm.Length; k++)
			{
				DestroyMaterial(mm[k]);
			}
		}
	}

	private void OnDestroy()
	{
		if (rms != null)
		{
			for (int k = 0; k < rms.Length; k++)
			{
				DestroyMaterialArray(rms[k].fxMatMask);
				DestroyMaterialArray(rms[k].fxMatOutline);
				DestroyMaterialArray(rms[k].fxMatGlow);
				DestroyMaterialArray(rms[k].fxMatSolidColor);
				DestroyMaterialArray(rms[k].fxMatSeeThroughInner);
				DestroyMaterialArray(rms[k].fxMatSeeThroughBorder);
				DestroyMaterialArray(rms[k].fxMatOverlay);
				DestroyMaterialArray(rms[k].fxMatInnerGlow);
			}
		}
		DestroyMaterial(fxMatGlowTemplate);
		DestroyMaterial(fxMatInnerGlow);
		DestroyMaterial(fxMatOutlineTemplate);
		DestroyMaterial(fxMatTarget);
		DestroyMaterial(fxMatComposeGlow);
		DestroyMaterial(fxMatComposeOutline);
		DestroyMaterial(fxMatBlurGlow);
		DestroyMaterial(fxMatBlurOutline);
		if (combinedMeshes.ContainsKey(combinedMeshesHashId))
		{
			combinedMeshes.Remove(combinedMeshesHashId);
		}
		foreach (Mesh instancedMesh in instancedMeshes)
		{
			if (!(instancedMesh == null) && sharedMeshUsage.TryGetValue(instancedMesh, out var usageCount))
			{
				if (usageCount <= 1)
				{
					sharedMeshUsage.Remove(instancedMesh);
					global::UnityEngine.Object.DestroyImmediate(instancedMesh);
				}
				else
				{
					sharedMeshUsage[instancedMesh] = usageCount - 1;
				}
			}
		}
	}

	private void OnBecameVisible()
	{
		isVisible = true;
	}

	private void OnBecameInvisible()
	{
		if (rms == null || rms.Length != 1 || rms[0].transform != base.transform)
		{
			isVisible = true;
		}
		else
		{
			isVisible = false;
		}
	}

	public void ProfileLoad(HighlightProfile profile)
	{
		if (profile != null)
		{
			this.profile = profile;
			profile.Load(this);
		}
	}

	public void ProfileReload()
	{
		if (profile != null)
		{
			profile.Load(this);
		}
	}

	public void ProfileSaveChanges(HighlightProfile profile)
	{
		if (profile != null)
		{
			profile.Save(this);
		}
	}

	public void ProfileSaveChanges()
	{
		if (profile != null)
		{
			profile.Save(this);
		}
	}

	public void Refresh(bool discardCachedMeshes = false)
	{
		if (discardCachedMeshes)
		{
			RefreshCachedMeshes();
		}
		InitIfNeeded();
		if (base.enabled)
		{
			SetupMaterial();
		}
	}

	public void SetCommandBuffer(CommandBuffer cmd)
	{
		cbHighlight = cmd;
		cbHighlightEmpty = false;
	}

	public CommandBuffer BuildCommandBuffer(Camera cam, RenderTargetIdentifier colorAttachmentBuffer, RenderTargetIdentifier depthAttachmentBuffer, bool clearStencil)
	{
		this.colorAttachmentBuffer = colorAttachmentBuffer;
		this.depthAttachmentBuffer = depthAttachmentBuffer;
		BuildCommandBuffer(cam, clearStencil);
		if (!cbHighlightEmpty)
		{
			return cbHighlight;
		}
		return null;
	}

	private void BuildCommandBuffer(Camera cam, bool clearStencil)
	{
		if (colorAttachmentBuffer == 0)
		{
			colorAttachmentBuffer = BuiltinRenderTextureType.CameraTarget;
		}
		if (depthAttachmentBuffer == 0)
		{
			depthAttachmentBuffer = BuiltinRenderTextureType.CameraTarget;
		}
		InitCommandBuffer();
		if (requireUpdateMaterial)
		{
			requireUpdateMaterial = false;
			UpdateMaterialProperties();
		}
		bool independentFullScreenNotExecuted = true;
		if (clearStencil)
		{
			ConfigureOutput();
			cbHighlight.DrawMesh(quadMesh, Matrix4x4.identity, fxMatClearStencil, 0, 0);
			independentFullScreenNotExecuted = false;
		}
		bool seeThroughReal = usesSeeThrough;
		if (seeThroughReal)
		{
			ConfigureOutput();
			seeThroughReal = RenderSeeThroughOccluders(cbHighlight, cam);
			if (seeThroughReal && (int)seeThroughOccluderMask != -1)
			{
				if (seeThroughOccluderMaskAccurate)
				{
					CheckOcclusionAccurate(cbHighlight, cam);
				}
				else
				{
					seeThroughReal = CheckOcclusion(cam);
				}
			}
		}
		bool showOverlay = hitActive || overlayMode == OverlayMode.Always;
		if (!_highlighted && !seeThroughReal && !showOverlay)
		{
			return;
		}
		ConfigureOutput();
		if (rms == null)
		{
			SetupMaterial();
			if (rms == null)
			{
				return;
			}
		}
		int cullingMask = cam.cullingMask;
		if (!ignoreObjectVisibility)
		{
			for (int k = 0; k < rmsCount; k++)
			{
				if (rms[k].renderer != null && rms[k].renderer.isVisible != rms[k].renderWasVisibleDuringSetup)
				{
					SetupMaterial();
					break;
				}
			}
		}
		float glowReal = (_highlighted ? glow : 0f);
		if (fxMatMask == null)
		{
			return;
		}
		float now = GetTime();
		Visibility smoothGlowVisibility = glowVisibility;
		Visibility smoothOutlineVisibility = outlineVisibility;
		float aspect = cam.aspect;
		bool somePartVisible = false;
		for (int i = 0; i < rmsCount; i++)
		{
			rms[i].render = false;
			Transform t = rms[i].transform;
			if (t == null)
			{
				continue;
			}
			if (rms[i].isSkinnedMesh && shouldBakeSkinnedMesh)
			{
				SkinnedMeshRenderer obj = (SkinnedMeshRenderer)rms[i].renderer;
				if (rms[i].bakedSkinnedMesh == null)
				{
					rms[i].bakedSkinnedMesh = new Mesh();
				}
				obj.BakeMesh(rms[i].bakedSkinnedMesh, useScale: true);
				rms[i].mesh = rms[i].bakedSkinnedMesh;
				rms[i].normalsOption = NormalsOption.Smooth;
			}
			Mesh mesh = rms[i].mesh;
			if (mesh == null)
			{
				continue;
			}
			if (!ignoreObjectVisibility)
			{
				int layer = t.gameObject.layer;
				if (((1 << layer) & cullingMask) == 0 || !rms[i].renderer.isVisible)
				{
					continue;
				}
			}
			rms[i].render = true;
			somePartVisible = true;
			if (rms[i].isCombined)
			{
				rms[i].renderingMatrix = t.localToWorldMatrix;
			}
			if (!outlineIndependent)
			{
				continue;
			}
			if (useSmoothBlend)
			{
				if (independentFullScreenNotExecuted)
				{
					independentFullScreenNotExecuted = false;
					cbHighlight.DrawMesh(quadMesh, Matrix4x4.identity, fxMatClearStencil, 0, 0);
				}
			}
			else
			{
				if (!(outline > 0f) && !(glow > 0f))
				{
					continue;
				}
				float width = outlineWidth;
				if (glow > 0f)
				{
					width = Mathf.Max(width, glowWidth);
				}
				for (int l = 0; l < mesh.subMeshCount; l++)
				{
					if (((1 << l) & subMeshMask) == 0)
					{
						continue;
					}
					if (outlineQuality.UsesMultipleOffsets())
					{
						for (int o = outlineOffsetsMin; o <= outlineOffsetsMax; o++)
						{
							Vector4 direction = offsets[o] * (width / 100f);
							direction.y *= aspect;
							cbHighlight.SetGlobalVector(ShaderParams.OutlineDirection, direction);
							if (rms[i].isCombined)
							{
								cbHighlight.DrawMesh(rms[i].mesh, rms[i].renderingMatrix, rms[i].fxMatOutline[l], l, 1);
							}
							else
							{
								cbHighlight.DrawRenderer(rms[i].renderer, rms[i].fxMatOutline[l], l, 1);
							}
						}
					}
					else
					{
						cbHighlight.SetGlobalVector(ShaderParams.OutlineDirection, Vector4.zero);
						if (rms[i].isCombined)
						{
							cbHighlight.DrawMesh(rms[i].mesh, rms[i].renderingMatrix, rms[i].fxMatOutline[l], l, 1);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[i].renderer, rms[i].fxMatOutline[l], l, 1);
						}
					}
				}
			}
		}
		bool renderMaskOnTop = _highlighted && ((outline > 0f && smoothOutlineVisibility != 0) || (glow > 0f && smoothGlowVisibility != 0) || (this.innerGlow > 0f && innerGlowVisibility != Visibility.Normal));
		renderMaskOnTop |= useSmoothBlend && outlineContourStyle == ContourStyle.AroundObjectShape;
		if (maskRequired)
		{
			for (int j = 0; j < rmsCount; j++)
			{
				if (rms[j].render)
				{
					RenderMask(j, rms[j].mesh, renderMaskOnTop);
				}
			}
		}
		float fadeGroup = 1f;
		float fade = 1f;
		if (fading != 0)
		{
			if (fading == FadingState.FadingIn)
			{
				if (fadeInDuration > 0f)
				{
					fadeGroup = (now - fadeStartTime) / fadeInDuration;
					if (fadeGroup > 1f)
					{
						fadeGroup = 1f;
						fading = FadingState.NoFading;
					}
				}
			}
			else if (fadeOutDuration > 0f)
			{
				fadeGroup = 1f - (now - fadeStartTime) / fadeOutDuration;
				if (fadeGroup < 0f)
				{
					fadeGroup = 0f;
					fading = FadingState.NoFading;
					_highlighted = false;
					requireUpdateMaterial = true;
					if (this.OnObjectHighlightEnd != null)
					{
						this.OnObjectHighlightEnd(base.gameObject);
					}
					SendMessage("HighlightEnd", null, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		if (glowQuality == QualityLevel.High)
		{
			glowReal *= 0.25f;
		}
		else if (glowQuality == QualityLevel.Medium)
		{
			glowReal *= 0.5f;
		}
		bool targetEffectRendered = false;
		bool usesSeeThroughBorder = seeThroughBorder * seeThroughBorderWidth > 0f;
		Bounds enclosingBounds = default(Bounds);
		if (useSmoothBlend || (targetFX && targetFXUseEnclosingBounds))
		{
			for (int m = 0; m < rmsCount; m++)
			{
				if (rms[m].render)
				{
					if (m == 0)
					{
						enclosingBounds = rms[m].renderer.bounds;
					}
					else
					{
						enclosingBounds.Encapsulate(rms[m].renderer.bounds);
					}
				}
			}
		}
		for (int n = 0; n < rmsCount; n++)
		{
			if (!rms[n].render)
			{
				continue;
			}
			Mesh mesh2 = rms[n].mesh;
			fade = fadeGroup;
			if (cameraDistanceFade)
			{
				fade *= ComputeCameraDistanceFade(rms[n].transform.position, cam.transform);
			}
			cbHighlight.SetGlobalFloat(ShaderParams.FadeFactor, fade);
			if (_highlighted || showOverlay)
			{
				Color overlayColor = this.overlayColor;
				float overlayMinIntensity = this.overlayMinIntensity;
				float overlayBlending = this.overlayBlending;
				Color innerGlowColorA = innerGlowColor;
				float innerGlow = this.innerGlow;
				if (hitActive)
				{
					overlayColor.a = (_highlighted ? overlay : 0f);
					innerGlowColorA.a = (_highlighted ? innerGlow : 0f);
					float t2 = ((hitFadeOutDuration > 0f) ? ((now - hitStartTime) / hitFadeOutDuration) : 1f);
					if (t2 >= 1f)
					{
						hitActive = false;
					}
					else if (hitFxMode == HitFxMode.InnerGlow)
					{
						bool lerpToCurrentInnerGlow = _highlighted && innerGlow > 0f;
						innerGlowColorA = (lerpToCurrentInnerGlow ? Color.Lerp(hitColor, innerGlowColor, t2) : hitColor);
						innerGlowColorA.a = (lerpToCurrentInnerGlow ? Mathf.Lerp(1f - t2, innerGlow, t2) : (1f - t2));
						innerGlowColorA.a *= hitInitialIntensity;
					}
					else
					{
						bool lerpToCurrentOverlay = _highlighted && overlay > 0f;
						overlayColor = (lerpToCurrentOverlay ? Color.Lerp(hitColor, overlayColor, t2) : hitColor);
						overlayColor.a = (lerpToCurrentOverlay ? Mathf.Lerp(1f - t2, overlay, t2) : (1f - t2));
						overlayColor.a *= hitInitialIntensity;
						overlayMinIntensity = 1f;
						overlayBlending = 0f;
					}
				}
				else
				{
					overlayColor.a = overlay * fade;
					innerGlowColorA.a = innerGlow * fade;
				}
				for (int num = 0; num < mesh2.subMeshCount; num++)
				{
					if (((1 << num) & subMeshMask) == 0)
					{
						continue;
					}
					if (overlayColor.a > 0f)
					{
						Material fxMat = rms[n].fxMatOverlay[num];
						fxMat.SetColor(ShaderParams.OverlayColor, overlayColor);
						fxMat.SetVector(ShaderParams.OverlayData, new Vector4(overlayAnimationSpeed, overlayMinIntensity, overlayBlending, overlayTextureScale));
						if (hitActive && hitFxMode == HitFxMode.LocalHit)
						{
							fxMat.SetVector(ShaderParams.OverlayHitPosData, new Vector4(hitPosition.x, hitPosition.y, hitPosition.z, hitRadius));
							fxMat.SetFloat(ShaderParams.OverlayHitStartTime, hitStartTime);
						}
						else
						{
							fxMat.SetVector(ShaderParams.OverlayHitPosData, Vector4.zero);
						}
						if (rms[n].isCombined)
						{
							cbHighlight.DrawMesh(rms[n].mesh, rms[n].renderingMatrix, rms[n].fxMatOverlay[num], num);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[n].renderer, rms[n].fxMatOverlay[num], num);
						}
					}
					if (innerGlowColorA.a > 0f)
					{
						rms[n].fxMatInnerGlow[num].SetColor(ShaderParams.InnerGlowColor, innerGlowColorA);
						if (rms[n].isCombined)
						{
							cbHighlight.DrawMesh(rms[n].mesh, rms[n].renderingMatrix, rms[n].fxMatInnerGlow[num], num);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[n].renderer, rms[n].fxMatInnerGlow[num], num);
						}
					}
				}
			}
			if (!_highlighted)
			{
				continue;
			}
			bool allowGPUInstancing = useGPUInstancing && (shouldBakeSkinnedMesh || !rms[n].isSkinnedMesh);
			for (int num2 = 0; num2 < mesh2.subMeshCount; num2++)
			{
				if (((1 << num2) & subMeshMask) == 0)
				{
					continue;
				}
				if (glow > 0f && glowQuality != QualityLevel.Highest)
				{
					matDataGlow.Clear();
					matDataColor.Clear();
					matDataDirection.Clear();
					for (int glowPass = 0; glowPass < glowPasses.Length; glowPass++)
					{
						if (glowQuality.UsesMultipleOffsets())
						{
							for (int num3 = glowOffsetsMin; num3 <= glowOffsetsMax; num3++)
							{
								Vector4 direction2 = offsets[num3];
								direction2.y *= aspect;
								Color dataColor = glowPasses[glowPass].color;
								Vector4 dataGlow = new Vector4(fade * glowReal * glowPasses[glowPass].alpha, glowPasses[glowPass].offset * glowWidth / 100f, glowMagicNumber1, glowMagicNumber2);
								if (allowGPUInstancing)
								{
									matDataDirection.Add(direction2);
									matDataGlow.Add(dataGlow);
									matDataColor.Add(new Vector4(dataColor.r, dataColor.g, dataColor.b, dataColor.a));
									continue;
								}
								cbHighlight.SetGlobalVector(ShaderParams.GlowDirection, direction2);
								cbHighlight.SetGlobalColor(ShaderParams.GlowColor, dataColor);
								cbHighlight.SetGlobalVector(ShaderParams.Glow, dataGlow);
								if (rms[n].isCombined)
								{
									cbHighlight.DrawMesh(rms[n].mesh, rms[n].renderingMatrix, rms[n].fxMatGlow[num2], num2);
								}
								else
								{
									cbHighlight.DrawRenderer(rms[n].renderer, rms[n].fxMatGlow[num2], num2);
								}
							}
							continue;
						}
						Vector4 dataGlow2 = new Vector4(fade * glowReal * glowPasses[glowPass].alpha, glowPasses[glowPass].offset * glowWidth / 100f, glowMagicNumber1, glowMagicNumber2);
						Color dataColor2 = glowPasses[glowPass].color;
						if (allowGPUInstancing)
						{
							matDataDirection.Add(Vector4.zero);
							matDataGlow.Add(dataGlow2);
							matDataColor.Add(new Vector4(dataColor2.r, dataColor2.g, dataColor2.b, dataColor2.a));
							continue;
						}
						cbHighlight.SetGlobalColor(ShaderParams.GlowColor, dataColor2);
						cbHighlight.SetGlobalVector(ShaderParams.Glow, dataGlow2);
						cbHighlight.SetGlobalVector(ShaderParams.GlowDirection, Vector4.zero);
						if (rms[n].isCombined)
						{
							cbHighlight.DrawMesh(rms[n].mesh, rms[n].renderingMatrix, rms[n].fxMatGlow[num2], num2);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[n].renderer, rms[n].fxMatGlow[num2], num2);
						}
					}
					if (allowGPUInstancing)
					{
						int instanceCount = matDataDirection.Count;
						if (instanceCount > 0)
						{
							glowPropertyBlock.Clear();
							glowPropertyBlock.SetVectorArray(ShaderParams.GlowDirection, matDataDirection);
							glowPropertyBlock.SetVectorArray(ShaderParams.GlowColor, matDataColor);
							glowPropertyBlock.SetVectorArray(ShaderParams.Glow, matDataGlow);
							if (matrices == null || matrices.Length < instanceCount)
							{
								matrices = new Matrix4x4[instanceCount];
							}
							if (rms[n].isCombined)
							{
								for (int num4 = 0; num4 < instanceCount; num4++)
								{
									matrices[num4] = rms[n].renderingMatrix;
								}
							}
							else
							{
								Matrix4x4 objectToWorld = rms[n].transform.localToWorldMatrix;
								for (int num5 = 0; num5 < instanceCount; num5++)
								{
									matrices[num5] = objectToWorld;
								}
							}
							cbHighlight.DrawMeshInstanced(mesh2, num2, rms[n].fxMatGlow[num2], 0, matrices, instanceCount, glowPropertyBlock);
						}
					}
				}
				if (!(outline > 0f) || outlineQuality == QualityLevel.Highest)
				{
					continue;
				}
				Color outlineColor = this.outlineColor;
				if (outlineColorStyle == ColorStyle.Gradient)
				{
					outlineColor.a *= outline * fade;
					Bounds bounds = (outlineGradientInLocalSpace ? mesh2.bounds : rms[n].renderer.bounds);
					cbHighlight.SetGlobalVector(ShaderParams.OutlineVertexData, new Vector4(bounds.min.y, bounds.size.y + 0.0001f, 0f, 0f));
				}
				else
				{
					outlineColor.a = outline * fade;
					cbHighlight.SetGlobalVector(ShaderParams.OutlineVertexData, new Vector4(-1000000f, 1f, 0f, 0f));
				}
				cbHighlight.SetGlobalColor(ShaderParams.OutlineColor, outlineColor);
				if (outlineQuality.UsesMultipleOffsets())
				{
					matDataDirection.Clear();
					for (int num6 = outlineOffsetsMin; num6 <= outlineOffsetsMax; num6++)
					{
						Vector4 direction3 = offsets[num6] * (outlineWidth / 100f);
						direction3.y *= aspect;
						if (allowGPUInstancing)
						{
							matDataDirection.Add(direction3);
							continue;
						}
						cbHighlight.SetGlobalVector(ShaderParams.OutlineDirection, direction3);
						if (rms[n].isCombined)
						{
							cbHighlight.DrawMesh(rms[n].mesh, rms[n].renderingMatrix, rms[n].fxMatOutline[num2], num2, 0);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[n].renderer, rms[n].fxMatOutline[num2], num2, 0);
						}
					}
					if (!allowGPUInstancing)
					{
						continue;
					}
					int instanceCount2 = matDataDirection.Count;
					if (instanceCount2 <= 0)
					{
						continue;
					}
					outlinePropertyBlock.Clear();
					outlinePropertyBlock.SetVectorArray(ShaderParams.OutlineDirection, matDataDirection);
					if (matrices == null || matrices.Length < instanceCount2)
					{
						matrices = new Matrix4x4[instanceCount2];
					}
					if (rms[n].isCombined)
					{
						for (int num7 = 0; num7 < instanceCount2; num7++)
						{
							matrices[num7] = rms[n].renderingMatrix;
						}
					}
					else
					{
						Matrix4x4 objectToWorld2 = rms[n].transform.localToWorldMatrix;
						for (int num8 = 0; num8 < instanceCount2; num8++)
						{
							matrices[num8] = objectToWorld2;
						}
					}
					cbHighlight.DrawMeshInstanced(mesh2, num2, rms[n].fxMatOutline[num2], 0, matrices, instanceCount2, outlinePropertyBlock);
				}
				else
				{
					cbHighlight.SetGlobalColor(ShaderParams.OutlineColor, outlineColor);
					cbHighlight.SetGlobalVector(ShaderParams.OutlineDirection, Vector4.zero);
					if (rms[n].isSkinnedMesh)
					{
						cbHighlight.DrawRenderer(rms[n].renderer, rms[n].fxMatOutline[num2], num2, 0);
					}
					else
					{
						cbHighlight.DrawMesh(mesh2, rms[n].transform.localToWorldMatrix, rms[n].fxMatOutline[num2], num2, 0);
					}
				}
			}
			if (!targetFX)
			{
				continue;
			}
			float fadeOut = 1f;
			if (targetFXStayDuration > 0f && Application.isPlaying)
			{
				fadeOut = now - targetFxStartTime;
				if (fadeOut >= targetFXStayDuration)
				{
					fadeOut -= targetFXStayDuration;
					fadeOut = 1f - fadeOut;
				}
				if (fadeOut > 1f)
				{
					fadeOut = 1f;
				}
			}
			bool usesTarget = targetFXCenter != null;
			if (!(fadeOut > 0f) || (targetEffectRendered && (usesTarget || targetFXUseEnclosingBounds)))
			{
				continue;
			}
			targetEffectRendered = true;
			float scaleT = 1f;
			float normalizedTime = 0f;
			float time;
			if (Application.isPlaying)
			{
				normalizedTime = (now - targetFxStartTime) / targetFXTransitionDuration;
				if (normalizedTime > 1f)
				{
					normalizedTime = 1f;
				}
				scaleT = Mathf.Sin(normalizedTime * MathF.PI * 0.5f);
				time = now;
			}
			else
			{
				time = (float)DateTime.Now.Subtract(DateTime.Today).TotalSeconds;
			}
			Bounds bounds2 = (targetFXUseEnclosingBounds ? enclosingBounds : rms[n].renderer.bounds);
			if (!targetFXScaleToRenderBounds)
			{
				bounds2.size = Vector3.one;
			}
			Vector3 scale = bounds2.size;
			float minSize = scale.x;
			if (scale.y < minSize)
			{
				minSize = scale.y;
			}
			if (scale.z < minSize)
			{
				minSize = scale.z;
			}
			scale.x = (scale.y = (scale.z = minSize));
			scale = Vector3.Lerp(scale * targetFXInitialScale, scale * targetFXEndScale, scaleT);
			Vector3 center = (usesTarget ? targetFXCenter.position : bounds2.center);
			center += targetFXOffset;
			if (targetFXAlignToGround)
			{
				Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
				center.y += 0.5f;
				if (Physics.Raycast(center, Vector3.down, out var groundHitInfo, targetFXGroundMaxDistance, targetFXGroundLayerMask))
				{
					center = groundHitInfo.point;
					center.y += 0.01f;
					Vector4 renderData = groundHitInfo.normal;
					renderData.w = targetFXFadePower;
					fxMatTarget.SetVector(ShaderParams.TargetFXRenderData, renderData);
					rotation = Quaternion.Euler(0f, time * targetFXRotationSpeed, 0f);
					if (this.OnTargetAnimates != null)
					{
						this.OnTargetAnimates(ref center, ref rotation, ref scale, normalizedTime);
					}
					Matrix4x4 m2 = Matrix4x4.TRS(center, rotation, scale);
					Color color = targetFXColor;
					color.a *= fade * fadeOut;
					fxMatTarget.color = color;
					cbHighlight.DrawMesh(cubeMesh, m2, fxMatTarget, 0, 0);
				}
			}
			else
			{
				Quaternion rotation = Quaternion.LookRotation(cam.transform.position - rms[n].transform.position);
				Quaternion animationRot = Quaternion.Euler(0f, 0f, time * targetFXRotationSpeed);
				rotation *= animationRot;
				if (this.OnTargetAnimates != null)
				{
					this.OnTargetAnimates(ref center, ref rotation, ref scale, normalizedTime);
				}
				Matrix4x4 m3 = Matrix4x4.TRS(center, rotation, scale);
				Color color2 = targetFXColor;
				color2.a *= fade * fadeOut;
				fxMatTarget.color = color2;
				cbHighlight.DrawMesh(quadMesh, m3, fxMatTarget, 0, 1);
			}
		}
		if (useSmoothBlend && _highlighted && somePartVisible)
		{
			int smoothRTWidth = cam.pixelWidth;
			if (smoothRTWidth <= 0)
			{
				smoothRTWidth = 1;
			}
			int smoothRTHeight = cam.pixelHeight;
			if (smoothRTHeight <= 0)
			{
				smoothRTHeight = 1;
			}
			sourceDesc = new RenderTextureDescriptor(smoothRTWidth, smoothRTHeight);
			sourceDesc.volumeDepth = 1;
			sourceDesc.colorFormat = ((!useSmoothOutline || outlineEdgeMode != OutlineEdgeMode.Any) ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
			sourceDesc.msaaSamples = 1;
			sourceDesc.useMipMap = false;
			sourceDesc.depthBufferBits = 0;
			cbHighlight.GetTemporaryRT(sourceRT, sourceDesc, FilterMode.Bilinear);
			RenderTargetIdentifier sourceTarget = new RenderTargetIdentifier(sourceRT, 0, CubemapFace.Unknown, -1);
			cbHighlight.SetRenderTarget(sourceTarget);
			cbHighlight.ClearRenderTarget(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 0f));
			for (int num9 = 0; num9 < rmsCount; num9++)
			{
				if (!rms[num9].render)
				{
					continue;
				}
				Mesh mesh3 = rms[num9].mesh;
				for (int num10 = 0; num10 < mesh3.subMeshCount; num10++)
				{
					if (((1 << num10) & subMeshMask) != 0 && num10 < rms[num9].fxMatSolidColor.Length)
					{
						if (rms[num9].isCombined)
						{
							cbHighlight.DrawMesh(rms[num9].mesh, rms[num9].renderingMatrix, rms[num9].fxMatSolidColor[num10], num10);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[num9].renderer, rms[num9].fxMatSolidColor[num10], num10);
						}
					}
				}
			}
			if (ComputeSmoothQuadMatrix(cam, enclosingBounds))
			{
				if (useSmoothGlow)
				{
					float intensity = glow * fade;
					fxMatComposeGlow.color = new Color(glowHQColor.r * intensity, glowHQColor.g * intensity, glowHQColor.b * intensity, glowHQColor.a * intensity);
					SmoothGlow(smoothRTWidth / glowDownsampling, smoothRTHeight / glowDownsampling);
				}
				if (useSmoothOutline)
				{
					float intensity2 = outline * fade;
					fxMatComposeOutline.color = new Color(this.outlineColor.r, this.outlineColor.g, this.outlineColor.b, this.outlineColor.a * intensity2 * 10f);
					SmoothOutline(smoothRTWidth / outlineDownsampling, smoothRTHeight / outlineDownsampling);
				}
				ComposeSmoothBlend(smoothGlowVisibility, smoothOutlineVisibility);
			}
		}
		if (!seeThroughReal)
		{
			return;
		}
		if (renderMaskOnTop)
		{
			for (int num11 = 0; num11 < rmsCount; num11++)
			{
				if (rms[num11].render)
				{
					Mesh mesh4 = rms[num11].mesh;
					RenderSeeThroughClearStencil(num11, mesh4);
				}
			}
			for (int num12 = 0; num12 < rmsCount; num12++)
			{
				if (rms[num12].render)
				{
					Mesh mesh5 = rms[num12].mesh;
					RenderSeeThroughMask(num12, mesh5);
				}
			}
		}
		for (int num13 = 0; num13 < rmsCount; num13++)
		{
			if (!rms[num13].render)
			{
				continue;
			}
			Mesh mesh6 = rms[num13].mesh;
			for (int num14 = 0; num14 < mesh6.subMeshCount; num14++)
			{
				if (((1 << num14) & subMeshMask) != 0 && num14 < rms[num13].fxMatSeeThroughInner.Length && rms[num13].fxMatSeeThroughInner[num14] != null)
				{
					if (rms[num13].isCombined)
					{
						cbHighlight.DrawMesh(mesh6, rms[num13].renderingMatrix, rms[num13].fxMatSeeThroughInner[num14], num14);
					}
					else
					{
						cbHighlight.DrawRenderer(rms[num13].renderer, rms[num13].fxMatSeeThroughInner[num14], num14);
					}
				}
			}
		}
		if (usesSeeThroughBorder)
		{
			for (int num15 = 0; num15 < rmsCount; num15++)
			{
				if (!rms[num15].render)
				{
					continue;
				}
				Mesh mesh7 = rms[num15].mesh;
				for (int num16 = 0; num16 < mesh7.subMeshCount; num16++)
				{
					if (((1 << num16) & subMeshMask) != 0)
					{
						if (rms[num15].isCombined)
						{
							cbHighlight.DrawMesh(mesh7, rms[num15].renderingMatrix, rms[num15].fxMatSeeThroughBorder[num16], num16);
						}
						else
						{
							cbHighlight.DrawRenderer(rms[num15].renderer, rms[num15].fxMatSeeThroughBorder[num16], num16);
						}
					}
				}
			}
		}
		if (!seeThroughOrdered)
		{
			return;
		}
		for (int num17 = 0; num17 < rmsCount; num17++)
		{
			if (!rms[num17].render)
			{
				continue;
			}
			Mesh mesh8 = rms[num17].mesh;
			for (int num18 = 0; num18 < mesh8.subMeshCount; num18++)
			{
				if (((1 << num18) & subMeshMask) != 0)
				{
					if (rms[num17].isCombined)
					{
						cbHighlight.DrawMesh(mesh8, rms[num17].renderingMatrix, fxMatClearStencil, num18, 1);
					}
					else
					{
						cbHighlight.DrawRenderer(rms[num17].renderer, fxMatClearStencil, num18, 1);
					}
				}
			}
		}
	}

	private void RenderMask(int k, Mesh mesh, bool renderMaskOnTop)
	{
		for (int l = 0; l < mesh.subMeshCount; l++)
		{
			if (((1 << l) & subMeshMask) != 0)
			{
				if (renderMaskOnTop)
				{
					rms[k].fxMatMask[l].SetInt(ShaderParams.ZTest, 8);
				}
				else
				{
					rms[k].fxMatMask[l].SetInt(ShaderParams.ZTest, 4);
				}
				if (rms[k].isCombined)
				{
					cbHighlight.DrawMesh(rms[k].mesh, rms[k].renderingMatrix, rms[k].fxMatMask[l], l, 0);
				}
				else
				{
					cbHighlight.DrawRenderer(rms[k].renderer, rms[k].fxMatMask[l], l, 0);
				}
			}
		}
	}

	private void RenderSeeThroughClearStencil(int k, Mesh mesh)
	{
		if (rms[k].isCombined)
		{
			for (int l = 0; l < mesh.subMeshCount; l++)
			{
				if (((1 << l) & subMeshMask) != 0)
				{
					cbHighlight.DrawMesh(mesh, rms[k].renderingMatrix, fxMatClearStencil, l, 1);
				}
			}
			return;
		}
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			if (((1 << i) & subMeshMask) != 0)
			{
				cbHighlight.DrawRenderer(rms[k].renderer, fxMatClearStencil, i, 1);
			}
		}
	}

	private void RenderSeeThroughMask(int k, Mesh mesh)
	{
		if (rms[k].isCombined)
		{
			for (int l = 0; l < mesh.subMeshCount; l++)
			{
				if (((1 << l) & subMeshMask) != 0)
				{
					cbHighlight.DrawMesh(mesh, rms[k].renderingMatrix, rms[k].fxMatMask[l], l, 1);
				}
			}
			return;
		}
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			if (((1 << i) & subMeshMask) != 0)
			{
				cbHighlight.DrawRenderer(rms[k].renderer, rms[k].fxMatMask[i], i, 1);
			}
		}
	}

	private void WorldToViewportPoint(ref Matrix4x4 m, ref Vector4 p, bool perspectiveProjection, float zBufferParamsZ, float zBufferParamsW)
	{
		p = m * p;
		p.x = (p.x / p.w + 1f) * 0.5f;
		p.y = (p.y / p.w + 1f) * 0.5f;
		if (perspectiveProjection)
		{
			p.z /= p.w;
			p.z = 1f / (zBufferParamsZ * p.z + zBufferParamsW);
			return;
		}
		if (usesReversedZBuffer)
		{
			p.z = 1f - p.z;
		}
		p.z = (zBufferParamsW - zBufferParamsZ) * p.z + zBufferParamsZ;
	}

	private bool ComputeSmoothQuadMatrix(Camera cam, Bounds bounds)
	{
		Vector3 shift = cam.transform.position;
		cam.transform.position = Vector3.zero;
		cam.ResetWorldToCameraMatrix();
		bounds.center -= shift;
		bool result = ComputeSmoothQuadMatrixOriginShifted(cam, ref bounds, ref shift);
		cam.transform.position = shift;
		return result;
	}

	private bool ComputeSmoothQuadMatrixOriginShifted(Camera cam, ref Bounds bounds, ref Vector3 shift)
	{
		Matrix4x4 mat = GL.GetGPUProjectionMatrix(cam.projectionMatrix, renderIntoTexture: false) * cam.worldToCameraMatrix;
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		corners[0] = new Vector4(min.x, min.y, min.z, 1f);
		corners[1] = new Vector4(min.x, min.y, max.z, 1f);
		corners[2] = new Vector4(max.x, min.y, min.z, 1f);
		corners[3] = new Vector4(max.x, min.y, max.z, 1f);
		corners[4] = new Vector4(min.x, max.y, min.z, 1f);
		corners[5] = new Vector4(min.x, max.y, max.z, 1f);
		corners[6] = new Vector4(max.x, max.y, min.z, 1f);
		corners[7] = new Vector4(max.x, max.y, max.z, 1f);
		Vector3 scrMin = new Vector3(float.MaxValue, float.MaxValue, 0f);
		Vector3 scrMax = new Vector3(float.MinValue, float.MinValue, 0f);
		float distanceMin = float.MaxValue;
		float distanceMax = float.MinValue;
		float nearClipPlane = cam.nearClipPlane;
		float farClipPlane = cam.farClipPlane;
		bool isPerspectiveCamera = !cam.orthographic;
		float z;
		float w;
		if (isPerspectiveCamera)
		{
			if (usesReversedZBuffer)
			{
				float num = -1f + farClipPlane / nearClipPlane;
				float y = 1f;
				z = num / farClipPlane;
				w = 1f / farClipPlane;
			}
			else
			{
				float num2 = 1f - farClipPlane / nearClipPlane;
				float y = farClipPlane / nearClipPlane;
				z = num2 / farClipPlane;
				w = y / farClipPlane;
			}
		}
		else
		{
			z = nearClipPlane;
			w = farClipPlane;
		}
		for (int k = 0; k < 8; k++)
		{
			WorldToViewportPoint(ref mat, ref corners[k], isPerspectiveCamera, z, w);
			if (corners[k].x < scrMin.x)
			{
				scrMin.x = corners[k].x;
			}
			if (corners[k].y < scrMin.y)
			{
				scrMin.y = corners[k].y;
			}
			if (corners[k].x > scrMax.x)
			{
				scrMax.x = corners[k].x;
			}
			if (corners[k].y > scrMax.y)
			{
				scrMax.y = corners[k].y;
			}
			if (corners[k].z < distanceMin)
			{
				distanceMin = corners[k].z;
				if (distanceMin < nearClipPlane)
				{
					distanceMin = (distanceMax = 0.01f + nearClipPlane);
					scrMin.x = (scrMin.y = 0f);
					scrMax.x = 1f;
					scrMax.y = 1f;
					break;
				}
			}
			if (corners[k].z > distanceMax)
			{
				distanceMax = corners[k].z;
			}
		}
		if (scrMax.y == scrMin.y)
		{
			return false;
		}
		int pixelWidth = cam.pixelWidth;
		int pixelHeight = cam.pixelHeight;
		scrMin.x *= pixelWidth;
		scrMax.x *= pixelWidth;
		scrMin.y *= pixelHeight;
		scrMax.y *= pixelHeight;
		scrMin.x += cam.pixelRect.xMin;
		scrMax.x += cam.pixelRect.xMin;
		scrMin.y += cam.pixelRect.yMin;
		scrMax.y += cam.pixelRect.yMin;
		if (spriteMode)
		{
			scrMin.z = (scrMax.z = (distanceMin + distanceMax) * 0.5f + nearClipPlane);
		}
		else
		{
			scrMin.z = (scrMax.z = (isVREnabled ? distanceMin : (0.05f + nearClipPlane)));
		}
		if (outline > 0f)
		{
			BuildMatrix(cam, scrMin, scrMax, (int)(10f + 20f * outlineWidth + (float)(5 * outlineDownsampling)), ref quadOutlineMatrix, ref shift);
		}
		if (glow > 0f)
		{
			BuildMatrix(cam, scrMin, scrMax, (int)(20f + 30f * glowWidth + (float)(10 * glowDownsampling)), ref quadGlowMatrix, ref shift);
		}
		return true;
	}

	private void BuildMatrix(Camera cam, Vector3 scrMin, Vector3 scrMax, int border, ref Matrix4x4 quadMatrix, ref Vector3 shift)
	{
		scrMin.x -= border;
		scrMin.y -= border;
		scrMax.x += border;
		scrMax.y += border;
		Vector3 third = new Vector3(scrMax.x, scrMin.y, scrMin.z);
		scrMin = cam.ScreenToWorldPoint(scrMin);
		scrMax = cam.ScreenToWorldPoint(scrMax);
		third = cam.ScreenToWorldPoint(third);
		float width = Vector3.Distance(scrMin, third);
		float height = Vector3.Distance(scrMax, third);
		quadMatrix = Matrix4x4.TRS((scrMin + scrMax) * 0.5f + shift, cam.transform.rotation, new Vector3(width, height, 1f));
	}

	private void SmoothGlow(int rtWidth, int rtHeight)
	{
		Material matBlur = fxMatBlurGlow;
		RenderTextureDescriptor glowDesc = sourceDesc;
		glowDesc.depthBufferBits = 0;
		if (glowBlurMethod == BlurMethod.Gaussian)
		{
			int bufferCount = 8;
			if (mipGlowBuffers == null || mipGlowBuffers.Length != bufferCount)
			{
				mipGlowBuffers = new int[bufferCount];
				for (int k = 0; k < bufferCount; k++)
				{
					mipGlowBuffers[k] = Shader.PropertyToID("_HPSmoothGlowTemp" + k);
				}
				mipGlowBuffers[bufferCount - 2] = ShaderParams.GlowRT;
			}
			for (int i = 0; i < bufferCount; i++)
			{
				float reduction = i / 2 + 2;
				int reducedWidth = (int)((float)rtWidth / reduction);
				int reducedHeight = (int)((float)rtHeight / reduction);
				if (reducedWidth <= 0)
				{
					reducedWidth = 1;
				}
				if (reducedHeight <= 0)
				{
					reducedHeight = 1;
				}
				glowDesc.width = reducedWidth;
				glowDesc.height = reducedHeight;
				cbHighlight.GetTemporaryRT(mipGlowBuffers[i], glowDesc, FilterMode.Bilinear);
			}
			for (int j = 0; j < bufferCount - 1; j += 2)
			{
				if (j == 0)
				{
					RenderingUtils.FullScreenBlit(cbHighlight, sourceRT, mipGlowBuffers[j + 1], fxMatBlurGlow, 0);
				}
				else
				{
					RenderingUtils.FullScreenBlit(cbHighlight, mipGlowBuffers[j], mipGlowBuffers[j + 1], fxMatBlurGlow, 0);
				}
				RenderingUtils.FullScreenBlit(cbHighlight, mipGlowBuffers[j + 1], mipGlowBuffers[j], fxMatBlurGlow, 1);
				if (j < bufferCount - 2)
				{
					RenderingUtils.FullScreenBlit(cbHighlight, mipGlowBuffers[j], mipGlowBuffers[j + 2], fxMatBlurGlow, 2);
				}
			}
			return;
		}
		int bufferCount2 = 4;
		if (mipGlowBuffers == null || mipGlowBuffers.Length != bufferCount2)
		{
			mipGlowBuffers = new int[bufferCount2];
			for (int l = 0; l < bufferCount2 - 1; l++)
			{
				mipGlowBuffers[l] = Shader.PropertyToID("_HPSmoothGlowTemp" + l);
			}
			mipGlowBuffers[bufferCount2 - 1] = ShaderParams.GlowRT;
		}
		for (int m = 0; m < bufferCount2; m++)
		{
			float reduction2 = m + 2;
			int reducedWidth2 = (int)((float)rtWidth / reduction2);
			int reducedHeight2 = (int)((float)rtHeight / reduction2);
			if (reducedWidth2 <= 0)
			{
				reducedWidth2 = 1;
			}
			if (reducedHeight2 <= 0)
			{
				reducedHeight2 = 1;
			}
			glowDesc.width = reducedWidth2;
			glowDesc.height = reducedHeight2;
			cbHighlight.GetTemporaryRT(mipGlowBuffers[m], glowDesc, FilterMode.Bilinear);
		}
		RenderingUtils.FullScreenBlit(cbHighlight, sourceRT, mipGlowBuffers[0], matBlur, 3);
		for (int n = 0; n < bufferCount2 - 1; n++)
		{
			cbHighlight.SetGlobalFloat(ShaderParams.ResampleScale, (float)n + 0.5f);
			RenderingUtils.FullScreenBlit(cbHighlight, mipGlowBuffers[n], mipGlowBuffers[n + 1], matBlur, 3);
		}
	}

	private void SmoothOutline(int rtWidth, int rtHeight)
	{
		int bufferCount = outlineBlurPasses * 2;
		if (mipOutlineBuffers == null || mipOutlineBuffers.Length != bufferCount)
		{
			mipOutlineBuffers = new int[bufferCount];
			for (int k = 0; k < bufferCount; k++)
			{
				mipOutlineBuffers[k] = Shader.PropertyToID("_HPSmoothOutlineTemp" + k);
			}
			mipOutlineBuffers[bufferCount - 2] = ShaderParams.OutlineRT;
		}
		RenderTextureDescriptor outlineDesc = sourceDesc;
		outlineDesc.depthBufferBits = 0;
		for (int i = 0; i < bufferCount; i++)
		{
			float reduction = i / 2 + 2;
			int reducedWidth = (int)((float)rtWidth / reduction);
			int reducedHeight = (int)((float)rtHeight / reduction);
			if (reducedWidth <= 0)
			{
				reducedWidth = 1;
			}
			if (reducedHeight <= 0)
			{
				reducedHeight = 1;
			}
			outlineDesc.width = reducedWidth;
			outlineDesc.height = reducedHeight;
			cbHighlight.GetTemporaryRT(mipOutlineBuffers[i], outlineDesc, FilterMode.Bilinear);
		}
		for (int j = 0; j < bufferCount - 1; j += 2)
		{
			if (j == 0)
			{
				RenderingUtils.FullScreenBlit(cbHighlight, sourceRT, mipOutlineBuffers[j + 1], fxMatBlurOutline, 3);
			}
			else
			{
				RenderingUtils.FullScreenBlit(cbHighlight, mipOutlineBuffers[j], mipOutlineBuffers[j + 1], fxMatBlurOutline, 0);
			}
			RenderingUtils.FullScreenBlit(cbHighlight, mipOutlineBuffers[j + 1], mipOutlineBuffers[j], fxMatBlurOutline, 1);
			if (j < bufferCount - 2)
			{
				RenderingUtils.FullScreenBlit(cbHighlight, mipOutlineBuffers[j], mipOutlineBuffers[j + 2], fxMatBlurOutline, 2);
			}
		}
	}

	private void ComposeSmoothBlend(Visibility smoothGlowVisibility, Visibility smoothOutlineVisibility)
	{
		cbHighlight.SetRenderTarget(colorAttachmentBuffer, depthAttachmentBuffer);
		int num;
		if (glow > 0f && glowWidth > 0f)
		{
			num = ((glowQuality == QualityLevel.Highest) ? 1 : 0);
			if (num != 0)
			{
				fxMatComposeGlow.SetVector(ShaderParams.Flip, (isVREnabled && flipY) ? new Vector4(1f, -1f) : new Vector4(0f, 1f));
				fxMatComposeGlow.SetInt(ShaderParams.ZTest, GetZTestValue(smoothGlowVisibility));
				cbHighlight.DrawMesh(quadMesh, quadGlowMatrix, fxMatComposeGlow, 0, 0);
			}
		}
		else
		{
			num = 0;
		}
		bool renderSmoothOutline = outline > 0f && outlineWidth > 0f && outlineQuality == QualityLevel.Highest;
		if (renderSmoothOutline)
		{
			fxMatComposeOutline.SetVector(ShaderParams.Flip, (isVREnabled && flipY) ? new Vector4(1f, -1f) : new Vector4(0f, 1f));
			fxMatComposeOutline.SetInt(ShaderParams.ZTest, GetZTestValue(smoothOutlineVisibility));
			cbHighlight.DrawMesh(quadMesh, quadOutlineMatrix, fxMatComposeOutline, 0, 0);
		}
		if (num != 0)
		{
			for (int k = 0; k < mipGlowBuffers.Length; k++)
			{
				cbHighlight.ReleaseTemporaryRT(mipGlowBuffers[k]);
			}
		}
		if (renderSmoothOutline)
		{
			for (int i = 0; i < mipOutlineBuffers.Length; i++)
			{
				cbHighlight.ReleaseTemporaryRT(mipOutlineBuffers[i]);
			}
		}
		cbHighlight.ReleaseTemporaryRT(sourceRT);
	}

	private void InitMaterial(ref Material material, string shaderName)
	{
		if (!(material != null))
		{
			Shader shaderFX = Shader.Find(shaderName);
			if (shaderFX == null)
			{
				Debug.LogError("Shader " + shaderName + " not found.");
			}
			else
			{
				material = new Material(shaderFX);
			}
		}
	}

	private void Fork(Material mat, ref Material[] mats, Mesh mesh)
	{
		if (!(mesh == null))
		{
			int count = mesh.subMeshCount;
			Fork(mat, ref mats, count);
		}
	}

	private void Fork(Material material, ref Material[] array, int count)
	{
		if (material == null)
		{
			_ = 9;
		}
		if (array == null || array.Length < count)
		{
			DestroyMaterialArray(array);
			array = new Material[count];
		}
		for (int k = 0; k < count; k++)
		{
			if (array[k] == null)
			{
				array[k] = global::UnityEngine.Object.Instantiate(material);
			}
		}
	}

	public void SetTarget(Transform transform)
	{
		if (transform == null)
		{
			return;
		}
		if (transform != target)
		{
			if (_highlighted)
			{
				ImmediateFadeOut();
			}
			target = transform;
			SetupMaterial();
		}
		else
		{
			UpdateVisibilityState();
		}
	}

	public void SetTargets(Transform transform, Renderer[] renderers)
	{
		if (!(transform == null))
		{
			if (_highlighted)
			{
				ImmediateFadeOut();
			}
			effectGroup = TargetOptions.Scripting;
			target = transform;
			SetupMaterial(renderers);
		}
	}

	public void SetHighlighted(bool state)
	{
		if (state != _highlighted && this.OnObjectHighlightStateChange != null && !this.OnObjectHighlightStateChange(base.gameObject, state))
		{
			return;
		}
		if (state)
		{
			lastHighlighted = this;
		}
		if (!Application.isPlaying)
		{
			_highlighted = state;
			return;
		}
		float now = GetTime();
		if (fading == FadingState.NoFading)
		{
			fadeStartTime = now;
		}
		if (state && !ignore)
		{
			if ((_highlighted && fading == FadingState.NoFading) || (this.OnObjectHighlightStart != null && !this.OnObjectHighlightStart(base.gameObject)))
			{
				return;
			}
			SendMessage("HighlightStart", null, SendMessageOptions.DontRequireReceiver);
			highlightStartTime = (targetFxStartTime = now);
			if (fadeInDuration > 0f)
			{
				if (fading == FadingState.FadingOut)
				{
					float remaining = fadeOutDuration - (now - fadeStartTime);
					fadeStartTime = now - remaining;
					fadeStartTime = Mathf.Min(fadeStartTime, now);
				}
				fading = FadingState.FadingIn;
			}
			else
			{
				fading = FadingState.NoFading;
			}
			_highlighted = true;
			requireUpdateMaterial = true;
		}
		else
		{
			if (!_highlighted)
			{
				return;
			}
			if (fadeOutDuration > 0f)
			{
				if (fading == FadingState.FadingIn)
				{
					float elapsed = now - fadeStartTime;
					fadeStartTime = now + elapsed - fadeInDuration;
					fadeStartTime = Mathf.Min(fadeStartTime, now);
				}
				fading = FadingState.FadingOut;
			}
			else
			{
				fading = FadingState.NoFading;
				ImmediateFadeOut();
				requireUpdateMaterial = true;
			}
		}
	}

	private void ImmediateFadeOut()
	{
		fading = FadingState.NoFading;
		_highlighted = false;
		if (this.OnObjectHighlightEnd != null)
		{
			this.OnObjectHighlightEnd(base.gameObject);
		}
		SendMessage("HighlightEnd", null, SendMessageOptions.DontRequireReceiver);
	}

	private void SetupMaterial()
	{
		if (target == null || fxMatMask == null)
		{
			return;
		}
		Renderer[] rr = null;
		switch (effectGroup)
		{
		case TargetOptions.OnlyThisObject:
		{
			Renderer renderer = target.GetComponent<Renderer>();
			if (renderer != null && ValidRenderer(renderer))
			{
				rr = new Renderer[1] { renderer };
			}
			break;
		}
		case TargetOptions.RootToChildren:
		{
			Transform root = target;
			while (root.parent != null)
			{
				root = root.parent;
			}
			rr = FindRenderersInChildren(root);
			break;
		}
		case TargetOptions.LayerInScene:
		{
			HighlightEffect eg2 = this;
			if (target != base.transform)
			{
				HighlightEffect targetEffect2 = target.GetComponent<HighlightEffect>();
				if (targetEffect2 != null)
				{
					eg2 = targetEffect2;
				}
			}
			rr = FindRenderersWithLayerInScene(eg2.effectGroupLayer);
			break;
		}
		case TargetOptions.LayerInChildren:
		{
			HighlightEffect eg = this;
			if (target != base.transform)
			{
				HighlightEffect targetEffect = target.GetComponent<HighlightEffect>();
				if (targetEffect != null)
				{
					eg = targetEffect;
				}
			}
			rr = FindRenderersWithLayerInChildren(eg.effectGroupLayer);
			break;
		}
		case TargetOptions.Children:
			rr = FindRenderersInChildren(target);
			break;
		case TargetOptions.Scripting:
			_ = rmsCount;
			_ = 0;
			return;
		}
		SetupMaterial(rr);
	}

	private void SetupMaterial(Renderer[] rr)
	{
		if (rr == null)
		{
			rr = new Renderer[0];
		}
		if (rms == null || rms.Length < rr.Length)
		{
			rms = new ModelMaterials[rr.Length];
		}
		InitCommandBuffer();
		spriteMode = false;
		rmsCount = 0;
		for (int k = 0; k < rr.Length; k++)
		{
			rms[rmsCount].Init();
			Renderer renderer = rr[k];
			if (renderer == null)
			{
				continue;
			}
			if (effectGroup != TargetOptions.OnlyThisObject && !string.IsNullOrEmpty(effectNameFilter))
			{
				if (effectNameUseRegEx)
				{
					try
					{
						lastRegExError = "";
						if (!Regex.IsMatch(renderer.name, effectNameFilter))
						{
							continue;
						}
					}
					catch (Exception ex)
					{
						lastRegExError = ex.Message;
						continue;
					}
				}
				else if (!renderer.name.Contains(effectNameFilter))
				{
					continue;
				}
			}
			rms[rmsCount].renderer = renderer;
			rms[rmsCount].renderWasVisibleDuringSetup = renderer.isVisible;
			sortingOffset = (renderer.bounds.size.x + renderer.bounds.size.y + renderer.bounds.size.z) % 0.0001f;
			if (renderer.transform != target)
			{
				HighlightEffect otherEffect = renderer.GetComponent<HighlightEffect>();
				if (otherEffect != null && otherEffect.enabled && otherEffect.ignore)
				{
					continue;
				}
			}
			if (this.OnRendererHighlightStart != null && !this.OnRendererHighlightStart(renderer))
			{
				rmsCount++;
				continue;
			}
			rms[rmsCount].isCombined = false;
			bool isSkinnedMesh = renderer is SkinnedMeshRenderer;
			rms[rmsCount].isSkinnedMesh = isSkinnedMesh;
			bool num = renderer is SpriteRenderer;
			rms[rmsCount].normalsOption = (isSkinnedMesh ? NormalsOption.PreserveOriginal : normalsOption);
			if (num)
			{
				rms[rmsCount].mesh = quadMesh;
				spriteMode = true;
			}
			else if (isSkinnedMesh)
			{
				rms[rmsCount].mesh = ((SkinnedMeshRenderer)renderer).sharedMesh;
			}
			else if (Application.isPlaying && renderer.isPartOfStaticBatch)
			{
				MeshCollider mc = renderer.GetComponent<MeshCollider>();
				if (mc != null)
				{
					rms[rmsCount].mesh = mc.sharedMesh;
				}
			}
			else
			{
				MeshFilter mf = renderer.GetComponent<MeshFilter>();
				if (mf != null)
				{
					rms[rmsCount].mesh = mf.sharedMesh;
				}
			}
			if (rms[rmsCount].mesh == null)
			{
				continue;
			}
			rms[rmsCount].transform = renderer.transform;
			Fork(fxMatMask, ref rms[rmsCount].fxMatMask, rms[rmsCount].mesh);
			Fork(fxMatOutlineTemplate, ref rms[rmsCount].fxMatOutline, rms[rmsCount].mesh);
			Fork(fxMatGlowTemplate, ref rms[rmsCount].fxMatGlow, rms[rmsCount].mesh);
			Fork(fxMatSeeThrough, ref rms[rmsCount].fxMatSeeThroughInner, rms[rmsCount].mesh);
			Fork(fxMatSeeThroughBorder, ref rms[rmsCount].fxMatSeeThroughBorder, rms[rmsCount].mesh);
			Fork(fxMatOverlay, ref rms[rmsCount].fxMatOverlay, rms[rmsCount].mesh);
			Fork(fxMatInnerGlow, ref rms[rmsCount].fxMatInnerGlow, rms[rmsCount].mesh);
			Fork(fxMatSolidColor, ref rms[rmsCount].fxMatSolidColor, rms[rmsCount].mesh);
			rms[rmsCount].originalMesh = rms[rmsCount].mesh;
			if (!rms[rmsCount].preserveOriginalMesh && (innerGlow > 0f || (glow > 0f && glowQuality != QualityLevel.Highest) || (outline > 0f && outlineQuality != QualityLevel.Highest)))
			{
				if (normalsOption == NormalsOption.Reorient)
				{
					ReorientNormals(rmsCount);
				}
				else
				{
					AverageNormals(rmsCount);
				}
			}
			rmsCount++;
		}
		if (spriteMode)
		{
			outlineIndependent = false;
			outlineQuality = QualityLevel.Highest;
			glowQuality = QualityLevel.Highest;
			innerGlow = 0f;
			cullBackFaces = false;
			seeThrough = SeeThroughMode.Never;
			if (alphaCutOff <= 0f)
			{
				alphaCutOff = 0.5f;
			}
		}
		else if (combineMeshes)
		{
			CombineMeshes();
		}
		UpdateMaterialProperties();
	}

	private bool ValidRenderer(Renderer r)
	{
		if (!(r is MeshRenderer) && !(r is SpriteRenderer))
		{
			return r is SkinnedMeshRenderer;
		}
		return true;
	}

	private Renderer[] FindRenderersWithLayerInScene(LayerMask layer)
	{
		Renderer[] rr = Misc.FindObjectsOfType<Renderer>();
		tempRR.Clear();
		foreach (Renderer r in rr)
		{
			if (((1 << r.gameObject.layer) & (int)layer) != 0 && ValidRenderer(r))
			{
				tempRR.Add(r);
			}
		}
		return tempRR.ToArray();
	}

	private Renderer[] FindRenderersWithLayerInChildren(LayerMask layer)
	{
		Renderer[] rr = target.GetComponentsInChildren<Renderer>();
		tempRR.Clear();
		foreach (Renderer r in rr)
		{
			if (((1 << r.gameObject.layer) & (int)layer) != 0 && ValidRenderer(r))
			{
				tempRR.Add(r);
			}
		}
		return tempRR.ToArray();
	}

	private Renderer[] FindRenderersInChildren(Transform parent)
	{
		tempRR.Clear();
		parent.GetComponentsInChildren(tempRR);
		for (int i = 0; i < tempRR.Count; i++)
		{
			Renderer r = tempRR[i];
			if (!ValidRenderer(r))
			{
				tempRR.RemoveAt(i);
				i--;
			}
		}
		return tempRR.ToArray();
	}

	private void CheckGeometrySupportDependencies()
	{
		InitMaterial(ref fxMatMask, "HighlightPlus/Geometry/Mask");
		InitMaterial(ref fxMatGlowTemplate, "HighlightPlus/Geometry/Glow");
		if (fxMatGlowTemplate != null)
		{
			Texture2D noiseTex = Resources.Load<Texture2D>("HighlightPlus/blueNoiseVL");
			fxMatGlowTemplate.SetTexture(ShaderParams.NoiseTex, noiseTex);
			if (useGPUInstancing)
			{
				fxMatGlowTemplate.enableInstancing = true;
			}
		}
		InitMaterial(ref fxMatInnerGlow, "HighlightPlus/Geometry/InnerGlow");
		InitMaterial(ref fxMatOutlineTemplate, "HighlightPlus/Geometry/Outline");
		if (fxMatOutlineTemplate != null && useGPUInstancing)
		{
			fxMatOutlineTemplate.enableInstancing = true;
		}
		InitMaterial(ref fxMatOverlay, "HighlightPlus/Geometry/Overlay");
		InitMaterial(ref fxMatSeeThrough, "HighlightPlus/Geometry/SeeThrough");
		InitMaterial(ref fxMatSeeThroughBorder, "HighlightPlus/Geometry/SeeThroughBorder");
		InitMaterial(ref fxMatSeeThroughMask, "HighlightPlus/Geometry/SeeThroughMask");
		InitMaterial(ref fxMatTarget, "HighlightPlus/Geometry/Target");
		InitMaterial(ref fxMatComposeGlow, "HighlightPlus/Geometry/ComposeGlow");
		InitMaterial(ref fxMatComposeOutline, "HighlightPlus/Geometry/ComposeOutline");
		InitMaterial(ref fxMatSolidColor, "HighlightPlus/Geometry/SolidColor");
		InitMaterial(ref fxMatBlurGlow, "HighlightPlus/Geometry/BlurGlow");
		InitMaterial(ref fxMatBlurOutline, "HighlightPlus/Geometry/BlurOutline");
		InitMaterial(ref fxMatClearStencil, "HighlightPlus/ClearStencil");
	}

	private void InitCommandBuffer()
	{
		if (cbHighlight == null)
		{
			cbHighlight = new CommandBuffer();
			cbHighlight.name = "Highlight Plus for " + base.name;
		}
		cbHighlightEmpty = true;
	}

	private void ConfigureOutput()
	{
		if (cbHighlightEmpty)
		{
			cbHighlightEmpty = false;
			bool useDepthRenderBuffer = colorAttachmentBuffer != BuiltinRenderTextureType.CameraTarget && depthAttachmentBuffer == BuiltinRenderTextureType.CameraTarget;
			cbHighlight.SetRenderTarget(colorAttachmentBuffer, useDepthRenderBuffer ? colorAttachmentBuffer : depthAttachmentBuffer);
		}
	}

	public void UpdateVisibilityState()
	{
		if (rms == null || rms.Length != 1 || rms[0].transform != base.transform || rms[0].renderer == null)
		{
			isVisible = true;
		}
		else if (rms[0].renderer != null)
		{
			isVisible = rms[0].renderer.isVisible;
		}
	}

	public void UpdateMaterialProperties()
	{
		if (rms == null)
		{
			return;
		}
		if (ignore)
		{
			_highlighted = false;
		}
		UpdateVisibilityState();
		maskRequired = (_highlighted && ((outline > 0f && outlineMaskMode != MaskMode.IgnoreMask) || (glow > 0f && glowMaskMode != MaskMode.IgnoreMask))) || seeThrough != SeeThroughMode.Never || (targetFX && targetFXAlignToGround);
		usesSeeThrough = seeThroughIntensity > 0f && (seeThrough == SeeThroughMode.AlwaysWhenOccluded || (seeThrough == SeeThroughMode.WhenHighlighted && _highlighted));
		if (usesSeeThrough && seeThroughChildrenSortingMode != 0 && rms.Length != 0)
		{
			if (seeThroughChildrenSortingMode == SeeThroughSortingMode.SortByMaterialsRenderQueue)
			{
				Array.Sort(rms, MaterialsRenderQueueComparer);
			}
			else
			{
				Array.Sort(rms, MaterialsRenderQueueInvertedComparer);
			}
		}
		Color seeThroughTintColor = this.seeThroughTintColor;
		seeThroughTintColor.a = seeThroughTintAlpha;
		if (lastOutlineVisibility != outlineVisibility)
		{
			if (glowQuality == QualityLevel.Highest && outlineQuality == QualityLevel.Highest)
			{
				glowVisibility = outlineVisibility;
			}
			lastOutlineVisibility = outlineVisibility;
		}
		if (outlineWidth < 0f)
		{
			outlineWidth = 0f;
		}
		if (outlineQuality == QualityLevel.Medium)
		{
			outlineOffsetsMin = 4;
			outlineOffsetsMax = 7;
		}
		else if (outlineQuality == QualityLevel.High)
		{
			outlineOffsetsMin = 0;
			outlineOffsetsMax = 7;
		}
		else
		{
			outlineOffsetsMin = (outlineOffsetsMax = 0);
		}
		if (glowWidth < 0f)
		{
			glowWidth = 0f;
		}
		if (glowQuality == QualityLevel.Medium)
		{
			glowOffsetsMin = 4;
			glowOffsetsMax = 7;
		}
		else if (glowQuality == QualityLevel.High)
		{
			glowOffsetsMin = 0;
			glowOffsetsMax = 7;
		}
		else
		{
			glowOffsetsMin = (glowOffsetsMax = 0);
		}
		if (targetFXTransitionDuration <= 0f)
		{
			targetFXTransitionDuration = 0.0001f;
		}
		if (targetFXStayDuration <= 0f)
		{
			targetFXStayDuration = 0f;
		}
		if (targetFXFadePower <= 0f)
		{
			targetFXFadePower = 0f;
		}
		if (seeThroughDepthOffset < 0f)
		{
			seeThroughDepthOffset = 0f;
		}
		if (seeThroughMaxDepth < 0f)
		{
			seeThroughMaxDepth = 0f;
		}
		if (seeThroughBorderWidth < 0f)
		{
			seeThroughBorderWidth = 0f;
		}
		if (outlineSharpness < 1f)
		{
			outlineSharpness = 1f;
		}
		shouldBakeSkinnedMesh = optimizeSkinnedMesh && ((outline > 0f && outlineQuality != QualityLevel.Highest) || (glow > 0f && glowQuality != QualityLevel.Highest));
		useSmoothGlow = glow > 0f && glowWidth > 0f && glowQuality == QualityLevel.Highest;
		useSmoothOutline = outline > 0f && outlineWidth > 0f && outlineQuality == QualityLevel.Highest;
		useSmoothBlend = useSmoothGlow || useSmoothOutline;
		if (useSmoothBlend)
		{
			if (useSmoothGlow && useSmoothOutline)
			{
				outlineVisibility = glowVisibility;
			}
			outlineEdgeThreshold = Mathf.Clamp01(outlineEdgeThreshold);
		}
		if (useSmoothGlow)
		{
			fxMatComposeGlow.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
			if (glowBlendMode == GlowBlendMode.Additive)
			{
				fxMatComposeGlow.SetInt(ShaderParams.BlendSrc, 1);
				fxMatComposeGlow.SetInt(ShaderParams.BlendDst, 1);
			}
			else
			{
				fxMatComposeGlow.SetInt(ShaderParams.BlendSrc, 5);
				fxMatComposeGlow.SetInt(ShaderParams.BlendDst, 10);
			}
			fxMatComposeGlow.SetColor(ShaderParams.Debug, glowBlitDebug ? debugColor : blackColor);
			fxMatComposeGlow.SetInt(ShaderParams.GlowStencilComp, (glowMaskMode != 0) ? 8 : 6);
			if (glowMaskMode == MaskMode.StencilAndCutout)
			{
				fxMatComposeGlow.EnableKeyword("HP_MASK_CUTOUT");
			}
			else
			{
				fxMatComposeGlow.DisableKeyword("HP_MASK_CUTOUT");
			}
			fxMatBlurGlow.SetFloat(ShaderParams.BlurScale, glowWidth / (float)glowDownsampling);
			fxMatBlurGlow.SetFloat(ShaderParams.Speed, glowAnimationSpeed);
		}
		if (useSmoothOutline)
		{
			fxMatComposeOutline.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
			fxMatComposeOutline.SetColor(ShaderParams.Debug, outlineBlitDebug ? debugColor : blackColor);
			fxMatComposeOutline.SetFloat(ShaderParams.OutlineSharpness, outlineSharpness);
			if (outlineEdgeMode == OutlineEdgeMode.Exterior)
			{
				fxMatComposeOutline.DisableKeyword("HP_ALL_EDGES");
			}
			else
			{
				fxMatComposeOutline.EnableKeyword("HP_ALL_EDGES");
				outlineDownsampling = 1;
			}
			if (outlineEdgeMode != 0 || outlineMaskMode == MaskMode.IgnoreMask)
			{
				fxMatComposeOutline.SetInt(ShaderParams.OutlineStencilComp, 8);
			}
			else
			{
				fxMatComposeOutline.SetInt(ShaderParams.OutlineStencilComp, 6);
			}
			if (outlineMaskMode == MaskMode.StencilAndCutout)
			{
				fxMatComposeOutline.EnableKeyword("HP_MASK_CUTOUT");
			}
			else
			{
				fxMatComposeOutline.DisableKeyword("HP_MASK_CUTOUT");
			}
			float edgeWidth = outlineWidth;
			if (outlineEdgeMode == OutlineEdgeMode.Any)
			{
				edgeWidth = Mathf.Clamp(edgeWidth, (float)outlineBlurPasses / 5f, outlineBlurPasses);
			}
			fxMatBlurOutline.SetFloat(ShaderParams.BlurScale, edgeWidth / (float)outlineDownsampling);
			fxMatBlurOutline.SetFloat(ShaderParams.BlurScaleFirstHoriz, edgeWidth * 2f);
		}
		if (outlineColorStyle == ColorStyle.Gradient && outlineGradient != null)
		{
			bool requiresUpdate = false;
			if (outlineGradientTex == null)
			{
				outlineGradientTex = new Texture2D(32, 1, TextureFormat.RGBA32, mipChain: false, linear: true);
				outlineGradientTex.wrapMode = TextureWrapMode.Clamp;
				requiresUpdate = true;
			}
			if (outlineGradientColors == null || outlineGradientColors.Length != 32)
			{
				outlineGradientColors = new Color[32];
				requiresUpdate = true;
			}
			for (int k = 0; k < 32; k++)
			{
				float t = (float)k / 32f;
				Color color = outlineGradient.Evaluate(t);
				if (color != outlineGradientColors[k])
				{
					outlineGradientColors[k] = color;
					requiresUpdate = true;
				}
			}
			if (requiresUpdate)
			{
				outlineGradientTex.SetPixels(outlineGradientColors);
				outlineGradientTex.Apply();
			}
		}
		if (targetFX)
		{
			if (targetFXTexture == null)
			{
				targetFXTexture = Resources.Load<Texture2D>("HighlightPlus/target");
			}
			fxMatTarget.mainTexture = targetFXTexture;
			fxMatTarget.SetInt(ShaderParams.ZTest, GetZTestValue(targetFXVisibility));
		}
		float scaledOutlineWidth = (outlineQuality.UsesMultipleOffsets() ? 0f : (outlineWidth / 100f));
		for (int i = 0; i < rmsCount; i++)
		{
			if (!(rms[i].mesh != null))
			{
				continue;
			}
			Renderer renderer = rms[i].renderer;
			if (renderer == null)
			{
				continue;
			}
			renderer.GetSharedMaterials(rendererSharedMaterials);
			for (int l = 0; l < rms[i].mesh.subMeshCount; l++)
			{
				if (((1 << l) & subMeshMask) == 0)
				{
					continue;
				}
				Material mat = null;
				if (l < rendererSharedMaterials.Count)
				{
					mat = rendererSharedMaterials[l];
				}
				if (mat == null)
				{
					continue;
				}
				bool hasTexture = false;
				Texture matTexture = null;
				Vector2 matTextureOffset = Vector2.zero;
				Vector2 matTextureScale = Vector2.one;
				if (mat.HasProperty(ShaderParams.MainTex))
				{
					matTexture = mat.GetTexture(ShaderParams.MainTex);
					matTextureOffset = mat.mainTextureOffset;
					matTextureScale = mat.mainTextureScale;
					hasTexture = true;
				}
				else if (mat.HasProperty(ShaderParams.BaseMap))
				{
					matTexture = mat.GetTexture(ShaderParams.BaseMap);
					hasTexture = true;
					if (mat.HasProperty(ShaderParams.BaseMapST))
					{
						Vector4 baseMapST = mat.GetVector(ShaderParams.BaseMapST);
						matTextureScale.x = baseMapST.x;
						matTextureScale.y = baseMapST.y;
						matTextureOffset.x = baseMapST.z;
						matTextureOffset.y = baseMapST.w;
					}
				}
				bool useAlphaTest = alphaCutOff > 0f && hasTexture;
				if (rms[i].fxMatMask != null && rms[i].fxMatMask.Length > l)
				{
					Material fxMat = rms[i].fxMatMask[l];
					if (fxMat != null)
					{
						fxMat.mainTexture = matTexture;
						fxMat.mainTextureOffset = matTextureOffset;
						fxMat.mainTextureScale = matTextureScale;
						if (useAlphaTest)
						{
							fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat.DisableKeyword("HP_ALPHACLIP");
						}
						fxMat.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
					}
				}
				if (rms[i].fxMatOutline != null && rms[i].fxMatOutline.Length > l)
				{
					Material fxMat2 = rms[i].fxMatOutline[l];
					if (fxMat2 != null)
					{
						fxMat2.SetFloat(ShaderParams.OutlineWidth, scaledOutlineWidth);
						fxMat2.SetInt(ShaderParams.OutlineZTest, GetZTestValue(outlineVisibility));
						fxMat2.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
						fxMat2.SetFloat(ShaderParams.ConstantWidth, constantWidth ? 1f : 0f);
						fxMat2.SetInt(ShaderParams.OutlineStencilComp, (outlineMaskMode == MaskMode.IgnoreMask) ? 8 : 6);
						if (useAlphaTest)
						{
							fxMat2.mainTexture = matTexture;
							fxMat2.mainTextureOffset = matTextureOffset;
							fxMat2.mainTextureScale = matTextureScale;
							fxMat2.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat2.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat2.DisableKeyword("HP_ALPHACLIP");
						}
						fxMat2.DisableKeyword("HP_OUTLINE_GRADIENT_LS");
						fxMat2.DisableKeyword("HP_OUTLINE_GRADIENT_WS");
						if (outlineColorStyle == ColorStyle.Gradient)
						{
							fxMat2.SetTexture(ShaderParams.OutlineGradientTex, outlineGradientTex);
							fxMat2.EnableKeyword(outlineGradientInLocalSpace ? "HP_OUTLINE_GRADIENT_LS" : "HP_OUTLINE_GRADIENT_WS");
						}
					}
				}
				if (rms[i].fxMatGlow != null && rms[i].fxMatGlow.Length > l)
				{
					Material fxMat3 = rms[i].fxMatGlow[l];
					if (fxMat3 != null)
					{
						fxMat3.SetVector(ShaderParams.Glow2, new Vector4((outline > 0f) ? (outlineWidth / 100f) : 0f, glowAnimationSpeed, glowDithering));
						if (glowDitheringStyle == GlowDitheringStyle.Noise)
						{
							fxMat3.EnableKeyword("HP_DITHER_BLUENOISE");
						}
						else
						{
							fxMat3.DisableKeyword("HP_DITHER_BLUENOISE");
						}
						fxMat3.SetInt(ShaderParams.GlowZTest, GetZTestValue(glowVisibility));
						fxMat3.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
						fxMat3.SetFloat(ShaderParams.ConstantWidth, constantWidth ? 1f : 0f);
						fxMat3.SetInt(ShaderParams.GlowStencilOp, (!glowBlendPasses) ? 2 : 0);
						fxMat3.SetInt(ShaderParams.GlowStencilComp, (glowMaskMode == MaskMode.IgnoreMask) ? 8 : 6);
						if (useAlphaTest)
						{
							fxMat3.mainTexture = matTexture;
							fxMat3.mainTextureOffset = matTextureOffset;
							fxMat3.mainTextureScale = matTextureScale;
							fxMat3.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat3.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat3.DisableKeyword("HP_ALPHACLIP");
						}
					}
				}
				bool usesSeeThroughBorder = rms[i].fxMatSeeThroughBorder != null && rms[i].fxMatSeeThroughBorder.Length > l && seeThroughBorder * seeThroughBorderWidth > 0f;
				if (rms[i].fxMatSeeThroughInner != null && rms[i].fxMatSeeThroughInner.Length > l)
				{
					Material fxMat4 = rms[i].fxMatSeeThroughInner[l];
					if (fxMat4 != null)
					{
						fxMat4.SetFloat(ShaderParams.SeeThrough, seeThroughIntensity);
						fxMat4.SetFloat(ShaderParams.SeeThroughNoise, seeThroughNoise);
						fxMat4.SetColor(ShaderParams.SeeThroughTintColor, seeThroughTintColor);
						if (seeThroughOccluderMaskAccurate && (int)seeThroughOccluderMask != -1)
						{
							fxMat4.SetInt(ShaderParams.SeeThroughStencilRef, 1);
							fxMat4.SetInt(ShaderParams.SeeThroughStencilComp, 3);
							fxMat4.SetInt(ShaderParams.SeeThroughStencilPassOp, 1);
						}
						else
						{
							fxMat4.SetInt(ShaderParams.SeeThroughStencilRef, 2);
							fxMat4.SetInt(ShaderParams.SeeThroughStencilComp, 5);
							fxMat4.SetInt(ShaderParams.SeeThroughStencilPassOp, 2);
						}
						fxMat4.mainTexture = matTexture;
						fxMat4.mainTextureOffset = matTextureOffset;
						fxMat4.mainTextureScale = matTextureScale;
						if (useAlphaTest)
						{
							fxMat4.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat4.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat4.DisableKeyword("HP_ALPHACLIP");
						}
						if (seeThroughDepthOffset > 0f || seeThroughMaxDepth > 0f)
						{
							fxMat4.SetFloat(ShaderParams.SeeThroughDepthOffset, (seeThroughDepthOffset > 0f) ? seeThroughDepthOffset : (-1f));
							fxMat4.SetFloat(ShaderParams.SeeThroughMaxDepth, (seeThroughMaxDepth > 0f) ? seeThroughMaxDepth : 999999f);
							fxMat4.EnableKeyword("HP_DEPTH_OFFSET");
						}
						else
						{
							fxMat4.DisableKeyword("HP_DEPTH_OFFSET");
						}
						if (seeThroughBorderOnly)
						{
							fxMat4.EnableKeyword("HP_SEETHROUGH_ONLY_BORDER");
						}
						else
						{
							fxMat4.DisableKeyword("HP_SEETHROUGH_ONLY_BORDER");
						}
						fxMat4.DisableKeyword("HP_TEXTURE_TRIPLANAR");
						fxMat4.DisableKeyword("HP_TEXTURE_OBJECTSPACE");
						fxMat4.DisableKeyword("HP_TEXTURE_SCREENSPACE");
						if (seeThroughTexture != null)
						{
							fxMat4.SetTexture(ShaderParams.SeeThroughTexture, seeThroughTexture);
							fxMat4.SetFloat(ShaderParams.SeeThroughTextureScale, seeThroughTextureScale);
							switch (seeThroughTextureUVSpace)
							{
							case TextureUVSpace.ScreenSpace:
								fxMat4.EnableKeyword("HP_TEXTURE_SCREENSPACE");
								break;
							case TextureUVSpace.ObjectSpace:
								fxMat4.EnableKeyword("HP_TEXTURE_OBJECTSPACE");
								break;
							default:
								fxMat4.EnableKeyword("HP_TEXTURE_TRIPLANAR");
								break;
							}
						}
					}
				}
				if (usesSeeThroughBorder)
				{
					Material fxMat5 = rms[i].fxMatSeeThroughBorder[l];
					if (fxMat5 != null)
					{
						fxMat5.SetColor(ShaderParams.SeeThroughBorderColor, new Color(seeThroughBorderColor.r, seeThroughBorderColor.g, seeThroughBorderColor.b, seeThroughBorder));
						fxMat5.SetFloat(ShaderParams.SeeThroughBorderWidth, (seeThroughBorder * seeThroughBorderWidth > 0f) ? (seeThroughBorderWidth / 100f) : 0f);
						fxMat5.SetFloat(ShaderParams.SeeThroughBorderConstantWidth, constantWidth ? 1f : 0f);
						if (seeThroughOccluderMaskAccurate && (int)seeThroughOccluderMask != -1)
						{
							fxMat5.SetInt(ShaderParams.SeeThroughStencilRef, 1);
							fxMat5.SetInt(ShaderParams.SeeThroughStencilComp, 3);
							fxMat5.SetInt(ShaderParams.SeeThroughStencilPassOp, 1);
						}
						else
						{
							fxMat5.SetInt(ShaderParams.SeeThroughStencilRef, 2);
							fxMat5.SetInt(ShaderParams.SeeThroughStencilComp, 5);
							fxMat5.SetInt(ShaderParams.SeeThroughStencilPassOp, 0);
						}
						fxMat5.mainTexture = matTexture;
						fxMat5.mainTextureOffset = matTextureOffset;
						fxMat5.mainTextureScale = matTextureScale;
						if (useAlphaTest)
						{
							fxMat5.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat5.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat5.DisableKeyword("HP_ALPHACLIP");
						}
						if (seeThroughDepthOffset > 0f || seeThroughMaxDepth > 0f)
						{
							fxMat5.SetFloat(ShaderParams.SeeThroughDepthOffset, (seeThroughDepthOffset > 0f) ? seeThroughDepthOffset : (-1f));
							fxMat5.SetFloat(ShaderParams.SeeThroughMaxDepth, (seeThroughMaxDepth > 0f) ? seeThroughMaxDepth : 999999f);
							fxMat5.EnableKeyword("HP_DEPTH_OFFSET");
						}
						else
						{
							fxMat5.DisableKeyword("HP_DEPTH_OFFSET");
						}
					}
				}
				if (rms[i].fxMatOverlay != null && rms[i].fxMatOverlay.Length > l)
				{
					Material fxMat6 = rms[i].fxMatOverlay[l];
					if (fxMat6 != null)
					{
						fxMat6.mainTexture = matTexture;
						fxMat6.mainTextureOffset = matTextureOffset;
						fxMat6.mainTextureScale = matTextureScale;
						if (mat.HasProperty(ShaderParams.Color))
						{
							fxMat6.SetColor(ShaderParams.OverlayBackColor, mat.GetColor(ShaderParams.Color));
						}
						fxMat6.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
						fxMat6.SetInt(ShaderParams.OverlayZTest, GetZTestValue(overlayVisibility));
						fxMat6.DisableKeyword("HP_TEXTURE_TRIPLANAR");
						fxMat6.DisableKeyword("HP_TEXTURE_OBJECTSPACE");
						fxMat6.DisableKeyword("HP_TEXTURE_SCREENSPACE");
						if (overlayTexture != null)
						{
							fxMat6.SetTexture(ShaderParams.OverlayTexture, overlayTexture);
							fxMat6.SetVector(ShaderParams.OverlayTextureScrolling, overlayTextureScrolling);
							switch (overlayTextureUVSpace)
							{
							case TextureUVSpace.ScreenSpace:
								fxMat6.EnableKeyword("HP_TEXTURE_SCREENSPACE");
								break;
							case TextureUVSpace.ObjectSpace:
								fxMat6.EnableKeyword("HP_TEXTURE_OBJECTSPACE");
								break;
							default:
								fxMat6.EnableKeyword("HP_TEXTURE_TRIPLANAR");
								break;
							}
						}
						if (useAlphaTest)
						{
							fxMat6.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat6.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat6.DisableKeyword("HP_ALPHACLIP");
						}
					}
				}
				if (rms[i].fxMatInnerGlow != null && rms[i].fxMatInnerGlow.Length > l)
				{
					Material fxMat7 = rms[i].fxMatInnerGlow[l];
					if (fxMat7 != null)
					{
						fxMat7.mainTexture = matTexture;
						fxMat7.mainTextureOffset = matTextureOffset;
						fxMat7.mainTextureScale = matTextureScale;
						fxMat7.SetFloat(ShaderParams.InnerGlowWidth, innerGlowWidth);
						fxMat7.SetInt(ShaderParams.InnerGlowZTest, GetZTestValue(innerGlowVisibility));
						fxMat7.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
						fxMat7.SetInt(ShaderParams.InnerGlowBlendMode, (innerGlowBlendMode == InnerGlowBlendMode.Additive) ? 1 : 10);
						if (useAlphaTest)
						{
							fxMat7.SetFloat(ShaderParams.CutOff, alphaCutOff);
							fxMat7.EnableKeyword("HP_ALPHACLIP");
						}
						else
						{
							fxMat7.DisableKeyword("HP_ALPHACLIP");
						}
					}
				}
				if (rms[i].fxMatSolidColor == null || rms[i].fxMatSolidColor.Length <= l)
				{
					continue;
				}
				Material fxMat8 = rms[i].fxMatSolidColor[l];
				if (fxMat8 != null)
				{
					fxMat8.color = glowHQColor;
					fxMat8.SetInt(ShaderParams.Cull, cullBackFaces ? 2 : 0);
					fxMat8.SetFloat(ShaderParams.OutlineEdgeThreshold, outlineEdgeThreshold);
					fxMat8.mainTexture = matTexture;
					fxMat8.mainTextureOffset = matTextureOffset;
					fxMat8.mainTextureScale = matTextureScale;
					if ((glow > 0f && glowQuality == QualityLevel.Highest && glowVisibility == Visibility.Normal) || (outline > 0f && outlineQuality == QualityLevel.Highest && outlineVisibility == Visibility.Normal))
					{
						fxMat8.DisableKeyword("HP_DEPTHCLIP_INV");
						fxMat8.EnableKeyword("HP_DEPTHCLIP");
					}
					else if ((glow > 0f && glowQuality == QualityLevel.Highest && glowVisibility == Visibility.OnlyWhenOccluded) || (outline > 0f && outlineQuality == QualityLevel.Highest && outlineVisibility == Visibility.OnlyWhenOccluded))
					{
						fxMat8.DisableKeyword("HP_DEPTHCLIP");
						fxMat8.EnableKeyword("HP_DEPTHCLIP_INV");
					}
					else
					{
						fxMat8.DisableKeyword("HP_DEPTHCLIP");
						fxMat8.DisableKeyword("HP_DEPTHCLIP_INV");
					}
					if (useAlphaTest)
					{
						fxMat8.SetFloat(ShaderParams.CutOff, alphaCutOff);
						fxMat8.EnableKeyword("HP_ALPHACLIP");
					}
					else
					{
						fxMat8.DisableKeyword("HP_ALPHACLIP");
					}
					if (outlineEdgeMode == OutlineEdgeMode.Any)
					{
						fxMat8.EnableKeyword("HP_ALL_EDGES");
					}
					else
					{
						fxMat8.DisableKeyword("HP_ALL_EDGES");
					}
				}
			}
		}
	}

	private int MaterialsRenderQueueComparer(ModelMaterials m1, ModelMaterials m2)
	{
		Material mat1 = ((m1.renderer != null) ? m1.renderer.sharedMaterial : null);
		Material mat2 = ((m2.renderer != null) ? m2.renderer.sharedMaterial : null);
		int mq1 = ((mat1 != null) ? mat1.renderQueue : 0);
		int mq2 = ((mat2 != null) ? mat2.renderQueue : 0);
		return mq1.CompareTo(mq2);
	}

	private int MaterialsRenderQueueInvertedComparer(ModelMaterials m1, ModelMaterials m2)
	{
		Material mat1 = ((m1.renderer != null) ? m1.renderer.sharedMaterial : null);
		Material mat2 = ((m2.renderer != null) ? m2.renderer.sharedMaterial : null);
		int mq1 = ((mat1 != null) ? mat1.renderQueue : 0);
		return ((mat2 != null) ? mat2.renderQueue : 0).CompareTo(mq1);
	}

	private float ComputeCameraDistanceFade(Vector3 position, Transform cameraTransform)
	{
		float distance = Vector3.Dot(position - cameraTransform.position, cameraTransform.forward);
		if (distance < cameraDistanceFadeNear)
		{
			return 1f - Mathf.Min(1f, cameraDistanceFadeNear - distance);
		}
		if (distance > cameraDistanceFadeFar)
		{
			return 1f - Mathf.Min(1f, distance - cameraDistanceFadeFar);
		}
		return 1f;
	}

	private int GetZTestValue(Visibility param)
	{
		return param switch
		{
			Visibility.AlwaysOnTop => 8, 
			Visibility.OnlyWhenOccluded => 5, 
			_ => 4, 
		};
	}

	private void BuildQuad()
	{
		quadMesh = new Mesh();
		Vector3[] newVertices = new Vector3[4];
		float halfHeight = 0.5f;
		float halfWidth = 0.5f;
		newVertices[0] = new Vector3(0f - halfWidth, 0f - halfHeight, 0f);
		newVertices[1] = new Vector3(0f - halfWidth, halfHeight, 0f);
		newVertices[2] = new Vector3(halfWidth, 0f - halfHeight, 0f);
		newVertices[3] = new Vector3(halfWidth, halfHeight, 0f);
		Vector2[] newUVs = new Vector2[newVertices.Length];
		newUVs[0] = new Vector2(0f, 0f);
		newUVs[1] = new Vector2(0f, 1f);
		newUVs[2] = new Vector2(1f, 0f);
		newUVs[3] = new Vector2(1f, 1f);
		int[] newTriangles = new int[6] { 0, 1, 2, 3, 2, 1 };
		Vector3[] newNormals = new Vector3[newVertices.Length];
		for (int i = 0; i < newNormals.Length; i++)
		{
			newNormals[i] = Vector3.forward;
		}
		quadMesh.vertices = newVertices;
		quadMesh.uv = newUVs;
		quadMesh.triangles = newTriangles;
		quadMesh.normals = newNormals;
		quadMesh.RecalculateBounds();
	}

	private void BuildCube()
	{
		cubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
	}

	public bool Includes(Transform transform)
	{
		for (int k = 0; k < rmsCount; k++)
		{
			if (rms[k].transform == transform)
			{
				return true;
			}
		}
		return false;
	}

	public void SetGlowColor(Color color)
	{
		if (glowPasses != null)
		{
			for (int k = 0; k < glowPasses.Length; k++)
			{
				glowPasses[k].color = color;
			}
		}
		glowHQColor = color;
		UpdateMaterialProperties();
	}

	private void AverageNormals(int objIndex)
	{
		if (rms == null || objIndex >= rms.Length)
		{
			return;
		}
		Mesh mesh = rms[objIndex].mesh;
		int hashCode = mesh.GetHashCode();
		if (!smoothMeshes.TryGetValue(hashCode, out var newMesh) || newMesh == null)
		{
			if (!mesh.isReadable)
			{
				return;
			}
			if (normals == null)
			{
				normals = new List<Vector3>();
			}
			else
			{
				normals.Clear();
			}
			mesh.GetNormals(normals);
			int normalsCount = normals.Count;
			if (normalsCount == 0)
			{
				return;
			}
			if (vertices == null)
			{
				vertices = new List<Vector3>();
			}
			else
			{
				vertices.Clear();
			}
			mesh.GetVertices(vertices);
			int vertexCount = vertices.Count;
			if (normalsCount < vertexCount)
			{
				vertexCount = normalsCount;
			}
			if (newNormals == null || newNormals.Length < vertexCount)
			{
				newNormals = new Vector3[vertexCount];
			}
			else
			{
				Vector3 zero = Vector3.zero;
				for (int k = 0; k < vertexCount; k++)
				{
					newNormals[k] = zero;
				}
			}
			if (matches == null || matches.Length < vertexCount)
			{
				matches = new int[vertexCount];
			}
			vv.Clear();
			for (int i = 0; i < vertexCount; i++)
			{
				Vector3 v = vertices[i];
				if (!vv.TryGetValue(v, out var i2))
				{
					i2 = (vv[v] = i);
				}
				matches[i] = i2;
			}
			for (int j = 0; j < vertexCount; j++)
			{
				int match = matches[j];
				newNormals[match] += normals[j];
			}
			for (int l = 0; l < vertexCount; l++)
			{
				int match2 = matches[l];
				normals[l] = newNormals[match2].normalized;
			}
			newMesh = global::UnityEngine.Object.Instantiate(mesh);
			newMesh.SetNormals(normals);
			smoothMeshes[hashCode] = newMesh;
			IncrementeMeshUsage(newMesh);
		}
		rms[objIndex].mesh = newMesh;
	}

	private void ReorientNormals(int objIndex)
	{
		if (rms == null || objIndex >= rms.Length)
		{
			return;
		}
		Mesh mesh = rms[objIndex].mesh;
		int hashCode = mesh.GetHashCode();
		if (!reorientedMeshes.TryGetValue(hashCode, out var newMesh) || newMesh == null)
		{
			if (!mesh.isReadable)
			{
				return;
			}
			if (normals == null)
			{
				normals = new List<Vector3>();
			}
			else
			{
				normals.Clear();
			}
			if (vertices == null)
			{
				vertices = new List<Vector3>();
			}
			else
			{
				vertices.Clear();
			}
			mesh.GetVertices(vertices);
			int vertexCount = vertices.Count;
			if (vertexCount == 0)
			{
				return;
			}
			Vector3 mid = Vector3.zero;
			for (int k = 0; k < vertexCount; k++)
			{
				mid += vertices[k];
			}
			mid /= (float)vertexCount;
			for (int i = 0; i < vertexCount; i++)
			{
				normals.Add((vertices[i] - mid).normalized);
			}
			newMesh = global::UnityEngine.Object.Instantiate(mesh);
			newMesh.SetNormals(normals);
			reorientedMeshes[hashCode] = newMesh;
			IncrementeMeshUsage(newMesh);
		}
		rms[objIndex].mesh = newMesh;
	}

	private void CombineMeshes()
	{
		if (rmsCount <= 1)
		{
			return;
		}
		if (combineInstances == null || combineInstances.Length != rmsCount)
		{
			combineInstances = new CombineInstance[rmsCount];
		}
		int first = -1;
		int count = 0;
		combinedMeshesHashId = 0;
		int vertexCount = 0;
		Matrix4x4 im = Matrix4x4.identity;
		for (int k = 0; k < rmsCount; k++)
		{
			combineInstances[k].mesh = null;
			if (rms[k].isSkinnedMesh)
			{
				continue;
			}
			Mesh mesh = rms[k].mesh;
			if (!(mesh == null) && mesh.isReadable && vertexCount + mesh.vertexCount <= 65535)
			{
				vertexCount += mesh.vertexCount;
				combineInstances[count].mesh = mesh;
				int instanceId = rms[k].renderer.gameObject.GetInstanceID();
				if (first < 0)
				{
					first = k;
					combinedMeshesHashId = instanceId;
					im = rms[k].transform.worldToLocalMatrix;
				}
				else
				{
					combinedMeshesHashId ^= instanceId;
					rms[k].mesh = null;
				}
				combineInstances[count].transform = im * rms[k].transform.localToWorldMatrix;
				count++;
			}
		}
		if (count >= 2)
		{
			if (count != rmsCount)
			{
				Array.Resize(ref combineInstances, count);
			}
			if (!combinedMeshes.TryGetValue(combinedMeshesHashId, out var combinedMesh) || combinedMesh == null)
			{
				combinedMesh = new Mesh();
				combinedMesh.CombineMeshes(combineInstances, mergeSubMeshes: true, useMatrices: true);
				combinedMeshes[combinedMeshesHashId] = combinedMesh;
				IncrementeMeshUsage(combinedMesh);
			}
			rms[first].mesh = combinedMesh;
			rms[first].isCombined = true;
		}
	}

	private void IncrementeMeshUsage(Mesh mesh)
	{
		sharedMeshUsage.TryGetValue(mesh, out var usageCount);
		usageCount++;
		sharedMeshUsage[mesh] = usageCount;
		instancedMeshes.Add(mesh);
	}

	public static void ClearMeshCache()
	{
		foreach (Mesh mesh in combinedMeshes.Values)
		{
			if (mesh != null)
			{
				global::UnityEngine.Object.DestroyImmediate(mesh);
			}
		}
		foreach (Mesh mesh2 in smoothMeshes.Values)
		{
			if (mesh2 != null)
			{
				global::UnityEngine.Object.DestroyImmediate(mesh2);
			}
		}
		foreach (Mesh mesh3 in reorientedMeshes.Values)
		{
			if (mesh3 != null)
			{
				global::UnityEngine.Object.DestroyImmediate(mesh3);
			}
		}
	}

	private void RefreshCachedMeshes()
	{
		if (combinedMeshes.TryGetValue(combinedMeshesHashId, out var combinedMesh))
		{
			global::UnityEngine.Object.DestroyImmediate(combinedMesh);
			combinedMeshes.Remove(combinedMeshesHashId);
		}
		if (rms == null)
		{
			return;
		}
		for (int k = 0; k < rms.Length; k++)
		{
			Mesh mesh = rms[k].mesh;
			if (mesh != null && (smoothMeshes.ContainsValue(mesh) || reorientedMeshes.ContainsValue(mesh)))
			{
				global::UnityEngine.Object.DestroyImmediate(mesh);
			}
		}
	}

	public static float GetTime()
	{
		if (!useUnscaledTime)
		{
			return Time.time;
		}
		return Time.unscaledTime;
	}

	public void HitFX()
	{
		HitFX(hitFxColor, hitFxFadeOutDuration, hitFxInitialIntensity);
	}

	public void HitFX(Vector3 position)
	{
		HitFX(hitFxColor, hitFxFadeOutDuration, hitFxInitialIntensity, position, hitFxRadius);
	}

	public void HitFX(Color color, float fadeOutDuration, float initialIntensity = 1f)
	{
		hitInitialIntensity = initialIntensity;
		hitFadeOutDuration = fadeOutDuration;
		hitColor = color;
		hitStartTime = GetTime();
		hitActive = true;
		if (overlay == 0f)
		{
			UpdateMaterialProperties();
		}
	}

	public void HitFX(Color color, float fadeOutDuration, float initialIntensity, Vector3 position, float radius)
	{
		hitInitialIntensity = initialIntensity;
		hitFadeOutDuration = fadeOutDuration;
		hitColor = color;
		hitStartTime = GetTime();
		hitActive = true;
		hitPosition = position;
		hitRadius = radius;
		if (overlay == 0f)
		{
			UpdateMaterialProperties();
		}
	}

	public void TargetFX()
	{
		targetFxStartTime = GetTime();
		if (!_highlighted)
		{
			highlighted = true;
		}
		if (!targetFX)
		{
			targetFX = true;
			UpdateMaterialProperties();
		}
	}

	public bool IsSeeThroughOccluded(Camera cam)
	{
		if (rms == null)
		{
			return false;
		}
		Bounds bounds = default(Bounds);
		for (int r = 0; r < rms.Length; r++)
		{
			if (rms[r].renderer != null)
			{
				if (bounds.size.x == 0f)
				{
					bounds = rms[r].renderer.bounds;
				}
				else
				{
					bounds.Encapsulate(rms[r].renderer.bounds);
				}
			}
		}
		Vector3 center = bounds.center;
		Vector3 camPos = cam.transform.position;
		Vector3 offset = center - camPos;
		float maxDistance = Vector3.Distance(center, camPos);
		if (hits == null || hits.Length == 0)
		{
			hits = new RaycastHit[64];
		}
		int occludersCount = occluders.Count;
		int hitCount = Physics.BoxCastNonAlloc(center - offset, bounds.extents * 0.9f, offset.normalized, hits, Quaternion.identity, maxDistance);
		for (int k = 0; k < hitCount; k++)
		{
			for (int j = 0; j < occludersCount; j++)
			{
				if (hits[k].collider.transform == occluders[j].transform)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void RegisterOccluder(HighlightSeeThroughOccluder occluder)
	{
		if (!occluders.Contains(occluder))
		{
			occluders.Add(occluder);
		}
	}

	public static void UnregisterOccluder(HighlightSeeThroughOccluder occluder)
	{
		if (occluders.Contains(occluder))
		{
			occluders.Remove(occluder);
		}
	}

	public bool RenderSeeThroughOccluders(CommandBuffer cb, Camera cam)
	{
		int occludersCount = occluders.Count;
		if (occludersCount == 0 || rmsCount == 0)
		{
			return true;
		}
		bool useRayCastCheck = false;
		for (int k = 0; k < occludersCount; k++)
		{
			HighlightSeeThroughOccluder occluder = occluders[k];
			if (!(occluder == null) && occluder.isActiveAndEnabled && occluder.mode == OccluderMode.BlocksSeeThrough && occluder.detectionMethod == DetectionMethod.RayCast)
			{
				useRayCastCheck = true;
				break;
			}
		}
		if (useRayCastCheck && IsSeeThroughOccluded(cam))
		{
			return false;
		}
		occludersFrameCount.TryGetValue(cam, out var lastFrameCount);
		int currentFrameCount = Time.frameCount;
		if (currentFrameCount == lastFrameCount)
		{
			return true;
		}
		occludersFrameCount[cam] = currentFrameCount;
		if (fxMatSeeThroughOccluder == null)
		{
			InitMaterial(ref fxMatSeeThroughOccluder, "HighlightPlus/Geometry/SeeThroughOccluder");
			if (fxMatSeeThroughOccluder == null)
			{
				return true;
			}
		}
		if (fxMatDepthWrite == null)
		{
			InitMaterial(ref fxMatDepthWrite, "HighlightPlus/Geometry/JustDepth");
			if (fxMatDepthWrite == null)
			{
				return true;
			}
		}
		for (int i = 0; i < occludersCount; i++)
		{
			HighlightSeeThroughOccluder occluder2 = occluders[i];
			if (occluder2 == null || !occluder2.isActiveAndEnabled || occluder2.detectionMethod != 0 || occluder2.meshData == null)
			{
				continue;
			}
			int meshDataLength = occluder2.meshData.Length;
			for (int m = 0; m < meshDataLength; m++)
			{
				Renderer renderer = occluder2.meshData[m].renderer;
				if (renderer.isVisible)
				{
					for (int s = 0; s < occluder2.meshData[m].subMeshCount; s++)
					{
						cb.DrawRenderer(renderer, (occluder2.mode == OccluderMode.BlocksSeeThrough) ? fxMatSeeThroughOccluder : fxMatDepthWrite, s);
					}
				}
			}
		}
		return true;
	}

	private bool CheckOcclusion(Camera cam)
	{
		if (!perCameraOcclusionData.TryGetValue(cam, out var occlusionData))
		{
			occlusionData = new PerCameraOcclusionData();
			perCameraOcclusionData[cam] = occlusionData;
		}
		float now = GetTime();
		int frameCount = Time.frameCount;
		if (now - occlusionData.checkLastTime < seeThroughOccluderCheckInterval && Application.isPlaying && occlusionData.occlusionRenderFrame != frameCount)
		{
			return occlusionData.lastOcclusionTestResult;
		}
		occlusionData.checkLastTime = now;
		occlusionData.occlusionRenderFrame = frameCount;
		if (rms == null || rms.Length == 0 || rms[0].renderer == null)
		{
			return false;
		}
		Vector3 camPos = cam.transform.position;
		Quaternion quaternionIdentity = Quaternion.identity;
		if (colliders == null || colliders.Length == 0)
		{
			colliders = new Collider[1];
		}
		if (seeThroughOccluderCheckIndividualObjects)
		{
			for (int r = 0; r < rms.Length; r++)
			{
				if (rms[r].renderer != null)
				{
					Bounds bounds = rms[r].renderer.bounds;
					Vector3 pos = bounds.center;
					float maxDistance = Vector3.Distance(pos, camPos);
					Vector3 extents = bounds.extents * seeThroughOccluderThreshold;
					if (Physics.OverlapBoxNonAlloc(pos, extents, colliders, quaternionIdentity, seeThroughOccluderMask) > 0)
					{
						occlusionData.lastOcclusionTestResult = true;
						return true;
					}
					if (Physics.BoxCast(pos, extents, (camPos - pos).normalized, quaternionIdentity, maxDistance, seeThroughOccluderMask))
					{
						occlusionData.lastOcclusionTestResult = true;
						return true;
					}
				}
			}
			occlusionData.lastOcclusionTestResult = false;
			return false;
		}
		Bounds bounds2 = rms[0].renderer.bounds;
		for (int i = 1; i < rms.Length; i++)
		{
			if (rms[i].renderer != null)
			{
				bounds2.Encapsulate(rms[i].renderer.bounds);
			}
		}
		Vector3 pos2 = bounds2.center;
		Vector3 extents2 = bounds2.extents * seeThroughOccluderThreshold;
		if (Physics.OverlapBoxNonAlloc(pos2, extents2, colliders, quaternionIdentity, seeThroughOccluderMask) > 0)
		{
			occlusionData.lastOcclusionTestResult = true;
			return true;
		}
		float maxDistance2 = Vector3.Distance(pos2, camPos);
		occlusionData.lastOcclusionTestResult = Physics.BoxCast(pos2, extents2, (camPos - pos2).normalized, quaternionIdentity, maxDistance2, seeThroughOccluderMask);
		return occlusionData.lastOcclusionTestResult;
	}

	private void AddWithoutRepetition(List<Renderer> target, List<Renderer> source)
	{
		int sourceCount = source.Count;
		for (int k = 0; k < sourceCount; k++)
		{
			Renderer entry = source[k];
			if (entry != null && !target.Contains(entry) && ValidRenderer(entry))
			{
				target.Add(entry);
			}
		}
	}

	private void CheckOcclusionAccurate(CommandBuffer cbuf, Camera cam)
	{
		if (!perCameraOcclusionData.TryGetValue(cam, out var occlusionData))
		{
			occlusionData = new PerCameraOcclusionData();
			perCameraOcclusionData[cam] = occlusionData;
		}
		float now = GetTime();
		int frameCount = Time.frameCount;
		if (!(now - occlusionData.checkLastTime < seeThroughOccluderCheckInterval) || !Application.isPlaying || occlusionData.occlusionRenderFrame == frameCount)
		{
			if (rms == null || rms.Length == 0 || rms[0].renderer == null)
			{
				return;
			}
			occlusionData.checkLastTime = now;
			occlusionData.occlusionRenderFrame = frameCount;
			Quaternion quaternionIdentity = Quaternion.identity;
			Vector3 camPos = cam.transform.position;
			occlusionData.cachedOccluders.Clear();
			if (occluderHits == null || occluderHits.Length < 50)
			{
				occluderHits = new RaycastHit[50];
			}
			if (seeThroughOccluderCheckIndividualObjects)
			{
				for (int r = 0; r < rms.Length; r++)
				{
					if (rms[r].renderer != null)
					{
						Bounds bounds = rms[r].renderer.bounds;
						Vector3 pos = bounds.center;
						float maxDistance = Vector3.Distance(pos, camPos);
						int numOccluderHits = Physics.BoxCastNonAlloc(pos, bounds.extents * seeThroughOccluderThreshold, (camPos - pos).normalized, occluderHits, quaternionIdentity, maxDistance, seeThroughOccluderMask);
						for (int k = 0; k < numOccluderHits; k++)
						{
							occluderHits[k].collider.transform.root.GetComponentsInChildren(tempRR);
							AddWithoutRepetition(occlusionData.cachedOccluders, tempRR);
						}
					}
				}
			}
			else
			{
				Bounds bounds2 = rms[0].renderer.bounds;
				for (int i = 1; i < rms.Length; i++)
				{
					if (rms[i].renderer != null)
					{
						bounds2.Encapsulate(rms[i].renderer.bounds);
					}
				}
				Vector3 pos2 = bounds2.center;
				float maxDistance2 = Vector3.Distance(pos2, camPos);
				int numOccluderHits2 = Physics.BoxCastNonAlloc(pos2, bounds2.extents * seeThroughOccluderThreshold, (camPos - pos2).normalized, occluderHits, quaternionIdentity, maxDistance2, seeThroughOccluderMask);
				for (int j = 0; j < numOccluderHits2; j++)
				{
					occluderHits[j].collider.transform.root.GetComponentsInChildren(tempRR);
					AddWithoutRepetition(occlusionData.cachedOccluders, tempRR);
				}
			}
		}
		int occluderRenderersCount = occlusionData.cachedOccluders.Count;
		if (occluderRenderersCount <= 0)
		{
			return;
		}
		for (int l = 0; l < occluderRenderersCount; l++)
		{
			Renderer r2 = occlusionData.cachedOccluders[l];
			if (r2 != null)
			{
				cbuf.DrawRenderer(r2, fxMatSeeThroughMask);
			}
		}
	}

	public List<Renderer> GetOccluders(Camera camera)
	{
		if (perCameraOcclusionData.TryGetValue(camera, out var occlusionData))
		{
			return occlusionData.cachedOccluders;
		}
		return null;
	}
}
