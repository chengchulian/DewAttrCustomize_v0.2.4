using System;
using UnityEngine;

namespace HighlightPlus;

public class Misc
{
	public static T FindObjectOfType<T>(bool includeInactive = false) where T : global::UnityEngine.Object
	{
		return global::UnityEngine.Object.FindObjectOfType<T>(includeInactive);
	}

	public static global::UnityEngine.Object[] FindObjectsOfType(Type type, bool includeInactive = false)
	{
		return global::UnityEngine.Object.FindObjectsOfType(type, includeInactive);
	}

	public static T[] FindObjectsOfType<T>(bool includeInactive = false) where T : global::UnityEngine.Object
	{
		return global::UnityEngine.Object.FindObjectsOfType<T>(includeInactive);
	}
}
