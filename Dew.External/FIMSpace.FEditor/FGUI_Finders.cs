using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FEditor;

public static class FGUI_Finders
{
	public static Component FoundAnimator;

	private static bool checkForAnim = true;

	private static int clicks = 0;

	public static void ResetFinders(bool resetClicks = true)
	{
		checkForAnim = true;
		FoundAnimator = null;
		if (resetClicks)
		{
			clicks = 0;
		}
	}

	public static bool CheckForAnimator(GameObject root, bool needAnimatorBox = true, bool drawInactiveWarning = true, int clicksTohide = 1)
	{
		bool working = false;
		if (checkForAnim)
		{
			FoundAnimator = SearchForParentWithAnimator(root);
		}
		if ((bool)FoundAnimator)
		{
			Animation legacy = FoundAnimator as Animation;
			Animator mec = FoundAnimator as Animator;
			if ((bool)legacy && legacy.enabled)
			{
				working = true;
			}
			if ((bool)mec)
			{
				if (mec.enabled)
				{
					working = true;
				}
				if (mec.runtimeAnimatorController == null)
				{
					drawInactiveWarning = false;
					working = false;
				}
			}
			if (needAnimatorBox && drawInactiveWarning && working)
			{
			}
		}
		else if (needAnimatorBox)
		{
			_ = clicks;
		}
		checkForAnim = false;
		return working;
	}

	public static Component SearchForParentWithAnimator(GameObject root)
	{
		Animation animation = root.GetComponentInChildren<Animation>();
		if ((bool)animation)
		{
			return animation;
		}
		Animator animator = root.GetComponentInChildren<Animator>();
		if ((bool)animator)
		{
			return animator;
		}
		if (root.transform.parent != null)
		{
			Transform pr = root.transform.parent;
			while (pr != null)
			{
				animation = pr.GetComponent<Animation>();
				if ((bool)animation)
				{
					return animation;
				}
				animator = pr.GetComponent<Animator>();
				if ((bool)animator)
				{
					return animator;
				}
				pr = pr.parent;
			}
		}
		return null;
	}

	public static SkinnedMeshRenderer GetBoneSearchArray(Transform root)
	{
		List<SkinnedMeshRenderer> skins = new List<SkinnedMeshRenderer>();
		SkinnedMeshRenderer largestSkin = null;
		Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			SkinnedMeshRenderer s = componentsInChildren[i].GetComponent<SkinnedMeshRenderer>();
			if ((bool)s)
			{
				skins.Add(s);
			}
		}
		if (skins.Count == 0)
		{
			Transform lastParent = root;
			while (lastParent != null && !(lastParent.parent == null))
			{
				lastParent = lastParent.parent;
			}
			componentsInChildren = lastParent.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				SkinnedMeshRenderer s2 = componentsInChildren[i].GetComponent<SkinnedMeshRenderer>();
				if (!skins.Contains(s2) && (bool)s2)
				{
					skins.Add(s2);
				}
			}
		}
		if (skins.Count > 1)
		{
			largestSkin = skins[0];
			for (int j = 1; j < skins.Count; j++)
			{
				if (skins[j].bones.Length > largestSkin.bones.Length)
				{
					largestSkin = skins[j];
				}
			}
		}
		else if (skins.Count > 0)
		{
			largestSkin = skins[0];
		}
		if (largestSkin == null)
		{
			return null;
		}
		return largestSkin;
	}

	public static bool IsChildOf(Transform child, Transform rootParent)
	{
		Transform tParent = child;
		while (tParent != null && tParent != rootParent)
		{
			tParent = tParent.parent;
		}
		if (tParent == null)
		{
			return false;
		}
		return true;
	}

	public static Transform GetLastChild(Transform rootParent)
	{
		Transform tChild = rootParent;
		while (tChild.childCount > 0)
		{
			tChild = tChild.GetChild(0);
		}
		return tChild;
	}

	public static bool? IsRightOrLeft(string name, bool includeNotSure = false)
	{
		string nameLower = name.ToLower();
		if (nameLower.Contains("right"))
		{
			return true;
		}
		if (nameLower.Contains("left"))
		{
			return false;
		}
		if (nameLower.StartsWith("r_"))
		{
			return true;
		}
		if (nameLower.StartsWith("l_"))
		{
			return false;
		}
		if (nameLower.EndsWith("_r"))
		{
			return true;
		}
		if (nameLower.EndsWith("_l"))
		{
			return false;
		}
		if (nameLower.StartsWith("r."))
		{
			return true;
		}
		if (nameLower.StartsWith("l."))
		{
			return false;
		}
		if (nameLower.EndsWith(".r"))
		{
			return true;
		}
		if (nameLower.EndsWith(".l"))
		{
			return false;
		}
		if (includeNotSure)
		{
			if (nameLower.Contains("r_"))
			{
				return true;
			}
			if (nameLower.Contains("l_"))
			{
				return false;
			}
			if (nameLower.Contains("_r"))
			{
				return true;
			}
			if (nameLower.Contains("_l"))
			{
				return false;
			}
			if (nameLower.Contains("r."))
			{
				return true;
			}
			if (nameLower.Contains("l."))
			{
				return false;
			}
			if (nameLower.Contains(".r"))
			{
				return true;
			}
			if (nameLower.Contains(".l"))
			{
				return false;
			}
		}
		return null;
	}

	public static bool? IsRightOrLeft(Transform child, Transform itsRoot)
	{
		Vector3 transformed = itsRoot.InverseTransformPoint(child.position);
		if (transformed.x < 0f)
		{
			return false;
		}
		if (transformed.x > 0f)
		{
			return true;
		}
		return null;
	}

	public static bool HaveKey(string text, string[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			if (text.Contains(keys[i]))
			{
				return true;
			}
		}
		return false;
	}
}
