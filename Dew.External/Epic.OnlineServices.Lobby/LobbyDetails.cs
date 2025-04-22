using System;

namespace Epic.OnlineServices.Lobby;

public sealed class LobbyDetails : Handle
{
	public const int LobbydetailsCopyattributebyindexApiLatest = 1;

	public const int LobbydetailsCopyattributebykeyApiLatest = 1;

	public const int LobbydetailsCopyinfoApiLatest = 1;

	public const int LobbydetailsCopymemberattributebyindexApiLatest = 1;

	public const int LobbydetailsCopymemberattributebykeyApiLatest = 1;

	public const int LobbydetailsCopymemberinfoApiLatest = 1;

	public const int LobbydetailsGetattributecountApiLatest = 1;

	public const int LobbydetailsGetlobbyownerApiLatest = 1;

	public const int LobbydetailsGetmemberattributecountApiLatest = 1;

	public const int LobbydetailsGetmemberbyindexApiLatest = 1;

	public const int LobbydetailsGetmembercountApiLatest = 1;

	public const int LobbydetailsInfoApiLatest = 3;

	public const int LobbydetailsMemberinfoApiLatest = 1;

	public LobbyDetails()
	{
	}

	public LobbyDetails(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyAttributeByIndex(ref LobbyDetailsCopyAttributeByIndexOptions options, out Attribute? outAttribute)
	{
		LobbyDetailsCopyAttributeByIndexOptionsInternal optionsInternal = default(LobbyDetailsCopyAttributeByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAttributeAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbyDetails_CopyAttributeByIndex(base.InnerHandle, ref optionsInternal, ref outAttributeAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<AttributeInternal, Attribute>(outAttributeAddress, out outAttribute);
		if (outAttribute.HasValue)
		{
			Bindings.EOS_Lobby_Attribute_Release(outAttributeAddress);
		}
		return result;
	}

	public Result CopyAttributeByKey(ref LobbyDetailsCopyAttributeByKeyOptions options, out Attribute? outAttribute)
	{
		LobbyDetailsCopyAttributeByKeyOptionsInternal optionsInternal = default(LobbyDetailsCopyAttributeByKeyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAttributeAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbyDetails_CopyAttributeByKey(base.InnerHandle, ref optionsInternal, ref outAttributeAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<AttributeInternal, Attribute>(outAttributeAddress, out outAttribute);
		if (outAttribute.HasValue)
		{
			Bindings.EOS_Lobby_Attribute_Release(outAttributeAddress);
		}
		return result;
	}

	public Result CopyInfo(ref LobbyDetailsCopyInfoOptions options, out LobbyDetailsInfo? outLobbyDetailsInfo)
	{
		LobbyDetailsCopyInfoOptionsInternal optionsInternal = default(LobbyDetailsCopyInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyDetailsInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbyDetails_CopyInfo(base.InnerHandle, ref optionsInternal, ref outLobbyDetailsInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<LobbyDetailsInfoInternal, LobbyDetailsInfo>(outLobbyDetailsInfoAddress, out outLobbyDetailsInfo);
		if (outLobbyDetailsInfo.HasValue)
		{
			Bindings.EOS_LobbyDetails_Info_Release(outLobbyDetailsInfoAddress);
		}
		return result;
	}

	public Result CopyMemberAttributeByIndex(ref LobbyDetailsCopyMemberAttributeByIndexOptions options, out Attribute? outAttribute)
	{
		LobbyDetailsCopyMemberAttributeByIndexOptionsInternal optionsInternal = default(LobbyDetailsCopyMemberAttributeByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAttributeAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbyDetails_CopyMemberAttributeByIndex(base.InnerHandle, ref optionsInternal, ref outAttributeAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<AttributeInternal, Attribute>(outAttributeAddress, out outAttribute);
		if (outAttribute.HasValue)
		{
			Bindings.EOS_Lobby_Attribute_Release(outAttributeAddress);
		}
		return result;
	}

	public Result CopyMemberAttributeByKey(ref LobbyDetailsCopyMemberAttributeByKeyOptions options, out Attribute? outAttribute)
	{
		LobbyDetailsCopyMemberAttributeByKeyOptionsInternal optionsInternal = default(LobbyDetailsCopyMemberAttributeByKeyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAttributeAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbyDetails_CopyMemberAttributeByKey(base.InnerHandle, ref optionsInternal, ref outAttributeAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<AttributeInternal, Attribute>(outAttributeAddress, out outAttribute);
		if (outAttribute.HasValue)
		{
			Bindings.EOS_Lobby_Attribute_Release(outAttributeAddress);
		}
		return result;
	}

	public Result CopyMemberInfo(ref LobbyDetailsCopyMemberInfoOptions options, out LobbyDetailsMemberInfo? outLobbyDetailsMemberInfo)
	{
		LobbyDetailsCopyMemberInfoOptionsInternal optionsInternal = default(LobbyDetailsCopyMemberInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyDetailsMemberInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbyDetails_CopyMemberInfo(base.InnerHandle, ref optionsInternal, ref outLobbyDetailsMemberInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<LobbyDetailsMemberInfoInternal, LobbyDetailsMemberInfo>(outLobbyDetailsMemberInfoAddress, out outLobbyDetailsMemberInfo);
		if (outLobbyDetailsMemberInfo.HasValue)
		{
			Bindings.EOS_LobbyDetails_MemberInfo_Release(outLobbyDetailsMemberInfoAddress);
		}
		return result;
	}

	public uint GetAttributeCount(ref LobbyDetailsGetAttributeCountOptions options)
	{
		LobbyDetailsGetAttributeCountOptionsInternal optionsInternal = default(LobbyDetailsGetAttributeCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_LobbyDetails_GetAttributeCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public ProductUserId GetLobbyOwner(ref LobbyDetailsGetLobbyOwnerOptions options)
	{
		LobbyDetailsGetLobbyOwnerOptionsInternal optionsInternal = default(LobbyDetailsGetLobbyOwnerOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_LobbyDetails_GetLobbyOwner(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out ProductUserId funcResultReturn);
		return funcResultReturn;
	}

	public uint GetMemberAttributeCount(ref LobbyDetailsGetMemberAttributeCountOptions options)
	{
		LobbyDetailsGetMemberAttributeCountOptionsInternal optionsInternal = default(LobbyDetailsGetMemberAttributeCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_LobbyDetails_GetMemberAttributeCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public ProductUserId GetMemberByIndex(ref LobbyDetailsGetMemberByIndexOptions options)
	{
		LobbyDetailsGetMemberByIndexOptionsInternal optionsInternal = default(LobbyDetailsGetMemberByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_LobbyDetails_GetMemberByIndex(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out ProductUserId funcResultReturn);
		return funcResultReturn;
	}

	public uint GetMemberCount(ref LobbyDetailsGetMemberCountOptions options)
	{
		LobbyDetailsGetMemberCountOptionsInternal optionsInternal = default(LobbyDetailsGetMemberCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_LobbyDetails_GetMemberCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_LobbyDetails_Release(base.InnerHandle);
	}
}
