using System.Collections;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
	public ParticleSystem stableEffect;

	public ParticleSystem triggerEffect;

	public float repeatTime = 20f;

	private bool canRepear = true;

	private AudioClip clip;

	private AudioSource soundComponent;

	private void Start()
	{
		if (soundComponent == null)
		{
			soundComponent = GetComponent<AudioSource>();
			clip = soundComponent.clip;
		}
		stableEffect.Play();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (canRepear)
		{
			canRepear = false;
			stableEffect.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			triggerEffect.Play();
			soundComponent.PlayOneShot(clip);
			StartCoroutine(ExecuteAfterTime());
		}
	}

	private IEnumerator ExecuteAfterTime()
	{
		yield return new WaitForSeconds(repeatTime);
		canRepear = true;
		Start();
	}
}
