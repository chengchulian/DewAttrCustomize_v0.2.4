using UnityEngine;

public class Forest_LoopCat_SpawnPosition : MonoBehaviour, IPlayerPathableArea, IBanRoomNodesNearby
{
	Vector3 IPlayerPathableArea.pathablePosition => base.transform.position;
}
