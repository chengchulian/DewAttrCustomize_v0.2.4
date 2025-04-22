using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Bars/Step Bar")]
public class UIStepBar : MonoBehaviour
{
	[Serializable]
	public struct StepFillInfo
	{
		public int index;

		public float amount;
	}

	[SerializeField]
	private List<GameObject> m_StepsGameObjects = new List<GameObject>();

	[SerializeField]
	private List<StepFillInfo> m_OverrideFillList = new List<StepFillInfo>();

	[SerializeField]
	private GameObject m_StepsGridGameObject;

	[SerializeField]
	private RectTransform m_StepsGridRect;

	[SerializeField]
	private GridLayoutGroup m_StepsGrid;

	[SerializeField]
	private int m_CurrentStep;

	[SerializeField]
	private int m_StepsCount = 1;

	[SerializeField]
	private RectOffset m_StepsGridPadding = new RectOffset();

	[SerializeField]
	private Sprite m_SeparatorSprite;

	[SerializeField]
	private Sprite m_SeparatorSpriteActive;

	[SerializeField]
	private Color m_SeparatorSpriteColor = Color.white;

	[SerializeField]
	private bool m_SeparatorAutoSize = true;

	[SerializeField]
	private Vector2 m_SeparatorSize = Vector2.zero;

	[SerializeField]
	private Image m_FillImage;

	[SerializeField]
	private RectTransform m_BubbleRect;

	[SerializeField]
	private Vector2 m_BubbleOffset = Vector2.zero;

	[SerializeField]
	private Text m_BubbleText;

	public int step
	{
		get
		{
			return m_CurrentStep;
		}
		set
		{
			GoToStep(value);
		}
	}

	public virtual bool IsActive()
	{
		if (base.enabled)
		{
			return base.gameObject.activeInHierarchy;
		}
		return false;
	}

	protected virtual void Start()
	{
		UpdateBubble();
	}

	public List<StepFillInfo> GetOverrideFillList()
	{
		return m_OverrideFillList;
	}

	public void SetOverrideFillList(List<StepFillInfo> list)
	{
		m_OverrideFillList = list;
	}

	public void ValidateOverrideFillList()
	{
		List<StepFillInfo> list = new List<StepFillInfo>();
		StepFillInfo[] array = m_OverrideFillList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			StepFillInfo info = array[i];
			if (info.index > 1 && info.index <= m_StepsCount && info.amount > 0f)
			{
				list.Add(info);
			}
		}
		m_OverrideFillList = list;
	}

	protected virtual void OnRectTransformDimensionsChange()
	{
		if (IsActive())
		{
			UpdateGridProperties();
		}
	}

	public void GoToStep(int step)
	{
		if (step < 0)
		{
			step = 0;
		}
		if (step > m_StepsCount)
		{
			step = m_StepsCount + 1;
		}
		m_CurrentStep = step;
		UpdateStepsProperties();
		UpdateFillImage();
		UpdateBubble();
	}

	public void UpdateFillImage()
	{
		if (!(m_FillImage == null))
		{
			int overrideIndex = m_OverrideFillList.FindIndex((StepFillInfo x) => x.index == m_CurrentStep);
			m_FillImage.fillAmount = ((overrideIndex >= 0) ? m_OverrideFillList[overrideIndex].amount : GetStepFillAmount(m_CurrentStep));
		}
	}

	public void UpdateBubble()
	{
		if (m_BubbleRect == null)
		{
			return;
		}
		if (m_CurrentStep > 0 && m_CurrentStep <= m_StepsCount)
		{
			if (!m_BubbleRect.gameObject.activeSelf)
			{
				m_BubbleRect.gameObject.SetActive(value: true);
			}
			GameObject stepObject = m_StepsGameObjects[m_CurrentStep];
			if (stepObject != null)
			{
				RectTransform stepRect = stepObject.transform as RectTransform;
				if (stepRect.anchoredPosition.x != 0f)
				{
					m_BubbleRect.anchoredPosition = new Vector2(m_BubbleOffset.x + (stepRect.anchoredPosition.x + stepRect.rect.width / 2f), m_BubbleOffset.y);
				}
			}
			if (m_BubbleText != null)
			{
				m_BubbleText.text = m_CurrentStep.ToString();
			}
		}
		else if (m_BubbleRect.gameObject.activeSelf)
		{
			m_BubbleRect.gameObject.SetActive(value: false);
		}
	}

	public float GetStepFillAmount(int step)
	{
		return 1f / (float)(m_StepsCount + 1) * (float)step;
	}

	protected void CreateStepsGrid()
	{
		if (!(m_StepsGridGameObject != null))
		{
			m_StepsGridGameObject = new GameObject("Steps Grid", typeof(RectTransform), typeof(GridLayoutGroup));
			m_StepsGridGameObject.layer = base.gameObject.layer;
			m_StepsGridGameObject.transform.SetParent(base.transform, worldPositionStays: false);
			m_StepsGridGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			m_StepsGridGameObject.transform.localPosition = Vector3.zero;
			m_StepsGridGameObject.transform.SetAsLastSibling();
			m_StepsGridRect = m_StepsGridGameObject.GetComponent<RectTransform>();
			m_StepsGridRect.sizeDelta = new Vector2(0f, 0f);
			m_StepsGridRect.anchorMin = new Vector2(0f, 0f);
			m_StepsGridRect.anchorMax = new Vector2(1f, 1f);
			m_StepsGridRect.pivot = new Vector2(0f, 1f);
			m_StepsGridRect.anchoredPosition = new Vector2(0f, 0f);
			m_StepsGrid = m_StepsGridGameObject.GetComponent<GridLayoutGroup>();
			if (m_BubbleRect != null)
			{
				m_BubbleRect.SetAsLastSibling();
			}
			m_StepsGameObjects.Clear();
		}
	}

	public void UpdateGridProperties()
	{
		if (!(m_StepsGrid == null))
		{
			int seps = m_StepsCount + 2;
			if (!m_StepsGrid.padding.Equals(m_StepsGridPadding))
			{
				m_StepsGrid.padding = m_StepsGridPadding;
			}
			if (m_SeparatorAutoSize && m_SeparatorSprite != null)
			{
				m_SeparatorSize = new Vector2(m_SeparatorSprite.rect.width, m_SeparatorSprite.rect.height);
			}
			if (!m_StepsGrid.cellSize.Equals(m_SeparatorSize))
			{
				m_StepsGrid.cellSize = m_SeparatorSize;
			}
			float spacingX = Mathf.Floor((m_StepsGridRect.rect.width - (float)m_StepsGridPadding.horizontal - (float)seps * m_SeparatorSize.x) / (float)(seps - 1) / 2f) * 2f;
			if (m_StepsGrid.spacing.x != spacingX)
			{
				m_StepsGrid.spacing = new Vector2(spacingX, 0f);
			}
		}
	}

	public void RebuildSteps()
	{
		if (m_StepsGridGameObject == null || m_StepsGameObjects.Count == m_StepsCount + 2)
		{
			return;
		}
		DestroySteps();
		int seps = m_StepsCount + 2;
		for (int i = 0; i < seps; i++)
		{
			GameObject step = new GameObject("Step " + i, typeof(RectTransform));
			step.layer = base.gameObject.layer;
			step.transform.localScale = new Vector3(1f, 1f, 1f);
			step.transform.localPosition = Vector3.zero;
			step.transform.SetParent(m_StepsGridGameObject.transform, worldPositionStays: false);
			if (i > 0 && i < seps - 1)
			{
				step.AddComponent<Image>();
			}
			m_StepsGameObjects.Add(step);
		}
	}

	protected void UpdateStepsProperties()
	{
		foreach (GameObject stepObject in m_StepsGameObjects)
		{
			bool active = m_StepsGameObjects.IndexOf(stepObject) + 1 <= m_CurrentStep;
			Image image = stepObject.GetComponent<Image>();
			if (image != null)
			{
				image.sprite = m_SeparatorSprite;
				image.overrideSprite = (active ? m_SeparatorSpriteActive : null);
				image.color = m_SeparatorSpriteColor;
				image.rectTransform.pivot = new Vector2(0f, 1f);
			}
		}
	}

	protected void DestroySteps()
	{
		if (Application.isPlaying)
		{
			foreach (GameObject stepsGameObject in m_StepsGameObjects)
			{
				global::UnityEngine.Object.Destroy(stepsGameObject);
			}
		}
		m_StepsGameObjects.Clear();
	}
}
