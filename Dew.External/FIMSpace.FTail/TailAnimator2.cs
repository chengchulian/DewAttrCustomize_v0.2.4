using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FIMSpace.FTools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FIMSpace.FTail;

[AddComponentMenu("FImpossible Creations/Tail Animator 2")]
[DefaultExecutionOrder(-4)]
public class TailAnimator2 : MonoBehaviour, IDropHandler, IEventSystemHandler, IFHierarchyIcon
{
	[Serializable]
	public class TailSegment
	{
		public Vector3 ProceduralPosition = Vector3.zero;

		public Vector3 ProceduralPositionWeightBlended = Vector3.zero;

		public Quaternion TrueTargetRotation = Quaternion.identity;

		public Quaternion PosRefRotation = Quaternion.identity;

		public Quaternion PreviousPosReferenceRotation = Quaternion.identity;

		public Vector3 PreviousPosition;

		public float BlendValue = 1f;

		public Vector3 BoneDimensionsScaled;

		public float BoneLengthScaled;

		public Vector3 InitialLocalPosition = Vector3.zero;

		public Vector3 InitialLocalPositionInRoot = Vector3.zero;

		public Quaternion InitialLocalRotationInRoot = Quaternion.identity;

		public Vector3 LocalOffset = Vector3.zero;

		public Quaternion InitialLocalRotation = Quaternion.identity;

		public float ColliderRadius = 1f;

		public bool CollisionContactFlag;

		public float CollisionContactRelevancy = -1f;

		public Collision collisionContacts;

		public Vector3 VelocityHelper = Vector3.zero;

		public Quaternion QVelocityHelper = Quaternion.identity;

		public Vector3 PreviousPush = Vector3.zero;

		public Quaternion Curving = Quaternion.identity;

		public Vector3 Gravity = Vector3.zero;

		public Vector3 GravityLookOffset = Vector3.zero;

		public float LengthMultiplier = 1f;

		public float PositionSpeed = 1f;

		public float RotationSpeed = 1f;

		public float Springiness;

		public float Slithery = 1f;

		public float Curling = 0.5f;

		public float Slippery = 1f;

		public Quaternion LastKeyframeLocalRotation;

		public Vector3 LastKeyframeLocalPosition;

		private float deflectionSmoothVelo;

		public TailSegment ParentBone { get; private set; }

		public TailSegment ChildBone { get; private set; }

		public Transform transform { get; private set; }

		public int Index { get; private set; }

		public float IndexOverlLength { get; private set; }

		public float BoneLength { get; private set; }

		public TailCollisionHelper CollisionHelper { get; internal set; }

		public bool IsDetachable { get; private set; }

		public Vector3 LastFinalPosition { get; private set; }

		public Quaternion LastFinalRotation { get; private set; }

		public float DeflectionFactor { get; private set; }

		public Vector3 Deflection { get; private set; }

		public float DeflectionSmooth { get; private set; }

		public Vector3 DeflectionWorldPosition { get; private set; }

		public int DeflectionRelevancy { get; private set; }

		public FImp_ColliderData_Base LatestSelectiveCollision { get; internal set; }

		public TailSegment()
		{
			Index = -1;
			Curving = Quaternion.identity;
			Gravity = Vector3.zero;
			LengthMultiplier = 1f;
			Deflection = Vector3.zero;
			DeflectionFactor = 0f;
			DeflectionRelevancy = -1;
			deflectionSmoothVelo = 0f;
		}

		public TailSegment(Transform transform)
			: this()
		{
			if (!(transform == null))
			{
				this.transform = transform;
				ProceduralPosition = transform.position;
				PreviousPosition = transform.position;
				PosRefRotation = transform.rotation;
				PreviousPosReferenceRotation = PosRefRotation;
				TrueTargetRotation = PosRefRotation;
				ReInitializeLocalPosRot(transform.localPosition, transform.localRotation);
				BoneLength = 0.1f;
			}
		}

		public TailSegment(TailSegment copyFrom)
			: this(copyFrom.transform)
		{
			transform = copyFrom.transform;
			Index = copyFrom.Index;
			IndexOverlLength = copyFrom.IndexOverlLength;
			ProceduralPosition = copyFrom.ProceduralPosition;
			PreviousPosition = copyFrom.PreviousPosition;
			ProceduralPositionWeightBlended = copyFrom.ProceduralPosition;
			PosRefRotation = copyFrom.PosRefRotation;
			PreviousPosReferenceRotation = PosRefRotation;
			TrueTargetRotation = copyFrom.TrueTargetRotation;
			ReInitializeLocalPosRot(copyFrom.InitialLocalPosition, copyFrom.InitialLocalRotation);
		}

		public void ReInitializeLocalPosRot(Vector3 initLocalPos, Quaternion initLocalRot)
		{
			InitialLocalPosition = initLocalPos;
			InitialLocalRotation = initLocalRot;
		}

		public void SetIndex(int i, int tailSegments)
		{
			Index = i;
			if (i < 0)
			{
				IndexOverlLength = 0f;
			}
			else
			{
				IndexOverlLength = (float)i / (float)tailSegments;
			}
		}

		public void SetParentRef(TailSegment parent)
		{
			ParentBone = parent;
			BoneLength = (ProceduralPosition - ParentBone.ProceduralPosition).magnitude;
		}

		public void SetChildRef(TailSegment child)
		{
			ChildBone = child;
		}

		public float GetRadiusScaled()
		{
			return ColliderRadius * transform.lossyScale.x;
		}

		public void AssignDetachedRootCoords(Transform root)
		{
			InitialLocalPositionInRoot = root.InverseTransformPoint(transform.position);
			InitialLocalRotationInRoot = root.rotation.QToLocal(transform.rotation);
			IsDetachable = true;
		}

		internal Vector3 BlendMotionWeight(Vector3 newPosition)
		{
			return Vector3.LerpUnclamped(ParentBone.ProceduralPosition + ParentBone.LastKeyframeLocalRotation.TransformVector(ParentBone.transform.lossyScale, LastKeyframeLocalPosition), newPosition, BlendValue);
		}

		internal void PreCalibrate()
		{
			transform.localPosition = InitialLocalPosition;
			transform.localRotation = InitialLocalRotation;
		}

		internal void Validate()
		{
			if (BoneLength == 0f)
			{
				BoneLength = 0.001f;
			}
		}

		public void RefreshKeyLocalPosition()
		{
			if ((bool)transform)
			{
				LastKeyframeLocalRotation = transform.localRotation;
			}
			else
			{
				LastKeyframeLocalRotation = InitialLocalRotation;
			}
		}

		public void RefreshKeyLocalPositionAndRotation()
		{
			if ((bool)transform)
			{
				RefreshKeyLocalPositionAndRotation(transform.localPosition, transform.localRotation);
			}
			else
			{
				RefreshKeyLocalPositionAndRotation(InitialLocalPosition, InitialLocalRotation);
			}
		}

		public void RefreshKeyLocalPositionAndRotation(Vector3 p, Quaternion r)
		{
			LastKeyframeLocalPosition = p;
			LastKeyframeLocalRotation = r;
		}

		internal Vector3 ParentToFrontOffset()
		{
			return ParentBone.LastKeyframeLocalRotation.TransformVector(ParentBone.transform.lossyScale, LastKeyframeLocalPosition);
		}

		public void RefreshFinalPos(Vector3 pos)
		{
			LastFinalPosition = pos;
		}

		public void RefreshFinalRot(Quaternion rot)
		{
			LastFinalRotation = rot;
		}

		public bool CheckDeflectionState(float zeroWhenLower, float smoothTime, float delta)
		{
			Vector3 deflection = LastKeyframeLocalPosition - ParentBone.transform.InverseTransformVector(ProceduralPosition - ParentBone.ProceduralPosition);
			DeflectionFactor = Vector3.Dot(LastKeyframeLocalPosition.normalized, deflection.normalized);
			if (DeflectionFactor < zeroWhenLower)
			{
				if (smoothTime <= Mathf.Epsilon)
				{
					DeflectionSmooth = 0f;
				}
				else
				{
					DeflectionSmooth = Mathf.SmoothDamp(DeflectionSmooth, 0f - Mathf.Epsilon, ref deflectionSmoothVelo, smoothTime / 1.5f, float.PositiveInfinity, delta);
				}
			}
			else if (smoothTime <= Mathf.Epsilon)
			{
				DeflectionSmooth = 1f;
			}
			else
			{
				DeflectionSmooth = Mathf.SmoothDamp(DeflectionSmooth, 1f, ref deflectionSmoothVelo, smoothTime, float.PositiveInfinity, delta);
			}
			if (DeflectionSmooth <= Mathf.Epsilon)
			{
				return true;
			}
			if (ChildBone.ChildBone != null)
			{
				DeflectionWorldPosition = ChildBone.ChildBone.ProceduralPosition;
			}
			else
			{
				DeflectionWorldPosition = ChildBone.ProceduralPosition;
			}
			return false;
		}

		public bool DeflectionRelevant()
		{
			if (DeflectionRelevancy == -1)
			{
				DeflectionRelevancy = 3;
				return true;
			}
			DeflectionRelevancy = 3;
			return false;
		}

		public bool? DeflectionRestoreState()
		{
			if (DeflectionRelevancy > 0)
			{
				DeflectionRelevancy--;
				if (DeflectionRelevancy == 0)
				{
					DeflectionRelevancy = -1;
					return null;
				}
				return true;
			}
			return false;
		}

		internal void ParamsFrom(TailSegment other)
		{
			BlendValue = other.BlendValue;
			ColliderRadius = other.ColliderRadius;
			Gravity = other.Gravity;
			LengthMultiplier = other.LengthMultiplier;
			BoneLength = other.BoneLength;
			BoneLengthScaled = other.BoneLengthScaled;
			BoneDimensionsScaled = other.BoneDimensionsScaled;
			collisionContacts = other.collisionContacts;
			CollisionHelper = other.CollisionHelper;
			PositionSpeed = other.PositionSpeed;
			RotationSpeed = other.RotationSpeed;
			Springiness = other.Springiness;
			Slithery = other.Slithery;
			Curling = other.Curling;
			Slippery = other.Slippery;
		}

		internal void ParamsFromAll(TailSegment other)
		{
			ParamsFrom(other);
			InitialLocalPosition = other.InitialLocalPosition;
			InitialLocalRotation = other.InitialLocalRotation;
			LastFinalPosition = other.LastFinalPosition;
			LastFinalRotation = other.LastFinalRotation;
			ProceduralPosition = other.ProceduralPosition;
			ProceduralPositionWeightBlended = other.ProceduralPositionWeightBlended;
			TrueTargetRotation = other.TrueTargetRotation;
			PosRefRotation = other.PosRefRotation;
			PreviousPosReferenceRotation = other.PreviousPosReferenceRotation;
			PreviousPosition = other.PreviousPosition;
			BoneLength = other.BoneLength;
			BoneDimensionsScaled = other.BoneDimensionsScaled;
			BoneLengthScaled = other.BoneLengthScaled;
			LocalOffset = other.LocalOffset;
			ColliderRadius = other.ColliderRadius;
			VelocityHelper = other.VelocityHelper;
			QVelocityHelper = other.QVelocityHelper;
			PreviousPush = other.PreviousPush;
		}

		internal void User_ReassignTransform(Transform t)
		{
			transform = t;
		}

		public void Reset()
		{
			PreviousPush = Vector3.zero;
			VelocityHelper = Vector3.zero;
			QVelocityHelper = Quaternion.identity;
			if ((bool)transform)
			{
				ProceduralPosition = transform.position;
				PosRefRotation = transform.rotation;
				PreviousPosReferenceRotation = transform.rotation;
			}
			else if ((bool)ParentBone.transform)
			{
				ProceduralPosition = ParentBone.transform.position + ParentToFrontOffset();
			}
			PreviousPosition = ProceduralPosition;
			ProceduralPositionWeightBlended = ProceduralPosition;
		}
	}

	public enum ECollisionSpace
	{
		World_Slow,
		Selective_Fast
	}

	public enum ECollisionMode
	{
		m_3DCollision,
		m_2DCollision
	}

	[Serializable]
	public class IKBoneSettings
	{
		[Range(0f, 181f)]
		public float AngleLimit = 45f;

		[Range(0f, 181f)]
		public float TwistAngleLimit = 5f;

		public bool UseInChain = true;
	}

	public enum FEWavingType
	{
		Simple,
		Advanced
	}

	public enum EFDeltaType
	{
		DeltaTime,
		SmoothDeltaTime,
		UnscaledDeltaTime,
		FixedDeltaTime,
		SafeDelta
	}

	public enum EAnimationStyle
	{
		Quick,
		Accelerating,
		Linear
	}

	public enum ETailCategory
	{
		Setup,
		Tweak,
		Features,
		Shaping
	}

	public enum ETailFeaturesCategory
	{
		Main,
		Collisions,
		IK,
		Experimental
	}

	public enum EFixedMode
	{
		None,
		Basic,
		Late
	}

	[Tooltip("Using some simple calculations to make tail bend on colliders")]
	public bool UseCollision;

	[Tooltip("How collision should be detected, world gives you collision on all world colliders but with more use of cpu (using unity's rigidbodies), 'Selective' gives you possibility to detect collision on selected colliders without using Rigidbodies, it also gives smoother motion (deactivated colliders will still detect collision, unless its game object is disabled)")]
	public ECollisionSpace CollisionSpace = ECollisionSpace.Selective_Fast;

	public ECollisionMode CollisionMode;

	[Tooltip("If you want to stop checking collision if segment collides with one collider\n\nSegment collision with two or more colliders in the same time with this option enabled can result in stuttery motion")]
	public bool CheapCollision;

	[Tooltip("Using trigger collider to include encountered colliders into collide with list")]
	public bool DynamicWorldCollidersInclusion;

	[Tooltip("Radius of trigger collider for dynamic inclusion of colliders")]
	public float InclusionRadius = 1f;

	public bool IgnoreMeshColliders = true;

	public List<Collider> IncludedColliders;

	public List<Collider2D> IncludedColliders2D;

	protected List<FImp_ColliderData_Base> IncludedCollidersData;

	protected List<FImp_ColliderData_Base> CollidersDataToCheck;

	[Tooltip("Capsules can give much more precise collision detection")]
	public int CollidersType;

	public bool CollideWithOtherTails;

	[Tooltip("Collision with colliders even if they're disabled (but game object must be enabled)\nHelpful to setup character limbs collisions without need to create new Layer")]
	public bool CollideWithDisabledColliders = true;

	[Range(0f, 1f)]
	public float CollisionSlippery = 1f;

	[Tooltip("If tail colliding objects should fit to colliders (0) or be reflect from them (Reflecting Only with 'Slithery' parameter greater than ~0.2)")]
	[Range(0f, 1f)]
	public float ReflectCollision;

	public AnimationCurve CollidersScaleCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	public float CollidersScaleMul = 6.5f;

	[Range(0f, 1f)]
	public float CollisionsAutoCurve = 0.5f;

	public List<Collider> IgnoredColliders;

	public List<Collider2D> IgnoredColliders2D;

	public bool CollidersSameLayer = true;

	[Tooltip("If you add rigidbodies to each tail segment's collider, collision will work on everything but it will be less optimal, you don't have to add here rigidbodies but then you must have not kinematic rigidbodies on objects segments can collide")]
	public bool CollidersAddRigidbody = true;

	public float RigidbodyMass = 1f;

	[FPD_Layers]
	public int CollidersLayer;

	public bool UseSlitheryCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1.2f, 0.1f, 0.8f, 1f, 0.9f)]
	public AnimationCurve SlitheryCurve = AnimationCurve.EaseInOut(0f, 0.75f, 1f, 1f);

	private float lastSlithery = -1f;

	private Keyframe[] lastSlitheryCurvKeys;

	public bool UseCurlingCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.65f, 0.4f, 1f, 0.9f)]
	public AnimationCurve CurlingCurve = AnimationCurve.EaseInOut(0f, 0.7f, 1f, 0.3f);

	private float lastCurling = -1f;

	private Keyframe[] lastCurlingCurvKeys;

	public bool UseSpringCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.9f, 0.7f, 0.2f, 0.9f)]
	public AnimationCurve SpringCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

	private float lastSpringiness = -1f;

	private Keyframe[] lastSpringCurvKeys;

	public bool UseSlipperyCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.2f, 0.9f, 0.6f, 0.9f)]
	public AnimationCurve SlipperyCurve = AnimationCurve.EaseInOut(0f, 0.7f, 1f, 1f);

	private float lastSlippery = -1f;

	private Keyframe[] lastSlipperyCurvKeys;

	public bool UsePosSpeedCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.2f, 1f, 0.3f, 0.9f)]
	public AnimationCurve PosCurve = AnimationCurve.EaseInOut(0f, 0.7f, 1f, 1f);

	private float lastPosSpeeds = -1f;

	private Keyframe[] lastPosCurvKeys;

	public bool UseRotSpeedCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.7f, 0.7f, 0.7f, 0.9f)]
	public AnimationCurve RotCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0.9f);

	private float lastRotSpeeds = -1f;

	private Keyframe[] lastRotCurvKeys;

	[Tooltip("Spreading Tail Animator motion weight over bones")]
	public bool UsePartialBlend;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.2f, 0.5f, 0.85f, 1f)]
	public AnimationCurve BlendCurve = AnimationCurve.EaseInOut(0f, 0.95f, 1f, 0.45f);

	private float lastTailAnimatorAmount = -1f;

	private Keyframe[] lastBlendCurvKeys;

	private TailSegment _ex_bone;

	public bool UseIK;

	private bool ikInitialized;

	[SerializeField]
	private FIK_CCDProcessor IK;

	[Tooltip("Target object to follow by IK")]
	public Transform IKTarget;

	public bool IKAutoWeights = true;

	[Range(0f, 1f)]
	public float IKBaseReactionWeight = 0.65f;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.2f, 0.5f, 0.85f, 1f)]
	public AnimationCurve IKReactionWeightCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0.25f);

	public bool IKAutoAngleLimits = true;

	[FPD_Suffix(0f, 181f, FPD_SuffixAttribute.SuffixMode.FromMinToMaxRounded, "°", true, 0)]
	public float IKAutoAngleLimit = 40f;

	[Tooltip("If ik process should work referencing to previously computed CCDIK pose (can be more precise but need more adjusting in weights and angle limits)")]
	public bool IKContinousSolve;

	[FPD_Suffix(0f, 1f, FPD_SuffixAttribute.SuffixMode.From0to100, "%", true, 0)]
	[Tooltip("How much IK motion sohuld be used in tail animator motion -> 0: turned off")]
	public float IKBlend = 1f;

	[FPD_Suffix(0f, 1f, FPD_SuffixAttribute.SuffixMode.From0to100, "%", true, 0)]
	[Tooltip("If syncing with animator then applying motion of keyframe animation for IK")]
	public float IKAnimatorBlend = 0.5f;

	[Range(1f, 32f)]
	[Tooltip("How much iterations should do CCDIK algorithm in one frame")]
	public int IKReactionQuality = 2;

	[Range(0f, 1f)]
	[Tooltip("Smoothing reactions in CCD IK algorithm")]
	public float IKSmoothing;

	[Range(0f, 1.5f)]
	public float IKStretchToTarget;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.9f, 0.4f, 0.5f, 1f)]
	public AnimationCurve IKStretchCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public List<IKBoneSettings> IKLimitSettings;

	public bool IKSelectiveChain;

	private Vector3? _IKCustomPos;

	private List<TailSegment> _pp_reference;

	private TailSegment _pp_ref_rootParent;

	private TailSegment _pp_ref_lastChild;

	private bool _pp_initialized;

	[Tooltip("Rotation offset for tail (just first (root) bone is rotated)")]
	public Quaternion RotationOffset = Quaternion.identity;

	[Tooltip("Rotate each segment a bit to create curving effect")]
	public Quaternion Curving = Quaternion.identity;

	[Tooltip("Spread curving rotation offset weight over tail segments")]
	public bool UseCurvingCurve;

	[FPD_FixedCurveWindow(0f, -1f, 1f, 1f, 0.75f, 0.75f, 0.75f, 0.85f)]
	public AnimationCurve CurvCurve = AnimationCurve.EaseInOut(0f, 0.75f, 1f, 1f);

	private Quaternion lastCurving = Quaternion.identity;

	private Keyframe[] lastCurvingKeys;

	[Tooltip("Make tail longer or shorter")]
	public float LengthMultiplier = 1f;

	[Tooltip("Spread length multiplier weight over tail segments")]
	public bool UseLengthMulCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 3f, 0f, 1f, 1f, 1f)]
	public AnimationCurve LengthMulCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);

	private float lastLengthMul = 1f;

	private Keyframe[] lastLengthKeys;

	[Tooltip("Spread gravity weight over tail segments")]
	public bool UseGravityCurve;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.85f, 0.35f, 0.25f, 0.85f)]
	[Tooltip("Spread gravity weight over tail segments")]
	public AnimationCurve GravityCurve = AnimationCurve.EaseInOut(0f, 0.65f, 1f, 1f);

	[Tooltip("Simulate gravity weight for tail logics")]
	public Vector3 Gravity = Vector3.zero;

	private Vector3 lastGravity = Vector3.zero;

	private Keyframe[] lastGravityKeys;

	[Tooltip("Using auto waving option to give floating effect")]
	public bool UseWaving = true;

	[Tooltip("Adding some variation to waving animation")]
	public bool CosinusAdd;

	[Tooltip("If you want few tails to wave in the same way you can set this sinus period cycle value")]
	public float FixedCycle;

	[Tooltip("How frequent swings should be")]
	public float WavingSpeed = 3f;

	[Tooltip("How big swings should be")]
	public float WavingRange = 0.8f;

	[Tooltip("What rotation axis should be used in auto waving")]
	public Vector3 WavingAxis = new Vector3(1f, 1f, 1f);

	[Tooltip("Type of waving animation algorithm, it can be simple trigonometric wave or animation based on noises (advanced)")]
	public FEWavingType WavingType = FEWavingType.Advanced;

	[Tooltip("Offsetting perlin noise to generate different variation of tail rotations")]
	public float AlternateWave = 1f;

	private float _waving_waveTime;

	private float _waving_cosTime;

	private Vector3 _waving_sustain = Vector3.zero;

	public bool UseWind;

	[FPD_Suffix(0f, 2.5f, FPD_SuffixAttribute.SuffixMode.PercentageUnclamped, "%", true, 0)]
	public float WindEffectPower = 1f;

	[FPD_Suffix(0f, 2.5f, FPD_SuffixAttribute.SuffixMode.PercentageUnclamped, "%", true, 0)]
	public float WindTurbulencePower = 1f;

	[FPD_Suffix(0f, 1.5f, FPD_SuffixAttribute.SuffixMode.PercentageUnclamped, "%", true, 0)]
	public float WindWorldNoisePower = 0.5f;

	public Vector3 WindEffect = Vector3.zero;

	public List<TailSegment> TailSegments;

	[SerializeField]
	private TailSegment GhostParent;

	[SerializeField]
	private TailSegment GhostChild;

	private Vector3 _limiting_limitPosition = Vector3.zero;

	private Vector3 _limiting_influenceOffset = Vector3.zero;

	private float _limiting_stretchingHelperTooLong;

	private float _limiting_stretchingHelperTooShort;

	private Quaternion _limiting_angle_ToTargetRot;

	private Quaternion _limiting_angle_targetInLocal;

	private Quaternion _limiting_angle_newLocal;

	private Vector3 _tc_segmentGravityOffset = Vector3.zero;

	private Vector3 _tc_segmentGravityToParentDir = Vector3.zero;

	private Vector3 _tc_preGravOff = Vector3.zero;

	[Tooltip("If you want to use max distance fade option to smoothly disable tail animator when object is going far away from camera")]
	public bool UseMaxDistance;

	[Tooltip("(By default camera transform) Measuring distance from this object to define if object is too far and not need to update tail animator")]
	public Transform DistanceFrom;

	[HideInInspector]
	public Transform _distanceFrom_Auto;

	[Tooltip("Max distance to main camera / target object to smoothly turn off tail animator.")]
	public float MaximumDistance = 35f;

	[Tooltip("If object in range should be detected only when is nearer than 'MaxDistance' to avoid stuttery enabled - disable switching")]
	[Range(0f, 1f)]
	public float MaxOutDistanceFactor;

	[Tooltip("If distance should be measured not using Up (y) axis")]
	public bool DistanceWithoutY;

	[Tooltip("Offsetting point from which we want to measure distance to target")]
	public Vector3 DistanceMeasurePoint;

	[Tooltip("Disable fade duration in seconds")]
	[Range(0.25f, 2f)]
	public float FadeDuration = 0.75f;

	private bool maxDistanceExceed;

	private Transform finalDistanceFrom;

	private bool wasCameraSearch;

	private float distanceWeight = 1f;

	private int _tc_startI;

	private int _tc_startII = 1;

	private TailSegment _tc_rootBone;

	private Quaternion _tc_lookRot = Quaternion.identity;

	private Quaternion _tc_targetParentRot = Quaternion.identity;

	private Quaternion _tc_startBoneRotOffset = Quaternion.identity;

	private float _tc_tangle = 1f;

	private float _sg_springVelo = 0.5f;

	private float _sg_curly = 0.5f;

	private Vector3 _sg_push;

	private Vector3 _sg_targetPos;

	private Vector3 _sg_targetChildWorldPosInParentFront;

	private Vector3 _sg_dirToTargetParentFront;

	private Quaternion _sg_orientation;

	private float _sg_slitFactor = 0.5f;

	private bool wasDisabled = true;

	private float justDelta = 0.016f;

	private float secPeriodDelta = 0.5f;

	private float deltaForLerps = 0.016f;

	private float rateDelta = 0.016f;

	protected float collectedDelta;

	protected int framesToSimulate = 1;

	protected int previousframesToSimulate = 1;

	private bool updateTailAnimator;

	private int startAfterTPoseCounter;

	private bool fixedUpdated;

	private bool lateFixedIsRunning;

	private bool fixedAllow = true;

	[Tooltip("Making tail segment deflection influence back segments")]
	[Range(0f, 1f)]
	public float Deflection;

	[FPD_Suffix(1f, 89f, FPD_SuffixAttribute.SuffixMode.FromMinToMaxRounded, "°", true, 0)]
	public float DeflectionStartAngle = 10f;

	[Range(0f, 1f)]
	public float DeflectionSmooth;

	[FPD_FixedCurveWindow(0f, 0f, 1f, 1f, 0.65f, 0.4f, 1f, 0.9f)]
	public AnimationCurve DeflectionFalloff = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[Tooltip("Deflection can be triggered every time tail is waving but you not always would want this feature be enabled (different behaviour of tail motion)")]
	public bool DeflectOnlyCollisions = true;

	private List<TailSegment> _defl_source;

	private float _defl_treshold = 0.01f;

	private bool _forceDisable;

	private float _forceDisableElapsed;

	public ETailCategory _Editor_Category;

	public ETailFeaturesCategory _Editor_FeaturesCategory;

	public bool DrawGizmos = true;

	[Tooltip("First bone of tail motion chain")]
	public Transform StartBone;

	[Tooltip("Finish bone of tail motion chain")]
	public Transform EndBone;

	[Tooltip("Adjusting end point for end tail bone motion")]
	public Vector3 EndBoneJointOffset = Vector3.zero;

	public List<Transform> _TransformsGhostChain;

	public int _GhostChainInitCount = -1;

	protected bool initialized;

	[Tooltip("Target FPS update rate for Tail Animator.\n\nIf you want Tail Animator to behave the same in low/high fps, set this value for example to 60.\nIt also can help optimizing if your game have more than 60 fps.")]
	public int UpdateRate;

	[Tooltip("If your character Unity's Animator have update mode set to 'Animate Physics' you should enable it here too")]
	public EFixedMode AnimatePhysics;

	[Tooltip("When using target fps rate you can interpolate coordinates for smoother effect when object with tail is moving a lot")]
	public bool InterpolateRate;

	[Tooltip("Simulating tail motion at initiation to prevent jiggle start")]
	public bool Prewarm;

	internal float OverrideWeight = 1f;

	protected float conditionalWeight = 1f;

	protected bool collisionInitialized;

	protected bool forceRefreshCollidersData;

	private Vector3 previousWorldPosition;

	protected Transform rootTransform;

	protected bool preAutoCorrect;

	[Tooltip("Blending Slithery - smooth & soft tentacle like movement (value = 1)\nwith more stiff & springy motion (value = 0)\n\n0: Stiff somewhat like tree branch\n1: Soft like squid tentacle / Animal tail")]
	[Range(0f, 1.2f)]
	public float Slithery = 1f;

	[Tooltip("How curly motion should be applied to tail segments")]
	[Range(0f, 1f)]
	public float Curling = 0.5f;

	[Tooltip("Elastic spring effect making motion more 'meaty'")]
	[Range(0f, 1f)]
	public float Springiness;

	[Tooltip("If you want to limit stretching/gumminess of position motion when object moves fast. Recommended adjust to go with it under 0.3 value.\nValue = 1: Unlimited stretching")]
	[Range(0f, 1f)]
	public float MaxStretching = 0.375f;

	[Tooltip("Limiting max rotation angle for each tail segment")]
	[FPD_Suffix(1f, 181f, FPD_SuffixAttribute.SuffixMode.FromMinToMaxRounded, "°", true, 0)]
	public float AngleLimit = 181f;

	[Tooltip("If you need specific axis to be limited.\nLeave unchanged to limit all axes.")]
	public Vector3 AngleLimitAxis = Vector3.zero;

	[Tooltip("If you want limit axes symmetrically leave this parameter unchanged, if you want limit one direction of axis more than reversed, tweak this parameter")]
	public Vector2 LimitAxisRange = Vector2.zero;

	[Tooltip("If limiting shouldn't be too rapidly performed")]
	[Range(0f, 1f)]
	public float LimitSmoothing = 0.5f;

	[Tooltip("If your object moves very fast making tail influenced by speed too much then you can controll it with this parameter")]
	[FPD_Suffix(0f, 1.5f, FPD_SuffixAttribute.SuffixMode.PercentageUnclamped, "%", true, 0)]
	public float MotionInfluence = 1f;

	[Tooltip("Additional Y influence controll useful when your character is jumping (works only when MotionInfluence value is other than 100%)")]
	[Range(0f, 1f)]
	public float MotionInfluenceInY = 1f;

	[Tooltip("If first bone of chain should also be affected with whole chain")]
	public bool IncludeParent = true;

	[Tooltip("By basic algorithm of Tail Animator different sized tails with different number of bones would animate with different bending thanks to this toggle every setup bends in very similar amount.\n\nShort tails will bend more and longer oner with bigger amount of bones less with this option enabled.")]
	[Range(0f, 1f)]
	public float UnifyBendiness;

	[Tooltip("Reaction Speed is defining how fast tail segments will return to target position, it gives animation more underwater/floaty feeling if it's lower")]
	[Range(0f, 1f)]
	public float ReactionSpeed = 0.9f;

	[Tooltip("Sustain is similar to reaction speed in reverse, but providing sustain motion effect when increased")]
	[Range(0f, 1f)]
	public float Sustain;

	[Tooltip("Rotation speed is defining how fast tail segments will return to target rotation, it gives animation more lazy feeling if it's lower")]
	[Range(0f, 1f)]
	public float RotationRelevancy = 1f;

	[Tooltip("Smoothing motion values change over time style to be applied for 'Reaction Speed' and 'Rotation Relevancy' parameters")]
	public EAnimationStyle SmoothingStyle = EAnimationStyle.Accelerating;

	[Tooltip("Slowmo or speedup tail animation reaction")]
	public float TimeScale = 1f;

	[Tooltip("Delta time type to be used by algorithm")]
	public EFDeltaType DeltaType = EFDeltaType.SafeDelta;

	[Tooltip("Useful when you use other components to affect bones hierarchy and you want this component to follow other component's changes\n\nIt can be really useful when working with 'Spine Animator'")]
	public bool UpdateAsLast = true;

	[Tooltip("Checking if keyframed animation has some empty keyframes which could cause unwanted twisting errors")]
	public bool DetectZeroKeyframes = true;

	[Tooltip("Initializing Tail Animator after first frames of game to not initialize with model's T-Pose but after playing some other animation")]
	public bool StartAfterTPose = true;

	[Tooltip("If you want Tail Animator to stop computing when choosed mesh is not visible in any camera view (editor's scene camera is detecting it too)")]
	public Renderer OptimizeWithMesh;

	[Tooltip("If you want to check multiple meshes visibility on screen to define if you want to disable tail animator. (useful when using LOD for skinned mesh renderer)")]
	public Renderer[] OptimizeWithMeshes;

	[Tooltip("Blend Source Animation (keyframed / unanimated) and Tail Animator")]
	[FPD_Suffix(0f, 1f, FPD_SuffixAttribute.SuffixMode.From0to100, "%", true, 0)]
	public float TailAnimatorAmount = 1f;

	[Tooltip("Removing transforms hierachy structure to optimize Unity's calculations on Matrixes.\nIt can give very big boost in performance for long tails but it can't work with animated models!")]
	public bool DetachChildren;

	[Tooltip("If tail movement should not move in depth you can use this parameter")]
	public int Axis2D;

	[Tooltip("[Experimental: Works only with Slithery Blend set to >= 1] Making each segment go to target pose in front of parent segment creating new animation effect")]
	[Range(-1f, 1f)]
	public float Tangle;

	[Tooltip("Making tail animate also roll rotation like it was done in Tail Animator V1 ! Use Rotation Relevancy Parameter (set lower than 0.5) !")]
	public bool AnimateRoll;

	[Tooltip("Overriding keyframe animation with just Tail Animator option (keyframe animation treated as t-pose bones rotations)")]
	[Range(0f, 1f)]
	public float OverrideKeyframeAnimation;

	private Transform _baseTransform;

	public List<Component> DynamicAlwaysInclude { get; private set; }

	public Quaternion WavingRotationOffset { get; private set; }

	public float _TC_TailLength { get; private set; }

	public string EditorIconPath
	{
		get
		{
			if (PlayerPrefs.GetInt("AnimsH", 1) == 0)
			{
				return "";
			}
			return "Tail Animator/Tail Animator Icon Small";
		}
	}

	public bool IsInitialized => initialized;

	public Transform BaseTransform
	{
		get
		{
			if ((bool)_baseTransform)
			{
				return _baseTransform;
			}
			if (_TransformsGhostChain != null && _TransformsGhostChain.Count > 0)
			{
				_baseTransform = _TransformsGhostChain[0];
			}
			if (_baseTransform != null)
			{
				return _baseTransform;
			}
			return base.transform;
		}
	}

	private void RefreshSegmentsColliders()
	{
		if (CollisionSpace == ECollisionSpace.Selective_Fast && TailSegments != null && TailSegments.Count > 1)
		{
			for (int i = 0; i < TailSegments.Count; i++)
			{
				TailSegments[i].ColliderRadius = GetColliderSphereRadiusFor(i);
			}
		}
	}

	private void BeginCollisionsUpdate()
	{
		if (CollisionSpace != ECollisionSpace.Selective_Fast)
		{
			return;
		}
		RefreshIncludedCollidersDataList();
		CollidersDataToCheck.Clear();
		for (int i = 0; i < IncludedCollidersData.Count; i++)
		{
			if (IncludedCollidersData[i].Transform == null)
			{
				forceRefreshCollidersData = true;
				break;
			}
			if (!IncludedCollidersData[i].Transform.gameObject.activeInHierarchy)
			{
				continue;
			}
			if (CollideWithDisabledColliders)
			{
				IncludedCollidersData[i].RefreshColliderData();
				CollidersDataToCheck.Add(IncludedCollidersData[i]);
			}
			else if (CollisionMode == ECollisionMode.m_3DCollision)
			{
				if (IncludedCollidersData[i].Collider == null)
				{
					forceRefreshCollidersData = true;
					break;
				}
				if (IncludedCollidersData[i].Collider.enabled)
				{
					IncludedCollidersData[i].RefreshColliderData();
					CollidersDataToCheck.Add(IncludedCollidersData[i]);
				}
			}
			else
			{
				if (IncludedCollidersData[i].Collider2D == null)
				{
					forceRefreshCollidersData = true;
					break;
				}
				if (IncludedCollidersData[i].Collider2D.enabled)
				{
					IncludedCollidersData[i].RefreshColliderData();
					CollidersDataToCheck.Add(IncludedCollidersData[i]);
				}
			}
		}
	}

	private void SetupSphereColliders()
	{
		if (CollisionSpace == ECollisionSpace.World_Slow)
		{
			for (int i = 1; i < _TransformsGhostChain.Count; i++)
			{
				if (CollidersSameLayer)
				{
					_TransformsGhostChain[i].gameObject.layer = base.gameObject.layer;
				}
				else
				{
					_TransformsGhostChain[i].gameObject.layer = CollidersLayer;
				}
			}
			if (CollidersType != 0)
			{
				for (int j = 1; j < _TransformsGhostChain.Count - 1; j++)
				{
					CapsuleCollider caps = _TransformsGhostChain[j].gameObject.AddComponent<CapsuleCollider>();
					TailCollisionHelper tcol = _TransformsGhostChain[j].gameObject.AddComponent<TailCollisionHelper>().Init(CollidersAddRigidbody, RigidbodyMass);
					tcol.TailCollider = caps;
					tcol.Index = j;
					tcol.ParentTail = this;
					caps.radius = GetColliderSphereRadiusFor(_TransformsGhostChain, j);
					caps.direction = 2;
					caps.height = (_TransformsGhostChain[j].position - _TransformsGhostChain[j + 1].position).magnitude * 2f - caps.radius;
					caps.center = _TransformsGhostChain[j].InverseTransformPoint(Vector3.Lerp(_TransformsGhostChain[j].position, _TransformsGhostChain[j + 1].position, 0.5f));
					TailSegments[j].ColliderRadius = caps.radius;
					TailSegments[j].CollisionHelper = tcol;
				}
			}
			else
			{
				for (int k = 1; k < _TransformsGhostChain.Count; k++)
				{
					SphereCollider s = _TransformsGhostChain[k].gameObject.AddComponent<SphereCollider>();
					TailCollisionHelper tcol2 = _TransformsGhostChain[k].gameObject.AddComponent<TailCollisionHelper>().Init(CollidersAddRigidbody, RigidbodyMass);
					tcol2.TailCollider = s;
					tcol2.Index = k;
					tcol2.ParentTail = this;
					s.radius = GetColliderSphereRadiusFor(_TransformsGhostChain, k);
					TailSegments[k].ColliderRadius = s.radius;
					TailSegments[k].CollisionHelper = tcol2;
				}
			}
		}
		else
		{
			for (int l = 0; l < _TransformsGhostChain.Count; l++)
			{
				TailSegments[l].ColliderRadius = GetColliderSphereRadiusFor(l);
			}
			IncludedCollidersData = new List<FImp_ColliderData_Base>();
			CollidersDataToCheck = new List<FImp_ColliderData_Base>();
			if (DynamicWorldCollidersInclusion)
			{
				if (CollisionMode == ECollisionMode.m_3DCollision)
				{
					for (int m = 0; m < IncludedColliders.Count; m++)
					{
						DynamicAlwaysInclude.Add(IncludedColliders[m]);
					}
				}
				else
				{
					for (int n = 0; n < IncludedColliders2D.Count; n++)
					{
						DynamicAlwaysInclude.Add(IncludedColliders2D[n]);
					}
				}
				Transform middleSegm = TailSegments[TailSegments.Count / 2].transform;
				float scaleRef = Vector3.Distance(_TransformsGhostChain[0].position, _TransformsGhostChain[_TransformsGhostChain.Count - 1].position);
				TailCollisionHelper cHelper = middleSegm.gameObject.AddComponent<TailCollisionHelper>();
				cHelper.ParentTail = this;
				SphereCollider triggerC = null;
				CircleCollider2D triggerC2D = null;
				if (CollisionMode == ECollisionMode.m_3DCollision)
				{
					triggerC = middleSegm.gameObject.AddComponent<SphereCollider>();
					triggerC.isTrigger = true;
					cHelper.TailCollider = triggerC;
				}
				else
				{
					triggerC2D = middleSegm.gameObject.AddComponent<CircleCollider2D>();
					triggerC2D.isTrigger = true;
					cHelper.TailCollider2D = triggerC2D;
				}
				cHelper.Init(addRigidbody: true, 1f, kinematic: true);
				float scale = Mathf.Abs(middleSegm.transform.lossyScale.z);
				if (scale == 0f)
				{
					scale = 1f;
				}
				if (triggerC != null)
				{
					triggerC.radius = scaleRef / scale;
				}
				else
				{
					triggerC2D.radius = scaleRef / scale;
				}
				if (CollidersSameLayer)
				{
					middleSegm.gameObject.layer = base.gameObject.layer;
				}
				else
				{
					middleSegm.gameObject.layer = CollidersLayer;
				}
			}
			RefreshIncludedCollidersDataList();
		}
		collisionInitialized = true;
	}

	internal void CollisionDetection(int index, Collision collision)
	{
		TailSegments[index].collisionContacts = collision;
	}

	internal void ExitCollision(int index)
	{
		TailSegments[index].collisionContacts = null;
	}

	protected bool UseCollisionContact(int index, ref Vector3 pos)
	{
		if (TailSegments[index].collisionContacts == null)
		{
			return false;
		}
		if (TailSegments[index].collisionContacts.contacts.Length == 0)
		{
			return false;
		}
		Collision collision = TailSegments[index].collisionContacts;
		float thisCollRadius = FImp_ColliderData_Sphere.CalculateTrueRadiusOfSphereCollider(TailSegments[index].transform, TailSegments[index].ColliderRadius) * 0.95f;
		if ((bool)collision.collider)
		{
			SphereCollider collidedSphere = collision.collider as SphereCollider;
			if ((bool)collidedSphere)
			{
				FImp_ColliderData_Sphere.PushOutFromSphereCollider(collidedSphere, thisCollRadius, ref pos, Vector3.zero);
			}
			else
			{
				CapsuleCollider collidedCapsule = collision.collider as CapsuleCollider;
				if ((bool)collidedCapsule)
				{
					FImp_ColliderData_Capsule.PushOutFromCapsuleCollider(collidedCapsule, thisCollRadius, ref pos, Vector3.zero);
				}
				else
				{
					BoxCollider collidedBox = collision.collider as BoxCollider;
					if ((bool)collidedBox)
					{
						if ((bool)TailSegments[index].CollisionHelper.RigBody)
						{
							if ((bool)collidedBox.attachedRigidbody)
							{
								if (TailSegments[index].CollisionHelper.RigBody.mass > 1f)
								{
									FImp_ColliderData_Box.PushOutFromBoxCollider(collidedBox, collision, thisCollRadius, ref pos);
									Vector3 pusherPos = pos;
									FImp_ColliderData_Box.PushOutFromBoxCollider(collidedBox, thisCollRadius, ref pos);
									pos = Vector3.Lerp(pos, pusherPos, TailSegments[index].CollisionHelper.RigBody.mass / 5f);
								}
								else
								{
									FImp_ColliderData_Box.PushOutFromBoxCollider(collidedBox, thisCollRadius, ref pos);
								}
							}
							else
							{
								FImp_ColliderData_Box.PushOutFromBoxCollider(collidedBox, thisCollRadius, ref pos);
							}
						}
						else
						{
							FImp_ColliderData_Box.PushOutFromBoxCollider(collidedBox, thisCollRadius, ref pos);
						}
					}
					else
					{
						MeshCollider collidedMesh = collision.collider as MeshCollider;
						if ((bool)collidedMesh)
						{
							FImp_ColliderData_Mesh.PushOutFromMeshCollider(collidedMesh, collision, thisCollRadius, ref pos);
						}
						else
						{
							FImp_ColliderData_Terrain.PushOutFromTerrain(collision.collider as TerrainCollider, thisCollRadius, ref pos);
						}
					}
				}
			}
		}
		return true;
	}

	public void RefreshIncludedCollidersDataList()
	{
		bool refr = false;
		if (CollisionMode == ECollisionMode.m_3DCollision)
		{
			if (IncludedColliders.Count != IncludedCollidersData.Count || forceRefreshCollidersData)
			{
				IncludedCollidersData.Clear();
				for (int i = IncludedColliders.Count - 1; i >= 0; i--)
				{
					if (IncludedColliders[i] == null)
					{
						IncludedColliders.RemoveAt(i);
					}
					else
					{
						FImp_ColliderData_Base colData = FImp_ColliderData_Base.GetColliderDataFor(IncludedColliders[i]);
						IncludedCollidersData.Add(colData);
					}
				}
				refr = true;
			}
		}
		else if (IncludedColliders2D.Count != IncludedCollidersData.Count || forceRefreshCollidersData)
		{
			IncludedCollidersData.Clear();
			for (int i2 = IncludedColliders2D.Count - 1; i2 >= 0; i2--)
			{
				if (IncludedColliders2D[i2] == null)
				{
					IncludedColliders2D.RemoveAt(i2);
				}
				else
				{
					FImp_ColliderData_Base colData2 = FImp_ColliderData_Base.GetColliderDataFor(IncludedColliders2D[i2]);
					IncludedCollidersData.Add(colData2);
				}
			}
			refr = true;
		}
		if (refr)
		{
			forceRefreshCollidersData = false;
		}
	}

	public bool PushIfSegmentInsideCollider(TailSegment bone, ref Vector3 targetPoint)
	{
		bool pushed = false;
		if (!CheapCollision)
		{
			for (int i = 0; i < CollidersDataToCheck.Count; i++)
			{
				bool push = CollidersDataToCheck[i].PushIfInside(ref targetPoint, bone.GetRadiusScaled(), Vector3.zero);
				if (!pushed && push)
				{
					pushed = true;
					bone.LatestSelectiveCollision = CollidersDataToCheck[i];
				}
			}
		}
		else
		{
			for (int j = 0; j < CollidersDataToCheck.Count; j++)
			{
				if (CollidersDataToCheck[j].PushIfInside(ref targetPoint, bone.GetRadiusScaled(), Vector3.zero))
				{
					bone.LatestSelectiveCollision = CollidersDataToCheck[j];
					return true;
				}
			}
		}
		return pushed;
	}

	protected float GetColliderSphereRadiusFor(int i)
	{
		_ = TailSegments[i];
		float refDistance = 1f;
		if (i >= _TransformsGhostChain.Count)
		{
			return refDistance;
		}
		if (_TransformsGhostChain.Count > 1)
		{
			refDistance = Vector3.Distance(_TransformsGhostChain[1].position, _TransformsGhostChain[0].position);
		}
		float singleScale = refDistance;
		if (i != 0)
		{
			singleScale = Mathf.Lerp(refDistance, Vector3.Distance(_TransformsGhostChain[i - 1].position, _TransformsGhostChain[i].position) * 0.5f, CollisionsAutoCurve);
		}
		float div = _TransformsGhostChain.Count - 1;
		if (div <= 0f)
		{
			div = 1f;
		}
		float step = 1f / div;
		return 0.5f * singleScale * CollidersScaleMul * CollidersScaleCurve.Evaluate(step * (float)i);
	}

	protected float GetColliderSphereRadiusFor(List<Transform> transforms, int i)
	{
		float refDistance = 1f;
		if (transforms.Count > 1)
		{
			refDistance = Vector3.Distance(_TransformsGhostChain[1].position, _TransformsGhostChain[0].position);
		}
		float nextDistance = refDistance;
		if (i != 0)
		{
			nextDistance = Vector3.Distance(_TransformsGhostChain[i - 1].position, _TransformsGhostChain[i].position);
		}
		float singleScale = Mathf.Lerp(refDistance, nextDistance * 0.5f, CollisionsAutoCurve);
		float step = 1f / (float)(transforms.Count - 1);
		return 0.5f * singleScale * CollidersScaleMul * CollidersScaleCurve.Evaluate(step * (float)i);
	}

	public void AddCollider(Collider collider)
	{
		if (!IncludedColliders.Contains(collider))
		{
			IncludedColliders.Add(collider);
		}
	}

	public void AddCollider(Collider2D collider)
	{
		if (!IncludedColliders2D.Contains(collider))
		{
			IncludedColliders2D.Add(collider);
		}
	}

	public void CheckForColliderDuplicatesAndNulls()
	{
		for (int i = 0; i < IncludedColliders.Count; i++)
		{
			Collider col = IncludedColliders[i];
			if (IncludedColliders.Count((Collider o) => o == col) > 1)
			{
				IncludedColliders.RemoveAll((Collider o) => o == col);
				IncludedColliders.Add(col);
			}
		}
		for (int i2 = IncludedColliders.Count - 1; i2 >= 0; i2--)
		{
			if (IncludedColliders[i2] == null)
			{
				IncludedColliders.RemoveAt(i2);
			}
		}
	}

	public void CheckForColliderDuplicatesAndNulls2D()
	{
		for (int i = 0; i < IncludedColliders2D.Count; i++)
		{
			Collider2D col = IncludedColliders2D[i];
			if (IncludedColliders2D.Count((Collider2D o) => o == col) > 1)
			{
				IncludedColliders2D.RemoveAll((Collider2D o) => o == col);
				IncludedColliders2D.Add(col);
			}
		}
	}

	private void TailCalculations_ComputeSegmentCollisions(TailSegment bone, ref Vector3 position)
	{
		if (bone.CollisionContactFlag)
		{
			bone.CollisionContactFlag = false;
		}
		else if (bone.CollisionContactRelevancy > 0f)
		{
			bone.CollisionContactRelevancy -= justDelta;
		}
		if (CollisionSpace == ECollisionSpace.Selective_Fast)
		{
			if (PushIfSegmentInsideCollider(bone, ref position))
			{
				bone.CollisionContactFlag = true;
				bone.CollisionContactRelevancy = justDelta * 7f;
				bone.ChildBone.CollisionContactRelevancy = Mathf.Max(bone.ChildBone.CollisionContactRelevancy, justDelta * 3.5f);
				if (bone.ChildBone.ChildBone != null)
				{
					bone.ChildBone.ChildBone.CollisionContactRelevancy = Mathf.Max(bone.ChildBone.CollisionContactRelevancy, justDelta * 3f);
				}
			}
		}
		else if (UseCollisionContact(bone.Index, ref position))
		{
			bone.CollisionContactFlag = true;
			bone.CollisionContactRelevancy = justDelta * 7f;
			bone.ChildBone.CollisionContactRelevancy = Mathf.Max(bone.ChildBone.CollisionContactRelevancy, justDelta * 3.5f);
			if (bone.ChildBone.ChildBone != null)
			{
				bone.ChildBone.ChildBone.CollisionContactRelevancy = Mathf.Max(bone.ChildBone.CollisionContactRelevancy, justDelta * 3f);
			}
		}
	}

	private void ExpertParamsUpdate()
	{
		Expert_UpdatePosSpeed();
		Expert_UpdateRotSpeed();
		Expert_UpdateSpringiness();
		Expert_UpdateSlithery();
		Expert_UpdateCurling();
		Expert_UpdateSlippery();
		Expert_UpdateBlending();
	}

	private void ExpertCurvesEndUpdate()
	{
		lastPosSpeeds = ReactionSpeed;
		if (!UsePosSpeedCurve && lastPosCurvKeys != null)
		{
			lastPosCurvKeys = null;
			lastPosSpeeds += 0.001f;
		}
		lastRotSpeeds = RotationRelevancy;
		if (!UseRotSpeedCurve && lastRotCurvKeys != null)
		{
			lastRotCurvKeys = null;
			lastRotSpeeds += 0.001f;
		}
		lastSpringiness = Springiness;
		if (!UseSpringCurve && lastSpringCurvKeys != null)
		{
			lastSpringCurvKeys = null;
			lastSpringiness += 0.001f;
		}
		lastSlithery = Slithery;
		if (!UseSlitheryCurve && lastSlitheryCurvKeys != null)
		{
			lastSlitheryCurvKeys = null;
			lastSlithery += 0.001f;
		}
		lastCurling = Curling;
		if (!UseCurlingCurve && lastCurlingCurvKeys != null)
		{
			lastCurlingCurvKeys = null;
			lastCurling += 0.001f;
		}
		lastSlippery = CollisionSlippery;
		if (!UseSlipperyCurve && lastSlipperyCurvKeys != null)
		{
			lastSlipperyCurvKeys = null;
			lastSlippery += 0.001f;
		}
		lastTailAnimatorAmount = TailAnimatorAmount;
		if (!UsePartialBlend && lastBlendCurvKeys != null)
		{
			lastBlendCurvKeys = null;
			lastTailAnimatorAmount += 0.001f;
		}
	}

	private void Expert_UpdatePosSpeed()
	{
		if (UsePosSpeedCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.PositionSpeed = PosCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastPosSpeeds != ReactionSpeed)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.PositionSpeed = ReactionSpeed;
			}
		}
	}

	private void Expert_UpdateRotSpeed()
	{
		if (UseRotSpeedCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.RotationSpeed = RotCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastRotSpeeds != RotationRelevancy)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.RotationSpeed = RotationRelevancy;
			}
		}
	}

	private void Expert_UpdateSpringiness()
	{
		if (UseSpringCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Springiness = SpringCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastSpringiness != Springiness)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Springiness = Springiness;
			}
		}
	}

	private void Expert_UpdateSlithery()
	{
		if (UseSlitheryCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Slithery = SlitheryCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastSlithery != Slithery)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Slithery = Slithery;
			}
		}
	}

	private void Expert_UpdateCurling()
	{
		if (UseCurlingCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Curling = CurlingCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastCurling != Curling)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Curling = Curling;
			}
		}
	}

	private void Expert_UpdateSlippery()
	{
		if (UseSlipperyCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Slippery = SlipperyCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastSlippery != CollisionSlippery)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.Slippery = CollisionSlippery;
			}
		}
	}

	private void Expert_UpdateBlending()
	{
		if (UsePartialBlend)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.BlendValue = BlendCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastTailAnimatorAmount != TailAnimatorAmount)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.BlendValue = TailAnimatorAmount;
			}
		}
	}

	private void InitIK()
	{
		if (!IKSelectiveChain)
		{
			IK = new FIK_CCDProcessor(_TransformsGhostChain.ToArray());
		}
		else
		{
			List<Transform> ikChain = new List<Transform>();
			if (IKLimitSettings.Count != _TransformsGhostChain.Count)
			{
				ikChain = _TransformsGhostChain;
			}
			else
			{
				for (int i = 0; i < _TransformsGhostChain.Count; i++)
				{
					if (IKLimitSettings[i].UseInChain)
					{
						ikChain.Add(_TransformsGhostChain[i]);
					}
				}
			}
			IK = new FIK_CCDProcessor(ikChain.ToArray());
		}
		if (IKAutoWeights)
		{
			IK.AutoWeightBones(IKBaseReactionWeight);
		}
		else
		{
			IK.AutoWeightBones(IKReactionWeightCurve);
		}
		if (IKAutoAngleLimits)
		{
			IK.AutoLimitAngle(IKAutoAngleLimit, 4f + IKAutoAngleLimit / 15f);
		}
		if (!IKSelectiveChain)
		{
			IK.Init(_TransformsGhostChain[0]);
		}
		else
		{
			IK.Init(IK.Bones[0].transform);
		}
		ikInitialized = true;
		IK_ApplyLimitBoneSettings();
	}

	public void IKSetCustomPosition(Vector3? tgt)
	{
		_IKCustomPos = tgt;
	}

	private void UpdateIK()
	{
		if (!ikInitialized)
		{
			InitIK();
		}
		if (IKBlend <= Mathf.Epsilon)
		{
			return;
		}
		if (_IKCustomPos.HasValue)
		{
			IK.IKTargetPosition = _IKCustomPos.Value;
		}
		else if (IKTarget == null)
		{
			IK.IKTargetPosition = TailSegments[TailSegments.Count - 1].ProceduralPosition;
		}
		else
		{
			IK.IKTargetPosition = IKTarget.position;
		}
		IK.IKWeight = IKBlend;
		IK.SyncWithAnimator = IKAnimatorBlend;
		IK.ReactionQuality = IKReactionQuality;
		IK.Smoothing = IKSmoothing;
		IK.StretchToTarget = IKStretchToTarget;
		IK.StretchCurve = IKStretchCurve;
		IK.ContinousSolving = IKContinousSolve;
		if (IK.StretchToTarget > 0f)
		{
			IK.ContinousSolving = false;
		}
		if (Axis2D == 3)
		{
			IK.Use2D = true;
		}
		else
		{
			IK.Use2D = false;
		}
		IK.Update();
		if (DetachChildren)
		{
			TailSegment child = TailSegments[0];
			child = TailSegments[1];
			if (!IncludeParent)
			{
				child.RefreshKeyLocalPositionAndRotation(child.InitialLocalPosition, child.InitialLocalRotation);
				child = TailSegments[2];
			}
			while (child != GhostChild)
			{
				child.RefreshKeyLocalPositionAndRotation(child.InitialLocalPosition, child.InitialLocalRotation);
				child = child.ChildBone;
			}
		}
		else
		{
			for (TailSegment child2 = TailSegments[0]; child2 != GhostChild; child2 = child2.ChildBone)
			{
				child2.RefreshKeyLocalPositionAndRotation();
			}
		}
	}

	public void IK_ApplyLimitBoneSettings()
	{
		if (!IKAutoAngleLimits)
		{
			if (IKLimitSettings.Count != _TransformsGhostChain.Count)
			{
				IK_RefreshLimitSettingsContainer();
			}
			if (IK.IKBones.Length != IKLimitSettings.Count)
			{
				Debug.Log("[TAIL ANIMATOR IK] Wrong IK bone count!");
				return;
			}
			if (!IKAutoAngleLimits)
			{
				for (int i = 0; i < IKLimitSettings.Count; i++)
				{
					IK.IKBones[i].AngleLimit = IKLimitSettings[i].AngleLimit;
					IK.IKBones[i].TwistAngleLimit = IKLimitSettings[i].TwistAngleLimit;
				}
			}
		}
		if (ikInitialized)
		{
			if (IKAutoWeights)
			{
				IK.AutoWeightBones(IKBaseReactionWeight);
			}
			else
			{
				IK.AutoWeightBones(IKReactionWeightCurve);
			}
		}
		if (IKAutoAngleLimits)
		{
			IK.AutoLimitAngle(IKAutoAngleLimit, 10f + IKAutoAngleLimit / 10f);
		}
	}

	public void IK_RefreshLimitSettingsContainer()
	{
		IKLimitSettings = new List<IKBoneSettings>();
		for (int i = 0; i < _TransformsGhostChain.Count; i++)
		{
			IKLimitSettings.Add(new IKBoneSettings());
		}
	}

	private bool PostProcessingNeeded()
	{
		if (Deflection > Mathf.Epsilon)
		{
			return true;
		}
		return false;
	}

	private void PostProcessing_Begin()
	{
		TailSegments_UpdateCoordsForRootBone(_pp_reference[_tc_startI]);
		if (Deflection > Mathf.Epsilon)
		{
			Deflection_BeginUpdate();
		}
	}

	private void PostProcessing_ReferenceUpdate()
	{
		TailSegment child;
		for (child = _pp_reference[_tc_startI]; child != _pp_ref_lastChild; child = child.ChildBone)
		{
			child.ParamsFrom(TailSegments[child.Index]);
			TailSegment_PrepareVelocity(child);
		}
		TailSegment_PrepareMotionParameters(_pp_ref_lastChild);
		TailSegment_PrepareVelocity(_pp_ref_lastChild);
		child = _pp_reference[_tc_startII];
		if (!DetachChildren)
		{
			while (child != _pp_ref_lastChild)
			{
				TailSegment_PrepareRotation(child);
				TailSegment_BaseSwingProcessing(child);
				TailCalculations_SegmentPreProcessingStack(child);
				TailSegment_PreRotationPositionBlend(child);
				child = child.ChildBone;
			}
		}
		else
		{
			while (child != _pp_ref_lastChild)
			{
				TailSegment_PrepareRotationDetached(child);
				TailSegment_BaseSwingProcessing(child);
				TailCalculations_SegmentPreProcessingStack(child);
				TailSegment_PreRotationPositionBlend(child);
				child = child.ChildBone;
			}
		}
		TailCalculations_UpdateArtificialChildBone(_pp_ref_lastChild);
		for (child = _pp_reference[_tc_startII]; child != _pp_ref_lastChild; child = child.ChildBone)
		{
			TailCalculations_SegmentRotation(child, child.LastKeyframeLocalPosition);
		}
		TailCalculations_SegmentRotation(child, child.LastKeyframeLocalPosition);
		child.ParentBone.RefreshFinalRot(child.ParentBone.TrueTargetRotation);
	}

	private void ShapingParamsUpdate()
	{
		Shaping_UpdateCurving();
		Shaping_UpdateGravity();
		Shaping_UpdateLengthMultiplier();
	}

	private void Shaping_UpdateCurving()
	{
		if (!Curving.QIsZero())
		{
			if (UseCurvingCurve)
			{
				for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
				{
					_ex_bone.Curving = Quaternion.LerpUnclamped(Quaternion.identity, Curving, CurvCurve.Evaluate(_ex_bone.IndexOverlLength));
				}
			}
			else if (!Curving.QIsSame(lastCurving))
			{
				for (int i = 0; i < TailSegments.Count; i++)
				{
					TailSegments[i].Curving = Curving;
				}
			}
		}
		else if (!Curving.QIsSame(lastCurving))
		{
			for (int j = 0; j < TailSegments.Count; j++)
			{
				TailSegments[j].Curving = Quaternion.identity;
			}
		}
	}

	private void Shaping_UpdateGravity()
	{
		if (!Gravity.VIsZero())
		{
			if (UseGravityCurve)
			{
				for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
				{
					_ex_bone.Gravity = Gravity * 40f * GravityCurve.Evaluate(_ex_bone.IndexOverlLength);
				}
			}
			else if (!Gravity.VIsSame(lastGravity))
			{
				for (int i = 0; i < TailSegments.Count; i++)
				{
					TailSegments[i].Gravity = Gravity / 40f;
					TailSegments[i].Gravity *= 1f + (float)TailSegments[i].Index / 2f * (1f - TailSegments[i].Slithery);
				}
			}
		}
		else if (!Gravity.VIsSame(lastGravity))
		{
			for (int j = 0; j < TailSegments.Count; j++)
			{
				TailSegments[j].Gravity = Vector3.zero;
				TailSegments[j].GravityLookOffset = Vector3.zero;
			}
		}
	}

	private void Shaping_UpdateLengthMultiplier()
	{
		if (UseLengthMulCurve)
		{
			for (_ex_bone = TailSegments[0]; _ex_bone != null; _ex_bone = _ex_bone.ChildBone)
			{
				_ex_bone.LengthMultiplier = LengthMulCurve.Evaluate(_ex_bone.IndexOverlLength);
			}
		}
		else if (lastLengthMul != LengthMultiplier)
		{
			for (int i = 0; i < TailSegments.Count; i++)
			{
				TailSegments[i].LengthMultiplier = LengthMultiplier;
			}
		}
	}

	private void ShapingEndUpdate()
	{
		lastCurving = Curving;
		if (!UseCurvingCurve && lastCurvingKeys != null)
		{
			lastCurvingKeys = null;
			lastCurving.x += 0.001f;
		}
		lastGravity = Gravity;
		if (!UseGravityCurve && lastGravityKeys != null)
		{
			lastGravityKeys = null;
			lastGravity.x += 0.001f;
		}
		lastLengthMul = LengthMultiplier;
		if (!UseLengthMulCurve && lastLengthKeys != null)
		{
			lastLengthKeys = null;
			lastLengthMul += 0.0001f;
		}
	}

	private void Waving_Initialize()
	{
		if (FixedCycle == 0f)
		{
			_waving_waveTime = global::UnityEngine.Random.Range(-MathF.PI, MathF.PI) * 100f;
			_waving_cosTime = global::UnityEngine.Random.Range(-MathF.PI, MathF.PI) * 50f;
		}
		else
		{
			_waving_waveTime = FixedCycle;
			_waving_cosTime = FixedCycle;
		}
		_waving_sustain = Vector3.zero;
	}

	private void Waving_AutoSwingUpdate()
	{
		_waving_waveTime += justDelta * (2f * WavingSpeed);
		if (WavingType == FEWavingType.Simple)
		{
			float sinVal = Mathf.Sin(_waving_waveTime) * (30f * WavingRange);
			if (CosinusAdd)
			{
				_waving_cosTime += justDelta * (2.535f * WavingSpeed);
				sinVal += Mathf.Cos(_waving_cosTime) * (27f * WavingRange);
			}
			WavingRotationOffset = Quaternion.Euler(sinVal * WavingAxis * TailSegments[0].BlendValue);
		}
		else
		{
			float perTime = _waving_waveTime * 0.23f;
			float altX = AlternateWave * -5f;
			float altY = AlternateWave * 100f;
			float x = AlternateWave * 20f;
			float x2 = Mathf.PerlinNoise(perTime, altX) * 2f - 1f;
			float y = Mathf.PerlinNoise(altY + perTime, perTime + altY) * 2f - 1f;
			float z = Mathf.PerlinNoise(x, perTime) * 2f - 1f;
			WavingRotationOffset = Quaternion.Euler(Vector3.Scale(WavingAxis * WavingRange * 35f * TailSegments[0].BlendValue, new Vector3(x2, y, z)));
		}
	}

	private void Waving_SustainUpdate()
	{
		TailSegment firstB = TailSegments[0];
		float tailRatio = _TC_TailLength / (float)TailSegments.Count;
		tailRatio = Mathf.Pow(tailRatio, 1.65f);
		tailRatio = _sg_curly / tailRatio / 6f;
		if (tailRatio < 0.1f)
		{
			tailRatio = 0.1f;
		}
		else if (tailRatio > 1f)
		{
			tailRatio = 1f;
		}
		int mid = (int)Mathf.LerpUnclamped((float)TailSegments.Count * 0.4f, (float)TailSegments.Count * 0.6f, Sustain);
		float susFact = FEasing.EaseOutExpo(1f, 0.09f, Sustain);
		float mild = 1.5f;
		mild *= 1f - TailSegments[0].Curling / 8f;
		mild *= 1.5f - tailRatio / 1.65f;
		mild *= Mathf.Lerp(0.7f, 1.2f, firstB.Slithery);
		mild *= FEasing.EaseOutExpo(1f, susFact, firstB.Springiness);
		Vector3 velo = TailSegments[mid].PreviousPush;
		if (mid + 1 < TailSegments.Count)
		{
			velo += TailSegments[mid + 1].PreviousPush;
		}
		if (mid - 1 > TailSegments.Count)
		{
			velo += TailSegments[mid - 1].PreviousPush;
		}
		_waving_sustain = velo * Sustain * mild * 2f;
	}

	private void WindEffectUpdate()
	{
		if ((bool)TailAnimatorWind.Instance)
		{
			TailAnimatorWind.Instance.AffectTailWithWind(this);
		}
	}

	protected virtual void Init()
	{
		if (initialized)
		{
			return;
		}
		if (_TransformsGhostChain == null || _TransformsGhostChain.Count == 0)
		{
			_TransformsGhostChain = new List<Transform>();
			GetGhostChain();
		}
		TailSegments = new List<TailSegment>();
		for (int i = 0; i < _TransformsGhostChain.Count; i++)
		{
			if (_TransformsGhostChain[i] == null)
			{
				Debug.Log("[Tail Animator] Null bones in " + base.name + " !");
				continue;
			}
			TailSegment b = new TailSegment(_TransformsGhostChain[i]);
			b.SetIndex(i, _TransformsGhostChain.Count);
			TailSegments.Add(b);
		}
		if (TailSegments.Count == 0)
		{
			Debug.Log("[Tail Animator] Could not create tail bones chain in " + base.name + " !");
			return;
		}
		_TC_TailLength = 0f;
		_baseTransform = _TransformsGhostChain[0];
		for (int j = 0; j < TailSegments.Count; j++)
		{
			TailSegment current = TailSegments[j];
			TailSegment parent;
			if (j == 0)
			{
				if ((bool)current.transform.parent)
				{
					parent = new TailSegment(current.transform.parent);
					parent.SetParentRef(new TailSegment(parent.transform.parent));
				}
				else
				{
					parent = new TailSegment(current.transform);
					Vector3 toStartDir;
					if (_TransformsGhostChain.Count > 1)
					{
						toStartDir = _TransformsGhostChain[0].position - _TransformsGhostChain[1].position;
						if (toStartDir.magnitude == 0f)
						{
							toStartDir = base.transform.position - _TransformsGhostChain[1].position;
						}
					}
					else
					{
						toStartDir = current.transform.position - _TransformsGhostChain[0].position;
					}
					if (toStartDir.magnitude == 0f)
					{
						toStartDir = base.transform.position - _TransformsGhostChain[0].position;
					}
					if (toStartDir.magnitude == 0f)
					{
						toStartDir = base.transform.forward;
					}
					parent.LocalOffset = parent.transform.InverseTransformPoint(parent.transform.position + toStartDir);
					parent.SetParentRef(new TailSegment(current.transform));
				}
				GhostParent = parent;
				GhostParent.Validate();
				current.SetParentRef(GhostParent);
			}
			else
			{
				parent = TailSegments[j - 1];
				current.ReInitializeLocalPosRot(parent.transform.InverseTransformPoint(current.transform.position), current.transform.localRotation);
			}
			if (j == TailSegments.Count - 1)
			{
				Transform childT = null;
				if (current.transform.childCount > 0)
				{
					childT = current.transform.GetChild(0);
				}
				GhostChild = new TailSegment(childT);
				Vector3 scaleDir = ((!EndBoneJointOffset.VIsZero()) ? current.transform.TransformVector(EndBoneJointOffset) : (current.transform.parent ? (current.transform.position - current.transform.parent.position) : ((current.transform.childCount <= 0) ? (current.transform.TransformDirection(Vector3.forward) * 0.05f) : (current.transform.GetChild(0).position - current.transform.position))));
				GhostChild.ProceduralPosition = current.transform.position + scaleDir;
				GhostChild.ProceduralPositionWeightBlended = GhostChild.ProceduralPosition;
				GhostChild.PreviousPosition = GhostChild.ProceduralPosition;
				GhostChild.PosRefRotation = Quaternion.identity;
				GhostChild.PreviousPosReferenceRotation = Quaternion.identity;
				GhostChild.ReInitializeLocalPosRot(current.transform.InverseTransformPoint(GhostChild.ProceduralPosition), Quaternion.identity);
				GhostChild.RefreshFinalPos(GhostChild.ProceduralPosition);
				GhostChild.RefreshFinalRot(GhostChild.PosRefRotation);
				GhostChild.TrueTargetRotation = GhostChild.PosRefRotation;
				current.TrueTargetRotation = current.transform.rotation;
				current.SetChildRef(GhostChild);
				GhostChild.SetParentRef(current);
			}
			else
			{
				current.SetChildRef(TailSegments[j + 1]);
			}
			current.SetParentRef(parent);
			_TC_TailLength += Vector3.Distance(current.ProceduralPosition, parent.ProceduralPosition);
			if (current.transform != _baseTransform)
			{
				current.AssignDetachedRootCoords(BaseTransform);
			}
		}
		GhostParent.SetIndex(-1, TailSegments.Count);
		GhostChild.SetIndex(TailSegments.Count, TailSegments.Count);
		GhostParent.SetChildRef(TailSegments[0]);
		previousWorldPosition = BaseTransform.position;
		WavingRotationOffset = Quaternion.identity;
		if (CollidersDataToCheck == null)
		{
			CollidersDataToCheck = new List<FImp_ColliderData_Base>();
		}
		DynamicAlwaysInclude = new List<Component>();
		if (UseCollision)
		{
			SetupSphereColliders();
		}
		if (_defl_source == null)
		{
			_defl_source = new List<TailSegment>();
		}
		Waving_Initialize();
		if (DetachChildren)
		{
			DetachChildrenTransforms();
		}
		initialized = true;
		if (TailSegments.Count == 1 && TailSegments[0].transform.parent == null)
		{
			Debug.Log("[Tail Animator] Can't initialize one-bone length chain on bone which don't have any parent!");
			Debug.LogError("[Tail Animator] Can't initialize one-bone length chain on bone which don't have any parent!");
			TailAnimatorAmount = 0f;
			initialized = false;
			return;
		}
		if (UseWind)
		{
			TailAnimatorWind.Refresh();
		}
		if (PostProcessingNeeded() && !_pp_initialized)
		{
			InitializePostProcessing();
		}
		if (!Prewarm)
		{
			return;
		}
		ShapingParamsUpdate();
		ExpertParamsUpdate();
		Update();
		LateUpdate();
		justDelta = rateDelta;
		secPeriodDelta = 1f;
		deltaForLerps = secPeriodDelta;
		rateDelta = 1f / 60f;
		CheckIfTailAnimatorShouldBeUpdated();
		if (updateTailAnimator)
		{
			int loopCount = 60 + TailSegments.Count / 2;
			for (int d = 0; d < loopCount; d++)
			{
				PreCalibrateBones();
				LateUpdate();
			}
		}
	}

	public void DetachChildrenTransforms()
	{
		int to = ((!IncludeParent) ? 1 : 0);
		for (int i = TailSegments.Count - 1; i >= to; i--)
		{
			if ((bool)TailSegments[i].transform)
			{
				TailSegments[i].transform.DetachChildren();
			}
		}
	}

	private void InitializePostProcessing()
	{
		_pp_reference = new List<TailSegment>();
		_pp_ref_rootParent = new TailSegment(GhostParent);
		for (int i = 0; i < TailSegments.Count; i++)
		{
			TailSegment bone = new TailSegment(TailSegments[i]);
			_pp_reference.Add(bone);
		}
		_pp_ref_lastChild = new TailSegment(GhostChild);
		_pp_ref_rootParent.SetChildRef(_pp_reference[0]);
		_pp_ref_rootParent.SetParentRef(new TailSegment(GhostParent.ParentBone.transform));
		for (int j = 0; j < _pp_reference.Count; j++)
		{
			TailSegment bone2 = _pp_reference[j];
			bone2.SetIndex(j, TailSegments.Count);
			if (j == 0)
			{
				bone2.SetParentRef(_pp_ref_rootParent);
				bone2.SetChildRef(_pp_reference[j + 1]);
			}
			else if (j == _pp_reference.Count - 1)
			{
				bone2.SetParentRef(_pp_reference[j - 1]);
				bone2.SetChildRef(_pp_ref_lastChild);
			}
			else
			{
				bone2.SetParentRef(_pp_reference[j - 1]);
				bone2.SetChildRef(_pp_reference[j + 1]);
			}
		}
		_pp_ref_lastChild.SetParentRef(_pp_reference[_pp_reference.Count - 1]);
		_pp_initialized = true;
	}

	protected void StretchingLimiting(TailSegment bone)
	{
		Vector3 backDir = bone.ParentBone.ProceduralPosition - bone.ProceduralPosition;
		float dist = backDir.magnitude;
		if (!(dist > 0f))
		{
			return;
		}
		float maxDist = bone.BoneLengthScaled + bone.BoneLengthScaled * 2.5f * MaxStretching;
		if (dist > maxDist)
		{
			if (MaxStretching == 0f)
			{
				_limiting_limitPosition = bone.ProceduralPosition + backDir * ((dist - bone.BoneLengthScaled) / dist);
				bone.ProceduralPosition = _limiting_limitPosition;
				return;
			}
			_limiting_limitPosition = bone.ParentBone.ProceduralPosition - backDir.normalized * maxDist;
			float limValue = Mathf.InverseLerp(dist, 0f, maxDist) + _limiting_stretchingHelperTooLong;
			if (limValue > 0.999f)
			{
				limValue = 0.99f;
			}
			if (ReactionSpeed < 0.5f)
			{
				limValue *= deltaForLerps * (10f + ReactionSpeed * 30f);
			}
			bone.ProceduralPosition = Vector3.Lerp(bone.ProceduralPosition, _limiting_limitPosition, limValue);
			return;
		}
		maxDist = bone.BoneLengthScaled + bone.BoneLengthScaled * 1.1f * MaxStretching;
		if (dist < maxDist)
		{
			_limiting_limitPosition = bone.ProceduralPosition + backDir * ((dist - bone.BoneLengthScaled) / dist);
			if (MaxStretching == 0f)
			{
				bone.ProceduralPosition = _limiting_limitPosition;
			}
			else
			{
				bone.ProceduralPosition = Vector3.LerpUnclamped(bone.ProceduralPosition, _limiting_limitPosition, Mathf.InverseLerp(dist, 0f, maxDist) + _limiting_stretchingHelperTooShort);
			}
		}
	}

	protected Vector3 AngleLimiting(TailSegment child, Vector3 targetPos)
	{
		float angleFactor = 0f;
		_limiting_limitPosition = targetPos;
		_limiting_angle_ToTargetRot = Quaternion.FromToRotation(child.ParentBone.transform.TransformDirection(child.LastKeyframeLocalPosition), targetPos - child.ParentBone.ProceduralPosition) * child.ParentBone.transform.rotation;
		_limiting_angle_targetInLocal = child.ParentBone.transform.rotation.QToLocal(_limiting_angle_ToTargetRot);
		float angleDiffToInitPose = 0f;
		if (AngleLimitAxis.sqrMagnitude == 0f)
		{
			angleDiffToInitPose = Quaternion.Angle(_limiting_angle_targetInLocal, child.LastKeyframeLocalRotation);
		}
		else
		{
			AngleLimitAxis.Normalize();
			if (LimitAxisRange.x == LimitAxisRange.y)
			{
				angleDiffToInitPose = Mathf.DeltaAngle(Vector3.Scale(child.InitialLocalRotation.eulerAngles, AngleLimitAxis).magnitude, Vector3.Scale(_limiting_angle_targetInLocal.eulerAngles, AngleLimitAxis).magnitude);
				if (angleDiffToInitPose < 0f)
				{
					angleDiffToInitPose = 0f - angleDiffToInitPose;
				}
			}
			else
			{
				angleDiffToInitPose = Mathf.DeltaAngle(Vector3.Scale(child.InitialLocalRotation.eulerAngles, AngleLimitAxis).magnitude, Vector3.Scale(_limiting_angle_targetInLocal.eulerAngles, AngleLimitAxis).magnitude);
				if (angleDiffToInitPose > LimitAxisRange.x && angleDiffToInitPose < LimitAxisRange.y)
				{
					angleDiffToInitPose = 0f;
				}
				if (angleDiffToInitPose < 0f)
				{
					angleDiffToInitPose = 0f - angleDiffToInitPose;
				}
			}
		}
		if (angleDiffToInitPose > AngleLimit)
		{
			float exceededAngle = Mathf.Abs(Mathf.DeltaAngle(angleDiffToInitPose, AngleLimit));
			angleFactor = Mathf.InverseLerp(0f, AngleLimit, exceededAngle);
			if (LimitSmoothing > Mathf.Epsilon)
			{
				float smooth = Mathf.Lerp(55f, 15f, LimitSmoothing);
				_limiting_angle_newLocal = Quaternion.SlerpUnclamped(_limiting_angle_targetInLocal, child.LastKeyframeLocalRotation, deltaForLerps * smooth * angleFactor);
			}
			else
			{
				_limiting_angle_newLocal = Quaternion.SlerpUnclamped(_limiting_angle_targetInLocal, child.LastKeyframeLocalRotation, angleFactor);
			}
			_limiting_angle_ToTargetRot = child.ParentBone.transform.rotation.QToWorld(_limiting_angle_newLocal);
			_limiting_limitPosition = child.ParentBone.ProceduralPosition + _limiting_angle_ToTargetRot * Vector3.Scale(child.transform.lossyScale, child.LastKeyframeLocalPosition);
		}
		if (angleFactor > Mathf.Epsilon)
		{
			return _limiting_limitPosition;
		}
		return targetPos;
	}

	private void MotionInfluenceLimiting()
	{
		if (MotionInfluence != 1f)
		{
			_limiting_influenceOffset = (BaseTransform.position - previousWorldPosition) * (1f - MotionInfluence);
			if (MotionInfluenceInY < 1f)
			{
				_limiting_influenceOffset.y = (BaseTransform.position.y - previousWorldPosition.y) * (1f - MotionInfluenceInY);
			}
			for (int i = 0; i < TailSegments.Count; i++)
			{
				TailSegments[i].ProceduralPosition += _limiting_influenceOffset;
				TailSegments[i].PreviousPosition += _limiting_influenceOffset;
			}
			GhostChild.ProceduralPosition += _limiting_influenceOffset;
			GhostChild.PreviousPosition += _limiting_influenceOffset;
		}
	}

	private void CalculateGravityPositionOffsetForSegment(TailSegment bone)
	{
		_tc_segmentGravityOffset = (bone.Gravity + WindEffect) * bone.BoneLengthScaled;
		_tc_segmentGravityToParentDir = bone.ProceduralPosition - bone.ParentBone.ProceduralPosition;
		_tc_preGravOff = (_tc_segmentGravityToParentDir + _tc_segmentGravityOffset).normalized * _tc_segmentGravityToParentDir.magnitude;
		bone.ProceduralPosition = bone.ParentBone.ProceduralPosition + _tc_preGravOff;
	}

	private void Axis2DLimit(TailSegment child)
	{
		child.ProceduralPosition -= child.ParentBone.transform.VAxis2DLimit(child.ParentBone.ProceduralPosition, child.ProceduralPosition, Axis2D);
	}

	public float GetDistanceMeasure(Vector3 targetPosition)
	{
		if (DistanceWithoutY)
		{
			Vector3 p = BaseTransform.position + BaseTransform.TransformVector(DistanceMeasurePoint);
			return Vector2.Distance(new Vector2(p.x, p.z), new Vector2(targetPosition.x, targetPosition.z));
		}
		return Vector3.Distance(BaseTransform.position + BaseTransform.TransformVector(DistanceMeasurePoint), targetPosition);
	}

	private void MaxDistanceCalculations()
	{
		if (DistanceFrom != null)
		{
			finalDistanceFrom = DistanceFrom;
		}
		else if (finalDistanceFrom == null)
		{
			if (_distanceFrom_Auto == null)
			{
				Camera c = Camera.main;
				if ((bool)c)
				{
					_distanceFrom_Auto = c.transform;
				}
				else if (!wasCameraSearch)
				{
					c = global::UnityEngine.Object.FindObjectOfType<Camera>();
					if ((bool)c)
					{
						_distanceFrom_Auto = c.transform;
					}
					wasCameraSearch = true;
				}
			}
			finalDistanceFrom = _distanceFrom_Auto;
		}
		if (MaximumDistance > 0f && finalDistanceFrom != null)
		{
			if (!maxDistanceExceed)
			{
				if (GetDistanceMeasure(finalDistanceFrom.position) > MaximumDistance + MaximumDistance * MaxOutDistanceFactor)
				{
					maxDistanceExceed = true;
				}
				distanceWeight += Time.unscaledDeltaTime * (1f / FadeDuration);
				if (distanceWeight > 1f)
				{
					distanceWeight = 1f;
				}
			}
			else
			{
				if (GetDistanceMeasure(finalDistanceFrom.position) <= MaximumDistance)
				{
					maxDistanceExceed = false;
				}
				distanceWeight -= Time.unscaledDeltaTime * (1f / FadeDuration);
				if (distanceWeight < 0f)
				{
					distanceWeight = 0f;
				}
			}
		}
		else
		{
			maxDistanceExceed = false;
			distanceWeight = 1f;
		}
	}

	private Vector3 TailCalculations_SmoothPosition(Vector3 from, Vector3 to, TailSegment bone)
	{
		if (SmoothingStyle == EAnimationStyle.Accelerating)
		{
			return TailCalculations_SmoothPositionSmoothDamp(from, to, ref bone.VelocityHelper, bone.PositionSpeed);
		}
		if (SmoothingStyle == EAnimationStyle.Quick)
		{
			return TailCalculations_SmoothPositionLerp(from, to, bone.PositionSpeed);
		}
		return TailCalculations_SmoothPositionLinear(from, to, bone.PositionSpeed);
	}

	private Vector3 TailCalculations_SmoothPositionLerp(Vector3 from, Vector3 to, float speed)
	{
		return Vector3.Lerp(from, to, secPeriodDelta * speed);
	}

	private Vector3 TailCalculations_SmoothPositionSmoothDamp(Vector3 from, Vector3 to, ref Vector3 velo, float speed)
	{
		return Vector3.SmoothDamp(from, to, ref velo, Mathf.LerpUnclamped(0.08f, 0.0001f, Mathf.Sqrt(Mathf.Sqrt(speed))), float.PositiveInfinity, rateDelta);
	}

	private Vector3 TailCalculations_SmoothPositionLinear(Vector3 from, Vector3 to, float speed)
	{
		return Vector3.MoveTowards(from, to, deltaForLerps * speed * 45f);
	}

	private Quaternion TailCalculations_SmoothRotation(Quaternion from, Quaternion to, TailSegment bone)
	{
		if (SmoothingStyle == EAnimationStyle.Accelerating)
		{
			return TailCalculations_SmoothRotationSmoothDamp(from, to, ref bone.QVelocityHelper, bone.RotationSpeed);
		}
		if (SmoothingStyle == EAnimationStyle.Quick)
		{
			return TailCalculations_SmoothRotationLerp(from, to, bone.RotationSpeed);
		}
		return TailCalculations_SmoothRotationLinear(from, to, bone.RotationSpeed);
	}

	private Quaternion TailCalculations_SmoothRotationLerp(Quaternion from, Quaternion to, float speed)
	{
		return Quaternion.Lerp(from, to, secPeriodDelta * speed);
	}

	private Quaternion TailCalculations_SmoothRotationSmoothDamp(Quaternion from, Quaternion to, ref Quaternion velo, float speed)
	{
		return from.SmoothDampRotation(to, ref velo, Mathf.LerpUnclamped(0.25f, 0.0001f, Mathf.Sqrt(Mathf.Sqrt(speed))), rateDelta);
	}

	private Quaternion TailCalculations_SmoothRotationLinear(Quaternion from, Quaternion to, float speed)
	{
		return Quaternion.RotateTowards(from, to, speed * deltaForLerps * 1600f);
	}

	private void TailCalculations_Begin()
	{
		if (IncludeParent)
		{
			_tc_startI = 0;
			_tc_rootBone = TailSegments[0];
		}
		else
		{
			_tc_startI = 1;
			if (TailSegments.Count > 1)
			{
				_tc_rootBone = TailSegments[1];
			}
			else
			{
				_tc_rootBone = TailSegments[0];
				_tc_startI = -1;
			}
		}
		_tc_startII = _tc_startI + 1;
		if (_tc_startII > TailSegments.Count - 1)
		{
			_tc_startII = -1;
		}
		if (Deflection > Mathf.Epsilon && !_pp_initialized)
		{
			InitializePostProcessing();
		}
		if (Tangle < 0f)
		{
			_tc_tangle = Mathf.LerpUnclamped(1f, 1.5f, Tangle + 1f);
		}
		else
		{
			_tc_tangle = Mathf.LerpUnclamped(1f, -4f, Tangle);
		}
	}

	private void TailSegments_UpdateRootFeatures()
	{
		if (UseWaving)
		{
			Waving_AutoSwingUpdate();
			_tc_startBoneRotOffset = WavingRotationOffset * RotationOffset;
		}
		else
		{
			_tc_startBoneRotOffset = RotationOffset;
		}
		if (Sustain > Mathf.Epsilon)
		{
			Waving_SustainUpdate();
		}
		if (PostProcessingNeeded())
		{
			PostProcessing_Begin();
		}
	}

	private void TailCalculations_SegmentPreProcessingStack(TailSegment child)
	{
		if (!UseCollision)
		{
			if (AngleLimit < 181f)
			{
				child.ProceduralPosition = AngleLimiting(child, child.ProceduralPosition);
			}
			if (child.PositionSpeed < 1f)
			{
				child.ProceduralPosition = TailCalculations_SmoothPosition(child.PreviousPosition, child.ProceduralPosition, child);
			}
		}
		else
		{
			if (child.PositionSpeed < 1f)
			{
				child.ProceduralPosition = TailCalculations_SmoothPosition(child.PreviousPosition, child.ProceduralPosition, child);
			}
			TailCalculations_ComputeSegmentCollisions(child, ref child.ProceduralPosition);
			if (AngleLimit < 181f)
			{
				child.ProceduralPosition = AngleLimiting(child, child.ProceduralPosition);
			}
		}
		if (MaxStretching < 1f)
		{
			StretchingLimiting(child);
		}
		if (!child.Gravity.VIsZero() || UseWind)
		{
			CalculateGravityPositionOffsetForSegment(child);
		}
		if (Axis2D > 0)
		{
			Axis2DLimit(child);
		}
	}

	private void TailCalculations_SegmentPostProcessing(TailSegment bone)
	{
		if (Deflection > Mathf.Epsilon)
		{
			Deflection_SegmentOffsetSimple(bone, ref bone.ProceduralPosition);
		}
	}

	private void TailCalculations_SegmentRotation(TailSegment child, Vector3 localOffset)
	{
		_tc_lookRot = Quaternion.FromToRotation(child.ParentBone.transform.TransformDirection(localOffset), child.ProceduralPositionWeightBlended - child.ParentBone.ProceduralPositionWeightBlended);
		_tc_targetParentRot = _tc_lookRot * child.ParentBone.transform.rotation;
		if (AnimateRoll)
		{
			_tc_targetParentRot = Quaternion.Lerp(child.ParentBone.TrueTargetRotation, _tc_targetParentRot, deltaForLerps * Mathf.LerpUnclamped(10f, 60f, child.RotationSpeed));
		}
		child.ParentBone.TrueTargetRotation = _tc_targetParentRot;
		child.ParentBone.PreviousPosReferenceRotation = child.ParentBone.PosRefRotation;
		if (!AnimateRoll && child.RotationSpeed < 1f)
		{
			_tc_targetParentRot = TailCalculations_SmoothRotation(child.ParentBone.PosRefRotation, _tc_targetParentRot, child);
		}
		child.ParentBone.PosRefRotation = _tc_targetParentRot;
	}

	private void TailCalculations_SegmentRotationDetached(TailSegment child, Vector3 localOffset)
	{
		_tc_lookRot = Quaternion.FromToRotation(child.ParentBone.transform.TransformDirection(localOffset), child.ProceduralPositionWeightBlended - child.ParentBone.ProceduralPositionWeightBlended);
		_tc_targetParentRot = _tc_lookRot * child.transform.rotation;
		if (AnimateRoll)
		{
			_tc_targetParentRot = Quaternion.Lerp(child.ParentBone.TrueTargetRotation, _tc_targetParentRot, deltaForLerps * Mathf.LerpUnclamped(10f, 60f, child.RotationSpeed));
		}
		child.ParentBone.TrueTargetRotation = _tc_targetParentRot;
		child.ParentBone.PreviousPosReferenceRotation = child.ParentBone.PosRefRotation;
		if (!AnimateRoll && child.RotationSpeed < 1f)
		{
			_tc_targetParentRot = TailCalculations_SmoothRotation(child.ParentBone.PosRefRotation, _tc_targetParentRot, child);
		}
		child.ParentBone.PosRefRotation = _tc_targetParentRot;
	}

	private void TailCalculations_ApplySegmentMotion(TailSegment child)
	{
		child.ParentBone.transform.rotation = child.ParentBone.TrueTargetRotation;
		child.transform.position = child.ProceduralPositionWeightBlended;
		child.RefreshFinalPos(child.ProceduralPositionWeightBlended);
		child.ParentBone.RefreshFinalRot(child.ParentBone.TrueTargetRotation);
	}

	private void TailSegment_PrepareBoneLength(TailSegment child)
	{
		child.BoneDimensionsScaled = Vector3.Scale(child.ParentBone.transform.lossyScale * child.LengthMultiplier, child.LastKeyframeLocalPosition);
		child.BoneLengthScaled = child.BoneDimensionsScaled.magnitude;
	}

	private void TailSegment_PrepareMotionParameters(TailSegment child)
	{
		_sg_curly = Mathf.LerpUnclamped(0.5f, 0.125f, child.Curling);
		_sg_springVelo = Mathf.LerpUnclamped(0.65f, 0.9f, child.Springiness);
		_sg_curly = Mathf.Lerp(_sg_curly, Mathf.LerpUnclamped(0.95f, 0.135f, child.Curling), child.Slithery);
		_sg_springVelo = Mathf.Lerp(_sg_springVelo, Mathf.LerpUnclamped(0.1f, 0.85f, child.Springiness), child.Slithery);
	}

	private void TailSegment_PrepareVelocity(TailSegment child)
	{
		_sg_push = child.ProceduralPosition - child.PreviousPosition;
		child.PreviousPosition = child.ProceduralPosition;
		float swinginess = _sg_springVelo;
		if (child.CollisionContactFlag)
		{
			swinginess *= child.Slippery;
		}
		child.ProceduralPosition += _sg_push * swinginess;
		child.PreviousPush = _sg_push;
	}

	private void TailSegment_PrepareRotation(TailSegment child)
	{
		_sg_targetChildWorldPosInParentFront = child.ParentBone.ProceduralPosition + TailSegment_GetSwingRotation(child, _sg_slitFactor) * child.BoneDimensionsScaled;
	}

	private void TailSegment_PrepareRotationDetached(TailSegment child)
	{
		_sg_targetChildWorldPosInParentFront = child.ParentBone.ProceduralPosition + TailSegment_GetSwingRotationDetached(child, _sg_slitFactor) * child.BoneDimensionsScaled;
	}

	private void TailSegment_BaseSwingProcessing(TailSegment child)
	{
		_sg_slitFactor = child.Slithery;
		if (child.CollisionContactRelevancy > 0f)
		{
			_sg_slitFactor = ReflectCollision;
		}
		_sg_dirToTargetParentFront = _sg_targetChildWorldPosInParentFront - child.ProceduralPosition;
		if (UnifyBendiness > 0f)
		{
			child.ProceduralPosition += _sg_dirToTargetParentFront * secPeriodDelta * _sg_curly * TailSegment_GetUnifiedBendinessMultiplier(child);
		}
		else
		{
			child.ProceduralPosition += _sg_dirToTargetParentFront * _sg_curly * secPeriodDelta;
		}
		if (Tangle != 0f && child.Slithery >= 1f)
		{
			child.ProceduralPosition = Vector3.LerpUnclamped(child.ProceduralPosition, _sg_targetChildWorldPosInParentFront, _tc_tangle);
		}
	}

	private void TailSegment_PreRotationPositionBlend(TailSegment child)
	{
		if (child.BlendValue * conditionalWeight < 1f)
		{
			child.ProceduralPositionWeightBlended = Vector3.LerpUnclamped(child.transform.position, child.ProceduralPosition, child.BlendValue * conditionalWeight);
		}
		else
		{
			child.ProceduralPositionWeightBlended = child.ProceduralPosition;
		}
	}

	private Quaternion TailSegment_RotationSlithery(TailSegment child)
	{
		if (!child.Curving.QIsZero())
		{
			return GetSlitheryReferenceRotation(child) * child.Curving * child.ParentBone.LastKeyframeLocalRotation;
		}
		return GetSlitheryReferenceRotation(child) * child.ParentBone.LastKeyframeLocalRotation;
	}

	private Quaternion TailSegment_RotationSlitheryDetached(TailSegment child)
	{
		if (!child.Curving.QIsZero())
		{
			return GetSlitheryReferenceRotation(child) * child.Curving * child.ParentBone.InitialLocalRotation;
		}
		return GetSlitheryReferenceRotation(child) * child.ParentBone.InitialLocalRotation;
	}

	private Quaternion GetSlitheryReferenceRotation(TailSegment child)
	{
		if (child.Slithery <= 1f)
		{
			return child.ParentBone.ParentBone.PosRefRotation;
		}
		return Quaternion.LerpUnclamped(child.ParentBone.ParentBone.PosRefRotation, child.ParentBone.ParentBone.PreviousPosReferenceRotation, (child.Slithery - 1f) * 5f);
	}

	private Quaternion TailSegment_RotationStiff(TailSegment child)
	{
		if (!child.Curving.QIsZero())
		{
			return child.ParentBone.transform.rotation * MultiplyQ(child.Curving, (float)child.Index * 2f);
		}
		return child.ParentBone.transform.rotation;
	}

	private Quaternion TailSegment_GetSwingRotation(TailSegment child, float curlFactor)
	{
		if (curlFactor >= 1f)
		{
			return TailSegment_RotationSlithery(child);
		}
		if (curlFactor > Mathf.Epsilon)
		{
			return Quaternion.LerpUnclamped(TailSegment_RotationStiff(child), TailSegment_RotationSlithery(child), curlFactor);
		}
		return TailSegment_RotationStiff(child);
	}

	private Quaternion TailSegment_GetSwingRotationDetached(TailSegment child, float curlFactor)
	{
		if (curlFactor >= 1f)
		{
			return TailSegment_RotationSlitheryDetached(child);
		}
		if (curlFactor > Mathf.Epsilon)
		{
			return Quaternion.LerpUnclamped(TailSegment_RotationStiff(child), TailSegment_RotationSlitheryDetached(child), curlFactor);
		}
		return TailSegment_RotationStiff(child);
	}

	private float TailSegment_GetUnifiedBendinessMultiplier(TailSegment child)
	{
		float segmentRatio = child.BoneLength / _TC_TailLength;
		segmentRatio = Mathf.Pow(segmentRatio, 0.5f);
		if (segmentRatio == 0f)
		{
			segmentRatio = 1f;
		}
		float unifyMul = _sg_curly / segmentRatio / 2f;
		unifyMul = Mathf.LerpUnclamped(_sg_curly, unifyMul, UnifyBendiness);
		if (unifyMul < 0.15f)
		{
			unifyMul = 0.15f;
		}
		else if (unifyMul > 1.4f)
		{
			unifyMul = 1.4f;
		}
		return unifyMul;
	}

	public void TailSegments_UpdateCoordsForRootBone(TailSegment parent)
	{
		TailSegment root = TailSegments[0];
		root.transform.localRotation = root.LastKeyframeLocalRotation * _tc_startBoneRotOffset;
		parent.PreviousPosReferenceRotation = parent.PosRefRotation;
		parent.PosRefRotation = parent.transform.rotation;
		parent.PreviousPosition = parent.ProceduralPosition;
		parent.ProceduralPosition = parent.transform.position;
		if (DetachChildren)
		{
			root.TrueTargetRotation = root.transform.rotation;
		}
		parent.RefreshFinalPos(parent.transform.position);
		parent.ProceduralPositionWeightBlended = parent.ProceduralPosition;
		if (parent.ParentBone.transform != null)
		{
			parent.ParentBone.PreviousPosReferenceRotation = parent.ParentBone.PosRefRotation;
			parent.ParentBone.PreviousPosition = parent.ParentBone.ProceduralPosition;
			parent.ParentBone.ProceduralPosition = parent.ParentBone.transform.position;
			parent.ParentBone.PosRefRotation = parent.ParentBone.transform.rotation;
			parent.ParentBone.ProceduralPositionWeightBlended = parent.ParentBone.ProceduralPosition;
		}
		TailSegments[_tc_startI].ChildBone.PreviousPosition += _waving_sustain;
		root.RefreshKeyLocalPosition();
	}

	public void TailCalculations_UpdateArtificialChildBone(TailSegment child)
	{
		if (DetachChildren)
		{
			TailSegment_PrepareRotationDetached(child);
		}
		else
		{
			TailSegment_PrepareRotation(child);
		}
		TailSegment_BaseSwingProcessing(child);
		if (child.PositionSpeed < 1f)
		{
			child.ProceduralPosition = TailCalculations_SmoothPosition(child.PreviousPosition, child.ProceduralPosition, child);
		}
		if (MaxStretching < 1f)
		{
			StretchingLimiting(child);
		}
		if (!child.Gravity.VIsZero() || UseWind)
		{
			CalculateGravityPositionOffsetForSegment(child);
		}
		if (Axis2D > 0)
		{
			Axis2DLimit(child);
		}
		child.CollisionContactRelevancy = -1f;
		if (child.BlendValue * conditionalWeight < 1f)
		{
			child.ProceduralPositionWeightBlended = Vector3.LerpUnclamped(child.ParentBone.transform.TransformPoint(child.LastKeyframeLocalPosition), child.ProceduralPosition, child.BlendValue * conditionalWeight);
		}
		else
		{
			child.ProceduralPositionWeightBlended = child.ProceduralPosition;
		}
	}

	public void Editor_TailCalculations_RefreshArtificialParentBone()
	{
		GhostParent.ProceduralPosition = GhostParent.transform.position + GhostParent.transform.rotation.TransformVector(GhostParent.transform.lossyScale, GhostParent.LocalOffset);
	}

	private void SimulateTailMotionFrame(bool pp)
	{
		TailSegments_UpdateRootFeatures();
		TailSegments_UpdateCoordsForRootBone(_tc_rootBone);
		if (pp)
		{
			PostProcessing_ReferenceUpdate();
		}
		if (_tc_startI > -1)
		{
			TailSegment child = TailSegments[_tc_startI];
			if (!DetachChildren)
			{
				while (child != GhostChild)
				{
					child.BoneDimensionsScaled = Vector3.Scale(child.ParentBone.transform.lossyScale * child.LengthMultiplier, child.LastKeyframeLocalPosition);
					child.BoneLengthScaled = child.BoneDimensionsScaled.magnitude;
					TailSegment_PrepareBoneLength(child);
					TailSegment_PrepareMotionParameters(child);
					TailSegment_PrepareVelocity(child);
					child = child.ChildBone;
				}
			}
			else
			{
				while (child != GhostChild)
				{
					child.BoneDimensionsScaled = Vector3.Scale(child.ParentBone.transform.lossyScale * child.LengthMultiplier, child.InitialLocalPosition);
					child.BoneLengthScaled = child.BoneDimensionsScaled.magnitude;
					TailSegment_PrepareMotionParameters(child);
					TailSegment_PrepareVelocity(child);
					child = child.ChildBone;
				}
			}
		}
		TailSegment_PrepareBoneLength(GhostChild);
		TailSegment_PrepareMotionParameters(GhostChild);
		TailSegment_PrepareVelocity(GhostChild);
		if (_tc_startII > -1)
		{
			TailSegment child2 = TailSegments[_tc_startII];
			if (!DetachChildren)
			{
				while (child2 != GhostChild)
				{
					TailSegment_PrepareRotation(child2);
					TailSegment_BaseSwingProcessing(child2);
					TailCalculations_SegmentPreProcessingStack(child2);
					if (pp)
					{
						TailCalculations_SegmentPostProcessing(child2);
					}
					TailSegment_PreRotationPositionBlend(child2);
					child2 = child2.ChildBone;
				}
			}
			else
			{
				while (child2 != GhostChild)
				{
					TailSegment_PrepareRotationDetached(child2);
					TailSegment_BaseSwingProcessing(child2);
					TailCalculations_SegmentPreProcessingStack(child2);
					if (pp)
					{
						TailCalculations_SegmentPostProcessing(child2);
					}
					TailSegment_PreRotationPositionBlend(child2);
					child2 = child2.ChildBone;
				}
			}
		}
		TailCalculations_UpdateArtificialChildBone(GhostChild);
	}

	private void UpdateTailAlgorithm()
	{
		TailCalculations_Begin();
		if (framesToSimulate != 0)
		{
			if (UseCollision)
			{
				BeginCollisionsUpdate();
			}
			bool postProcesses = PostProcessingNeeded();
			MotionInfluenceLimiting();
			for (int i = 0; i < framesToSimulate; i++)
			{
				SimulateTailMotionFrame(postProcesses);
			}
			TailSegments[_tc_startI].transform.position = TailSegments[_tc_startI].ProceduralPositionWeightBlended;
			TailSegments[_tc_startI].RefreshFinalPos(TailSegments[_tc_startI].ProceduralPositionWeightBlended);
			if (!DetachChildren)
			{
				if (_tc_startII > -1)
				{
					for (TailSegment child = TailSegments[_tc_startII]; child != GhostChild; child = child.ChildBone)
					{
						TailCalculations_SegmentRotation(child, child.LastKeyframeLocalPosition);
						TailCalculations_ApplySegmentMotion(child);
					}
				}
			}
			else if (_tc_startII > -1)
			{
				for (TailSegment child2 = TailSegments[_tc_startII]; child2 != GhostChild; child2 = child2.ChildBone)
				{
					TailCalculations_SegmentRotation(child2, child2.InitialLocalPosition);
					TailCalculations_ApplySegmentMotion(child2);
				}
			}
			TailCalculations_SegmentRotation(GhostChild, GhostChild.LastKeyframeLocalPosition);
			GhostChild.ParentBone.transform.rotation = GhostChild.ParentBone.TrueTargetRotation;
			GhostChild.ParentBone.RefreshFinalRot(GhostChild.ParentBone.TrueTargetRotation);
			if ((bool)GhostChild.transform)
			{
				GhostChild.RefreshFinalPos(GhostChild.transform.position);
				GhostChild.RefreshFinalRot(GhostChild.transform.rotation);
			}
		}
		else if (InterpolateRate)
		{
			secPeriodDelta = rateDelta / 24f;
			deltaForLerps = secPeriodDelta;
			SimulateTailMotionFrame(PostProcessingNeeded());
			if (_tc_startII > -1)
			{
				for (TailSegment child3 = TailSegments[_tc_startII]; child3 != GhostChild; child3 = child3.ChildBone)
				{
					TailCalculations_SegmentRotation(child3, child3.LastKeyframeLocalPosition);
					TailCalculations_ApplySegmentMotion(child3);
				}
			}
			TailCalculations_SegmentRotation(GhostChild, GhostChild.LastKeyframeLocalPosition);
			GhostChild.ParentBone.transform.rotation = GhostChild.ParentBone.TrueTargetRotation;
			GhostChild.ParentBone.RefreshFinalRot(GhostChild.ParentBone.TrueTargetRotation);
		}
		else if (_tc_startI > -1)
		{
			TailSegment segment = TailSegments[_tc_startI];
			while (segment != null && (bool)segment.transform)
			{
				segment.transform.position = segment.LastFinalPosition;
				segment.transform.rotation = segment.LastFinalRotation;
				segment = segment.ChildBone;
			}
		}
	}

	private void CheckIfTailAnimatorShouldBeUpdated()
	{
		if (!initialized)
		{
			if (StartAfterTPose)
			{
				startAfterTPoseCounter++;
				if (startAfterTPoseCounter > 6)
				{
					Init();
				}
			}
			updateTailAnimator = false;
			return;
		}
		if (UseMaxDistance)
		{
			MaxDistanceCalculations();
			conditionalWeight = OverrideWeight * distanceWeight;
		}
		else
		{
			conditionalWeight = OverrideWeight;
		}
		if (_forceDisable)
		{
			if (FadeDuration > 0f)
			{
				_forceDisableElapsed += Time.unscaledDeltaTime * (1f / FadeDuration);
				if (_forceDisableElapsed > 1f)
				{
					_forceDisableElapsed = 1f;
				}
			}
			else
			{
				_forceDisableElapsed = 1f;
			}
			conditionalWeight *= 1f - _forceDisableElapsed;
		}
		else if (_forceDisableElapsed > 0f && FadeDuration > 0f)
		{
			_forceDisableElapsed -= Time.unscaledDeltaTime * (1f / FadeDuration);
			if (_forceDisableElapsed < 0f)
			{
				_forceDisableElapsed = 0f;
			}
			conditionalWeight *= 1f - _forceDisableElapsed;
		}
		if (DisabledByInvisibility())
		{
			return;
		}
		if (UseCollision && !collisionInitialized)
		{
			SetupSphereColliders();
		}
		if (TailSegments.Count == 0)
		{
			Debug.LogError("[TAIL ANIMATOR] No tail bones defined in " + base.name + " !");
			initialized = false;
			updateTailAnimator = false;
			return;
		}
		if (TailAnimatorAmount * conditionalWeight <= Mathf.Epsilon)
		{
			wasDisabled = true;
			updateTailAnimator = false;
			return;
		}
		if (wasDisabled)
		{
			User_ReposeTail();
			previousWorldPosition = base.transform.position;
			wasDisabled = false;
		}
		if (IncludeParent && TailSegments.Count > 0 && !TailSegments[0].transform.parent)
		{
			IncludeParent = false;
		}
		if (TailSegments.Count < 1)
		{
			updateTailAnimator = false;
		}
		else
		{
			updateTailAnimator = true;
		}
	}

	public bool DisabledByInvisibility()
	{
		if (OptimizeWithMesh != null)
		{
			bool somethingVisible = false;
			if (OptimizeWithMesh.isVisible)
			{
				somethingVisible = true;
			}
			else if (OptimizeWithMeshes != null && OptimizeWithMeshes.Length != 0)
			{
				for (int i = 0; i < OptimizeWithMeshes.Length; i++)
				{
					if (!(OptimizeWithMeshes[i] == null) && OptimizeWithMeshes[i].isVisible)
					{
						somethingVisible = true;
						break;
					}
				}
			}
			if (!somethingVisible)
			{
				updateTailAnimator = false;
				return true;
			}
		}
		return false;
	}

	private void DeltaTimeCalculations()
	{
		if (UpdateRate > 0)
		{
			switch (DeltaType)
			{
			case EFDeltaType.DeltaTime:
			case EFDeltaType.SafeDelta:
				justDelta = Time.deltaTime / Mathf.Clamp(Time.timeScale, 0.01f, 1f);
				break;
			case EFDeltaType.SmoothDeltaTime:
				justDelta = Time.smoothDeltaTime;
				break;
			case EFDeltaType.UnscaledDeltaTime:
				justDelta = Time.unscaledDeltaTime;
				break;
			case EFDeltaType.FixedDeltaTime:
				justDelta = Time.fixedDeltaTime;
				break;
			}
			justDelta *= TimeScale;
			secPeriodDelta = 1f;
			deltaForLerps = secPeriodDelta;
			rateDelta = 1f / (float)UpdateRate;
			StableUpdateRateCalculations();
			return;
		}
		switch (DeltaType)
		{
		case EFDeltaType.SafeDelta:
			justDelta = Mathf.Lerp(justDelta, GetClampedSmoothDelta(), 0.075f);
			break;
		case EFDeltaType.DeltaTime:
			justDelta = Time.deltaTime;
			break;
		case EFDeltaType.SmoothDeltaTime:
			justDelta = Time.smoothDeltaTime;
			break;
		case EFDeltaType.UnscaledDeltaTime:
			justDelta = Time.unscaledDeltaTime;
			break;
		case EFDeltaType.FixedDeltaTime:
			justDelta = Time.fixedDeltaTime;
			break;
		}
		rateDelta = justDelta;
		deltaForLerps = Mathf.Pow(secPeriodDelta, 0.1f) * 0.02f;
		justDelta *= TimeScale;
		secPeriodDelta = Mathf.Min(1f, justDelta * 60f);
		framesToSimulate = 1;
		previousframesToSimulate = 1;
	}

	private void StableUpdateRateCalculations()
	{
		previousframesToSimulate = framesToSimulate;
		collectedDelta += justDelta;
		framesToSimulate = 0;
		while (collectedDelta >= rateDelta)
		{
			collectedDelta -= rateDelta;
			framesToSimulate++;
			if (framesToSimulate >= 3)
			{
				collectedDelta = 0f;
				break;
			}
		}
	}

	private void PreCalibrateBones()
	{
		for (TailSegment child = TailSegments[0]; child != GhostChild; child = child.ChildBone)
		{
			child.PreCalibrate();
		}
	}

	private void CalibrateBones()
	{
		if (UseIK && IKBlend > 0f)
		{
			UpdateIK();
		}
		_limiting_stretchingHelperTooLong = Mathf.Lerp(0.4f, 0f, MaxStretching);
		_limiting_stretchingHelperTooShort = _limiting_stretchingHelperTooLong * 1.5f;
	}

	public void CheckForNullsInGhostChain()
	{
		if (_TransformsGhostChain == null)
		{
			_TransformsGhostChain = new List<Transform>();
		}
		for (int i = _TransformsGhostChain.Count - 1; i >= 0; i--)
		{
			if (_TransformsGhostChain[i] == null)
			{
				_TransformsGhostChain.RemoveAt(i);
			}
		}
	}

	private float GetClampedSmoothDelta()
	{
		return Mathf.Clamp(Time.smoothDeltaTime, 0f, 0.25f);
	}

	private Quaternion MultiplyQ(Quaternion rotation, float times)
	{
		return Quaternion.AngleAxis(rotation.x * 57.29578f * times, Vector3.right) * Quaternion.AngleAxis(rotation.z * 57.29578f * times, Vector3.forward) * Quaternion.AngleAxis(rotation.y * 57.29578f * times, Vector3.up);
	}

	public float GetValueFromCurve(int i, AnimationCurve c)
	{
		if (!initialized)
		{
			return c.Evaluate((float)i / (float)_TransformsGhostChain.Count);
		}
		return c.Evaluate(TailSegments[i].IndexOverlLength);
	}

	public AnimationCurve ClampCurve(AnimationCurve a, float timeStart, float timeEnd, float lowest, float highest)
	{
		Keyframe[] keys = a.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			if (keys[i].time < timeStart)
			{
				keys[i].time = timeStart;
			}
			else if (keys[i].time > timeEnd)
			{
				keys[i].time = timeEnd;
			}
			if (keys[i].value < lowest)
			{
				keys[i].value = lowest;
			}
			else if (keys[i].value > highest)
			{
				keys[i].value = highest;
			}
		}
		a.keys = keys;
		return a;
	}

	public void RefreshTransformsList()
	{
		if (_TransformsGhostChain == null)
		{
			_TransformsGhostChain = new List<Transform>();
			return;
		}
		for (int i = _TransformsGhostChain.Count - 1; i >= 0; i--)
		{
			if (_TransformsGhostChain[0] == null)
			{
				_TransformsGhostChain.RemoveAt(i);
			}
		}
	}

	public TailSegment GetGhostChild()
	{
		return GhostChild;
	}

	private IEnumerator LateFixed()
	{
		WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
		lateFixedIsRunning = true;
		while (true)
		{
			yield return fixedWait;
			PreCalibrateBones();
			fixedAllow = true;
		}
	}

	private void Deflection_BeginUpdate()
	{
		_defl_treshold = DeflectionStartAngle / 90f;
		float smoothTime = DeflectionSmooth / 9f;
		for (int i = _tc_startII; i < TailSegments.Count; i++)
		{
			TailSegment ppChild = _pp_reference[i];
			if (!ppChild.CheckDeflectionState(_defl_treshold, smoothTime, rateDelta))
			{
				bool dependenciesMeet = true;
				if (DeflectOnlyCollisions && ppChild.CollisionContactRelevancy <= 0f)
				{
					dependenciesMeet = false;
				}
				if (dependenciesMeet)
				{
					Deflection_AddDeflectionSource(ppChild);
				}
				else
				{
					Deflection_RemoveDeflectionSource(ppChild);
				}
			}
			else
			{
				Deflection_RemoveDeflectionSource(ppChild);
			}
		}
	}

	private void Deflection_RemoveDeflectionSource(TailSegment child)
	{
		if (!child.DeflectionRestoreState().HasValue && _defl_source.Contains(child))
		{
			_defl_source.Remove(child);
		}
	}

	private void Deflection_AddDeflectionSource(TailSegment child)
	{
		if (child.DeflectionRelevant() && !_defl_source.Contains(child))
		{
			_defl_source.Add(child);
		}
	}

	private void Deflection_SegmentOffsetSimple(TailSegment child, ref Vector3 position)
	{
		if (child.Index == _tc_startI)
		{
			return;
		}
		float lastDeflectionPower = 0f;
		for (int i = 0; i < _defl_source.Count; i++)
		{
			if (child.Index <= _defl_source[i].Index && child.Index != _defl_source[i].Index && !(_defl_source[i].DeflectionFactor < lastDeflectionPower))
			{
				lastDeflectionPower = _defl_source[i].DeflectionFactor;
				float preI = 0f;
				if (i > 0)
				{
					preI = _defl_source[i].Index;
				}
				float timeOnCurve = Mathf.InverseLerp(preI, _defl_source[i].Index, child.Index);
				Vector3 towardDeflection = _defl_source[i].DeflectionWorldPosition - child.ParentBone.ProceduralPosition;
				Vector3 deflectedSegmentPos = child.ParentBone.ProceduralPosition;
				deflectedSegmentPos += towardDeflection.normalized * child.BoneLengthScaled;
				child.ProceduralPosition = Vector3.LerpUnclamped(child.ProceduralPosition, deflectedSegmentPos, Deflection * DeflectionFalloff.Evaluate(timeOnCurve) * _defl_source[i].DeflectionSmooth);
			}
		}
	}

	public void User_SetTailTransforms(List<Transform> list)
	{
		StartBone = list[0];
		EndBone = list[list.Count - 1];
		_TransformsGhostChain = list;
		StartAfterTPose = false;
		initialized = false;
		Init();
	}

	public TailSegment User_AddTailTransform(Transform transform)
	{
		TailSegment newSeg = new TailSegment(transform);
		TailSegment last = TailSegments[TailSegments.Count - 1];
		newSeg.ParamsFromAll(last);
		newSeg.RefreshFinalPos(newSeg.transform.position);
		newSeg.RefreshFinalRot(newSeg.transform.rotation);
		newSeg.ProceduralPosition = newSeg.transform.position;
		newSeg.PosRefRotation = newSeg.transform.rotation;
		_TransformsGhostChain.Add(transform);
		TailSegments.Add(newSeg);
		last.SetChildRef(newSeg);
		newSeg.SetParentRef(last);
		newSeg.SetChildRef(GhostChild);
		GhostChild.SetParentRef(newSeg);
		for (int i = 0; i < TailSegments.Count; i++)
		{
			TailSegments[i].SetIndex(i, TailSegments.Count);
		}
		return newSeg;
	}

	public void User_CutEndSegmentsTo(int fromLastTo)
	{
		if (fromLastTo < TailSegments.Count)
		{
			GhostChild = TailSegments[fromLastTo];
			GhostChild.SetChildRef(null);
			for (int i = TailSegments.Count - 1; i >= fromLastTo; i--)
			{
				TailSegments.RemoveAt(i);
				_TransformsGhostChain.RemoveAt(i);
			}
		}
		else
		{
			Debug.Log("[Tail Animator Cutting] Wrong index, you want cut from end to " + fromLastTo + " segment but there are only " + TailSegments.Count + " segments!");
		}
	}

	public void User_ReposeTail()
	{
		GhostParent.Reset();
		for (int i = 0; i < TailSegments.Count; i++)
		{
			TailSegments[i].Reset();
		}
		GhostChild.Reset();
	}

	public void User_ForceDisabled(bool disable)
	{
		_forceDisable = disable;
	}

	public void OnDrop(PointerEventData data)
	{
	}

	private void OnValidate()
	{
		if (UpdateRate < 0)
		{
			UpdateRate = 0;
		}
		if (Application.isPlaying)
		{
			RefreshSegmentsColliders();
			if (UseIK)
			{
				IK_ApplyLimitBoneSettings();
			}
		}
		if (UsePartialBlend)
		{
			ClampCurve(BlendCurve, 0f, 1f, 0f, 1f);
		}
	}

	public void GetGhostChain()
	{
		if (_TransformsGhostChain == null)
		{
			_TransformsGhostChain = new List<Transform>();
		}
		if (EndBone == null)
		{
			_TransformsGhostChain.Clear();
			Transform tChild = StartBone;
			if (tChild == null)
			{
				tChild = base.transform;
			}
			_TransformsGhostChain.Add(tChild);
			while (tChild.childCount > 0)
			{
				tChild = tChild.GetChild(0);
				if (!_TransformsGhostChain.Contains(tChild))
				{
					_TransformsGhostChain.Add(tChild);
				}
			}
			_GhostChainInitCount = _TransformsGhostChain.Count;
			return;
		}
		List<Transform> newTrs = new List<Transform>();
		Transform sBone = StartBone;
		if (sBone == null)
		{
			sBone = base.transform;
		}
		Transform tParent = EndBone;
		newTrs.Add(tParent);
		while (tParent != null && tParent != StartBone)
		{
			tParent = tParent.parent;
			if (!newTrs.Contains(tParent))
			{
				newTrs.Add(tParent);
			}
		}
		if (tParent == null)
		{
			Debug.Log("[Tail Animator Editor] " + EndBone.name + " is not child of " + sBone.name + "!");
			Debug.LogError("[Tail Animator Editor] " + EndBone.name + " is not child of " + sBone.name + "!");
		}
		else
		{
			if (!newTrs.Contains(tParent))
			{
				newTrs.Add(tParent);
			}
			_TransformsGhostChain.Clear();
			_TransformsGhostChain = newTrs;
			_TransformsGhostChain.Reverse();
			_GhostChainInitCount = _TransformsGhostChain.Count;
		}
	}

	private void Start()
	{
		if (UpdateAsLast)
		{
			base.enabled = false;
			base.enabled = true;
		}
		if (StartAfterTPose)
		{
			startAfterTPoseCounter = 6;
		}
		else
		{
			Init();
		}
	}

	private void Reset()
	{
		Keyframe key1 = new Keyframe(0f, 0f, 0.1f, 0.1f, 0f, 0.5f);
		Keyframe key2 = new Keyframe(1f, 1f, 5f, 0f, 0.1f, 0f);
		DeflectionFalloff = new AnimationCurve(key1, key2);
	}

	private void Update()
	{
		CheckIfTailAnimatorShouldBeUpdated();
		DeltaTimeCalculations();
		if (UseWind)
		{
			WindEffectUpdate();
		}
		if (AnimatePhysics != 0 || !updateTailAnimator)
		{
			return;
		}
		if (DetachChildren)
		{
			if (_tc_rootBone != null && (bool)_tc_rootBone.transform)
			{
				_tc_rootBone.PreCalibrate();
			}
		}
		else if (OverrideKeyframeAnimation < 1f)
		{
			PreCalibrateBones();
		}
	}

	private void FixedUpdate()
	{
		if (AnimatePhysics != EFixedMode.Basic || !updateTailAnimator)
		{
			return;
		}
		if (DetachChildren)
		{
			if (_tc_rootBone != null && (bool)_tc_rootBone.transform)
			{
				_tc_rootBone.PreCalibrate();
			}
		}
		else
		{
			fixedUpdated = true;
			PreCalibrateBones();
		}
	}

	private void LateUpdate()
	{
		if (!updateTailAnimator)
		{
			return;
		}
		if (AnimatePhysics == EFixedMode.Late)
		{
			if (!lateFixedIsRunning)
			{
				StartCoroutine(LateFixed());
			}
			if (!fixedAllow)
			{
				return;
			}
			fixedAllow = false;
		}
		else
		{
			if (lateFixedIsRunning)
			{
				StopCoroutine(LateFixed());
				lateFixedIsRunning = false;
			}
			if (AnimatePhysics == EFixedMode.Basic)
			{
				if (!fixedUpdated)
				{
					return;
				}
				fixedUpdated = false;
			}
		}
		if (DetachChildren)
		{
			TailSegment child = TailSegments[0];
			TailSegments[0].RefreshKeyLocalPositionAndRotation(child.InitialLocalPosition, child.InitialLocalRotation);
			TailSegments[0].PreCalibrate();
			child = TailSegments[1];
			if (!IncludeParent)
			{
				child.RefreshKeyLocalPositionAndRotation(child.InitialLocalPosition, child.InitialLocalRotation);
				child.PreCalibrate();
				child = TailSegments[2];
			}
			while (child != GhostChild)
			{
				child.RefreshKeyLocalPositionAndRotation(child.InitialLocalPosition, child.InitialLocalRotation);
				child.transform.position = _baseTransform.TransformPoint(child.InitialLocalPositionInRoot);
				child.transform.rotation = _baseTransform.rotation.QToWorld(child.InitialLocalRotationInRoot);
				child = child.ChildBone;
			}
		}
		else if (OverrideKeyframeAnimation > 0f)
		{
			if (OverrideKeyframeAnimation >= 1f)
			{
				PreCalibrateBones();
				for (TailSegment child2 = TailSegments[0]; child2 != GhostChild; child2 = child2.ChildBone)
				{
					child2.RefreshKeyLocalPositionAndRotation();
				}
			}
			else
			{
				for (TailSegment child3 = TailSegments[0]; child3 != GhostChild; child3 = child3.ChildBone)
				{
					child3.transform.localPosition = Vector3.LerpUnclamped(child3.transform.localPosition, child3.InitialLocalPosition, OverrideKeyframeAnimation);
					child3.transform.localRotation = Quaternion.LerpUnclamped(child3.transform.localRotation, child3.InitialLocalRotation, OverrideKeyframeAnimation);
					child3.RefreshKeyLocalPositionAndRotation();
				}
			}
		}
		else
		{
			for (TailSegment child4 = TailSegments[0]; child4 != GhostChild; child4 = child4.ChildBone)
			{
				child4.RefreshKeyLocalPositionAndRotation();
			}
		}
		ExpertParamsUpdate();
		ShapingParamsUpdate();
		CalibrateBones();
		UpdateTailAlgorithm();
		EndUpdate();
	}

	private void EndUpdate()
	{
		ShapingEndUpdate();
		ExpertCurvesEndUpdate();
		previousWorldPosition = BaseTransform.position;
	}
}
