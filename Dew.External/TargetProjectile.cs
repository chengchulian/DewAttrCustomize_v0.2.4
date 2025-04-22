using UnityEngine;

public class TargetProjectile : MonoBehaviour
{
	public float speed = 15f;

	public GameObject hit;

	public GameObject flash;

	public GameObject[] Detached;

	private Transform target;

	private Vector3 targetOffset;

	[Space]
	[Header("PROJECTILE PATH")]
	private float randomUpAngle;

	private float randomSideAngle;

	public float sideAngle = 25f;

	public float upAngle = 20f;

	private void Start()
	{
		FlashEffect();
		newRandom();
	}

	private void newRandom()
	{
		randomUpAngle = Random.Range(0f, upAngle);
		randomSideAngle = Random.Range(0f - sideAngle, sideAngle);
	}

	public void UpdateTarget(Transform targetPosition, Vector3 Offset)
	{
		target = targetPosition;
		targetOffset = Offset;
	}

	private void Update()
	{
		if (target == null)
		{
			GameObject[] detached = Detached;
			foreach (GameObject detachedPrefab in detached)
			{
				if (detachedPrefab != null)
				{
					detachedPrefab.transform.parent = null;
				}
			}
			Object.Destroy(base.gameObject);
		}
		else
		{
			Vector3 crossDirection = Vector3.Cross(target.position + targetOffset - base.transform.position, Vector3.up);
			Vector3 direction = Quaternion.Euler(0f, randomSideAngle, 0f) * Quaternion.AngleAxis(randomUpAngle, crossDirection) * (target.position + targetOffset - base.transform.position);
			float distanceThisFrame = Time.deltaTime * speed;
			if (direction.magnitude <= distanceThisFrame)
			{
				HitTarget();
				return;
			}
			base.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
			base.transform.rotation = Quaternion.LookRotation(direction);
		}
	}

	private void FlashEffect()
	{
		if (flash != null)
		{
			GameObject flashInstance = Object.Instantiate(flash, base.transform.position, Quaternion.identity);
			flashInstance.transform.forward = base.gameObject.transform.forward;
			ParticleSystem flashPs = flashInstance.GetComponent<ParticleSystem>();
			if (flashPs != null)
			{
				Object.Destroy(flashInstance, flashPs.main.duration);
				return;
			}
			ParticleSystem flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
			Object.Destroy(flashInstance, flashPsParts.main.duration);
		}
	}

	private void HitTarget()
	{
		if (hit != null)
		{
			GameObject hitInstance = Object.Instantiate(hit, target.position + targetOffset, base.transform.rotation);
			ParticleSystem hitPs = hitInstance.GetComponent<ParticleSystem>();
			if (hitPs != null)
			{
				Object.Destroy(hitInstance, hitPs.main.duration);
			}
			else
			{
				ParticleSystem hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
				Object.Destroy(hitInstance, hitPsParts.main.duration);
			}
		}
		GameObject[] detached = Detached;
		foreach (GameObject detachedPrefab in detached)
		{
			if (detachedPrefab != null)
			{
				detachedPrefab.transform.parent = null;
			}
		}
		Object.Destroy(base.gameObject);
	}
}
