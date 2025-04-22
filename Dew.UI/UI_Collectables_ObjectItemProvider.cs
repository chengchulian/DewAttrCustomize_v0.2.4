using System;
using UnityEngine;

public abstract class UI_Collectables_ObjectItemProvider : MonoBehaviour
{
	[NonSerialized]
	public global::UnityEngine.Object targetObj;

	public virtual void OnSetup(global::UnityEngine.Object obj, UnlockStatus status, int index)
	{
	}

	public abstract DewProfile.UnlockData GetUnlockData();

	public abstract Color GetRarityColor();

	public abstract Sprite GetIcon();
}
