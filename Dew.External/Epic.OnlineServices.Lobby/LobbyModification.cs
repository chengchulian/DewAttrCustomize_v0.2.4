using System;

namespace Epic.OnlineServices.Lobby;

public sealed class LobbyModification : Handle
{
	public const int LobbymodificationAddattributeApiLatest = 2;

	public const int LobbymodificationAddmemberattributeApiLatest = 2;

	public const int LobbymodificationMaxAttributeLength = 64;

	public const int LobbymodificationMaxAttributes = 64;

	public const int LobbymodificationRemoveattributeApiLatest = 1;

	public const int LobbymodificationRemovememberattributeApiLatest = 1;

	public const int LobbymodificationSetallowedplatformidsApiLatest = 1;

	public const int LobbymodificationSetbucketidApiLatest = 1;

	public const int LobbymodificationSetinvitesallowedApiLatest = 1;

	public const int LobbymodificationSetmaxmembersApiLatest = 1;

	public const int LobbymodificationSetpermissionlevelApiLatest = 1;

	public LobbyModification()
	{
	}

	public LobbyModification(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result AddAttribute(ref LobbyModificationAddAttributeOptions options)
	{
		LobbyModificationAddAttributeOptionsInternal optionsInternal = default(LobbyModificationAddAttributeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_AddAttribute(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result AddMemberAttribute(ref LobbyModificationAddMemberAttributeOptions options)
	{
		LobbyModificationAddMemberAttributeOptionsInternal optionsInternal = default(LobbyModificationAddMemberAttributeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_AddMemberAttribute(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_LobbyModification_Release(base.InnerHandle);
	}

	public Result RemoveAttribute(ref LobbyModificationRemoveAttributeOptions options)
	{
		LobbyModificationRemoveAttributeOptionsInternal optionsInternal = default(LobbyModificationRemoveAttributeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_RemoveAttribute(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result RemoveMemberAttribute(ref LobbyModificationRemoveMemberAttributeOptions options)
	{
		LobbyModificationRemoveMemberAttributeOptionsInternal optionsInternal = default(LobbyModificationRemoveMemberAttributeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_RemoveMemberAttribute(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetAllowedPlatformIds(ref LobbyModificationSetAllowedPlatformIdsOptions options)
	{
		LobbyModificationSetAllowedPlatformIdsOptionsInternal optionsInternal = default(LobbyModificationSetAllowedPlatformIdsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_SetAllowedPlatformIds(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetBucketId(ref LobbyModificationSetBucketIdOptions options)
	{
		LobbyModificationSetBucketIdOptionsInternal optionsInternal = default(LobbyModificationSetBucketIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_SetBucketId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetInvitesAllowed(ref LobbyModificationSetInvitesAllowedOptions options)
	{
		LobbyModificationSetInvitesAllowedOptionsInternal optionsInternal = default(LobbyModificationSetInvitesAllowedOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_SetInvitesAllowed(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetMaxMembers(ref LobbyModificationSetMaxMembersOptions options)
	{
		LobbyModificationSetMaxMembersOptionsInternal optionsInternal = default(LobbyModificationSetMaxMembersOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_SetMaxMembers(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetPermissionLevel(ref LobbyModificationSetPermissionLevelOptions options)
	{
		LobbyModificationSetPermissionLevelOptionsInternal optionsInternal = default(LobbyModificationSetPermissionLevelOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbyModification_SetPermissionLevel(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}
}
