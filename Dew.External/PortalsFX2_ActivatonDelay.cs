using System.Collections;
using UnityEngine;

public class PortalsFX2_ActivatonDelay : MonoBehaviour
{
	public GameObject objectToActivate;

	public float Delay;

	private void Start()
	{
		StartCoroutine(ActivationRoutine());
	}

	private IEnumerator ActivationRoutine()
	{
		yield return new WaitForSeconds(Delay);
		objectToActivate.SetActive(value: true);
	}
}
