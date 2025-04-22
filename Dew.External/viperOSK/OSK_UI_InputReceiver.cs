using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace viperOSK;

public class OSK_UI_InputReceiver : OSK_Receiver, ISubmitHandler, IEventSystemHandler, IPointerClickHandler, IDragHandler
{
	public enum OSK_RECEIVER
	{
		NONE,
		INPUTFIELD,
		TMPRO_INPUTFIELD
	}

	private InputField inputReceiver;

	private TMP_InputField inputTMPReceiver;

	private OSK_RECEIVER receiver;

	public UnityEvent onSelectClick;

	private void Awake()
	{
	}

	private void Start()
	{
		if (TryGetComponent<InputField>(out inputReceiver))
		{
			inputReceiver.keyboardType = (TouchScreenKeyboardType)(-1);
			receiver = OSK_RECEIVER.INPUTFIELD;
		}
		else if (TryGetComponent<TMP_InputField>(out inputTMPReceiver))
		{
			inputTMPReceiver.keyboardType = (TouchScreenKeyboardType)(-1);
			receiver = OSK_RECEIVER.TMPRO_INPUTFIELD;
		}
		else
		{
			Debug.LogError("viperOSK does not have a valid text receiver. Ensure you have a valid receiver (or create one) and attach this component to the GameObject");
		}
		if (charMask.Length > 0)
		{
			useCharMask = true;
		}
	}

	private void TMPInputFieldReActivate()
	{
		inputTMPReceiver.interactable = true;
	}

	public int SelectionEnd()
	{
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
		{
			int start = Mathf.Min(inputReceiver.caretPosition, inputReceiver.selectionAnchorPosition);
			int end = Mathf.Max(inputReceiver.caretPosition, inputReceiver.selectionAnchorPosition);
			if (Mathf.Abs(end - start) > 0)
			{
				return end;
			}
			return -1;
		}
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
		{
			int start = Mathf.Min(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition);
			int end = Mathf.Max(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition);
			if (Mathf.Abs(end - start) > 0)
			{
				return end;
			}
			return -1;
		}
		default:
			return -1;
		}
	}

	public override void Submit()
	{
		OnSubmit.Invoke(Text());
	}

	public override void AddText(string newchar)
	{
		switch (newchar)
		{
		case "\b":
			Backspace();
			ValueChanged();
			return;
		case "\u007f":
			Del();
			ValueChanged();
			return;
		case "\n":
			NewLine();
			ValueChanged();
			return;
		}
		string displaychar = ((charMask.Length > 0 && useCharMask) ? charMask.Substring(0, 1) : newchar);
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
		{
			if (newchar.Length <= 0 || inputReceiver.text.Length >= textLimit)
			{
				break;
			}
			int caret_ = ((inputReceiver.caretPosition != cursorIndex) ? cursorIndex : inputReceiver.caretPosition);
			int start2 = cursorSel.x;
			int sel2 = Mathf.Abs(cursorSel.y - start2);
			if (sel2 > 0)
			{
				inputReceiver.text = inputReceiver.text.Remove(start2, sel2);
				inputReceiver.text = inputReceiver.text.Insert(start2, displaychar);
				inputReceiver.caretPosition = start2 + 1;
				text = text.Remove(start2, sel2);
				text = text.Insert(start2, newchar);
			}
			else if (caret_ < text.Length && caret_ >= 0)
			{
				text = text.Insert(caret_, newchar);
				if (useCharMask)
				{
					inputReceiver.text = new string(displaychar[0], text.Length);
				}
				else
				{
					inputReceiver.text = text;
				}
				inputReceiver.caretPosition = caret_ + 1;
			}
			else
			{
				inputReceiver.text += displaychar;
				inputReceiver.caretPosition = inputReceiver.text.Length + 1;
				text += newchar;
			}
			cursorIndex = inputReceiver.caretPosition;
			cursorSel = -1 * Vector2Int.one;
			break;
		}
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
			if (newchar.Length > 0 && inputTMPReceiver.text.Length < textLimit && (inputTMPReceiver.lineLimit == 0 || inputTMPReceiver.textComponent.textInfo.lineCount <= inputTMPReceiver.lineLimit))
			{
				int start = Mathf.Min(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition);
				int sel = Mathf.Abs(Mathf.Max(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition) - start);
				if (sel > 0)
				{
					text = text.Remove(start, sel);
					text = text.Insert(start, newchar);
					if (useCharMask)
					{
						inputTMPReceiver.text = new string(displaychar[0], text.Length);
					}
					else
					{
						inputTMPReceiver.text = text;
					}
					inputTMPReceiver.caretPosition = start + 1;
				}
				else if (inputTMPReceiver.caretPosition < text.Length && inputTMPReceiver.caretPosition >= 0)
				{
					int caret = inputTMPReceiver.caretPosition;
					text = text.Insert(inputTMPReceiver.caretPosition, newchar);
					if (useCharMask)
					{
						inputTMPReceiver.text = new string(displaychar[0], text.Length);
					}
					else
					{
						inputTMPReceiver.text = text;
					}
					inputTMPReceiver.ForceLabelUpdate();
					inputTMPReceiver.caretPosition = caret + 1;
				}
				else
				{
					text += newchar;
					if (useCharMask)
					{
						inputTMPReceiver.text = new string(displaychar[0], text.Length);
					}
					else
					{
						inputTMPReceiver.text = text;
					}
					inputTMPReceiver.caretPosition = text.Length;
				}
			}
			cursorIndex = inputTMPReceiver.caretPosition;
			break;
		}
		ValueChanged();
	}

	public override string Text()
	{
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public override string ParsedText()
	{
		return receiver switch
		{
			OSK_RECEIVER.INPUTFIELD => inputReceiver.text, 
			OSK_RECEIVER.TMPRO_INPUTFIELD => inputTMPReceiver.text, 
			_ => "", 
		};
	}

	public override void ToggleCharMask(bool on_off_charmask)
	{
		useCharMask = on_off_charmask;
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			if (useCharMask)
			{
				inputReceiver.text = new string(charMask[0], text.Length);
			}
			else
			{
				inputReceiver.text = text;
			}
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
			if (useCharMask)
			{
				inputTMPReceiver.text = new string(charMask[0], text.Length);
			}
			else
			{
				inputTMPReceiver.text = text;
			}
			break;
		}
	}

	public override void OnFocus()
	{
		OSK_RECEIVER oSK_RECEIVER = receiver;
		if (oSK_RECEIVER != OSK_RECEIVER.INPUTFIELD)
		{
			_ = 2;
		}
	}

	public override void OnFocusLost()
	{
	}

	public override void NewLine()
	{
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			if (inputReceiver.text.Length < textLimit && inputTMPReceiver.textComponent.textInfo.lineCount < inputTMPReceiver.lineLimit)
			{
				int caret_ = ((inputReceiver.caretPosition != cursorIndex) ? cursorIndex : inputReceiver.caretPosition);
				int start2 = cursorSel.x;
				int sel2 = Mathf.Abs(cursorSel.y - start2);
				if (sel2 > 0)
				{
					inputReceiver.text = inputReceiver.text.Remove(start2, sel2);
					inputReceiver.text = inputReceiver.text.Insert(start2, "\n");
					inputReceiver.caretPosition = start2 + 1;
					text = text.Remove(start2, sel2);
					text = text.Insert(start2, "\n");
				}
				else if (caret_ < text.Length && caret_ >= 0)
				{
					text = text.Insert(caret_, "\n");
					inputReceiver.caretPosition = caret_ + 1;
				}
				else
				{
					inputReceiver.text += "\n";
					inputReceiver.caretPosition = inputReceiver.text.Length + 1;
					text += "\n";
				}
				cursorIndex = inputReceiver.caretPosition;
				cursorSel = -1 * Vector2Int.one;
			}
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
			if (inputTMPReceiver.text.Length < textLimit && inputTMPReceiver.textComponent.textInfo.lineCount < inputTMPReceiver.lineLimit)
			{
				int start = Mathf.Min(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition);
				int sel = Mathf.Abs(Mathf.Max(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition) - start);
				if (sel > 0)
				{
					text = text.Remove(start, sel);
					text = text.Insert(start, "\n");
					inputTMPReceiver.caretPosition = start + 1;
				}
				else if (inputTMPReceiver.caretPosition < text.Length && inputTMPReceiver.caretPosition >= 0)
				{
					int caret = inputTMPReceiver.caretPosition;
					text = text.Insert(inputTMPReceiver.caretPosition, "\n");
					inputTMPReceiver.ForceLabelUpdate();
					inputTMPReceiver.caretPosition = caret + 1;
				}
				else
				{
					text += "\n";
					inputTMPReceiver.text += "\n";
					inputTMPReceiver.caretPosition = text.Length;
				}
			}
			cursorIndex = inputTMPReceiver.caretPosition;
			break;
		}
		ValueChanged();
	}

	public override void Backspace()
	{
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			if (inputReceiver.text.Length > 0)
			{
				int caret_ = ((inputReceiver.caretPosition != cursorIndex) ? cursorIndex : inputReceiver.caretPosition);
				int start2 = cursorSel.x;
				int sel2 = Mathf.Abs(cursorSel.y - start2);
				if (sel2 > 0)
				{
					inputReceiver.text = inputReceiver.text.Remove(start2, sel2);
					text = text.Remove(start2, sel2);
					inputReceiver.caretPosition = start2;
				}
				else
				{
					start2 = Mathf.Max(0, caret_ - 1);
					inputReceiver.text = inputReceiver.text.Remove(start2, 1);
					text = text.Remove(start2, 1);
					inputReceiver.caretPosition = start2;
				}
				cursorIndex = start2;
				cursorSel = -1 * Vector2Int.one;
			}
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
		{
			if (inputTMPReceiver.text.Length <= 0)
			{
				break;
			}
			int start = Mathf.Min(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition);
			int sel = Mathf.Abs(Mathf.Max(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition) - start);
			if (sel > 0)
			{
				text = text.Remove(start, sel);
				if (useCharMask)
				{
					inputTMPReceiver.text = new string(charMask[0], text.Length);
				}
				else
				{
					inputTMPReceiver.text = text;
				}
				inputTMPReceiver.caretPosition = start;
			}
			else
			{
				start = Mathf.Max(0, inputTMPReceiver.caretPosition - 1);
				text = text.Remove(start, 1);
				if (useCharMask)
				{
					inputTMPReceiver.text = new string(charMask[0], text.Length);
				}
				else
				{
					inputTMPReceiver.text = text;
				}
				inputTMPReceiver.caretPosition = start;
			}
			cursorIndex = inputTMPReceiver.caretPosition;
			break;
		}
		}
	}

	public override void Del()
	{
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			if (inputReceiver.text.Length > 0)
			{
				int caret_ = ((inputReceiver.caretPosition != cursorIndex) ? cursorIndex : inputReceiver.caretPosition);
				int start2 = cursorSel.x;
				int sel2 = Mathf.Abs(cursorSel.y - start2);
				if (sel2 > 0)
				{
					inputReceiver.text = inputReceiver.text.Remove(start2, sel2);
					text = text.Remove(start2, sel2);
				}
				else
				{
					start2 = Mathf.Min(inputReceiver.text.Length - 1, caret_);
					inputReceiver.text = inputReceiver.text.Remove(start2, 1);
					text = text.Remove(start2, 1);
					inputReceiver.caretPosition = start2;
				}
				cursorIndex = start2;
				cursorSel = -1 * Vector2Int.one;
			}
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
		{
			if (inputTMPReceiver.text.Length <= 0)
			{
				break;
			}
			int start = Mathf.Min(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition);
			int sel = Mathf.Abs(Mathf.Max(inputTMPReceiver.caretPosition, inputTMPReceiver.selectionAnchorPosition) - start);
			if (sel > 0)
			{
				text = text.Remove(start, sel);
				if (useCharMask)
				{
					inputTMPReceiver.text = new string(charMask[0], text.Length);
				}
				else
				{
					inputTMPReceiver.text = text;
				}
				inputTMPReceiver.caretPosition = start;
			}
			else if (inputTMPReceiver.caretPosition < inputTMPReceiver.text.Length - 1)
			{
				start = Mathf.Min(inputTMPReceiver.text.Length - 1, inputTMPReceiver.caretPosition);
				text = text.Remove(start, 1);
				if (useCharMask)
				{
					inputTMPReceiver.text = new string(charMask[0], text.Length);
				}
				else
				{
					inputTMPReceiver.text = text;
				}
				inputTMPReceiver.caretPosition = start;
			}
			cursorIndex = inputTMPReceiver.caretPosition;
			break;
		}
		}
	}

	public override void ClearText()
	{
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			inputReceiver.text = "";
			text = "";
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
			inputTMPReceiver.text = "";
			text = "";
			break;
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (onSelectClick != null)
		{
			onSelectClick.Invoke();
		}
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			cursorIndex = inputReceiver.caretPosition;
			cursorSel.x = Mathf.Min(inputReceiver.caretPosition, inputReceiver.selectionAnchorPosition);
			cursorSel.y = Mathf.Max(inputReceiver.caretPosition, inputReceiver.selectionAnchorPosition);
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
			cursorIndex = inputTMPReceiver.caretPosition;
			break;
		}
		if (!isFocused())
		{
			OnFocus();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		switch (receiver)
		{
		case OSK_RECEIVER.INPUTFIELD:
			cursorIndex = inputReceiver.caretPosition;
			cursorSel.x = Mathf.Min(inputReceiver.caretPosition, inputReceiver.selectionAnchorPosition);
			cursorSel.y = Mathf.Max(inputReceiver.caretPosition, inputReceiver.selectionAnchorPosition);
			break;
		case OSK_RECEIVER.TMPRO_INPUTFIELD:
			cursorIndex = inputTMPReceiver.caretPosition;
			break;
		}
		if (!isFocused())
		{
			OnFocus();
		}
	}

	void ISubmitHandler.OnSubmit(BaseEventData eventData)
	{
		if (onSelectClick != null)
		{
			onSelectClick.Invoke();
		}
		if (!isFocused())
		{
			OnFocus();
		}
	}
}
