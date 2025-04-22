using System;

namespace UnityJSON;

public class DeserializationException : Exception
{
	public DeserializationException()
	{
	}

	public DeserializationException(string message)
		: base(message)
	{
	}
}
