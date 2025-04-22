using System;
using System.Collections;
using DewInternal;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;

public class UI_InGame_Conversations_Instance : MonoBehaviour, IGamepadFocusable, IGamepadFocusListener, IGamepadFocusableOverrideInput
{
	public TextMeshProUGUI nameText;

	public TextMeshProUGUI text;

	public DewAudioSource startSource;

	public DewAudioSource[] clickSources;

	public CanvasGroup advanceGroup;

	public CanvasGroup choicesGroup;

	public UI_InGame_Conversations_Instance_ChoiceDisplay choiceDisplay;

	public ButtonDisplay advanceButton;

	private DewInputTrigger it_advanceConversation;

	private int _nextAudioSource;

	private UI_InGame_FollowAndFaceHero_AboveEntity _follow;

	private TypewriterCore _typewriter;

	private CanvasGroup _cg;

	private uint _id;

	private DewConversationSettings _settings;

	private ShownConversation _shown;

	private ConversationData _data;

	private string _cachedDataKey;

	public static DewBinding GetAdvanceConversationBinding()
	{
		DewBinding dewBinding = DewSave.profile.controls.interact.CloneWith(MouseButton.Left);
		dewBinding.gamepadBinds.Clear();
		dewBinding.gamepadBinds.Add(GamepadButtonEx.A);
		return dewBinding;
	}

	private void Awake()
	{
		it_advanceConversation = new DewInputTrigger
		{
			binding = GetAdvanceConversationBinding,
			owner = this,
			priority = -20,
			isValidCheck = () => _settings != null && _settings.isLocalAuthority,
			checkGameAreaForMouse = true
		};
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			advanceButton.gamepadButton = (ButtonDisplay.GamepadButtonForDisplay)GetAdvanceConversationBinding().gamepadBinds[0];
		}
		else
		{
			advanceButton.mouseButton = GetAdvanceConversationBinding().pcBinds[0].mouse;
			advanceButton.key = GetAdvanceConversationBinding().pcBinds[0].key;
		}
		advanceButton.UpdateButtonDisplay();
	}

	public void Setup(uint id)
	{
		if (!NetworkedManagerBase<ConversationManager>.instance.convSettings.ContainsKey(id))
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		_cg = GetComponent<CanvasGroup>();
		_follow = GetComponent<UI_InGame_FollowAndFaceHero_AboveEntity>();
		_typewriter = text.GetComponent<TypewriterCore>();
		_typewriter.onCharacterVisible.AddListener(delegate(char ch)
		{
			if (!char.IsWhiteSpace(ch))
			{
				clickSources[_nextAudioSource].Stop();
				clickSources[_nextAudioSource].Play();
				_nextAudioSource = (_nextAudioSource + 1) % clickSources.Length;
			}
		});
		_typewriter.onTextShowed.AddListener(delegate
		{
			if (!_settings.isLocalAuthority)
			{
				HideUserInputObjects();
			}
			else
			{
				advanceGroup.SetActivationState(_shown.choices == null);
				choicesGroup.SetActivationState(_shown.choices != null);
				if (choicesGroup.interactable && DewInput.currentMode == InputMode.Gamepad)
				{
					ManagerBase<GlobalUIManager>.instance.SetFocusOnFirstFocusable(choicesGroup.gameObject);
				}
			}
		});
		_id = id;
		_settings = NetworkedManagerBase<ConversationManager>.instance.convSettings[id];
		NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnStopConversation += new Action<uint>(ClientEventOnStopConversation);
		NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnConversationShowLineAndRequestUserInput += new Action<uint, ShownConversation>(ClientEventOnConversationShowLineAndRequestUserInput);
		NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnConversationLineRequestedCompletion += new Action<uint>(ClientEventOnConversationLineRequestedCompletion);
		UI_InGame_Conversations_Instance_ChoiceDisplay uI_InGame_Conversations_Instance_ChoiceDisplay = choiceDisplay;
		uI_InGame_Conversations_Instance_ChoiceDisplay.OnChoiceClick = (Action<int>)Delegate.Combine(uI_InGame_Conversations_Instance_ChoiceDisplay.OnChoiceClick, new Action<int>(ChoiceClick));
		_cg.alpha = 0f;
		_cg.blocksRaycasts = false;
		HideUserInputObjects();
		_follow.stayInSafeArea = _settings.isLocalAuthority;
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			ManagerBase<GlobalUIManager>.instance.SetFocus(this);
		}
	}

	private void ChoiceClick(int chosenIndex)
	{
		NetworkedManagerBase<ConversationManager>.instance.CmdDoUserInputOnConversation(_id, _shown.lineIndex, chosenIndex);
		choicesGroup.alpha = 0f;
		choicesGroup.interactable = false;
		choicesGroup.blocksRaycasts = false;
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			ManagerBase<GlobalUIManager>.instance.SetFocus(this);
		}
	}

	private void ClientEventOnConversationShowLineAndRequestUserInput(uint obj, ShownConversation shown)
	{
		if (obj != _id)
		{
			return;
		}
		_shown = shown;
		if (shown.key != _cachedDataKey)
		{
			_cachedDataKey = shown.key;
			_data = DewLocalization.GetConversationData(shown.key);
		}
		choiceDisplay.Setup(_data, shown.choices);
		LineData line = _data.lines[shown.lineIndex];
		Entity speakerEnt = _settings.speakers[0];
		if (!string.IsNullOrEmpty(line.speaker))
		{
			Entity[] speakers = _settings.speakers;
			foreach (Entity s in speakers)
			{
				if (s.GetType().Name.Contains(line.speaker, StringComparison.InvariantCulture))
				{
					speakerEnt = s;
					break;
				}
			}
		}
		DewAudioClip startSound = speakerEnt.Sound.conversationStart;
		if (startSound != null)
		{
			startSource.Stop();
			startSource.clip = startSound;
			startSource.Play();
		}
		DewAudioClip click = speakerEnt.Sound.conversationClick;
		if (click == null)
		{
			click = Resources.Load<DewAudioClip>("sfxFallbackConversationClick");
		}
		DewAudioSource[] array = clickSources;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].clip = click;
		}
		string inputText = DewLocalization.HighlightKeywords(line.text);
		DewConversationSettings settings = NetworkedManagerBase<ConversationManager>.instance.convSettings[_id];
		int seed = settings.seed;
		seed += shown.key.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
		seed += shown.lineIndex * 10;
		global::System.Random random = new global::System.Random(seed);
		RefValue<string> processed = new RefValue<string>();
		DewLocalizationNodeParser.ParseBacktickedString(inputText, delegate(string normal)
		{
			processed.value += normal;
		}, delegate(string tagType)
		{
			RefValue<string> refValue = processed;
			refValue.value = refValue.value + "<" + tagType + ">";
		}, delegate(string exp)
		{
			if (exp.StartsWith("%"))
			{
				string text;
				string text2;
				if (exp.Contains(":"))
				{
					string[] array2 = exp.Split(":");
					text = array2[0].Substring(1);
					text2 = array2[1];
				}
				else
				{
					text = exp.Substring(1);
					text2 = "";
				}
				if (settings.variables == null || !settings.variables.TryGetValue(text, out var value))
				{
					value = "!" + text;
				}
				if (text2.Equals("ui", StringComparison.InvariantCultureIgnoreCase))
				{
					processed.value += DewLocalization.GetUIValue(value);
				}
				else if (text2.Equals("artifactShortStory", StringComparison.InvariantCultureIgnoreCase))
				{
					string[] artifactShortStory = DewLocalization.GetArtifactShortStory(DewLocalization.GetArtifactKey(value));
					processed.value += artifactShortStory[random.Next(0, artifactShortStory.Length)];
				}
				else
				{
					processed.value += value;
				}
			}
			else
			{
				processed.value += exp;
			}
		});
		HideUserInputObjects();
		_typewriter.ShowText("");
		_typewriter.StartShowingText();
		_typewriter.ShowText(processed);
		if (speakerEnt is Hero && speakerEnt.owner != null && speakerEnt.owner.isHumanPlayer)
		{
			nameText.text = ChatManager.GetDescribedPlayerName(speakerEnt.owner);
		}
		else if (speakerEnt is MockEntity m && !string.IsNullOrEmpty(m.conversationNameUIKey))
		{
			nameText.text = DewLocalization.GetUIValue(m.conversationNameUIKey);
		}
		else
		{
			nameText.text = DewLocalization.GetUIValue(speakerEnt.GetType().Name + "_Name");
		}
		_follow.target = speakerEnt;
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return null;
			_cg.alpha = 1f;
			_cg.blocksRaycasts = true;
			_typewriter.StartShowingText();
		}
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ConversationManager>.instance != null)
		{
			NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnStopConversation -= new Action<uint>(ClientEventOnStopConversation);
			NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnConversationShowLineAndRequestUserInput -= new Action<uint, ShownConversation>(ClientEventOnConversationShowLineAndRequestUserInput);
			NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnConversationLineRequestedCompletion -= new Action<uint>(ClientEventOnConversationLineRequestedCompletion);
		}
	}

	private void Update()
	{
		if (!_settings.isLocalAuthority)
		{
			return;
		}
		if (it_advanceConversation.down)
		{
			AdvanceConversation();
		}
		if (DewInput.currentMode == InputMode.Gamepad && ManagerBase<GlobalUIManager>.instance.focused == null)
		{
			if (choicesGroup.interactable)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocusOnFirstFocusable(choicesGroup.gameObject);
			}
			else
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(this);
			}
		}
	}

	private void AdvanceConversation()
	{
		if (_settings.isLocalAuthority)
		{
			if (_typewriter.isShowingText)
			{
				NetworkedManagerBase<ConversationManager>.instance.CmdRequestLineCompletion(_id, _shown.lineIndex);
			}
			else if (advanceGroup.interactable)
			{
				NetworkedManagerBase<ConversationManager>.instance.CmdDoUserInputOnConversation(_id, _shown.lineIndex, 0);
			}
		}
	}

	private void ClientEventOnConversationLineRequestedCompletion(uint obj)
	{
		if (obj == _id)
		{
			_typewriter.SkipTypewriter();
		}
	}

	private void ClientEventOnStopConversation(uint obj)
	{
		if (obj == _id)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void HideUserInputObjects()
	{
		advanceGroup.SetActivationState(value: false);
		choicesGroup.SetActivationState(value: false);
	}

	public bool CanBeFocused()
	{
		if (NetworkedManagerBase<ConversationManager>.instance.ongoingLocalConversation == _settings)
		{
			return !choicesGroup.interactable;
		}
		return false;
	}

	public SelectionDisplayType GetSelectionDisplayType()
	{
		return SelectionDisplayType.Dont;
	}

	public bool OnGamepadDpadDown()
	{
		return true;
	}

	public bool OnGamepadDpadLeft()
	{
		return true;
	}

	public bool OnGamepadDpadRight()
	{
		return true;
	}

	public bool OnGamepadDpadUp()
	{
		return true;
	}

	public bool OnGamepadConfirm()
	{
		AdvanceConversation();
		return true;
	}
}
