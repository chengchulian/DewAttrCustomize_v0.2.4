using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

public class UI_Common_MenuView : View, IMenuView
{
	public enum StateType
	{
		None,
		MenuList,
		Settings,
		Guide
	}

	public string[] availableStates;

	public View menuListView;

	public View settingsView;

	public View guideView;

	private string _resumeState;

	private bool _isQuitting;

	public StateType state { get; private set; }

	protected override void Start()
	{
		base.Start();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		ManagerBase<GlobalUIManager>.instance.currentMenuView = this;
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 0, delegate
		{
			if (!base.isShowing && !availableStates.Contains(ManagerBase<UIManager>.instance.state))
			{
				return false;
			}
			if (!base.isShowing && showOn.Count > 0 && DewInput.currentMode == InputMode.KeyboardAndMouse)
			{
				ShowMenu();
			}
			else if (base.isShowing)
			{
				HideMenu();
			}
			return true;
		});
	}

	protected override void OnShow()
	{
		base.OnShow();
		SetState(StateType.MenuList);
		Dew.CallDelayed(delegate
		{
			ManagerBase<GlobalUIManager>.instance.SetFocusOnFirstFocusable(base.gameObject);
		});
	}

	protected override void OnHide()
	{
		base.OnHide();
		SetState(StateType.None);
	}

	public void SetState(StateType newState)
	{
		if (state != newState)
		{
			state = newState;
			if (state == StateType.MenuList)
			{
				menuListView.Show();
			}
			else
			{
				menuListView.Hide();
			}
			if (state == StateType.Settings)
			{
				settingsView.Show();
			}
			else
			{
				settingsView.Hide();
			}
			if (state == StateType.Guide)
			{
				guideView.Show();
			}
			else
			{
				guideView.Hide();
			}
		}
	}

	public void SetMenuState()
	{
		if (!_isQuitting)
		{
			SetState(StateType.MenuList);
		}
	}

	public void OpenSettings()
	{
		if (!_isQuitting)
		{
			SetState(StateType.Settings);
		}
	}

	public void OpenGuide()
	{
		if (!_isQuitting)
		{
			SetState(StateType.Guide);
		}
	}

	public void ClickResume()
	{
		if (!_isQuitting)
		{
			ManagerBase<UIManager>.instance.SetState(_resumeState);
		}
	}

	public void ClickQuitToMenu()
	{
		if (_isQuitting)
		{
			return;
		}
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			rawContent = DewLocalization.GetUIValue("Menu_Message_QuitToMenuConfirm"),
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
			defaultButton = DewMessageSettings.ButtonType.No,
			destructiveConfirm = true,
			onClose = delegate(DewMessageSettings.ButtonType button)
			{
				if (!_isQuitting && button == DewMessageSettings.ButtonType.Yes)
				{
					_isQuitting = true;
					if (NetworkServer.active && NetworkedManagerBase<GameManager>.instance != null)
					{
						NetworkedManagerBase<GameManager>.instance.SetDisconnectedForEveryoneElse();
					}
					DewSave.SaveProfile();
					DewSave.SavePlatformSettings();
					if (DewNetworkManager.instance != null)
					{
						DewNetworkManager.instance.EndSession();
					}
					else if (ManagerBase<CollectablesManager>.instance != null)
					{
						ManagerBase<CollectablesManager>.instance.TransitionToScene("Title");
					}
				}
			}
		});
	}

	public void ClickQuitToDesktop()
	{
		if (_isQuitting)
		{
			return;
		}
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			rawContent = DewLocalization.GetUIValue("Menu_Message_QuitToDesktopConfirm"),
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
			defaultButton = DewMessageSettings.ButtonType.No,
			destructiveConfirm = true,
			onClose = delegate(DewMessageSettings.ButtonType button)
			{
				if (!_isQuitting && button == DewMessageSettings.ButtonType.Yes)
				{
					ManagerBase<LogicUpdateManager>.instance.StartCoroutine(Routine());
				}
			}
		});
		IEnumerator Routine()
		{
			_isQuitting = true;
			EOSManager.ResetEOSOnTitle = false;
			if (NetworkServer.active && NetworkedManagerBase<GameManager>.instance != null)
			{
				NetworkedManagerBase<GameManager>.instance.SetDisconnectedForEveryoneElse();
			}
			DewSave.SaveProfile();
			DewSave.SavePlatformSettings();
			if (DewNetworkManager.instance != null)
			{
				DewNetworkManager.instance.EndSession();
			}
			yield return new WaitWhile(() => NetworkServer.active || NetworkClient.active);
			Dew.QuitApplication();
		}
	}

	public bool IsShowing()
	{
		return base.isShowing;
	}

	public bool CanShowMenu()
	{
		if (!base.isShowing)
		{
			return availableStates.Contains(ManagerBase<UIManager>.instance.state);
		}
		return false;
	}

	public void ShowMenu()
	{
		_resumeState = ManagerBase<UIManager>.instance.state;
		ManagerBase<UIManager>.instance.SetState(showOn[0]);
	}

	public void HideMenu()
	{
		ManagerBase<UIManager>.instance.SetState(_resumeState);
	}
}
