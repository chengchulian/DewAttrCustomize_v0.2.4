using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class SkyNightFishProp : DewNetworkBehaviour
{
	public GameObject dest;

	public float interval;

	public float moveTime;

	private Vector3 _startPos;

	private float _elapsedTime;

	private Vector3 _endPos;

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (base.isServer)
		{
			_startPos = base.transform.position;
			_endPos = dest.transform.position;
			base.transform.LookAt(_endPos, Vector3.up);
		}
	}

	[ClientRpc]
	public void Open()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void SkyNightFishProp::Open()", 474949345, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void OnDrawGizmosSelected()
	{
		if (dest != null)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(base.transform.position, dest.transform.position);
		}
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_Open()
	{
		SkyNightFishes[] componentsInChildren = GetComponentsInChildren<SkyNightFishes>(includeInactive: true);
		foreach (SkyNightFishes skyNightFishes in componentsInChildren)
		{
			if (!skyNightFishes.gameObject.activeSelf)
			{
				skyNightFishes.gameObject.SetActive(value: true);
			}
		}
		float num = Vector3.Distance(_startPos, _endPos) / moveTime;
		float movedisPerInterval = num * interval;
		Vector3 dir = (_endPos - _startPos).normalized;
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			_elapsedTime = 0f;
			while (_elapsedTime < moveTime)
			{
				if (Time.timeScale <= 0f)
				{
					yield return null;
				}
				base.transform.position += dir * movedisPerInterval;
				_elapsedTime += interval;
				yield return new SI.WaitForSeconds(interval);
			}
			yield return new SI.WaitForSeconds(moveTime);
			Object.Destroy(base.gameObject);
		}
	}

	protected static void InvokeUserCode_Open(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Open called on server.");
		}
		else
		{
			((SkyNightFishProp)obj).UserCode_Open();
		}
	}

	static SkyNightFishProp()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(SkyNightFishProp), "System.Void SkyNightFishProp::Open()", InvokeUserCode_Open);
	}
}
