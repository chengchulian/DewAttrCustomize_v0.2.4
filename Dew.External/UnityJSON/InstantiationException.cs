using System;

namespace UnityJSON;

public class InstantiationException : Exception
{
	public InstantiationException()
	{
	}

	public InstantiationException(string message)
		: base(message)
	{
	}
}
