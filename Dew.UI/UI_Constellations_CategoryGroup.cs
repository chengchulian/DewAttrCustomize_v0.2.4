using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Constellations_CategoryGroup : UI_GamepadFocusable, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IGamepadFocusableOverrideInput
{
	private static readonly int OutlineAlpha = Shader.PropertyToID("_OutlineAlpha");

	private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

	private static readonly int ShadowColor = Shader.PropertyToID("_ShadowColor");

	public UI_Constellations_StarEdge edgeTemplate;

	public GameObject selectHitboxObject;

	public GameObject fxSelect;

	public Image backgroundImage;

	public TextMeshProUGUI progressText;

	public TextMeshProUGUI nameText;

	public GameObject comingSoonObj;

	private UI_Constellations_StarItem[] _stars;

	private RectTransform _rt;

	private Vector2 _nonSelectedAnchorPos;

	private Vector3 _nonSelectedScale;

	private CanvasGroup _cg;

	private UI_Constellations _parent;

	private Material _matBg;

	private float _matBgOriginalOutlineAlpha;

	private Color _matBgOriginalShadowColor;

	private Color _matBgOriginalOutlineColor;

	private void Start()
	{
		_matBg = global::UnityEngine.Object.Instantiate(backgroundImage.material);
		backgroundImage.material = _matBg;
		_matBgOriginalOutlineAlpha = _matBg.GetFloat(OutlineAlpha);
		_matBgOriginalShadowColor = _matBg.GetColor(ShadowColor);
		_matBgOriginalOutlineColor = _matBg.GetColor(OutlineColor);
		edgeTemplate.gameObject.SetActive(value: false);
		_stars = GetComponentsInChildren<UI_Constellations_StarItem>();
		_cg = GetComponent<CanvasGroup>();
		_rt = (RectTransform)base.transform;
		_nonSelectedAnchorPos = _rt.anchoredPosition;
		_nonSelectedScale = _rt.localScale;
		if (comingSoonObj != null)
		{
			comingSoonObj.SetActive(value: false);
		}
		UI_Constellations_StarItem[] stars = _stars;
		foreach (UI_Constellations_StarItem s in stars)
		{
			foreach (UI_Constellations_StarItem prev in s.prevStars)
			{
				UI_Constellations_StarEdge uI_Constellations_StarEdge = global::UnityEngine.Object.Instantiate(edgeTemplate, base.transform);
				uI_Constellations_StarEdge.gameObject.SetActive(value: true);
				uI_Constellations_StarEdge.Setup(s, prev, this);
				uI_Constellations_StarEdge.transform.SetAsLastSibling();
			}
		}
		stars = _stars;
		for (int i = 0; i < stars.Length; i++)
		{
			stars[i].transform.SetAsLastSibling();
		}
		_parent = GetComponentInParent<UI_Constellations>();
		if (_parent != null)
		{
			UI_Constellations parent = _parent;
			parent.onSelectedGroupChanged = (Action<UI_Constellations_CategoryGroup>)Delegate.Combine(parent.onSelectedGroupChanged, new Action<UI_Constellations_CategoryGroup>(OnSelectedGroupChanged));
			UI_Constellations parent2 = _parent;
			parent2.onHoveredGroupChanged = (Action<UI_Constellations_CategoryGroup>)Delegate.Combine(parent2.onHoveredGroupChanged, new Action<UI_Constellations_CategoryGroup>(OnHoveredGroupChanged));
			UI_Constellations parent3 = _parent;
			parent3.onStateChanged = (Action)Delegate.Combine(parent3.onStateChanged, new Action(OnStateChanged));
			OnSelectedGroupChanged(null);
			OnStateChanged();
		}
		NetworkedManagerBase<PlayLobbyManager>.instance.ClientEvent_OnLocalPlayerHeroChanged += new Action<string>(ClientEventOnLocalPlayerHeroChanged);
		selectHitboxObject.transform.SetAsLastSibling();
	}

	private void ClientEventOnLocalPlayerHeroChanged(string obj)
	{
		OnSelectedGroupChanged(null);
	}

	private void OnStateChanged()
	{
		int total = 0;
		int unlocked = 0;
		UI_Constellations_StarItem[] stars = _stars;
		foreach (UI_Constellations_StarItem s in stars)
		{
			total += s.maxLevel;
			unlocked += s.data.level;
		}
		if (total == 0)
		{
			progressText.text = "";
		}
		else if (unlocked >= total)
		{
			progressText.text = DewLocalization.GetUIValue("Constellations_Perfection");
		}
		else
		{
			progressText.text = $"{unlocked:#,##0}/{total:#,##0}";
		}
	}

	private void OnDestroy()
	{
		if (_matBg != null)
		{
			global::UnityEngine.Object.Destroy(_matBg);
		}
	}

	private void OnHoveredGroupChanged(UI_Constellations_CategoryGroup obj)
	{
		if (!(_parent.selectedGroup != null))
		{
			_matBg.DOKill(complete: true);
			_matBg.SetFloat(OutlineAlpha, (obj == this) ? (_matBgOriginalOutlineAlpha + 0.2f) : _matBgOriginalOutlineAlpha);
		}
	}

	public void Flash(float durationMultiplier)
	{
		bool isSelected = _parent.selectedGroup == this;
		_matBg.DOKill(complete: true);
		_matBg.SetColor(ShadowColor, _matBgOriginalShadowColor);
		_matBg.SetColor(OutlineColor, _matBgOriginalOutlineColor);
		_matBg.SetFloat(OutlineAlpha, 1f);
		_matBg.DOFloat(isSelected ? 0.4f : _matBgOriginalOutlineAlpha, OutlineAlpha, 0.5f * durationMultiplier);
		_matBg.DOColor(isSelected ? Color.black : _matBgOriginalShadowColor, ShadowColor, 0.5f * durationMultiplier);
		_matBg.DOColor(isSelected ? Color.black : _matBgOriginalOutlineColor, OutlineColor, 0.5f * durationMultiplier);
	}

	private void OnSelectedGroupChanged(UI_Constellations_CategoryGroup obj)
	{
		if (comingSoonObj != null)
		{
			comingSoonObj.SetActive(obj == this);
		}
		if (obj == this)
		{
			DewEffect.Play(fxSelect);
			Flash(1f);
		}
		else
		{
			_matBg.DOKill(complete: true);
			_matBg.DOFloat(_matBgOriginalOutlineAlpha, OutlineAlpha, 0.5f);
			_matBg.DOColor(_matBgOriginalShadowColor, ShadowColor, 0.5f);
			_matBg.DOColor(_matBgOriginalOutlineColor, OutlineColor, 0.5f);
		}
		selectHitboxObject.SetActive(obj == null);
		if ((!base.name.StartsWith("Hero_") || (DewPlayer.local != null && base.name == DewPlayer.local.selectedHeroType)) && (obj == null || obj == this))
		{
			base.gameObject.SetActive(value: true);
			_cg.DOKill();
			_cg.DOFade(1f, 0.3f);
			_cg.blocksRaycasts = true;
			_cg.interactable = true;
		}
		else
		{
			_cg.DOKill();
			_cg.blocksRaycasts = false;
			_cg.interactable = false;
			DOTween.Sequence().Append(_cg.DOFade(0f, 0.3f)).AppendCallback(delegate
			{
				base.gameObject.SetActive(value: false);
			})
				.SetId(_cg);
		}
		if (base.gameObject.activeSelf)
		{
			_rt.DOKill(complete: true);
			if (obj == this)
			{
				_rt.DOAnchorPos(Vector2.zero, 0.5f);
				_rt.DOScale(Vector3.one, 0.5f);
			}
			else if (obj == null)
			{
				_rt.DOAnchorPos(_nonSelectedAnchorPos, 0.5f);
				_rt.DOScale(_nonSelectedScale, 0.5f);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!(_parent == null))
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				_parent.SelectGroup(this);
			}
			else if (eventData.button == PointerEventData.InputButton.Right && _parent.selectedGroup == this)
			{
				_parent.SelectGroup(null);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_parent != null)
		{
			_parent.HoverGroup(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_parent != null)
		{
			_parent.HoverGroup(null);
		}
	}

	private void OnValidate()
	{
		if (!(nameText == null) && !(progressText == null))
		{
			Color c = nameText.color;
			progressText.color = new Color(c.r, c.g, c.b, progressText.color.a);
		}
	}

	private void PrintTotalStardust()
	{
		UI_Constellations_StarItem[] componentsInChildren = GetComponentsInChildren<UI_Constellations_StarItem>();
		int sum = 0;
		UI_Constellations_StarItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			int[] requiredStardusts = array[i].requiredStardusts;
			foreach (int a in requiredStardusts)
			{
				sum += a;
			}
		}
		Debug.Log($"Constellation '{base.name}' requires {sum} stardusts to complete");
	}

	public override bool CanBeFocused()
	{
		if (base.CanBeFocused())
		{
			return _parent.selectedGroup == null;
		}
		return false;
	}

	public bool OnGamepadDpadLeft()
	{
		return MoveFocusToConstellation(Vector3.left);
	}

	public bool OnGamepadDpadRight()
	{
		return MoveFocusToConstellation(Vector3.right);
	}

	public bool OnGamepadDpadUp()
	{
		return MoveFocusToConstellation(Vector3.up);
	}

	public bool OnGamepadDpadDown()
	{
		return MoveFocusToConstellation(Vector3.down);
	}

	private bool MoveFocusToConstellation(Vector3 direction)
	{
		ListReturnHandle<UI_Constellations_CategoryGroup> handle;
		UI_Constellations_CategoryGroup best = Dew.SelectBestWithScore(((Component)base.transform.parent).GetComponentsInChildrenNonAlloc(out handle), (UI_Constellations_CategoryGroup group, int _) => Vector3.Dot(direction, group.transform.position));
		handle.Return();
		if (best == ManagerBase<GlobalUIManager>.instance.focused)
		{
			return false;
		}
		ManagerBase<GlobalUIManager>.instance.SetFocus(best);
		return true;
	}
}
