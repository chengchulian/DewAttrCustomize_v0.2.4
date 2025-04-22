namespace Epic.OnlineServices.RTCAudio;

public struct AudioOutputDeviceInfo
{
	public bool DefaultDevice { get; set; }

	public Utf8String DeviceId { get; set; }

	public Utf8String DeviceName { get; set; }

	internal void Set(ref AudioOutputDeviceInfoInternal other)
	{
		DefaultDevice = other.DefaultDevice;
		DeviceId = other.DeviceId;
		DeviceName = other.DeviceName;
	}
}
