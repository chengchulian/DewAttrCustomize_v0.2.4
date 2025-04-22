using System;
using System.Globalization;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Common_ChatBox_Item : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public TextMeshProUGUI text;

	public float sustainTime = 10f;

	public float decayTime = 2f;

	private CanvasGroup _cg;

	private float _startTime;

	private bool _enableFade;

	private UI_Common_ChatBox _parent;

	private ChatManager.Message _msg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
	}

	private void Update()
	{
		if (_enableFade)
		{
			float elapsedTime = Time.unscaledTime - _startTime;
			if (elapsedTime < sustainTime || _parent._cg.interactable)
			{
				_cg.alpha = 1f;
			}
			else
			{
				_cg.alpha = 1f - (elapsedTime - sustainTime) / decayTime;
			}
			_cg.blocksRaycasts = !string.IsNullOrEmpty(_msg.itemType) && _cg.alpha > 0.5f;
		}
	}

	public void Setup(ChatManager.Message msg, bool enableFade)
	{
		_msg = msg;
		_parent = GetComponentInParent<UI_Common_ChatBox>();
		_enableFade = enableFade;
		_startTime = Time.unscaledTime;
		switch (msg.type)
		{
		case ChatManager.MessageType.Raw:
			text.text = msg.content;
			break;
		case ChatManager.MessageType.Chat:
		{
			DewPlayer player = NetworkClient.spawned[uint.Parse(msg.args[0], CultureInfo.InvariantCulture)].GetComponent<DewPlayer>();
			text.text = ChatManager.GetFormattedChatContent(ChatManager.GetDescribedPlayerName(player), msg.content);
			break;
		}
		case ChatManager.MessageType.WorldEvent:
		{
			string content3 = DewLocalization.GetUIValue(msg.content);
			if (msg.args != null)
			{
				string format2 = content3;
				object[] args = msg.args;
				content3 = string.Format(format2, args);
			}
			text.text = "<color=#e3bb71>" + content3 + "</color>";
			break;
		}
		case ChatManager.MessageType.Notice:
		{
			string content2 = DewLocalization.GetUIValue(msg.content);
			if (msg.args != null)
			{
				string format = content2;
				object[] args = msg.args;
				content2 = string.Format(format, args);
			}
			text.text = "<color=#94aab5><i>" + content2 + "</i></color>";
			break;
		}
		case ChatManager.MessageType.UnlockedAchievement:
		{
			string template = DewLocalization.GetUIValue("Chat_AchievementUnlocked");
			string playerName = msg.args[0];
			string colorHex = Dew.GetAchievementColorHex(Dew.achievementsByName[msg.args[1]]);
			text.text = "<color=#94aab5><i>" + string.Format(template, "<b>" + playerName + "</b>", "<b><color=" + colorHex + ">" + DewLocalization.GetAchievementName(msg.args[1]) + "</color></b>") + "</i></color>";
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (string.IsNullOrEmpty(_msg.itemType))
		{
			return;
		}
		if (_msg.itemType.StartsWith("St_"))
		{
			tooltip.ShowSkillTooltipRaw((Func<Vector2>)delegate
			{
				Rect screenSpaceRect = ((RectTransform)base.transform).GetScreenSpaceRect();
				return screenSpaceRect.max.WithX(screenSpaceRect.center.x);
			}, DewResources.GetByShortTypeName<SkillTrigger>(_msg.itemType), _msg.itemLevel);
		}
		else if (_msg.itemType.StartsWith("Gem_"))
		{
			tooltip.ShowGemTooltipRaw((Func<Vector2>)delegate
			{
				Rect screenSpaceRect2 = ((RectTransform)base.transform).GetScreenSpaceRect();
				return screenSpaceRect2.max.WithX(screenSpaceRect2.center.x);
			}, DewResources.GetByShortTypeName<Gem>(_msg.itemType), _msg.itemLevel);
		}
		else if (_msg.itemType.StartsWith("Treasure_"))
		{
			tooltip.ShowTreasureTooltip((Func<Vector2>)delegate
			{
				Rect screenSpaceRect3 = ((RectTransform)base.transform).GetScreenSpaceRect();
				return screenSpaceRect3.max.WithX(screenSpaceRect3.center.x);
			}, DewResources.GetByShortTypeName<Treasure>(_msg.itemType), _msg.itemPrice.gold, _msg.itemCustomData);
		}
	}
}
