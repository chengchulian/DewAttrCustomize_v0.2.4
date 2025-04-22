using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace viperOSK;

public class OSK_UI_Key : MonoBehaviour, I_OSK_Key
{
	public KeyCode key;

	public UnityEvent<KeyCode, OSK_Receiver> callBack;

	public OSK_KEY_TYPES keyType;

	private OSK_Receiver tmpOutput;

	public TextMeshProUGUI keyName;

	public Button bk;

	public bool isPressed;

	private float lastPressed;

	public float x_size;

	private Color bk_baseColor;

	private Vector2Int layoutLoc;

	public object GetObject()
	{
		return base.gameObject;
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
		SetBkColor(bk_color);
		keyName.color = label_color;
	}

	public void SetBkColor(Color bk_color, bool reset_base_color = true)
	{
		if (reset_base_color)
		{
			bk_baseColor = bk_color;
		}
		ColorBlock cb = bk.colors;
		cb.normalColor = bk_color;
		cb.selectedColor = bk_color;
		cb.highlightedColor = bk_color + new Color(0.1f, 0.1f, 0.1f, 1f);
		cb.pressedColor = bk_color - new Color(0.1f, 0.1f, 0.1f, 1f);
		bk.colors = cb;
	}

	public void BackScale(Vector3 scale)
	{
		bk.image.rectTransform.sizeDelta = scale;
		GetComponent<BoxCollider>().size = scale;
	}

	public float getXSize()
	{
		return x_size;
	}

	public void OnPressed()
	{
		if (!isPressed)
		{
			base.transform.localPosition = base.transform.localPosition + new Vector3(0.025f, -0.05f, 0f);
			isPressed = true;
		}
	}

	public void OnDepressed()
	{
		if (isPressed)
		{
			callBack.Invoke(key, tmpOutput);
			lastPressed = Time.time;
			base.transform.localPosition = base.transform.localPosition - new Vector3(0.025f, -0.05f, 0f);
			isPressed = false;
		}
	}

	public void Click(OSK_Receiver inputfield = null)
	{
		if (!isPressed)
		{
			tmpOutput = inputfield;
			OnPressed();
			StartCoroutine(ClickCoroutine());
		}
	}

	public void Click()
	{
		if (!isPressed)
		{
			tmpOutput = null;
			OnPressed();
			StartCoroutine(ClickCoroutine());
		}
	}

	private IEnumerator ClickCoroutine()
	{
		yield return new WaitForSecondsRealtime(0.15f);
		OnDepressed();
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
		if (hi)
		{
			SetBkColor(c, reset_base_color: false);
		}
		else
		{
			SetBkColor(bk_baseColor);
		}
	}

	private OSK_UI_Key Dir(int x, int y)
	{
		OSK_UI_Key u = null;
		if (x > 0)
		{
			u = bk.FindSelectableOnRight().GetComponent<OSK_UI_Key>();
		}
		if (x < 0)
		{
			u = bk.FindSelectableOnLeft().GetComponent<OSK_UI_Key>();
		}
		if (y > 0)
		{
			u = bk.FindSelectableOnUp().GetComponent<OSK_UI_Key>();
		}
		if (x < 0)
		{
			u = bk.FindSelectableOnDown().GetComponent<OSK_UI_Key>();
		}
		return u;
	}

	private void Start()
	{
		if (keyName == null)
		{
			keyName = base.transform.GetComponentInChildren<TextMeshProUGUI>();
		}
		if (bk == null)
		{
			bk = base.transform.GetComponent<Button>();
		}
		if (callBack == null)
		{
			callBack.AddListener(base.transform.GetComponentInParent<OSK_UI_Keyboard>().KeyCall);
		}
	}

	private void Update()
	{
	}
}
