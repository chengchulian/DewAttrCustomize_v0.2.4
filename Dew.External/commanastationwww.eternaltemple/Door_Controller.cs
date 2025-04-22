using System.Collections;
using UnityEngine;

namespace commanastationwww.eternaltemple;

public class Door_Controller : MonoBehaviour
{
	public bool stayOpen = true;

	public bool locked;

	public float openingSpeed = 5f;

	private Transform[] allTransform;

	private Transform[] childrenTransform;

	private void Start()
	{
		allTransform = GetComponentsInChildren<Transform>();
		childrenTransform = new Transform[allTransform.Length - 1];
		for (int i = 1; i < allTransform.Length; i++)
		{
			childrenTransform[i - 1] = allTransform[i];
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Open();
	}

	private void OnTriggerExit(Collider other)
	{
		if (!stayOpen)
		{
			Close();
		}
	}

	private IEnumerator openInterpolation()
	{
		while (childrenTransform[0].localPosition.y > -2.6f)
		{
			Transform[] array = childrenTransform;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Translate(Vector3.down * openingSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}

	private IEnumerator closeInterpolation()
	{
		while (childrenTransform[0].localPosition.y < 0f)
		{
			Transform[] array = childrenTransform;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Translate(Vector3.up * openingSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}

	public void Open()
	{
		if (!locked)
		{
			StopAllCoroutines();
			StartCoroutine(openInterpolation());
		}
	}

	public void Close()
	{
		StopAllCoroutines();
		StartCoroutine(closeInterpolation());
	}

	private void Update()
	{
	}
}
