namespace Epic.OnlineServices.Platform;

public struct DesktopCrossplayStatusInfo
{
	public DesktopCrossplayStatus Status { get; set; }

	public int ServiceInitResult { get; set; }

	internal void Set(ref DesktopCrossplayStatusInfoInternal other)
	{
		Status = other.Status;
		ServiceInitResult = other.ServiceInitResult;
	}
}
