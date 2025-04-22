using System;
using System.Collections;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
[AddComponentMenu("UI/Loading Overlay", 58)]
public class UILoadingOverlay : MonoBehaviour
{
	[SerializeField]
	private UIProgressBar m_ProgressBar;

	[SerializeField]
	private CanvasGroup m_CanvasGroup;

	[Header("Transition")]
	[SerializeField]
	private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;

	[SerializeField]
	private float m_TransitionDuration = 0.4f;

	private bool m_Showing;

	private int m_LoadSceneId;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	protected UILoadingOverlay()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected void Awake()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Canvas[] array = global::UnityEngine.Object.FindObjectsOfType<Canvas>();
		Canvas currentCanvas = base.gameObject.GetComponent<Canvas>();
		Canvas[] array2 = array;
		foreach (Canvas canvas in array2)
		{
			if (!canvas.Equals(currentCanvas) && canvas.sortingOrder > currentCanvas.sortingOrder)
			{
				currentCanvas.sortingOrder = canvas.sortingOrder + 1;
			}
		}
		if (m_ProgressBar != null)
		{
			m_ProgressBar.fillAmount = 0f;
		}
		if (m_CanvasGroup != null)
		{
			m_CanvasGroup.alpha = 0f;
		}
	}

	protected void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	protected void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}

	public void LoadScene(string sceneName)
	{
		Scene scene = SceneManager.GetSceneByName(sceneName);
		if (scene.IsValid())
		{
			LoadScene(scene.buildIndex);
		}
	}

	public void LoadScene(int sceneIndex)
	{
		m_Showing = true;
		m_LoadSceneId = sceneIndex;
		if (m_ProgressBar != null)
		{
			m_ProgressBar.fillAmount = 0f;
		}
		if (m_CanvasGroup != null)
		{
			m_CanvasGroup.alpha = 0f;
		}
		StartAlphaTween(1f, m_TransitionDuration, ignoreTimeScale: true);
	}

	public void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
	{
		if (!(m_CanvasGroup == null))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = m_CanvasGroup.alpha;
			floatTween.targetFloat = targetAlpha;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetCanvasAlpha);
			floatTween2.AddOnFinishCallback(OnTweenFinished);
			floatTween2.ignoreTimeScale = ignoreTimeScale;
			floatTween2.easing = m_TransitionEasing;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	protected void SetCanvasAlpha(float alpha)
	{
		if (!(m_CanvasGroup == null))
		{
			m_CanvasGroup.alpha = alpha;
		}
	}

	protected void OnTweenFinished()
	{
		if (m_Showing)
		{
			m_Showing = false;
			StartCoroutine(AsynchronousLoad());
		}
		else
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator AsynchronousLoad()
	{
		yield return null;
		AsyncOperation ao = SceneManager.LoadSceneAsync(m_LoadSceneId);
		ao.allowSceneActivation = false;
		while (!ao.isDone)
		{
			float progress = Mathf.Clamp01(ao.progress / 0.9f);
			if (m_ProgressBar != null)
			{
				m_ProgressBar.fillAmount = progress;
			}
			if (ao.progress == 0.9f)
			{
				ao.allowSceneActivation = true;
			}
			yield return null;
		}
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex == m_LoadSceneId)
		{
			StartAlphaTween(0f, m_TransitionDuration, ignoreTimeScale: true);
		}
	}
}
