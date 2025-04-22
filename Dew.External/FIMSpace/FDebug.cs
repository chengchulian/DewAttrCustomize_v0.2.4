using System.Diagnostics;
using UnityEngine;

namespace FIMSpace;

public static class FDebug
{
	private static readonly Stopwatch _debugWatch = new Stopwatch();

	public static long _LastMeasureMilliseconds = 0L;

	public static long _LastMeasureTicks = 0L;

	public static void Log(string log)
	{
		global::UnityEngine.Debug.Log("LOG: " + log);
	}

	public static void Log(string log, string category)
	{
		global::UnityEngine.Debug.Log(MarkerColor("#1A6600") + "[" + category + "]" + EndColorMarker() + " " + log);
	}

	public static void LogRed(string log)
	{
		global::UnityEngine.Debug.Log(MarkerColor("red") + log + EndColorMarker());
	}

	public static void LogOrange(string log)
	{
		global::UnityEngine.Debug.Log(MarkerColor("#D1681D") + log + EndColorMarker());
	}

	public static void LogYellow(string log)
	{
		global::UnityEngine.Debug.Log(MarkerColor("#E0D300") + log + EndColorMarker());
	}

	public static void StartMeasure()
	{
		_debugWatch.Reset();
		_debugWatch.Start();
	}

	public static void PauseMeasure()
	{
		_debugWatch.Stop();
	}

	public static void ResumeMeasure()
	{
		_debugWatch.Start();
	}

	public static void EndMeasureAndLog(string v)
	{
		_debugWatch.Stop();
		_LastMeasureMilliseconds = _debugWatch.ElapsedMilliseconds;
		_LastMeasureTicks = _debugWatch.ElapsedTicks;
		global::UnityEngine.Debug.Log("Measure " + v + ": " + _debugWatch.ElapsedTicks + " ticks   " + _debugWatch.ElapsedMilliseconds + "ms");
	}

	public static long EndMeasureAndGetTicks()
	{
		_debugWatch.Stop();
		_LastMeasureMilliseconds = _debugWatch.ElapsedMilliseconds;
		_LastMeasureTicks = _debugWatch.ElapsedTicks;
		return _debugWatch.ElapsedTicks;
	}

	public static string MarkerColor(string color)
	{
		return "<color='" + color + "'>";
	}

	public static string EndColorMarker()
	{
		return "</color>";
	}

	public static void DrawBounds2D(this Bounds b, Color c, float y = 0f, float scale = 1f, float duration = 1.1f)
	{
		Vector3 fr1 = new Vector3(b.max.x, y, b.max.z) * scale;
		Vector3 br1 = new Vector3(b.max.x, y, b.min.z) * scale;
		Vector3 bl1 = new Vector3(b.min.x, y, b.min.z) * scale;
		Vector3 fl1 = new Vector3(b.min.x, y, b.max.z) * scale;
		global::UnityEngine.Debug.DrawLine(fr1, br1, c, duration);
		global::UnityEngine.Debug.DrawLine(br1, bl1, c, duration);
		global::UnityEngine.Debug.DrawLine(br1, bl1, c, duration);
		global::UnityEngine.Debug.DrawLine(bl1, fl1, c, duration);
		global::UnityEngine.Debug.DrawLine(fl1, fr1, c, duration);
	}

	public static void DrawBounds3D(this Bounds b, Color c, float scale = 1f, float time = 1.01f)
	{
		Vector3 fr1 = new Vector3(b.max.x, b.min.y, b.max.z) * scale;
		Vector3 br1 = new Vector3(b.max.x, b.min.y, b.min.z) * scale;
		Vector3 bl1 = new Vector3(b.min.x, b.min.y, b.min.z) * scale;
		Vector3 fl1 = new Vector3(b.min.x, b.min.y, b.max.z) * scale;
		global::UnityEngine.Debug.DrawLine(fr1, br1, c, time);
		global::UnityEngine.Debug.DrawLine(br1, bl1, c, time);
		global::UnityEngine.Debug.DrawLine(br1, bl1, c, time);
		global::UnityEngine.Debug.DrawLine(bl1, fl1, c, time);
		global::UnityEngine.Debug.DrawLine(fl1, fr1, c, time);
		Vector3 fr2 = new Vector3(b.max.x, b.max.y, b.max.z) * scale;
		Vector3 br2 = new Vector3(b.max.x, b.max.y, b.min.z) * scale;
		Vector3 bl2 = new Vector3(b.min.x, b.max.y, b.min.z) * scale;
		Vector3 fl2 = new Vector3(b.min.x, b.max.y, b.max.z) * scale;
		global::UnityEngine.Debug.DrawLine(fr2, br2, c, time);
		global::UnityEngine.Debug.DrawLine(br2, bl2, c, time);
		global::UnityEngine.Debug.DrawLine(br2, bl2, c, time);
		global::UnityEngine.Debug.DrawLine(bl2, fl2, c, time);
		global::UnityEngine.Debug.DrawLine(fl2, fr2, c, time);
		global::UnityEngine.Debug.DrawLine(fr1, fr1, c, time);
		global::UnityEngine.Debug.DrawLine(br2, br1, c, time);
		global::UnityEngine.Debug.DrawLine(bl1, bl2, c, time);
		global::UnityEngine.Debug.DrawLine(fl1, fl2, c, time);
	}
}
