namespace Epic.OnlineServices.Sessions;

public struct ActiveSessionInfo
{
	public Utf8String SessionName { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public OnlineSessionState State { get; set; }

	public SessionDetailsInfo? SessionDetails { get; set; }

	internal void Set(ref ActiveSessionInfoInternal other)
	{
		SessionName = other.SessionName;
		LocalUserId = other.LocalUserId;
		State = other.State;
		SessionDetails = other.SessionDetails;
	}
}
