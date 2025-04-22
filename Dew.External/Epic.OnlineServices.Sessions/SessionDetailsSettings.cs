namespace Epic.OnlineServices.Sessions;

public struct SessionDetailsSettings
{
	public Utf8String BucketId { get; set; }

	public uint NumPublicConnections { get; set; }

	public bool AllowJoinInProgress { get; set; }

	public OnlineSessionPermissionLevel PermissionLevel { get; set; }

	public bool InvitesAllowed { get; set; }

	public bool SanctionsEnabled { get; set; }

	public uint[] AllowedPlatformIds { get; set; }

	internal void Set(ref SessionDetailsSettingsInternal other)
	{
		BucketId = other.BucketId;
		NumPublicConnections = other.NumPublicConnections;
		AllowJoinInProgress = other.AllowJoinInProgress;
		PermissionLevel = other.PermissionLevel;
		InvitesAllowed = other.InvitesAllowed;
		SanctionsEnabled = other.SanctionsEnabled;
		AllowedPlatformIds = other.AllowedPlatformIds;
	}
}
