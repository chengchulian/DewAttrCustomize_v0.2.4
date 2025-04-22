using UnityEngine;

public class UI_Dev_LogWindowBottomPartScaler : MonoBehaviour
{
	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponentInParent<CanvasGroup>();
	}

	private void Update()
	{
		if (!(_cg.alpha < 0.5f))
		{
			RectTransform rt = (RectTransform)base.transform;
			Rect parentRect = ((RectTransform)rt.parent).GetScreenSpaceRect();
			rt.sizeDelta = rt.sizeDelta.WithY(((float)Screen.height - parentRect.height) / rt.lossyScale.y);
		}
	}
}
