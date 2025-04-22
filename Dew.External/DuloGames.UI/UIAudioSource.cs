using UnityEngine;

namespace DuloGames.UI;

[AddComponentMenu("UI/Audio/Audio Source")]
[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class UIAudioSource : MonoBehaviour
{
	private static UIAudioSource m_Instance;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_Volume = 1f;

	private AudioSource m_AudioSource;

	public static UIAudioSource Instance => m_Instance;

	public float volume
	{
		get
		{
			return m_Volume;
		}
		set
		{
			m_Volume = value;
		}
	}

	protected void Awake()
	{
		if (m_Instance != null)
		{
			Debug.LogWarning("You have more than one UIAudioSource in the scene, please make sure you have only one.");
			return;
		}
		m_Instance = this;
		m_AudioSource = base.gameObject.GetComponent<AudioSource>();
		m_AudioSource.playOnAwake = false;
	}

	public void PlayAudio(AudioClip clip)
	{
		m_AudioSource.PlayOneShot(clip, m_Volume);
	}

	public void PlayAudio(AudioClip clip, float volume)
	{
		m_AudioSource.PlayOneShot(clip, m_Volume * volume);
	}
}
