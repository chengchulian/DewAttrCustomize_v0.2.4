using UnityEngine;
using UnityEngine.UI;

public class SpawnProjectiles : MonoBehaviour
{
	public GameObject firePoint;

	public GameObject[] Effects;

	public RotateToMouse rotateToMouse;

	private int selectedPrefab;

	private GameObject effectToSpawn;

	private float timeToFire;

	private Text prefabName;

	private void Start()
	{
		effectToSpawn = Effects[0];
		prefabName = GameObject.Find("PrefabName").GetComponent<Text>();
	}

	private void Update()
	{
		if (Input.GetMouseButton(0) && Time.time >= timeToFire)
		{
			timeToFire = Time.time + 1f / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
			SpawnEffects();
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			selectedPrefab++;
			if (selectedPrefab >= Effects.Length)
			{
				selectedPrefab = 0;
			}
			effectToSpawn = Effects[selectedPrefab];
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			selectedPrefab--;
			if (selectedPrefab < 0)
			{
				selectedPrefab = Effects.Length - 1;
			}
			effectToSpawn = Effects[selectedPrefab];
		}
		if (Effects.Length != 0 && selectedPrefab >= 0 && selectedPrefab < Effects.Length)
		{
			prefabName.text = Effects[selectedPrefab].name;
		}
	}

	private void SpawnEffects()
	{
		if (firePoint != null)
		{
			GameObject Effects = Object.Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
			if (rotateToMouse != null)
			{
				Effects.transform.localRotation = rotateToMouse.GetRotation();
			}
		}
		else
		{
			Debug.Log("No Fire Point");
		}
	}
}
