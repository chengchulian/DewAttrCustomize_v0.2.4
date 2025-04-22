using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltip_BaseObj : MonoBehaviour
{
	public int objIndex;

	public IReadOnlyList<object> currentObjects;

	[NonSerialized]
	public TextMeshProUGUI text;

	public object currentObject
	{
		get
		{
			if (currentObjects == null || objIndex < 0 || objIndex >= currentObjects.Count)
			{
				return null;
			}
			return currentObjects[objIndex];
		}
	}

	public virtual void OnSetup()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
}
