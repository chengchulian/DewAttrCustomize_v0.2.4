using System.Collections;
using UnityEngine;

public class FrontAttack : MonoBehaviour
{
	public Transform pivot;

	public Vector3 startRotation;

	public float speed = 15f;

	public float drug = 1f;

	public GameObject craterPrefab;

	public ParticleSystem ps;

	public bool playPS;

	public float spawnRate = 1f;

	public float spawnDuration = 1f;

	public float positionOffset;

	public bool changeScale;

	private float randomTimer;

	private float attackingTimer;

	private float startSpeed;

	private Vector3 stepPosition;

	[Space]
	[Header("Effect with Mesh animation")]
	public bool effectWithAnimation;

	public Animator[] anim;

	public float delay;

	public bool playMeshEffect;

	private void Update()
	{
		if (playMeshEffect)
		{
			StartCoroutine(MeshEffect());
			playMeshEffect = false;
		}
	}

	public void PrepeareAttack(Vector3 targetPoint)
	{
		if (effectWithAnimation)
		{
			StartCoroutine(MeshEffect());
			return;
		}
		if (playPS)
		{
			ps.Play();
		}
		startSpeed = speed;
		base.transform.parent = null;
		base.transform.position = pivot.position;
		Vector3 lookPos = targetPoint - base.transform.position;
		lookPos.y = 0f;
		if (!playPS)
		{
			base.transform.rotation = Quaternion.LookRotation(lookPos);
		}
		else
		{
			base.transform.rotation = Quaternion.LookRotation(lookPos) * Quaternion.Euler(startRotation);
		}
		stepPosition = pivot.position;
		randomTimer = 0f;
		StartCoroutine(StartMove());
	}

	public IEnumerator MeshEffect()
	{
		if (playPS)
		{
			ps.Play();
		}
		yield return new WaitForSeconds(delay);
		Animator[] array = anim;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetTrigger("Attack");
		}
	}

	public IEnumerator StartMove()
	{
		attackingTimer += Time.deltaTime;
		while (true)
		{
			randomTimer += Time.deltaTime;
			startSpeed *= drug;
			base.transform.position += base.transform.forward * (startSpeed * Time.deltaTime);
			if ((base.transform.position - stepPosition).magnitude > spawnRate)
			{
				if (craterPrefab != null)
				{
					Vector3 randomPosition = new Vector3(Random.Range(0f - positionOffset, positionOffset), 0f, Random.Range(0f - positionOffset, positionOffset));
					Vector3 pos = base.transform.position + randomPosition * randomTimer * 2f;
					if (Terrain.activeTerrain != null)
					{
						pos.y = Terrain.activeTerrain.SampleHeight(base.transform.position);
					}
					GameObject craterInstance = Object.Instantiate(craterPrefab, pos, Quaternion.identity);
					if (changeScale)
					{
						craterInstance.transform.localScale += new Vector3(randomTimer * 2f, randomTimer * 2f, randomTimer * 2f);
					}
					ParticleSystem craterPs = craterInstance.GetComponent<ParticleSystem>();
					if (craterPs != null)
					{
						Object.Destroy(craterInstance, craterPs.main.duration);
					}
					else
					{
						ParticleSystem flashPsParts = craterInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
						Object.Destroy(craterInstance, flashPsParts.main.duration);
					}
				}
				stepPosition = base.transform.position;
			}
			if (randomTimer > spawnDuration)
			{
				break;
			}
			yield return null;
		}
		base.transform.parent = pivot;
		base.transform.position = pivot.position;
		base.transform.rotation = Quaternion.Euler(startRotation);
	}
}
