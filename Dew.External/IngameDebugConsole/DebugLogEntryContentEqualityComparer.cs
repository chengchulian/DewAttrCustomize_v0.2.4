using System.Collections.Generic;

namespace IngameDebugConsole;

public class DebugLogEntryContentEqualityComparer : EqualityComparer<DebugLogEntry>
{
	public override bool Equals(DebugLogEntry x, DebugLogEntry y)
	{
		if (x.logString == y.logString)
		{
			return x.stackTrace == y.stackTrace;
		}
		return false;
	}

	public override int GetHashCode(DebugLogEntry obj)
	{
		return obj.GetContentHashCode();
	}
}
