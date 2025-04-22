using UnityEngine;

namespace viperOSK_Examples;

public class viperOSK_Rotator : MonoBehaviour
{
	private float nextRotateTime;

	public float rotationFrequency;

	public bool randomizeDirection;

	private Quaternion current;

	private Quaternion target;

	private float t = -1f;

	public void ShowHide(bool show)
	{
		base.gameObject.SetActive(show);
	}

	private void Start()
	{
		nextRotateTime = Time.time + Random.Range(rotationFrequency - 0.2f, rotationFrequency + 0.2f);
	}

	private void Update()
	{
		if (Time.time > nextRotateTime)
		{
			current = base.transform.rotation;
			float r = Random.Range(0f, 100f);
			r = ((!(r >= 50f)) ? (-90f) : 90f);
			target = Quaternion.Euler(0f, 0f, r);
			t = 0f;
			nextRotateTime = Time.time + Random.Range(rotationFrequency - 0.2f, rotationFrequency + 0.2f);
		}
		if (t >= 0f)
		{
			base.transform.rotation = Quaternion.Slerp(current, target, t);
			t += Time.deltaTime;
			if (t >= 1f)
			{
				t = -1f;
			}
		}
	}
}
