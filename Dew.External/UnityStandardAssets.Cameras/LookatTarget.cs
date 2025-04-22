using UnityEngine;

namespace UnityStandardAssets.Cameras;

public class LookatTarget : AbstractTargetFollower
{
	[SerializeField]
	private Vector2 m_RotationRange;

	[SerializeField]
	private float m_FollowSpeed = 1f;

	private Vector3 m_FollowAngles;

	private Quaternion m_OriginalRotation;

	protected Vector3 m_FollowVelocity;

	protected override void Start()
	{
		base.Start();
		m_OriginalRotation = base.transform.localRotation;
	}

	protected override void FollowTarget(float deltaTime)
	{
		base.transform.localRotation = m_OriginalRotation;
		Vector3 localTarget = base.transform.InverseTransformPoint(m_Target.position);
		float yAngle = Mathf.Atan2(localTarget.x, localTarget.z) * 57.29578f;
		yAngle = Mathf.Clamp(yAngle, (0f - m_RotationRange.y) * 0.5f, m_RotationRange.y * 0.5f);
		base.transform.localRotation = m_OriginalRotation * Quaternion.Euler(0f, yAngle, 0f);
		localTarget = base.transform.InverseTransformPoint(m_Target.position);
		float xAngle = Mathf.Atan2(localTarget.y, localTarget.z) * 57.29578f;
		xAngle = Mathf.Clamp(xAngle, (0f - m_RotationRange.x) * 0.5f, m_RotationRange.x * 0.5f);
		m_FollowAngles = Vector3.SmoothDamp(target: new Vector3(m_FollowAngles.x + Mathf.DeltaAngle(m_FollowAngles.x, xAngle), m_FollowAngles.y + Mathf.DeltaAngle(m_FollowAngles.y, yAngle)), current: m_FollowAngles, currentVelocity: ref m_FollowVelocity, smoothTime: m_FollowSpeed);
		base.transform.localRotation = m_OriginalRotation * Quaternion.Euler(0f - m_FollowAngles.x, m_FollowAngles.y, 0f);
	}
}
