using System;

namespace Epic.OnlineServices;

public static class Extensions
{
	public static bool IsOperationComplete(this Result result)
	{
		return Common.IsOperationComplete(result);
	}

	public static string ToHexString(this byte[] byteArray)
	{
		return Common.ToString(new ArraySegment<byte>(byteArray));
	}
}
