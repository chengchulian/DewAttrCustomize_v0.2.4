using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_ExpandableStory : MonoBehaviour
{
	public TextMeshProUGUI text;

	public GameObject maskObject;

	public GameObject fxOpen;

	public GameObject fxClose;

	private LayoutElement _layoutElement;

	private float _shrunkHeight;

	private int _expandFrame;

	public bool isExpanded { get; private set; }

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
		_layoutElement = GetComponent<LayoutElement>();
		_shrunkHeight = _layoutElement.preferredHeight;
	}

	private void Start()
	{
		NetworkedManagerBase<PlayLobbyManager>.instance.ClientEvent_OnLocalPlayerHeroChanged += (Action<string>)delegate
		{
			Shrink();
		};
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 1000, delegate
		{
			if (isExpanded)
			{
				Shrink();
				return true;
			}
			return false;
		});
	}

	private void Update()
	{
		if (isExpanded && _expandFrame != Time.frameCount && Input.GetKeyUp(KeyCode.Mouse0))
		{
			Shrink();
		}
	}

	private void OnClick()
	{
		if (!isExpanded)
		{
			Expand();
		}
	}

	public void Shrink()
	{
		if (isExpanded)
		{
			isExpanded = false;
			_layoutElement.DOKill();
			_layoutElement.preferredHeight = _shrunkHeight;
			maskObject.SetActive(value: true);
			base.transform.DOKill();
			base.transform.DOScale(Vector3.one, 0.15f);
			DewEffect.Play(fxClose);
			_layoutElement.ignoreLayout = false;
		}
	}

	public void Expand()
	{
		if (!isExpanded)
		{
			isExpanded = true;
			_expandFrame = Time.frameCount;
			_layoutElement.ignoreLayout = true;
			_layoutElement.DOKill();
			_layoutElement.preferredHeight = text.preferredHeight + 65f;
			RectTransform obj = (RectTransform)base.transform;
			obj.sizeDelta = obj.sizeDelta.WithY(text.preferredHeight + 65f);
			maskObject.SetActive(value: false);
			base.transform.DOKill();
			base.transform.DOScale(Vector3.one * 1.15f, 0.15f);
			DewEffect.Play(fxOpen);
		}
	}
}
