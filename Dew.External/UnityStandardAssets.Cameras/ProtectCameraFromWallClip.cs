using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Cameras;

public class ProtectCameraFromWallClip : MonoBehaviour
{
	public class RayHitComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}

	public float clipMoveTime = 0.05f;

	public float returnTime = 0.4f;

	public float sphereCastRadius = 0.1f;

	public bool visualiseInEditor;

	public float closestDistance = 0.5f;

	public string dontClipTag = "Player";

	private Transform m_Cam;

	private Transform m_Pivot;

	private float m_OriginalDist;

	private float m_MoveVelocity;

	private float m_CurrentDist;

	private Ray m_Ray;

	private RaycastHit[] m_Hits;

	private RayHitComparer m_RayHitComparer;

	public bool protecting { get; private set; }

	private void Start()
	{
		m_Cam = GetComponentInChildren<Camera>().transform;
		m_Pivot = m_Cam.parent;
		m_OriginalDist = m_Cam.localPosition.magnitude;
		m_CurrentDist = m_OriginalDist;
		m_RayHitComparer = new RayHitComparer();
	}

	private void LateUpdate()
	{
		float targetDist = m_OriginalDist;
		m_Ray.origin = m_Pivot.position + m_Pivot.forward * sphereCastRadius;
		m_Ray.direction = -m_Pivot.forward;
		Collider[] cols = Physics.OverlapSphere(m_Ray.origin, sphereCastRadius);
		bool initialIntersect = false;
		bool hitSomething = false;
		for (int i = 0; i < cols.Length; i++)
		{
			if (!cols[i].isTrigger && (!(cols[i].attachedRigidbody != null) || !cols[i].attachedRigidbody.CompareTag(dontClipTag)))
			{
				initialIntersect = true;
				break;
			}
		}
		if (initialIntersect)
		{
			m_Ray.origin += m_Pivot.forward * sphereCastRadius;
			m_Hits = Physics.RaycastAll(m_Ray, m_OriginalDist - sphereCastRadius);
		}
		else
		{
			m_Hits = Physics.SphereCastAll(m_Ray, sphereCastRadius, m_OriginalDist + sphereCastRadius);
		}
		Array.Sort(m_Hits, m_RayHitComparer);
		float nearest = float.PositiveInfinity;
		for (int j = 0; j < m_Hits.Length; j++)
		{
			if (m_Hits[j].distance < nearest && !m_Hits[j].collider.isTrigger && (!(m_Hits[j].collider.attachedRigidbody != null) || !m_Hits[j].collider.attachedRigidbody.CompareTag(dontClipTag)))
			{
				nearest = m_Hits[j].distance;
				targetDist = 0f - m_Pivot.InverseTransformPoint(m_Hits[j].point).z;
				hitSomething = true;
			}
		}
		if (hitSomething)
		{
			Debug.DrawRay(m_Ray.origin, -m_Pivot.forward * (targetDist + sphereCastRadius), Color.red);
		}
		protecting = hitSomething;
		m_CurrentDist = Mathf.SmoothDamp(m_CurrentDist, targetDist, ref m_MoveVelocity, (m_CurrentDist > targetDist) ? clipMoveTime : returnTime);
		m_CurrentDist = Mathf.Clamp(m_CurrentDist, closestDistance, m_OriginalDist);
		m_Cam.localPosition = -Vector3.forward * m_CurrentDist;
	}
}
