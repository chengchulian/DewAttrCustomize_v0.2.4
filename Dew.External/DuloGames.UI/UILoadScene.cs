using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("Miscellaneous/Load Scene")]
public class UILoadScene : MonoBehaviour
{
	private enum InputKey
	{
		None,
		Submit,
		Cancel,
		Jump
	}

	[SerializeField]
	private string m_Scene;

	[SerializeField]
	private bool m_UseLoadingOverlay;

	[SerializeField]
	private InputKey m_InputKey;

	[SerializeField]
	private Button m_HookToButton;

	protected void OnEnable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.AddListener(LoadScene);
		}
	}

	protected void OnDisable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.RemoveListener(LoadScene);
		}
	}

	public void LoadScene()
	{
		if (string.IsNullOrEmpty(m_Scene))
		{
			return;
		}
		int id;
		bool isNumeric = int.TryParse(m_Scene, out id);
		if (m_UseLoadingOverlay && UILoadingOverlayManager.Instance != null)
		{
			UILoadingOverlay loadingOverlay = UILoadingOverlayManager.Instance.Create();
			if (loadingOverlay != null)
			{
				if (isNumeric)
				{
					loadingOverlay.LoadScene(id);
				}
				else
				{
					loadingOverlay.LoadScene(m_Scene);
				}
			}
			else
			{
				Debug.LogWarning("Failed to instantiate the loading overlay prefab, make sure it's assigned on the manager.");
			}
		}
		else if (isNumeric)
		{
			SceneManager.LoadScene(id);
		}
		else
		{
			SceneManager.LoadScene(m_Scene);
		}
	}

	protected void Update()
	{
		if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy && m_InputKey != 0 && (!(EventSystem.current.currentSelectedGameObject != null) || !(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() != null)) && (m_InputKey != InputKey.Cancel || !(UIWindowManager.Instance != null) || !(UIWindowManager.Instance.escapeInputName == "Cancel") || !UIWindowManager.Instance.escapedUsed) && (m_InputKey != InputKey.Cancel || !(UIModalBoxManager.Instance != null) || UIModalBoxManager.Instance.activeBoxes.Length == 0))
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
				LoadScene();
			}
		}
	}
}
