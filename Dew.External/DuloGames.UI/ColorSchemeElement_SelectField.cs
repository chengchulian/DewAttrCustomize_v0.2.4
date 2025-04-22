using UnityEngine;

namespace DuloGames.UI;

[ExecuteInEditMode]
[AddComponentMenu("UI/Color Scheme Element - Select Field", 48)]
public class ColorSchemeElement_SelectField : MonoBehaviour, IColorSchemeElement
{
	public enum ElementType
	{
		List,
		Separator
	}

	[SerializeField]
	private ElementType m_ElementType;

	[SerializeField]
	private ColorSchemeShade m_Shade;

	public ColorSchemeShade shade
	{
		get
		{
			return m_Shade;
		}
		set
		{
			m_Shade = value;
		}
	}

	protected void Awake()
	{
		if (ColorSchemeManager.Instance != null && ColorSchemeManager.Instance.activeColorScheme != null)
		{
			ColorSchemeManager.Instance.activeColorScheme.ApplyToElement(this);
		}
	}

	public void Apply(Color newColor)
	{
		UISelectField select = base.gameObject.GetComponent<UISelectField>();
		if (!(select == null))
		{
			switch (m_ElementType)
			{
			case ElementType.List:
				select.listBackgroundColor = newColor;
				break;
			case ElementType.Separator:
				select.listSeparatorColor = newColor;
				break;
			}
		}
	}
}
