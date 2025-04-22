using System;

namespace CI.QuickSave;

public class QuickSaveException : Exception
{
	public QuickSaveException()
	{
	}

	public QuickSaveException(string message)
		: base(message)
	{
	}

	public QuickSaveException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
