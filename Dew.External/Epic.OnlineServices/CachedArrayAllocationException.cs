using System;

namespace Epic.OnlineServices;

internal class CachedArrayAllocationException : AllocationException
{
	public CachedArrayAllocationException(IntPtr address, int foundLength, int expectedLength)
		: base(string.Format("Cached array allocation has length {0} but expected {1} at {2}", foundLength, expectedLength, address.ToString("X")))
	{
	}
}
