using System;
using UnityEngine;
using UnityEngine.UI;

namespace IngameDebugConsole;

public class DebugLogRecycledListView : MonoBehaviour
{
	[SerializeField]
	private RectTransform transformComponent;

	[SerializeField]
	private RectTransform viewportTransform;

	[SerializeField]
	private Color logItemNormalColor1;

	[SerializeField]
	private Color logItemNormalColor2;

	[SerializeField]
	private Color logItemSelectedColor;

	internal DebugLogManager manager;

	private ScrollRect scrollView;

	private float logItemHeight;

	private DynamicCircularBuffer<DebugLogEntry> entriesToShow;

	private DynamicCircularBuffer<DebugLogEntryTimestamp> timestampsOfEntriesToShow;

	private DebugLogEntry selectedLogEntry;

	private int indexOfSelectedLogEntry = int.MaxValue;

	private float heightOfSelectedLogEntry;

	private readonly DynamicCircularBuffer<DebugLogItem> visibleLogItems = new DynamicCircularBuffer<DebugLogItem>(32);

	private bool isCollapseOn;

	private int currentTopIndex = -1;

	private int currentBottomIndex = -1;

	private Predicate<DebugLogItem> shouldRemoveLogItemPredicate;

	private Action<DebugLogItem> poolLogItemAction;

	private float DeltaHeightOfSelectedLogEntry => heightOfSelectedLogEntry - logItemHeight;

	public float ItemHeight => logItemHeight;

	public float SelectedItemHeight => heightOfSelectedLogEntry;

	private void Awake()
	{
		scrollView = viewportTransform.GetComponentInParent<ScrollRect>();
		scrollView.onValueChanged.AddListener(delegate
		{
			if (manager.IsLogWindowVisible)
			{
				UpdateItemsInTheList(updateAllVisibleItemContents: false);
			}
		});
	}

	public void Initialize(DebugLogManager manager, DynamicCircularBuffer<DebugLogEntry> entriesToShow, DynamicCircularBuffer<DebugLogEntryTimestamp> timestampsOfEntriesToShow, float logItemHeight)
	{
		this.manager = manager;
		this.entriesToShow = entriesToShow;
		this.timestampsOfEntriesToShow = timestampsOfEntriesToShow;
		this.logItemHeight = logItemHeight;
		shouldRemoveLogItemPredicate = ShouldRemoveLogItem;
		poolLogItemAction = manager.PoolLogItem;
	}

	public void SetCollapseMode(bool collapse)
	{
		isCollapseOn = collapse;
	}

	public void OnLogItemClicked(DebugLogItem item)
	{
		OnLogItemClickedInternal(item.Index, item);
	}

	public void SelectAndFocusOnLogItemAtIndex(int itemIndex)
	{
		if (indexOfSelectedLogEntry != itemIndex)
		{
			OnLogItemClickedInternal(itemIndex);
		}
		float viewportHeight = viewportTransform.rect.height;
		float transformComponentCenterYAtTop = viewportHeight * 0.5f;
		float transformComponentCenterYAtBottom = transformComponent.sizeDelta.y - viewportHeight * 0.5f;
		float transformComponentTargetCenterY = (float)itemIndex * logItemHeight + viewportHeight * 0.5f;
		if (transformComponentCenterYAtTop == transformComponentCenterYAtBottom)
		{
			scrollView.verticalNormalizedPosition = 0.5f;
		}
		else
		{
			scrollView.verticalNormalizedPosition = Mathf.Clamp01(Mathf.InverseLerp(transformComponentCenterYAtBottom, transformComponentCenterYAtTop, transformComponentTargetCenterY));
		}
		manager.SnapToBottom = false;
	}

	private void OnLogItemClickedInternal(int itemIndex, DebugLogItem referenceItem = null)
	{
		int num = indexOfSelectedLogEntry;
		DeselectSelectedLogItem();
		if (num != itemIndex)
		{
			selectedLogEntry = entriesToShow[itemIndex];
			indexOfSelectedLogEntry = itemIndex;
			CalculateSelectedLogEntryHeight(referenceItem);
			manager.SnapToBottom = false;
		}
		CalculateContentHeight();
		UpdateItemsInTheList(updateAllVisibleItemContents: true);
		manager.ValidateScrollPosition();
	}

	public void DeselectSelectedLogItem()
	{
		selectedLogEntry = null;
		indexOfSelectedLogEntry = int.MaxValue;
		heightOfSelectedLogEntry = 0f;
	}

	public void OnLogEntriesUpdated(bool updateAllVisibleItemContents)
	{
		CalculateContentHeight();
		UpdateItemsInTheList(updateAllVisibleItemContents);
	}

	public void OnCollapsedLogEntryAtIndexUpdated(int index)
	{
		if (index >= currentTopIndex && index <= currentBottomIndex)
		{
			DebugLogItem logItem = GetLogItemAtIndex(index);
			logItem.ShowCount();
			if (timestampsOfEntriesToShow != null)
			{
				logItem.UpdateTimestamp(timestampsOfEntriesToShow[index]);
			}
		}
	}

	public void RefreshCollapsedLogEntryCounts()
	{
		for (int i = 0; i < visibleLogItems.Count; i++)
		{
			visibleLogItems[i].ShowCount();
		}
	}

	public void OnLogEntriesRemoved(int removedLogCount)
	{
		if (selectedLogEntry != null)
		{
			if (isCollapseOn ? (selectedLogEntry.count == 0) : (indexOfSelectedLogEntry < removedLogCount))
			{
				DeselectSelectedLogItem();
			}
			else
			{
				indexOfSelectedLogEntry = (isCollapseOn ? FindIndexOfLogEntryInReverseDirection(selectedLogEntry, indexOfSelectedLogEntry) : (indexOfSelectedLogEntry - removedLogCount));
			}
		}
		if (!manager.IsLogWindowVisible && manager.SnapToBottom)
		{
			visibleLogItems.TrimStart(visibleLogItems.Count, poolLogItemAction);
		}
		else if (!isCollapseOn)
		{
			visibleLogItems.TrimStart(Mathf.Clamp(removedLogCount - currentTopIndex, 0, visibleLogItems.Count), poolLogItemAction);
		}
		else
		{
			visibleLogItems.RemoveAll(shouldRemoveLogItemPredicate);
			if (visibleLogItems.Count > 0)
			{
				removedLogCount = currentTopIndex - FindIndexOfLogEntryInReverseDirection(visibleLogItems[0].Entry, visibleLogItems[0].Index);
			}
		}
		if (visibleLogItems.Count == 0)
		{
			currentTopIndex = -1;
			if (!manager.SnapToBottom)
			{
				transformComponent.anchoredPosition = Vector2.zero;
			}
			return;
		}
		currentTopIndex = Mathf.Max(0, currentTopIndex - removedLogCount);
		currentBottomIndex = currentTopIndex + visibleLogItems.Count - 1;
		float firstVisibleLogItemInitialYPos = visibleLogItems[0].Transform.anchoredPosition.y;
		for (int i = 0; i < visibleLogItems.Count; i++)
		{
			DebugLogItem logItem = visibleLogItems[i];
			logItem.Index = currentTopIndex + i;
			if (manager.IsLogWindowVisible)
			{
				RepositionLogItem(logItem);
				ColorLogItem(logItem);
				if (isCollapseOn)
				{
					logItem.ShowCount();
				}
			}
		}
		if (!manager.SnapToBottom)
		{
			transformComponent.anchoredPosition = new Vector2(0f, Mathf.Max(0f, transformComponent.anchoredPosition.y - (visibleLogItems[0].Transform.anchoredPosition.y - firstVisibleLogItemInitialYPos)));
		}
	}

	private bool ShouldRemoveLogItem(DebugLogItem logItem)
	{
		if (logItem.Entry.count == 0)
		{
			poolLogItemAction(logItem);
			return true;
		}
		return false;
	}

	private int FindIndexOfLogEntryInReverseDirection(DebugLogEntry logEntry, int startIndex)
	{
		for (int i = Mathf.Min(startIndex, entriesToShow.Count - 1); i >= 0; i--)
		{
			if (entriesToShow[i] == logEntry)
			{
				return i;
			}
		}
		return -1;
	}

	public void OnViewportWidthChanged()
	{
		if (indexOfSelectedLogEntry < entriesToShow.Count)
		{
			CalculateSelectedLogEntryHeight();
			CalculateContentHeight();
			UpdateItemsInTheList(updateAllVisibleItemContents: true);
			manager.ValidateScrollPosition();
		}
	}

	public void OnViewportHeightChanged()
	{
		UpdateItemsInTheList(updateAllVisibleItemContents: false);
	}

	private void CalculateContentHeight()
	{
		float newHeight = Mathf.Max(1f, (float)entriesToShow.Count * logItemHeight);
		if (selectedLogEntry != null)
		{
			newHeight += DeltaHeightOfSelectedLogEntry;
		}
		transformComponent.sizeDelta = new Vector2(0f, newHeight);
	}

	private void CalculateSelectedLogEntryHeight(DebugLogItem referenceItem = null)
	{
		if (!referenceItem)
		{
			if (visibleLogItems.Count == 0)
			{
				UpdateItemsInTheList(updateAllVisibleItemContents: false);
				if (visibleLogItems.Count == 0)
				{
					return;
				}
			}
			referenceItem = visibleLogItems[0];
		}
		heightOfSelectedLogEntry = referenceItem.CalculateExpandedHeight(selectedLogEntry, (timestampsOfEntriesToShow != null) ? new DebugLogEntryTimestamp?(timestampsOfEntriesToShow[indexOfSelectedLogEntry]) : ((DebugLogEntryTimestamp?)null));
	}

	private void UpdateItemsInTheList(bool updateAllVisibleItemContents)
	{
		if (entriesToShow.Count > 0)
		{
			float contentPosTop = transformComponent.anchoredPosition.y - 1f;
			float contentPosBottom = contentPosTop + viewportTransform.rect.height + 2f;
			float positionOfSelectedLogEntry = (float)indexOfSelectedLogEntry * logItemHeight;
			if (positionOfSelectedLogEntry <= contentPosBottom)
			{
				if (positionOfSelectedLogEntry <= contentPosTop)
				{
					contentPosTop = Mathf.Max(contentPosTop - DeltaHeightOfSelectedLogEntry, positionOfSelectedLogEntry - 1f);
					contentPosBottom = Mathf.Max(contentPosBottom - DeltaHeightOfSelectedLogEntry, contentPosTop + 2f);
				}
				else
				{
					contentPosBottom = Mathf.Max(contentPosBottom - DeltaHeightOfSelectedLogEntry, positionOfSelectedLogEntry + 1f);
				}
			}
			int newBottomIndex = Mathf.Min((int)(contentPosBottom / logItemHeight), entriesToShow.Count - 1);
			int newTopIndex = Mathf.Clamp((int)(contentPosTop / logItemHeight), 0, newBottomIndex);
			if (currentTopIndex == -1)
			{
				updateAllVisibleItemContents = true;
				int i = 0;
				for (int count = newBottomIndex - newTopIndex + 1; i < count; i++)
				{
					visibleLogItems.Add(manager.PopLogItem());
				}
			}
			else if (newBottomIndex < currentTopIndex || newTopIndex > currentBottomIndex)
			{
				updateAllVisibleItemContents = true;
				visibleLogItems.TrimStart(visibleLogItems.Count, poolLogItemAction);
				int j = 0;
				for (int count2 = newBottomIndex - newTopIndex + 1; j < count2; j++)
				{
					visibleLogItems.Add(manager.PopLogItem());
				}
			}
			else
			{
				if (newTopIndex > currentTopIndex)
				{
					visibleLogItems.TrimStart(newTopIndex - currentTopIndex, poolLogItemAction);
				}
				if (newBottomIndex < currentBottomIndex)
				{
					visibleLogItems.TrimEnd(currentBottomIndex - newBottomIndex, poolLogItemAction);
				}
				if (newTopIndex < currentTopIndex)
				{
					int k = 0;
					for (int count3 = currentTopIndex - newTopIndex; k < count3; k++)
					{
						visibleLogItems.AddFirst(manager.PopLogItem());
					}
					if (!updateAllVisibleItemContents)
					{
						UpdateLogItemContentsBetweenIndices(newTopIndex, currentTopIndex - 1, newTopIndex);
					}
				}
				if (newBottomIndex > currentBottomIndex)
				{
					int l = 0;
					for (int count4 = newBottomIndex - currentBottomIndex; l < count4; l++)
					{
						visibleLogItems.Add(manager.PopLogItem());
					}
					if (!updateAllVisibleItemContents)
					{
						UpdateLogItemContentsBetweenIndices(currentBottomIndex + 1, newBottomIndex, newTopIndex);
					}
				}
			}
			currentTopIndex = newTopIndex;
			currentBottomIndex = newBottomIndex;
			if (updateAllVisibleItemContents)
			{
				UpdateLogItemContentsBetweenIndices(currentTopIndex, currentBottomIndex, newTopIndex);
			}
		}
		else if (currentTopIndex != -1)
		{
			visibleLogItems.TrimStart(visibleLogItems.Count, poolLogItemAction);
			currentTopIndex = -1;
		}
	}

	private DebugLogItem GetLogItemAtIndex(int index)
	{
		return visibleLogItems[index - currentTopIndex];
	}

	private void UpdateLogItemContentsBetweenIndices(int topIndex, int bottomIndex, int logItemOffset)
	{
		for (int i = topIndex; i <= bottomIndex; i++)
		{
			DebugLogItem logItem = visibleLogItems[i - logItemOffset];
			logItem.SetContent(entriesToShow[i], (timestampsOfEntriesToShow != null) ? new DebugLogEntryTimestamp?(timestampsOfEntriesToShow[i]) : ((DebugLogEntryTimestamp?)null), i, i == indexOfSelectedLogEntry);
			RepositionLogItem(logItem);
			ColorLogItem(logItem);
			if (isCollapseOn)
			{
				logItem.ShowCount();
			}
			else
			{
				logItem.HideCount();
			}
		}
	}

	private void RepositionLogItem(DebugLogItem logItem)
	{
		int index = logItem.Index;
		Vector2 anchoredPosition = new Vector2(1f, (float)(-index) * logItemHeight);
		if (index > indexOfSelectedLogEntry)
		{
			anchoredPosition.y -= DeltaHeightOfSelectedLogEntry;
		}
		logItem.Transform.anchoredPosition = anchoredPosition;
	}

	private void ColorLogItem(DebugLogItem logItem)
	{
		int index = logItem.Index;
		if (index == indexOfSelectedLogEntry)
		{
			logItem.Image.color = logItemSelectedColor;
		}
		else if (index % 2 == 0)
		{
			logItem.Image.color = logItemNormalColor1;
		}
		else
		{
			logItem.Image.color = logItemNormalColor2;
		}
	}
}
