using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Lobby_CharacterView : View
{
	public GameObject changeTravelerObject;

	public GameObject changeAppearanceObject;

	protected override void Update()
	{
		base.Update();
		if (!Application.IsPlaying(this) || !LobbyUIManager.instance.IsState(showOn[0]) || ManagerBase<MessageManager>.instance.isShowingMessage || ManagerBase<GlobalUIManager>.instance.isTutorialHighlighting || !DewInput.GetButtonDown(MouseButton.Right, checkGameArea: false))
		{
			return;
		}
		ListReturnHandle<RaycastResult> handle;
		List<RaycastResult> list = Dew.RaycastAllUIElementsBelowCursor(out handle);
		bool shouldGoBack = true;
		foreach (RaycastResult item in list)
		{
			if (item.gameObject.GetComponentInParent<UI_Constellations_StarItem>() != null)
			{
				shouldGoBack = false;
				break;
			}
		}
		handle.Return();
		if (shouldGoBack)
		{
			ManagerBase<GlobalUIManager>.instance.GoBack();
		}
	}

	protected override void OnShow()
	{
		base.OnShow();
		ShowChangeTraveler();
	}

	public void ShowChangeTraveler()
	{
		changeTravelerObject.SetActive(value: true);
		changeAppearanceObject.SetActive(value: false);
	}

	public void ShowChangeAppearance()
	{
		changeTravelerObject.SetActive(value: false);
		changeAppearanceObject.SetActive(value: true);
	}
}
