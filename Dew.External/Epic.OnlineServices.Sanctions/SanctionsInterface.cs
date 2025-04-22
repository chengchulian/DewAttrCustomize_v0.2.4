using System;

namespace Epic.OnlineServices.Sanctions;

public sealed class SanctionsInterface : Handle
{
	public const int CopyplayersanctionbyindexApiLatest = 1;

	public const int GetplayersanctioncountApiLatest = 1;

	public const int PlayersanctionApiLatest = 2;

	public const int QueryactiveplayersanctionsApiLatest = 2;

	public SanctionsInterface()
	{
	}

	public SanctionsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyPlayerSanctionByIndex(ref CopyPlayerSanctionByIndexOptions options, out PlayerSanction? outSanction)
	{
		CopyPlayerSanctionByIndexOptionsInternal optionsInternal = default(CopyPlayerSanctionByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSanctionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sanctions_CopyPlayerSanctionByIndex(base.InnerHandle, ref optionsInternal, ref outSanctionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<PlayerSanctionInternal, PlayerSanction>(outSanctionAddress, out outSanction);
		if (outSanction.HasValue)
		{
			Bindings.EOS_Sanctions_PlayerSanction_Release(outSanctionAddress);
		}
		return result;
	}

	public uint GetPlayerSanctionCount(ref GetPlayerSanctionCountOptions options)
	{
		GetPlayerSanctionCountOptionsInternal optionsInternal = default(GetPlayerSanctionCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Sanctions_GetPlayerSanctionCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryActivePlayerSanctions(ref QueryActivePlayerSanctionsOptions options, object clientData, OnQueryActivePlayerSanctionsCallback completionDelegate)
	{
		QueryActivePlayerSanctionsOptionsInternal optionsInternal = default(QueryActivePlayerSanctionsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryActivePlayerSanctionsCallbackInternal completionDelegateInternal = OnQueryActivePlayerSanctionsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sanctions_QueryActivePlayerSanctions(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnQueryActivePlayerSanctionsCallbackInternal))]
	internal static void OnQueryActivePlayerSanctionsCallbackInternalImplementation(ref QueryActivePlayerSanctionsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryActivePlayerSanctionsCallbackInfoInternal, OnQueryActivePlayerSanctionsCallback, QueryActivePlayerSanctionsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
