using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace viperOSK;

public class OSK_UI_Cursor : MonoBehaviour, I_OSK_Cursor
{
	private bool blink;

	public float bps;

	private Vector3 startingCursorPos;

	private Vector3 cursorPos;

	public OSK_Receiver input;

	public TMP_Text textComponent;

	private TMP_TextInfo textInfo;

	private TMP_CharacterInfo charInfo;

	public Image cursorImg;

	private Color cursorImgColor;

	private void Start()
	{
		if (input == null)
		{
			input = GetComponentInParent<OSK_Receiver>();
		}
		if (textComponent == null)
		{
			textComponent = GetComponentInParent<TMP_Text>();
		}
		if (cursorImg == null)
		{
			cursorImg = GetComponent<Image>();
		}
		if (input == null)
		{
			Debug.LogError("viperOSK OSK_UI_Cursor object must be the child of a OSK_UI_CustomReceiver object to work");
		}
		if (textComponent == null)
		{
			Debug.LogError("viperOSK OSK_UI_Cursor object must be the child of a TextMeshPro object to work");
		}
		if (cursorImg == null)
		{
			Debug.LogError("viperOSK OSK_UI_Cursor object must have a UI.Image component to work");
		}
		else
		{
			cursorImgColor = cursorImg.color;
		}
		textComponent.text = "A";
		textComponent.ForceMeshUpdate();
		startingCursorPos = textComponent.textInfo.characterInfo[0].vertex_BL.position;
		startingCursorPos.y = (textComponent.textInfo.characterInfo[0].vertex_BR.position.y + textComponent.textInfo.characterInfo[0].vertex_TR.position.y) * 0.5f;
		textComponent.text = "";
		textInfo = textComponent.textInfo;
		StartCoroutine(BlinkCoroutine());
	}

	public void Cursor()
	{
		if (textComponent.text.Length == 0)
		{
			cursorPos = startingCursorPos;
		}
		else if (input.cursorIndex >= 0 && textInfo.characterCount > 0)
		{
			charInfo = textInfo.characterInfo[Mathf.Clamp(input.cursorIndex, 0, textInfo.characterCount - 1)];
			cursorPos.x = ((input.cursorIndex < textInfo.characterCount) ? charInfo.bottomLeft.x : charInfo.bottomRight.x);
			cursorPos.y = (charInfo.bottomRight.y + charInfo.topRight.y) * 0.5f;
		}
		GetComponent<RectTransform>().localPosition = new Vector3(cursorPos.x + cursorImg.rectTransform.rect.width * 0.5f, cursorPos.y, cursorPos.z);
	}

	private IEnumerator BlinkCoroutine()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		while (true)
		{
			yield return new WaitForSecondsRealtime(bps);
			if (base.isActiveAndEnabled)
			{
				cursorImg.color = new Color(cursorImg.color.r, cursorImg.color.g, cursorImg.color.b, 0f);
			}
			yield return new WaitForSecondsRealtime(bps);
			if (base.isActiveAndEnabled)
			{
				cursorImg.color = cursorImgColor;
			}
		}
	}

	public void Show(bool show)
	{
		base.gameObject.SetActive(show);
	}

	private void Update()
	{
		Cursor();
	}
}
