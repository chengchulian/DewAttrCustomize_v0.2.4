using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Guide_View : View
{
	public Button backButton;

	public UnityEvent onGoBack;

	public Transform guideContentParent;

	public Transform listItemParent;

	public GameObject listItemPrefab;

	public GameObject listSeparatorPrefab;

	public TextMeshProUGUI titleText;

	private bool _isSaveDirty;

	private GameObject _previouslyActiveGuide;

	protected override void Awake()
	{
		base.Awake();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		backButton.onClick.AddListener(ClickGoBack);
		for (int i = 0; i < guideContentParent.childCount; i++)
		{
			Transform child = guideContentParent.GetChild(i);
			if (child.name.StartsWith("-- "))
			{
				GameObject sep = Object.Instantiate(listSeparatorPrefab, listItemParent);
				sep.name = child.name;
				sep.GetComponentInChildren<DewLocalizedText>().key = sep.name.Substring(3);
				child.gameObject.SetActive(value: false);
			}
			else
			{
				Object.Instantiate(listItemPrefab, listItemParent).name = child.name;
				child.gameObject.SetActive(value: false);
			}
		}
		titleText.text = "";
	}

	private void ClickGoBack()
	{
		onGoBack.Invoke();
	}

	protected override void OnShow()
	{
		base.OnShow();
		if (!(_previouslyActiveGuide != null))
		{
			return;
		}
		_previouslyActiveGuide.SetActive(value: false);
		_previouslyActiveGuide.SetActive(value: true);
		titleText.text = DewLocalization.GetUIValue(_previouslyActiveGuide.name);
		for (int i = 0; i < listItemParent.childCount; i++)
		{
			Transform child = listItemParent.GetChild(i);
			if (child.name == _previouslyActiveGuide.name)
			{
				child.GetComponent<Selectable>().Select();
				break;
			}
		}
	}

	protected override void OnHide()
	{
		base.OnHide();
		if (_isSaveDirty)
		{
			_isSaveDirty = false;
			DewSave.SaveProfile();
		}
	}

	public void ShowItem(string item)
	{
		Transform targetGuideTransform = guideContentParent.Find(item);
		if (!(targetGuideTransform == null))
		{
			if (_previouslyActiveGuide != null)
			{
				_previouslyActiveGuide.SetActive(value: false);
				_previouslyActiveGuide = null;
			}
			if (!DewSave.profile.seenGuides.Contains(item))
			{
				DewSave.profile.seenGuides.Add(item);
				_isSaveDirty = true;
			}
			titleText.text = DewLocalization.GetUIValue(item);
			_previouslyActiveGuide = targetGuideTransform.gameObject;
			_previouslyActiveGuide.SetActive(value: true);
		}
	}
}
