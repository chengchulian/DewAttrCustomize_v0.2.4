namespace Epic.OnlineServices.Auth;

public struct VerifyIdTokenCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public Utf8String ApplicationId { get; set; }

	public Utf8String ClientId { get; set; }

	public Utf8String ProductId { get; set; }

	public Utf8String SandboxId { get; set; }

	public Utf8String DeploymentId { get; set; }

	public Utf8String DisplayName { get; set; }

	public bool IsExternalAccountInfoPresent { get; set; }

	public ExternalAccountType ExternalAccountIdType { get; set; }

	public Utf8String ExternalAccountId { get; set; }

	public Utf8String ExternalAccountDisplayName { get; set; }

	public Utf8String Platform { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref VerifyIdTokenCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		ApplicationId = other.ApplicationId;
		ClientId = other.ClientId;
		ProductId = other.ProductId;
		SandboxId = other.SandboxId;
		DeploymentId = other.DeploymentId;
		DisplayName = other.DisplayName;
		IsExternalAccountInfoPresent = other.IsExternalAccountInfoPresent;
		ExternalAccountIdType = other.ExternalAccountIdType;
		ExternalAccountId = other.ExternalAccountId;
		ExternalAccountDisplayName = other.ExternalAccountDisplayName;
		Platform = other.Platform;
	}
}
