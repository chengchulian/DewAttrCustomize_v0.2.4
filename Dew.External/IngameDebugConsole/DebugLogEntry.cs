using System.Globalization;
using UnityEngine;

namespace IngameDebugConsole;

public class DebugLogEntry
{
	private const int HASH_NOT_CALCULATED = -623218;

	public string logString;

	public string stackTrace;

	private string completeLog;

	public Sprite logTypeSpriteRepresentation;

	public int count;

	public int collapsedIndex;

	private int hashValue;

	public void Initialize(string logString, string stackTrace)
	{
		this.logString = logString;
		this.stackTrace = stackTrace;
		completeLog = null;
		count = 1;
		hashValue = -623218;
	}

	public void Clear()
	{
		logString = null;
		stackTrace = null;
		completeLog = null;
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

	public override string ToString()
	{
		if (completeLog == null)
		{
			completeLog = logString + "\n" + stackTrace;
		}
		return completeLog;
	}

	public int GetContentHashCode()
	{
		if (hashValue == -623218)
		{
			hashValue = 17;
			hashValue = hashValue * 23 + ((logString != null) ? logString.GetHashCode() : 0);
			hashValue = hashValue * 23 + ((stackTrace != null) ? stackTrace.GetHashCode() : 0);
		}
		return hashValue;
	}
}
