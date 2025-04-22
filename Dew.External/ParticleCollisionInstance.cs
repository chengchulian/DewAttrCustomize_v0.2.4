using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionInstance : MonoBehaviour
{
	public GameObject[] EffectsOnCollision;

	public float DestroyTimeDelay = 5f;

	public bool UseWorldSpacePosition;

	public float Offset;

	public Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

	public bool useOnlyRotationOffset = true;

	public bool UseFirePointRotation;

	public bool DestoyMainEffect = true;

	private ParticleSystem part;

	private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

	private ParticleSystem ps;

	private void Start()
	{
		part = GetComponent<ParticleSystem>();
	}

	private void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
		for (int i = 0; i < numCollisionEvents; i++)
		{
			GameObject[] effectsOnCollision = EffectsOnCollision;
			for (int j = 0; j < effectsOnCollision.Length; j++)
			{
				GameObject instance = Object.Instantiate(effectsOnCollision[j], collisionEvents[i].intersection + collisionEvents[i].normal * Offset, default(Quaternion));
				if (!UseWorldSpacePosition)
				{
					instance.transform.parent = base.transform;
				}
				if (UseFirePointRotation)
				{
					instance.transform.LookAt(base.transform.position);
				}
				else if (rotationOffset != Vector3.zero && useOnlyRotationOffset)
				{
					instance.transform.rotation = Quaternion.Euler(rotationOffset);
				}
				else
				{
					instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
					instance.transform.rotation *= Quaternion.Euler(rotationOffset);
				}
				Object.Destroy(instance, DestroyTimeDelay);
			}
		}
		if (DestoyMainEffect)
		{
			Object.Destroy(base.gameObject, DestroyTimeDelay + 0.5f);
		}
	}
}
