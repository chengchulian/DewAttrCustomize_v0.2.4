using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Bars/Bullet Bar")]
[RequireComponent(typeof(RectTransform))]
public class UIBulletBar : UIBehaviour, IUIProgressBar
{
	public enum BarType
	{
		Horizontal,
		Vertical,
		Radial
	}

	[SerializeField]
	private BarType m_BarType;

	[SerializeField]
	private bool m_FixedSize;

	[SerializeField]
	private Vector2 m_BulletSize = Vector2.zero;

	[SerializeField]
	private Sprite m_BulletSprite;

	[SerializeField]
	private Color m_BulletSpriteColor = Color.white;

	[SerializeField]
	private Sprite m_BulletSpriteActive;

	[SerializeField]
	private Color m_BulletSpriteActiveColor = Color.white;

	[SerializeField]
	private float m_SpriteRotation;

	[SerializeField]
	private Vector2 m_ActivePosition = Vector2.zero;

	[SerializeField]
	[Range(0f, 360f)]
	private float m_AngleMin;

	[SerializeField]
	[Range(0f, 360f)]
	private float m_AngleMax = 360f;

	[SerializeField]
	private int m_BulletCount = 10;

	[SerializeField]
	private float m_Distance = 100f;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_FillAmount = 1f;

	[SerializeField]
	private bool m_InvertFill = true;

	[SerializeField]
	[HideInInspector]
	private GameObject m_BulletsContainer;

	[SerializeField]
	[HideInInspector]
	private List<GameObject> m_FillBullets;

	public float fillAmount
	{
		get
		{
			return m_FillAmount;
		}
		set
		{
			m_FillAmount = Mathf.Clamp01(value);
			UpdateFill();
		}
	}

	public bool invertFill
	{
		get
		{
			return m_InvertFill;
		}
		set
		{
			m_InvertFill = value;
			UpdateFill();
		}
	}

	public RectTransform rectTransform => base.transform as RectTransform;

	protected override void Start()
	{
		base.Start();
		if (m_BulletSprite == null || m_BulletSpriteActive == null)
		{
			Debug.LogWarning("The Bullet Bar script on game object " + base.gameObject.name + " requires that both bullet sprites are assigned to work.");
			base.enabled = false;
		}
		else if (m_BulletsContainer == null)
		{
			ConstructBullets();
		}
	}

	public void UpdateFill()
	{
		if (!base.isActiveAndEnabled || m_FillBullets == null || m_FillBullets.Count == 0)
		{
			return;
		}
		GameObject[] list = m_FillBullets.ToArray();
		if (m_InvertFill)
		{
			Array.Reverse(list);
		}
		int index = 0;
		GameObject[] array = list;
		foreach (GameObject obj in array)
		{
			float currentPct = (float)index / (float)m_BulletCount;
			Image img = obj.GetComponent<Image>();
			if (img != null)
			{
				img.enabled = m_FillAmount > 0f && currentPct <= m_FillAmount;
			}
			index++;
		}
	}

	public void ConstructBullets()
	{
		if (m_BulletSprite == null || m_BulletSpriteActive == null || !base.isActiveAndEnabled)
		{
			return;
		}
		DestroyBullets();
		m_BulletsContainer = new GameObject("Bullets", typeof(RectTransform));
		m_BulletsContainer.transform.SetParent(base.transform);
		m_BulletsContainer.layer = base.gameObject.layer;
		RectTransform obj = m_BulletsContainer.transform as RectTransform;
		obj.localScale = new Vector3(1f, 1f, 1f);
		obj.sizeDelta = rectTransform.sizeDelta;
		obj.localPosition = Vector3.zero;
		obj.anchoredPosition = Vector2.zero;
		Vector2 pos = default(Vector2);
		Vector2 pos2 = default(Vector2);
		Vector2 pos3 = default(Vector2);
		for (int i = 0; i < m_BulletCount; i++)
		{
			float pct = (float)i / (float)m_BulletCount;
			GameObject obj2 = new GameObject("Bullet " + i, typeof(RectTransform));
			obj2.transform.SetParent(m_BulletsContainer.transform);
			obj2.layer = base.gameObject.layer;
			RectTransform rt = obj2.transform as RectTransform;
			rt.localScale = new Vector3(1f, 1f, 1f);
			rt.localPosition = Vector3.zero;
			Image img = obj2.AddComponent<Image>();
			img.sprite = m_BulletSprite;
			img.color = m_BulletSpriteColor;
			if (m_FixedSize)
			{
				rt.sizeDelta = m_BulletSize;
			}
			else
			{
				img.SetNativeSize();
			}
			if (m_BarType == BarType.Radial)
			{
				float ang = m_AngleMin + pct * (m_AngleMax - m_AngleMin);
				pos.x = 0f + m_Distance * Mathf.Sin(ang * (MathF.PI / 180f));
				pos.y = 0f + m_Distance * Mathf.Cos(ang * (MathF.PI / 180f));
				rt.anchoredPosition = pos;
				rt.Rotate(new Vector3(0f, 0f, (m_SpriteRotation + ang) * -1f));
			}
			else if (m_BarType == BarType.Horizontal)
			{
				rt.pivot = new Vector2(0.5f, 0.5f);
				rt.anchorMin = new Vector2(1f, 0.5f);
				rt.anchorMax = new Vector2(1f, 0.5f);
				float occupiedSpace = rt.sizeDelta.x * (float)m_BulletCount;
				float spacing = (rectTransform.rect.width - occupiedSpace) / (float)(m_BulletCount - 1);
				float offsetX = rt.sizeDelta.x * (float)i + spacing * (float)i;
				pos2.x = (offsetX + rt.sizeDelta.x / 2f) * -1f;
				pos2.y = 0f;
				rt.anchoredPosition = pos2;
				rt.Rotate(new Vector3(0f, 0f, m_SpriteRotation));
			}
			else if (m_BarType == BarType.Vertical)
			{
				rt.pivot = new Vector2(0.5f, 0.5f);
				rt.anchorMin = new Vector2(0.5f, 1f);
				rt.anchorMax = new Vector2(0.5f, 1f);
				float occupiedSpace2 = rt.sizeDelta.y * (float)m_BulletCount;
				float spacing2 = (rectTransform.rect.height - occupiedSpace2) / (float)(m_BulletCount - 1);
				float offsetY = rt.sizeDelta.y * (float)i + spacing2 * (float)i;
				pos3.x = 0f;
				pos3.y = (offsetY + rt.sizeDelta.y / 2f) * -1f;
				rt.anchoredPosition = pos3;
				rt.Rotate(new Vector3(0f, 0f, m_SpriteRotation));
			}
			GameObject objFill = new GameObject("Fill", typeof(RectTransform));
			objFill.transform.SetParent(obj2.transform);
			objFill.layer = base.gameObject.layer;
			RectTransform rtFill = objFill.transform as RectTransform;
			rtFill.localScale = new Vector3(1f, 1f, 1f);
			rtFill.localPosition = Vector3.zero;
			rtFill.anchoredPosition = m_ActivePosition;
			rtFill.rotation = rt.rotation;
			Image imgFill = objFill.AddComponent<Image>();
			imgFill.sprite = m_BulletSpriteActive;
			imgFill.color = m_BulletSpriteActiveColor;
			if (m_FixedSize)
			{
				rtFill.sizeDelta = m_BulletSize;
			}
			else
			{
				imgFill.SetNativeSize();
			}
			m_FillBullets.Add(objFill);
		}
		UpdateFill();
	}

	protected void DestroyBullets()
	{
		m_FillBullets.Clear();
		GameObject go = m_BulletsContainer;
		if (!Application.isEditor)
		{
			global::UnityEngine.Object.Destroy(go);
		}
		m_BulletsContainer = null;
	}
}
