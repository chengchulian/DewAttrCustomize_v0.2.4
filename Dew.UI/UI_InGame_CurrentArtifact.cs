using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_CurrentArtifact : UI_GamepadFocusable, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public UI_InGame_HeroDetailWindow detailWindow;

	public GameObject slottedInObject;

	public Transform tooltipPivot;

	public RectTransform iconAnimatePivot;

	public GameObject fxAppear;

	public GameObject fxSlotInPlace;

	public GameObject fxDisappear;

	public GameObject fxTouch;

	public DewAudioSource touchAudioSource;

	public Image icon;

	public Image backdrop;

	public Transform editModePosition;

	public Transform nonEditModePosition;

	private CanvasGroup _cg;

	private Material[] _materials;

	private float _nextShineTime;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
		_cg.blocksRaycasts = false;
		slottedInObject.SetActive(value: false);
		_materials = new Material[1];
		_materials[0] = global::UnityEngine.Object.Instantiate(icon.material);
		icon.material = _materials[0];
	}

	private void OnDestroy()
	{
		if (_materials == null)
		{
			return;
		}
		Material[] materials = _materials;
		foreach (Material m in materials)
		{
			if (m != null)
			{
				global::UnityEngine.Object.Destroy(m);
			}
		}
	}

	private void Start()
	{
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnArtifactPickedUp += new Action<string, DewPlayer, Vector3>(ClientEventOnArtifactPickedUp);
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnArtifactRemoved += new Action<string>(ClientEventOnArtifactRemoved);
		UI_InGame_HeroDetailWindow uI_InGame_HeroDetailWindow = detailWindow;
		uI_InGame_HeroDetailWindow.onVisibilityChanged = (Action<bool>)Delegate.Combine(uI_InGame_HeroDetailWindow.onVisibilityChanged, new Action<bool>(OnVisibilityChanged));
	}

	private void OnVisibilityChanged(bool obj)
	{
		UpdateCanvasGroupAlpha();
	}

	private void ClientEventOnArtifactPickedUp(string artifactType, DewPlayer player, Vector3 worldPos)
	{
		StopAllAnimations();
		Artifact artifact = DewResources.GetByShortTypeName<Artifact>(artifactType);
		touchAudioSource.clip = artifact.touchSound;
		icon.sprite = artifact.icon;
		Color c = artifact.mainColor;
		c.a = backdrop.color.a;
		backdrop.color = c;
		Material[] materials = _materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetColor("_ShineColor", artifact.mainColor);
		}
		if (player.isOwned)
		{
			DewEffect.PlayNew(fxTouch);
			_cg.DOFade(1f, 0.1f);
			Vector2 targetPos = iconAnimatePivot.anchoredPosition;
			Vector3 fromPos = ManagerBase<DewCamera>.instance.mainCamera.WorldToScreenPoint(worldPos);
			fromPos.z = iconAnimatePivot.position.z;
			iconAnimatePivot.position = fromPos;
			iconAnimatePivot.DOAnchorPos(targetPos, 0.7f);
			Vector3 targetScale = iconAnimatePivot.localScale;
			iconAnimatePivot.localScale *= 0.25f;
			iconAnimatePivot.DOScale(targetScale, 0.25f);
			DewEffect.Play(fxAppear);
			StartCoroutine(Routine());
		}
		else
		{
			DewEffect.PlayNew(fxTouch);
			DewEffect.Play(fxAppear);
			AnimateSlotInPlace();
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.7f);
			AnimateSlotInPlace();
		}
	}

	private void AnimateSlotInPlace()
	{
		DewEffect.Play(fxSlotInPlace);
		FlashStrong();
		iconAnimatePivot.DOPunchScale(Vector3.one * 0.25f, 0.4f);
		_cg.blocksRaycasts = true;
		slottedInObject.SetActive(value: true);
		_nextShineTime = Time.time + 1f;
		DewEffect.PlayNew(fxTouch);
		UpdateCanvasGroupAlpha();
	}

	private void ClientEventOnArtifactRemoved(string prev)
	{
		StopAllAnimations();
		DewEffect.Play(fxDisappear);
		FlashStrong();
		_cg.DOFade(0f, 0.5f);
		_cg.blocksRaycasts = false;
		slottedInObject.SetActive(value: false);
		SingletonBehaviour<UI_TooltipManager>.instance.Hide();
	}

	private void StopAllAnimations()
	{
		StopAllCoroutines();
		iconAnimatePivot.DOKill(complete: true);
		_cg.DOKill(complete: true);
		_cg.blocksRaycasts = false;
		slottedInObject.SetActive(value: false);
	}

	private void UpdateCanvasGroupAlpha()
	{
		_cg.alpha = ((NetworkedManagerBase<QuestManager>.instance.currentArtifact != null) ? 1 : 0);
		base.transform.DOKill(complete: true);
		base.transform.DOMove((!detailWindow.isShown) ? nonEditModePosition.position : editModePosition.position, 0.3f);
	}

	private void Update()
	{
		if (Time.time > _nextShineTime && _cg.alpha > 0.5f)
		{
			_nextShineTime = Time.time + 5f;
			Material[] materials = _materials;
			foreach (Material obj in materials)
			{
				obj.DOKill(complete: true);
				obj.SetFloat("_ShineLocation", 0f);
				obj.SetFloat("_ShineGlow", 2f);
				obj.DOFloat(1f, "_ShineLocation", 0.4f);
				obj.DOFloat(0f, "_ShineGlow", 0.75f);
			}
		}
	}

	public void FlashStrong()
	{
		Material[] materials = _materials;
		foreach (Material obj in materials)
		{
			DOTween.Kill(obj, complete: true);
			obj.SetFloat("_Glow", 15f);
			obj.DOFloat(0f, "_Glow", 0.85f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!(_cg.alpha < 0.5f))
		{
			icon.transform.DOKill(complete: true);
			icon.transform.DOPunchScale(Vector3.one * 0.2f, 0.4f);
			_nextShineTime = Time.time;
			DewEffect.PlayNew(fxTouch);
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		Artifact artifact = DewResources.GetByShortTypeName<Artifact>(NetworkedManagerBase<QuestManager>.instance.currentArtifact);
		string artifactName = DewLocalization.GetArtifactName(DewLocalization.GetArtifactKey(NetworkedManagerBase<QuestManager>.instance.currentArtifact));
		string suffix = ((DewSave.profile.artifacts[NetworkedManagerBase<QuestManager>.instance.currentArtifact].status == UnlockStatus.Complete) ? ("<color=#c8d5de>(" + DewLocalization.GetUIValue("InGame_Artifact_AlreadyDiscoveredArtifact") + ")</color>") : ("<color=#ffdd80>(" + DewLocalization.GetUIValue("InGame_Artifact_NewArtifact") + ")</color>"));
		SingletonBehaviour<UI_TooltipManager>.instance.ShowTitleDescTooltip(tooltipPivot.position, "<color=" + Dew.GetHex(artifact.mainColor) + ">" + artifactName + "</color> " + suffix, DewLocalization.GetUIValue("InGame_Tooltip_Artifact_Description"));
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			icon.transform.DOKill(complete: true);
			icon.transform.DOPunchScale(Vector3.one * 0.2f, 0.4f);
			_nextShineTime = Time.time;
			DewEffect.PlayNew(fxTouch);
		}
	}

	public override bool CanBeFocused()
	{
		if (base.CanBeFocused())
		{
			return !NetworkedManagerBase<ConversationManager>.instance.hasOngoingLocalConversation;
		}
		return false;
	}
}
