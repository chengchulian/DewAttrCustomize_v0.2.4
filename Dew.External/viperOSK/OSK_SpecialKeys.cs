using System;
using UnityEngine;
using UnityEngine.Events;

namespace viperOSK;

[Serializable]
public class OSK_SpecialKeys
{
	public KeyCode keycode;

	public string name;

	public Color col;

	public float x_size;

	public int keySoundCode;

	public UnityEvent<KeyCode, OSK_Receiver> specialAction;

	public OSK_SpecialKeys(KeyCode k, string n, Color c, float s)
	{
		keycode = k;
		name = n;
		col = c;
		x_size = s;
	}
}
