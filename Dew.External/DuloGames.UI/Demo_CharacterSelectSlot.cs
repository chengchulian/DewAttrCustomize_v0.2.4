using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

public class Demo_CharacterSelectSlot : MonoBehaviour
{
	[SerializeField]
	private Light m_Light;

	private Demo_CharacterInfo m_Info;

	private int m_Index;

	private float m_Intensity;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_TweenRunner;

	public Demo_CharacterInfo info
	{
		get
		{
			return m_Info;
		}
		set
		{
			m_Info = value;
		}
	}

	public int index
	{
		get
		{
			return m_Index;
		}
		set
		{
			m_Index = value;
		}
	}

	protected Demo_CharacterSelectSlot()
	{
		if (m_TweenRunner == null)
		{
			m_TweenRunner = new TweenRunner<FloatTween>();
		}
		m_TweenRunner.Init(this);
	}

	protected void Awake()
	{
		if (m_Light != null)
		{
			m_Light.enabled = false;
			m_Intensity = m_Light.intensity;
			m_Light.intensity = 0f;
		}
	}

	public void OnSelected()
	{
		if (m_Light != null)
		{
			m_Light.enabled = true;
			StartIntensityTween(m_Intensity, 0.3f);
		}
	}

	public void OnDeselected()
	{
		if (m_Light != null)
		{
			m_Light.enabled = false;
			m_Light.intensity = 0f;
		}
	}

	private void OnMouseDown()
	{
		if (m_Info != null && !EventSystem.current.IsPointerOverGameObject() && Demo_CharacterSelectMgr.instance != null)
		{
			Demo_CharacterSelectMgr.instance.SelectCharacter(this);
		}
	}

	private void StartIntensityTween(float target, float duration)
	{
		if (!(m_Light == null))
		{
			if (!Application.isPlaying || duration == 0f)
			{
				m_Light.intensity = target;
				return;
			}
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = m_Light.intensity;
			floatTween.targetFloat = target;
			FloatTween colorTween = floatTween;
			colorTween.AddOnChangedCallback(SetIntensity);
			colorTween.ignoreTimeScale = true;
			m_TweenRunner.StartTween(colorTween);
		}
	}

	private void SetIntensity(float intensity)
	{
		if (!(m_Light == null))
		{
			m_Light.intensity = intensity;
		}
	}
}
