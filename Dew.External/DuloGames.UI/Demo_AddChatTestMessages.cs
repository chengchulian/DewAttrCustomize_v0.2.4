using UnityEngine;

namespace DuloGames.UI;

public class Demo_AddChatTestMessages : MonoBehaviour
{
	[SerializeField]
	private Demo_Chat m_Chat;

	protected void Start()
	{
		AddMessages();
	}

	[ContextMenu("Add Messages")]
	public void AddMessages()
	{
		if (m_Chat != null)
		{
			m_Chat.ReceiveChatMessage(1, "<b><color=#ce4627ff>Jeff</color></b> <color=#59524bff>said:</color> Eget vulputate justo, at molestie urna. Pellentesque eu nunc...");
			m_Chat.ReceiveChatMessage(1, "<b><color=#b59e8aff>Subzero</color></b> <color=#59524bff>said:</color> Phasellus eget vulputate justo, at molestie urna. Pellentesque zesu nunc gravida felis finibus maximus.");
			m_Chat.ReceiveChatMessage(1, "<b><color=#318fd0ff>Jossy</color></b> <color=#59524bff>said:</color> Shasellus eget lputate justo, at molestie urna. Pellentesque eu nunc gravida felis finibus amaximus  justo, at molestie urna.");
			m_Chat.ReceiveChatMessage(1, "<b><color=#ce4627ff>Jeff</color></b> <color=#59524bff>said:</color> Eget vulputate justo, at molestie urna. Pellentesque eu nunc...");
			m_Chat.ReceiveChatMessage(1, "<b><color=#b37b4aff>Gandalf</color></b> <color=#59524bff>said:</color> Phasellus eget vulputate justo, at molestie urna. Pellentesque eu nunc gravida felis finibus maximus.");
			m_Chat.ReceiveChatMessage(1, "<b><color=#318fd0ff>Jossy</color></b> <color=#59524bff>said:</color> Shasellus eget lputate justo, at molestie urna. Pellentesque eu nunc gravida felis finibus amaximus  justo, at molestie urna.");
			m_Chat.ReceiveChatMessage(1, "<b><color=#ce4627ff>Jeff</color></b> <color=#59524bff>said:</color> Eget vulputate justo, at molestie urna. Pellentesque eu nunc...");
			m_Chat.ReceiveChatMessage(1, "<b><color=#b59e8aff>Subzero</color></b> <color=#59524bff>said:</color> Phasellus eget vulputate justo, at molestie urna. Pellentesque zesu nunc gravida felis finibus maximus.");
		}
	}
}
