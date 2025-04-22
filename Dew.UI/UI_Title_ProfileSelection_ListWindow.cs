using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title_ProfileSelection_ListWindow : UI_Title_ProfileSelection_Window
{
	public Transform itemsParent;

	public UI_Title_ProfileSelection_ListWindow_Item itemPrefab;

	public Button deleteButton;

	public Button loadButton;

	public Button cancelButton;

	public Button newProfileButton;

	public TextMeshProUGUI loadButtonText;

	private List<DewProfileItem> _profiles;

	private UI_ToggleGroup _tg;

	public override UI_Title_ProfileSelection.StateType GetMyState()
	{
		return UI_Title_ProfileSelection.StateType.List;
	}

	protected override void Awake()
	{
		base.Awake();
		_tg = itemsParent.GetComponent<UI_ToggleGroup>();
		_tg.onCurrentIndexChanged.AddListener(ProfileIndexChanged);
		deleteButton.onClick.AddListener(ClickDelete);
		loadButton.onClick.AddListener(ClickLoad);
		cancelButton.onClick.AddListener(ClickCancel);
	}

	private void ProfileIndexChanged(int arg0)
	{
		if (_tg.currentIndex < 0 || _tg.currentIndex >= _profiles.Count)
		{
			deleteButton.interactable = false;
			loadButton.interactable = false;
			loadButtonText.text = DewLocalization.GetUIValue("Title_Profile_Load");
		}
		else
		{
			DewProfileState state = _profiles[_tg.currentIndex].state;
			bool isOnlyProfile = _profiles.Count <= 1;
			deleteButton.interactable = arg0 >= 0 && !isOnlyProfile;
			loadButton.interactable = arg0 >= 0 && state != DewProfileState.Corrupted;
			loadButtonText.text = ((state == DewProfileState.Convertible) ? DewLocalization.GetUIValue("Title_Profile_Convert") : DewLocalization.GetUIValue("Title_Profile_Load"));
		}
	}

	private void RefreshList()
	{
		for (int i = itemsParent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(itemsParent.GetChild(i).gameObject);
		}
		_profiles = DewSave.GetProfiles();
		_profiles.Sort(delegate(DewProfileItem a, DewProfileItem b)
		{
			long num = ((a.peek != null) ? a.peek.creationDate : long.MaxValue);
			long value = ((b.peek != null) ? b.peek.creationDate : long.MaxValue);
			return num.CompareTo(value);
		});
		_profiles.Sort((DewProfileItem a, DewProfileItem b) => a.state.CompareTo(b.state));
		for (int j = 0; j < _profiles.Count; j++)
		{
			DewProfileItem p2 = _profiles[j];
			UI_Title_ProfileSelection_ListWindow_Item uI_Title_ProfileSelection_ListWindow_Item = global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent);
			uI_Title_ProfileSelection_ListWindow_Item.Setup(p2);
			uI_Title_ProfileSelection_ListWindow_Item.GetComponent<UI_Toggle>().index = j;
		}
		newProfileButton.gameObject.SetActive(_profiles.Count < 10);
		int index = _profiles.FindIndex((DewProfileItem p) => p.path == DewSave.profilePath);
		if (index == -1)
		{
			index = 0;
		}
		_tg.currentIndex = index;
		ProfileIndexChanged(index);
		ScrollRect componentInChildren = GetComponentInChildren<ScrollRect>();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)componentInChildren.transform);
		componentInChildren.normalizedPosition = Vector2.one;
		cancelButton.gameObject.SetActive(!string.IsNullOrEmpty(DewSave.profilePath));
	}

	private void OnEnable()
	{
		RefreshList();
	}

	private void ClickDelete()
	{
		global::UnityEngine.Object.FindObjectOfType<UI_Title_ProfileSelection_DeleteProfileWindow>(includeInactive: true).deletingItem = _profiles[_tg.currentIndex];
		base.parent.SetState(UI_Title_ProfileSelection.StateType.Delete);
	}

	private void ClickLoad()
	{
		if (_tg.currentIndex < 0 || _tg.currentIndex >= _profiles.Count)
		{
			return;
		}
		switch (_profiles[_tg.currentIndex].state)
		{
		case DewProfileState.Normal:
			if (_profiles[_tg.currentIndex].path != DewSave.profilePath)
			{
				DewSave.LoadProfile(_profiles[_tg.currentIndex].path);
			}
			ManagerBase<UIManager>.instance.SetState("Title");
			break;
		case DewProfileState.Convertible:
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				rawContent = DewLocalization.GetUIValue("Title_Profile_Message_Convertible"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				owner = this,
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						ManagerBase<TitleManager>.instance.didConvertSave = true;
						DewSave.ConvertProfile(_profiles[_tg.currentIndex].path);
						ManagerBase<MessageManager>.instance.ShowMessageLocalized("Title_Profile_Message_ConversionComplete");
						RefreshList();
					}
				}
			});
			break;
		case DewProfileState.UnsupportedEdition:
			ManagerBase<MessageManager>.instance.ShowMessageLocalized("Title_Profile_Message_UnsupportedEdition");
			break;
		case DewProfileState.UnsupportedVersion:
			ManagerBase<MessageManager>.instance.ShowMessageLocalized("Title_Profile_Message_UnsupportedVersion");
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case DewProfileState.Corrupted:
			break;
		}
	}

	public void CreateNewProfile()
	{
		base.parent.SetState(UI_Title_ProfileSelection.StateType.Create);
	}

	private void ClickCancel()
	{
		ManagerBase<UIManager>.instance.SetState("Title");
	}
}
