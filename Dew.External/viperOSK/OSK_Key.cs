using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace viperOSK;

public class OSK_Key : MonoBehaviour, I_OSK_Key
{
	public KeyCode key;

	public UnityEvent<KeyCode, OSK_Receiver> callBack;

	public OSK_KEY_TYPES keyType;

	private OSK_Receiver tmpOutput;

	public TextMeshPro keyName;

	public SpriteRenderer bk;

	public bool isPressed;

	private float lastPressed;

	private Color bk_baseColor;

	private Vector2Int layoutLoc;

	public object GetObject()
	{
		return this;
	}

	public string GetKeyName()
	{
		return key.ToString();
	}

	public float LastPressed()
	{
		return lastPressed;
	}

	public OSK_KEY_TYPES KeyType()
	{
		return keyType;
	}

	public Transform GetKeyTransform()
	{
		return base.transform;
	}

	public void Assign(KeyCode newKey, OSK_KEY_TYPES ktype, string name = "")
	{
		key = newKey;
		if (name.Length == 0)
		{
			keyName.text = ((char)key).ToString();
		}
		else
		{
			keyName.text = name;
		}
		keyType = ktype;
	}

	public void SetLayoutLocation(int x, int y)
	{
		layoutLoc.x = x;
		layoutLoc.y = y;
	}

	public Vector2Int GetLayoutLocation()
	{
		return layoutLoc;
	}

	public void KeyFont(TMP_FontAsset keyfont)
	{
		keyName.font = keyfont;
	}

	public void SetColors(Color bk_color, Color label_color)
	{
		bk_baseColor = bk_color;
		bk.color = bk_color;
		keyName.color = label_color;
	}

	public void BackScale(Vector3 scale)
	{
		bk.size = scale * 0.9f;
		GetComponent<BoxCollider>().size = scale;
		keyName.rectTransform.sizeDelta = scale * 0.9f;
	}

	public float getXSize()
	{
		return GetComponent<BoxCollider>().size.x;
	}

	public void OnMouseDown()
	{
		if (!isPressed)
		{
			base.transform.localPosition = base.transform.localPosition + new Vector3(0.025f, -0.05f, 0f);
			Color c = bk.color + new Color(0.1f, 0.1f, 0.1f);
			bk.color = c;
			isPressed = true;
		}
	}

	public void OnMouseUp()
	{
		if (isPressed)
		{
			lastPressed = Time.time;
			base.transform.localPosition = base.transform.localPosition - new Vector3(0.025f, -0.05f, 0f);
			bk.color = bk_baseColor;
			isPressed = false;
			callBack.Invoke(key, tmpOutput);
		}
	}

	public void Click(OSK_Receiver inputfield = null)
	{
		if (!isPressed)
		{
			tmpOutput = inputfield;
			OnMouseDown();
			StartCoroutine(ClickCoroutine());
		}
	}

	private IEnumerator ClickCoroutine()
	{
		yield return new WaitForSecondsRealtime(0.15f);
		OnMouseUp();
	}

	public void ShiftUp()
	{
		if (keyType == OSK_KEY_TYPES.LETTER)
		{
			if (keyName.text.Length <= 1)
			{
				keyName.text = keyName.text.ToUpper();
			}
			else
			{
				keyName.text = char.ToUpper(keyName.text[0]) + keyName.text.Substring(1);
			}
		}
	}

	public void ShiftDown()
	{
		if (keyType == OSK_KEY_TYPES.LETTER)
		{
			if (keyName.text.Length <= 1)
			{
				keyName.text = keyName.text.ToLower();
			}
			else
			{
				keyName.text = char.ToLower(keyName.text[0]) + keyName.text.Substring(1);
			}
		}
	}

	public void Highlight(bool hi, Color c)
	{
		if (hi && c.a > 0f)
		{
			bk.color = c;
		}
		else
		{
			bk.color = bk_baseColor;
		}
	}

	private void Start()
	{
		if (keyName == null)
		{
			keyName = base.transform.GetComponentInChildren<TextMeshPro>();
		}
		if (bk == null)
		{
			bk = base.transform.GetComponentInChildren<SpriteRenderer>();
		}
		if (callBack == null)
		{
			callBack.AddListener(base.transform.GetComponentInParent<OSK_Keyboard>().KeyCall);
		}
	}

	private void Update()
	{
	}
}
