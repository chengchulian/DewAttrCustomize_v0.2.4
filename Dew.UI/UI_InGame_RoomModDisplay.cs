using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_RoomModDisplay : SingletonBehaviour<UI_InGame_RoomModDisplay>
{
	public LayoutElement spacer;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI descriptionText;

	public Image initialFlash;

	public Image icon;

	public Sprite defaultIcon;

	public Image backdrop;

	private Color _initialFlashOriginalColor;

	private CanvasGroup _cg;

	public bool isAnnouncing { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		_cg = GetComponent<CanvasGroup>();
		_initialFlashOriginalColor = initialFlash.color;
		base.gameObject.SetActive(value: false);
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoadStarted += new Action<EventInfoLoadRoom>(ClientEventOnRoomLoadStarted);
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += (Action<EventInfoLoadRoom>)delegate
		{
			Refresh();
		};
	}

	private void ClientEventOnRoomLoadStarted(EventInfoLoadRoom _)
	{
		base.gameObject.SetActive(value: false);
	}

	private void Refresh()
	{
		base.gameObject.SetActive(value: false);
		if (SingletonBehaviour<Special_RoomAnnouncer>.instance != null)
		{
			Special_RoomAnnouncer s = SingletonBehaviour<Special_RoomAnnouncer>.instance;
			string nameRaw = DewLocalization.GetUIValue(s.key + "_Name");
			string descRaw = DewLocalization.GetUIValue(s.key + "_Description");
			ShowModifier(nameRaw, descRaw, s.color, s.sprite, s.spriteScale, SingletonDewNetworkBehaviour<Room>.instance.isRevisit);
			return;
		}
		foreach (ModifierData modifier in NetworkedManagerBase<ZoneManager>.instance.currentNode.modifiers)
		{
			RoomModifierBase mod = DewResources.GetByShortTypeName<RoomModifierBase>(modifier.type);
			if (!(mod == null) && mod.isMain && (!mod.hiddenOnVisitedNode || !SingletonDewNetworkBehaviour<Room>.instance.isRevisit))
			{
				ShowModifier(mod, !(mod is RoomMod_Hunted) && SingletonDewNetworkBehaviour<Room>.instance.isRevisit);
				break;
			}
		}
	}

	private void ShowModifier(RoomModifierBase mod, bool isRevisit)
	{
		string text = mod.GetType().Name;
		string nameRaw = DewLocalization.GetUIValue(text + "_Name");
		string descRaw = DewLocalization.GetUIValue(text + "_Description");
		if (mod is RoomMod_Hunted)
		{
			nameRaw = nameRaw + " " + Dew.IntToRoman(Mathf.Clamp(NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel, 1, 5));
			if (NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel == 5)
			{
				string heroKey = DewPlayer.local.hero.GetType().Name.Substring(5);
				if (DewLocalization.TryGetUIValue("RoomMod_Hunted_PrimusWarnTo" + heroKey + "_" + global::UnityEngine.Random.Range(0, 3), out var val))
				{
					descRaw = val;
				}
			}
		}
		else if (mod is RoomMod_StarCookie)
		{
			string heroKey2 = DewPlayer.local.hero.GetType().Name.Substring(5);
			if (DewLocalization.TryGetUIValue("RoomMod_StarCookie_SpecialCommentBy" + heroKey2, out var val2))
			{
				descRaw = val2 ?? "";
			}
		}
		ShowModifier(nameRaw, descRaw, mod.mainColor, mod.mapSprite, mod.mapSpriteScale, isRevisit);
	}

	private void ShowModifier(string nameRaw, string descRaw, Color color, Sprite sprite, float spriteScale, bool isRevisit)
	{
		nameText.text = nameRaw;
		descriptionText.text = descRaw;
		nameText.color = color;
		backdrop.color = color.WithV(color.GetV() * 0.7f);
		initialFlash.color = (isRevisit ? Color.clear : (_initialFlashOriginalColor * color));
		icon.color = color;
		icon.sprite = ((sprite != null) ? sprite : defaultIcon);
		icon.transform.localScale = Vector3.one * spriteScale * 2f;
		base.gameObject.SetActive(value: true);
		_cg.alpha = 0f;
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return null;
			ShowAndAnimate(isRevisit);
		}
	}

	private void ShowAndAnimate(bool isRevisit)
	{
		RectTransform thisRt = (RectTransform)base.transform;
		RectTransform spacerRt = (RectTransform)spacer.transform;
		float height = LayoutUtility.GetPreferredHeight(thisRt);
		thisRt.pivot = Vector2.one;
		thisRt.anchorMin = spacerRt.anchorMin;
		thisRt.anchorMax = spacerRt.anchorMax;
		thisRt.anchoredPosition = spacerRt.anchoredPosition;
		spacer.preferredHeight = 0f;
		thisRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		Vector3 firstPos = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.7f);
		Vector2 lastAnchorPos = thisRt.anchoredPosition;
		Quaternion firstRot = Quaternion.identity;
		Quaternion lastRot = thisRt.rotation;
		Vector3 firstScale = thisRt.localScale * 1.2f;
		Vector3 lastScale = thisRt.localScale;
		if (isRevisit)
		{
			_cg.alpha = 1f;
			thisRt.localScale = lastScale;
			thisRt.pivot = Vector2.one;
			thisRt.anchoredPosition = lastAnchorPos;
			thisRt.rotation = lastRot;
			spacer.preferredHeight = height;
			return;
		}
		isAnnouncing = true;
		float animateTime = 0.5f;
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Game_RoomModAppear");
		thisRt.localScale = firstScale * 0.9f;
		thisRt.position = firstPos + (float)Screen.height * 0.05f * Vector3.down;
		thisRt.rotation = firstRot;
		thisRt.pivot = new Vector2(0.5f, 0.5f);
		_cg.DOFade(1f, 0.25f);
		thisRt.DOMove(firstPos, 0.25f);
		thisRt.DOScale(firstScale, 0.25f);
		this.DOKill(complete: true);
		DOTween.Sequence().AppendInterval(2.75f).AppendCallback(delegate
		{
			thisRt.DOScale(lastScale, animateTime);
			thisRt.DOPivot(Vector2.one, animateTime);
			Vector3 localPosition = thisRt.localPosition;
			thisRt.anchoredPosition = lastAnchorPos;
			Vector2 vector = thisRt.localPosition.WithZ(0f);
			thisRt.localPosition = localPosition;
			thisRt.DOLocalMove(vector, animateTime);
			thisRt.DORotateQuaternion(lastRot, animateTime);
			DOTween.To(() => spacer.preferredHeight, delegate(float v)
			{
				spacer.preferredHeight = v;
			}, height, animateTime);
			DOTween.Sequence().AppendInterval(1f).AppendCallback(delegate
			{
				isAnnouncing = false;
			})
				.SetId(this);
		})
			.SetId(this);
	}

	private void OnDisable()
	{
		spacer.preferredHeight = 0f;
	}
}
