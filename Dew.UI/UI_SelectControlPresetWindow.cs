using UnityEngine;

public class UI_SelectControlPresetWindow : SingletonBehaviour<UI_SelectControlPresetWindow>, IControlPresetWindow
{
	private Canvas _canvas;

	public GameObject cancelButton;

	private UI_ToggleGroup _group;

	protected override void Awake()
	{
		base.Awake();
		if (Application.IsPlaying(this))
		{
			_group = GetComponentInChildren<UI_ToggleGroup>();
			_canvas = GetComponent<Canvas>();
			_canvas.enabled = false;
			base.gameObject.SetActive(value: false);
		}
	}

	public void Show(bool showCancel)
	{
		if (DewInput.currentMode != InputMode.Gamepad)
		{
			cancelButton.SetActive(showCancel);
			_group.currentIndex = (DewSave.profile.controls.enableDirMoveKeys ? 2 : 0);
			_canvas.enabled = true;
			base.gameObject.SetActive(value: true);
		}
	}

	void IControlPresetWindow.Hide()
	{
		Close();
	}

	public bool IsShown()
	{
		return base.gameObject.activeInHierarchy;
	}

	public void Close()
	{
		_canvas.enabled = false;
		base.gameObject.SetActive(value: false);
	}

	public void ConfirmControlScheme()
	{
		if (IsInSettingsView(out var _, out var _))
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				rawContent = DewLocalization.GetUIValue("ControlPreset_Message_ConfirmSettingsChange"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				destructiveConfirm = true,
				onClose = delegate(DewMessageSettings.ButtonType button)
				{
					if (button == DewMessageSettings.ButtonType.Yes)
					{
						ApplyPreset();
						Close();
					}
				}
			});
		}
		else
		{
			ApplyPreset();
			Close();
		}
	}

	private bool IsInSettingsView(out DewControlSettings editing, out UI_Settings_View view)
	{
		UI_Settings_View[] array = Object.FindObjectsOfType<UI_Settings_View>(includeInactive: true);
		foreach (UI_Settings_View v in array)
		{
			if (v.isShowing)
			{
				editing = v.controls;
				view = v;
				return true;
			}
		}
		editing = null;
		view = null;
		return false;
	}

	private void ApplyPreset()
	{
		DewControlSettings settings;
		UI_Settings_View view;
		bool num = IsInSettingsView(out settings, out view);
		if (!num)
		{
			settings = DewSave.profile.controls;
		}
		settings.ApplyPreset((_group.currentIndex != 0) ? DewControlSettings.PresetType.WASD : DewControlSettings.PresetType.MOBA);
		if (!num)
		{
			DewSave.SaveProfile();
			DewSave.ApplySettings();
		}
		else
		{
			view.MarkAsDirty();
			view.UpdateCategoryObjects();
		}
	}

	public void Cancel()
	{
		Close();
	}
}
