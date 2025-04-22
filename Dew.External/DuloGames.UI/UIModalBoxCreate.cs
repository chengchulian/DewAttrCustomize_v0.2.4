using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Modal Box Create", 8)]
[DisallowMultipleComponent]
public class UIModalBoxCreate : MonoBehaviour
{
	[SerializeField]
	private string m_Text1;

	[SerializeField]
	[TextArea]
	private string m_Text2;

	[SerializeField]
	private string m_ConfirmText;

	[SerializeField]
	private string m_CancelText;

	[SerializeField]
	private Button m_HookToButton;

	[Header("Events")]
	public UnityEvent onConfirm = new UnityEvent();

	public UnityEvent onCancel = new UnityEvent();

	protected void OnEnable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.AddListener(CreateAndShow);
		}
	}

	protected void OnDisable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.RemoveListener(CreateAndShow);
		}
	}

	public void CreateAndShow()
	{
		if (UIModalBoxManager.Instance == null)
		{
			Debug.LogWarning("Could not load the modal box manager while creating a modal box.");
			return;
		}
		UIModalBox box = UIModalBoxManager.Instance.Create(base.gameObject);
		if (box != null)
		{
			box.SetText1(m_Text1);
			box.SetText2(m_Text2);
			box.SetConfirmButtonText(m_ConfirmText);
			box.SetCancelButtonText(m_CancelText);
			box.onConfirm.AddListener(OnConfirm);
			box.onCancel.AddListener(OnCancel);
			box.Show();
		}
	}

	public void OnConfirm()
	{
		if (onConfirm != null)
		{
			onConfirm.Invoke();
		}
	}

	public void OnCancel()
	{
		if (onCancel != null)
		{
			onCancel.Invoke();
		}
	}
}
