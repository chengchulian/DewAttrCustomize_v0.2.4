using UnityEngine;

namespace commanastationwww.eternaltemple;

public class HideGroup : MonoBehaviour
{
	public bool hideable = true;

	public bool disableParentGeo = true;

	private int collisionEntriesCounter;

	private HideablePart[] hidableGroup;

	private ParticleSystem[] particlesGroup;

	private Renderer rendererComponent;

	private void Start()
	{
		collisionEntriesCounter = 0;
		hidableGroup = base.gameObject.GetComponentsInChildren<HideablePart>();
		particlesGroup = GetComponentsInChildren<ParticleSystem>();
		if (disableParentGeo)
		{
			rendererComponent = GetComponent<Renderer>();
			if (rendererComponent != null)
			{
				rendererComponent.enabled = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (hideable && other.gameObject.tag == "MainCamera")
		{
			collisionEntriesCounter++;
			HideablePart[] array = hidableGroup;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].hide();
			}
			ParticleSystem[] array2 = particlesGroup;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Stop();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!hideable || !(other.gameObject.tag == "MainCamera"))
		{
			return;
		}
		collisionEntriesCounter--;
		if (collisionEntriesCounter <= 0)
		{
			HideablePart[] array = hidableGroup;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].unhide();
			}
			ParticleSystem[] array2 = particlesGroup;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Play();
			}
			collisionEntriesCounter = 0;
		}
	}

	private void Update()
	{
	}
}
