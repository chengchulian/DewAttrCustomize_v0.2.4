using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Lobby_CharacterRotator : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	private bool _isRotating;

	private PlayLobby_Character[] _characters;

	private Vector2 _prevCursorPos;

	private float _lastJoystickUnscaledTime;

	private void Start()
	{
		_characters = Object.FindObjectsOfType<PlayLobby_Character>(includeInactive: true);
		_characters.Sort((PlayLobby_Character a, PlayLobby_Character b) => a.playerIndex.CompareTo(b.playerIndex));
	}

	private void Update()
	{
		if (!LobbyUIManager.instance.IsState("Character"))
		{
			if (_isRotating)
			{
				_isRotating = false;
				PlayLobby_Character[] characters = _characters;
				for (int i = 0; i < characters.Length; i++)
				{
					characters[i].characterRotation = null;
				}
			}
			return;
		}
		if (DewInput.currentMode == InputMode.Gamepad && DewInput.GetRightJoystick().sqrMagnitude > 0.001f)
		{
			OnPointerDown(null);
		}
		if (!_isRotating)
		{
			return;
		}
		float delta = ((DewInput.currentMode == InputMode.KeyboardAndMouse) ? ((Input.mousePosition.ToXY() - _prevCursorPos).x / (float)Screen.height * -200f) : (DewInput.GetRightJoystick().x * Time.deltaTime * -400f));
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			if (Mathf.Abs(delta) > 0.001f)
			{
				_lastJoystickUnscaledTime = Time.unscaledTime;
			}
			else if (Time.unscaledTime - _lastJoystickUnscaledTime > 0.75f)
			{
				OnPointerUp(null);
				return;
			}
		}
		_prevCursorPos = Input.mousePosition;
		int index = DewPlayer.humanPlayers.IndexOf(DewPlayer.local);
		for (int j = 0; j < _characters.Length; j++)
		{
			PlayLobby_Character c = _characters[j];
			if (c.characterRotation.HasValue && index == j)
			{
				c.characterRotation = c.characterRotation.Value + delta;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData == null || eventData.button == PointerEventData.InputButton.Left)
		{
			_isRotating = true;
			_prevCursorPos = Input.mousePosition;
			int index = DewPlayer.humanPlayers.IndexOf(DewPlayer.local);
			for (int i = 0; i < _characters.Length; i++)
			{
				_characters[i].characterRotation = ((i == index) ? (_characters[index].characterRotation = _characters[index].model.transform.localRotation.eulerAngles.y) : ((float?)null));
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData == null || eventData.button == PointerEventData.InputButton.Left)
		{
			_isRotating = false;
			PlayLobby_Character[] characters = _characters;
			for (int i = 0; i < characters.Length; i++)
			{
				characters[i].characterRotation = null;
			}
		}
	}
}
