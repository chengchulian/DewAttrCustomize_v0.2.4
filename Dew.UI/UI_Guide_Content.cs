using System;
using System.Linq;
using System.Reflection;
using DewInternal;
using TMPro;
using UnityEngine;

public class UI_Guide_Content : MonoBehaviour
{
	public int index;

	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		string inputText = DewLocalization.HighlightKeywords(DewLocalization.GetUIValue(base.transform.parent.name + "_" + index));
		_text.text = "";
		DewLocalizationNodeParser.ParseBacktickedString(inputText, delegate(string t)
		{
			_text.text += t;
		}, delegate(string tagType)
		{
			TextMeshProUGUI text = _text;
			text.text = text.text + "<" + tagType + ">";
		}, delegate(string backticked)
		{
			string[] array = backticked.Split(",");
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split("|");
				string text2 = array2[0];
				string text3 = ((array2.Length > 1) ? DewLocalizationNodeParser.GetFormat(array2[1]) : "0");
				if (text2.Contains("::"))
				{
					string[] array3 = text2.Split("::");
					Type type = Type.GetType(array3[0]);
					if (type == null)
					{
						foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies().Reverse())
						{
							type = item.GetType(array3[0]);
							if (type != null)
							{
								break;
							}
						}
					}
					float num = float.Parse(type.GetField(array3[1], BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetRawConstantValue().ToString());
					_text.text += num.ToString(text3);
					break;
				}
				if ((text2 == "ping" || text2 == "emote") && DewInput.currentMode == InputMode.Gamepad)
				{
					if (DewSave.profile.controls.leftJoystickClickAction == JoystickClickAction.PingAndEmotes)
					{
						TextMeshProUGUI text4 = _text;
						text4.text = text4.text + "<color=#ae2>" + DewInput.GetReadableTextForCurrentMode(DewBinding.GamepadOnly(GamepadButtonEx.LeftStick)) + "</color>";
					}
					else if (DewSave.profile.controls.rightJoystickClickAction == JoystickClickAction.PingAndEmotes)
					{
						TextMeshProUGUI text5 = _text;
						text5.text = text5.text + "<color=#ae2>" + DewInput.GetReadableTextForCurrentMode(DewBinding.GamepadOnly(GamepadButtonEx.RightStick)) + "</color>";
					}
					else
					{
						TextMeshProUGUI text6 = _text;
						text6.text = text6.text + "<color=#ae2>" + DewInput.GetReadableTextForCurrentMode(DewBinding.MockBinding) + "</color>";
					}
					break;
				}
				DewBinding dewBinding = (DewBinding)typeof(DewControlSettings).GetField(text2).GetValue(DewSave.profile.controls);
				if (dewBinding.HasAssignedForCurrentMode() || i == array.Length - 1)
				{
					TextMeshProUGUI text7 = _text;
					text7.text = text7.text + "<color=#ae2>" + DewInput.GetReadableTextForCurrentMode(dewBinding) + "</color>";
					break;
				}
			}
		});
	}
}
