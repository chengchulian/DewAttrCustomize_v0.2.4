using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole;

public class DebugLogPopup : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private RectTransform popupTransform;

	private Vector2 halfSize;

	private Image backgroundImage;

	private CanvasGroup canvasGroup;

	[SerializeField]
	private DebugLogManager debugManager;

	[SerializeField]
	private Text newInfoCountText;

	[SerializeField]
	private Text newWarningCountText;

	[SerializeField]
	private Text newErrorCountText;

	[SerializeField]
	private Color alertColorInfo;

	[SerializeField]
	private Color alertColorWarning;

	[SerializeField]
	private Color alertColorError;

	private int newInfoCount;

	private int newWarningCount;

	private int newErrorCount;

	private Color normalColor;

	private bool isPopupBeingDragged;

	private Vector2 normalizedPosition;

	private IEnumerator moveToPosCoroutine;

	private void Awake()
	{
		popupTransform = (RectTransform)base.transform;
		backgroundImage = GetComponent<Image>();
		canvasGroup = GetComponent<CanvasGroup>();
		normalColor = backgroundImage.color;
		halfSize = popupTransform.sizeDelta * 0.5f;
		Vector2 pos = popupTransform.anchoredPosition;
		if (pos.x != 0f || pos.y != 0f)
		{
			normalizedPosition = pos.normalized;
		}
		else
		{
			normalizedPosition = new Vector2(0.5f, 0f);
		}
	}

	public void NewLogsArrived(int newInfo, int newWarning, int newError)
	{
		if (newInfo > 0)
		{
			newInfoCount += newInfo;
			newInfoCountText.text = newInfoCount.ToString();
		}
		if (newWarning > 0)
		{
			newWarningCount += newWarning;
			newWarningCountText.text = newWarningCount.ToString();
		}
		if (newError > 0)
		{
			newErrorCount += newError;
			newErrorCountText.text = newErrorCount.ToString();
		}
		if (newErrorCount > 0)
		{
			backgroundImage.color = alertColorError;
		}
		else if (newWarningCount > 0)
		{
			backgroundImage.color = alertColorWarning;
		}
		else
		{
			backgroundImage.color = alertColorInfo;
		}
	}

	private void ResetValues()
	{
		newInfoCount = 0;
		newWarningCount = 0;
		newErrorCount = 0;
		newInfoCountText.text = "0";
		newWarningCountText.text = "0";
		newErrorCountText.text = "0";
		backgroundImage.color = normalColor;
	}

	private IEnumerator MoveToPosAnimation(Vector2 targetPos)
	{
		float modifier = 0f;
		Vector2 initialPos = popupTransform.anchoredPosition;
		while (modifier < 1f)
		{
			modifier += 4f * Time.unscaledDeltaTime;
			popupTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, modifier);
			yield return null;
		}
	}

	public void OnPointerClick(PointerEventData data)
	{
		if (!isPopupBeingDragged)
		{
			debugManager.ShowLogWindow();
		}
	}

	public void Show()
	{
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1f;
		ResetValues();
		UpdatePosition(immediately: true);
	}

	public void Hide()
	{
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0f;
		isPopupBeingDragged = false;
	}

	public void OnBeginDrag(PointerEventData data)
	{
		isPopupBeingDragged = true;
		if (moveToPosCoroutine != null)
		{
			StopCoroutine(moveToPosCoroutine);
			moveToPosCoroutine = null;
		}
	}

	public void OnDrag(PointerEventData data)
	{
		popupTransform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData data)
	{
		isPopupBeingDragged = false;
		UpdatePosition(immediately: false);
	}

	public void UpdatePosition(bool immediately)
	{
		Vector2 canvasRawSize = debugManager.canvasTR.rect.size;
		float canvasWidth = canvasRawSize.x;
		float canvasHeight = canvasRawSize.y;
		float canvasBottomLeftX = 0f;
		float canvasBottomLeftY = 0f;
		_ = debugManager.popupAvoidsScreenCutout;
		Vector2 pos = canvasRawSize * 0.5f + (immediately ? new Vector2(normalizedPosition.x * canvasWidth, normalizedPosition.y * canvasHeight) : (popupTransform.anchoredPosition - new Vector2(canvasBottomLeftX, canvasBottomLeftY)));
		float distToLeft = pos.x;
		float distToRight = canvasWidth - distToLeft;
		float distToBottom = pos.y;
		float distToTop = canvasHeight - distToBottom;
		float num = Mathf.Min(distToLeft, distToRight);
		float vertDistance = Mathf.Min(distToBottom, distToTop);
		if (num < vertDistance)
		{
			pos = ((!(distToLeft < distToRight)) ? new Vector2(canvasWidth - halfSize.x, pos.y) : new Vector2(halfSize.x, pos.y));
			pos.y = Mathf.Clamp(pos.y, halfSize.y, canvasHeight - halfSize.y);
		}
		else
		{
			pos = ((!(distToBottom < distToTop)) ? new Vector2(pos.x, canvasHeight - halfSize.y) : new Vector2(pos.x, halfSize.y));
			pos.x = Mathf.Clamp(pos.x, halfSize.x, canvasWidth - halfSize.x);
		}
		pos -= canvasRawSize * 0.5f;
		normalizedPosition.Set(pos.x / canvasWidth, pos.y / canvasHeight);
		pos += new Vector2(canvasBottomLeftX, canvasBottomLeftY);
		if (moveToPosCoroutine != null)
		{
			StopCoroutine(moveToPosCoroutine);
			moveToPosCoroutine = null;
		}
		if (immediately)
		{
			popupTransform.anchoredPosition = pos;
			return;
		}
		moveToPosCoroutine = MoveToPosAnimation(pos);
		StartCoroutine(moveToPosCoroutine);
	}
}
