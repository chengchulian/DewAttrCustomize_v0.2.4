using UnityEngine;

namespace PixelPlay.OffScreenIndicator;

public class OffScreenIndicatorCore
{
	public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
	{
		return mainCamera.WorldToScreenPoint(targetPosition);
	}

	public static bool IsTargetVisible(Vector3 screenPosition)
	{
		if (screenPosition.z > 0f && screenPosition.x > 0f && screenPosition.x < (float)Screen.width && screenPosition.y > 0f)
		{
			return screenPosition.y < (float)Screen.height;
		}
		return false;
	}

	public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
	{
		screenPosition -= screenCentre;
		if (screenPosition.z < 0f)
		{
			screenPosition *= -1f;
		}
		angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
		float slope = Mathf.Tan(angle);
		if (screenPosition.x > 0f)
		{
			screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0f);
		}
		else
		{
			screenPosition = new Vector3(0f - screenBounds.x, (0f - screenBounds.x) * slope, 0f);
		}
		if (screenPosition.y > screenBounds.y)
		{
			screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0f);
		}
		else if (screenPosition.y < 0f - screenBounds.y)
		{
			screenPosition = new Vector3((0f - screenBounds.y) / slope, 0f - screenBounds.y, 0f);
		}
		screenPosition += screenCentre;
	}
}
