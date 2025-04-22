using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	public GameObject[] prefabs;

	public Camera sceneCamera;

	public string nameOfThePrefab;

	private int index;

	private void Start()
	{
		nameOfThePrefab = prefabs[index].name;
	}

	private void Update()
	{
		nameOfThePrefab = prefabs[index].name;
	}

	public void SpawnPrefab()
	{
		if (Physics.Raycast(sceneCamera.ScreenPointToRay(Input.mousePosition), out var hit))
		{
			Object.Instantiate(prefabs[index], hit.point, Quaternion.identity);
		}
	}

	public void ChangePrefabIntex(bool bo)
	{
		if (bo)
		{
			index++;
			if (index == prefabs.Length)
			{
				index = 0;
			}
		}
		else
		{
			index--;
			if (index == -1)
			{
				index = prefabs.Length - 1;
			}
		}
	}
}
