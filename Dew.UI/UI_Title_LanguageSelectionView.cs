using System;
using UnityEngine;

public class UI_Title_LanguageSelectionView : View
{
	public string[] languages;

	public UI_Title_LanguageSelection_Item itemPrefab;

	public UI_ToggleGroup toggleGroup;

	public Transform itemParent;

	public GameObject cancelButton;

	protected override void Start()
	{
		base.Start();
		if (Application.IsPlaying(this))
		{
			int recommendedIndex = Array.IndexOf(languages, DewLocalization.GetRecommendedSupportedLanguage());
			if (recommendedIndex != 0)
			{
				ref string reference = ref languages[recommendedIndex];
				ref string reference2 = ref languages[0];
				string text = languages[0];
				string text2 = languages[recommendedIndex];
				reference = text;
				reference2 = text2;
			}
			for (int i = 0; i < languages.Length; i++)
			{
				global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).Setup(languages[i], i);
			}
			toggleGroup.currentIndex = 0;
		}
	}

	protected override void OnShow()
	{
		base.OnShow();
		int index = Array.IndexOf(languages, DewSave.profile.language);
		if (index < 0 || index >= languages.Length)
		{
			index = 0;
		}
		toggleGroup.currentIndex = index;
	}

	public void ShowWithCancel()
	{
		cancelButton.SetActive(value: true);
		Show();
	}

	public void ShowWithoutCancel()
	{
		cancelButton.SetActive(value: false);
		Show();
	}

	public void ApplyLanguage()
	{
		DewSave.profile.language = languages[toggleGroup.currentIndex];
		DewSave.SaveProfile();
		DewSave.ApplySettings();
		Hide();
	}
}
