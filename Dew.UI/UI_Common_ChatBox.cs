using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[LogicUpdatePriority(400)]
public class UI_Common_ChatBox : LogicBehaviour
{
	public TMP_InputField inputField;

	public UI_Common_ChatBox_Item itemTemplate;

	public Transform itemParent;

	public int displayedMessageCount;

	public bool toggleWithChatKey;

	public DewAudioSource chatSound;

	public int backButtonPriority = 20;

	public CanvasGroup scrollBarCg;

	private DewInputTrigger it_scrollUp;

	private DewInputTrigger it_scrollDown;

	private int _chatClosedFrame;

	internal CanvasGroup _cg;

	private Queue<UI_Common_ChatBox_Item> _displayedItems = new Queue<UI_Common_ChatBox_Item>();

	private bool _shouldIgnoreSubmit;

	private void Awake()
	{
		GetComponent(out _cg);
		inputField.onSubmit.AddListener(OnSubmit);
		_cg.interactable = !toggleWithChatKey;
		if (toggleWithChatKey)
		{
			inputField.onDeselect.AddListener(delegate
			{
				inputField.text = "";
				_cg.interactable = false;
			});
			inputField.gameObject.SetActive(value: false);
		}
	}

	private void Start()
	{
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, backButtonPriority, delegate
		{
			if (toggleWithChatKey && _cg.interactable)
			{
				CloseChatBox();
				return true;
			}
			return false;
		});
		it_scrollUp = new DewInputTrigger
		{
			owner = this,
			binding = () => DewBinding.KeyboardAndMouseOnly(MouseButton.ScrollUp),
			priority = -1,
			canConsume = true,
			isValidCheck = () => toggleWithChatKey && _cg.interactable,
			checkGameAreaForMouse = false,
			ignoreConsumeCheck = false
		};
		it_scrollDown = new DewInputTrigger
		{
			owner = this,
			binding = () => DewBinding.KeyboardAndMouseOnly(MouseButton.ScrollDown),
			priority = -1,
			canConsume = true,
			isValidCheck = () => toggleWithChatKey && _cg.interactable,
			checkGameAreaForMouse = false,
			ignoreConsumeCheck = false
		};
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (!(NetworkedManagerBase<ChatManager>.instance == null))
		{
			NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnMessageReceived += new Action<ChatManager.Message>(ClientEventOnMessageReceived);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (NetworkedManagerBase<ChatManager>.instance != null)
		{
			NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnMessageReceived -= new Action<ChatManager.Message>(ClientEventOnMessageReceived);
		}
	}

	private void ClientEventOnMessageReceived(ChatManager.Message obj)
	{
		ScrollRect sRect = GetComponentInChildren<ScrollRect>();
		bool shouldScrollToBottom = sRect.normalizedPosition.y < 0.0001f;
		chatSound.Play();
		UI_Common_ChatBox_Item newItem = global::UnityEngine.Object.Instantiate(itemTemplate, itemParent);
		try
		{
			newItem.Setup(obj, toggleWithChatKey);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		_displayedItems.Enqueue(newItem);
		if (_displayedItems.Count > displayedMessageCount)
		{
			UI_Common_ChatBox_Item deleted = _displayedItems.Dequeue();
			if (deleted != null)
			{
				global::UnityEngine.Object.Destroy(deleted.gameObject);
			}
		}
		if (shouldScrollToBottom)
		{
			Dew.CallDelayed(delegate
			{
				sRect.normalizedPosition = Vector2.zero;
			});
		}
	}

	private void OnSubmit(string arg0)
	{
		if (!_shouldIgnoreSubmit)
		{
			if (arg0.Trim().Length > 0)
			{
				NetworkedManagerBase<ChatManager>.instance.CmdSendChatMessage(inputField.text);
			}
			inputField.text = "";
			if (!toggleWithChatKey)
			{
				inputField.ActivateInputField();
			}
			else
			{
				CloseChatBox();
			}
		}
	}

	private void CloseChatBox()
	{
		_cg.interactable = false;
		_chatClosedFrame = Time.frameCount;
		inputField.DeactivateInputField();
		EventSystem.current.SetSelectedGameObject(null);
		inputField.gameObject.SetActive(value: false);
		GetComponentInChildren<ScrollRect>().normalizedPosition = Vector2.zero;
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (DewInput.GetButtonDown(DewSave.profile.controls.chat, checkGameAreaForMouse: false) && !_cg.interactable && Time.frameCount != _chatClosedFrame && !ControlManager.IsInputFieldFocused())
		{
			_cg.interactable = true;
			inputField.gameObject.SetActive(value: true);
			inputField.ActivateInputField();
			_shouldIgnoreSubmit = true;
		}
		if (toggleWithChatKey)
		{
			scrollBarCg.SetActivationState(_cg.interactable);
			scrollBarCg.blocksRaycasts = false;
		}
		if (toggleWithChatKey && _cg.interactable)
		{
			inputField.ActivateInputField();
		}
		if (_shouldIgnoreSubmit && !DewInput.GetButton(DewSave.profile.controls.chat, checkGameAreaForMouse: false))
		{
			_shouldIgnoreSubmit = false;
		}
	}
}
