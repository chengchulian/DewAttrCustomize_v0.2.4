using UnityEngine;

namespace DuloGames.UI;

[AddComponentMenu("UI/Raycast Filters/Ignore Raycast Filter")]
public class UIIgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
{
	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		return false;
	}
}
