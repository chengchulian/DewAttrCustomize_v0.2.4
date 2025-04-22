using DG.Tweening;
using UnityEngine;

public class UI_InGame_QuickText : MonoBehaviour, IWorldPositionReceiver
{
	public float punch = 0.2f;

	public float punchDuration = 0.4f;

	public float sustainDuration = 0.5f;

	public float disappearDuration = 0.3f;

	public Vector3? worldPosition;

	private void Start()
	{
		DOTween.Sequence().Append(base.transform.DOPunchScale(Vector3.one * punch, punchDuration)).AppendInterval(sustainDuration)
			.Append(base.transform.DOScale(Vector3.zero, disappearDuration))
			.AppendCallback(delegate
			{
				Object.Destroy(base.gameObject);
			})
			.SetUpdate(isIndependentUpdate: true);
		if (worldPosition.HasValue)
		{
			UpdatePosition();
		}
	}

	private void LateUpdate()
	{
		if (worldPosition.HasValue)
		{
			UpdatePosition();
		}
	}

	private void UpdatePosition()
	{
		if (worldPosition.HasValue)
		{
			base.transform.position = Dew.mainCamera.WorldToScreenPoint(worldPosition.Value);
		}
	}

	public void SetWorldPosition(Vector3 worldPos)
	{
		worldPosition = worldPos;
	}
}
