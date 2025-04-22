using System;
using System.Collections;
using UnityEngine;

public class HS_CameraShaker : MonoBehaviour
{
	public Transform cameraObject;

	public float amplitude;

	public float frequency;

	public float duration;

	public float timeRemaining;

	private Vector3 noiseOffset;

	private Vector3 noise;

	private AnimationCurve smoothCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, MathF.PI * 4f), new Keyframe(0.2f, 1f), new Keyframe(1f, 0f));

	private void Start()
	{
		float rand = 32f;
		noiseOffset.x = global::UnityEngine.Random.Range(0f, rand);
		noiseOffset.y = global::UnityEngine.Random.Range(0f, rand);
		noiseOffset.z = global::UnityEngine.Random.Range(0f, rand);
	}

	public IEnumerator Shake(float amp, float freq, float dur, float wait)
	{
		yield return new WaitForSeconds(wait);
		float rand = 32f;
		noiseOffset.x = global::UnityEngine.Random.Range(0f, rand);
		noiseOffset.y = global::UnityEngine.Random.Range(0f, rand);
		noiseOffset.z = global::UnityEngine.Random.Range(0f, rand);
		amplitude = amp;
		frequency = freq;
		duration = dur;
		timeRemaining += dur;
		if (timeRemaining > dur)
		{
			timeRemaining = dur;
		}
	}

	private void Update()
	{
		if (!(timeRemaining <= 0f))
		{
			float deltaTime = Time.deltaTime;
			timeRemaining -= deltaTime;
			float noiseOffsetDelta = deltaTime * frequency;
			noiseOffset.x += noiseOffsetDelta;
			noiseOffset.y += noiseOffsetDelta;
			noiseOffset.z += noiseOffsetDelta;
			noise.x = Mathf.PerlinNoise(noiseOffset.x, 0f);
			noise.y = Mathf.PerlinNoise(noiseOffset.y, 1f);
			noise.z = Mathf.PerlinNoise(noiseOffset.z, 2f);
			noise -= Vector3.one * 0.5f;
			noise *= amplitude;
			float agePercent = 1f - timeRemaining / duration;
			noise *= smoothCurve.Evaluate(agePercent);
		}
	}

	private void LateUpdate()
	{
		if (!(timeRemaining <= 0f))
		{
			Vector3 positionOffset = Vector3.zero;
			Vector3 rotationOffset = Vector3.zero;
			positionOffset += noise;
			rotationOffset += noise;
			cameraObject.transform.localPosition = positionOffset;
			cameraObject.transform.localEulerAngles = rotationOffset;
		}
	}
}
