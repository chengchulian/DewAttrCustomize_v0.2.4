using System;
using System.Collections;
using UnityEngine;

public class Forest_FogVoid : MonoBehaviour
{
	public float interval;

	public float increment;

	public float maxSize;

	public GameObject ppVolume;

	private void Start()
	{
		GameManager.CallOnReady(delegate
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnDeath += new Action<EventInfoKill>(OnEntityDeath);
		});
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ClientEventManager>.instance != null)
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnDeath -= new Action<EventInfoKill>(OnEntityDeath);
		}
	}

	private void OnEntityDeath(EventInfoKill i)
	{
		if (i.victim is Monster { type: Monster.MonsterType.Boss })
		{
			FogVoidSequenceStart(i.victim);
		}
	}

	private void FogVoidSequenceStart(Entity e)
	{
		base.transform.position = new Vector3(e.transform.position.x, base.transform.position.y, e.transform.position.z);
		global::UnityEngine.Object.Destroy(ppVolume);
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (base.transform.localScale.x < maxSize)
			{
				base.transform.localScale = new Vector3(base.transform.localScale.x + increment, base.transform.localScale.y, base.transform.localScale.z + increment);
				yield return new WaitForSeconds(interval);
			}
		}
	}
}
