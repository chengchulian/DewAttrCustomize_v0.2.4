using System;
using System.Reflection;
using UnityEngine;

public class UI_Settings_ItemSpawner : MonoBehaviour
{
	public UI_Settings_Item.CategoryType type;

	public string[] keys;

	public UI_Settings_Item dewBindingPrefab;

	public UI_Settings_Item enumPrefab;

	public UI_Settings_Item boolPrefab;

	private void Awake()
	{
		Type configType = type switch
		{
			UI_Settings_Item.CategoryType.Gameplay => typeof(DewGameplaySettings_User), 
			UI_Settings_Item.CategoryType.Controls => typeof(DewControlSettings), 
			UI_Settings_Item.CategoryType.Graphics => typeof(DewGraphicsSettings), 
			UI_Settings_Item.CategoryType.AudioUser => typeof(DewAudioSettings_User), 
			UI_Settings_Item.CategoryType.AudioPlatform => typeof(DewAudioSettings_Platform), 
			UI_Settings_Item.CategoryType.GameplayPlatform => typeof(DewGameplaySettings_Platform), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		int sibIndex = base.transform.GetSiblingIndex();
		for (int i = keys.Length - 1; i >= 0; i--)
		{
			FieldInfo f = configType.GetField(keys[i]);
			if (f == null)
			{
				Debug.LogWarning("Field not found: " + configType.Name + "::" + keys[i]);
				continue;
			}
			UI_Settings_Item prefab;
			if (f.FieldType == typeof(bool))
			{
				prefab = boolPrefab;
			}
			else if (f.FieldType == typeof(DewBinding))
			{
				prefab = dewBindingPrefab;
			}
			else
			{
				if (!f.FieldType.IsEnum)
				{
					Debug.LogWarning("Unknown type: " + f.FieldType.Name + " " + configType.Name + "::" + keys[i]);
					continue;
				}
				prefab = enumPrefab;
			}
			UI_Settings_Item uI_Settings_Item = global::UnityEngine.Object.Instantiate(prefab, base.transform.parent);
			uI_Settings_Item.transform.SetSiblingIndex(sibIndex);
			uI_Settings_Item.type = type;
			uI_Settings_Item.key = keys[i];
			uI_Settings_Item.Init();
		}
	}
}
