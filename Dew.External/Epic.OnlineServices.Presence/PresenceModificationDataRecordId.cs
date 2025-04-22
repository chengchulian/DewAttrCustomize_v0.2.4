namespace Epic.OnlineServices.Presence;

public struct PresenceModificationDataRecordId
{
	public Utf8String Key { get; set; }

	internal void Set(ref PresenceModificationDataRecordIdInternal other)
	{
		Key = other.Key;
	}
}
