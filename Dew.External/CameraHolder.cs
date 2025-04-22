using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
	public struct SVA
	{
		public float S;

		public float V;

		public float A;
	}

	public Transform Holder;

	public float currDistance = 5f;

	public float xRotate = 250f;

	public float yRotate = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float prevDistance;

	private float x;

	private float y;

	[Header("GUI")]
	private float windowDpi;

	public GameObject[] Prefabs;

	private int Prefab;

	private GameObject Instance;

	private float StartColor;

	private float HueColor;

	public Texture HueTexture;

	private ParticleSystem[] particleSystems = new ParticleSystem[0];

	private List<SVA> svList = new List<SVA>();

	private float H;

	private void Start()
	{
		if (Screen.dpi < 1f)
		{
			windowDpi = 1f;
		}
		if (Screen.dpi < 200f)
		{
			windowDpi = 1f;
		}
		else
		{
			windowDpi = Screen.dpi / 200f;
		}
		Vector3 angles = base.transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		Counter(0);
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(5f * windowDpi, 5f * windowDpi, 110f * windowDpi, 35f * windowDpi), "Previous effect"))
		{
			Counter(-1);
		}
		if (GUI.Button(new Rect(120f * windowDpi, 5f * windowDpi, 110f * windowDpi, 35f * windowDpi), "Play again"))
		{
			Counter(0);
		}
		if (GUI.Button(new Rect(235f * windowDpi, 5f * windowDpi, 110f * windowDpi, 35f * windowDpi), "Next effect"))
		{
			Counter(1);
		}
		StartColor = HueColor;
		HueColor = GUI.HorizontalSlider(new Rect(5f * windowDpi, 45f * windowDpi, 340f * windowDpi, 35f * windowDpi), HueColor, 0f, 1f);
		GUI.DrawTexture(new Rect(5f * windowDpi, 65f * windowDpi, 340f * windowDpi, 15f * windowDpi), HueTexture, ScaleMode.StretchToFill, alphaBlend: false, 0f);
		if (HueColor != StartColor)
		{
			int i = 0;
			ParticleSystem[] array = particleSystems;
			for (int j = 0; j < array.Length; j++)
			{
				ParticleSystem.MainModule main = array[j].main;
				Color colorHSV = Color.HSVToRGB(HueColor + H * 0f, svList[i].S, svList[i].V);
				main.startColor = new Color(colorHSV.r, colorHSV.g, colorHSV.b, svList[i].A);
				i++;
			}
		}
	}

	private void Counter(int count)
	{
		Prefab += count;
		if (Prefab > Prefabs.Length - 1)
		{
			Prefab = 0;
		}
		else if (Prefab < 0)
		{
			Prefab = Prefabs.Length - 1;
		}
		if (Instance != null)
		{
			Object.Destroy(Instance);
		}
		Instance = Object.Instantiate(Prefabs[Prefab]);
		particleSystems = Instance.GetComponentsInChildren<ParticleSystem>();
		svList.Clear();
		ParticleSystem[] array = particleSystems;
		for (int i = 0; i < array.Length; i++)
		{
			Color baseColor = array[i].main.startColor.color;
			SVA baseSVA = default(SVA);
			Color.RGBToHSV(baseColor, out H, out baseSVA.S, out baseSVA.V);
			baseSVA.A = baseColor.a;
			svList.Add(baseSVA);
		}
	}

	private void LateUpdate()
	{
		if (currDistance < 2f)
		{
			currDistance = 2f;
		}
		currDistance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
		if ((bool)Holder && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
		{
			Vector3 pos = Input.mousePosition;
			float dpiScale = 1f;
			if (Screen.dpi < 1f)
			{
				dpiScale = 1f;
			}
			dpiScale = ((!(Screen.dpi < 200f)) ? (Screen.dpi / 200f) : 1f);
			if (pos.x < 380f * dpiScale && (float)Screen.height - pos.y < 250f * dpiScale)
			{
				return;
			}
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			x += (float)((double)(Input.GetAxis("Mouse X") * xRotate) * 0.02);
			y -= (float)((double)(Input.GetAxis("Mouse Y") * yRotate) * 0.02);
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion rotation = Quaternion.Euler(y, x, 0f);
			Vector3 position = rotation * new Vector3(0f, 0f, 0f - currDistance) + Holder.position;
			base.transform.rotation = rotation;
			base.transform.position = position;
		}
		else
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		if (prevDistance != currDistance)
		{
			prevDistance = currDistance;
			Quaternion rot = Quaternion.Euler(y, x, 0f);
			Vector3 po = rot * new Vector3(0f, 0f, 0f - currDistance) + Holder.position;
			base.transform.rotation = rot;
			base.transform.position = po;
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
