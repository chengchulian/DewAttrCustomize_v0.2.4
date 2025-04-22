using TMPro;

public class UI_InGame_GameTimer : LogicBehaviour
{
	public TextMeshProUGUI timeText;

	public TextMeshProUGUI timeSubsecondsText;

	private char[] _subseconds = new char[2];

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(NetworkedManagerBase<GameManager>.softInstance == null))
		{
			float elapsedGameTime = NetworkedManagerBase<GameManager>.softInstance.elapsedGameTime;
			int totalSeconds = (int)elapsedGameTime;
			int minutes = totalSeconds / 60;
			int seconds = totalSeconds % 60;
			int subseconds = (int)((elapsedGameTime - (float)totalSeconds) * 100f);
			timeText.text = $"{minutes:00}:{seconds:00}";
			_subseconds[0] = (char)(subseconds / 10 + 48);
			_subseconds[1] = (char)(subseconds % 10 + 48);
			timeSubsecondsText.SetText(_subseconds);
		}
	}
}
