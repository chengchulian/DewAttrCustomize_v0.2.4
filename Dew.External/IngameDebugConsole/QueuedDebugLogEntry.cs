using System.Globalization;
using UnityEngine;

namespace IngameDebugConsole;

public struct QueuedDebugLogEntry
{
	public readonly string logString;

	public readonly string stackTrace;

	public readonly LogType logType;

	public QueuedDebugLogEntry(string logString, string stackTrace, LogType logType)
	{
		this.logString = logString;
		this.stackTrace = stackTrace;
		this.logType = logType;
	}

	public bool MatchesSearchTerm(string searchTerm)
	{
		if (logString == null || DebugLogConsole.caseInsensitiveComparer.IndexOf(logString, searchTerm, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) < 0)
		{
			if (stackTrace != null)
			{
				return DebugLogConsole.caseInsensitiveComparer.IndexOf(stackTrace, searchTerm, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
			}
			return false;
		}
		return true;
	}
}
