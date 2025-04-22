using System.Collections.Generic;
using UnityEngine;

public class SineUIControllerGalaxyMaterials : MonoBehaviour
{
	public Transform prefabHolder;

	public CanvasGroup canvasGroup;

	private Transform[] prefabs;

	private List<Transform> lt;

	private int activeNumber;

	private void Start()
	{
		lt = new List<Transform>();
		prefabs = prefabHolder.GetComponentsInChildren<Transform>(includeInactive: true);
		Transform[] array = prefabs;
		foreach (Transform tran in array)
		{
			if (tran.parent == prefabHolder)
			{
				lt.Add(tran);
			}
		}
		prefabs = lt.ToArray();
		EnableActive();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			canvasGroup.alpha = 1f - canvasGroup.alpha;
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			ChangeEffect(bo: true);
		}
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			ChangeEffect(bo: false);
		}
	}

	public void EnableActive()
	{
		for (int i = 0; i < prefabs.Length; i++)
		{
			if (i == activeNumber)
			{
				prefabs[i].gameObject.SetActive(value: true);
			}
			else
			{
				prefabs[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void ChangeEffect(bool bo)
	{
		if (bo)
		{
			activeNumber++;
			if (activeNumber == prefabs.Length)
			{
				activeNumber = 0;
			}
		}
		else
		{
			activeNumber--;
			if (activeNumber == -1)
			{
				activeNumber = prefabs.Length - 1;
			}
		}
		EnableActive();
	}
}
