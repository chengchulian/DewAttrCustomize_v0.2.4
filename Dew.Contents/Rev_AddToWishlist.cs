using Steamworks;

public class Rev_AddToWishlist : DewReverieItem
{
	public override int grantedStardust => 50;

	public override bool excludeFromPool => true;

	public override string reverieListButtonText => DewLocalization.GetUIValue("Rev_AddToWishlist_Action");

	public override void OnReverieListButtonClick()
	{
		base.OnReverieListButtonClick();
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				rawContent = DewLocalization.GetUIValue("PromotionBanner_ConfirmOpenStorePage"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						Open();
						Complete();
					}
				}
			});
		}
		else
		{
			Open();
			Complete();
		}
		static void Open()
		{
			if (DewBuildProfile.current.platform == PlatformType.STEAM && SteamManagerBase.Initialized)
			{
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(2444750u), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			}
			else
			{
				Dew.OpenURL("https://store.steampowered.com/app/2444750/Shape_of_Dream/");
			}
		}
	}
}
