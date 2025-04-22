using UnityEngine;

public class PickaxeTrigger : MonoBehaviour
{
	private bool hasEntered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("OreNode") && !hasEntered)
		{
			other.GetComponent<OreNode>()?.oreHit();
			hasEntered = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		hasEntered = false;
	}
}
