using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using viperTools;

namespace viperOSK;

[ExecuteInEditMode]
public class OSK_Keyboard : MonoBehaviour
{
	public enum KEYBOARD_WRAP
	{
		NO_WRAP,
		XY_WRAP,
		X_WRAP,
		Y_WRAP,
		X_CASCADE
	}

	public bool bypassDefaultInput;

	public bool generateOnStart;

	protected bool hasFocus = true;

	public OSK_Receiver output;

	public GameObject KeyPrefab;

	public Transform topLeft;

	public Color keyLabelColor = Color.white;

	public Vector3 keySize;

	public TMP_FontAsset keyFont;

	public bool caps;

	public bool shift;

	public bool acceptPhysicalKeyboard;

	[Header("Gamepad/Joystick")]
	public bool acceptGamePadInput;

	public KEYBOARD_WRAP gamepadKeyboardWrap;

	public Color highlighterColor = new Color(0.7f, 0.5f, 0.5f);

	private OSK_Key currentSelectedKey;

	protected Vector2Int DpadSelection;

	protected float inputTimer;

	protected float inputTimerThreshold = 0.15f;

	[Header("Sound Effects")]
	public bool soundFX;

	protected Action<int> sound;

	protected Action selectSound;

	[Header("Keys Layout and Settings")]
	protected List<List<OSK_Key>> keyLayout = new List<List<OSK_Key>>();

	[TextArea(15, 6)]
	public string layout = "1 2 3 4 5 6 7 8 9 0 \n Q W E R T Y U I O P \n A S D F G H J K L Exclaim \n Z X C V B N M Period Backspace \n LeftShift Space Return";

	public List<OSK_SpecialKeys> specialKeys = new List<OSK_SpecialKeys>
	{
		new OSK_SpecialKeys(KeyCode.Backspace, "«", new Color(0.93f, 0.77f, 0.22f), 2f),
		new OSK_SpecialKeys(KeyCode.Return, "Send", new Color(0.21f, 0.57f, 0.74f), 2f),
		new OSK_SpecialKeys(KeyCode.LeftShift, "Shft", new Color(0.93f, 0.77f, 0.22f), 2f),
		new OSK_SpecialKeys(KeyCode.Space, " ", new Color(0.93f, 0.77f, 0.22f), 4f)
	};

	public List<OSK_KeyTypeMeta> keyTypeMeta = new List<OSK_KeyTypeMeta>(4)
	{
		new OSK_KeyTypeMeta(OSK_KEY_TYPES.DIGIT, new Color(0.4f, 0.4f, 0.4f)),
		new OSK_KeyTypeMeta(OSK_KEY_TYPES.LETTER, new Color(0.35f, 0.35f, 0.35f)),
		new OSK_KeyTypeMeta(OSK_KEY_TYPES.PUNCTUATION, new Color(0.45f, 0.45f, 0.45f)),
		new OSK_KeyTypeMeta(OSK_KEY_TYPES.CONTROLS, new Color(0.55f, 0.55f, 0.55f))
	};

	protected Dictionary<KeyCode, OSK_SpecialKeys> keySounds = new Dictionary<KeyCode, OSK_SpecialKeys>();

	protected Dictionary<KeyCode, I_OSK_Key> keyDict = new Dictionary<KeyCode, I_OSK_Key>();

	public virtual bool HasKey(KeyCode k)
	{
		if (keyDict != null)
		{
			return keyDict.ContainsKey(k);
		}
		return false;
	}

	public virtual void AddText(string newText)
	{
		output.AddText(newText);
	}

	public virtual void AddNewLine()
	{
		output.NewLine();
	}

	public virtual void AddText_ShftEnabled(string newText)
	{
		if (shift || caps)
		{
			output.AddText(newText.ToUpperInvariant());
		}
		else
		{
			output.AddText(newText);
		}
	}

	public virtual string Text()
	{
		return output.Text();
	}

	public virtual void HasFocus(bool isFocus)
	{
		hasFocus = isFocus;
	}

	public virtual void SetOutput(OSK_Receiver newOutput)
	{
		if (output != null)
		{
			output.OnFocusLost();
		}
		output = newOutput;
		output.OnFocus();
	}

	protected void AcceptPhysicalKeyboard(bool accept)
	{
		if (accept)
		{
			viperInput.RegisterKeyStrokeCallback(OnPhysicalKeyStroke, enable: true);
		}
		else
		{
			viperInput.RegisterKeyStrokeCallback(OnPhysicalKeyStroke, enable: false);
		}
	}

	protected void Prep()
	{
		if (output != null)
		{
			output.OnFocus();
		}
		OSK_KeySounds ks = GetComponent<OSK_KeySounds>();
		if (ks != null)
		{
			sound = (Action<int>)Delegate.Combine(sound, new Action<int>(ks.PlaySound));
			selectSound = (Action)Delegate.Combine(selectSound, new Action(ks.PlaySelectKeySound));
		}
		if (Application.isPlaying)
		{
			if (generateOnStart)
			{
				Generate();
			}
			else
			{
				Traverse();
			}
			currentSelectedKey = base.transform.GetComponentInChildren<OSK_Key>();
		}
	}

	public void LoadLayout(string lay)
	{
		layout = lay;
	}

	public KeyCode GetKeyCode(string c)
	{
		KeyCode k = KeyCode.None;
		if (OSK_Keymap.chartoKeycode.TryGetValue(c, out k))
		{
			return k;
		}
		try
		{
			string prefix = "";
			if (c[0] >= '0' && c[0] <= '9')
			{
				prefix = "Alpha";
			}
			return (KeyCode)Enum.Parse(typeof(KeyCode), prefix + c);
		}
		catch (Exception ex)
		{
			Debug.LogError("OSK_Keyboard could not parse the layout for this key (" + c + ") make sure you are using KeyCode names for the characters\nException error: " + ex.Message);
			return KeyCode.None;
		}
	}

	public virtual void Generate()
	{
		OSK_Key[] list = base.transform.GetComponentsInChildren<OSK_Key>();
		if (list.Length != 0)
		{
			OSK_Key[] array = list;
			for (int i = 0; i < array.Length; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(array[i].gameObject);
			}
		}
		keyDict.Clear();
		string[] chars = layout.Split(' ');
		Vector3 keyOffset = new Vector3(keySize.x, 0f - keySize.y, keySize.z);
		Vector3 v = topLeft.localPosition;
		keyLayout.Clear();
		int lx = 0;
		int ly = 0;
		keyLayout.Add(new List<OSK_Key>());
		for (int j = 0; j < chars.Length; j++)
		{
			if (chars[j][0] == '\n')
			{
				v.x = topLeft.localPosition.x;
				v.y += keyOffset.y;
				ly++;
				lx = 0;
				keyLayout.Add(new List<OSK_Key>());
			}
			else if (chars[j].Contains("Skip"))
			{
				string ns = string.Join("", from item in chars[j].ToCharArray()
					where char.IsDigit(item) || item == '.' || item == '-'
					select item);
				if (ns.Length > 0)
				{
					if (float.TryParse(ns, out var n))
					{
						v.x += keySize.x * n;
					}
					else
					{
						v.x += keySize.x;
					}
				}
			}
			else
			{
				if (chars[j].Length < 1)
				{
					continue;
				}
				GameObject k = global::UnityEngine.Object.Instantiate(KeyPrefab, base.transform);
				OSK_Key thisKey = k.GetComponent<OSK_Key>();
				if (keyFont != null)
				{
					thisKey.KeyFont(keyFont);
				}
				k.name = chars[j];
				KeyCode key = GetKeyCode(chars[j]);
				OSK_SpecialKeys special = specialKeys.Find((OSK_SpecialKeys item) => item.keycode == key);
				OSK_KEY_TYPES keyType = OSK_KeyTypeMeta.KeyType(key);
				if (special != null)
				{
					thisKey.Assign(key, keyType, special.name);
					thisKey.SetColors(special.col, keyLabelColor);
					Vector3 keysz = keySize;
					keysz.x *= special.x_size;
					thisKey.BackScale(keysz);
					v.x += keysz.x * 0.5f;
					k.transform.localPosition = v;
					v.x += keysz.x * 0.5f;
					thisKey.SetLayoutLocation(lx, ly);
					for (int l = 0; (float)l < special.x_size; l++)
					{
						keyLayout[ly].Add(thisKey);
						lx++;
					}
					if (special.keySoundCode >= 0 && !keySounds.ContainsKey(key))
					{
						keySounds.Add(key, special);
					}
					if (special.specialAction.GetPersistentEventCount() == 0)
					{
						thisKey.callBack.AddListener(KeyCallBase);
						thisKey.callBack.AddListener(KeyCall);
					}
					else
					{
						thisKey.callBack = special.specialAction;
						thisKey.callBack.AddListener(KeyCallBase);
					}
				}
				else
				{
					if (base.gameObject.name == "KeyBoard (2.1)")
					{
						Debug.Log("regular key " + key);
					}
					thisKey.Assign(key, keyType);
					OSK_KeyTypeMeta ktColor = keyTypeMeta.Find((OSK_KeyTypeMeta item) => item.keyType == keyType);
					if (ktColor != null)
					{
						thisKey.SetColors(ktColor.col, keyLabelColor);
					}
					thisKey.BackScale(keySize);
					v.x += keySize.x * 0.5f;
					k.transform.localPosition = v;
					v.x += keySize.x * 0.5f;
					keyLayout[ly].Add(thisKey);
					thisKey.SetLayoutLocation(lx, ly);
					lx++;
					thisKey.callBack.AddListener(KeyCallBase);
					thisKey.callBack.AddListener(KeyCall);
				}
				keyDict.Add(thisKey.key, thisKey);
				if (Application.isEditor && shift)
				{
					thisKey.ShiftUp();
				}
			}
		}
		if (Application.isPlaying)
		{
			if (shift || caps)
			{
				BroadcastMessage("ShiftUp");
			}
			else
			{
				BroadcastMessage("ShiftDown");
			}
		}
	}

	public virtual void Traverse()
	{
		OSK_Key[] list = base.transform.GetComponentsInChildren<OSK_Key>();
		if (list.Length == 0)
		{
			return;
		}
		keyDict.Clear();
		keyLayout.Clear();
		OSK_Key[] array = list;
		foreach (OSK_Key k in array)
		{
			keyDict.Add(k.key, k);
		}
		string[] chars = layout.Split(' ');
		int lx = 0;
		int ly = 0;
		keyLayout.Add(new List<OSK_Key>());
		for (int j = 0; j < chars.Length; j++)
		{
			if (chars[j][0] == '\n')
			{
				ly++;
				lx = 0;
				keyLayout.Add(new List<OSK_Key>());
			}
			else
			{
				if (chars[j].Contains("Skip") || chars[j].Length < 1)
				{
					continue;
				}
				KeyCode key = GetKeyCode(chars[j]);
				OSK_Key thisKey = list.First((OSK_Key item) => item.key == key);
				if (!(thisKey != null))
				{
					continue;
				}
				thisKey.SetLayoutLocation(lx, ly);
				for (int l = 0; (float)l < thisKey.getXSize(); l++)
				{
					keyLayout[ly].Add(thisKey);
					lx++;
				}
				OSK_SpecialKeys special = specialKeys.Find((OSK_SpecialKeys item) => item.keycode == key);
				if (special != null)
				{
					if (special.keySoundCode >= 0 && !keySounds.ContainsKey(key))
					{
						keySounds.Add(key, special);
					}
					if (special.specialAction.GetPersistentEventCount() == 0)
					{
						thisKey.callBack.AddListener(KeyCallBase);
						thisKey.callBack.AddListener(KeyCall);
					}
					else
					{
						thisKey.callBack = special.specialAction;
						thisKey.callBack.AddListener(KeyCallBase);
					}
				}
				else
				{
					thisKey.callBack.AddListener(KeyCallBase);
					thisKey.callBack.AddListener(KeyCall);
				}
			}
		}
		if (Application.isPlaying)
		{
			if (shift || caps)
			{
				BroadcastMessage("ShiftUp");
			}
			else
			{
				BroadcastMessage("ShiftDown");
			}
		}
	}

	protected void ClickSound(int keytypecode)
	{
		if (soundFX)
		{
			sound(keytypecode);
		}
	}

	public void SelectSound()
	{
		if (soundFX)
		{
			selectSound();
		}
	}

	protected void OutputTextUpdate(string newchar, OSK_Receiver receiver)
	{
		if (base.gameObject.activeSelf)
		{
			if (receiver == null)
			{
				output.AddText(newchar);
			}
			else
			{
				receiver.AddText(newchar);
			}
		}
	}

	public virtual void KeyCallBase(KeyCode k, OSK_Receiver receiver)
	{
		if (!hasFocus || !HasKey(k))
		{
			return;
		}
		if (keySounds.ContainsKey(k))
		{
			ClickSound(keySounds[k].keySoundCode);
			return;
		}
		OSK_KeyTypeMeta ktm = keyTypeMeta.Find((OSK_KeyTypeMeta item) => item.keyType == keyDict[k].KeyType());
		if (ktm != null)
		{
			ClickSound(ktm.keySoundCode);
		}
		else
		{
			ClickSound(0);
		}
	}

	public virtual void KeyCall(KeyCode k, OSK_Receiver receiver)
	{
		if (!hasFocus || !keyDict.ContainsKey(k))
		{
			return;
		}
		switch (k)
		{
		case KeyCode.Backspace:
			KeyBackspace(receiver);
			break;
		case KeyCode.Delete:
			KeyDelete(receiver);
			break;
		case KeyCode.Return:
			if (receiver != null)
			{
				receiver.Submit();
			}
			else if (output != null)
			{
				output.Submit();
			}
			break;
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			KeyShift();
			break;
		case KeyCode.CapsLock:
			caps = !caps;
			shift = !caps;
			KeyShift();
			break;
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
		case KeyCode.RightAlt:
		case KeyCode.LeftAlt:
			break;
		default:
		{
			string n = ((shift || caps) ? ((char)k).ToString().ToUpper() : ((char)k).ToString());
			OutputTextUpdate(n, receiver);
			if (!caps && shift)
			{
				KeyShift();
			}
			break;
		}
		}
	}

	public virtual void KeyBackspace(OSK_Receiver receiver)
	{
		if (receiver != null)
		{
			if (receiver.Text().Length > 0)
			{
				OutputTextUpdate("\b", receiver);
			}
		}
		else if (output.Text().Length > 0)
		{
			OutputTextUpdate("\b", null);
		}
	}

	public virtual void KeyDelete(OSK_Receiver receiver)
	{
		if (receiver != null)
		{
			if (receiver.Text().Length > 0)
			{
				OutputTextUpdate("\u007f", receiver);
			}
		}
		else if (output.Text().Length > 0)
		{
			OutputTextUpdate("\u007f", null);
		}
	}

	public virtual void Submit()
	{
		output.Submit();
	}

	public virtual void KeyShift()
	{
		shift = !shift;
		if (shift)
		{
			BroadcastMessage("ShiftUp");
		}
		else
		{
			BroadcastMessage("ShiftDown");
		}
	}

	public virtual void ButtonA()
	{
		((I_OSK_Key)keyLayout[DpadSelection.y][DpadSelection.x])?.Click((OSK_Receiver)null);
		StartCoroutine(ReHighlightKey());
	}

	protected IEnumerator ReHighlightKey()
	{
		yield return new WaitForSecondsRealtime(0.2f);
		currentSelectedKey.Highlight(hi: true, highlighterColor);
	}

	public virtual void SetSelectedKey(KeyCode k)
	{
		if (HasKey(k))
		{
			if (currentSelectedKey != null)
			{
				currentSelectedKey.Highlight(hi: false, Color.white);
			}
			currentSelectedKey = (OSK_Key)keyDict[k];
			DpadSelection = currentSelectedKey.GetLayoutLocation();
			currentSelectedKey.Highlight(hi: true, highlighterColor);
		}
	}

	public virtual void SetSelectedKey(string c)
	{
		KeyCode k = GetKeyCode(c);
		SetSelectedKey(k);
	}

	public OSK_Key GetSelectedKey()
	{
		return currentSelectedKey;
	}

	public OSK_Key GetOSKKey(string k)
	{
		KeyCode keycode = GetKeyCode(k);
		OSK_Key key = null;
		if (HasKey(keycode))
		{
			key = (OSK_Key)keyDict[keycode];
		}
		return key;
	}

	public virtual void DpadMove(Vector2 dir)
	{
		OSK_Key k = keyLayout[DpadSelection.y][DpadSelection.x];
		k.Highlight(hi: false, Color.white);
		dir.x = Mathf.RoundToInt(dir.x);
		dir.y = Mathf.RoundToInt(dir.y);
		if (dir.y != 0f)
		{
			DpadSelection.y += (int)dir.y;
			if (DpadSelection.y < 0)
			{
				if (gamepadKeyboardWrap == KEYBOARD_WRAP.Y_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
				{
					DpadSelection.y = keyLayout.Count - 1;
				}
				else
				{
					DpadSelection.y = 0;
				}
			}
			else if (DpadSelection.y >= keyLayout.Count)
			{
				if (gamepadKeyboardWrap == KEYBOARD_WRAP.Y_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
				{
					DpadSelection.y = 0;
				}
				else
				{
					DpadSelection.y = keyLayout.Count - 1;
				}
			}
		}
		if (dir.x != 0f)
		{
			int x = DpadSelection.x;
			do
			{
				x += (int)dir.x;
				if (x < 0)
				{
					if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
					{
						x = keyLayout[DpadSelection.y].Count - 1;
					}
					else if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_CASCADE)
					{
						DpadSelection.y = Mathf.Clamp(DpadSelection.y - 1, 0, keyLayout.Count - 1);
						x = keyLayout[DpadSelection.y].Count - 1;
					}
					else
					{
						x = 0;
					}
					break;
				}
				if (x >= keyLayout[DpadSelection.y].Count)
				{
					if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
					{
						x = 0;
					}
					else if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_CASCADE)
					{
						DpadSelection.y = Mathf.Clamp(DpadSelection.y + 1, 0, keyLayout.Count - 1);
						x = 0;
					}
					else
					{
						x = keyLayout[DpadSelection.y].Count - 1;
					}
					break;
				}
			}
			while (keyLayout[DpadSelection.y][x].key == k.key);
			DpadSelection.x = x;
		}
		try
		{
			DpadSelection.Clamp(new Vector2Int(0, 0), new Vector2Int(keyLayout[DpadSelection.y].Count - 1, keyLayout.Count - 1));
			keyLayout[DpadSelection.y][DpadSelection.x].Highlight(hi: true, highlighterColor);
			currentSelectedKey = keyLayout[DpadSelection.y][DpadSelection.x];
			if (k != keyLayout[DpadSelection.y][DpadSelection.x])
			{
				SelectSound();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Exemption in moving through OSK layout DpadSel=" + DpadSelection.ToString() + " x=" + DpadSelection.x + " y=" + DpadSelection.y + "\ne=" + ex.Message);
		}
	}

	public virtual OSK_Key SelectedKeyMove(Vector2 dir, Vector2Int currentLoc, bool makeSoundIfMove = true)
	{
		Vector2Int newLoc = currentLoc;
		OSK_Key k = keyLayout[currentLoc.y][currentLoc.x];
		dir.x = Mathf.RoundToInt(dir.x);
		dir.y = Mathf.RoundToInt(dir.y);
		if (dir.y != 0f)
		{
			newLoc.y += (int)dir.y;
			if (newLoc.y < 0)
			{
				if (gamepadKeyboardWrap == KEYBOARD_WRAP.Y_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
				{
					newLoc.y = keyLayout.Count - 1;
				}
				else
				{
					newLoc.y = 0;
				}
			}
			else if (newLoc.y >= keyLayout.Count)
			{
				if (gamepadKeyboardWrap == KEYBOARD_WRAP.Y_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
				{
					newLoc.y = 0;
				}
				else
				{
					newLoc.y = keyLayout.Count - 1;
				}
			}
		}
		if (dir.x != 0f)
		{
			int x = newLoc.x;
			do
			{
				x += (int)dir.x;
				if (x < 0)
				{
					if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
					{
						x = keyLayout[newLoc.y].Count - 1;
					}
					else if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_CASCADE)
					{
						newLoc.y = Mathf.Clamp(newLoc.y - 1, 0, keyLayout.Count - 1);
						x = keyLayout[newLoc.y].Count - 1;
					}
					else
					{
						x = 0;
					}
					break;
				}
				if (x >= keyLayout[newLoc.y].Count)
				{
					if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
					{
						x = 0;
					}
					else if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_CASCADE)
					{
						newLoc.y = Mathf.Clamp(newLoc.y + 1, 0, keyLayout.Count - 1);
						x = 0;
					}
					else
					{
						x = keyLayout[newLoc.y].Count - 1;
					}
					break;
				}
			}
			while (keyLayout[newLoc.y][x].key == k.key);
			newLoc.x = x;
		}
		try
		{
			newLoc.Clamp(new Vector2Int(0, 0), new Vector2Int(keyLayout[newLoc.y].Count - 1, keyLayout.Count - 1));
			if (newLoc != currentLoc && makeSoundIfMove)
			{
				SelectSound();
			}
			return keyLayout[newLoc.y][newLoc.x];
		}
		catch (Exception ex)
		{
			Debug.LogError("Exemption in moving through OSK layout DpadSel=" + newLoc.ToString() + " x=" + newLoc.x + " y=" + newLoc.y + "\ne=" + ex.Message);
		}
		return null;
	}

	protected virtual void OnPhysicalKeyStroke(char c)
	{
		if (OSK_Keymap.chartoKeycode.TryGetValue(c.ToString(), out var k) && keyDict.TryGetValue(k, out var key))
		{
			key.Click();
		}
	}

	protected void InputFromPointerDevice()
	{
		Vector2 pointPos = viperInput.GetPointerPos();
		if (viperInput.PointerDown() && Physics.Raycast(Camera.main.ScreenPointToRay(pointPos), out var hit))
		{
			hit.transform.gameObject.GetComponent<I_OSK_Key>()?.Click();
		}
	}

	public virtual void GamepadInput_Horizontal(float f)
	{
		DpadMove(Vector2.right * f);
	}

	public virtual void GamepadInput_Vertical(float f)
	{
		DpadMove(Vector2.down * f);
	}

	public virtual void GamepadInput_Submit()
	{
		ButtonA();
	}

	public virtual void GamepadInput_Cancel()
	{
		KeyBackspace(null);
	}

	private void Awake()
	{
		Prep();
	}

	private void Start()
	{
		if (output != null)
		{
			output.OnFocus();
		}
	}

	private void OnGUI()
	{
		if (hasFocus && Application.isPlaying && acceptPhysicalKeyboard && Event.current.isKey && keyDict.ContainsKey(Event.current.keyCode))
		{
			keyDict[Event.current.keyCode].Click(output);
		}
	}

	private void Update()
	{
		if (!hasFocus || bypassDefaultInput || !Application.isPlaying)
		{
			return;
		}
		InputFromPointerDevice();
		if (!acceptGamePadInput || keyLayout.Count <= 0)
		{
			return;
		}
		inputTimer += Time.unscaledDeltaTime;
		if (inputTimer > inputTimerThreshold)
		{
			inputTimer = 0f;
			if (viperInput.GetAxis(AXIS_INPUT.DPAD_Y) != 0f)
			{
				DpadMove(Vector2.down * viperInput.GetAxis(AXIS_INPUT.DPAD_Y));
				viperInput.ResetAllAxis();
			}
			else if (viperInput.GetAxis(AXIS_INPUT.LEFTSTICK_Y) != 0f)
			{
				DpadMove(Vector2.down * viperInput.GetAxis(AXIS_INPUT.LEFTSTICK_Y));
				viperInput.ResetAllAxis();
			}
			else if (viperInput.GetAxis(AXIS_INPUT.DPAD_X) != 0f)
			{
				DpadMove(Vector2.right * viperInput.GetAxis(AXIS_INPUT.DPAD_X));
				viperInput.ResetAllAxis();
			}
			else if (viperInput.GetAxis(AXIS_INPUT.LEFTSTICK_X) != 0f)
			{
				DpadMove(Vector2.right * viperInput.GetAxis(AXIS_INPUT.LEFTSTICK_X));
				viperInput.ResetAllAxis();
			}
		}
	}
}
