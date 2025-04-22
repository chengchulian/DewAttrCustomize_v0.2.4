using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/UI Scene/Open")]
public class UISceneOpen : MonoBehaviour
{
	private enum ActionType
	{
		SpecificID,
		LastScene
	}

	private enum InputKey
	{
		None,
		Submit,
		Cancel,
		Jump
	}

	[SerializeField]
	private ActionType m_ActionType;

	[SerializeField]
	private int m_SceneId;

	[SerializeField]
	private InputKey m_InputKey;

	[SerializeField]
	private Button m_HookToButton;

	protected void OnEnable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.AddListener(Open);
		}
	}

	protected void OnDisable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.RemoveListener(Open);
		}
	}

	public void Open()
	{
		UIScene scene = null;
		switch (m_ActionType)
		{
		case ActionType.SpecificID:
			scene = UISceneRegistry.instance.GetScene(m_SceneId);
			break;
		case ActionType.LastScene:
			scene = UISceneRegistry.instance.lastScene;
			break;
		}
		if (scene != null)
		{
			scene.TransitionTo();
		}
	}

	protected void Update()
	{
		if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy && m_InputKey != 0 && (m_InputKey != InputKey.Cancel || !(UIWindowManager.Instance != null) || !(UIWindowManager.Instance.escapeInputName == "Cancel") || !UIWindowManager.Instance.escapedUsed) && (m_InputKey != InputKey.Cancel || !(UIModalBoxManager.Instance != null) || UIModalBoxManager.Instance.activeBoxes.Length == 0))
		{
			string buttonName = string.Empty;
			switch (m_InputKey)
			{
			case InputKey.Submit:
				buttonName = "Submit";
				break;
			case InputKey.Cancel:
				buttonName = "Cancel";
				break;
			case InputKey.Jump:
				buttonName = "Jump";
				break;
			}
			if (!string.IsNullOrEmpty(buttonName) && Input.GetButtonDown(buttonName))
			{
				Open();
			}
		}
	}
}
