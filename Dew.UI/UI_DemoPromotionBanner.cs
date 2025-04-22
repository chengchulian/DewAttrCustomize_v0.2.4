using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DemoPromotionBanner : MonoBehaviour, ILangaugeChangedCallback
{
	public CanvasGroup titleCg;

	public TextMeshProUGUI titleText;

	public CanvasGroup subtitleCg;

	public TextMeshProUGUI subtitleText;

	public Image qrImage;

	public GameObject qrObject;

	public Sprite steamQr;

	public Sprite stoveQr;

	public GameObject stovePlatformIconObject;

	public float widthWithQr;

	public float widthNoQr;

	private List<string> _titleKeys = new List<string>();

	private LayoutElement _layout;

	private int _currentTitleIndex = -1;

	private void Start()
	{
		_layout = GetComponent<LayoutElement>();
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			qrObject.SetActive(value: true);
			qrImage.sprite = ((DewBuildProfile.current.platform == PlatformType.STOVE) ? stoveQr : steamQr);
			_layout.preferredWidth = widthWithQr;
			_titleKeys.Add("PromotionBanner_Title_Genre");
			_titleKeys.Add("PromotionBanner_Title_WishlistUs");
		}
		else
		{
			qrObject.SetActive(value: false);
			_layout.preferredWidth = widthNoQr;
			_titleKeys.Add("PromotionBanner_Title_MultipleEndingNewCharactersAndLucidDreams");
			_titleKeys.Add("PromotionBanner_Title_MoreMemoriesAndEssencesWillBeAdded");
			_titleKeys.Add("PromotionBanner_Title_WishlistUs");
			_titleKeys.Add("PromotionBanner_Title_NewMapsNewBossesMoreChallenges");
		}
		if (GetComponentInParent<LayoutGroup>() == null)
		{
			ContentSizeFitter contentSizeFitter = base.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		}
		stovePlatformIconObject.SetActive(DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth) || DewBuildProfile.current.platform == PlatformType.STOVE);
		UpdateTitleText();
		UpdateSubtitleText();
	}

	private void ShowNextTitle()
	{
		_currentTitleIndex++;
		UpdateTitleText();
	}

	private void UpdateTitleText()
	{
		if (_titleKeys.Count != 0)
		{
			_currentTitleIndex %= _titleKeys.Count;
			titleText.text = DewLocalization.GetUIValue(_titleKeys[_currentTitleIndex]);
		}
	}

	private void UpdateSubtitleText()
	{
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			subtitleText.text = DewLocalization.GetUIValue("PromotionBanner_Subtitle_DemoIsAvailableNowComingIn2025");
		}
		else
		{
			subtitleText.text = DewLocalization.GetUIValue("PromotionBanner_Subtitle_AddtoWishlistComingIn2025");
		}
	}

	private void OnEnable()
	{
		StartCoroutine(FlickerRoutine());
		StartCoroutine(SequenceRoutine());
		IEnumerator FlickerRoutine()
		{
			while (true)
			{
				subtitleCg.alpha = 0.6f + Mathf.Sin(Time.time * 3f) * 0.4f;
				yield return null;
			}
		}
		IEnumerator SequenceRoutine()
		{
			_currentTitleIndex = 0;
			UpdateTitleText();
			while (true)
			{
				for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * 3f)
				{
					titleCg.alpha = t;
					yield return null;
				}
				titleCg.alpha = 1f;
				yield return new WaitForSeconds(3.75f);
				for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * 3f)
				{
					titleCg.alpha = 1f - t;
					yield return null;
				}
				titleCg.alpha = 0f;
				ShowNextTitle();
			}
		}
	}

	public void OpenSteamStore()
	{
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			return;
		}
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				rawContent = DewLocalization.GetUIValue("PromotionBanner_ConfirmOpenStorePage"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						Open();
					}
				}
			});
		}
		else
		{
			Open();
		}
		static void Open()
		{
			if (DewBuildProfile.current.platform == PlatformType.STEAM && SteamManagerBase.Initialized)
			{
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(2444750u), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			}
			else
			{
				Dew.OpenURL("https://store.steampowered.com/app/2444750/Shape_of_Dreams/");
			}
		}
	}

	public void OnLanguageChanged()
	{
		UpdateTitleText();
		UpdateSubtitleText();
	}
}
