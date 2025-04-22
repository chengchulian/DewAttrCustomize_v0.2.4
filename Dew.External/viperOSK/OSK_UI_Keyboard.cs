using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using viperTools;

namespace viperOSK;

[ExecuteInEditMode]
public class OSK_UI_Keyboard : OSK_Keyboard
{
	private new List<List<OSK_UI_Key>> keyLayout = new List<List<OSK_UI_Key>>();

	private OSK_UI_Key currentSelUIKey;

	private OSK_UI_Key nextKey;

	private Transform keyboardAssets;

	public void ShowHideKeyboard(bool show)
	{
		keyboardAssets.gameObject.SetActive(show);
	}

	public override bool HasKey(KeyCode k)
	{
		if (keyDict != null)
		{
			return keyDict.ContainsKey(k);
		}
		return false;
	}

	public override void Generate()
	{
		OSK_UI_Key[] list = base.transform.GetComponentsInChildren<OSK_UI_Key>();
		if (list.Length != 0)
		{
			OSK_UI_Key[] array = list;
			for (int i = 0; i < array.Length; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(array[i].gameObject);
			}
		}
		keyDict.Clear();
		string[] chars = layout.Split(' ');
		RectTransform k_transform = KeyPrefab.GetComponent<RectTransform>();
		Vector3 keyOffset = new Vector3(keySize.x * k_transform.rect.width, keySize.y * k_transform.rect.height, keySize.z);
		Vector3 v = topLeft.localPosition;
		keyLayout.Clear();
		int lx = 0;
		int ly = 0;
		keyLayout.Add(new List<OSK_UI_Key>());
		for (int j = 0; j < chars.Length; j++)
		{
			if (chars[j][0] == '\n')
			{
				v.x = topLeft.localPosition.x;
				v.y -= keyOffset.y;
				ly++;
				lx = 0;
				keyLayout.Add(new List<OSK_UI_Key>());
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
						v.x += keyOffset.x * n;
					}
					else
					{
						v.x += keyOffset.x;
					}
				}
			}
			else
			{
				if (chars[j].Length < 1)
				{
					continue;
				}
				GameObject obj = global::UnityEngine.Object.Instantiate(KeyPrefab, keyboardAssets);
				k_transform = obj.GetComponent<RectTransform>();
				OSK_UI_Key thisKey = obj.GetComponent<OSK_UI_Key>();
				if (keyFont != null)
				{
					thisKey.KeyFont(keyFont);
				}
				obj.name = chars[j];
				KeyCode key = GetKeyCode(chars[j]);
				OSK_SpecialKeys special = specialKeys.Find((OSK_SpecialKeys item) => item.keycode == key);
				OSK_KEY_TYPES keyType = OSK_KeyTypeMeta.KeyType(key);
				if (special != null)
				{
					thisKey.Assign(key, keyType, special.name);
					thisKey.SetColors(special.col, keyLabelColor);
					Vector3 keysz = keyOffset;
					keysz.x *= special.x_size;
					thisKey.x_size = special.x_size;
					thisKey.BackScale(keysz);
					v.x += keysz.x * 0.5f;
					k_transform.localPosition = v;
					v.x += keysz.x * 0.5f;
					thisKey.SetLayoutLocation(lx, ly);
					for (int k = 0; (float)k < special.x_size; k++)
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
					thisKey.Assign(key, keyType);
					OSK_KeyTypeMeta ktColor = keyTypeMeta.Find((OSK_KeyTypeMeta item) => item.keyType == keyType);
					if (ktColor != null)
					{
						thisKey.SetColors(ktColor.col, keyLabelColor);
					}
					thisKey.x_size = 1f;
					thisKey.BackScale(keyOffset);
					v.x += keyOffset.x * 0.5f;
					k_transform.localPosition = v;
					v.x += keyOffset.x * 0.5f;
					keyLayout[ly].Add(thisKey);
					thisKey.SetLayoutLocation(lx, ly);
					lx++;
					thisKey.callBack.AddListener(KeyCallBase);
					thisKey.callBack.AddListener(KeyCall);
				}
				keyDict.Add(key, thisKey);
				ColorBlock keyColors = thisKey.bk.colors;
				keyColors.selectedColor = highlighterColor;
				thisKey.bk.colors = keyColors;
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
		GamepadWrapNavigation();
	}

	public override void Traverse()
	{
		OSK_UI_Key[] list = base.transform.GetComponentsInChildren<OSK_UI_Key>();
		keyDict.Clear();
		keyLayout.Clear();
		OSK_UI_Key[] array = list;
		foreach (OSK_UI_Key k in array)
		{
			keyDict.Add(k.key, k);
		}
		string[] chars = layout.Split(' ');
		int lx = 0;
		int ly = 0;
		keyLayout.Add(new List<OSK_UI_Key>());
		for (int j = 0; j < chars.Length; j++)
		{
			if (chars[j][0] == '\n')
			{
				ly++;
				lx = 0;
				keyLayout.Add(new List<OSK_UI_Key>());
			}
			else
			{
				if (chars[j].Contains("Skip") || chars[j].Length < 1)
				{
					continue;
				}
				string prefix = "";
				if (chars[j][0] >= '0' && chars[j][0] <= '9')
				{
					prefix = "Alpha";
				}
				KeyCode key = KeyCode.Asterisk;
				if (OSK_Keymap.chartoKeycode.ContainsKey(chars[j]))
				{
					key = OSK_Keymap.chartoKeycode[chars[j]];
				}
				else
				{
					try
					{
						key = (KeyCode)Enum.Parse(typeof(KeyCode), prefix + chars[j]);
					}
					catch (Exception ex)
					{
						Debug.LogError("OSK_Keyboard could not parse the layout for this text=" + chars[j] + " make sure you are using KeyCode names for the characters\nException error: " + ex.Message);
					}
				}
				OSK_UI_Key thisKey = list.First((OSK_UI_Key item) => item.key == key);
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
		GamepadWrapNavigation();
	}

	private void GamepadWrapNavigation()
	{
		if (gamepadKeyboardWrap == KEYBOARD_WRAP.NO_WRAP)
		{
			return;
		}
		if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_WRAP)
		{
			for (int j = 0; j < keyLayout.Count; j++)
			{
				Navigation left = keyLayout[j][0].bk.navigation;
				Navigation right = keyLayout[j][keyLayout[j].Count - 1].bk.navigation;
				left.mode = Navigation.Mode.Explicit;
				right.mode = Navigation.Mode.Explicit;
				left.selectOnLeft = keyLayout[j][keyLayout[j].Count - 1].bk;
				left.selectOnRight = keyLayout[j][0].bk.FindSelectableOnRight().gameObject.GetComponent<Button>();
				left.selectOnUp = keyLayout[Mathf.Clamp(j - 1, 0, keyLayout.Count - 1)][0].bk;
				left.selectOnDown = keyLayout[Mathf.Clamp(j + 1, 0, keyLayout.Count - 1)][0].bk;
				right.selectOnRight = keyLayout[j][0].bk;
				right.selectOnLeft = keyLayout[j][keyLayout[j].Count - 1].bk.FindSelectableOnLeft().gameObject.GetComponent<Button>();
				right.selectOnUp = keyLayout[Mathf.Clamp(j - 1, 0, keyLayout.Count - 1)][keyLayout[j].Count - 1].bk;
				right.selectOnDown = keyLayout[Mathf.Clamp(j + 1, 0, keyLayout.Count - 1)][keyLayout[j].Count - 1].bk;
				keyLayout[j][0].bk.navigation = left;
				keyLayout[j][keyLayout[j].Count - 1].bk.navigation = right;
			}
		}
		if (gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.Y_WRAP)
		{
			Debug.LogWarning("Keyboard Wrapping XY_WRAP and Y_WRAP not *presently* supported in UI mode");
		}
		if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_CASCADE)
		{
			for (int i = 0; i < keyLayout.Count; i++)
			{
				Navigation left2 = keyLayout[i][0].bk.navigation;
				Navigation right2 = keyLayout[i][keyLayout[i].Count - 1].bk.navigation;
				left2.mode = Navigation.Mode.Explicit;
				right2.mode = Navigation.Mode.Explicit;
				left2.selectOnLeft = keyLayout[Mathf.Clamp(i - 1, 0, keyLayout.Count - 1)][keyLayout[i].Count - 1].bk;
				left2.selectOnRight = keyLayout[i][0].bk.FindSelectableOnRight().gameObject.GetComponent<Button>();
				left2.selectOnUp = keyLayout[Mathf.Clamp(i - 1, 0, keyLayout.Count - 1)][0].bk;
				left2.selectOnDown = keyLayout[Mathf.Clamp(i + 1, 0, keyLayout.Count - 1)][0].bk;
				right2.selectOnRight = keyLayout[Mathf.Clamp(i + 1, 0, keyLayout.Count - 1)][0].bk;
				right2.selectOnLeft = keyLayout[i][keyLayout[i].Count - 1].bk.FindSelectableOnLeft().gameObject.GetComponent<Button>();
				right2.selectOnUp = keyLayout[Mathf.Clamp(i - 1, 0, keyLayout.Count - 1)][keyLayout[i].Count - 1].bk;
				right2.selectOnDown = keyLayout[Mathf.Clamp(i + 1, 0, keyLayout.Count - 1)][keyLayout[i].Count - 1].bk;
				keyLayout[i][0].bk.navigation = left2;
				keyLayout[i][keyLayout[i].Count - 1].bk.navigation = right2;
			}
		}
	}

	public override void KeyCallBase(KeyCode k, OSK_Receiver receiver)
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

	public override void KeyCall(KeyCode k, OSK_Receiver receiver)
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

	public override void ButtonA()
	{
	}

	public OSK_UI_Key SelectedKey()
	{
		return currentSelUIKey;
	}

	public override void SetSelectedKey(KeyCode k)
	{
		if (!HasKey(k))
		{
			return;
		}
		GameObject keyGameObj = keyDict[k].GetObject() as GameObject;
		currentSelUIKey = keyGameObj.GetComponent<OSK_UI_Key>();
		if (!(currentSelUIKey == null))
		{
			if (EventSystem.current.alreadySelecting)
			{
				nextKey = currentSelUIKey;
				StartCoroutine(SelectKey(nextKey));
			}
			else
			{
				StartCoroutine(SelectKey(currentSelUIKey));
			}
		}
	}

	private IEnumerator SelectKey(OSK_UI_Key selKey)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForSecondsRealtime(0.15f);
		EventSystem.current.SetSelectedGameObject(selKey.gameObject);
	}

	public override void SetSelectedKey(string c)
	{
		KeyCode k = GetKeyCode(c);
		SetSelectedKey(k);
	}

	public void SetSelectedKey(OSK_UI_Key k)
	{
		if (currentSelUIKey != null)
		{
			currentSelUIKey.Highlight(hi: false, Color.white);
		}
		currentSelUIKey = k;
		EventSystem.current.SetSelectedGameObject(currentSelUIKey.gameObject);
	}

	public override void DpadMove(Vector2 dir)
	{
		OSK_UI_Key k = keyLayout[DpadSelection.y][DpadSelection.x];
		k.Highlight(hi: false, Color.white);
		if (dir.y != 0f)
		{
			_ = DpadSelection.y;
			DpadSelection.y += Mathf.RoundToInt(dir.y);
			if ((float)DpadSelection.y < 0f)
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
			float x = DpadSelection.x;
			do
			{
				x += dir.x;
				if (x < 0f)
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
						x = 0f;
					}
					break;
				}
				if (x >= (float)keyLayout[DpadSelection.y].Count)
				{
					if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_WRAP || gamepadKeyboardWrap == KEYBOARD_WRAP.XY_WRAP)
					{
						x = 0f;
					}
					else if (gamepadKeyboardWrap == KEYBOARD_WRAP.X_CASCADE)
					{
						DpadSelection.y = Mathf.Clamp(DpadSelection.y + 1, 0, keyLayout.Count - 1);
						x = 0f;
					}
					else
					{
						x = keyLayout[DpadSelection.y].Count - 1;
					}
					break;
				}
			}
			while (keyLayout[DpadSelection.y][(int)x].key == k.key);
			DpadSelection.x = (int)x;
		}
		currentSelUIKey = keyLayout[DpadSelection.y][DpadSelection.x];
		keyLayout[DpadSelection.y][DpadSelection.x].Highlight(hi: true, highlighterColor);
		currentSelUIKey = keyLayout[DpadSelection.y][DpadSelection.x];
	}

	private void Awake()
	{
		PrepAssetGroup();
		Prep();
	}

	private void Start()
	{
	}

	public void PrepAssetGroup()
	{
		keyboardAssets = base.transform.GetChild(0);
	}

	private void FixedUpdate()
	{
		if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.TryGetComponent<OSK_UI_Key>(out var ui_key))
		{
			if (currentSelUIKey != null && currentSelUIKey != ui_key && viperInput.GetAllAxis() != 0f)
			{
				SelectSound();
			}
			currentSelUIKey = ui_key;
		}
		else
		{
			currentSelUIKey = null;
		}
	}
}
