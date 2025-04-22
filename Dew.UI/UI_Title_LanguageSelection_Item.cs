using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Title_LanguageSelection_Item : MonoBehaviour
{
	private static readonly Dictionary<string, string> _readableLanguageNames = new Dictionary<string, string>
	{
		{ "de-DE", "Deutsch" },
		{ "en-US", "English" },
		{ "es-MX", "Español" },
		{ "fr-FR", "Français" },
		{ "it-IT", "Italiano" },
		{ "ja-JP", "日本語" },
		{ "ko-KR", "한국어" },
		{ "pt-BR", "Português" },
		{ "ru-RU", "Русский" },
		{ "zh-CN", "简体中文" },
		{ "zh-TW", "繁體中文" }
	};

	public TextMeshProUGUI text;

	public void Setup(string lang, int index)
	{
		text.text = (_readableLanguageNames.TryGetValue(lang, out var val) ? val : lang);
		GetComponentInChildren<UI_Toggle>().index = index;
	}
}
