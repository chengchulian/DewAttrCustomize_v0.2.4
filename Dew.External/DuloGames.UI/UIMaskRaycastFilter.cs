using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Raycast Filters/Mask Raycast Filter")]
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class UIMaskRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
	private Image m_Image;

	private Sprite m_Sprite;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_AlphaTreshold = 0.01f;

	protected void Awake()
	{
		m_Image = base.gameObject.GetComponent<Image>();
		if (m_Image != null)
		{
			m_Sprite = m_Image.sprite;
		}
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (m_Image == null || m_Sprite == null)
		{
			return false;
		}
		RectTransform rectTransform = (RectTransform)base.transform;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out var localPositionPivotRelative);
		Vector2 localPosition = new Vector2(localPositionPivotRelative.x + rectTransform.pivot.x * rectTransform.rect.width, localPositionPivotRelative.y + rectTransform.pivot.y * rectTransform.rect.height);
		Rect spriteRect = m_Sprite.textureRect;
		Rect maskRect = rectTransform.rect;
		int x = 0;
		int y = 0;
		Image.Type type = m_Image.type;
		if (type != 0 && type == Image.Type.Sliced)
		{
			Vector4 border = m_Sprite.border;
			x = ((localPosition.x < border.x) ? Mathf.FloorToInt(spriteRect.x + localPosition.x) : ((!(localPosition.x > maskRect.width - border.z)) ? Mathf.FloorToInt(spriteRect.x + border.x + (localPosition.x - border.x) / (maskRect.width - border.x - border.z) * (spriteRect.width - border.x - border.z)) : Mathf.FloorToInt(spriteRect.x + spriteRect.width - (maskRect.width - localPosition.x))));
			y = ((localPosition.y < border.y) ? Mathf.FloorToInt(spriteRect.y + localPosition.y) : ((!(localPosition.y > maskRect.height - border.w)) ? Mathf.FloorToInt(spriteRect.y + border.y + (localPosition.y - border.y) / (maskRect.height - border.y - border.w) * (spriteRect.height - border.y - border.w)) : Mathf.FloorToInt(spriteRect.y + spriteRect.height - (maskRect.height - localPosition.y))));
		}
		else
		{
			x = Mathf.FloorToInt(spriteRect.x + spriteRect.width * localPosition.x / maskRect.width);
			y = Mathf.FloorToInt(spriteRect.y + spriteRect.height * localPosition.y / maskRect.height);
		}
		try
		{
			return m_Sprite.texture.GetPixel(x, y).a > m_AlphaTreshold;
		}
		catch (UnityException message)
		{
			Debug.LogError(message);
			Object.Destroy(this);
			return false;
		}
	}
}
