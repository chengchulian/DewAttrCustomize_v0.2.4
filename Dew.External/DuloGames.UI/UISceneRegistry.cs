using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI;

public class UISceneRegistry
{
	private static UISceneRegistry m_Instance;

	private List<UIScene> m_Scenes;

	private UIScene m_LastScene;

	public static UISceneRegistry instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new UISceneRegistry();
			}
			return m_Instance;
		}
	}

	public UIScene[] scenes => m_Scenes.ToArray();

	public UIScene lastScene => m_LastScene;

	protected UISceneRegistry()
	{
		m_Scenes = new List<UIScene>();
	}

	public void RegisterScene(UIScene scene)
	{
		if (m_Scenes == null)
		{
			m_Scenes = new List<UIScene>();
		}
		if (m_Scenes.Contains(scene))
		{
			Debug.LogWarning("Trying to register a UIScene multiple times.");
		}
		else
		{
			m_Scenes.Add(scene);
		}
	}

	public void UnregisterScene(UIScene scene)
	{
		if (m_Scenes != null)
		{
			m_Scenes.Remove(scene);
		}
	}

	public UIScene[] GetActiveScenes()
	{
		return m_Scenes.FindAll((UIScene x) => x.isActivated).ToArray();
	}

	public UIScene GetScene(int id)
	{
		if (m_Scenes == null || m_Scenes.Count == 0)
		{
			return null;
		}
		return m_Scenes.Find((UIScene x) => x.id == id);
	}

	public int GetAvailableSceneId()
	{
		if (m_Scenes.Count == 0)
		{
			return 0;
		}
		int id = 0;
		foreach (UIScene scene in m_Scenes)
		{
			if (scene.id > id)
			{
				id = scene.id;
			}
		}
		return id + 1;
	}

	public void TransitionToScene(UIScene scene)
	{
		UIScene[] activeScenes = GetActiveScenes();
		foreach (UIScene activeScene in activeScenes)
		{
			activeScene.TransitionOut();
			m_LastScene = activeScene;
		}
		scene.TransitionIn();
	}
}
