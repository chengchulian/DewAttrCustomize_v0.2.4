using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Toggle))]
[AddComponentMenu("UI/Toggle OnOff", 58)]
public class UIToggle_OnOff : MonoBehaviour, IEventSystemHandler
{
	public enum Transition
	{
		SpriteSwap,
		Reposition
	}

	[SerializeField]
	private Image m_Target;

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private Sprite m_ActiveSprite;

	[SerializeField]
	private Vector2 m_InactivePosition = Vector2.zero;

	[SerializeField]
	private Vector2 m_ActivePosition = Vector2.zero;

	public Toggle toggle => base.gameObject.GetComponent<Toggle>();

	protected void OnEnable()
	{
		toggle.onValueChanged.AddListener(OnValueChanged);
		OnValueChanged(toggle.isOn);
	}

	protected void OnDisable()
	{
		toggle.onValueChanged.RemoveListener(OnValueChanged);
	}

	public void OnValueChanged(bool state)
	{
		if (!(m_Target == null) && base.isActiveAndEnabled)
		{
			if (m_Transition == Transition.SpriteSwap)
			{
				m_Target.overrideSprite = (state ? m_ActiveSprite : null);
			}
			else if (m_Transition == Transition.Reposition)
			{
				m_Target.rectTransform.anchoredPosition = (state ? m_ActivePosition : m_InactivePosition);
			}
		}
	}
}
