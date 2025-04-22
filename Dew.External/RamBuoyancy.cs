using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RamBuoyancy : MonoBehaviour
{
	public float buoyancy = 30f;

	public float viscosity = 2f;

	public float viscosityAngular = 0.4f;

	public LayerMask layer = 16;

	public Collider collider;

	[Range(2f, 10f)]
	public int pointsInAxis = 2;

	private Rigidbody rigidbody;

	private static RamSpline[] ramSplines;

	private static LakePolygon[] lakePolygons;

	public List<Vector3> volumePoints = new List<Vector3>();

	public bool autoGenerateVolumePoints = true;

	private Vector3[] volumePointsMatrix;

	private Vector3 lowestPoint;

	private Vector3 center = Vector3.zero;

	public bool debug;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		if (ramSplines == null)
		{
			ramSplines = Object.FindObjectsOfType<RamSpline>();
		}
		if (lakePolygons == null)
		{
			lakePolygons = Object.FindObjectsOfType<LakePolygon>();
		}
		if (collider == null)
		{
			collider = GetComponent<Collider>();
		}
		if (collider == null)
		{
			Debug.LogError("Buoyancy doesn't have collider");
			base.enabled = false;
			return;
		}
		if (autoGenerateVolumePoints)
		{
			Vector3 size = collider.bounds.size;
			Vector3 min = collider.bounds.min;
			Vector3 step = new Vector3(size.x / (float)pointsInAxis, size.y / (float)pointsInAxis, size.z / (float)pointsInAxis);
			for (int x = 0; x <= pointsInAxis; x++)
			{
				for (int y = 0; y <= pointsInAxis; y++)
				{
					for (int z = 0; z <= pointsInAxis; z++)
					{
						Vector3 vertice = new Vector3(min.x + (float)x * step.x, min.y + (float)y * step.y, min.z + (float)z * step.z);
						if (Vector3.Distance(collider.ClosestPoint(vertice), vertice) < float.Epsilon)
						{
							volumePoints.Add(base.transform.InverseTransformPoint(vertice));
						}
					}
				}
			}
		}
		volumePointsMatrix = new Vector3[volumePoints.Count];
	}

	private void FixedUpdate()
	{
		WaterPhysics();
	}

	public void WaterPhysics()
	{
		if (volumePoints.Count == 0)
		{
			Debug.Log("Not initiated Buoyancy");
			return;
		}
		Ray ray = default(Ray);
		ray.direction = Vector3.up;
		bool backFace = Physics.queriesHitBackfaces;
		Physics.queriesHitBackfaces = true;
		Matrix4x4 thisMatrix = base.transform.localToWorldMatrix;
		lowestPoint = volumePoints[0];
		float minY = float.MaxValue;
		for (int i = 0; i < volumePoints.Count; i++)
		{
			volumePointsMatrix[i] = thisMatrix.MultiplyPoint3x4(volumePoints[i]);
			if (minY > volumePointsMatrix[i].y)
			{
				lowestPoint = volumePointsMatrix[i];
				minY = lowestPoint.y;
			}
		}
		ray.origin = lowestPoint;
		center = Vector3.zero;
		if (Physics.Raycast(ray, out var hit, 100f, layer))
		{
			Mathf.Max(collider.bounds.size.x, collider.bounds.size.z);
			int verticesCount = 0;
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityDirection = velocity.normalized;
			minY = hit.point.y;
			for (int j = 0; j < volumePointsMatrix.Length; j++)
			{
				if (volumePointsMatrix[j].y <= minY)
				{
					center += volumePointsMatrix[j];
					verticesCount++;
				}
			}
			center /= (float)verticesCount;
			rigidbody.AddForceAtPosition(Vector3.up * buoyancy * (minY - center.y), center);
			rigidbody.AddForce(velocity * -1f * viscosity);
			if (velocity.magnitude > 0.01f)
			{
				Vector3 v1 = Vector3.Cross(velocity, new Vector3(1f, 1f, 1f)).normalized;
				_ = Vector3.Cross(velocity, v1).normalized;
				Vector3 pointFront = velocity.normalized * 10f;
				Vector3[] array = volumePointsMatrix;
				foreach (Vector3 item in array)
				{
					Vector3 start = pointFront + item;
					Ray rayCollider = new Ray(start, -velocityDirection);
					if (collider.Raycast(rayCollider, out var hitCollider, 50f))
					{
						Vector3 pointVelocity = rigidbody.GetPointVelocity(hitCollider.point);
						rigidbody.AddForceAtPosition(-pointVelocity * viscosityAngular, hitCollider.point);
						if (debug)
						{
							Debug.DrawRay(hitCollider.point, -pointVelocity * viscosityAngular, Color.red, 0.1f);
						}
					}
				}
			}
			RamSpline ramSpline = hit.collider.GetComponent<RamSpline>();
			LakePolygon lakePolygon = hit.collider.GetComponent<LakePolygon>();
			if (ramSpline != null)
			{
				Mesh sharedMesh = ramSpline.meshfilter.sharedMesh;
				int verticeId1 = sharedMesh.triangles[hit.triangleIndex * 3];
				Vector3 verticeDirection = ramSpline.verticeDirection[verticeId1];
				Vector2 uv4 = sharedMesh.uv4[verticeId1];
				verticeDirection = verticeDirection * uv4.y - new Vector3(verticeDirection.z, verticeDirection.y, 0f - verticeDirection.x) * uv4.x;
				rigidbody.AddForce(new Vector3(verticeDirection.x, 0f, verticeDirection.z) * ramSpline.floatSpeed);
				if (debug)
				{
					Debug.DrawRay(center, Vector3.up * buoyancy * (minY - center.y) * 5f, Color.blue);
				}
				if (debug)
				{
					Debug.DrawRay(base.transform.position, velocity * -1f * viscosity * 5f, Color.magenta);
				}
				if (debug)
				{
					Debug.DrawRay(base.transform.position, velocity * 5f, Color.grey);
				}
				if (debug)
				{
					Debug.DrawRay(base.transform.position, rigidbody.angularVelocity * 5f, Color.black);
				}
			}
			if (lakePolygon != null)
			{
				Mesh sharedMesh2 = lakePolygon.meshfilter.sharedMesh;
				int verticeId2 = sharedMesh2.triangles[hit.triangleIndex * 3];
				Vector2 uv5 = -sharedMesh2.uv4[verticeId2];
				Vector3 verticeDirection2 = new Vector3(uv5.x, 0f, uv5.y);
				rigidbody.AddForce(new Vector3(verticeDirection2.x, 0f, verticeDirection2.z) * lakePolygon.floatSpeed);
				float singleStep = 1f * Time.deltaTime;
				Vector3 newDirection = Vector3.RotateTowards(base.transform.forward, verticeDirection2, singleStep, 0f);
				base.transform.rotation = Quaternion.LookRotation(newDirection);
				if (debug)
				{
					Debug.DrawRay(base.transform.position + Vector3.up, verticeDirection2 * 5f, Color.red);
				}
				if (debug)
				{
					Debug.DrawRay(center, Vector3.up * buoyancy * (minY - center.y) * 5f, Color.blue);
				}
				if (debug)
				{
					Debug.DrawRay(base.transform.position, velocity * -1f * viscosity * 5f, Color.magenta);
				}
				if (debug)
				{
					Debug.DrawRay(base.transform.position, velocity * 5f, Color.grey);
				}
				if (debug)
				{
					Debug.DrawRay(base.transform.position, rigidbody.angularVelocity * 5f, Color.black);
				}
			}
		}
		Physics.queriesHitBackfaces = backFace;
	}

	private void OnDrawGizmosSelected()
	{
		if (!debug)
		{
			return;
		}
		if (collider != null && volumePointsMatrix != null)
		{
			_ = base.transform.localToWorldMatrix;
			Vector3[] array = volumePointsMatrix;
			foreach (Vector3 vector in array)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(vector, 0.08f);
			}
		}
		_ = lowestPoint;
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(lowestPoint, 0.08f);
		_ = center;
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(center, 0.08f);
	}
}
