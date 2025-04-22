using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole;

public class DebugLogManager : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, console window will persist between scenes (i.e. not be destroyed when scene changes)")]
	private bool singleton = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("Minimum height of the console window")]
	private float minimumHeight = 200f;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, console window can be resized horizontally, as well")]
	private bool enableHorizontalResizing;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, console window's resize button will be located at bottom-right corner. Otherwise, it will be located at bottom-left corner")]
	private bool resizeFromRight = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("Minimum width of the console window")]
	private float minimumWidth = 240f;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If disabled, no popup will be shown when the console window is hidden")]
	private bool enablePopup = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, console will be initialized as a popup")]
	private bool startInPopupMode = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, console window will initially be invisible")]
	private bool startMinimized;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, pressing the Toggle Key will show/hide (i.e. toggle) the console window at runtime")]
	private bool toggleWithKey;

	[SerializeField]
	[HideInInspector]
	private KeyCode toggleKey = KeyCode.BackQuote;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, the console window will have a searchbar")]
	private bool enableSearchbar = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("Width of the canvas determines whether the searchbar will be located inside the menu bar or underneath the menu bar. This way, the menu bar doesn't get too crowded on narrow screens. This value determines the minimum width of the canvas for the searchbar to appear inside the menu bar")]
	private float topSearchbarMinWidth = 360f;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, the console window will continue receiving logs in the background even if its GameObject is inactive. But the console window's GameObject needs to be activated at least once because its Awake function must be triggered for this to work")]
	private bool receiveLogsWhileInactive;

	[SerializeField]
	[HideInInspector]
	private bool receiveInfoLogs = true;

	[SerializeField]
	[HideInInspector]
	private bool receiveWarningLogs = true;

	[SerializeField]
	[HideInInspector]
	private bool receiveErrorLogs = true;

	[SerializeField]
	[HideInInspector]
	private bool receiveExceptionLogs = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, the arrival times of logs will be recorded and displayed when a log is expanded")]
	private bool captureLogTimestamps;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, timestamps will be displayed for logs even if they aren't expanded")]
	internal bool alwaysDisplayTimestamps;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If the number of logs reach this limit, the oldest log(s) will be deleted to limit the RAM usage. It's recommended to set this value as low as possible")]
	private int maxLogCount = int.MaxValue;

	[SerializeField]
	[HideInInspector]
	[Tooltip("How many log(s) to delete when the threshold is reached (all logs are iterated during this operation so it should neither be too low nor too high)")]
	private int logsToRemoveAfterMaxLogCount = 16;

	[SerializeField]
	[HideInInspector]
	[Tooltip("While the console window is hidden, incoming logs will be queued but not immediately processed until the console window is opened (to avoid wasting CPU resources). When the log queue exceeds this limit, the first logs in the queue will be processed to enforce this limit. Processed logs won't increase RAM usage if they've been seen before (i.e. collapsible logs) but this is not the case for queued logs, so if a log is spammed every frame, it will fill the whole queue in an instant. Which is why there is a queue limit")]
	private int queuedLogLimit = 256;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, the command input field at the bottom of the console window will automatically be cleared after entering a command")]
	private bool clearCommandAfterExecution = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("Console keeps track of the previously entered commands. This value determines the capacity of the command history (you can scroll through the history via up and down arrow keys while the command input field is focused)")]
	private int commandHistorySize = 15;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, while typing a command, all of the matching commands' signatures will be displayed in a popup")]
	private bool showCommandSuggestions = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, on Android platform, logcat entries of the application will also be logged to the console with the prefix \"LOGCAT: \". This may come in handy especially if you want to access the native logs of your Android plugins (like Admob)")]
	private bool receiveLogcatLogsInAndroid;

	[SerializeField]
	[HideInInspector]
	[Tooltip("Native logs will be filtered using these arguments. If left blank, all native logs of the application will be logged to the console. But if you want to e.g. see Admob's logs only, you can enter \"-s Ads\" (without quotes) here")]
	private string logcatArguments;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, on Android and iOS devices with notch screens, the console window will be repositioned so that the cutout(s) don't obscure it")]
	private bool avoidScreenCutout = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, on Android and iOS devices with notch screens, the console window's popup won't be obscured by the screen cutouts")]
	internal bool popupAvoidsScreenCutout;

	[SerializeField]
	[Tooltip("If a log is longer than this limit, it will be truncated. This helps avoid reaching Unity's 65000 vertex limit for UI canvases")]
	private int maxLogLength = 10000;

	[SerializeField]
	[HideInInspector]
	[Tooltip("If enabled, on standalone platforms, command input field will automatically be focused (start receiving keyboard input) after opening the console window")]
	private bool autoFocusOnCommandInputField = true;

	[Header("Visuals")]
	[SerializeField]
	private DebugLogItem logItemPrefab;

	[SerializeField]
	private Text commandSuggestionPrefab;

	[SerializeField]
	private Sprite infoLog;

	[SerializeField]
	private Sprite warningLog;

	[SerializeField]
	private Sprite errorLog;

	private Sprite[] logSpriteRepresentations;

	[SerializeField]
	private Sprite resizeIconAllDirections;

	[SerializeField]
	private Sprite resizeIconVerticalOnly;

	[SerializeField]
	private Color collapseButtonNormalColor;

	[SerializeField]
	private Color collapseButtonSelectedColor;

	[SerializeField]
	private Color filterButtonsNormalColor;

	[SerializeField]
	private Color filterButtonsSelectedColor;

	[SerializeField]
	private string commandSuggestionHighlightStart = "<color=orange>";

	[SerializeField]
	private string commandSuggestionHighlightEnd = "</color>";

	[Header("Internal References")]
	[SerializeField]
	private RectTransform logWindowTR;

	internal RectTransform canvasTR;

	[SerializeField]
	private RectTransform logItemsContainer;

	[SerializeField]
	private RectTransform commandSuggestionsContainer;

	[SerializeField]
	private InputField commandInputField;

	[SerializeField]
	private Button hideButton;

	[SerializeField]
	private Button clearButton;

	[SerializeField]
	private Image collapseButton;

	[SerializeField]
	private Image filterInfoButton;

	[SerializeField]
	private Image filterWarningButton;

	[SerializeField]
	private Image filterErrorButton;

	[SerializeField]
	private Text infoEntryCountText;

	[SerializeField]
	private Text warningEntryCountText;

	[SerializeField]
	private Text errorEntryCountText;

	[SerializeField]
	private RectTransform searchbar;

	[SerializeField]
	private RectTransform searchbarSlotTop;

	[SerializeField]
	private RectTransform searchbarSlotBottom;

	[SerializeField]
	private Image resizeButton;

	[SerializeField]
	private GameObject snapToBottomButton;

	[SerializeField]
	private CanvasGroup logWindowCanvasGroup;

	[SerializeField]
	private DebugLogPopup popupManager;

	[SerializeField]
	private ScrollRect logItemsScrollRect;

	private RectTransform logItemsScrollRectTR;

	private Vector2 logItemsScrollRectOriginalSize;

	[SerializeField]
	private DebugLogRecycledListView recycledListView;

	private bool isLogWindowVisible = true;

	private bool screenDimensionsChanged = true;

	private float logWindowPreviousWidth;

	private int infoEntryCount;

	private int warningEntryCount;

	private int errorEntryCount;

	private bool entryCountTextsDirty;

	private int newInfoEntryCount;

	private int newWarningEntryCount;

	private int newErrorEntryCount;

	private bool isCollapseOn;

	private DebugLogFilter logFilter = DebugLogFilter.All;

	private string searchTerm;

	private bool isInSearchMode;

	[NonSerialized]
	public bool SnapToBottom = true;

	private DynamicCircularBuffer<DebugLogEntry> collapsedLogEntries;

	private DynamicCircularBuffer<DebugLogEntryTimestamp> collapsedLogEntriesTimestamps;

	private Dictionary<DebugLogEntry, DebugLogEntry> collapsedLogEntriesMap;

	private DynamicCircularBuffer<DebugLogEntry> uncollapsedLogEntries;

	private DynamicCircularBuffer<DebugLogEntryTimestamp> uncollapsedLogEntriesTimestamps;

	private DynamicCircularBuffer<DebugLogEntry> logEntriesToShow;

	private DynamicCircularBuffer<DebugLogEntryTimestamp> timestampsOfLogEntriesToShow;

	private int indexOfLogEntryToSelectAndFocus = -1;

	private bool shouldUpdateRecycledListView = true;

	private DynamicCircularBuffer<QueuedDebugLogEntry> queuedLogEntries;

	private DynamicCircularBuffer<DebugLogEntryTimestamp> queuedLogEntriesTimestamps;

	private object logEntriesLock;

	private int pendingLogToAutoExpand;

	private List<Text> commandSuggestionInstances;

	private int visibleCommandSuggestionInstances;

	private List<ConsoleMethodInfo> matchingCommandSuggestions;

	private List<int> commandCaretIndexIncrements;

	private string commandInputFieldPrevCommand;

	private string commandInputFieldPrevCommandName;

	private int commandInputFieldPrevParamCount = -1;

	private int commandInputFieldPrevCaretPos = -1;

	private int commandInputFieldPrevCaretArgumentIndex = -1;

	private string commandInputFieldAutoCompleteBase;

	private bool commandInputFieldAutoCompletedNow;

	private Stack<DebugLogEntry> pooledLogEntries;

	private Stack<DebugLogItem> pooledLogItems;

	private bool anyCollapsedLogRemoved;

	private int removedLogEntriesToShowCount;

	private CircularBuffer<string> commandHistory;

	private int commandHistoryIndex = -1;

	private string unfinishedCommand;

	internal StringBuilder sharedStringBuilder;

	private TimeSpan localTimeUtcOffset;

	private float lastElapsedSeconds;

	private int lastFrameCount;

	private DebugLogEntryTimestamp dummyLogEntryTimestamp;

	private PointerEventData nullPointerEventData;

	private Action<DebugLogEntry> poolLogEntryAction;

	private Action<DebugLogEntry> removeUncollapsedLogEntryAction;

	private Predicate<DebugLogEntry> shouldRemoveCollapsedLogEntryPredicate;

	private Predicate<DebugLogEntry> shouldRemoveLogEntryToShowPredicate;

	private Action<DebugLogEntry, int> updateLogEntryCollapsedIndexAction;

	public Action OnLogWindowShown;

	public Action OnLogWindowHidden;

	public static DebugLogManager Instance { get; private set; }

	public bool IsLogWindowVisible => isLogWindowVisible;

	public bool PopupEnabled
	{
		get
		{
			return popupManager.gameObject.activeSelf;
		}
		set
		{
			popupManager.gameObject.SetActive(value);
		}
	}

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
			if (singleton)
			{
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		else if (Instance != this)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		pooledLogEntries = new Stack<DebugLogEntry>(64);
		pooledLogItems = new Stack<DebugLogItem>(16);
		commandSuggestionInstances = new List<Text>(8);
		matchingCommandSuggestions = new List<ConsoleMethodInfo>(8);
		commandCaretIndexIncrements = new List<int>(8);
		queuedLogEntries = new DynamicCircularBuffer<QueuedDebugLogEntry>(Mathf.Clamp(queuedLogLimit, 16, 4096));
		commandHistory = new CircularBuffer<string>(commandHistorySize);
		logEntriesLock = new object();
		sharedStringBuilder = new StringBuilder(1024);
		canvasTR = (RectTransform)base.transform;
		logItemsScrollRectTR = (RectTransform)logItemsScrollRect.transform;
		logItemsScrollRectOriginalSize = logItemsScrollRectTR.sizeDelta;
		logSpriteRepresentations = new Sprite[5];
		logSpriteRepresentations[3] = infoLog;
		logSpriteRepresentations[2] = warningLog;
		logSpriteRepresentations[0] = errorLog;
		logSpriteRepresentations[4] = errorLog;
		logSpriteRepresentations[1] = errorLog;
		filterInfoButton.color = filterButtonsSelectedColor;
		filterWarningButton.color = filterButtonsSelectedColor;
		filterErrorButton.color = filterButtonsSelectedColor;
		resizeButton.sprite = (enableHorizontalResizing ? resizeIconAllDirections : resizeIconVerticalOnly);
		collapsedLogEntries = new DynamicCircularBuffer<DebugLogEntry>(128);
		collapsedLogEntriesMap = new Dictionary<DebugLogEntry, DebugLogEntry>(128, new DebugLogEntryContentEqualityComparer());
		uncollapsedLogEntries = new DynamicCircularBuffer<DebugLogEntry>(256);
		logEntriesToShow = new DynamicCircularBuffer<DebugLogEntry>(256);
		if (captureLogTimestamps)
		{
			collapsedLogEntriesTimestamps = new DynamicCircularBuffer<DebugLogEntryTimestamp>(128);
			uncollapsedLogEntriesTimestamps = new DynamicCircularBuffer<DebugLogEntryTimestamp>(256);
			timestampsOfLogEntriesToShow = new DynamicCircularBuffer<DebugLogEntryTimestamp>(256);
			queuedLogEntriesTimestamps = new DynamicCircularBuffer<DebugLogEntryTimestamp>(queuedLogEntries.Capacity);
		}
		recycledListView.Initialize(this, logEntriesToShow, timestampsOfLogEntriesToShow, logItemPrefab.Transform.sizeDelta.y);
		if (minimumWidth < 100f)
		{
			minimumWidth = 100f;
		}
		if (minimumHeight < 200f)
		{
			minimumHeight = 200f;
		}
		if (!resizeFromRight)
		{
			RectTransform resizeButtonTR = (RectTransform)resizeButton.GetComponentInParent<DebugLogResizeListener>().transform;
			resizeButtonTR.anchorMin = new Vector2(0f, resizeButtonTR.anchorMin.y);
			resizeButtonTR.anchorMax = new Vector2(0f, resizeButtonTR.anchorMax.y);
			resizeButtonTR.pivot = new Vector2(0f, resizeButtonTR.pivot.y);
			((RectTransform)commandInputField.transform).anchoredPosition += new Vector2(resizeButtonTR.sizeDelta.x, 0f);
		}
		if (enableSearchbar)
		{
			searchbar.GetComponent<InputField>().onValueChanged.AddListener(SearchTermChanged);
		}
		else
		{
			searchbar = null;
			searchbarSlotTop.gameObject.SetActive(value: false);
			searchbarSlotBottom.gameObject.SetActive(value: false);
		}
		filterInfoButton.gameObject.SetActive(receiveInfoLogs);
		filterWarningButton.gameObject.SetActive(receiveWarningLogs);
		filterErrorButton.gameObject.SetActive(receiveErrorLogs || receiveExceptionLogs);
		if (commandSuggestionsContainer.gameObject.activeSelf)
		{
			commandSuggestionsContainer.gameObject.SetActive(value: false);
		}
		InputField inputField = commandInputField;
		inputField.onValidateInput = (InputField.OnValidateInput)Delegate.Combine(inputField.onValidateInput, new InputField.OnValidateInput(OnValidateCommand));
		commandInputField.onValueChanged.AddListener(OnEditCommand);
		commandInputField.onEndEdit.AddListener(OnEndEditCommand);
		hideButton.onClick.AddListener(HideLogWindow);
		clearButton.onClick.AddListener(ClearLogs);
		collapseButton.GetComponent<Button>().onClick.AddListener(CollapseButtonPressed);
		filterInfoButton.GetComponent<Button>().onClick.AddListener(FilterLogButtonPressed);
		filterWarningButton.GetComponent<Button>().onClick.AddListener(FilterWarningButtonPressed);
		filterErrorButton.GetComponent<Button>().onClick.AddListener(FilterErrorButtonPressed);
		snapToBottomButton.GetComponent<Button>().onClick.AddListener(delegate
		{
			SnapToBottom = true;
		});
		localTimeUtcOffset = DateTime.Now - DateTime.UtcNow;
		dummyLogEntryTimestamp = default(DebugLogEntryTimestamp);
		nullPointerEventData = new PointerEventData(null);
		poolLogEntryAction = PoolLogEntry;
		removeUncollapsedLogEntryAction = RemoveUncollapsedLogEntry;
		shouldRemoveCollapsedLogEntryPredicate = ShouldRemoveCollapsedLogEntry;
		shouldRemoveLogEntryToShowPredicate = ShouldRemoveLogEntryToShow;
		updateLogEntryCollapsedIndexAction = UpdateLogEntryCollapsedIndex;
		if (receiveLogsWhileInactive)
		{
			Application.logMessageReceivedThreaded -= ReceivedLog;
			Application.logMessageReceivedThreaded += ReceivedLog;
		}
	}

	private void OnEnable()
	{
		if (!(Instance != this))
		{
			if (!receiveLogsWhileInactive)
			{
				Application.logMessageReceivedThreaded -= ReceivedLog;
				Application.logMessageReceivedThreaded += ReceivedLog;
			}
			_ = receiveLogcatLogsInAndroid;
			DebugLogConsole.AddCommand("logs.save", "Saves logs to persistentDataPath", SaveLogsToFile);
			DebugLogConsole.AddCommand("logs.save", "Saves logs to the specified file", (Action<string>)SaveLogsToFile);
		}
	}

	private void OnDisable()
	{
		if (!(Instance != this))
		{
			if (!receiveLogsWhileInactive)
			{
				Application.logMessageReceivedThreaded -= ReceivedLog;
			}
			DebugLogConsole.RemoveCommand("logs.save");
		}
	}

	private void Start()
	{
		if ((enablePopup && startInPopupMode) || (!enablePopup && startMinimized))
		{
			HideLogWindow();
		}
		else
		{
			ShowLogWindow();
		}
		PopupEnabled = enablePopup;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		if (receiveLogsWhileInactive)
		{
			Application.logMessageReceivedThreaded -= ReceivedLog;
		}
	}

	private void OnRectTransformDimensionsChange()
	{
		screenDimensionsChanged = true;
	}

	private void Update()
	{
		lastElapsedSeconds = Time.realtimeSinceStartup;
		lastFrameCount = Time.frameCount;
		if (toggleWithKey && Input.GetKeyDown(toggleKey))
		{
			if (isLogWindowVisible)
			{
				HideLogWindow();
			}
			else
			{
				ShowLogWindow();
			}
		}
	}

	private void LateUpdate()
	{
		int numberOfLogsToProcess = (isLogWindowVisible ? queuedLogEntries.Count : (queuedLogEntries.Count - queuedLogLimit));
		ProcessQueuedLogs(numberOfLogsToProcess);
		if (uncollapsedLogEntries.Count >= maxLogCount)
		{
			int numberOfLogsToRemove = Mathf.Min((!isLogWindowVisible) ? logsToRemoveAfterMaxLogCount : (uncollapsedLogEntries.Count - maxLogCount + logsToRemoveAfterMaxLogCount), uncollapsedLogEntries.Count);
			RemoveOldestLogs(numberOfLogsToRemove);
		}
		if (!isLogWindowVisible && !PopupEnabled)
		{
			return;
		}
		int newInfoEntryCount;
		int newWarningEntryCount;
		int newErrorEntryCount;
		lock (logEntriesLock)
		{
			newInfoEntryCount = this.newInfoEntryCount;
			newWarningEntryCount = this.newWarningEntryCount;
			newErrorEntryCount = this.newErrorEntryCount;
			this.newInfoEntryCount = 0;
			this.newWarningEntryCount = 0;
			this.newErrorEntryCount = 0;
		}
		if (newInfoEntryCount > 0 || newWarningEntryCount > 0 || newErrorEntryCount > 0)
		{
			if (newInfoEntryCount > 0)
			{
				infoEntryCount += newInfoEntryCount;
				if (isLogWindowVisible)
				{
					infoEntryCountText.text = infoEntryCount.ToString();
				}
			}
			if (newWarningEntryCount > 0)
			{
				warningEntryCount += newWarningEntryCount;
				if (isLogWindowVisible)
				{
					warningEntryCountText.text = warningEntryCount.ToString();
				}
			}
			if (newErrorEntryCount > 0)
			{
				errorEntryCount += newErrorEntryCount;
				if (isLogWindowVisible)
				{
					errorEntryCountText.text = errorEntryCount.ToString();
				}
			}
			if (!isLogWindowVisible)
			{
				entryCountTextsDirty = true;
				popupManager.NewLogsArrived(newInfoEntryCount, newWarningEntryCount, newErrorEntryCount);
			}
		}
		if (isLogWindowVisible)
		{
			if (shouldUpdateRecycledListView)
			{
				OnLogEntriesUpdated(updateAllVisibleItemContents: false, validateScrollPosition: false);
			}
			if (indexOfLogEntryToSelectAndFocus >= 0)
			{
				if (indexOfLogEntryToSelectAndFocus < logEntriesToShow.Count)
				{
					recycledListView.SelectAndFocusOnLogItemAtIndex(indexOfLogEntryToSelectAndFocus);
				}
				indexOfLogEntryToSelectAndFocus = -1;
			}
			if (entryCountTextsDirty)
			{
				infoEntryCountText.text = infoEntryCount.ToString();
				warningEntryCountText.text = warningEntryCount.ToString();
				errorEntryCountText.text = errorEntryCount.ToString();
				entryCountTextsDirty = false;
			}
			float logWindowWidth = logWindowTR.rect.width;
			if (!Mathf.Approximately(logWindowWidth, logWindowPreviousWidth))
			{
				logWindowPreviousWidth = logWindowWidth;
				if ((bool)searchbar)
				{
					if (logWindowWidth >= topSearchbarMinWidth)
					{
						if (searchbar.parent == searchbarSlotBottom)
						{
							searchbarSlotTop.gameObject.SetActive(value: true);
							searchbar.SetParent(searchbarSlotTop, worldPositionStays: false);
							searchbarSlotBottom.gameObject.SetActive(value: false);
							logItemsScrollRectTR.anchoredPosition = Vector2.zero;
							logItemsScrollRectTR.sizeDelta = logItemsScrollRectOriginalSize;
						}
					}
					else if (searchbar.parent == searchbarSlotTop)
					{
						searchbarSlotBottom.gameObject.SetActive(value: true);
						searchbar.SetParent(searchbarSlotBottom, worldPositionStays: false);
						searchbarSlotTop.gameObject.SetActive(value: false);
						float searchbarHeight = searchbarSlotBottom.sizeDelta.y;
						logItemsScrollRectTR.anchoredPosition = new Vector2(0f, searchbarHeight * -0.5f);
						logItemsScrollRectTR.sizeDelta = logItemsScrollRectOriginalSize - new Vector2(0f, searchbarHeight);
					}
				}
				recycledListView.OnViewportWidthChanged();
			}
			if (SnapToBottom)
			{
				logItemsScrollRect.verticalNormalizedPosition = 0f;
				if (snapToBottomButton.activeSelf)
				{
					snapToBottomButton.SetActive(value: false);
				}
			}
			else
			{
				float scrollPos = logItemsScrollRect.verticalNormalizedPosition;
				if (snapToBottomButton.activeSelf != (scrollPos > 1E-06f && scrollPos < 0.9999f))
				{
					snapToBottomButton.SetActive(!snapToBottomButton.activeSelf);
				}
			}
			if (showCommandSuggestions && commandInputField.isFocused && commandInputField.caretPosition != commandInputFieldPrevCaretPos)
			{
				RefreshCommandSuggestions(commandInputField.text);
			}
			if (commandInputField.isFocused && commandHistory.Count > 0)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					if (commandHistoryIndex == -1)
					{
						commandHistoryIndex = commandHistory.Count - 1;
						unfinishedCommand = commandInputField.text;
					}
					else if (--commandHistoryIndex < 0)
					{
						commandHistoryIndex = 0;
					}
					commandInputField.text = commandHistory[commandHistoryIndex];
					commandInputField.caretPosition = commandInputField.text.Length;
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow) && commandHistoryIndex != -1)
				{
					if (++commandHistoryIndex < commandHistory.Count)
					{
						commandInputField.text = commandHistory[commandHistoryIndex];
					}
					else
					{
						commandHistoryIndex = -1;
						commandInputField.text = unfinishedCommand ?? string.Empty;
					}
				}
			}
		}
		if (screenDimensionsChanged)
		{
			if (isLogWindowVisible)
			{
				recycledListView.OnViewportHeightChanged();
			}
			else
			{
				popupManager.UpdatePosition(immediately: true);
			}
			screenDimensionsChanged = false;
		}
	}

	public void ShowLogWindow()
	{
		logWindowCanvasGroup.blocksRaycasts = true;
		logWindowCanvasGroup.alpha = 1f;
		popupManager.Hide();
		OnLogEntriesUpdated(updateAllVisibleItemContents: true, validateScrollPosition: true);
		if (autoFocusOnCommandInputField)
		{
			StartCoroutine(ActivateCommandInputFieldCoroutine());
		}
		isLogWindowVisible = true;
		if (OnLogWindowShown != null)
		{
			OnLogWindowShown();
		}
	}

	public void HideLogWindow()
	{
		logWindowCanvasGroup.blocksRaycasts = false;
		logWindowCanvasGroup.alpha = 0f;
		if (commandInputField.isFocused)
		{
			commandInputField.DeactivateInputField();
		}
		popupManager.Show();
		isLogWindowVisible = false;
		if (EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		if (OnLogWindowHidden != null)
		{
			OnLogWindowHidden();
		}
	}

	private char OnValidateCommand(string text, int charIndex, char addedChar)
	{
		switch (addedChar)
		{
		case '\t':
			if (!string.IsNullOrEmpty(text))
			{
				if (string.IsNullOrEmpty(commandInputFieldAutoCompleteBase))
				{
					commandInputFieldAutoCompleteBase = text;
				}
				string autoCompletedCommand = DebugLogConsole.GetAutoCompleteCommand(commandInputFieldAutoCompleteBase, text);
				if (!string.IsNullOrEmpty(autoCompletedCommand) && autoCompletedCommand != text)
				{
					commandInputFieldAutoCompletedNow = true;
					commandInputField.text = autoCompletedCommand;
				}
			}
			return '\0';
		case '\n':
			if (clearCommandAfterExecution)
			{
				commandInputField.text = string.Empty;
			}
			if (text.Length > 0)
			{
				if (commandHistory.Count == 0 || commandHistory[commandHistory.Count - 1] != text)
				{
					commandHistory.Add(text);
				}
				commandHistoryIndex = -1;
				unfinishedCommand = null;
				DebugLogConsole.ExecuteCommand(text);
				SnapToBottom = true;
			}
			return '\0';
		default:
			return addedChar;
		}
	}

	public void ReceivedLog(string logString, string stackTrace, LogType logType)
	{
		switch (logType)
		{
		case LogType.Log:
			if (!receiveInfoLogs)
			{
				return;
			}
			break;
		case LogType.Warning:
			if (!receiveWarningLogs)
			{
				return;
			}
			break;
		case LogType.Error:
			if (!receiveErrorLogs)
			{
				return;
			}
			break;
		case LogType.Assert:
		case LogType.Exception:
			if (!receiveExceptionLogs)
			{
				return;
			}
			break;
		}
		int logLength = logString.Length;
		if (stackTrace == null)
		{
			if (logLength > maxLogLength)
			{
				logString = logString.Substring(0, maxLogLength - 11) + "<truncated>";
			}
		}
		else
		{
			logLength += stackTrace.Length;
			if (logLength > maxLogLength)
			{
				int halfMaxLogLength = maxLogLength / 2;
				if (logString.Length >= halfMaxLogLength)
				{
					if (stackTrace.Length >= halfMaxLogLength)
					{
						logString = logString.Substring(0, halfMaxLogLength - 11) + "<truncated>";
						stackTrace = stackTrace.Substring(0, halfMaxLogLength - 12) + "<truncated>\n";
					}
					else
					{
						logString = logString.Substring(0, maxLogLength - stackTrace.Length - 11) + "<truncated>";
					}
				}
				else
				{
					stackTrace = stackTrace.Substring(0, maxLogLength - logString.Length - 12) + "<truncated>\n";
				}
			}
		}
		QueuedDebugLogEntry queuedLogEntry = new QueuedDebugLogEntry(logString, stackTrace, logType);
		DebugLogEntryTimestamp queuedLogEntryTimestamp;
		if (queuedLogEntriesTimestamps != null)
		{
			DateTime dateTime = DateTime.UtcNow + localTimeUtcOffset;
			queuedLogEntryTimestamp = new DebugLogEntryTimestamp(dateTime, lastElapsedSeconds, lastFrameCount);
		}
		else
		{
			queuedLogEntryTimestamp = dummyLogEntryTimestamp;
		}
		lock (logEntriesLock)
		{
			if (queuedLogEntries.Count + 1 >= maxLogCount)
			{
				switch (queuedLogEntries.RemoveFirst().logType)
				{
				case LogType.Log:
					newInfoEntryCount--;
					break;
				case LogType.Warning:
					newWarningEntryCount--;
					break;
				default:
					newErrorEntryCount--;
					break;
				}
				if (queuedLogEntriesTimestamps != null)
				{
					queuedLogEntriesTimestamps.RemoveFirst();
				}
			}
			queuedLogEntries.Add(queuedLogEntry);
			if (queuedLogEntriesTimestamps != null)
			{
				queuedLogEntriesTimestamps.Add(queuedLogEntryTimestamp);
			}
			switch (logType)
			{
			case LogType.Log:
				newInfoEntryCount++;
				break;
			case LogType.Warning:
				newWarningEntryCount++;
				break;
			default:
				newErrorEntryCount++;
				break;
			}
		}
	}

	private void ProcessQueuedLogs(int numberOfLogsToProcess)
	{
		for (int i = 0; i < numberOfLogsToProcess; i++)
		{
			QueuedDebugLogEntry logEntry;
			DebugLogEntryTimestamp timestamp;
			lock (logEntriesLock)
			{
				logEntry = queuedLogEntries.RemoveFirst();
				timestamp = ((queuedLogEntriesTimestamps != null) ? queuedLogEntriesTimestamps.RemoveFirst() : dummyLogEntryTimestamp);
			}
			ProcessLog(logEntry, timestamp);
		}
	}

	private void ProcessLog(QueuedDebugLogEntry queuedLogEntry, DebugLogEntryTimestamp timestamp)
	{
		LogType logType = queuedLogEntry.logType;
		DebugLogEntry logEntry = ((pooledLogEntries.Count <= 0) ? new DebugLogEntry() : pooledLogEntries.Pop());
		logEntry.Initialize(queuedLogEntry.logString, queuedLogEntry.stackTrace);
		DebugLogEntry existingLogEntry;
		bool isEntryInCollapsedEntryList = collapsedLogEntriesMap.TryGetValue(logEntry, out existingLogEntry);
		if (!isEntryInCollapsedEntryList)
		{
			logEntry.logTypeSpriteRepresentation = logSpriteRepresentations[(int)logType];
			logEntry.collapsedIndex = collapsedLogEntries.Count;
			collapsedLogEntries.Add(logEntry);
			collapsedLogEntriesMap[logEntry] = logEntry;
			if (collapsedLogEntriesTimestamps != null)
			{
				collapsedLogEntriesTimestamps.Add(timestamp);
			}
		}
		else
		{
			PoolLogEntry(logEntry);
			logEntry = existingLogEntry;
			logEntry.count++;
			if (collapsedLogEntriesTimestamps != null)
			{
				collapsedLogEntriesTimestamps[logEntry.collapsedIndex] = timestamp;
			}
		}
		uncollapsedLogEntries.Add(logEntry);
		if (uncollapsedLogEntriesTimestamps != null)
		{
			uncollapsedLogEntriesTimestamps.Add(timestamp);
		}
		int logEntryIndexInEntriesToShow = -1;
		Sprite logTypeSpriteRepresentation = logEntry.logTypeSpriteRepresentation;
		if (isCollapseOn && isEntryInCollapsedEntryList)
		{
			if (isLogWindowVisible || timestampsOfLogEntriesToShow != null)
			{
				logEntryIndexInEntriesToShow = ((isInSearchMode || logFilter != DebugLogFilter.All) ? logEntriesToShow.IndexOf(logEntry) : logEntry.collapsedIndex);
				if (logEntryIndexInEntriesToShow >= 0)
				{
					if (timestampsOfLogEntriesToShow != null)
					{
						timestampsOfLogEntriesToShow[logEntryIndexInEntriesToShow] = timestamp;
					}
					if (isLogWindowVisible)
					{
						recycledListView.OnCollapsedLogEntryAtIndexUpdated(logEntryIndexInEntriesToShow);
					}
				}
			}
		}
		else if ((!isInSearchMode || queuedLogEntry.MatchesSearchTerm(searchTerm)) && (logFilter == DebugLogFilter.All || (logTypeSpriteRepresentation == infoLog && (logFilter & DebugLogFilter.Info) == DebugLogFilter.Info) || (logTypeSpriteRepresentation == warningLog && (logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning) || (logTypeSpriteRepresentation == errorLog && (logFilter & DebugLogFilter.Error) == DebugLogFilter.Error)))
		{
			logEntriesToShow.Add(logEntry);
			logEntryIndexInEntriesToShow = logEntriesToShow.Count - 1;
			if (timestampsOfLogEntriesToShow != null)
			{
				timestampsOfLogEntriesToShow.Add(timestamp);
			}
			shouldUpdateRecycledListView = true;
		}
		if (pendingLogToAutoExpand > 0 && --pendingLogToAutoExpand <= 0 && logEntryIndexInEntriesToShow >= 0)
		{
			indexOfLogEntryToSelectAndFocus = logEntryIndexInEntriesToShow;
		}
	}

	private void RemoveOldestLogs(int numberOfLogsToRemove)
	{
		if (numberOfLogsToRemove <= 0)
		{
			return;
		}
		DebugLogEntry logEntryToSelectAndFocus = ((indexOfLogEntryToSelectAndFocus >= 0 && indexOfLogEntryToSelectAndFocus < logEntriesToShow.Count) ? logEntriesToShow[indexOfLogEntryToSelectAndFocus] : null);
		anyCollapsedLogRemoved = false;
		removedLogEntriesToShowCount = 0;
		uncollapsedLogEntries.TrimStart(numberOfLogsToRemove, removeUncollapsedLogEntryAction);
		if (uncollapsedLogEntriesTimestamps != null)
		{
			uncollapsedLogEntriesTimestamps.TrimStart(numberOfLogsToRemove);
		}
		if (removedLogEntriesToShowCount > 0)
		{
			logEntriesToShow.TrimStart(removedLogEntriesToShowCount);
			if (timestampsOfLogEntriesToShow != null)
			{
				timestampsOfLogEntriesToShow.TrimStart(removedLogEntriesToShowCount);
			}
		}
		if (anyCollapsedLogRemoved)
		{
			collapsedLogEntries.RemoveAll(shouldRemoveCollapsedLogEntryPredicate, updateLogEntryCollapsedIndexAction, collapsedLogEntriesTimestamps);
			if (isCollapseOn)
			{
				removedLogEntriesToShowCount = logEntriesToShow.RemoveAll(shouldRemoveLogEntryToShowPredicate, null, timestampsOfLogEntriesToShow);
			}
		}
		if (removedLogEntriesToShowCount > 0)
		{
			if (logEntryToSelectAndFocus == null || logEntryToSelectAndFocus.count == 0)
			{
				indexOfLogEntryToSelectAndFocus = -1;
			}
			else
			{
				for (int i = Mathf.Min(indexOfLogEntryToSelectAndFocus, logEntriesToShow.Count - 1); i >= 0; i--)
				{
					if (logEntriesToShow[i] == logEntryToSelectAndFocus)
					{
						indexOfLogEntryToSelectAndFocus = i;
						break;
					}
				}
			}
			recycledListView.OnLogEntriesRemoved(removedLogEntriesToShowCount);
			if (isLogWindowVisible)
			{
				OnLogEntriesUpdated(updateAllVisibleItemContents: false, validateScrollPosition: true);
			}
		}
		else if (isLogWindowVisible && isCollapseOn)
		{
			recycledListView.RefreshCollapsedLogEntryCounts();
		}
		entryCountTextsDirty = true;
	}

	private void RemoveUncollapsedLogEntry(DebugLogEntry logEntry)
	{
		if (--logEntry.count <= 0)
		{
			anyCollapsedLogRemoved = true;
		}
		if (!isCollapseOn && logEntriesToShow[removedLogEntriesToShowCount] == logEntry)
		{
			removedLogEntriesToShowCount++;
		}
		if (logEntry.logTypeSpriteRepresentation == infoLog)
		{
			infoEntryCount--;
		}
		else if (logEntry.logTypeSpriteRepresentation == warningLog)
		{
			warningEntryCount--;
		}
		else
		{
			errorEntryCount--;
		}
	}

	private bool ShouldRemoveCollapsedLogEntry(DebugLogEntry logEntry)
	{
		if (logEntry.count <= 0)
		{
			PoolLogEntry(logEntry);
			collapsedLogEntriesMap.Remove(logEntry);
			return true;
		}
		return false;
	}

	private bool ShouldRemoveLogEntryToShow(DebugLogEntry logEntry)
	{
		return logEntry.count <= 0;
	}

	private void UpdateLogEntryCollapsedIndex(DebugLogEntry logEntry, int collapsedIndex)
	{
		logEntry.collapsedIndex = collapsedIndex;
	}

	private void OnLogEntriesUpdated(bool updateAllVisibleItemContents, bool validateScrollPosition)
	{
		recycledListView.OnLogEntriesUpdated(updateAllVisibleItemContents);
		shouldUpdateRecycledListView = false;
		if (validateScrollPosition)
		{
			ValidateScrollPosition();
		}
	}

	private void PoolLogEntry(DebugLogEntry logEntry)
	{
		if (pooledLogEntries.Count < 4096)
		{
			logEntry.Clear();
			pooledLogEntries.Push(logEntry);
		}
	}

	internal void ValidateScrollPosition()
	{
		if (logItemsScrollRect.verticalNormalizedPosition <= Mathf.Epsilon)
		{
			logItemsScrollRect.verticalNormalizedPosition = 0.0001f;
		}
		logItemsScrollRect.OnScroll(nullPointerEventData);
	}

	public void AdjustLatestPendingLog(bool autoExpand, bool stripStackTrace)
	{
		lock (logEntriesLock)
		{
			if (queuedLogEntries.Count != 0)
			{
				if (autoExpand)
				{
					pendingLogToAutoExpand = queuedLogEntries.Count;
				}
				if (stripStackTrace)
				{
					QueuedDebugLogEntry log = queuedLogEntries[queuedLogEntries.Count - 1];
					queuedLogEntries[queuedLogEntries.Count - 1] = new QueuedDebugLogEntry(log.logString, string.Empty, log.logType);
				}
			}
		}
	}

	public void ClearLogs()
	{
		SnapToBottom = true;
		indexOfLogEntryToSelectAndFocus = -1;
		infoEntryCount = 0;
		warningEntryCount = 0;
		errorEntryCount = 0;
		infoEntryCountText.text = "0";
		warningEntryCountText.text = "0";
		errorEntryCountText.text = "0";
		collapsedLogEntries.ForEach(poolLogEntryAction);
		collapsedLogEntries.Clear();
		collapsedLogEntriesMap.Clear();
		uncollapsedLogEntries.Clear();
		logEntriesToShow.Clear();
		if (collapsedLogEntriesTimestamps != null)
		{
			collapsedLogEntriesTimestamps.Clear();
			uncollapsedLogEntriesTimestamps.Clear();
			timestampsOfLogEntriesToShow.Clear();
		}
		recycledListView.DeselectSelectedLogItem();
		OnLogEntriesUpdated(updateAllVisibleItemContents: true, validateScrollPosition: true);
	}

	private void CollapseButtonPressed()
	{
		isCollapseOn = !isCollapseOn;
		SnapToBottom = true;
		collapseButton.color = (isCollapseOn ? collapseButtonSelectedColor : collapseButtonNormalColor);
		recycledListView.SetCollapseMode(isCollapseOn);
		FilterLogs();
	}

	private void FilterLogButtonPressed()
	{
		logFilter ^= DebugLogFilter.Info;
		if ((logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)
		{
			filterInfoButton.color = filterButtonsSelectedColor;
		}
		else
		{
			filterInfoButton.color = filterButtonsNormalColor;
		}
		FilterLogs();
	}

	private void FilterWarningButtonPressed()
	{
		logFilter ^= DebugLogFilter.Warning;
		if ((logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)
		{
			filterWarningButton.color = filterButtonsSelectedColor;
		}
		else
		{
			filterWarningButton.color = filterButtonsNormalColor;
		}
		FilterLogs();
	}

	private void FilterErrorButtonPressed()
	{
		logFilter ^= DebugLogFilter.Error;
		if ((logFilter & DebugLogFilter.Error) == DebugLogFilter.Error)
		{
			filterErrorButton.color = filterButtonsSelectedColor;
		}
		else
		{
			filterErrorButton.color = filterButtonsNormalColor;
		}
		FilterLogs();
	}

	private void SearchTermChanged(string searchTerm)
	{
		if (searchTerm != null)
		{
			searchTerm = searchTerm.Trim();
		}
		this.searchTerm = searchTerm;
		bool isInSearchMode = !string.IsNullOrEmpty(searchTerm);
		if (isInSearchMode || this.isInSearchMode)
		{
			this.isInSearchMode = isInSearchMode;
			FilterLogs();
		}
	}

	private void RefreshCommandSuggestions(string command)
	{
		if (!showCommandSuggestions)
		{
			return;
		}
		commandInputFieldPrevCaretPos = commandInputField.caretPosition;
		bool commandChanged = command != commandInputFieldPrevCommand;
		bool commandNameOrParametersChanged = false;
		if (commandChanged)
		{
			commandInputFieldPrevCommand = command;
			matchingCommandSuggestions.Clear();
			commandCaretIndexIncrements.Clear();
			string text = commandInputFieldPrevCommandName;
			DebugLogConsole.GetCommandSuggestions(command, matchingCommandSuggestions, commandCaretIndexIncrements, ref commandInputFieldPrevCommandName, out var numberOfParameters);
			if (text != commandInputFieldPrevCommandName || numberOfParameters != commandInputFieldPrevParamCount)
			{
				commandInputFieldPrevParamCount = numberOfParameters;
				commandNameOrParametersChanged = true;
			}
		}
		int caretArgumentIndex = 0;
		int caretPos = commandInputField.caretPosition;
		for (int i = 0; i < commandCaretIndexIncrements.Count && caretPos > commandCaretIndexIncrements[i]; i++)
		{
			caretArgumentIndex++;
		}
		if (caretArgumentIndex != commandInputFieldPrevCaretArgumentIndex)
		{
			commandInputFieldPrevCaretArgumentIndex = caretArgumentIndex;
		}
		else if (!commandChanged || !commandNameOrParametersChanged)
		{
			return;
		}
		if (matchingCommandSuggestions.Count == 0)
		{
			OnEndEditCommand(command);
			return;
		}
		if (!commandSuggestionsContainer.gameObject.activeSelf)
		{
			commandSuggestionsContainer.gameObject.SetActive(value: true);
		}
		int suggestionInstancesCount = commandSuggestionInstances.Count;
		int suggestionsCount = matchingCommandSuggestions.Count;
		for (int j = 0; j < suggestionsCount; j++)
		{
			if (j >= visibleCommandSuggestionInstances)
			{
				if (j >= suggestionInstancesCount)
				{
					commandSuggestionInstances.Add(global::UnityEngine.Object.Instantiate(commandSuggestionPrefab, commandSuggestionsContainer, worldPositionStays: false));
				}
				else
				{
					commandSuggestionInstances[j].gameObject.SetActive(value: true);
				}
				visibleCommandSuggestionInstances++;
			}
			ConsoleMethodInfo suggestedCommand = matchingCommandSuggestions[j];
			sharedStringBuilder.Length = 0;
			if (caretArgumentIndex > 0)
			{
				sharedStringBuilder.Append(suggestedCommand.command);
			}
			else
			{
				sharedStringBuilder.Append(commandSuggestionHighlightStart).Append(matchingCommandSuggestions[j].command).Append(commandSuggestionHighlightEnd);
			}
			if (suggestedCommand.parameters.Length != 0)
			{
				sharedStringBuilder.Append(" ");
				int caretParameterIndex = caretArgumentIndex - 1;
				if (caretParameterIndex >= suggestedCommand.parameters.Length)
				{
					caretParameterIndex = suggestedCommand.parameters.Length - 1;
				}
				for (int k = 0; k < suggestedCommand.parameters.Length; k++)
				{
					if (caretParameterIndex != k)
					{
						sharedStringBuilder.Append(suggestedCommand.parameters[k]);
					}
					else
					{
						sharedStringBuilder.Append(commandSuggestionHighlightStart).Append(suggestedCommand.parameters[k]).Append(commandSuggestionHighlightEnd);
					}
				}
			}
			commandSuggestionInstances[j].text = sharedStringBuilder.ToString();
		}
		for (int i2 = visibleCommandSuggestionInstances - 1; i2 >= suggestionsCount; i2--)
		{
			commandSuggestionInstances[i2].gameObject.SetActive(value: false);
		}
		visibleCommandSuggestionInstances = suggestionsCount;
	}

	private void OnEditCommand(string command)
	{
		RefreshCommandSuggestions(command);
		if (!commandInputFieldAutoCompletedNow)
		{
			commandInputFieldAutoCompleteBase = null;
		}
		else
		{
			commandInputFieldAutoCompletedNow = false;
		}
	}

	private void OnEndEditCommand(string command)
	{
		if (commandSuggestionsContainer.gameObject.activeSelf)
		{
			commandSuggestionsContainer.gameObject.SetActive(value: false);
		}
	}

	internal void Resize(PointerEventData eventData)
	{
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTR, eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			return;
		}
		Rect resizeButtonRect = ((RectTransform)resizeButton.rectTransform.parent).rect;
		float resizeButtonWidth = resizeButtonRect.width;
		float resizeButtonHeight = resizeButtonRect.height;
		Vector2 canvasPivot = canvasTR.pivot;
		Vector2 canvasSize = canvasTR.rect.size;
		Vector2 anchorMin = logWindowTR.anchorMin;
		if (enableHorizontalResizing)
		{
			if (resizeFromRight)
			{
				localPoint.x += canvasPivot.x * canvasSize.x + resizeButtonWidth;
				if (localPoint.x < minimumWidth)
				{
					localPoint.x = minimumWidth;
				}
				Vector2 anchorMax = logWindowTR.anchorMax;
				anchorMax.x = Mathf.Clamp01(localPoint.x / canvasSize.x);
				logWindowTR.anchorMax = anchorMax;
			}
			else
			{
				localPoint.x += canvasPivot.x * canvasSize.x - resizeButtonWidth;
				if (localPoint.x > canvasSize.x - minimumWidth)
				{
					localPoint.x = canvasSize.x - minimumWidth;
				}
				anchorMin.x = Mathf.Clamp01(localPoint.x / canvasSize.x);
			}
		}
		float notchHeight = 0f - logWindowTR.sizeDelta.y;
		localPoint.y += canvasPivot.y * canvasSize.y - resizeButtonHeight;
		if (localPoint.y > canvasSize.y - minimumHeight - notchHeight)
		{
			localPoint.y = canvasSize.y - minimumHeight - notchHeight;
		}
		anchorMin.y = Mathf.Clamp01(localPoint.y / canvasSize.y);
		logWindowTR.anchorMin = anchorMin;
		recycledListView.OnViewportHeightChanged();
	}

	private void FilterLogs()
	{
		logEntriesToShow.Clear();
		if (timestampsOfLogEntriesToShow != null)
		{
			timestampsOfLogEntriesToShow.Clear();
		}
		if (logFilter != 0)
		{
			DynamicCircularBuffer<DebugLogEntry> targetLogEntries = (isCollapseOn ? collapsedLogEntries : uncollapsedLogEntries);
			DynamicCircularBuffer<DebugLogEntryTimestamp> targetLogEntriesTimestamps = (isCollapseOn ? collapsedLogEntriesTimestamps : uncollapsedLogEntriesTimestamps);
			if (logFilter == DebugLogFilter.All)
			{
				if (!isInSearchMode)
				{
					logEntriesToShow.AddRange(targetLogEntries);
					if (timestampsOfLogEntriesToShow != null)
					{
						timestampsOfLogEntriesToShow.AddRange(targetLogEntriesTimestamps);
					}
				}
				else
				{
					int i = 0;
					for (int count = targetLogEntries.Count; i < count; i++)
					{
						if (targetLogEntries[i].MatchesSearchTerm(searchTerm))
						{
							logEntriesToShow.Add(targetLogEntries[i]);
							if (timestampsOfLogEntriesToShow != null)
							{
								timestampsOfLogEntriesToShow.Add(targetLogEntriesTimestamps[i]);
							}
						}
					}
				}
			}
			else
			{
				bool isInfoEnabled = (logFilter & DebugLogFilter.Info) == DebugLogFilter.Info;
				bool isWarningEnabled = (logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning;
				bool isErrorEnabled = (logFilter & DebugLogFilter.Error) == DebugLogFilter.Error;
				int j = 0;
				for (int count2 = targetLogEntries.Count; j < count2; j++)
				{
					DebugLogEntry logEntry = targetLogEntries[j];
					if (isInSearchMode && !logEntry.MatchesSearchTerm(searchTerm))
					{
						continue;
					}
					bool shouldShowLog = false;
					if (logEntry.logTypeSpriteRepresentation == infoLog)
					{
						if (isInfoEnabled)
						{
							shouldShowLog = true;
						}
					}
					else if (logEntry.logTypeSpriteRepresentation == warningLog)
					{
						if (isWarningEnabled)
						{
							shouldShowLog = true;
						}
					}
					else if (isErrorEnabled)
					{
						shouldShowLog = true;
					}
					if (shouldShowLog)
					{
						logEntriesToShow.Add(logEntry);
						if (timestampsOfLogEntriesToShow != null)
						{
							timestampsOfLogEntriesToShow.Add(targetLogEntriesTimestamps[j]);
						}
					}
				}
			}
		}
		recycledListView.DeselectSelectedLogItem();
		OnLogEntriesUpdated(updateAllVisibleItemContents: true, validateScrollPosition: true);
	}

	public string GetAllLogs()
	{
		ProcessQueuedLogs(queuedLogEntries.Count);
		int count = uncollapsedLogEntries.Count;
		int length = 0;
		int newLineLength = Environment.NewLine.Length;
		for (int i = 0; i < count; i++)
		{
			DebugLogEntry entry = uncollapsedLogEntries[i];
			length += entry.logString.Length + entry.stackTrace.Length + newLineLength * 3;
		}
		if (uncollapsedLogEntriesTimestamps != null)
		{
			length += count * 12;
		}
		length += 100;
		StringBuilder sb = new StringBuilder(length);
		for (int j = 0; j < count; j++)
		{
			DebugLogEntry entry2 = uncollapsedLogEntries[j];
			if (uncollapsedLogEntriesTimestamps != null)
			{
				uncollapsedLogEntriesTimestamps[j].AppendTime(sb);
				sb.Append(": ");
			}
			sb.AppendLine(entry2.logString).AppendLine(entry2.stackTrace).AppendLine();
		}
		return sb.ToString();
	}

	private void SaveLogsToFile()
	{
		SaveLogsToFile(Path.Combine(Application.persistentDataPath, DateTime.Now.ToString("dd-MM-yyyy--HH-mm-ss") + ".txt"));
	}

	private void SaveLogsToFile(string filePath)
	{
		File.WriteAllText(filePath, GetAllLogs());
		Debug.Log("Logs saved to: " + filePath);
	}

	private void CheckScreenCutout()
	{
		_ = avoidScreenCutout;
	}

	private IEnumerator ActivateCommandInputFieldCoroutine()
	{
		yield return null;
		commandInputField.ActivateInputField();
		yield return null;
		commandInputField.MoveTextEnd(shift: false);
	}

	internal void PoolLogItem(DebugLogItem logItem)
	{
		logItem.CanvasGroup.alpha = 0f;
		logItem.CanvasGroup.blocksRaycasts = false;
		pooledLogItems.Push(logItem);
	}

	internal DebugLogItem PopLogItem()
	{
		DebugLogItem newLogItem;
		if (pooledLogItems.Count > 0)
		{
			newLogItem = pooledLogItems.Pop();
			newLogItem.CanvasGroup.alpha = 1f;
			newLogItem.CanvasGroup.blocksRaycasts = true;
		}
		else
		{
			newLogItem = global::UnityEngine.Object.Instantiate(logItemPrefab, logItemsContainer, worldPositionStays: false);
			newLogItem.Initialize(recycledListView);
		}
		return newLogItem;
	}
}
