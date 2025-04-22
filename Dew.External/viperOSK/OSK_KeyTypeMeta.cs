using System;
using UnityEngine;

namespace viperOSK;

[Serializable]
public class OSK_KeyTypeMeta
{
	public OSK_KEY_TYPES keyType;

	public Color col;

	public int keySoundCode;

	public static OSK_KEY_TYPES KeyType(KeyCode key)
	{
		if (char.IsControl((char)key))
		{
			return OSK_KEY_TYPES.CONTROLS;
		}
		if (char.IsDigit((char)key))
		{
			return OSK_KEY_TYPES.DIGIT;
		}
		if (char.IsLetter((char)key) && key != KeyCode.LeftShift && key != KeyCode.RightShift && key != KeyCode.CapsLock)
		{
			return OSK_KEY_TYPES.LETTER;
		}
		if (char.IsPunctuation((char)key) || char.IsSymbol((char)key))
		{
			return OSK_KEY_TYPES.PUNCTUATION;
		}
		return OSK_KEY_TYPES.CONTROLS;
	}

	public OSK_KeyTypeMeta()
	{
	}

	public OSK_KeyTypeMeta(OSK_KEY_TYPES kt, Color c)
	{
		keyType = kt;
		col = c;
	}
}
