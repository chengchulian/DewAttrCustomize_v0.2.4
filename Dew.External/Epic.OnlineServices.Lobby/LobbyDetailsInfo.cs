namespace Epic.OnlineServices.Lobby;

public struct LobbyDetailsInfo
{
	public Utf8String LobbyId { get; set; }

	public ProductUserId LobbyOwnerUserId { get; set; }

	public LobbyPermissionLevel PermissionLevel { get; set; }

	public uint AvailableSlots { get; set; }

	public uint MaxMembers { get; set; }

	public bool AllowInvites { get; set; }

	public Utf8String BucketId { get; set; }

	public bool AllowHostMigration { get; set; }

	public bool RTCRoomEnabled { get; set; }

	public bool AllowJoinById { get; set; }

	public bool RejoinAfterKickRequiresInvite { get; set; }

	public bool PresenceEnabled { get; set; }

	public uint[] AllowedPlatformIds { get; set; }

	internal void Set(ref LobbyDetailsInfoInternal other)
	{
		LobbyId = other.LobbyId;
		LobbyOwnerUserId = other.LobbyOwnerUserId;
		PermissionLevel = other.PermissionLevel;
		AvailableSlots = other.AvailableSlots;
		MaxMembers = other.MaxMembers;
		AllowInvites = other.AllowInvites;
		BucketId = other.BucketId;
		AllowHostMigration = other.AllowHostMigration;
		RTCRoomEnabled = other.RTCRoomEnabled;
		AllowJoinById = other.AllowJoinById;
		RejoinAfterKickRequiresInvite = other.RejoinAfterKickRequiresInvite;
		PresenceEnabled = other.PresenceEnabled;
		AllowedPlatformIds = other.AllowedPlatformIds;
	}
}
