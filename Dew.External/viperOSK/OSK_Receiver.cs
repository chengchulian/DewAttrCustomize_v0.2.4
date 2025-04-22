using TMPro;
using UnityEngine;
using UnityEngine.Events;
using viperTools;

namespace viperOSK;

public class OSK_Receiver : MonoBehaviour
{
	protected string text = "";

	public int textLimit = 20;

	public TMP_Text textReceiver;

	[HideInInspector]
	public int cursorIndex;

	protected Vector2Int cursorSel = new Vector2Int(-1, -1);

	public I_OSK_Cursor cursor;

	public bool interactable;

	public bool allowTextSelection;

	public Color32 normalColor;

	public Color32 highlightColor;

	public string charMask = "";

	public bool useCharMask;

	public UnityEvent<string> OnSubmit;

	public UnityEvent<string> OnValueChanged;

	public UnityEvent<string> onFocus;

	public UnityEvent<string> onLostFocus;

	private bool hasFocus;

	public virtual int selection
	{
		get
		{
			if (cursorSel.x < 0 || cursorSel.y < 0)
			{
				return 0;
			}
			return cursorSel.y - cursorSel.x;
		}
	}

	private void Awake()
	{
		text = "";
		if (base.gameObject.TryGetComponent<TMP_Text>(out textReceiver))
		{
			textReceiver.color = normalColor;
			cursor = base.transform.GetComponentInChildren<I_OSK_Cursor>();
			if (cursor == null)
			{
				Debug.LogWarning("This TMPro text object does not have a cursor. If you want full edit functionality add a OSK_Cursor prefab as a child");
			}
			OnFocusLost();
		}
		else
		{
			Debug.LogWarning("OSK_Receiver must be in the same gameobject as the TextMeshPro text object receiving text input");
		}
	}

	private void Start()
	{
		useCharMask = charMask.Length > 0;
	}

	private void LateUpdate()
	{
	}

	public void OnMouseDown()
	{
		if (interactable && cursor != null && allowTextSelection)
		{
			Deselect();
			cursorSel.x = Selection(viperInput.GetPointerPos());
			cursorSel.x = Mathf.Min(cursorSel.x, textReceiver.text.Length - 1);
		}
	}

	public void OnMouseUp()
	{
		if (!hasFocus)
		{
			OSK_Keyboard keyboard = Object.FindObjectOfType<OSK_Keyboard>();
			if (keyboard != null && keyboard.output != this)
			{
				keyboard.SetOutput(this);
			}
			OnFocus();
		}
		if (!interactable || cursor == null)
		{
			return;
		}
		if (cursorSel.x >= 0 && allowTextSelection)
		{
			cursorSel.y = Selection(viperInput.GetPointerPos(), charhit: true);
			if (cursorSel.y < 0)
			{
				cursorSel.y = Selection(viperInput.GetPointerPos());
			}
			cursorIndex = cursorSel.y + ((cursorSel.x < cursorSel.y) ? 1 : 0);
			cursorSel.y = Mathf.Min(cursorSel.y, textReceiver.text.Length - 1);
			if (cursorSel.x == cursorSel.y)
			{
				ref Vector2Int reference = ref cursorSel;
				int x = (cursorSel.y = -1);
				reference.x = x;
			}
			else
			{
				int temp = cursorSel.x;
				cursorSel.x = Mathf.Min(cursorSel.x, cursorSel.y);
				cursorSel.y = Mathf.Max(temp, cursorSel.y);
				SelectionHighlight(highlightColor);
			}
		}
		else
		{
			Deselect();
			cursorIndex = Selection(viperInput.GetPointerPos());
		}
	}

	public virtual int Selection(Vector3 hitpoint, bool charhit = false)
	{
		Camera cam = Camera.main;
		if (charhit)
		{
			return TMP_TextUtilities.FindIntersectingCharacter(textReceiver, hitpoint, cam, visibleOnly: false);
		}
		return TMP_TextUtilities.GetCursorIndexFromPosition(textReceiver, hitpoint, cam);
	}

	public virtual void Deselect()
	{
		textReceiver.text = ((charMask.Length > 0 && useCharMask) ? new string(charMask[0], text.Length) : text);
		SelectionHighlight(normalColor, all: true);
		ref Vector2Int reference = ref cursorSel;
		int x = (cursorSel.y = -1);
		reference.x = x;
	}

	public virtual void SelectionHighlight(Color32 c, bool all = false)
	{
		if (selection != 0)
		{
			_ = textReceiver.textInfo;
			int start = cursorSel.x;
			int end = cursorSel.y;
			if (all)
			{
				start = 0;
				end = textReceiver.text.Length - 1;
			}
			for (int i = start; i <= end; i++)
			{
				int meshIndex = textReceiver.textInfo.characterInfo[i].materialReferenceIndex;
				int vertexIndex = textReceiver.textInfo.characterInfo[i].vertexIndex;
				Color32[] colors = textReceiver.textInfo.meshInfo[meshIndex].colors32;
				colors[vertexIndex] = c;
				colors[vertexIndex + 1] = c;
				colors[vertexIndex + 2] = c;
				colors[vertexIndex + 3] = c;
			}
			textReceiver.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}
	}

	public virtual void Submit()
	{
		OnSubmit.Invoke(text);
	}

	public virtual void ValueChanged()
	{
		OnValueChanged.Invoke(text);
	}

	public virtual void SetText(string newText)
	{
		newText = newText.Substring(0, Mathf.Min(textLimit, newText.Length));
		text = newText;
		if (charMask.Length > 0 && useCharMask)
		{
			textReceiver.text = new string(charMask[0], text.Length);
		}
		else
		{
			textReceiver.text = newText;
		}
		cursorIndex = text.Length;
		ValueChanged();
	}

	public virtual void AddText(string newchar)
	{
		NewLineFix();
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
			return;
		}
		newchar = newchar.Substring(0, 1);
		string displayChar = ((charMask.Length > 0 && useCharMask) ? charMask.Substring(0, 1) : newchar);
		if (selection > 0)
		{
			textReceiver.text = textReceiver.text.Remove(cursorSel.x, selection + 1);
			textReceiver.text = textReceiver.text.Insert(cursorSel.x, displayChar);
			text = text.Remove(cursorSel.x, selection);
			text = text.Insert(cursorSel.x, newchar);
			cursorIndex = cursorSel.x + 1;
			ref Vector2Int reference = ref cursorSel;
			int x = (cursorSel.y = -1);
			reference.x = x;
		}
		else if ((text.Length == 0 || cursorIndex >= text.Length) && textReceiver.text.Length < textLimit)
		{
			textReceiver.text += displayChar;
			text += newchar;
			cursorIndex++;
		}
		else if (textReceiver.text.Length < textLimit)
		{
			textReceiver.text = textReceiver.text.Insert(cursorIndex, displayChar);
			text = text.Insert(cursorIndex, newchar);
			cursorIndex++;
		}
		ValueChanged();
	}

	public virtual void NewLine()
	{
		if (selection > 0)
		{
			textReceiver.text = textReceiver.text.Remove(cursorSel.x, selection + 1);
			textReceiver.text = textReceiver.text.Insert(cursorSel.x, "\n");
			text = text.Remove(cursorSel.x, selection);
			text = text.Insert(cursorSel.x, "\n");
			cursorIndex = cursorSel.x + 1;
			ref Vector2Int reference = ref cursorSel;
			int x = (cursorSel.y = -1);
			reference.x = x;
		}
		else if ((text.Length == 0 || cursorIndex >= text.Length) && textReceiver.text.Length < textLimit)
		{
			textReceiver.text += "\n\u00a0";
			textReceiver.ForceMeshUpdate();
			text += "\n";
			cursorIndex++;
		}
		else if (textReceiver.text.Length < textLimit)
		{
			textReceiver.text = textReceiver.text.Insert(cursorIndex, "\n");
			text = text.Insert(cursorIndex, "\n");
			cursorIndex++;
		}
		cursor.Cursor();
		ValueChanged();
	}

	private void NewLineFix()
	{
		if (textReceiver.text.EndsWith("\u00a0"))
		{
			textReceiver.text = textReceiver.text.Remove(textReceiver.text.Length - 1);
		}
	}

	public virtual string Text()
	{
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public virtual string ParsedText()
	{
		return textReceiver.GetParsedText();
	}

	public virtual void OnFocus()
	{
		hasFocus = true;
		onFocus.Invoke(Text());
		if (textReceiver != null && text != null && text.Length == 0 && textReceiver.text.Length > 0)
		{
			text = textReceiver.text;
			cursorIndex = text.Length;
		}
		if (cursor != null)
		{
			cursor.Show(show: true);
		}
	}

	public virtual void ToggleCharMask()
	{
		ToggleCharMask(!useCharMask);
	}

	public virtual void ToggleCharMask(bool on_off_charmask)
	{
		useCharMask = on_off_charmask;
		if (useCharMask)
		{
			textReceiver.text = new string(charMask[0], text.Length);
		}
		else
		{
			textReceiver.text = text;
		}
	}

	public virtual bool isFocused()
	{
		return hasFocus;
	}

	public virtual void OnFocusLost()
	{
		hasFocus = false;
		onLostFocus.Invoke(Text());
		Deselect();
		if (cursor != null)
		{
			cursor.Show(show: false);
		}
	}

	public virtual void Backspace()
	{
		if (selection > 0)
		{
			textReceiver.text = textReceiver.text.Remove(cursorSel.x, selection + 1);
			text = text.Remove(cursorSel.x, selection + 1);
			cursorIndex = cursorSel.x;
			ref Vector2Int reference = ref cursorSel;
			int x = (cursorSel.y = -1);
			reference.x = x;
		}
		else if (cursorIndex > 0)
		{
			textReceiver.text = textReceiver.text.Remove(Mathf.Min(cursorIndex - 1, textReceiver.text.Length - 1), 1);
			text = text.Remove(Mathf.Min(cursorIndex - 1, text.Length - 1), 1);
			cursorIndex--;
		}
	}

	public virtual void Del()
	{
		if (selection > 0)
		{
			textReceiver.text = textReceiver.text.Remove(cursorSel.x, selection + 1);
			text = text.Remove(cursorSel.x, selection + 1);
			cursorIndex = cursorSel.x;
			ref Vector2Int reference = ref cursorSel;
			int x = (cursorSel.y = -1);
			reference.x = x;
		}
		else if (cursorIndex <= textReceiver.text.Length - 1)
		{
			textReceiver.text = textReceiver.text.Remove(cursorIndex, 1);
			text = text.Remove(cursorIndex, 1);
		}
	}

	public virtual void ClearText()
	{
		textReceiver.text = "";
		text = "";
		ValueChanged();
	}
}
