using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace IngameDebugConsole;

[DefaultExecutionOrder(1000)]
public class EventSystemHandler : MonoBehaviour
{
	[SerializeField]
	private GameObject embeddedEventSystem;

	private void OnEnable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		ActivateEventSystemIfNeeded();
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
		DeactivateEventSystem();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		DeactivateEventSystem();
		ActivateEventSystemIfNeeded();
	}

	private void OnSceneUnloaded(Scene current)
	{
		DeactivateEventSystem();
	}

	private void ActivateEventSystemIfNeeded()
	{
		if ((bool)embeddedEventSystem && !EventSystem.current)
		{
			embeddedEventSystem.SetActive(value: true);
		}
	}

	private void DeactivateEventSystem()
	{
		if ((bool)embeddedEventSystem)
		{
			embeddedEventSystem.SetActive(value: false);
		}
	}
}
