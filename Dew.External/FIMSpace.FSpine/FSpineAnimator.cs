using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FIMSpace.FSpine;

[AddComponentMenu("FImpossible Creations/Spine Animator 2")]
[DefaultExecutionOrder(-11)]
public class FSpineAnimator : MonoBehaviour, IDropHandler, IEventSystemHandler, IFHierarchyIcon
{
	public enum EFSpineEditorCategory
	{
		Setup,
		Tweak,
		Adjust,
		Physical
	}

	public enum EFDeltaType
	{
		DeltaTime,
		SmoothDeltaTime,
		UnscaledDeltaTime,
		FixedDeltaTime,
		SafeDelta
	}

	public enum EParamChange
	{
		GoBackSpeed,
		SpineAnimatorAmount,
		AngleLimit,
		StraightenSpeed,
		PositionSmoother,
		RotationSmoother
	}

	public class HeadBone
	{
		public Transform baseTransform;

		public Transform transform;

		private Vector3 snapshotPoseBaseTrSpacePosition;

		public Vector3 SnapshotPosition;

		private Quaternion snapshotPoseBaseTrSpaceRotationF;

		private Quaternion snapshotPoseBaseTrSpaceRotationB;

		public Quaternion snapshotPoseLocalRotation;

		public Quaternion SnapshotRotation;

		public Vector3 InitialLocalPosition { get; private set; }

		public Quaternion InitialLocalRotation { get; private set; }

		public HeadBone(Transform t)
		{
			transform = t;
		}

		public void PrepareBone(Transform baseTransform, List<SpineBone> bones, int index)
		{
			TakePoseSnapshot(baseTransform, bones, index);
			InitialLocalPosition = transform.localPosition;
			InitialLocalRotation = transform.localRotation;
		}

		internal Quaternion GetLocalRotationDiff()
		{
			return transform.rotation * Quaternion.Inverse(snapshotPoseLocalRotation);
		}

		public void SetCoordsForFrameForward()
		{
			SnapshotPosition = baseTransform.TransformPoint(snapshotPoseBaseTrSpacePosition);
			SnapshotRotation = baseTransform.rotation * snapshotPoseBaseTrSpaceRotationF;
		}

		public void SetCoordsForFrameBackward()
		{
			SnapshotPosition = baseTransform.TransformPoint(snapshotPoseBaseTrSpacePosition);
			SnapshotRotation = baseTransform.rotation * snapshotPoseBaseTrSpaceRotationB;
		}

		public void TakePoseSnapshot(Transform targetSpace, List<SpineBone> bones, int index)
		{
			baseTransform = targetSpace;
			snapshotPoseBaseTrSpacePosition = targetSpace.InverseTransformPoint(transform.position);
			Vector3 targetPosF;
			Vector3 targetPosB;
			if (index == bones.Count - 1)
			{
				Vector3 backDir = targetSpace.InverseTransformPoint(transform.position) - targetSpace.InverseTransformPoint(bones[index - 1].transform.position);
				targetPosF = snapshotPoseBaseTrSpacePosition + backDir;
				targetPosB = targetSpace.InverseTransformPoint(bones[index - 1].transform.position);
			}
			else if (index == 0)
			{
				targetPosF = targetSpace.InverseTransformPoint(bones[index + 1].transform.position);
				Vector3 backDir2 = targetSpace.InverseTransformPoint(transform.position) - targetSpace.InverseTransformPoint(bones[index + 1].transform.position);
				targetPosB = snapshotPoseBaseTrSpacePosition + backDir2;
			}
			else
			{
				targetPosF = targetSpace.InverseTransformPoint(bones[index + 1].transform.position);
				targetPosB = targetSpace.InverseTransformPoint(bones[index - 1].transform.position);
			}
			snapshotPoseBaseTrSpaceRotationF = Quaternion.Inverse(targetSpace.rotation) * Quaternion.LookRotation(targetPosF - snapshotPoseBaseTrSpacePosition);
			snapshotPoseBaseTrSpaceRotationB = Quaternion.Inverse(targetSpace.rotation) * Quaternion.LookRotation(targetPosB - snapshotPoseBaseTrSpacePosition);
			snapshotPoseLocalRotation = Quaternion.Inverse(targetSpace.rotation) * transform.rotation;
		}
	}

	[Serializable]
	public class SpineBone
	{
		public Transform transform;

		public Vector3 ProceduralPosition;

		public Quaternion ProceduralRotation;

		public Vector3 HelperDiffPosition;

		public Quaternion HelperDiffRoation;

		public Vector3 PreviousPosition;

		public Vector3 DefaultForward;

		public float StraightenFactor;

		public float TargetStraightenFactor;

		private float boneLengthB = 0.1f;

		private float boneLengthF = 0.1f;

		private Vector3 boneLocalOffsetB;

		private Vector3 boneLocalOffsetF;

		public float MotionWeight = 1f;

		public Quaternion FinalRotation;

		public Vector3 FinalPosition;

		public Vector3 ManualPosOffset;

		public Quaternion ManualRotOffset;

		public Vector3 ReferencePosition;

		public Vector3 PreviousReferencePosition;

		public Quaternion ReferenceRotation;

		private Quaternion lastKeyframeRotation;

		private Vector3 lastKeyframePosition;

		private Vector3 lastFinalLocalPosition;

		private Quaternion lastFinalLocalRotation;

		public Vector3 forward;

		public Vector3 right;

		public Vector3 up;

		public bool Collide = true;

		public float CollisionRadius = 1f;

		public Vector3 ColliderOffset = Vector3.zero;

		public float BoneLength { get; private set; }

		public Vector3 BoneLocalOffset { get; private set; }

		public Vector3 InitialLocalPosition { get; private set; }

		public Quaternion InitialLocalRotation { get; private set; }

		public void UpdateReferencePosition(Vector3 pos)
		{
			PreviousReferencePosition = ReferencePosition;
			ReferencePosition = pos;
		}

		public void ZeroKeyframeCheck()
		{
			if (lastFinalLocalRotation.QIsSame(transform.localRotation))
			{
				transform.localRotation = lastKeyframeRotation;
			}
			else
			{
				lastKeyframeRotation = transform.localRotation;
			}
			if (lastFinalLocalPosition.VIsSame(transform.localPosition))
			{
				transform.localPosition = lastKeyframePosition;
			}
			else
			{
				lastKeyframePosition = transform.localPosition;
			}
		}

		public void RefreshFinalLocalPose()
		{
			lastFinalLocalPosition = transform.localPosition;
			lastFinalLocalRotation = transform.localRotation;
		}

		public SpineBone(Transform t)
		{
			transform = t;
			ManualPosOffset = Vector3.zero;
			ColliderOffset = Vector3.zero;
			Collide = true;
			CollisionRadius = 1f;
		}

		public void PrepareBone(Transform baseTransform, List<SpineBone> bones, int index)
		{
			InitialLocalPosition = transform.localPosition;
			InitialLocalRotation = transform.localRotation;
			Vector3 nextPos = ((index != bones.Count - 1) ? bones[index + 1].transform.position : ((bones[index].transform.childCount <= 0) ? bones[index - 1].transform.position : bones[index].transform.GetChild(0).position));
			if (index == 0)
			{
				nextPos = bones[index + 1].transform.position;
			}
			if (Vector3.Distance(baseTransform.InverseTransformPoint(nextPos), baseTransform.InverseTransformPoint(bones[index].transform.position)) < 0.01f)
			{
				int nInd = index + 2;
				if (nInd < bones.Count)
				{
					DefaultForward = transform.InverseTransformPoint(bones[nInd].transform.position);
				}
				else
				{
					DefaultForward = transform.InverseTransformPoint(nextPos - baseTransform.position);
				}
			}
			else
			{
				DefaultForward = transform.InverseTransformPoint(nextPos);
			}
			boneLengthB = (baseTransform.InverseTransformPoint(transform.position) - baseTransform.InverseTransformPoint(nextPos)).magnitude;
			boneLocalOffsetB = baseTransform.InverseTransformPoint(nextPos);
			boneLengthF = (baseTransform.InverseTransformPoint(transform.position) - baseTransform.InverseTransformPoint(nextPos)).magnitude;
			boneLocalOffsetF = baseTransform.InverseTransformPoint(nextPos);
			if (ManualPosOffset.sqrMagnitude == 0f)
			{
				ManualPosOffset = Vector3.zero;
			}
			if (ManualRotOffset.eulerAngles.sqrMagnitude == 0f)
			{
				ManualRotOffset = Quaternion.identity;
			}
			SetDistanceForFrameForward();
			PrepareAxes(baseTransform, bones, index);
		}

		public void SetDistanceForFrameForward()
		{
			BoneLength = boneLengthF;
			BoneLocalOffset = boneLocalOffsetF;
		}

		public void SetDistanceForFrameBackward()
		{
			BoneLength = boneLengthB;
			BoneLocalOffset = boneLocalOffsetB;
		}

		public float GetUnscalledBoneLength()
		{
			if (boneLengthF > boneLengthB)
			{
				return boneLengthF;
			}
			return boneLengthB;
		}

		private void PrepareAxes(Transform baseTransform, List<SpineBone> bonesList, int index)
		{
			Transform parentTr;
			Vector3 parentPos;
			Vector3 childPos;
			if (index == bonesList.Count - 1)
			{
				if (transform.childCount == 1)
				{
					parentTr = transform;
					Transform child = transform.GetChild(0);
					parentPos = parentTr.position;
					childPos = child.position;
				}
				else
				{
					parentTr = transform;
					_ = transform;
					parentPos = bonesList[index - 1].transform.position;
					childPos = transform.position;
				}
			}
			else
			{
				parentTr = transform;
				Transform obj = bonesList[index + 1].transform;
				parentPos = parentTr.position;
				childPos = obj.position;
			}
			Vector3 forwardInBoneOrientation = parentTr.InverseTransformDirection(childPos) - parentTr.InverseTransformDirection(parentPos);
			Vector3 projectedUp = Vector3.ProjectOnPlane(baseTransform.up, transform.TransformDirection(forwardInBoneOrientation).normalized).normalized;
			Vector3 upInBoneOrientation = parentTr.InverseTransformDirection(parentPos + projectedUp) - parentTr.InverseTransformDirection(parentPos);
			Vector3 crossRight = Vector3.Cross(transform.TransformDirection(forwardInBoneOrientation), transform.TransformDirection(upInBoneOrientation));
			right = (parentTr.InverseTransformDirection(parentPos + crossRight) - parentTr.InverseTransformDirection(parentPos)).normalized;
			up = upInBoneOrientation.normalized;
			forward = forwardInBoneOrientation.normalized;
		}

		internal void CalculateDifferencePose(Vector3 upAxis, Vector3 rightAxis)
		{
			HelperDiffPosition = ProceduralPosition - ReferencePosition;
			Quaternion fixedBendRotation = ProceduralRotation * Quaternion.FromToRotation(up, upAxis) * Quaternion.FromToRotation(right, rightAxis);
			Quaternion fixedRefRotation = ReferenceRotation * Quaternion.FromToRotation(up, upAxis) * Quaternion.FromToRotation(right, rightAxis);
			HelperDiffRoation = fixedBendRotation * Quaternion.Inverse(fixedRefRotation);
		}

		internal void ApplyDifferencePose()
		{
			FinalPosition = transform.position + HelperDiffPosition;
			FinalRotation = HelperDiffRoation * transform.rotation;
		}

		public void Editor_SetLength(float length)
		{
			if (!Application.isPlaying)
			{
				BoneLength = length;
			}
		}

		public float GetCollisionRadiusScaled()
		{
			return CollisionRadius * transform.lossyScale.x;
		}
	}

	public enum EFixedMode
	{
		None,
		Basic,
		Late
	}

	private bool collisionInitialized;

	private bool forceRefreshCollidersData;

	[FPD_Percentage(0f, 1f, false, true, "%", false)]
	[Tooltip("You can use this variable to blend intensity of spine animator motion over skeleton animation\n\nValue = 1: Animation with spine Animator motion\nValue = 0: Only skeleton animation")]
	public float SpineAnimatorAmount = 1f;

	private Quaternion Rotate180 = Quaternion.Euler(0f, 180f, 0f);

	private int initAfterTPoseCounter;

	private bool fixedUpdated;

	private bool lateFixedIsRunning;

	private bool fixedAllow = true;

	private bool chainReverseFlag;

	public EFSpineEditorCategory _Editor_Category;

	public bool _Editor_PivotoffsetXYZ;

	private bool _editor_isQuitting;

	private int leadingBoneIndex;

	private int chainIndexDirection = 1;

	private int chainIndexOffset = 1;

	protected float delta = 0.016f;

	protected float unifiedDelta = 0.016f;

	protected float elapsedDeltaHelper;

	protected int updateLoops = 1;

	private bool initialized;

	private Vector3 previousPos;

	private bool wasBlendedOut;

	private List<FSpineBoneConnector> connectors;

	private float referenceDistance = 0.1f;

	public Vector3 ModelForwardAxis = Vector3.forward;

	public Vector3 ModelForwardAxisScaled = Vector3.forward;

	public Vector3 ModelUpAxis = Vector3.up;

	public Vector3 ModelUpAxisScaled = Vector3.up;

	internal Vector3 ModelRightAxis = Vector3.right;

	internal Vector3 ModelRightAxisScaled = Vector3.right;

	public List<SpineBone> SpineBones;

	public List<Transform> SpineTransforms;

	private HeadBone frontHead;

	private HeadBone backHead;

	private HeadBone headBone;

	[Tooltip("Main character object - by default it is game object to which Spine Animator is attached.\n\nYou can use it to control spine of character from different game object.")]
	public Transform BaseTransform;

	public Transform ForwardReference;

	[Tooltip("If your spine lead bone is in beggining of your hierarchy chain then toggle it.\n\nComponent's gizmos can help you out to define which bone should be leading (check head gizmo when you switch this toggle).")]
	public bool LastBoneLeading = true;

	[Tooltip("Sometimes spine chain can face in different direction than desired or you want your characters to move backward with spine motion.")]
	public bool ReverseForward;

	[Tooltip("If you're using 'Animate Physics' on animator you should set this variable to be enabled.")]
	public EFixedMode AnimatePhysics;

	public Transform AnchorRoot;

	[Tooltip("Connecting lead bone position to given transform, useful when it is tail and you already animating spine with other Spine Animator component.")]
	public Transform HeadAnchor;

	[Tooltip("Letting head anchor to animate rotation")]
	public bool AnimateAnchor = true;

	[Tooltip("If you need to offset leading bone rotation.")]
	public Vector3 LeadBoneRotationOffset = Vector3.zero;

	[Tooltip("If Lead Bone Rotation Offset should affect reference pose or bone rotation")]
	public bool LeadBoneOffsetReference = true;

	[Tooltip("List of bone positioning/rotation fixers if using paws positioning with IK controlls disconnected out of arms/legs in the hierarchy")]
	public List<SpineAnimator_FixIKControlledBones> BonesFixers = new List<SpineAnimator_FixIKControlledBones>();

	[Tooltip("Useful when you use few spine animators and want to rely on animated position and rotation by other spine animator.")]
	public bool UpdateAsLast;

	public bool QueueToLastUpdate;

	[Tooltip("If corrections should affect spine chain children.")]
	public bool ManualAffectChain;

	[Tooltip("Often when you drop model to scene, it's initial pose is much different than animations, which causes problems, this toggle solves it at start.")]
	public bool StartAfterTPose = true;

	[Tooltip("If you want spine animator to stop computing when choosed mesh is not visible in any camera view (editor's scene camera is detecting it too)")]
	public Renderer OptimizeWithMesh;

	[Tooltip("Delta Time for Spine Animator calculations")]
	public EFDeltaType DeltaType = EFDeltaType.SafeDelta;

	[Tooltip("Making update rate stable for target rate.\nIf this value is = 0 then update rate is unlimited.")]
	public float UpdateRate;

	[Tooltip("In some cases you need to use chain corrections, it will cost a bit more in performance, not much but always.")]
	public bool UseCorrections;

	[Tooltip("Sometimes offsetting model's pivot position gives better results using spine animator, offset forward axis so front legs are in centrum and see the difference (generating additional transform inside hierarchy)")]
	public Vector3 MainPivotOffset = new Vector3(0f, 0f, 0f);

	[Tooltip("Generating offset runtime only, allows you to adjust it on prefabs on scene")]
	public bool PivotOffsetOnStart = true;

	[Range(0f, 1f)]
	[Tooltip("If animation of changing segments position should be smoothed - creating a little gumy effect.")]
	public float PosSmoother;

	[Range(0f, 1f)]
	[Tooltip("If animation of changing segments rotation should be smoothed - making it more soft, but don't overuse it!")]
	public float RotSmoother;

	[Range(0f, 1f)]
	[Tooltip("We stretching segments to bigger value than bones are by default to create some extra effect which looks good but sometimes it can stretch to much if you using position smoothing, you can adjust it here.")]
	public float MaxStretching = 1f;

	[Tooltip("Making algorithm referencing back to static rotation if value = 0f | at 1 motion have more range and is more slithery.")]
	[Range(0f, 1f)]
	public float Slithery = 1f;

	[Range(1f, 91f)]
	[Tooltip("Limiting rotation angle difference between each segment of spine.")]
	public float AngleLimit = 40f;

	[Range(0f, 1f)]
	[Tooltip("Smoothing how fast limiting should make segments go back to marginal pose.")]
	public float LimitSmoother = 0.35f;

	[Range(0f, 15f)]
	[Tooltip("How fast spine should be rotated to straight pose when your character moves.")]
	public float StraightenSpeed = 7.5f;

	public bool TurboStraighten;

	[Tooltip("Spine going back to straight position constantly with choosed speed intensity.")]
	[Range(0f, 1f)]
	public float GoBackSpeed;

	[Tooltip("Elastic spring effect good for tails to make them more 'meaty'.")]
	[Range(0f, 1f)]
	public float Springiness;

	[Tooltip("How much effect on spine chain should have character movement.")]
	[Range(0f, 1f)]
	public float MotionInfluence = 1f;

	[Tooltip("Useful when your creature jumps on moving platform, so when platform moves spine is not reacting, by default world space is used (null).")]
	public Transform MotionSpace;

	[Tooltip("Fade rotations to sides or rotation up/down with this parameter - can be helpful for character jump handling")]
	public Vector2 RotationsFade = Vector2.one;

	[SerializeField]
	[HideInInspector]
	private Transform mainPivotOffsetTransform;

	[Tooltip("<! Most models can not need this !> Offset for bones rotations, thanks to that animation is able to rotate to segments in a correct way, like from center of mass.")]
	public Vector3 SegmentsPivotOffset = new Vector3(0f, 0f, 0f);

	[Tooltip("Multiplies distance value between bones segments - can be useful for use with humanoid skeletons")]
	public float DistancesMultiplier = 1f;

	[Tooltip("Pushing segments in world direction (should have included ground collider to collide with).")]
	public Vector3 GravityPower = Vector3.zero;

	protected Vector3 gravityScale = Vector3.zero;

	[Tooltip("[Experimental] Using some simple calculations to make spine bend on colliders.")]
	public bool UseCollisions;

	public List<Collider> IncludedColliders;

	protected List<FImp_ColliderData_Base> IncludedCollidersData;

	protected List<FImp_ColliderData_Base> CollidersDataToCheck;

	[Tooltip("If disabled Colliders can be offsetted a bit in wrong way - check pink spheres in scene view (playmode, with true positions disabled colliders are fitting to stiff reference pose) - but it gives more stable collision projection! But to avoid stuttery you can increase position smoothing.")]
	public bool UseTruePosition;

	public Vector3 OffsetAllColliders = Vector3.zero;

	public AnimationCurve CollidersScale = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	public float CollidersScaleMul = 6.5f;

	[Range(0f, 1f)]
	public float DifferenceScaleFactor;

	[Tooltip("If you want to continue checking collision if segment collides with one collider (very useful for example when you using gravity power with ground)")]
	public bool DetailedCollision = true;

	[SerializeField]
	[HideInInspector]
	private bool _CheckedPivot;

	private bool updateSpineAnimator;

	private bool callSpineReposeCalculations = true;

	public string EditorIconPath
	{
		get
		{
			if (PlayerPrefs.GetInt("AnimsH", 1) == 0)
			{
				return "";
			}
			return "Spine Animator/SpineAnimator_SmallIcon";
		}
	}

	[Obsolete("Use SpineAnimatorAmount instead, but remember that it works in reversed way -> SpineAnimatorAmount 1 = BlendToOriginal 0  and  SpineAnimatorAmount 0 = BlendToOriginal 1")]
	public float BlendToOriginal
	{
		get
		{
			return 1f - SpineAnimatorAmount;
		}
		set
		{
			SpineAnimatorAmount = 1f - value;
		}
	}

	public bool EndBoneIsHead
	{
		get
		{
			return LastBoneLeading;
		}
		set
		{
			LastBoneLeading = EndBoneIsHead;
		}
	}

	private void RemovePivotOffset()
	{
		if (!Application.isPlaying && (bool)mainPivotOffsetTransform)
		{
			RestoreBasePivotChildren();
		}
	}

	public void UpdatePivotOffsetState()
	{
		if (SpineBones.Count <= 1)
		{
			return;
		}
		if (MainPivotOffset == Vector3.zero)
		{
			if ((bool)mainPivotOffsetTransform && mainPivotOffsetTransform.childCount > 0)
			{
				mainPivotOffsetTransform.localPosition = MainPivotOffset;
				RestoreBasePivotChildren();
			}
			return;
		}
		if (!mainPivotOffsetTransform)
		{
			mainPivotOffsetTransform = new GameObject("Main Pivot Offset-Spine Animator-" + base.name).transform;
			mainPivotOffsetTransform.SetParent(GetBaseTransform(), worldPositionStays: false);
			mainPivotOffsetTransform.localPosition = Vector3.zero;
			mainPivotOffsetTransform.localRotation = Quaternion.identity;
			mainPivotOffsetTransform.localScale = Vector3.one;
		}
		if (mainPivotOffsetTransform.childCount == 0)
		{
			for (int i = GetBaseTransform().childCount - 1; i >= 0; i--)
			{
				if (!(GetBaseTransform().GetChild(i) == mainPivotOffsetTransform))
				{
					GetBaseTransform().GetChild(i).SetParent(mainPivotOffsetTransform, worldPositionStays: true);
				}
			}
		}
		mainPivotOffsetTransform.localPosition = MainPivotOffset;
	}

	private void RestoreBasePivotChildren()
	{
		if (!_editor_isQuitting)
		{
			for (int i = mainPivotOffsetTransform.childCount - 1; i >= 0; i--)
			{
				mainPivotOffsetTransform.GetChild(i).SetParent(mainPivotOffsetTransform.parent, worldPositionStays: true);
			}
		}
	}

	private void PreMotionBoneOffsets()
	{
		if (UseCorrections && ManualAffectChain && callSpineReposeCalculations)
		{
			PreMotionNoHead();
			PreMotionHead();
		}
	}

	private void PreMotionNoHead()
	{
		if (SegmentsPivotOffset.sqrMagnitude != 0f)
		{
			for (int i = 1 - chainIndexOffset; i < SpineBones.Count - chainIndexOffset; i++)
			{
				SegmentPreOffsetWithPivot(i);
			}
		}
		else
		{
			for (int j = 1 - chainIndexOffset; j < SpineBones.Count - chainIndexOffset; j++)
			{
				SegmentPreOffset(j);
			}
		}
	}

	private void PreMotionHead()
	{
		if (SegmentsPivotOffset.sqrMagnitude != 0f)
		{
			SegmentPreOffsetWithPivot(leadingBoneIndex);
		}
		else
		{
			SegmentPreOffset(leadingBoneIndex);
		}
	}

	private void SegmentPreOffset(int i)
	{
		if (SpineBones[i].ManualPosOffset.sqrMagnitude != 0f)
		{
			SpineBones[i].transform.position += SpineBones[i].ProceduralRotation * SpineBones[i].ManualPosOffset;
		}
		SpineBones[i].transform.rotation *= SpineBones[i].ManualRotOffset;
	}

	private void SegmentPreOffsetWithPivot(int i)
	{
		if (SpineBones[i].ManualPosOffset.sqrMagnitude != 0f)
		{
			SpineBones[i].transform.position += SpineBones[i].ProceduralRotation * SpineBones[i].ManualPosOffset;
		}
		SpineBones[i].transform.position += SpineBones[i].ProceduralRotation * (SegmentsPivotOffset * (SpineBones[i].BoneLength * DistancesMultiplier * BaseTransform.lossyScale.z));
		SpineBones[i].transform.rotation *= SpineBones[i].ManualRotOffset;
	}

	private void PostMotionBoneOffsets()
	{
		if (UseCorrections && !ManualAffectChain)
		{
			PostMotionHead();
			PostMotionNoHead();
		}
	}

	private void PostMotionNoHead()
	{
		if (SegmentsPivotOffset.sqrMagnitude != 0f)
		{
			for (int i = 1 - chainIndexOffset; i < SpineBones.Count - chainIndexOffset; i++)
			{
				SegmentPostOffsetWithPivot(i);
			}
		}
		else
		{
			for (int j = 1 - chainIndexOffset; j < SpineBones.Count - chainIndexOffset; j++)
			{
				SegmentPostOffset(j);
			}
		}
	}

	private void PostMotionHead()
	{
		if (SegmentsPivotOffset.sqrMagnitude != 0f)
		{
			SegmentPostOffsetWithPivot(leadingBoneIndex);
		}
		else
		{
			SegmentPostOffset(leadingBoneIndex);
		}
	}

	private void SegmentPostOffset(int i)
	{
		if (SpineBones[i].ManualPosOffset.sqrMagnitude != 0f)
		{
			SpineBones[i].FinalPosition += SpineBones[i].ProceduralRotation * SpineBones[i].ManualPosOffset;
		}
		SpineBones[i].FinalRotation *= SpineBones[i].ManualRotOffset;
	}

	private void SegmentPostOffsetWithPivot(int i)
	{
		if (SpineBones[i].ManualPosOffset.sqrMagnitude != 0f)
		{
			SpineBones[i].FinalPosition += SpineBones[i].ProceduralRotation * SpineBones[i].ManualPosOffset;
		}
		SpineBones[i].FinalPosition += SpineBones[i].ProceduralRotation * (SegmentsPivotOffset * (SpineBones[i].BoneLength * DistancesMultiplier * BaseTransform.lossyScale.z));
		SpineBones[i].FinalRotation *= SpineBones[i].ManualRotOffset;
	}

	private void BeginPhysicsUpdate()
	{
		gravityScale = GravityPower * delta;
		if (!UseCollisions)
		{
			return;
		}
		if (!collisionInitialized)
		{
			InitColliders();
		}
		else
		{
			RefreshCollidersDataList();
		}
		CollidersDataToCheck.Clear();
		for (int i = 0; i < IncludedCollidersData.Count; i++)
		{
			if (IncludedCollidersData[i].Collider == null)
			{
				forceRefreshCollidersData = true;
				break;
			}
			if (IncludedCollidersData[i].Collider.gameObject.activeInHierarchy)
			{
				IncludedCollidersData[i].RefreshColliderData();
				CollidersDataToCheck.Add(IncludedCollidersData[i]);
			}
		}
	}

	public void RefreshCollidersDataList()
	{
		if (IncludedColliders.Count == IncludedCollidersData.Count && !forceRefreshCollidersData)
		{
			return;
		}
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
		forceRefreshCollidersData = false;
	}

	private float GetColliderSphereRadiusFor(int i)
	{
		int backBone = i - 1;
		if (LastBoneLeading)
		{
			if (i == SpineBones.Count - 1)
			{
				return 0f;
			}
			backBone = i + 1;
		}
		else if (i == 0)
		{
			return 0f;
		}
		float refDistance = 1f;
		if (SpineBones.Count > 1)
		{
			refDistance = Vector3.Distance(SpineBones[1].transform.position, SpineBones[0].transform.position);
		}
		float singleScale = Mathf.Lerp(refDistance, (SpineBones[i].transform.position - SpineBones[backBone].transform.position).magnitude * 0.5f, DifferenceScaleFactor);
		float div = SpineBones.Count - 1;
		if (div <= 0f)
		{
			div = 1f;
		}
		float step = 1f / div;
		return 0.5f * singleScale * CollidersScaleMul * CollidersScale.Evaluate(step * (float)i);
	}

	public void AddCollider(Collider collider)
	{
		if (!IncludedColliders.Contains(collider))
		{
			IncludedColliders.Add(collider);
		}
	}

	private void InitColliders()
	{
		for (int i = 0; i < SpineBones.Count; i++)
		{
			SpineBones[i].CollisionRadius = GetColliderSphereRadiusFor(i);
		}
		IncludedCollidersData = new List<FImp_ColliderData_Base>();
		RefreshCollidersDataList();
		collisionInitialized = true;
	}

	public void CheckForColliderDuplicates()
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
	}

	public void PushIfSegmentInsideCollider(SpineBone bone, ref Vector3 targetPoint)
	{
		Vector3 offset;
		if (UseTruePosition)
		{
			Vector3 theTarget = targetPoint;
			offset = bone.FinalPosition - theTarget + bone.transform.TransformVector(bone.ColliderOffset + OffsetAllColliders);
		}
		else
		{
			offset = bone.transform.TransformVector(bone.ColliderOffset + OffsetAllColliders);
		}
		if (!DetailedCollision)
		{
			for (int i = 0; i < CollidersDataToCheck.Count && !CollidersDataToCheck[i].PushIfInside(ref targetPoint, bone.GetCollisionRadiusScaled(), offset); i++)
			{
			}
			return;
		}
		for (int j = 0; j < CollidersDataToCheck.Count; j++)
		{
			CollidersDataToCheck[j].PushIfInside(ref targetPoint, bone.GetCollisionRadiusScaled(), offset);
		}
	}

	private void CalculateBonesCoordinates()
	{
		if (LastBoneLeading)
		{
			for (int i = SpineBones.Count - 2; i >= 0; i--)
			{
				CalculateTargetBoneRotation(i);
				CalculateTargetBonePosition(i);
				SpineBones[i].CalculateDifferencePose(ModelUpAxis, ModelRightAxis);
				SpineBones[i].ApplyDifferencePose();
			}
		}
		else
		{
			for (int j = 1; j < SpineBones.Count; j++)
			{
				CalculateTargetBoneRotation(j);
				CalculateTargetBonePosition(j);
				SpineBones[j].CalculateDifferencePose(ModelUpAxis, ModelRightAxis);
				SpineBones[j].ApplyDifferencePose();
			}
		}
	}

	private void CalculateTargetBonePosition(int index)
	{
		SpineBone otherBone = SpineBones[index - chainIndexDirection];
		SpineBone currentBone = SpineBones[index];
		Vector3 targetPosition = otherBone.ProceduralPosition - currentBone.ProceduralRotation * ModelForwardAxisScaled * (currentBone.BoneLength * DistancesMultiplier);
		if (currentBone.Collide)
		{
			targetPosition += gravityScale;
		}
		if (Springiness > 0f && !LastBoneLeading)
		{
			Vector3 backPosDiff = currentBone.ProceduralPosition - currentBone.PreviousPosition;
			Vector3 newPos = currentBone.ProceduralPosition;
			currentBone.PreviousPosition = currentBone.ProceduralPosition;
			newPos += backPosDiff * (1f - Mathf.Lerp(0.05f, 0.25f, Springiness));
			float magnitude = (otherBone.ProceduralPosition - newPos).magnitude;
			Matrix4x4 otherLocalToWorld = otherBone.transform.localToWorldMatrix;
			otherLocalToWorld.SetColumn(3, otherBone.ProceduralPosition);
			Vector3 vector = otherLocalToWorld.MultiplyPoint3x4(currentBone.transform.localPosition);
			Vector3 diffPosVector = vector - newPos;
			newPos += diffPosVector * Mathf.Lerp(0.05f, 0.2f, Springiness);
			diffPosVector = vector - newPos;
			float distance = diffPosVector.magnitude;
			float maxDistance = magnitude * (1f - Mathf.Lerp(0f, 0.2f, Springiness)) * 2f;
			if (distance > maxDistance)
			{
				newPos += diffPosVector * ((distance - maxDistance) / distance);
			}
			if (MaxStretching < 1f)
			{
				float dist = Vector3.Distance(currentBone.ProceduralPosition, newPos);
				if (dist > 0f)
				{
					float maxDist = currentBone.BoneLength * 4f * MaxStretching;
					if (dist > maxDist)
					{
						newPos = Vector3.Lerp(newPos, targetPosition, Mathf.InverseLerp(dist, 0f, maxDist));
					}
				}
			}
			targetPosition = Vector3.Lerp(targetPosition, newPos, Mathf.Lerp(0.3f, 0.9f, Springiness));
		}
		if (PosSmoother > 0f && MaxStretching < 1f)
		{
			float dist2 = Vector3.Distance(currentBone.ProceduralPosition, targetPosition);
			if (dist2 > 0f)
			{
				float maxDist2 = currentBone.BoneLength * 4f * MaxStretching;
				if (dist2 > maxDist2)
				{
					currentBone.ProceduralPosition = Vector3.Lerp(currentBone.ProceduralPosition, targetPosition, Mathf.InverseLerp(dist2, 0f, maxDist2));
				}
			}
		}
		if (UseCollisions && currentBone.Collide)
		{
			PushIfSegmentInsideCollider(currentBone, ref targetPosition);
		}
		if (PosSmoother == 0f)
		{
			currentBone.ProceduralPosition = targetPosition;
		}
		else
		{
			currentBone.ProceduralPosition = Vector3.LerpUnclamped(currentBone.ProceduralPosition, targetPosition, Mathf.LerpUnclamped(1f, unifiedDelta, PosSmoother));
		}
	}

	private void CalculateTargetBoneRotation(int index)
	{
		SpineBone otherBone = SpineBones[index - chainIndexDirection];
		SpineBone currentBone = SpineBones[index];
		Quaternion backRotationRef = ((Slithery >= 1f) ? otherBone.ProceduralRotation : ((!(Slithery > 0f)) ? currentBone.ReferenceRotation : Quaternion.LerpUnclamped(currentBone.ReferenceRotation, otherBone.ProceduralRotation, Slithery)));
		Vector3 towards = otherBone.ProceduralPosition - currentBone.ProceduralPosition;
		if (towards == Vector3.zero)
		{
			towards = currentBone.transform.rotation * currentBone.DefaultForward;
		}
		if (RotationsFade != Vector2.one)
		{
			towards.x *= RotationsFade.x;
			towards.z *= RotationsFade.x;
			towards.y *= RotationsFade.y;
		}
		Quaternion targetLookRotation = Quaternion.LookRotation(towards, otherBone.ProceduralRotation * ModelUpAxis);
		targetLookRotation = Quaternion.Euler(currentBone.ProceduralRotation.eulerAngles.x, targetLookRotation.eulerAngles.y, currentBone.ProceduralRotation.eulerAngles.z);
		if (AngleLimit < 91f)
		{
			float lookDiff = Quaternion.Angle(targetLookRotation, backRotationRef);
			if (lookDiff > AngleLimit)
			{
				float limiting = 0f;
				limiting = Mathf.InverseLerp(0f, lookDiff, lookDiff - AngleLimit);
				Quaternion limitRange = Quaternion.LerpUnclamped(targetLookRotation, backRotationRef, limiting);
				float elasticPush = Mathf.Min(1f, lookDiff / (AngleLimit / 0.75f));
				elasticPush = Mathf.Sqrt(Mathf.Pow(elasticPush, 4f)) * elasticPush;
				targetLookRotation = ((LimitSmoother != 0f) ? Quaternion.LerpUnclamped(targetLookRotation, limitRange, unifiedDelta * (1f - LimitSmoother) * 50f * elasticPush) : Quaternion.LerpUnclamped(targetLookRotation, limitRange, elasticPush));
			}
		}
		if (GoBackSpeed <= 0f)
		{
			if (StraightenSpeed > 0f)
			{
				float diff = (currentBone.ReferencePosition - currentBone.PreviousReferencePosition).magnitude / currentBone.GetUnscalledBoneLength();
				if (diff > 0.5f)
				{
					diff = 0.5f;
				}
				float target = diff * (1f + StraightenSpeed / 5f);
				currentBone.StraightenFactor = Mathf.Lerp(currentBone.StraightenFactor, target, unifiedDelta * (7f + StraightenSpeed));
				if (diff > 0.0001f)
				{
					targetLookRotation = Quaternion.Lerp(targetLookRotation, backRotationRef, unifiedDelta * currentBone.StraightenFactor * (StraightenSpeed + 5f) * (TurboStraighten ? 6f : 1f));
				}
			}
		}
		else
		{
			float straightenVal = 0f;
			if (StraightenSpeed > 0f)
			{
				if (previousPos != RoundPosDiff(SpineBones[leadingBoneIndex].ProceduralPosition))
				{
					currentBone.TargetStraightenFactor = 1f;
				}
				else if (currentBone.TargetStraightenFactor > 0f)
				{
					currentBone.TargetStraightenFactor -= delta * (5f + StraightenSpeed);
				}
				currentBone.StraightenFactor = Mathf.Lerp(currentBone.StraightenFactor, currentBone.TargetStraightenFactor, unifiedDelta * (1f + StraightenSpeed));
				if (currentBone.StraightenFactor > 0.025f)
				{
					straightenVal = currentBone.StraightenFactor * StraightenSpeed * (TurboStraighten ? 6f : 1f);
				}
			}
			targetLookRotation = Quaternion.Lerp(targetLookRotation, backRotationRef, unifiedDelta * (Mathf.Lerp(0f, 55f, GoBackSpeed) + straightenVal));
		}
		if (RotSmoother == 0f)
		{
			currentBone.ProceduralRotation = targetLookRotation;
		}
		else
		{
			currentBone.ProceduralRotation = Quaternion.LerpUnclamped(currentBone.ProceduralRotation, targetLookRotation, Mathf.LerpUnclamped(0f, Mathf.LerpUnclamped(1f, unifiedDelta, RotSmoother), MotionInfluence));
		}
	}

	private void UpdateChainIndexHelperVariables()
	{
		if (chainReverseFlag == LastBoneLeading)
		{
			return;
		}
		chainReverseFlag = LastBoneLeading;
		if (LastBoneLeading)
		{
			leadingBoneIndex = SpineBones.Count - 1;
			chainIndexDirection = -1;
			chainIndexOffset = 1;
			headBone = backHead;
		}
		else
		{
			leadingBoneIndex = 0;
			chainIndexDirection = 1;
			chainIndexOffset = 0;
			headBone = frontHead;
		}
		if (LastBoneLeading)
		{
			for (int i = 0; i < SpineBones.Count; i++)
			{
				SpineBones[i].SetDistanceForFrameBackward();
			}
		}
		else
		{
			for (int j = 0; j < SpineBones.Count; j++)
			{
				SpineBones[j].SetDistanceForFrameForward();
			}
		}
	}

	private void RefreshReferencePose()
	{
		if (headBone == null)
		{
			return;
		}
		if ((bool)HeadAnchor && !AnimateAnchor)
		{
			SpineBones[leadingBoneIndex].transform.localRotation = SpineBones[leadingBoneIndex].InitialLocalRotation;
		}
		if (LastBoneLeading)
		{
			headBone.SetCoordsForFrameBackward();
			if (!HeadAnchor)
			{
				SpineBones[leadingBoneIndex].UpdateReferencePosition(headBone.SnapshotPosition);
				SpineBones[leadingBoneIndex].ReferenceRotation = BaseTransform.rotation;
			}
			else
			{
				SpineBones[leadingBoneIndex].UpdateReferencePosition(headBone.transform.position);
				SpineBones[leadingBoneIndex].ReferenceRotation = BaseTransform.rotation;
			}
			if (LeadBoneRotationOffset.sqrMagnitude != 0f && LeadBoneOffsetReference)
			{
				SpineBones[leadingBoneIndex].ReferenceRotation *= Quaternion.Euler(LeadBoneRotationOffset);
			}
			if (ReverseForward)
			{
				SpineBones[leadingBoneIndex].ReferenceRotation *= Rotate180;
			}
			for (int i = SpineBones.Count - 2; i >= 0; i--)
			{
				SpineBones[i].ReferenceRotation = SpineBones[i + 1].ReferenceRotation;
				SpineBones[i].UpdateReferencePosition(SpineBones[i + 1].ReferencePosition - SpineBones[i].ReferenceRotation * ModelForwardAxis * (SpineBones[i].BoneLength * DistancesMultiplier * BaseTransform.lossyScale.x));
			}
		}
		else
		{
			headBone.SetCoordsForFrameForward();
			if (!HeadAnchor)
			{
				SpineBones[leadingBoneIndex].UpdateReferencePosition(headBone.SnapshotPosition);
				SpineBones[leadingBoneIndex].ReferenceRotation = BaseTransform.rotation;
			}
			else
			{
				SpineBones[leadingBoneIndex].UpdateReferencePosition(headBone.transform.position);
				SpineBones[leadingBoneIndex].ReferenceRotation = headBone.GetLocalRotationDiff();
			}
			if (LeadBoneRotationOffset.sqrMagnitude != 0f && LeadBoneOffsetReference)
			{
				SpineBones[leadingBoneIndex].ReferenceRotation *= Quaternion.Euler(LeadBoneRotationOffset);
			}
			if (ReverseForward)
			{
				SpineBones[leadingBoneIndex].ReferenceRotation *= Rotate180;
			}
			for (int j = 1; j < SpineBones.Count; j++)
			{
				SpineBones[j].ReferenceRotation = SpineBones[j - 1].ReferenceRotation;
				SpineBones[j].UpdateReferencePosition(SpineBones[j - 1].ReferencePosition - SpineBones[j].ReferenceRotation * ModelForwardAxis * (SpineBones[j].BoneLength * DistancesMultiplier * BaseTransform.lossyScale.x));
			}
		}
	}

	private void ReposeSpine()
	{
		UpdateChainIndexHelperVariables();
		RefreshReferencePose();
		for (int i = 0; i < SpineBones.Count; i++)
		{
			SpineBones[i].ProceduralPosition = SpineBones[i].ReferencePosition;
			SpineBones[i].ProceduralRotation = SpineBones[i].ReferenceRotation;
			SpineBones[i].PreviousPosition = SpineBones[i].ReferencePosition;
			SpineBones[i].FinalPosition = SpineBones[i].ReferencePosition;
			SpineBones[i].FinalRotation = SpineBones[i].ReferenceRotation;
		}
	}

	private void BeginBaseBonesUpdate()
	{
		if (HeadAnchor != null)
		{
			SpineBones[leadingBoneIndex].ProceduralRotation = headBone.GetLocalRotationDiff();
			SpineBones[leadingBoneIndex].ProceduralPosition = SpineBones[leadingBoneIndex].transform.position;
		}
		else
		{
			SpineBones[leadingBoneIndex].ProceduralPosition = SpineBones[leadingBoneIndex].ReferencePosition;
			SpineBones[leadingBoneIndex].ProceduralRotation = SpineBones[leadingBoneIndex].ReferenceRotation;
		}
		if (LeadBoneRotationOffset.sqrMagnitude != 0f && !LeadBoneOffsetReference)
		{
			SpineBones[leadingBoneIndex].ProceduralRotation *= Quaternion.Euler(LeadBoneRotationOffset);
		}
		SpineBones[leadingBoneIndex].CalculateDifferencePose(ModelUpAxis, ModelRightAxis);
		SpineBones[leadingBoneIndex].ApplyDifferencePose();
	}

	private IEnumerator LateFixed()
	{
		WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
		lateFixedIsRunning = true;
		do
		{
			yield return fixedWait;
			PreCalibrateBones();
			fixedAllow = true;
		}
		while (lateFixedIsRunning);
	}

	public void OnDestroy()
	{
		RemovePivotOffset();
	}

	private void OnValidate()
	{
		if (!_CheckedPivot)
		{
			if (MainPivotOffset != Vector3.zero)
			{
				PivotOffsetOnStart = false;
			}
			_CheckedPivot = true;
		}
		if (SpineBones == null)
		{
			SpineBones = new List<SpineBone>();
		}
		if (!PivotOffsetOnStart)
		{
			UpdatePivotOffsetState();
		}
		if (UseCollisions)
		{
			CheckForColliderDuplicates();
		}
		if (UpdateRate < 0f)
		{
			UpdateRate = 0f;
		}
		ModelRightAxis = Vector3.Cross(ModelForwardAxis, ModelUpAxis);
	}

	public void AddConnector(FSpineBoneConnector connector)
	{
		if (connectors == null)
		{
			connectors = new List<FSpineBoneConnector>();
		}
		if (!connectors.Contains(connector))
		{
			connectors.Add(connector);
		}
	}

	public void Init()
	{
		if (SpineBones.Count == 0)
		{
			if (SpineTransforms.Count <= 2)
			{
				Debug.Log("[SPINE ANIMATOR] could not initialize Spine Animator inside '" + base.name + "' because there are no bones to animate!");
				return;
			}
			CreateSpineChain(SpineTransforms[0], SpineTransforms[SpineTransforms.Count - 1]);
			Debug.Log("[SPINE ANIMATOR] Auto Bone Conversion from old version of Spine Animator! Please select your objects with Spine Animator to pre-convert it instead of automatically doing it when game Starts! (" + base.name + ")");
		}
		if (initialized)
		{
			Debug.Log("[Spine Animator] " + base.name + " is already initialized!");
			return;
		}
		if (BaseTransform == null)
		{
			BaseTransform = FindBaseTransform();
		}
		for (int i = 0; i < SpineBones.Count; i++)
		{
			if (Vector3.Distance(b: (i != SpineBones.Count - 1) ? SpineBones[i + 1].transform.position : ((SpineBones[i].transform.childCount <= 0) ? (SpineBones[i - 1].transform.position + (SpineBones[i - 1].transform.position - SpineBones[i].transform.position)) : SpineBones[i].transform.GetChild(0).position), a: SpineBones[i].transform.position) < 0.01f)
			{
				float refDistance = (SpineBones[SpineBones.Count - 1].transform.position - SpineBones[SpineBones.Count - 2].transform.parent.position).magnitude;
				Vector3 forw = SpineBones[i].transform.position - BaseTransform.position;
				Vector3 loc = BaseTransform.InverseTransformDirection(forw);
				loc.y = 0f;
				loc.Normalize();
				if (i + 1 >= SpineBones.Count)
				{
					return;
				}
				SpineBones[i + 1].DefaultForward = loc;
				SpineBones[i + 1].transform.position = SpineBones[i + 1].transform.position + BaseTransform.TransformDirection(loc) * refDistance * -0.125f;
			}
		}
		referenceDistance = 0f;
		for (int j = 0; j < SpineBones.Count; j++)
		{
			SpineBones[j].PrepareBone(BaseTransform, SpineBones, j);
			referenceDistance += SpineBones[j].BoneLength;
		}
		referenceDistance /= SpineBones.Count;
		frontHead = new HeadBone(SpineBones[0].transform);
		frontHead.PrepareBone(BaseTransform, SpineBones, 0);
		backHead = new HeadBone(SpineBones[SpineBones.Count - 1].transform);
		backHead.PrepareBone(BaseTransform, SpineBones, SpineBones.Count - 1);
		if (LastBoneLeading)
		{
			headBone = backHead;
		}
		else
		{
			headBone = frontHead;
		}
		CollidersDataToCheck = new List<FImp_ColliderData_Base>();
		chainReverseFlag = !LastBoneLeading;
		UpdateChainIndexHelperVariables();
		ReposeSpine();
		initialized = true;
	}

	public void CreateSpineChain(Transform start, Transform end)
	{
		if (start == null || end == null)
		{
			Debug.Log("[SPINE ANIMATOR] Can't create spine chain if one of the bones is null!");
			return;
		}
		List<Transform> fullChain = new List<Transform>();
		Transform p = end;
		while (p != null && !(p == start))
		{
			fullChain.Add(p);
			p = p.parent;
		}
		if (p == null)
		{
			Debug.Log("[SPINE ANIMATOR] '" + start.name + "' is not child of '" + end.name + "' !");
			return;
		}
		fullChain.Add(start);
		fullChain.Reverse();
		SpineBones = new List<SpineBone>();
		for (int i = 0; i < fullChain.Count; i++)
		{
			SpineBone bone = new SpineBone(fullChain[i]);
			SpineBones.Add(bone);
		}
	}

	private void PreCalibrateBones()
	{
		for (int i = 0; i < SpineBones.Count; i++)
		{
			SpineBones[i].transform.localPosition = SpineBones[i].InitialLocalPosition;
			SpineBones[i].transform.localRotation = SpineBones[i].InitialLocalRotation;
		}
		if (BonesFixers.Count > 0)
		{
			for (int j = 0; j < BonesFixers.Count; j++)
			{
				BonesFixers[j].Calibration();
			}
		}
	}

	private void CalibrateBones()
	{
		if (BonesFixers.Count > 0)
		{
			for (int i = 0; i < BonesFixers.Count; i++)
			{
				BonesFixers[i].UpdateOnAnimator();
			}
		}
		if (connectors != null)
		{
			for (int j = 0; j < connectors.Count; j++)
			{
				connectors[j].RememberAnimatorState();
			}
		}
		ModelForwardAxisScaled = Vector3.Scale(ModelForwardAxis, BaseTransform.localScale);
		ModelUpAxisScaled = Vector3.Scale(ModelUpAxis, BaseTransform.localScale);
	}

	private void DeltaTimeCalculations()
	{
		switch (DeltaType)
		{
		case EFDeltaType.SafeDelta:
			delta = Mathf.Lerp(delta, GetClampedSmoothDelta(), 0.05f);
			break;
		case EFDeltaType.DeltaTime:
			delta = Time.deltaTime;
			break;
		case EFDeltaType.SmoothDeltaTime:
			delta = Time.smoothDeltaTime;
			break;
		case EFDeltaType.UnscaledDeltaTime:
			delta = Time.unscaledDeltaTime;
			break;
		case EFDeltaType.FixedDeltaTime:
			delta = Time.fixedDeltaTime;
			break;
		}
		unifiedDelta = Mathf.Pow(delta, 0.1f) * 0.04f;
	}

	private void StableUpdateRateCalculations()
	{
		updateLoops = 1;
		float targetDelta = 1f / UpdateRate;
		elapsedDeltaHelper += delta;
		updateLoops = 0;
		while (elapsedDeltaHelper >= targetDelta)
		{
			elapsedDeltaHelper -= targetDelta;
			if (++updateLoops >= 3)
			{
				elapsedDeltaHelper = 0f;
				break;
			}
		}
	}

	private void ApplyNewBonesCoordinates()
	{
		if (SpineAnimatorAmount >= 1f)
		{
			SpineBones[leadingBoneIndex].transform.position = SpineBones[leadingBoneIndex].FinalPosition;
			SpineBones[leadingBoneIndex].transform.rotation = SpineBones[leadingBoneIndex].FinalRotation;
			for (int i = 1 - chainIndexOffset; i < SpineBones.Count - chainIndexOffset; i++)
			{
				SpineBones[i].transform.position = SpineBones[i].FinalPosition;
				SpineBones[i].transform.rotation = SpineBones[i].FinalRotation;
				SpineBones[i].RefreshFinalLocalPose();
			}
			SpineBones[leadingBoneIndex].RefreshFinalLocalPose();
		}
		else
		{
			SpineBones[leadingBoneIndex].transform.position = Vector3.LerpUnclamped(SpineBones[leadingBoneIndex].transform.position, SpineBones[leadingBoneIndex].FinalPosition, SpineAnimatorAmount * SpineBones[leadingBoneIndex].MotionWeight);
			SpineBones[leadingBoneIndex].transform.rotation = Quaternion.LerpUnclamped(SpineBones[leadingBoneIndex].transform.rotation, SpineBones[leadingBoneIndex].FinalRotation, SpineAnimatorAmount * SpineBones[leadingBoneIndex].MotionWeight);
			for (int j = 1 - chainIndexOffset; j < SpineBones.Count - chainIndexOffset; j++)
			{
				SpineBones[j].transform.position = Vector3.LerpUnclamped(SpineBones[j].transform.position, SpineBones[j].FinalPosition, SpineAnimatorAmount * SpineBones[j].MotionWeight);
				SpineBones[j].transform.rotation = Quaternion.LerpUnclamped(SpineBones[j].transform.rotation, SpineBones[j].FinalRotation, SpineAnimatorAmount * SpineBones[j].MotionWeight);
				SpineBones[j].RefreshFinalLocalPose();
			}
			SpineBones[leadingBoneIndex].RefreshFinalLocalPose();
		}
	}

	private void EndUpdate()
	{
		previousPos = SpineBones[leadingBoneIndex].ProceduralPosition;
		if (connectors != null)
		{
			for (int i = 0; i < connectors.Count; i++)
			{
				connectors[i].RefreshAnimatorState();
			}
		}
		if (BonesFixers.Count > 0)
		{
			for (int j = 0; j < BonesFixers.Count; j++)
			{
				BonesFixers[j].UpdateAfterProcedural();
			}
		}
	}

	public void OnDrop(PointerEventData data)
	{
	}

	public Transform FindBaseTransform()
	{
		Transform target = base.transform;
		Transform c = base.transform.parent;
		FSpineAnimator mySpine = null;
		if (c != null)
		{
			for (int i = 0; i < 32; i++)
			{
				Transform p = c.parent;
				mySpine = c.GetComponent<FSpineAnimator>();
				if ((bool)mySpine)
				{
					break;
				}
				c = p;
				if (p == null)
				{
					break;
				}
			}
		}
		if (mySpine != null)
		{
			target = ((!(mySpine.BaseTransform != null)) ? mySpine.transform : mySpine.BaseTransform);
			if (mySpine.transform != base.transform)
			{
				UpdateAsLast = true;
			}
		}
		return target;
	}

	public SpineBone GetLeadingBone()
	{
		if (SpineBones == null || SpineBones.Count == 0)
		{
			return null;
		}
		if (LastBoneLeading)
		{
			return SpineBones[SpineBones.Count - 1];
		}
		return SpineBones[0];
	}

	public SpineBone GetEndBone()
	{
		if (SpineBones == null || SpineBones.Count == 0)
		{
			return null;
		}
		if (LastBoneLeading)
		{
			return SpineBones[0];
		}
		return SpineBones[SpineBones.Count - 1];
	}

	public Transform GetHeadBone()
	{
		if (SpineBones.Count <= 0)
		{
			return base.transform;
		}
		if (LastBoneLeading)
		{
			return SpineBones[SpineBones.Count - 1].transform;
		}
		return SpineBones[0].transform;
	}

	public SpineBone GetLeadBone()
	{
		if (LastBoneLeading)
		{
			return SpineBones[SpineBones.Count - 1];
		}
		return SpineBones[0];
	}

	public Transform GetBaseTransform()
	{
		if (BaseTransform == null)
		{
			return base.transform;
		}
		return BaseTransform;
	}

	private Vector3 RoundPosDiff(Vector3 pos, int digits = 1)
	{
		return new Vector3((float)Math.Round(pos.x, digits), (float)Math.Round(pos.y, digits), (float)Math.Round(pos.z, digits));
	}

	private Vector3 RoundToBiggestValue(Vector3 vec)
	{
		int biggest = 0;
		if (Mathf.Abs(vec.y) > Mathf.Abs(vec.x))
		{
			biggest = 1;
			if (Mathf.Abs(vec.z) > Mathf.Abs(vec.y))
			{
				biggest = 2;
			}
		}
		else if (Mathf.Abs(vec.z) > Mathf.Abs(vec.x))
		{
			biggest = 2;
		}
		vec = biggest switch
		{
			0 => new Vector3(Mathf.Round(vec.x), 0f, 0f), 
			1 => new Vector3(0f, Mathf.Round(vec.y), 0f), 
			_ => new Vector3(0f, 0f, Mathf.Round(vec.z)), 
		};
		return vec;
	}

	private float GetClampedSmoothDelta()
	{
		return Mathf.Clamp(Time.smoothDeltaTime, 0f, 0.1f);
	}

	public List<Transform> GetOldSpineTransforms()
	{
		return SpineTransforms;
	}

	public void ClearOldSpineTransforms()
	{
		if (SpineTransforms != null)
		{
			SpineTransforms.Clear();
		}
	}

	public void User_ChangeParameter(EParamChange parameter, float to, float transitionDuration, float executionDelay = 0f)
	{
		if (transitionDuration <= 0f && executionDelay <= 0f)
		{
			SetValue(parameter, to);
		}
		else
		{
			StartCoroutine(IEChangeValue(parameter, to, transitionDuration, executionDelay));
		}
	}

	public void User_ChangeParameterAndRestore(EParamChange parameter, float to, float transitionDuration, float restoreAfter = 0f)
	{
		float startVal = GetValue(parameter);
		StartCoroutine(IEChangeValue(parameter, to, transitionDuration, 0f));
		StartCoroutine(IEChangeValue(parameter, startVal, transitionDuration, transitionDuration + restoreAfter));
	}

	public void User_ResetBones()
	{
		_ResetBones();
	}

	private IEnumerator IEChangeValue(EParamChange param, float to, float duration, float executionDelay)
	{
		if (executionDelay > 0f)
		{
			yield return new WaitForSeconds(executionDelay);
		}
		if (duration > 0f)
		{
			float elapsed = 0f;
			float startVal = GetValue(param);
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float progress = elapsed / duration;
				if (progress > 1f)
				{
					progress = 1f;
				}
				SetValue(param, Mathf.LerpUnclamped(startVal, to, progress));
				yield return null;
			}
		}
		SetValue(param, to);
	}

	private float GetValue(EParamChange param)
	{
		return param switch
		{
			EParamChange.GoBackSpeed => GoBackSpeed, 
			EParamChange.SpineAnimatorAmount => SpineAnimatorAmount, 
			EParamChange.AngleLimit => AngleLimit, 
			EParamChange.StraightenSpeed => StraightenSpeed, 
			EParamChange.PositionSmoother => PosSmoother, 
			EParamChange.RotationSmoother => RotSmoother, 
			_ => 0f, 
		};
	}

	private void SetValue(EParamChange param, float val)
	{
		switch (param)
		{
		case EParamChange.GoBackSpeed:
			GoBackSpeed = val;
			break;
		case EParamChange.SpineAnimatorAmount:
			SpineAnimatorAmount = val;
			break;
		case EParamChange.AngleLimit:
			AngleLimit = val;
			break;
		case EParamChange.StraightenSpeed:
			StraightenSpeed = val;
			break;
		case EParamChange.PositionSmoother:
			PosSmoother = val;
			break;
		case EParamChange.RotationSmoother:
			RotSmoother = val;
			break;
		}
	}

	private void _ResetBones()
	{
		if (!LastBoneLeading)
		{
			for (int i = SpineBones.Count - 1; i >= 0; i--)
			{
				SpineBones[i].ProceduralPosition = SpineBones[i].ReferencePosition;
				SpineBones[i].ProceduralRotation = SpineBones[i].ReferenceRotation;
				SpineBones[i].PreviousPosition = SpineBones[i].ReferencePosition;
				SpineBones[i].FinalPosition = SpineBones[i].ReferencePosition;
				SpineBones[i].FinalRotation = SpineBones[i].ReferenceRotation;
			}
		}
		else
		{
			for (int j = 0; j < SpineBones.Count; j++)
			{
				SpineBones[j].ProceduralPosition = SpineBones[j].ReferencePosition;
				SpineBones[j].ProceduralRotation = SpineBones[j].ReferenceRotation;
				SpineBones[j].PreviousPosition = SpineBones[j].ReferencePosition;
				SpineBones[j].FinalPosition = SpineBones[j].ReferencePosition;
				SpineBones[j].FinalRotation = SpineBones[j].ReferenceRotation;
			}
		}
		float preBack = GoBackSpeed;
		GoBackSpeed = 10f;
		Update();
		FixedUpdate();
		delta = 0.25f;
		LateUpdate();
		GoBackSpeed = preBack;
	}

	private void Reset()
	{
		BaseTransform = FindBaseTransform();
		_CheckedPivot = true;
	}

	private void Start()
	{
		if (UpdateAsLast)
		{
			base.enabled = false;
			base.enabled = true;
		}
		if (BaseTransform == null)
		{
			BaseTransform = base.transform;
		}
		initialized = false;
		if (PivotOffsetOnStart && mainPivotOffsetTransform == null)
		{
			UpdatePivotOffsetState();
		}
		if (!StartAfterTPose)
		{
			Init();
		}
		else
		{
			initAfterTPoseCounter = 0;
		}
	}

	internal void Update()
	{
		if (!initialized)
		{
			if (!StartAfterTPose)
			{
				updateSpineAnimator = false;
				return;
			}
			if (initAfterTPoseCounter <= 5)
			{
				initAfterTPoseCounter++;
				updateSpineAnimator = false;
				return;
			}
			Init();
		}
		if (OptimizeWithMesh != null && !OptimizeWithMesh.isVisible)
		{
			updateSpineAnimator = false;
			return;
		}
		if (delta <= Mathf.Epsilon)
		{
			updateSpineAnimator = false;
		}
		if (SpineBones.Count == 0)
		{
			Debug.LogError("[SPINE ANIMATOR] No spine bones defined in " + base.name + " !");
			initialized = false;
			updateSpineAnimator = false;
			return;
		}
		if (BaseTransform == null)
		{
			BaseTransform = base.transform;
		}
		UpdateChainIndexHelperVariables();
		if (SpineAnimatorAmount <= 0.01f)
		{
			wasBlendedOut = true;
			updateSpineAnimator = false;
			return;
		}
		if (wasBlendedOut)
		{
			ReposeSpine();
			wasBlendedOut = false;
		}
		updateSpineAnimator = true;
		if (AnimatePhysics == EFixedMode.None)
		{
			PreCalibrateBones();
			callSpineReposeCalculations = true;
		}
	}

	internal void FixedUpdate()
	{
		if (updateSpineAnimator && AnimatePhysics == EFixedMode.Basic)
		{
			PreCalibrateBones();
			callSpineReposeCalculations = true;
			fixedUpdated = true;
		}
	}

	internal void LateUpdate()
	{
		if (!updateSpineAnimator)
		{
			return;
		}
		if (AnimatePhysics == EFixedMode.Late)
		{
			if (!lateFixedIsRunning)
			{
				StartCoroutine(LateFixed());
			}
			if (fixedAllow)
			{
				fixedAllow = false;
				callSpineReposeCalculations = true;
			}
		}
		else
		{
			if (lateFixedIsRunning)
			{
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
		CalibrateBones();
		DeltaTimeCalculations();
		if (UpdateRate > 0f)
		{
			StableUpdateRateCalculations();
			unifiedDelta = delta;
			if (UseCorrections && ManualAffectChain)
			{
				if (updateLoops > 0)
				{
					PreMotionNoHead();
				}
				PreMotionHead();
			}
			RefreshReferencePose();
			BeginBaseBonesUpdate();
			for (int i = 0; i < updateLoops; i++)
			{
				BeginPhysicsUpdate();
				if (callSpineReposeCalculations)
				{
					CalculateBonesCoordinates();
				}
			}
			if (UseCorrections && !ManualAffectChain && callSpineReposeCalculations)
			{
				if (updateLoops > 0)
				{
					PostMotionNoHead();
				}
				PostMotionHead();
			}
			if (callSpineReposeCalculations)
			{
				callSpineReposeCalculations = false;
			}
		}
		else
		{
			RefreshReferencePose();
			PreMotionBoneOffsets();
			BeginPhysicsUpdate();
			BeginBaseBonesUpdate();
			if (callSpineReposeCalculations)
			{
				CalculateBonesCoordinates();
				PostMotionBoneOffsets();
				callSpineReposeCalculations = false;
			}
		}
		ApplyNewBonesCoordinates();
		EndUpdate();
	}
}
