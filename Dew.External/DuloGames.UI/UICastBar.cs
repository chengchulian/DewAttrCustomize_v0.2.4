using System;
using System.Collections;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasGroup))]
public class UICastBar : MonoBehaviour
{
	[Serializable]
	public enum DisplayTime
	{
		Elapsed,
		Remaining
	}

	[Serializable]
	public enum Transition
	{
		Instant,
		Fade
	}

	[Serializable]
	public class ColorStage
	{
		public Color fillColor = Color.white;

		public Color titleColor = Color.white;

		public Color timeColor = Color.white;
	}

	[SerializeField]
	private UIProgressBar m_ProgressBar;

	[SerializeField]
	private Text m_TitleLabel;

	[SerializeField]
	private Text m_TimeLabel;

	[SerializeField]
	private DisplayTime m_DisplayTime = DisplayTime.Remaining;

	[SerializeField]
	private string m_TimeFormat = "0.0 sec";

	[SerializeField]
	private Text m_FullTimeLabel;

	[SerializeField]
	private string m_FullTimeFormat = "0.0 sec";

	[SerializeField]
	private bool m_UseSpellIcon;

	[SerializeField]
	private GameObject m_IconFrame;

	[SerializeField]
	private Image m_IconImage;

	[SerializeField]
	private Image m_FillImage;

	[SerializeField]
	private ColorStage m_NormalColors = new ColorStage();

	[SerializeField]
	private ColorStage m_OnInterruptColors = new ColorStage();

	[SerializeField]
	private ColorStage m_OnFinishColors = new ColorStage();

	[SerializeField]
	private Transition m_InTransition;

	[SerializeField]
	private float m_InTransitionDuration = 0.1f;

	[SerializeField]
	private bool m_BrindToFront = true;

	[SerializeField]
	private Transition m_OutTransition = Transition.Fade;

	[SerializeField]
	private float m_OutTransitionDuration = 0.1f;

	[SerializeField]
	private float m_HideDelay = 0.3f;

	private bool m_IsCasting;

	private float currentCastDuration;

	private float currentCastEndTime;

	private CanvasGroup m_CanvasGroup;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	public bool IsCasting => m_IsCasting;

	public CanvasGroup canvasGroup => m_CanvasGroup;

	protected UICastBar()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected virtual void Awake()
	{
		m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
	}

	protected virtual void Start()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		ApplyColorStage(m_NormalColors);
		if (Application.isPlaying)
		{
			if (m_IconFrame != null)
			{
				m_IconFrame.SetActive(value: false);
			}
			Hide(instant: true);
		}
	}

	public virtual void ApplyColorStage(ColorStage stage)
	{
		if (m_FillImage != null)
		{
			m_FillImage.canvasRenderer.SetColor(stage.fillColor);
		}
		if (m_TitleLabel != null)
		{
			m_TitleLabel.canvasRenderer.SetColor(stage.titleColor);
		}
		if (m_TimeLabel != null)
		{
			m_TimeLabel.canvasRenderer.SetColor(stage.timeColor);
		}
	}

	public void Show()
	{
		Show(instant: false);
	}

	public virtual void Show(bool instant)
	{
		if (m_BrindToFront)
		{
			UIUtility.BringToFront(base.gameObject);
		}
		if (instant || m_InTransition == Transition.Instant)
		{
			m_CanvasGroup.alpha = 1f;
		}
		else
		{
			StartAlphaTween(1f, m_InTransitionDuration, ignoreTimeScale: true);
		}
	}

	public void Hide()
	{
		Hide(instant: false);
	}

	public virtual void Hide(bool instant)
	{
		if (instant || m_OutTransition == Transition.Instant)
		{
			m_CanvasGroup.alpha = 0f;
			OnHideTweenFinished();
		}
		else
		{
			StartAlphaTween(0f, m_OutTransitionDuration, ignoreTimeScale: true);
		}
	}

	public void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
	{
		if (!(m_CanvasGroup == null))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = m_CanvasGroup.alpha;
			floatTween.targetFloat = targetAlpha;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetCanvasAlpha);
			floatTween2.AddOnFinishCallback(OnHideTweenFinished);
			floatTween2.ignoreTimeScale = ignoreTimeScale;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	protected void SetCanvasAlpha(float alpha)
	{
		if (!(m_CanvasGroup == null))
		{
			m_CanvasGroup.alpha = alpha;
		}
	}

	protected virtual void OnHideTweenFinished()
	{
		if (m_IconFrame != null)
		{
			m_IconFrame.SetActive(value: false);
		}
		if (m_IconImage != null)
		{
			m_IconImage.sprite = null;
		}
	}

	public void SetFillAmount(float amount)
	{
		if (m_ProgressBar != null)
		{
			m_ProgressBar.fillAmount = amount;
		}
	}

	private IEnumerator AnimateCast()
	{
		if (m_TimeLabel != null)
		{
			m_TimeLabel.text = ((m_DisplayTime == DisplayTime.Elapsed) ? 0f.ToString(m_TimeFormat) : currentCastDuration.ToString(m_TimeFormat));
		}
		float startTime = ((currentCastEndTime > 0f) ? (currentCastEndTime - currentCastDuration) : Time.time);
		while (Time.time < startTime + currentCastDuration)
		{
			float RemainingTime = startTime + currentCastDuration - Time.time;
			float ElapsedTime = currentCastDuration - RemainingTime;
			float ElapsedTimePct = ElapsedTime / currentCastDuration;
			if (m_TimeLabel != null)
			{
				m_TimeLabel.text = ((m_DisplayTime == DisplayTime.Elapsed) ? ElapsedTime.ToString(m_TimeFormat) : RemainingTime.ToString(m_TimeFormat));
			}
			SetFillAmount(ElapsedTimePct);
			yield return 0;
		}
		SetFillAmount(1f);
		if (m_TimeLabel != null)
		{
			m_TimeLabel.text = ((m_DisplayTime == DisplayTime.Elapsed) ? currentCastDuration.ToString(m_TimeFormat) : 0f.ToString(m_TimeFormat));
		}
		OnFinishedCasting();
		StartCoroutine("DelayHide");
	}

	private IEnumerator DelayHide()
	{
		yield return new WaitForSeconds(m_HideDelay);
		Hide();
	}

	public virtual void StartCasting(UISpellInfo spellInfo, float duration, float endTime)
	{
		if (m_IsCasting)
		{
			return;
		}
		StopCoroutine("AnimateCast");
		StopCoroutine("DelayHide");
		ApplyColorStage(m_NormalColors);
		SetFillAmount(0f);
		if (m_TitleLabel != null)
		{
			m_TitleLabel.text = spellInfo.Name;
		}
		if (m_FullTimeLabel != null)
		{
			m_FullTimeLabel.text = spellInfo.CastTime.ToString(m_FullTimeFormat);
		}
		if (m_UseSpellIcon && spellInfo.Icon != null)
		{
			if (m_IconImage != null)
			{
				m_IconImage.sprite = spellInfo.Icon;
			}
			if (m_IconFrame != null)
			{
				m_IconFrame.SetActive(value: true);
			}
		}
		currentCastDuration = duration;
		currentCastEndTime = endTime;
		m_IsCasting = true;
		Show();
		StartCoroutine("AnimateCast");
	}

	public virtual void Interrupt()
	{
		if (m_IsCasting)
		{
			StopCoroutine("AnimateCast");
			m_IsCasting = false;
			ApplyColorStage(m_OnInterruptColors);
			StartCoroutine("DelayHide");
		}
	}

	protected void OnFinishedCasting()
	{
		m_IsCasting = false;
		ApplyColorStage(m_OnFinishColors);
	}
}
