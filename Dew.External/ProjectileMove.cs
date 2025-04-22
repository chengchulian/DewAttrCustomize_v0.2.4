using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
	public float speed;

	public float fireRate;

	public GameObject muzzlePrefab;

	public GameObject hitPrefab;

	private void Start()
	{
		if (muzzlePrefab != null)
		{
			GameObject muzzleVFX = Object.Instantiate(muzzlePrefab, base.transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = base.gameObject.transform.forward;
			ParticleSystem psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
			if (psMuzzle != null)
			{
				Object.Destroy(muzzleVFX, psMuzzle.main.duration);
				return;
			}
			ParticleSystem psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
			Object.Destroy(muzzleVFX, psChild.main.duration);
		}
	}

	private void Update()
	{
		if (speed != 0f)
		{
			base.transform.position += base.transform.forward * (speed * Time.deltaTime);
		}
		else
		{
			Debug.Log("No Speed");
		}
	}

	private void OnCollisionEnter(Collision co)
	{
		speed = 0f;
		ContactPoint contact = co.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		if (hitPrefab != null)
		{
			GameObject hitVFX = Object.Instantiate(hitPrefab, pos, rot);
			ParticleSystem psHit = hitVFX.GetComponent<ParticleSystem>();
			if (psHit != null)
			{
				Object.Destroy(hitVFX, psHit.main.duration);
			}
			else
			{
				ParticleSystem psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Object.Destroy(hitVFX, psChild.main.duration);
			}
		}
		Object.Destroy(base.gameObject);
	}
}
