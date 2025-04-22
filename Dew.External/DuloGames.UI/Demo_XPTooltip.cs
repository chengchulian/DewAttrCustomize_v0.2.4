using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Demo_XPTooltip : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private GameObject m_TooltipObject;

	[SerializeField]
	private UIProgressBar m_ProgressBar;

	[SerializeField]
	private Text m_PercentText;

	[SerializeField]
	private float m_OffsetY;

	[SerializeField]
	[Tooltip("How long of a delay to expect before showing the tooltip.")]
	[Range(0f, 10f)]
	private float m_Delay = 1f;

	private bool m_IsTooltipShown;

	protected override void Awake()
	{
		base.Awake();
		if (m_TooltipObject != null)
		{
			RectTransform obj = m_TooltipObject.transform as RectTransform;
			obj.anchorMin = new Vector2(0f, 1f);
			obj.anchorMax = new Vector2(0f, 1f);
			obj.pivot = new Vector2(0.5f, 0f);
			m_TooltipObject.SetActive(value: false);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (m_ProgressBar != null)
		{
			m_ProgressBar.onChange.AddListener(OnProgressChange);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (m_ProgressBar != null)
		{
			m_ProgressBar.onChange.RemoveListener(OnProgressChange);
		}
	}

	private void OnProgressChange(float value)
	{
		UpdatePosition();
	}

	public virtual void OnTooltip(bool show)
	{
		if (!(m_TooltipObject == null))
		{
			if (show)
			{
				UpdatePosition();
				m_TooltipObject.SetActive(value: true);
			}
			else
			{
				m_TooltipObject.SetActive(value: false);
			}
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (base.enabled && IsActive())
		{
			if (m_Delay > 0f)
			{
				StartCoroutine("DelayedShow");
			}
			else
			{
				InternalShowTooltip();
			}
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		InternalHideTooltip();
	}

	protected void InternalShowTooltip()
	{
		if (!m_IsTooltipShown)
		{
			m_IsTooltipShown = true;
			OnTooltip(show: true);
		}
	}

	protected void InternalHideTooltip()
	{
		StopCoroutine("DelayedShow");
		if (m_IsTooltipShown)
		{
			m_IsTooltipShown = false;
			OnTooltip(show: false);
		}
	}

	protected IEnumerator DelayedShow()
	{
		yield return new WaitForSeconds(m_Delay);
		InternalShowTooltip();
	}

	public void UpdatePosition()
	{
		if (!(m_ProgressBar == null) && !(m_TooltipObject == null))
		{
			RectTransform obj = m_TooltipObject.transform as RectTransform;
			RectTransform fillRect = ((m_ProgressBar.type == UIProgressBar.Type.Filled) ? (m_ProgressBar.targetImage.transform as RectTransform) : (m_ProgressBar.targetTransform.parent as RectTransform));
			Transform parent = obj.parent;
			obj.SetParent(fillRect, worldPositionStays: true);
			obj.anchoredPosition = new Vector2(fillRect.rect.width * m_ProgressBar.fillAmount, m_OffsetY);
			obj.SetParent(parent, worldPositionStays: true);
			if (m_PercentText != null)
			{
				m_PercentText.text = (m_ProgressBar.fillAmount * 100f).ToString("0") + "%";
			}
		}
	}
}
