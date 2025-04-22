using System.Collections.Generic;
using UnityEngine;

namespace viperOSK;

public class OSK_KeySounds : MonoBehaviour
{
	public List<AudioClip> keySounds = new List<AudioClip>();

	public AudioClip selectKeySound;

	private void Start()
	{
	}

	public void PlaySound(int k)
	{
		if (keySounds.Count > 0 && k >= 0 && k < keySounds.Count)
		{
			GetComponent<AudioSource>().PlayOneShot(keySounds[k]);
		}
	}

	public void PlaySelectKeySound()
	{
		if (selectKeySound != null)
		{
			GetComponent<AudioSource>().PlayOneShot(selectKeySound);
		}
	}

	private void Update()
	{
	}
}
