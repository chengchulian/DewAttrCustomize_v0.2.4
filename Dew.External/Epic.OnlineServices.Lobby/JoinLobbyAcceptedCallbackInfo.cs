namespace Epic.OnlineServices.Lobby;

public struct JoinLobbyAcceptedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ulong UiEventId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref JoinLobbyAcceptedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		UiEventId = other.UiEventId;
	}
}
