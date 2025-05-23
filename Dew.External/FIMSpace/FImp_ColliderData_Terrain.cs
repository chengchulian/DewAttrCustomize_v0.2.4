using UnityEngine;

namespace FIMSpace;

public class FImp_ColliderData_Terrain : FImp_ColliderData_Base
{
	public TerrainCollider TerrCollider { get; private set; }

	public Terrain TerrainComponent { get; private set; }

	public FImp_ColliderData_Terrain(TerrainCollider collider)
	{
		base.Collider = collider;
		base.Transform = collider.transform;
		TerrCollider = collider;
		base.ColliderType = EFColliderType.Terrain;
		TerrainComponent = collider.GetComponent<Terrain>();
	}

	public override bool PushIfInside(ref Vector3 segmentPosition, float segmentRadius, Vector3 segmentOffset)
	{
		if (segmentPosition.x + segmentRadius < TerrainComponent.GetPosition().x - segmentRadius || segmentPosition.x > TerrainComponent.GetPosition().x + TerrainComponent.terrainData.size.x || segmentPosition.z + segmentRadius < TerrainComponent.GetPosition().z - segmentRadius || segmentPosition.z > TerrainComponent.GetPosition().z + TerrainComponent.terrainData.size.z)
		{
			return false;
		}
		Vector3 offsettedPosition = segmentPosition + segmentOffset;
		Vector3 terrPoint = offsettedPosition;
		terrPoint.y = TerrCollider.transform.position.y + TerrainComponent.SampleHeight(offsettedPosition);
		float magnitude = (offsettedPosition - terrPoint).magnitude;
		float underMul = 1f;
		if (offsettedPosition.y < terrPoint.y)
		{
			underMul = 4f;
		}
		else if (offsettedPosition.y + segmentRadius * 2f < terrPoint.y)
		{
			underMul = 8f;
		}
		if (magnitude < segmentRadius * underMul)
		{
			Vector3 toNormal = terrPoint - offsettedPosition;
			Vector3 pushNormal = ((!(underMul > 1f)) ? (toNormal - toNormal.normalized * segmentRadius) : (toNormal + toNormal.normalized * segmentRadius));
			segmentPosition += pushNormal;
			return true;
		}
		return false;
	}

	public static void PushOutFromTerrain(TerrainCollider terrainCollider, float segmentRadius, ref Vector3 point)
	{
		Terrain terrain = terrainCollider.GetComponent<Terrain>();
		Vector3 rayOrigin = point;
		rayOrigin.y = terrainCollider.transform.position.y + terrain.SampleHeight(point) + segmentRadius;
		Ray ray = new Ray(rayOrigin, Vector3.down);
		if (terrainCollider.Raycast(ray, out var hit, segmentRadius * 2f))
		{
			float magnitude = (point - hit.point).magnitude;
			float underMul = 1f;
			if (hit.point.y > point.y + segmentRadius * 0.9f)
			{
				underMul = 8f;
			}
			else if (hit.point.y > point.y)
			{
				underMul = 4f;
			}
			if (magnitude < segmentRadius * underMul)
			{
				Vector3 toNormal = hit.point - point;
				Vector3 pushNormal = ((!(underMul > 1f)) ? (toNormal - toNormal.normalized * segmentRadius) : (toNormal + toNormal.normalized * segmentRadius));
				point += pushNormal;
			}
		}
	}
}
