using UnityEngine;

public class IwasiOcean : MonoBehaviour
{
	public int iwasicount;

	public GameObject zaru;

	public bool isCleared;

	public int maxIwasicount = 10;

	private void OnTriggerEnter(Collider other)
	{
		Rigidbody componentInParent = other.GetComponentInParent<Rigidbody>();
		other.transform.root.rotation = Quaternion.identity;
		IwasiMove aniwasimove = other.GetComponentInParent<IwasiMove>();
		if (!aniwasimove.enabled)
		{
			other.GetComponentInParent<AudioSource>().Play();
			aniwasimove.enabled = true;
			iwasicount++;
			if ((iwasicount > maxIwasicount - 1) & !isCleared)
			{
				Object.Destroy(zaru);
				isCleared = true;
			}
		}
		componentInParent.constraints = (RigidbodyConstraints)84;
	}
}
