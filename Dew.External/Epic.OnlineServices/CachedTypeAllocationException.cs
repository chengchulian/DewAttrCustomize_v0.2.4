using System;

namespace Epic.OnlineServices;

internal class CachedTypeAllocationException : AllocationException
{
	public CachedTypeAllocationException(IntPtr address, Type foundType, Type expectedType)
		: base(string.Format("Cached allocation is '{0}' but expected '{1}' at {2}", foundType, expectedType, address.ToString("X")))
	{
	}
}
