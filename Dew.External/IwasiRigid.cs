using UnityEngine;

public class IwasiRigid : MonoBehaviour
{
	public bool mizuwoeta;

	private void OnMouseDown()
	{
		GetComponent<Rigidbody>().AddForce(Vector3.up * 5f + Vector3.forward * 2f, ForceMode.Impulse);
		GetComponents<AudioSource>()[1].Play();
	}
}
