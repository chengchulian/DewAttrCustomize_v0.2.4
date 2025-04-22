using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Settings_View : View
{
	public UI_ToggleGroup toggleGroup;

	[NonSerialized]
	public DewGameplaySettings_User gameplayUser;

	[NonSerialized]
	public DewGameplaySettings_Platform gameplayPlatform;

	[NonSerialized]
	public DewControlSettings controls;

	[NonSerialized]
	public DewGraphicsSettings graphics;

	[NonSerialized]
	public DewAudioSettings_User audioUser;

	[NonSerialized]
	public DewAudioSettings_Platform audioPlatform;

	private bool _isDirty;

	public TextMeshProUGUI tooltipText;

	public GameObject[] categoryObjects;

	public Button applyButton;

	public Button undoButton;

	public Button backButton;

	public UnityEvent onGoBack;

	internal bool _resetTutorial;

	protected override void Awake()
	{
		base.Awake();
		if (Application.IsPlaying(this))
		{
			toggleGroup.onCurrentIndexChanged.AddListener(delegate
			{
				UpdateCategoryObjects();
			});
			applyButton.onClick.AddListener(ApplySettings);
			undoButton.onClick.AddListener(UndoChanges);
			backButton.onClick.AddListener(ClickGoBack);
			CloneSettings();
		}
	}

	protected override void Start()
	{
		base.Start();
		if (Application.IsPlaying(this))
		{
			UI_Settings_Item[] componentsInChildren = GetComponentsInChildren<UI_Settings_Item>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Init();
			}
		}
	}

	protected override void OnShow()
	{
		base.OnShow();
		toggleGroup.currentIndex = 0;
		CloneSettings();
		UpdateCategoryObjects();
		HideTooltip();
	}

	public void UpdateCategoryObjects()
	{
		for (int i = 0; i < categoryObjects.Length; i++)
		{
			categoryObjects[i].SetActive(value: false);
			if (toggleGroup.currentIndex != i)
			{
				continue;
			}
			categoryObjects[i].SetActive(value: true);
			UI_Settings_Item[] items = categoryObjects[i].GetComponentsInChildren<UI_Settings_Item>();
			UI_Settings_Item[] array = items;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].OnLoad();
			}
			if (DewInput.currentMode == InputMode.Gamepad)
			{
				items.Sort((UI_Settings_Item x, UI_Settings_Item y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));
				if (items.Length != 0)
				{
					ManagerBase<GlobalUIManager>.instance.SetFocus(items[0]);
				}
			}
		}
		HideTooltip();
	}

	private void CloneSettings()
	{
		TryClone(DewSave.profile.gameplay, out gameplayUser);
		TryClone(DewSave.profile.controls, out controls);
		TryClone(DewSave.platformSettings.graphics, out graphics);
		TryClone(DewSave.profile.audio, out audioUser);
		TryClone(DewSave.platformSettings.audio, out audioPlatform);
		TryClone(DewSave.platformSettings.gameplay, out gameplayPlatform);
		_isDirty = false;
		applyButton.interactable = false;
		undoButton.gameObject.SetActive(value: false);
	}

	private void TryClone<T>(T from, out T to) where T : ICloneable, new()
	{
		try
		{
			to = (T)from.Clone();
		}
		catch (Exception)
		{
			Debug.LogWarning("Cloning current " + typeof(T).Name + " failed, falling back to default settings");
			to = new T();
		}
	}

	public void ApplySettings()
	{
		DewSave.profile.gameplay = gameplayUser;
		DewSave.profile.controls = controls;
		DewSave.platformSettings.graphics = graphics;
		DewSave.profile.audio = audioUser;
		DewSave.platformSettings.audio = audioPlatform;
		DewSave.platformSettings.gameplay = gameplayPlatform;
		if (_resetTutorial)
		{
			_resetTutorial = false;
			if (ManagerBase<InGameTutorialManager>.instance != null)
			{
				ManagerBase<InGameTutorialManager>.instance.StopTutorials();
			}
			DewSave.profile.doneTutorials = new List<string>();
			if (ManagerBase<InGameTutorialManager>.instance != null)
			{
				ManagerBase<InGameTutorialManager>.instance.StartTutorials();
			}
			DewSave.profile.didPlayTutorial = false;
		}
		DewSave.SaveProfile();
		DewSave.SavePlatformSettings();
		DewSave.ApplySettings();
		CloneSettings();
	}

	public void UndoChanges()
	{
		CloneSettings();
		UpdateCategoryObjects();
	}

	private void ClickGoBack()
	{
		if (_isDirty)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				validator = () => base.isShowing,
				rawContent = DewLocalization.GetUIValue("Settings_ConfirmUnsavedChanges"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
				defaultButton = DewMessageSettings.ButtonType.No,
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						UndoChanges();
						onGoBack.Invoke();
					}
				}
			});
		}
		else
		{
			onGoBack.Invoke();
		}
	}

	public void MarkAsDirty()
	{
		_isDirty = true;
		applyButton.interactable = true;
		undoButton.gameObject.SetActive(value: true);
	}

	public void ResetToDefaults()
	{
		UI_Settings_Item[] componentsInChildren = categoryObjects[toggleGroup.currentIndex].GetComponentsInChildren<UI_Settings_Item>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].LoadDefaults();
		}
		MarkAsDirty();
	}

	public void HideTooltip()
	{
		tooltipText.text = "";
	}

	public void ShowTooltip(UI_Settings_Item.CategoryType category, string key)
	{
		if (DewLocalization.TryGetUIValue($"Settings_{category}_{key}_Description", out var value))
		{
			ShowTooltipRaw(value);
		}
		else
		{
			ShowTooltipRaw("");
		}
	}

	public void ShowTooltip(UI_Settings_Item.CategoryType category, string key, string typeKey)
	{
		string text = "";
		if (DewLocalization.TryGetUIValue($"Settings_{category}_{key}_Description", out var value))
		{
			text = value;
		}
		if (DewLocalization.TryGetUIValue("Settings_" + typeKey + "_Description", out var typeValue))
		{
			text = ((!(text == "")) ? (text + "\n\n" + typeValue) : typeValue);
		}
		ShowTooltipRaw(text);
	}

	public void ShowTooltipRaw(string text)
	{
		text = text.Replace("[", "<b><color=yellow>");
		text = text.Replace("]", "</color></b>");
		tooltipText.text = text;
	}
}
