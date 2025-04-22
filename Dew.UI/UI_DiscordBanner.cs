using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DiscordBanner : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ILangaugeChangedCallback
{
	private void Start()
	{
		OnLanguageChanged();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			return;
		}
		string url = ((DewSave.profile.language == "ko-KR") ? "https://discord.com/invite/Fp2rKwtmKZ" : "https://discord.com/invite/PEsbaxuSzS");
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				rawContent = DewLocalization.GetUIValue("PromotionBanner_ConfirmOpenExternalLink"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						Dew.OpenURL(url);
					}
				}
			});
		}
		else
		{
			Dew.OpenURL(url);
		}
	}

	public void OnLanguageChanged()
	{
		bool isDiscordAvailable = true;
		base.gameObject.SetActive(isDiscordAvailable);
	}
}
