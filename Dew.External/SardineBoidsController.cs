using UnityEngine;

public class SardineBoidsController : MonoBehaviour
{
	public GameObject sardinePrefab;

	private GameObject[] sardines;

	public int maxXNum = 2;

	public int maxYNum = 3;

	public int maxZNum = 4;

	public Vector3 meanPos;

	public int sardineCount;

	public float rotateSpeed = 1f;

	public GameObject meanDummy;

	private void Start()
	{
		sardineCount = maxZNum * maxYNum * maxXNum;
		sardines = new GameObject[sardineCount];
		for (int k = 0; k < maxZNum; k++)
		{
			for (int j = 0; j < maxYNum; j++)
			{
				for (int i = 0; i < maxXNum; i++)
				{
					int sNum = k * maxXNum * maxYNum + j * maxXNum + i;
					sardines[sNum] = Object.Instantiate(sardinePrefab, base.transform.position + Vector3.right * i + Vector3.up * j + Vector3.forward * k, base.transform.rotation);
					Collider[] componentsInChildren = sardines[k * maxXNum * maxYNum + j * maxXNum + i].GetComponentsInChildren<Collider>();
					for (int l = 0; l < componentsInChildren.Length; l++)
					{
						componentsInChildren[l].name = "SardineCol";
					}
				}
			}
		}
	}

	private void Update()
	{
		meanPos = Vector3.zero;
		for (int i = 0; i < sardineCount; i++)
		{
			meanPos += sardines[i].transform.position;
		}
		meanPos /= (float)sardineCount;
		meanDummy.transform.position = meanPos;
		for (int j = 0; j < sardineCount; j++)
		{
			Vector3 targetRelPos = meanPos - sardines[j].transform.position;
			targetRelPos.Normalize();
			float dottigawa = Vector3.Dot(targetRelPos, sardines[j].transform.right);
			Rigidbody component = sardines[j].GetComponent<Rigidbody>();
			component.AddTorque(sardines[j].transform.up * dottigawa * rotateSpeed);
			sardines[j].GetComponent<Animator>().SetFloat("Turn", dottigawa * rotateSpeed);
			dottigawa = Vector3.Dot(targetRelPos, sardines[j].transform.up);
			component.AddTorque(-sardines[j].transform.right * dottigawa * rotateSpeed);
			sardines[j].GetComponent<Animator>().SetFloat("Up", dottigawa * rotateSpeed);
		}
	}
}
