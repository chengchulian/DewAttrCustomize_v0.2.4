using System;

namespace Epic.OnlineServices.Sessions;

public sealed class SessionModification : Handle
{
	public const int SessionmodificationAddattributeApiLatest = 2;

	public const int SessionmodificationMaxSessionAttributeLength = 64;

	public const int SessionmodificationMaxSessionAttributes = 64;

	public const int SessionmodificationMaxSessionidoverrideLength = 64;

	public const int SessionmodificationMinSessionidoverrideLength = 16;

	public const int SessionmodificationRemoveattributeApiLatest = 1;

	public const int SessionmodificationSetallowedplatformidsApiLatest = 1;

	public const int SessionmodificationSetbucketidApiLatest = 1;

	public const int SessionmodificationSethostaddressApiLatest = 1;

	public const int SessionmodificationSetinvitesallowedApiLatest = 1;

	public const int SessionmodificationSetjoininprogressallowedApiLatest = 1;

	public const int SessionmodificationSetmaxplayersApiLatest = 1;

	public const int SessionmodificationSetpermissionlevelApiLatest = 1;

	public SessionModification()
	{
	}

	public SessionModification(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result AddAttribute(ref SessionModificationAddAttributeOptions options)
	{
		SessionModificationAddAttributeOptionsInternal optionsInternal = default(SessionModificationAddAttributeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_AddAttribute(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_SessionModification_Release(base.InnerHandle);
	}

	public Result RemoveAttribute(ref SessionModificationRemoveAttributeOptions options)
	{
		SessionModificationRemoveAttributeOptionsInternal optionsInternal = default(SessionModificationRemoveAttributeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_RemoveAttribute(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetAllowedPlatformIds(ref SessionModificationSetAllowedPlatformIdsOptions options)
	{
		SessionModificationSetAllowedPlatformIdsOptionsInternal optionsInternal = default(SessionModificationSetAllowedPlatformIdsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetAllowedPlatformIds(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetBucketId(ref SessionModificationSetBucketIdOptions options)
	{
		SessionModificationSetBucketIdOptionsInternal optionsInternal = default(SessionModificationSetBucketIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetBucketId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetHostAddress(ref SessionModificationSetHostAddressOptions options)
	{
		SessionModificationSetHostAddressOptionsInternal optionsInternal = default(SessionModificationSetHostAddressOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetHostAddress(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetInvitesAllowed(ref SessionModificationSetInvitesAllowedOptions options)
	{
		SessionModificationSetInvitesAllowedOptionsInternal optionsInternal = default(SessionModificationSetInvitesAllowedOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetInvitesAllowed(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetJoinInProgressAllowed(ref SessionModificationSetJoinInProgressAllowedOptions options)
	{
		SessionModificationSetJoinInProgressAllowedOptionsInternal optionsInternal = default(SessionModificationSetJoinInProgressAllowedOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetJoinInProgressAllowed(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetMaxPlayers(ref SessionModificationSetMaxPlayersOptions options)
	{
		SessionModificationSetMaxPlayersOptionsInternal optionsInternal = default(SessionModificationSetMaxPlayersOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetMaxPlayers(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetPermissionLevel(ref SessionModificationSetPermissionLevelOptions options)
	{
		SessionModificationSetPermissionLevelOptionsInternal optionsInternal = default(SessionModificationSetPermissionLevelOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionModification_SetPermissionLevel(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}
}
