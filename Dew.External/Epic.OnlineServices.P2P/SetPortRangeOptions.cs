namespace Epic.OnlineServices.P2P;

public struct SetPortRangeOptions
{
	public ushort Port { get; set; }

	public ushort MaxAdditionalPortsToTry { get; set; }
}
