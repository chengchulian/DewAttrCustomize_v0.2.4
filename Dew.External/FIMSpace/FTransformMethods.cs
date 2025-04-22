using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace;

public static class FTransformMethods
{
	public static Transform FindChildByNameInDepth(string name, Transform transform, bool findInDeactivated = true, string[] additionalContains = null)
	{
		if (transform.name == name)
		{
			return transform;
		}
		Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>(findInDeactivated);
		foreach (Transform child in componentsInChildren)
		{
			if (!child.name.ToLower().Contains(name.ToLower()))
			{
				continue;
			}
			bool allow = false;
			if (additionalContains == null || additionalContains.Length == 0)
			{
				allow = true;
			}
			else
			{
				for (int j = 0; j < additionalContains.Length; j++)
				{
					if (child.name.ToLower().Contains(additionalContains[j].ToLower()))
					{
						allow = true;
						break;
					}
				}
			}
			if (allow)
			{
				return child;
			}
		}
		return null;
	}

	public static List<T> FindComponentsInAllChildren<T>(Transform transformToSearchIn, bool includeInactive = false, bool tryGetMultipleOutOfSingleObject = false) where T : Component
	{
		List<T> components = new List<T>();
		T[] components2 = transformToSearchIn.GetComponents<T>();
		foreach (T child in components2)
		{
			if ((bool)child)
			{
				components.Add(child);
			}
		}
		Transform[] componentsInChildren = transformToSearchIn.GetComponentsInChildren<Transform>(includeInactive);
		foreach (Transform child2 in componentsInChildren)
		{
			if (!tryGetMultipleOutOfSingleObject)
			{
				T component = child2.GetComponent<T>();
				if ((bool)component && !components.Contains(component))
				{
					components.Add(component);
				}
				continue;
			}
			components2 = child2.GetComponents<T>();
			foreach (T component2 in components2)
			{
				if ((bool)component2 && !components.Contains(component2))
				{
					components.Add(component2);
				}
			}
		}
		return components;
	}

	public static T FindComponentInAllChildren<T>(Transform transformToSearchIn) where T : Component
	{
		Transform[] componentsInChildren = transformToSearchIn.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			T component = componentsInChildren[i].GetComponent<T>();
			if ((bool)component)
			{
				return component;
			}
		}
		return null;
	}

	public static T FindComponentInAllParents<T>(Transform transformToSearchIn) where T : Component
	{
		Transform p = transformToSearchIn.parent;
		for (int i = 0; i < 100; i++)
		{
			T component = p.GetComponent<T>();
			if ((bool)component)
			{
				return component;
			}
			p = p.parent;
			if (p == null)
			{
				return null;
			}
		}
		return null;
	}

	public static void ChangeActiveChildrenInside(Transform parentOfThem, bool active)
	{
		for (int i = 0; i < parentOfThem.childCount; i++)
		{
			parentOfThem.GetChild(i).gameObject.SetActive(active);
		}
	}

	public static void ChangeActiveThroughParentTo(Transform start, Transform end, bool active, bool changeParentsChildrenActivation = false)
	{
		start.gameObject.SetActive(active);
		Transform p = start.parent;
		for (int i = 0; i < 100; i++)
		{
			if (p == end)
			{
				break;
			}
			if (p == null)
			{
				break;
			}
			if (changeParentsChildrenActivation)
			{
				ChangeActiveChildrenInside(p, active);
			}
			p = p.parent;
		}
	}
}
