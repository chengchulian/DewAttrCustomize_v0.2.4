using System;
using System.Diagnostics;
using UnityEngine;

namespace FIMSpace;

public static class FEditor_OneShotLog
{
	public static bool CanDrawLog(string id, int delayToNextCallInSeconds = int.MaxValue, int callLimitBeforeTimeMove = 1, int logSeparation = 0)
	{
		int session = Process.GetCurrentProcess().Id;
		if (PlayerPrefs.GetInt(id + "s", 0) != session)
		{
			PlayerPrefs.SetInt(id + "s", session);
			PlayerPrefs.SetString(id + "acc", DateTime.Now.ToBinary().ToString());
			PlayerPrefs.SetInt(id + "counter", 0);
			PlayerPrefs.SetInt(id + "sep", logSeparation);
			if (delayToNextCallInSeconds == int.MaxValue)
			{
				return true;
			}
		}
		else if (delayToNextCallInSeconds == int.MaxValue)
		{
			return false;
		}
		string @string = PlayerPrefs.GetString(id + "acc");
		int errorCounter = PlayerPrefs.GetInt(id + "counter");
		int separations = PlayerPrefs.GetInt(id + "sep");
		if (long.TryParse(@string, out var dateBin))
		{
			DateTime lastAccessTime = DateTime.FromBinary(dateBin);
			if (DateTime.Now.Subtract(lastAccessTime).TotalSeconds > (double)delayToNextCallInSeconds)
			{
				PlayerPrefs.SetInt(id + "counter", 0);
				errorCounter = 0;
				PlayerPrefs.SetString(id + "acc", DateTime.Now.ToBinary().ToString());
			}
			separations++;
			PlayerPrefs.SetInt(id + "sep", separations);
			if (separations >= logSeparation)
			{
				separations = 0;
				PlayerPrefs.SetInt(id + "sep", separations);
				errorCounter++;
				PlayerPrefs.SetInt(id + "counter", errorCounter);
				if (errorCounter - 1 < callLimitBeforeTimeMove)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public static bool EditorCanDrawLog(string id, int delayToNextCallInSeconds = int.MaxValue, int callLimitBeforeTimeMove = 1, int logSeparation = 0)
	{
		return false;
	}
}
