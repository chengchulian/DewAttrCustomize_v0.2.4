using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Shrine_LoopCat : Shrine, ICustomInteractable
{
	public GameObject effect;

	public GameObject telephoneSound;

	private Animator _animator;

	public string nameRawText => DewLocalization.GetUIValue(GetType().Name + "_Name");

	public string interactActionRawText => DewLocalization.GetUIValue("InGame_Tooltip_Pet");

	public Vector3 worldOffset => new Vector3(0f, 2.3f, 0f);

	protected override void OnCreate()
	{
		base.OnCreate();
		_animator = GetComponentInChildren<Animator>();
	}

	protected override bool OnUse(Entity entity)
	{
		if (Random.value <= 0.1f)
		{
			FxPlayNewNetworked(telephoneSound);
		}
		OnUseRoutine();
		return true;
	}

	[ClientRpc]
	private void OnUseRoutine()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void Shrine_LoopCat::OnUseRoutine()", 68360535, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_OnUseRoutine()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			_animator.Play("Up");
			yield return new WaitForSeconds(2f);
			FxPlayNew(effect, base.transform.position, null);
			if (base.isServer)
			{
				Destroy();
			}
		}
	}

	protected static void InvokeUserCode_OnUseRoutine(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC OnUseRoutine called on server.");
		}
		else
		{
			((Shrine_LoopCat)obj).UserCode_OnUseRoutine();
		}
	}

	static Shrine_LoopCat()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Shrine_LoopCat), "System.Void Shrine_LoopCat::OnUseRoutine()", InvokeUserCode_OnUseRoutine);
	}
}
