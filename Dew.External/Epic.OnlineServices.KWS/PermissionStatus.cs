namespace Epic.OnlineServices.KWS;

public struct PermissionStatus
{
	public Utf8String Name { get; set; }

	public KWSPermissionStatus Status { get; set; }

	internal void Set(ref PermissionStatusInternal other)
	{
		Name = other.Name;
		Status = other.Status;
	}
}
