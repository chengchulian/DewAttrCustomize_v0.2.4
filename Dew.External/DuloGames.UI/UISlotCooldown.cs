using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Slot Cooldown", 28)]
public class UISlotCooldown : MonoBehaviour
{
	public class CooldownInfo
	{
		public float duration;

		public float startTime;

		public float endTime;

		public CooldownInfo(float duration, float startTime, float endTime)
		{
			this.duration = duration;
			this.startTime = startTime;
			this.endTime = endTime;
		}
	}

	private static Dictionary<int, CooldownInfo> spellCooldowns = new Dictionary<int, CooldownInfo>();

	[SerializeField]
	private UISlotBase m_TargetSlot;

	[SerializeField]
	private Image m_TargetGraphic;

	[SerializeField]
	private Text m_TargetText;

	[SerializeField]
	private Image m_FinishGraphic;

	[SerializeField]
	private float m_FinishOffsetY;

	[SerializeField]
	private float m_FinishFadingPct = 25f;

	private IUISlotHasCooldown m_CooldownSlot;

	private bool m_IsOnCooldown;

	private int m_CurrentSpellId;

	public bool IsOnCooldown => m_IsOnCooldown;

	protected void Awake()
	{
		if (m_TargetSlot != null && m_TargetSlot is IUISlotHasCooldown)
		{
			m_CooldownSlot = m_TargetSlot as IUISlotHasCooldown;
			m_CooldownSlot.SetCooldownComponent(this);
		}
		else
		{
			Debug.LogWarning("The slot cooldown script cannot operate without a target slot with a IUISlotHasCooldown interface, disabling script.");
			base.enabled = false;
		}
	}

	protected void Start()
	{
		if (m_TargetGraphic != null)
		{
			m_TargetGraphic.enabled = false;
		}
		if (m_TargetText != null)
		{
			m_TargetText.enabled = false;
		}
		if (m_FinishGraphic != null)
		{
			m_FinishGraphic.enabled = false;
			m_FinishGraphic.rectTransform.anchorMin = new Vector2(m_FinishGraphic.rectTransform.anchorMin.x, 1f);
			m_FinishGraphic.rectTransform.anchorMax = new Vector2(m_FinishGraphic.rectTransform.anchorMax.x, 1f);
		}
	}

	protected void OnEnable()
	{
		CheckForActiveCooldown();
	}

	protected void OnDisable()
	{
		InterruptCooldown();
	}

	public void OnAssignSpell()
	{
		CheckForActiveCooldown();
	}

	public void OnUnassignSpell()
	{
		InterruptCooldown();
	}

	public void CheckForActiveCooldown()
	{
		if (m_CooldownSlot == null)
		{
			return;
		}
		UISpellInfo spellInfo = m_CooldownSlot.GetSpellInfo();
		if (spellInfo != null && spellCooldowns.ContainsKey(spellInfo.ID))
		{
			if (spellCooldowns[spellInfo.ID].endTime > Time.time)
			{
				m_IsOnCooldown = true;
				ResumeCooldown(spellInfo.ID);
			}
			else
			{
				spellCooldowns.Remove(spellInfo.ID);
			}
		}
	}

	public void StartCooldown(int spellId, float duration)
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy || m_TargetGraphic == null)
		{
			return;
		}
		m_CurrentSpellId = spellId;
		if (!m_TargetGraphic.enabled)
		{
			m_TargetGraphic.enabled = true;
		}
		m_TargetGraphic.fillAmount = 1f;
		if (m_TargetText != null)
		{
			if (!m_TargetText.enabled)
			{
				m_TargetText.enabled = true;
			}
			m_TargetText.text = duration.ToString("0");
		}
		if (m_FinishGraphic != null)
		{
			m_FinishGraphic.canvasRenderer.SetAlpha(0f);
			m_FinishGraphic.enabled = true;
			m_FinishGraphic.rectTransform.anchoredPosition = new Vector2(m_FinishGraphic.rectTransform.anchoredPosition.x, m_FinishOffsetY);
		}
		m_IsOnCooldown = true;
		CooldownInfo cooldownInfo = new CooldownInfo(duration, Time.time, Time.time + duration);
		if (!spellCooldowns.ContainsKey(spellId))
		{
			spellCooldowns.Add(spellId, cooldownInfo);
		}
		StartCoroutine("_StartCooldown", cooldownInfo);
	}

	public void ResumeCooldown(int spellId)
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy || m_TargetGraphic == null || !spellCooldowns.ContainsKey(spellId))
		{
			return;
		}
		CooldownInfo cooldownInfo = spellCooldowns[spellId];
		float remainingTime = cooldownInfo.endTime - Time.time;
		float remainingTimePct = remainingTime / cooldownInfo.duration;
		m_CurrentSpellId = spellId;
		if (!m_TargetGraphic.enabled)
		{
			m_TargetGraphic.enabled = true;
		}
		m_TargetGraphic.fillAmount = remainingTime / cooldownInfo.duration;
		if (m_TargetText != null)
		{
			if (!m_TargetText.enabled)
			{
				m_TargetText.enabled = true;
			}
			m_TargetText.text = remainingTime.ToString("0");
		}
		if (m_FinishGraphic != null)
		{
			m_FinishGraphic.enabled = true;
			UpdateFinishPosition(remainingTimePct);
		}
		StartCoroutine("_StartCooldown", cooldownInfo);
	}

	public void InterruptCooldown()
	{
		StopCoroutine("_StartCooldown");
		OnCooldownFinished();
	}

	private IEnumerator _StartCooldown(CooldownInfo cooldownInfo)
	{
		while (Time.time < cooldownInfo.startTime + cooldownInfo.duration)
		{
			float RemainingTime = cooldownInfo.startTime + cooldownInfo.duration - Time.time;
			float RemainingTimePct = RemainingTime / cooldownInfo.duration;
			if (m_TargetGraphic != null)
			{
				m_TargetGraphic.fillAmount = RemainingTimePct;
			}
			if (m_TargetText != null)
			{
				m_TargetText.text = RemainingTime.ToString("0");
			}
			UpdateFinishPosition(RemainingTimePct);
			yield return 0;
		}
		OnCooldownCompleted();
	}

	private void OnCooldownCompleted()
	{
		if (spellCooldowns.ContainsKey(m_CurrentSpellId))
		{
			spellCooldowns.Remove(m_CurrentSpellId);
		}
		OnCooldownFinished();
	}

	private void OnCooldownFinished()
	{
		m_IsOnCooldown = false;
		m_CurrentSpellId = 0;
		if (m_TargetGraphic != null)
		{
			m_TargetGraphic.enabled = false;
		}
		if (m_TargetText != null)
		{
			m_TargetText.enabled = false;
		}
		if (m_FinishGraphic != null)
		{
			m_FinishGraphic.enabled = false;
		}
	}

	protected void UpdateFinishPosition(float RemainingTimePct)
	{
		if (m_FinishGraphic != null && m_TargetGraphic != null)
		{
			float newY = 0f - m_TargetGraphic.rectTransform.rect.height + m_TargetGraphic.rectTransform.rect.height * RemainingTimePct;
			newY += m_FinishOffsetY;
			m_FinishGraphic.rectTransform.anchoredPosition = new Vector2(m_FinishGraphic.rectTransform.anchoredPosition.x, newY);
			float fadingPct = m_FinishFadingPct / 100f;
			if (RemainingTimePct <= fadingPct)
			{
				m_FinishGraphic.canvasRenderer.SetAlpha(RemainingTimePct / fadingPct);
			}
			else if (RemainingTimePct >= 1f - fadingPct)
			{
				m_FinishGraphic.canvasRenderer.SetAlpha(1f - (RemainingTimePct - (1f - fadingPct)) / fadingPct);
			}
			else if (RemainingTimePct > fadingPct && RemainingTimePct < 1f - fadingPct)
			{
				m_FinishGraphic.canvasRenderer.SetAlpha(1f);
			}
		}
	}
}
