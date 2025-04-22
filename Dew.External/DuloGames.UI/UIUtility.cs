using System;
using UnityEngine;

namespace DuloGames.UI;

public static class UIUtility
{
	public static void BringToFront(GameObject go)
	{
		BringToFront(go, allowReparent: true);
	}

	public static void BringToFront(GameObject go, bool allowReparent)
	{
		Transform root = null;
		UIScene scene = FindInParents<UIScene>(go);
		if (scene != null && scene.content != null)
		{
			root = scene.content;
		}
		else
		{
			Canvas canvas = FindInParents<Canvas>(go);
			if (canvas != null)
			{
				root = canvas.transform;
			}
		}
		if (allowReparent && root != null)
		{
			go.transform.SetParent(root, worldPositionStays: true);
		}
		go.transform.SetAsLastSibling();
		if (!(root != null))
		{
			return;
		}
		UIAlwaysOnTop[] alwaysOnTopComponenets = root.gameObject.GetComponentsInChildren<UIAlwaysOnTop>();
		if (alwaysOnTopComponenets.Length != 0)
		{
			Array.Sort(alwaysOnTopComponenets);
			UIAlwaysOnTop[] array = alwaysOnTopComponenets;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.SetAsLastSibling();
			}
		}
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return null;
		}
		T comp = go.GetComponent<T>();
		if (comp != null)
		{
			return comp;
		}
		Transform t = go.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}
}
