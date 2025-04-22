using UnityEngine;
using UnityEngine.UI;

public class VariousEffectsScene : MonoBehaviour
{
	public Transform[] m_effects;

	public GameObject scaleform;

	public GameObject[] m_destroyObjects = new GameObject[30];

	public GameObject FriendlyEnemyObject;

	private GameObject gm;

	public int inputLocation;

	public Text m_scalefactor;

	public static float m_gaph_scenesizefactor = 1f;

	public Text m_effectName;

	private int index;

	private void Awake()
	{
		inputLocation = 0;
		m_effectName.text = m_effects[index].name.ToString();
		MakeObject();
	}

	private void Update()
	{
		InputKey();
		if (index < 70)
		{
			FriendlyEnemyObject.SetActive(value: false);
		}
		else
		{
			FriendlyEnemyObject.SetActive(value: true);
		}
	}

	private void InputKey()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (index <= 0)
			{
				index = m_effects.Length - 1;
			}
			else
			{
				index--;
			}
			MakeObject();
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			if (index >= m_effects.Length - 1)
			{
				index = 0;
			}
			else
			{
				index++;
			}
			MakeObject();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			MakeObject();
		}
	}

	private void MakeObject()
	{
		DestroyGameObject();
		gm = Object.Instantiate(m_effects[index], m_effects[index].transform.position, m_effects[index].transform.rotation).gameObject;
		m_effectName.text = index + 1 + " : " + m_effects[index].name.ToString();
		scaleform.transform.position = gm.transform.position;
		gm.transform.parent = scaleform.transform;
		gm.transform.localScale = new Vector3(1f, 1f, 1f);
		float submit_scalefactor = m_gaph_scenesizefactor;
		if (index < 70)
		{
			submit_scalefactor *= 0.5f;
		}
		gm.transform.localScale = new Vector3(submit_scalefactor, submit_scalefactor, submit_scalefactor);
		m_destroyObjects[inputLocation] = gm;
		inputLocation++;
	}

	private void DestroyGameObject()
	{
		for (int i = 0; i < inputLocation; i++)
		{
			Object.Destroy(m_destroyObjects[i]);
		}
		inputLocation = 0;
	}

	public void GetSizeFactor()
	{
		m_gaph_scenesizefactor = float.Parse(m_scalefactor.text.ToString());
		float submit_scalefactor = m_gaph_scenesizefactor;
		if (index < 70)
		{
			submit_scalefactor *= 0.5f;
		}
		gm.transform.localScale = new Vector3(submit_scalefactor, submit_scalefactor, submit_scalefactor);
	}
}
