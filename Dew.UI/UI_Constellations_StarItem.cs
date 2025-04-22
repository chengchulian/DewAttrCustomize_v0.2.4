using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Constellations_StarItem : UI_GamepadFocusable, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IGamepadFocusableOverrideInput
{
	public Sprite icon;

	public Color color;

	public Image iconOutline;

	public Image iconFill;

	public List<UI_Constellations_StarItem> prevStars = new List<UI_Constellations_StarItem>();

	private List<UI_Constellations_StarItem> _nextStars = new List<UI_Constellations_StarItem>();

	[SerializeField]
	private int _requiredAdjacentLevels = 1;

	[FormerlySerializedAs("requiredKarmas")]
	public int[] requiredStardusts = new int[0];

	public GameObject lockedObject;

	public Image imgCanUnlock;

	public float offsetSmoothTime;

	public float offsetMagnitude;

	public Vector2 offsetUpdatePeriod;

	public TextMeshProUGUI progressText;

	public GameObject fxClickNoAction;

	public GameObject fxUpgrade;

	private float _nextUpdateTime;

	private Vector2 _cv;

	private Vector2 _currentOffset;

	private Vector2 _originalPos;

	private Vector3 _originalScale;

	private RectTransform _rt;

	private CanvasGroup _cg;

	private UI_Constellations_CategoryGroup _group;

	private UI_Constellations _constellations;

	private Vector3 _lockedObjectOriginalScale;

	public DewProfile.StarData data => SingletonBehaviour<UI_Constellations>.instance.state.stars[base.name];

	public bool isMaxedOut => data.level >= maxLevel;

	public int maxLevel => Dew.oldStarsByName[base.name].maxLevel;

	public int requiredStardust
	{
		get
		{
			if (data.level >= 0 && data.level < requiredStardusts.Length)
			{
				return requiredStardusts[data.level];
			}
			return 0;
		}
	}

	public bool isLocked => currentAdjacentLevels < requiredAdjacentLevels;

	public int requiredAdjacentLevels
	{
		get
		{
			if (!Dew.IsStarIncludedInGame(base.name))
			{
				return 999;
			}
			return _requiredAdjacentLevels;
		}
	}

	public int currentAdjacentLevels
	{
		get
		{
			int sum = 0;
			foreach (UI_Constellations_StarItem a in prevStars)
			{
				sum += a.data.level;
			}
			return sum;
		}
	}

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_rt = (RectTransform)base.transform;
		_originalPos = _rt.anchoredPosition;
		_originalScale = _rt.localScale;
		_cg.alpha = 0.9f;
		_cg.blocksRaycasts = false;
		progressText.color = new Color(1f, 1f, 1f, 0.5f);
		_group = GetComponentInParent<UI_Constellations_CategoryGroup>();
		_constellations = GetComponentInParent<UI_Constellations>();
		UI_Constellations constellations = _constellations;
		constellations.onSelectedGroupChanged = (Action<UI_Constellations_CategoryGroup>)Delegate.Combine(constellations.onSelectedGroupChanged, new Action<UI_Constellations_CategoryGroup>(OnSelectedGroupChanged));
		_lockedObjectOriginalScale = lockedObject.transform.localScale;
		UI_Constellations constellations2 = _constellations;
		constellations2.onStateChanged = (Action)Delegate.Combine(constellations2.onStateChanged, new Action(UpdateStatus));
		foreach (UI_Constellations_StarItem prevStar in prevStars)
		{
			prevStar._nextStars.Add(this);
		}
	}

	private void OnSelectedGroupChanged(UI_Constellations_CategoryGroup obj)
	{
		UpdateStatus();
	}

	private void Update()
	{
		if (Time.time > _nextUpdateTime)
		{
			_nextUpdateTime = Time.time + global::UnityEngine.Random.Range(offsetUpdatePeriod.x, offsetUpdatePeriod.y);
			_currentOffset = global::UnityEngine.Random.insideUnitCircle * offsetMagnitude;
		}
		_rt.anchoredPosition = Vector2.SmoothDamp(_rt.anchoredPosition, _originalPos + _currentOffset, ref _cv, offsetSmoothTime);
	}

	private void UpdateStatus()
	{
		bool isSelected = _constellations.selectedGroup == _group;
		float fill = (float)data.level / (float)maxLevel;
		bool canAfford = data.level < maxLevel && requiredStardusts[data.level] <= SingletonBehaviour<UI_Constellations>.instance.state.stardust;
		iconFill.gameObject.SetActive(fill > 0.001f);
		iconFill.fillAmount = fill;
		iconOutline.gameObject.SetActive(value: true);
		lockedObject.gameObject.SetActive(isLocked);
		imgCanUnlock.gameObject.SetActive(!isLocked && data.level < maxLevel && isSelected && canAfford);
		progressText.gameObject.SetActive(isSelected);
		lockedObject.transform.DOKill();
		lockedObject.transform.DOScale(_lockedObjectOriginalScale * (isSelected ? 1f : 1.4f), 0.5f);
		_rt.DOKill();
		_rt.DOScale(isSelected ? _originalScale : (_originalScale * 1.75f), 0.5f);
		if (progressText.gameObject.activeSelf)
		{
			progressText.text = $"{data.level}/{maxLevel}";
		}
		StopAllCoroutines();
		if (!isSelected)
		{
			_cg.blocksRaycasts = false;
		}
		else
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.5f);
			_cg.blocksRaycasts = true;
			if (ManagerBase<GlobalUIManager>.instance.focused == null && !isLocked && data.level < maxLevel)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(this);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_rt.DOKill();
		_rt.localScale = _originalScale * 1.1f;
		_cg.DOKill();
		_cg.DOFade(1f, 0.1f);
		progressText.DOColor(new Color(1f, 1f, 1f, 1f), 0.1f);
		if (!(SingletonBehaviour<UI_Constellations_StarDetails>.instance == null))
		{
			SingletonBehaviour<UI_Constellations_StarDetails>.instance.Setup(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_constellations.selectedGroup == _group)
		{
			_rt.localScale = _originalScale;
		}
		_cg.DOKill();
		_cg.DOFade(0.9f, 0.1f);
		progressText.DOColor(new Color(1f, 1f, 1f, 0.5f), 0.1f);
		if (!(SingletonBehaviour<UI_Constellations_StarDetails>.instance == null))
		{
			SingletonBehaviour<UI_Constellations_StarDetails>.instance.gameObject.SetActive(value: false);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			bool canAfford = data.level < maxLevel && requiredStardusts[data.level] <= SingletonBehaviour<UI_Constellations>.instance.state.stardust;
			if (!isLocked && data.level < maxLevel && canAfford)
			{
				DewEffect.Play(fxUpgrade);
				SingletonBehaviour<UI_Constellations>.instance.state.stardust -= requiredStardust;
				data.level++;
				SingletonBehaviour<UI_Constellations>.instance.SetDirtyAndInvokeChanged();
				_rt.DOPunchScale(Vector3.one * 0.5f, 0.3f);
				_group.Flash(1.2f);
			}
			else
			{
				DewEffect.Play(fxClickNoAction);
				_rt.DOKill(complete: true);
				_rt.DOPunchScale(-Vector3.one * 0.2f, 0.25f);
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			DewEffect.Play(fxClickNoAction);
			_rt.DOKill(complete: true);
			_rt.DOPunchScale(-Vector3.one * 0.2f, 0.25f);
			if (IsDirty())
			{
				Undo();
			}
		}
	}

	private bool IsDirty()
	{
		return DewSave.profile.stars[base.name].level < data.level;
	}

	private void Undo(bool recursive = false)
	{
		if (!IsDirty())
		{
			return;
		}
		SingletonBehaviour<UI_Constellations>.instance.state.stardust += requiredStardusts[data.level - 1];
		data.level--;
		foreach (UI_Constellations_StarItem n in _nextStars)
		{
			if (n.isLocked)
			{
				while (n.IsDirty())
				{
					n.Undo(recursive: true);
				}
			}
		}
		if (!recursive)
		{
			SingletonBehaviour<UI_Constellations>.instance.SetDirtyAndInvokeChanged(needsToCheckUnDirty: true);
		}
	}

	public override void OnFocusStateChanged(bool state)
	{
		base.OnFocusStateChanged(state);
		if (state)
		{
			OnPointerEnter(null);
		}
		else
		{
			OnPointerExit(null);
		}
	}

	public bool OnGamepadDpadUp()
	{
		return MoveFocus(Vector3.up);
	}

	public bool OnGamepadDpadLeft()
	{
		return MoveFocus(Vector3.left);
	}

	public bool OnGamepadDpadDown()
	{
		return MoveFocus(Vector3.down);
	}

	public bool OnGamepadDpadRight()
	{
		return MoveFocus(Vector3.right);
	}

	private bool MoveFocus(Vector3 direction)
	{
		Func<IGamepadFocusable, bool> validator = (IGamepadFocusable f) => f.GetTransform().IsSelfOrDescendantOf(base.transform.parent);
		if (!ManagerBase<GlobalUIManager>.instance.MoveFocus(direction, 45.1f, 1f, float.PositiveInfinity, validator))
		{
			ManagerBase<GlobalUIManager>.instance.MoveFocus(direction, 75f, 1f, float.PositiveInfinity, validator);
		}
		return true;
	}
}
