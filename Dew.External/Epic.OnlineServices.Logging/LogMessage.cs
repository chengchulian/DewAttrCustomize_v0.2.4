namespace Epic.OnlineServices.Logging;

public struct LogMessage
{
	public Utf8String Category { get; set; }

	public Utf8String Message { get; set; }

	public LogLevel Level { get; set; }

	internal void Set(ref LogMessageInternal other)
	{
		Category = other.Category;
		Message = other.Message;
		Level = other.Level;
	}
}
