using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;

namespace DuloGames.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[AddComponentMenu("UI/UI Scene/Manager")]
public class UISceneManager : MonoBehaviour
{
	private static UISceneManager m_Instance;

	private List<UIScene> m_Scenes;

	public static UISceneManager instance => m_Instance;

	public UIScene[] scenes => m_Scenes.ToArray();

	protected void Awake()
	{
		if (m_Instance != null)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(this);
			}
			else
			{
				Object.DestroyImmediate(this);
			}
			Debug.LogWarning("Multiple UISceneManagers are not allowed, destroying.");
			return;
		}
		m_Instance = this;
		if (m_Scenes == null)
		{
			m_Scenes = new List<UIScene>();
		}
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	protected void OnDestroy()
	{
		m_Instance = null;
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
		UIScene.Transition transition = scene.transition;
		float transitionDuration = scene.transitionDuration;
		TweenEasing transitionEasing = scene.transitionEasing;
		UIScene[] activeScenes = GetActiveScenes();
		for (int i = 0; i < activeScenes.Length; i++)
		{
			activeScenes[i].TransitionOut(transition, transitionDuration, transitionEasing);
		}
		scene.TransitionIn(transition, transitionDuration, transitionEasing);
	}
}
