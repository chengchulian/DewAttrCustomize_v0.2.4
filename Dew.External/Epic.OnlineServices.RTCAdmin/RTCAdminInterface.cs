using System;

namespace Epic.OnlineServices.RTCAdmin;

public sealed class RTCAdminInterface : Handle
{
	public const int CopyusertokenbyindexApiLatest = 2;

	public const int CopyusertokenbyuseridApiLatest = 2;

	public const int KickApiLatest = 1;

	public const int QueryjoinroomtokenApiLatest = 2;

	public const int SetparticipanthardmuteApiLatest = 1;

	public const int UsertokenApiLatest = 1;

	public RTCAdminInterface()
	{
	}

	public RTCAdminInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyUserTokenByIndex(ref CopyUserTokenByIndexOptions options, out UserToken? outUserToken)
	{
		CopyUserTokenByIndexOptionsInternal optionsInternal = default(CopyUserTokenByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outUserTokenAddress = IntPtr.Zero;
		Result result = Bindings.EOS_RTCAdmin_CopyUserTokenByIndex(base.InnerHandle, ref optionsInternal, ref outUserTokenAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<UserTokenInternal, UserToken>(outUserTokenAddress, out outUserToken);
		if (outUserToken.HasValue)
		{
			Bindings.EOS_RTCAdmin_UserToken_Release(outUserTokenAddress);
		}
		return result;
	}

	public Result CopyUserTokenByUserId(ref CopyUserTokenByUserIdOptions options, out UserToken? outUserToken)
	{
		CopyUserTokenByUserIdOptionsInternal optionsInternal = default(CopyUserTokenByUserIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outUserTokenAddress = IntPtr.Zero;
		Result result = Bindings.EOS_RTCAdmin_CopyUserTokenByUserId(base.InnerHandle, ref optionsInternal, ref outUserTokenAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<UserTokenInternal, UserToken>(outUserTokenAddress, out outUserToken);
		if (outUserToken.HasValue)
		{
			Bindings.EOS_RTCAdmin_UserToken_Release(outUserTokenAddress);
		}
		return result;
	}

	public void Kick(ref KickOptions options, object clientData, OnKickCompleteCallback completionDelegate)
	{
		KickOptionsInternal optionsInternal = default(KickOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnKickCompleteCallbackInternal completionDelegateInternal = OnKickCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAdmin_Kick(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryJoinRoomToken(ref QueryJoinRoomTokenOptions options, object clientData, OnQueryJoinRoomTokenCompleteCallback completionDelegate)
	{
		QueryJoinRoomTokenOptionsInternal optionsInternal = default(QueryJoinRoomTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryJoinRoomTokenCompleteCallbackInternal completionDelegateInternal = OnQueryJoinRoomTokenCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAdmin_QueryJoinRoomToken(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void SetParticipantHardMute(ref SetParticipantHardMuteOptions options, object clientData, OnSetParticipantHardMuteCompleteCallback completionDelegate)
	{
		SetParticipantHardMuteOptionsInternal optionsInternal = default(SetParticipantHardMuteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSetParticipantHardMuteCompleteCallbackInternal completionDelegateInternal = OnSetParticipantHardMuteCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAdmin_SetParticipantHardMute(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnKickCompleteCallbackInternal))]
	internal static void OnKickCompleteCallbackInternalImplementation(ref KickCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<KickCompleteCallbackInfoInternal, OnKickCompleteCallback, KickCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryJoinRoomTokenCompleteCallbackInternal))]
	internal static void OnQueryJoinRoomTokenCompleteCallbackInternalImplementation(ref QueryJoinRoomTokenCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryJoinRoomTokenCompleteCallbackInfoInternal, OnQueryJoinRoomTokenCompleteCallback, QueryJoinRoomTokenCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSetParticipantHardMuteCompleteCallbackInternal))]
	internal static void OnSetParticipantHardMuteCompleteCallbackInternalImplementation(ref SetParticipantHardMuteCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SetParticipantHardMuteCompleteCallbackInfoInternal, OnSetParticipantHardMuteCompleteCallback, SetParticipantHardMuteCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
