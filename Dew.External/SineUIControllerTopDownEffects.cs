using UnityEngine;
using UnityEngine.UI;

public class SineUIControllerTopDownEffects : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public PrefabSpawner prefabSpawnerObject;

	public Text nameInUI;

	private string nameOfThePrafab;

	private void Start()
	{
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
		if (Input.GetMouseButtonDown(1))
		{
			prefabSpawnerObject.SpawnPrefab();
		}
		nameOfThePrafab = prefabSpawnerObject.nameOfThePrefab;
		nameInUI.text = "Spawn - " + nameOfThePrafab;
	}

	public void ChangeEffect(bool bo)
	{
		prefabSpawnerObject.ChangePrefabIntex(bo);
		nameOfThePrafab = prefabSpawnerObject.nameOfThePrefab;
	}
}
