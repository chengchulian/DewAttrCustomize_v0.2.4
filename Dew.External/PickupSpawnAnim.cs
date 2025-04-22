using System;
using System.Collections;
using UnityEngine;

public class PickupSpawnAnim : MonoBehaviour
{
	[SerializeField]
	private float jumpDuration = 1f;

	[SerializeField]
	private float jumpDistance = 3f;

	[SerializeField]
	private float jumpHeight = 2f;

	[SerializeField]
	private float spinRate = 20f;

	[SerializeField]
	private AnimationCurve jumpCurve;

	private bool jumpTrigger = true;

	private void Update()
	{
		if (jumpTrigger)
		{
			StartCoroutine(Animate());
		}
		else
		{
			base.transform.Rotate(0f, spinRate * Time.deltaTime, 0f);
		}
	}

	private IEnumerator Animate()
	{
		float t = 0f;
		Vector3 origin = base.transform.position;
		Vector3 jumpTo = base.transform.forward * jumpDistance + origin;
		jumpTrigger = false;
		while (t < jumpDuration)
		{
			Vector3 heightJump = Vector3.up * Mathf.Sin(t / jumpDuration * MathF.PI) * jumpHeight;
			float v = jumpCurve.Evaluate(t / jumpDuration);
			base.transform.position = Vector3.Lerp(origin, jumpTo, v) + heightJump;
			t += Time.deltaTime;
			yield return null;
		}
	}
}
