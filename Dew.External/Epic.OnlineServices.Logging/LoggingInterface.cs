namespace Epic.OnlineServices.Logging;

public sealed class LoggingInterface
{
	public static Result SetCallback(LogMessageFunc callback)
	{
		LogMessageFuncInternal callbackInternal = LogMessageFuncInternalImplementation;
		Helper.AddStaticCallback("LogMessageFuncInternalImplementation", callback, callbackInternal);
		return Bindings.EOS_Logging_SetCallback(callbackInternal);
	}

	public static Result SetLogLevel(LogCategory logCategory, LogLevel logLevel)
	{
		return Bindings.EOS_Logging_SetLogLevel(logCategory, logLevel);
	}

	[MonoPInvokeCallback(typeof(LogMessageFuncInternal))]
	internal static void LogMessageFuncInternalImplementation(ref LogMessageInternal message)
	{
		if (Helper.TryGetStaticCallback<LogMessageFunc>("LogMessageFuncInternalImplementation", out var callback))
		{
			Helper.Get<LogMessageInternal, LogMessage>(ref message, out var messageObj);
			callback(ref messageObj);
		}
	}
}
