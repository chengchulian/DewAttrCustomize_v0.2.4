using UnityEngine;

public class SardineBoidScript : MonoBehaviour
{
	public Transform maneru;

	public float speed = 1.5f;

	public GameObject[] eyes;

	private Rigidbody iwasirigid;

	public SardineBoidsController sardineBoidsController;

	public float maneruSpeed = 1f;

	private void Start()
	{
		iwasirigid = GetComponent<Rigidbody>();
		GetComponent<Animator>().SetFloat("Forward", speed);
	}

	private void Update()
	{
		Miru();
		Maneru();
		iwasirigid.velocity = base.transform.forward * speed;
	}

	private void Miru()
	{
		GameObject[] array = eyes;
		foreach (GameObject eye in array)
		{
			if (Physics.Raycast(eye.transform.position, eye.transform.up, out var hitInfo, 100f) && hitInfo.collider.name == "SardineCol")
			{
				maneru = hitInfo.transform;
			}
		}
	}

	private void Maneru()
	{
		if (maneru != null)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, maneru.rotation, Time.deltaTime * maneruSpeed);
		}
	}
}
