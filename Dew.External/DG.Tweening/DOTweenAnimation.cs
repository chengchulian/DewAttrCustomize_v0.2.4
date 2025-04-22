using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening;

[AddComponentMenu("DOTween/DOTween Animation")]
public class DOTweenAnimation : ABSAnimationComponent
{
	public enum AnimationType
	{
		None,
		Move,
		LocalMove,
		Rotate,
		LocalRotate,
		Scale,
		Color,
		Fade,
		Text,
		PunchPosition,
		PunchRotation,
		PunchScale,
		ShakePosition,
		ShakeRotation,
		ShakeScale,
		CameraAspect,
		CameraBackgroundColor,
		CameraFieldOfView,
		CameraOrthoSize,
		CameraPixelRect,
		CameraRect,
		UIWidthHeight,
		FillAmount
	}

	public enum TargetType
	{
		Unset,
		Camera,
		CanvasGroup,
		Image,
		Light,
		RectTransform,
		Renderer,
		SpriteRenderer,
		Rigidbody,
		Rigidbody2D,
		Text,
		Transform,
		tk2dBaseSprite,
		tk2dTextMesh,
		TextMeshPro,
		TextMeshProUGUI
	}

	public bool targetIsSelf = true;

	public GameObject targetGO;

	public bool tweenTargetIsTargetGO = true;

	public float delay;

	public float duration = 1f;

	public Ease easeType = Ease.OutQuad;

	public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public LoopType loopType;

	public int loops = 1;

	public string id = "";

	public bool isRelative;

	public bool isFrom;

	public bool isIndependentUpdate;

	public bool autoKill = true;

	public bool autoGenerate = true;

	public bool isActive = true;

	public bool isValid;

	public Component target;

	public AnimationType animationType;

	public TargetType targetType;

	public TargetType forcedTargetType;

	public bool autoPlay = true;

	public bool useTargetAsV3;

	public float endValueFloat;

	public Vector3 endValueV3;

	public Vector2 endValueV2;

	public Color endValueColor = new Color(1f, 1f, 1f, 1f);

	public string endValueString = "";

	public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

	public Transform endValueTransform;

	public bool optionalBool0;

	public bool optionalBool1;

	public float optionalFloat0;

	public int optionalInt0;

	public RotateMode optionalRotationMode;

	public ScrambleMode optionalScrambleMode;

	public ShakeRandomnessMode optionalShakeRandomnessMode;

	public string optionalString;

	private bool _tweenAutoGenerationCalled;

	private int _playCount = -1;

	private readonly List<Tween> _tmpTweens = new List<Tween>();

	public static event Action<DOTweenAnimation> OnReset;

	private static void Dispatch_OnReset(DOTweenAnimation anim)
	{
		if (DOTweenAnimation.OnReset != null)
		{
			DOTweenAnimation.OnReset(anim);
		}
	}

	private void Awake()
	{
		if (isActive && autoGenerate && (animationType != AnimationType.Move || !useTargetAsV3))
		{
			CreateTween(regenerateIfExists: false, autoPlay);
			_tweenAutoGenerationCalled = true;
		}
	}

	private void Start()
	{
		if (!_tweenAutoGenerationCalled && isActive && autoGenerate)
		{
			CreateTween(regenerateIfExists: false, autoPlay);
			_tweenAutoGenerationCalled = true;
		}
	}

	private void Reset()
	{
		Dispatch_OnReset(this);
	}

	private void OnDestroy()
	{
		if (tween != null && tween.active)
		{
			tween.Kill();
		}
		tween = null;
	}

	public void RewindThenRecreateTween()
	{
		if (tween != null && tween.active)
		{
			tween.Rewind();
		}
		CreateTween(regenerateIfExists: true, andPlay: false);
	}

	public void RewindThenRecreateTweenAndPlay()
	{
		if (tween != null && tween.active)
		{
			tween.Rewind();
		}
		CreateTween(regenerateIfExists: true);
	}

	public void RecreateTween()
	{
		CreateTween(regenerateIfExists: true, andPlay: false);
	}

	public void RecreateTweenAndPlay()
	{
		CreateTween(regenerateIfExists: true);
	}

	public void CreateTween(bool regenerateIfExists = false, bool andPlay = true)
	{
		if (!isValid)
		{
			if (regenerateIfExists)
			{
				Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation isn't valid and its tween won't be created", base.gameObject);
			}
			return;
		}
		if (tween != null)
		{
			if (tween.active)
			{
				if (!regenerateIfExists)
				{
					return;
				}
				tween.Kill();
			}
			tween = null;
		}
		GameObject tweenGO = GetTweenGO();
		if (target == null || tweenGO == null)
		{
			if (targetIsSelf && target == null)
			{
				Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject);
			}
			else
			{
				Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject);
			}
			return;
		}
		if (forcedTargetType != 0)
		{
			targetType = forcedTargetType;
		}
		if (targetType == TargetType.Unset)
		{
			targetType = TypeToDOTargetType(target.GetType());
		}
		switch (animationType)
		{
		case AnimationType.Move:
			if (useTargetAsV3)
			{
				isRelative = false;
				if (endValueTransform == null)
				{
					Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject);
					endValueV3 = Vector3.zero;
				}
				else if (targetType == TargetType.RectTransform)
				{
					RectTransform endValueT = endValueTransform as RectTransform;
					if (endValueT == null)
					{
						Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject);
						endValueV3 = Vector3.zero;
					}
					else
					{
						RectTransform rTarget = target as RectTransform;
						if (rTarget == null)
						{
							Debug.LogWarning($"{base.gameObject.name} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject);
						}
						else
						{
							endValueV3 = DOTweenModuleUI.Utils.SwitchToRectTransform(endValueT, rTarget);
						}
					}
				}
				else
				{
					endValueV3 = endValueTransform.position;
				}
			}
			switch (targetType)
			{
			case TargetType.Transform:
				tween = ((Transform)target).DOMove(endValueV3, duration, optionalBool0);
				break;
			case TargetType.RectTransform:
				tween = ((RectTransform)target).DOAnchorPos3D(endValueV3, duration, optionalBool0);
				break;
			case TargetType.Rigidbody:
				tween = ((Rigidbody)target).DOMove(endValueV3, duration, optionalBool0);
				break;
			case TargetType.Rigidbody2D:
				tween = ((Rigidbody2D)target).DOMove(endValueV3, duration, optionalBool0);
				break;
			}
			break;
		case AnimationType.LocalMove:
			tween = tweenGO.transform.DOLocalMove(endValueV3, duration, optionalBool0);
			break;
		case AnimationType.Rotate:
			switch (targetType)
			{
			case TargetType.Transform:
				tween = ((Transform)target).DORotate(endValueV3, duration, optionalRotationMode);
				break;
			case TargetType.Rigidbody:
				tween = ((Rigidbody)target).DORotate(endValueV3, duration, optionalRotationMode);
				break;
			case TargetType.Rigidbody2D:
				tween = ((Rigidbody2D)target).DORotate(endValueFloat, duration);
				break;
			}
			break;
		case AnimationType.LocalRotate:
			tween = tweenGO.transform.DOLocalRotate(endValueV3, duration, optionalRotationMode);
			break;
		case AnimationType.Scale:
			_ = targetType;
			tween = tweenGO.transform.DOScale(optionalBool0 ? new Vector3(endValueFloat, endValueFloat, endValueFloat) : endValueV3, duration);
			break;
		case AnimationType.UIWidthHeight:
			tween = ((RectTransform)target).DOSizeDelta(optionalBool0 ? new Vector2(endValueFloat, endValueFloat) : endValueV2, duration);
			break;
		case AnimationType.FillAmount:
			tween = ((Image)target).DOFillAmount(endValueFloat, duration);
			break;
		case AnimationType.Color:
			isRelative = false;
			switch (targetType)
			{
			case TargetType.Renderer:
				tween = ((Renderer)target).material.DOColor(endValueColor, duration);
				break;
			case TargetType.Light:
				tween = ((Light)target).DOColor(endValueColor, duration);
				break;
			case TargetType.SpriteRenderer:
				tween = ((SpriteRenderer)target).DOColor(endValueColor, duration);
				break;
			case TargetType.Image:
				tween = ((Graphic)target).DOColor(endValueColor, duration);
				break;
			case TargetType.Text:
				tween = ((Text)target).DOColor(endValueColor, duration);
				break;
			case TargetType.TextMeshProUGUI:
				tween = ((TextMeshProUGUI)target).DOColor(endValueColor, duration);
				break;
			case TargetType.TextMeshPro:
				tween = ((TextMeshPro)target).DOColor(endValueColor, duration);
				break;
			}
			break;
		case AnimationType.Fade:
			isRelative = false;
			switch (targetType)
			{
			case TargetType.Renderer:
				tween = ((Renderer)target).material.DOFade(endValueFloat, duration);
				break;
			case TargetType.Light:
				tween = ((Light)target).DOIntensity(endValueFloat, duration);
				break;
			case TargetType.SpriteRenderer:
				tween = ((SpriteRenderer)target).DOFade(endValueFloat, duration);
				break;
			case TargetType.Image:
				tween = ((Graphic)target).DOFade(endValueFloat, duration);
				break;
			case TargetType.Text:
				tween = ((Text)target).DOFade(endValueFloat, duration);
				break;
			case TargetType.CanvasGroup:
				tween = ((CanvasGroup)target).DOFade(endValueFloat, duration);
				break;
			case TargetType.TextMeshProUGUI:
				tween = ((TextMeshProUGUI)target).DOFade(endValueFloat, duration);
				break;
			case TargetType.TextMeshPro:
				tween = ((TextMeshPro)target).DOFade(endValueFloat, duration);
				break;
			}
			break;
		case AnimationType.Text:
			if (targetType == TargetType.Text)
			{
				tween = ((Text)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
			}
			switch (targetType)
			{
			case TargetType.TextMeshProUGUI:
				tween = ((TextMeshProUGUI)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
				break;
			case TargetType.TextMeshPro:
				tween = ((TextMeshPro)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
				break;
			}
			break;
		case AnimationType.PunchPosition:
			switch (targetType)
			{
			case TargetType.Transform:
				tween = ((Transform)target).DOPunchPosition(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
				break;
			case TargetType.RectTransform:
				tween = ((RectTransform)target).DOPunchAnchorPos(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
				break;
			}
			break;
		case AnimationType.PunchScale:
			tween = tweenGO.transform.DOPunchScale(endValueV3, duration, optionalInt0, optionalFloat0);
			break;
		case AnimationType.PunchRotation:
			tween = tweenGO.transform.DOPunchRotation(endValueV3, duration, optionalInt0, optionalFloat0);
			break;
		case AnimationType.ShakePosition:
			switch (targetType)
			{
			case TargetType.Transform:
				tween = ((Transform)target).DOShakePosition(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0, optionalBool1, optionalShakeRandomnessMode);
				break;
			case TargetType.RectTransform:
				tween = ((RectTransform)target).DOShakeAnchorPos(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0, optionalBool1, optionalShakeRandomnessMode);
				break;
			}
			break;
		case AnimationType.ShakeScale:
			tween = tweenGO.transform.DOShakeScale(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool1, optionalShakeRandomnessMode);
			break;
		case AnimationType.ShakeRotation:
			tween = tweenGO.transform.DOShakeRotation(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool1, optionalShakeRandomnessMode);
			break;
		case AnimationType.CameraAspect:
			tween = ((Camera)target).DOAspect(endValueFloat, duration);
			break;
		case AnimationType.CameraBackgroundColor:
			tween = ((Camera)target).DOColor(endValueColor, duration);
			break;
		case AnimationType.CameraFieldOfView:
			tween = ((Camera)target).DOFieldOfView(endValueFloat, duration);
			break;
		case AnimationType.CameraOrthoSize:
			tween = ((Camera)target).DOOrthoSize(endValueFloat, duration);
			break;
		case AnimationType.CameraPixelRect:
			tween = ((Camera)target).DOPixelRect(endValueRect, duration);
			break;
		case AnimationType.CameraRect:
			tween = ((Camera)target).DORect(endValueRect, duration);
			break;
		}
		if (tween == null)
		{
			return;
		}
		if (isFrom)
		{
			((Tweener)tween).From(isRelative);
		}
		else
		{
			tween.SetRelative(isRelative);
		}
		GameObject setTarget = GetTweenTarget();
		tween.SetTarget(setTarget).SetDelay(delay).SetLoops(loops, loopType)
			.SetAutoKill(autoKill)
			.OnKill(delegate
			{
				tween = null;
			});
		if (isSpeedBased)
		{
			tween.SetSpeedBased();
		}
		if (easeType == Ease.INTERNAL_Custom)
		{
			tween.SetEase(easeCurve);
		}
		else
		{
			tween.SetEase(easeType);
		}
		if (!string.IsNullOrEmpty(id))
		{
			tween.SetId(id);
		}
		tween.SetUpdate(isIndependentUpdate);
		if (hasOnStart)
		{
			if (onStart != null)
			{
				tween.OnStart(onStart.Invoke);
			}
		}
		else
		{
			onStart = null;
		}
		if (hasOnPlay)
		{
			if (onPlay != null)
			{
				tween.OnPlay(onPlay.Invoke);
			}
		}
		else
		{
			onPlay = null;
		}
		if (hasOnUpdate)
		{
			if (onUpdate != null)
			{
				tween.OnUpdate(onUpdate.Invoke);
			}
		}
		else
		{
			onUpdate = null;
		}
		if (hasOnStepComplete)
		{
			if (onStepComplete != null)
			{
				tween.OnStepComplete(onStepComplete.Invoke);
			}
		}
		else
		{
			onStepComplete = null;
		}
		if (hasOnComplete)
		{
			if (onComplete != null)
			{
				tween.OnComplete(onComplete.Invoke);
			}
		}
		else
		{
			onComplete = null;
		}
		if (hasOnRewind)
		{
			if (onRewind != null)
			{
				tween.OnRewind(onRewind.Invoke);
			}
		}
		else
		{
			onRewind = null;
		}
		if (andPlay)
		{
			tween.Play();
		}
		else
		{
			tween.Pause();
		}
		if (hasOnTweenCreated && onTweenCreated != null)
		{
			onTweenCreated.Invoke();
		}
	}

	public List<Tween> GetTweens()
	{
		List<Tween> result = new List<Tween>();
		DOTweenAnimation[] components = GetComponents<DOTweenAnimation>();
		foreach (DOTweenAnimation anim in components)
		{
			if (anim.tween != null && anim.tween.active)
			{
				result.Add(anim.tween);
			}
		}
		return result;
	}

	public void SetAnimationTarget(Component tweenTarget, bool useTweenTargetGameObjectForGroupOperations = true)
	{
		if (TypeToDOTargetType(target.GetType()) != targetType)
		{
			Debug.LogError("DOTweenAnimation ► SetAnimationTarget: the new target is of a different type from the one set in the Inspector");
			return;
		}
		target = tweenTarget;
		targetGO = target.gameObject;
		tweenTargetIsTargetGO = useTweenTargetGameObjectForGroupOperations;
	}

	public override void DOPlay()
	{
		DOTween.Play(GetTweenTarget());
	}

	public override void DOPlayBackwards()
	{
		DOTween.PlayBackwards(GetTweenTarget());
	}

	public override void DOPlayForward()
	{
		DOTween.PlayForward(GetTweenTarget());
	}

	public override void DOPause()
	{
		DOTween.Pause(GetTweenTarget());
	}

	public override void DOTogglePause()
	{
		DOTween.TogglePause(GetTweenTarget());
	}

	public override void DORewind()
	{
		_playCount = -1;
		DOTweenAnimation[] anims = base.gameObject.GetComponents<DOTweenAnimation>();
		for (int i = anims.Length - 1; i > -1; i--)
		{
			Tween t = anims[i].tween;
			if (t != null && t.IsInitialized())
			{
				anims[i].tween.Rewind();
			}
		}
	}

	public override void DORestart()
	{
		DORestart(fromHere: false);
	}

	public override void DORestart(bool fromHere)
	{
		_playCount = -1;
		if (tween == null)
		{
			if (Debugger.logPriority > 1)
			{
				Debugger.LogNullTween(tween);
			}
			return;
		}
		if (fromHere && isRelative)
		{
			ReEvaluateRelativeTween();
		}
		DOTween.Restart(GetTweenTarget());
	}

	public override void DOComplete()
	{
		DOTween.Complete(GetTweenTarget());
	}

	public override void DOGotoAndPause(float time)
	{
		DOGoto(time, andPlay: false);
	}

	public override void DOGotoAndPlay(float time)
	{
		DOGoto(time, andPlay: true);
	}

	private void DOGoto(float time, bool andPlay)
	{
		_tmpTweens.Clear();
		DOTween.TweensByTarget(GetTweenTarget(), playingOnly: false, _tmpTweens);
		if (_tmpTweens.Count == 0)
		{
			Debugger.LogWarning((andPlay ? "DOGotoAndPlay" : "DoGotoAndPause") + " ► tween doesn't exist");
		}
		else
		{
			for (int i = 0; i < _tmpTweens.Count; i++)
			{
				_tmpTweens[i].Goto(time, andPlay);
			}
		}
		_tmpTweens.Clear();
	}

	public override void DOKill()
	{
		DOTween.Kill(GetTweenTarget());
		tween = null;
	}

	public void DOPlayById(string id)
	{
		DOTween.Play(GetTweenTarget(), id);
	}

	public void DOPlayAllById(string id)
	{
		DOTween.Play(id);
	}

	public void DOPauseAllById(string id)
	{
		DOTween.Pause(id);
	}

	public void DOPlayBackwardsById(string id)
	{
		DOTween.PlayBackwards(GetTweenTarget(), id);
	}

	public void DOPlayBackwardsAllById(string id)
	{
		DOTween.PlayBackwards(id);
	}

	public void DOPlayForwardById(string id)
	{
		DOTween.PlayForward(GetTweenTarget(), id);
	}

	public void DOPlayForwardAllById(string id)
	{
		DOTween.PlayForward(id);
	}

	public void DOPlayNext()
	{
		DOTweenAnimation[] anims = GetComponents<DOTweenAnimation>();
		while (_playCount < anims.Length - 1)
		{
			_playCount++;
			DOTweenAnimation anim = anims[_playCount];
			if (anim != null && anim.tween != null && anim.tween.active && !anim.tween.IsPlaying() && !anim.tween.IsComplete())
			{
				anim.tween.Play();
				break;
			}
		}
	}

	public void DORewindAndPlayNext()
	{
		_playCount = -1;
		DOTween.Rewind(GetTweenTarget());
		DOPlayNext();
	}

	public void DORewindAllById(string id)
	{
		_playCount = -1;
		DOTween.Rewind(id);
	}

	public void DORestartById(string id)
	{
		_playCount = -1;
		DOTween.Restart(GetTweenTarget(), id);
	}

	public void DORestartAllById(string id)
	{
		_playCount = -1;
		DOTween.Restart(id);
	}

	public void DOKillById(string id)
	{
		DOTween.Kill(GetTweenTarget(), id);
	}

	public void DOKillAllById(string id)
	{
		DOTween.Kill(id);
	}

	public static TargetType TypeToDOTargetType(Type t)
	{
		string str = t.ToString();
		int dotIndex = str.LastIndexOf(".");
		if (dotIndex != -1)
		{
			str = str.Substring(dotIndex + 1);
		}
		if (str.IndexOf("Renderer") != -1 && str != "SpriteRenderer")
		{
			str = "Renderer";
		}
		if (str == "RawImage" || str == "Graphic")
		{
			str = "Image";
		}
		return (TargetType)Enum.Parse(typeof(TargetType), str);
	}

	public Tween CreateEditorPreview()
	{
		if (Application.isPlaying)
		{
			return null;
		}
		CreateTween(regenerateIfExists: true, autoPlay);
		return tween;
	}

	private GameObject GetTweenGO()
	{
		if (!targetIsSelf)
		{
			return targetGO;
		}
		return base.gameObject;
	}

	private GameObject GetTweenTarget()
	{
		if (!targetIsSelf && tweenTargetIsTargetGO)
		{
			return targetGO;
		}
		return base.gameObject;
	}

	private void ReEvaluateRelativeTween()
	{
		GameObject tweenGO = GetTweenGO();
		if (tweenGO == null)
		{
			Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject);
		}
		else if (animationType == AnimationType.Move)
		{
			((Tweener)tween).ChangeEndValue(tweenGO.transform.position + endValueV3, snapStartValue: true);
		}
		else if (animationType == AnimationType.LocalMove)
		{
			((Tweener)tween).ChangeEndValue(tweenGO.transform.localPosition + endValueV3, snapStartValue: true);
		}
	}
}
