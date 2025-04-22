using TMPro;
using UnityEngine;
using viperOSK;

namespace viperOSK_Examples;

public class Example5UI : MonoBehaviour
{
	public OSK_UI_Keyboard keyboard;

	public TextMeshProUGUI label_viperOSKReceiver;

	public TextMeshProUGUI label_UnityInputField;

	public GameObject arrowRight;

	public GameObject arrowLeft;

	private float t;

	private void Start()
	{
		if (keyboard == null)
		{
			keyboard = Object.FindObjectOfType<OSK_UI_Keyboard>();
			if (keyboard == null)
			{
				Debug.LogError("keyboard needs to be assigned an OSK_Keyboard, you can do this in the inspector");
			}
		}
		Reset();
	}

	private void Update()
	{
	}

	public void print_viperOSK(string s)
	{
		label_viperOSKReceiver.text = s;
	}

	public void print_UnityInputField(string s)
	{
		label_UnityInputField.text = s;
	}

	public void select_viperOSKInputField()
	{
		arrowLeft.SetActive(value: true);
		arrowRight.SetActive(value: false);
	}

	public void select_UnityInputField()
	{
		arrowLeft.SetActive(value: false);
		arrowRight.SetActive(value: true);
	}

	public void ShowHideKeyboard(bool show)
	{
		keyboard.ShowHideKeyboard(show);
	}

	public void Reset()
	{
		ShowHideKeyboard(show: false);
		arrowLeft.SetActive(value: false);
		arrowRight.SetActive(value: false);
	}
}
