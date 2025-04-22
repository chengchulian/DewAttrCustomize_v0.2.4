using UnityEngine;
using UnityEngine.UI;

public class UI_Title_TitleView : View
{
	public enum StateType
	{
		None,
		Coop,
		Extras
	}

	public GameObject blocker;

	public GameObject mainSection;

	public CanvasGroup mainButtonsGroup;

	public Button returnToMainButton;

	public GameObject coopObject;

	public GameObject extraObject;

	public StateType state { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		if (Application.IsPlaying(this))
		{
			returnToMainButton.onClick.AddListener(delegate
			{
				SetState(StateType.None);
			});
		}
	}

	protected override void Start()
	{
		base.Start();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		OnStateChanged();
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, -1, delegate
		{
			if (ManagerBase<UIManager>.instance.IsState("Title") && state != 0)
			{
				SetState(StateType.None);
				return true;
			}
			return false;
		});
	}

	public void SetState(StateType newState)
	{
		if (state != newState)
		{
			state = newState;
			OnStateChanged();
		}
	}

	public void EnterCoopOptions()
	{
		ManagerBase<TitleManager>.instance.CheckTutorial(delegate
		{
			SetState(StateType.Coop);
		});
	}

	public void EnterExtras()
	{
		SetState(StateType.Extras);
	}

	private void OnStateChanged()
	{
		ManagerBase<TitleManager>.instance.onlineCamera.SetActive(state == StateType.Coop);
		ManagerBase<TitleManager>.instance.extrasCamera.SetActive(state == StateType.Extras);
		returnToMainButton.gameObject.SetActive(state != StateType.None);
		coopObject.SetActive(state == StateType.Coop);
		extraObject.SetActive(state == StateType.Extras);
		mainButtonsGroup.gameObject.SetActive(state == StateType.None);
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			Dew.CallDelayed(delegate
			{
				ManagerBase<GlobalUIManager>.instance.SetFocusOnFirstFocusable(mainSection);
			});
		}
	}
}
