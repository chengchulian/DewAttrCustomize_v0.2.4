using System.Collections;
using UnityEngine;

public class OreNode : MonoBehaviour
{
	[SerializeField]
	private GameObject pieces;

	[SerializeField]
	private GameObject refinedPickup;

	[SerializeField]
	private int dropOnHit;

	[SerializeField]
	private int hitsToDestroy;

	[SerializeField]
	private int dropOnDestroy;

	[SerializeField]
	private Vector3 knockAngle;

	[SerializeField]
	private AnimationCurve knockCurve;

	[SerializeField]
	private float knockDuration = 1f;

	private int dropIndex;

	private int hitIndex;

	private void OnMouseDown()
	{
		oreHit();
	}

	public void oreHit()
	{
		hitIndex++;
		if (hitIndex < hitsToDestroy)
		{
			dropIndex = dropOnHit;
		}
		else
		{
			dropIndex = dropOnDestroy;
		}
		Bounds worldBounds = GetComponent<Renderer>().bounds;
		float minX = worldBounds.min.x;
		float maxX = worldBounds.max.x;
		float centerY = worldBounds.center.y;
		float minZ = worldBounds.min.z;
		float maxZ = worldBounds.max.z;
		for (int i = 0; i < dropIndex; i++)
		{
			Object.Instantiate(position: new Vector3(Random.Range(minX, maxX), centerY, Random.Range(minZ, maxZ)), original: refinedPickup, rotation: Quaternion.Euler(0f, Random.Range(0, 360), 0f));
		}
		if (hitIndex < hitsToDestroy)
		{
			StartCoroutine(Animate());
			return;
		}
		Vector3 position2 = base.transform.position;
		Quaternion rotation = base.transform.rotation;
		Object.Instantiate(pieces, position2, rotation);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator Animate()
	{
		float t = 0f;
		while (t < knockDuration)
		{
			float v = knockCurve.Evaluate(t / knockDuration);
			base.transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(knockAngle), v);
			t += Time.deltaTime;
			yield return null;
		}
	}
}
