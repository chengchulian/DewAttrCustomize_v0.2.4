using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(UIWindow))]
[RequireComponent(typeof(UIAlwaysOnTop))]
public class UIModalBox : MonoBehaviour
{
	[Header("Texts")]
	[SerializeField]
	private Text m_Text1;

	[SerializeField]
	private Text m_Text2;

	[Header("Buttons")]
	[SerializeField]
	private Button m_ConfirmButton;

	[SerializeField]
	private Text m_ConfirmButtonText;

	[SerializeField]
	private Button m_CancelButton;

	[SerializeField]
	private Text m_CancelButtonText;

	[Header("Inputs")]
	[SerializeField]
	private string m_ConfirmInput = "Submit";

	[SerializeField]
	private string m_CancelInput = "Cancel";

	private UIWindow m_Window;

	private bool m_IsActive;

	[Header("Events")]
	public UnityEvent onConfirm = new UnityEvent();

	public UnityEvent onCancel = new UnityEvent();

	public bool isActive => m_IsActive;

	protected void Awake()
	{
		if (m_Window == null)
		{
			m_Window = base.gameObject.GetComponent<UIWindow>();
		}
		m_Window.ID = UIWindowID.ModalBox;
		m_Window.escapeKeyAction = UIWindow.EscapeKeyAction.None;
		m_Window.onTransitionComplete.AddListener(OnWindowTransitionEnd);
		base.gameObject.GetComponent<UIAlwaysOnTop>().order = 99996;
		if (m_ConfirmButton != null)
		{
			m_ConfirmButton.onClick.AddListener(Confirm);
		}
		if (m_CancelButton != null)
		{
			m_CancelButton.onClick.AddListener(Close);
		}
	}

	protected void Update()
	{
		if (!string.IsNullOrEmpty(m_CancelInput) && Input.GetButtonDown(m_CancelInput))
		{
			Close();
		}
		if (!string.IsNullOrEmpty(m_ConfirmInput) && Input.GetButtonDown(m_ConfirmInput))
		{
			Confirm();
		}
	}

	public void SetText1(string text)
	{
		if (m_Text1 != null)
		{
			m_Text1.text = text;
			m_Text1.gameObject.SetActive(!string.IsNullOrEmpty(text));
		}
	}

	public void SetText2(string text)
	{
		if (m_Text2 != null)
		{
			m_Text2.text = text;
			m_Text2.gameObject.SetActive(!string.IsNullOrEmpty(text));
		}
	}

	public void SetConfirmButtonText(string text)
	{
		if (m_ConfirmButtonText != null)
		{
			m_ConfirmButtonText.text = text;
		}
	}

	public void SetCancelButtonText(string text)
	{
		if (m_CancelButtonText != null)
		{
			m_CancelButtonText.text = text;
		}
	}

	public void Show()
	{
		m_IsActive = true;
		if (UIModalBoxManager.Instance != null)
		{
			UIModalBoxManager.Instance.RegisterActiveBox(this);
		}
		if (m_Window != null)
		{
			m_Window.Show();
		}
	}

	public void Close()
	{
		_Hide();
		if (onCancel != null)
		{
			onCancel.Invoke();
		}
	}

	public void Confirm()
	{
		_Hide();
		if (onConfirm != null)
		{
			onConfirm.Invoke();
		}
	}

	private void _Hide()
	{
		m_IsActive = false;
		if (UIModalBoxManager.Instance != null)
		{
			UIModalBoxManager.Instance.UnregisterActiveBox(this);
		}
		if (m_Window != null)
		{
			m_Window.Hide();
		}
	}

	public void OnWindowTransitionEnd(UIWindow window, UIWindow.VisualState state)
	{
		if (state == UIWindow.VisualState.Hidden)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
