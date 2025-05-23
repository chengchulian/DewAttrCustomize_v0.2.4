using UnityEngine;

[DefaultExecutionOrder(0)]
public class Target : MonoBehaviour
{
	[Tooltip("Change this color to change the indicators color for this target")]
	[SerializeField]
	private Color targetColor = Color.red;

	[Tooltip("Select if box indicator is required for this target")]
	[SerializeField]
	private bool needBoxIndicator = true;

	[Tooltip("Select if arrow indicator is required for this target")]
	[SerializeField]
	private bool needArrowIndicator = true;

	[Tooltip("Select if distance text is required for this target")]
	[SerializeField]
	private bool needDistanceText = true;

	[HideInInspector]
	public Indicator indicator;

	public Color TargetColor => targetColor;

	public bool NeedBoxIndicator => needBoxIndicator;

	public bool NeedArrowIndicator => needArrowIndicator;

	public bool NeedDistanceText => needDistanceText;

	private void OnEnable()
	{
		if (OffScreenIndicator.TargetStateChanged != null)
		{
			OffScreenIndicator.TargetStateChanged(this, arg2: true);
		}
	}

	private void OnDisable()
	{
		if (OffScreenIndicator.TargetStateChanged != null)
		{
			OffScreenIndicator.TargetStateChanged(this, arg2: false);
		}
	}

	public float GetDistanceFromCamera(Vector3 cameraPosition)
	{
		return Vector3.Distance(cameraPosition, base.transform.position);
	}
}
