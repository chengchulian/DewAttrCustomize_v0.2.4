using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_World_NodeItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IShowTooltip, IPingableWorldNode
{
	[Serializable]
	public class IconSettings
	{
		public GameObject gobj;

		public Sprite[] sprites;
	}

	public bool isMiniMapVariant = true;

	public Button button;

	public GameObject youAreHereObject;

	public GameObject selectedObject;

	public GameObject canTraverseObject;

	public GameObject gamepadHoverObject;

	public IconSettings visitedObject;

	public Image[] changedMaterialImages;

	public Material matVisited;

	public Material matVisitedHover;

	public Material matNotVisited;

	public Material matNotVisitedHover;

	public Image[] modifierDots;

	public TextMeshProUGUI debugText;

	public GameObject hunterStart;

	public GameObject[] hunterLevelObjects;

	public IconSettings unexplored;

	public IconSettings normal;

	public IconSettings merchant;

	public IconSettings quest;

	public IconSettings exitBoss;

	private UI_InGame_WorldMap _parent;

	private Vector2 _originalPos;

	private Vector2 _cv;

	private Vector2 _currentOffset;

	private float _nextOffsetSetTime;

	private float _startTime;

	public int index { get; private set; }

	public WorldNodeData node { get; private set; }

	int IPingableWorldNode.nodeIndex => index;

	public void Setup(int i, UI_InGame_WorldMap parent)
	{
		gamepadHoverObject.SetActive(value: false);
		_startTime = Time.time;
		index = i;
		node = NetworkedManagerBase<ZoneManager>.instance.nodes[index];
		youAreHereObject.SetActive(index == NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex);
		if (!isMiniMapVariant && youAreHereObject.activeSelf)
		{
			youAreHereObject.transform.localScale = Vector3.zero;
		}
		canTraverseObject.SetActive(InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.Shown && NetworkedManagerBase<ZoneManager>.instance.IsNodeConnected(NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex, index));
		if (canTraverseObject.activeSelf)
		{
			canTraverseObject.transform.localScale = Vector3.one * global::UnityEngine.Random.Range(0.9f, 1.2f);
			canTraverseObject.transform.localRotation = Quaternion.Euler(0f, 0f, global::UnityEngine.Random.Range(0f, 360f));
		}
		RectTransform rt = (RectTransform)base.transform;
		rt.anchoredPosition = Vector2.zero;
		_originalPos = new Vector2(node.position.x / ZoneManager.WorldWidth, node.position.y / ZoneManager.WorldHeight);
		if (isMiniMapVariant)
		{
			rt.anchorMin = _originalPos;
		}
		else
		{
			rt.anchorMin = new Vector2(0.5f, 0.5f) * -0.12f + _originalPos * 1.12f;
		}
		rt.anchorMax = rt.anchorMin;
		_parent = parent;
		if (!isMiniMapVariant)
		{
			UI_InGame_WorldMap parent2 = _parent;
			parent2.onHoveringNodeChanged = (Action<int, int>)Delegate.Combine(parent2.onHoveringNodeChanged, new Action<int, int>(OnHoveringNodeChanged));
			button.onClick.AddListener(delegate
			{
				_parent.TravelToNode(index);
			});
		}
		else
		{
			global::UnityEngine.Object.Destroy(GetComponent<UI_ButtonAudio>());
		}
		selectedObject.SetActive(value: false);
		UpdateMaterial();
		HunterStatus status = NetworkedManagerBase<ZoneManager>.instance.hunterStatuses[index];
		hunterLevelObjects[0].SetActive(status == HunterStatus.AboutToBeTaken);
		hunterLevelObjects[1].SetActive(status == HunterStatus.Level1);
		hunterLevelObjects[2].SetActive(status == HunterStatus.Level2);
		hunterLevelObjects[3].SetActive(status == HunterStatus.Level3);
		hunterStart.SetActive(status == HunterStatus.None && NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex == index);
		GameObject[] array = hunterLevelObjects;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].transform.localRotation = Quaternion.Euler(0f, 0f, global::UnityEngine.Random.Range(0f, 360f));
		}
		if (node.status == WorldNodeStatus.Unexplored)
		{
			SetActive(unexplored, value: true);
			SetActive(normal, value: false);
			SetActive(merchant, value: false);
			SetActive(quest, value: false);
			SetActive(exitBoss, value: false);
		}
		else
		{
			SetActive(unexplored, value: false);
			SetActive(normal, node.type.IsNormalNode());
			SetActive(merchant, node.type == WorldNodeType.Merchant);
			SetActive(quest, node.type == WorldNodeType.Quest);
			SetActive(exitBoss, node.type == WorldNodeType.ExitBoss);
		}
		SetActive(visitedObject, index != NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex && node.status == WorldNodeStatus.HasVisited);
		for (int k = 0; k < modifierDots.Length; k++)
		{
			modifierDots[k].gameObject.SetActive(value: false);
		}
		int k2 = 0;
		for (int l = 0; l < node.modifiers.Count; l++)
		{
			if (k2 >= modifierDots.Length)
			{
				break;
			}
			if (node.IsModifierVisible(l))
			{
				modifierDots[k2].gameObject.SetActive(value: true);
				RoomModifierBase mod = DewResources.GetByShortTypeName<RoomModifierBase>(node.modifiers[l].type);
				modifierDots[k2].color = mod.mainColor;
				if (mod.mapSprite != null)
				{
					modifierDots[k2].sprite = mod.mapSprite;
				}
				modifierDots[k2].transform.localScale *= mod.mapSpriteScale;
				modifierDots[k2].transform.localRotation = Quaternion.Euler(0f, 0f, global::UnityEngine.Random.Range(-10, 10));
				k2++;
			}
		}
		if (!isMiniMapVariant)
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			component.alpha = 0.35f;
			component.DOFade(1f, 1f);
			base.transform.localScale = Vector3.one * 2.5f;
			base.transform.DOScale(1f, 0.5f);
		}
		base.transform.localRotation = Quaternion.Euler(0f, 0f, global::UnityEngine.Random.Range(-15, 15));
		debugText.gameObject.SetActive(value: false);
		(isMiniMapVariant ? InGameUIManager.instance.miniWorldMapNodeItems : InGameUIManager.instance.fullWorldMapNodeItems)[index] = (RectTransform)base.transform;
	}

	private void SetActive(IconSettings icon, bool value)
	{
		icon.gobj.SetActive(value);
		if (value)
		{
			icon.gobj.GetComponent<Image>().sprite = icon.sprites[global::UnityEngine.Random.Range(0, icon.sprites.Length)];
		}
	}

	private void Update()
	{
		if (youAreHereObject.activeSelf)
		{
			if (isMiniMapVariant)
			{
				youAreHereObject.transform.localScale = Vector3.one * (1f + Mathf.Sin((Time.time - _startTime) * 0.75f * 5f + MathF.PI / 2f) * 0.05f);
			}
			else
			{
				youAreHereObject.transform.localScale = Vector3.one * (1.35f + Mathf.Sin((Time.time - _startTime) * 5f + MathF.PI / 2f) * 0.3f);
			}
		}
		if (!isMiniMapVariant)
		{
			RectTransform obj = (RectTransform)base.transform;
			obj.anchorMin = Vector2.SmoothDamp(obj.anchorMin, _originalPos + _currentOffset, ref _cv, 0.6f);
			obj.anchorMax = obj.anchorMin;
			if (Time.time > _nextOffsetSetTime)
			{
				_nextOffsetSetTime = Time.time + global::UnityEngine.Random.Range(0.25f, 0.7f);
				_currentOffset = global::UnityEngine.Random.insideUnitCircle * 0.0075f;
			}
		}
	}

	private void OnDestroy()
	{
		if (!(_parent == null))
		{
			UI_InGame_WorldMap parent = _parent;
			parent.onHoveringNodeChanged = (Action<int, int>)Delegate.Remove(parent.onHoveringNodeChanged, new Action<int, int>(OnHoveringNodeChanged));
		}
	}

	private void OnHoveringNodeChanged(int arg1, int arg2)
	{
		if (!(this == null))
		{
			UpdateMaterial();
			if (DewInput.currentMode == InputMode.Gamepad && _parent.hoveringNode == index)
			{
				ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
			}
			gamepadHoverObject.SetActive(DewInput.currentMode == InputMode.Gamepad && _parent.hoveringNode == index);
		}
	}

	private void UpdateMaterial()
	{
		bool isHover = _parent.hoveringNode == index;
		bool shouldBeMute = node.status == WorldNodeStatus.HasVisited;
		Image[] array = changedMaterialImages;
		foreach (Image img in array)
		{
			if (isHover)
			{
				img.material = (shouldBeMute ? matVisitedHover : matNotVisitedHover);
				if (SingletonBehaviour<DevCanvas>.instance.state == DevCanvas.DevCanvasState.Shown)
				{
					debugText.gameObject.SetActive(value: true);
					debugText.color = Color.green;
					debugText.text = index.ToString();
				}
				else
				{
					debugText.gameObject.SetActive(value: false);
				}
				continue;
			}
			img.material = (shouldBeMute ? matVisited : matNotVisited);
			if (SingletonBehaviour<DevCanvas>.instance.state == DevCanvas.DevCanvasState.Shown)
			{
				debugText.gameObject.SetActive(value: true);
				debugText.color = Color.red;
				if (_parent.hoveringNode >= 0)
				{
					debugText.text = NetworkedManagerBase<ZoneManager>.instance.GetNodeDistance(_parent.hoveringNode, index).ToString();
				}
				else
				{
					debugText.text = "";
				}
			}
			else
			{
				debugText.gameObject.SetActive(value: false);
			}
		}
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		_parent.HoverNode(index);
		SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		_parent.UnhoverNode(index);
		SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (ManagerBase<MessageManager>.instance.isShowingMessage)
		{
			return;
		}
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			tooltip.ShowWorldNodeTooltip((Func<Vector2>)(() => base.transform.position + Vector3.up * 30f), index);
		}
		else
		{
			tooltip.ShowWorldNodeTooltip(null, index);
		}
	}
}
