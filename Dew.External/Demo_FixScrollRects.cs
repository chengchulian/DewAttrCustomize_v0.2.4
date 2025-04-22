using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Demo_FixScrollRects : MonoBehaviour
{
	protected void OnEnable()
	{
		SceneManager.sceneLoaded += OnLoaded;
	}

	protected void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLoaded;
	}

	private void OnLoaded(Scene scene, LoadSceneMode mode)
	{
		ScrollRect[] array = Object.FindObjectsOfType<ScrollRect>();
		for (int i = 0; i < array.Length; i++)
		{
			LayoutRebuilder.MarkLayoutForRebuild(array[i].viewport);
		}
	}
}
