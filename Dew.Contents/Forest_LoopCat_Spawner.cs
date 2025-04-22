using System.Collections;

using UnityEngine;

public class Forest_LoopCat_Spawner : Actor
{
    public float appearChance;

    public bool enableForceSpawn;

    protected override void OnCreate()
    {
        base.OnCreate();
        if (base.isServer && (enableForceSpawn || (NetworkedManagerBase<ZoneManager>.instance.loopIndex >= 1 && !SingletonDewNetworkBehaviour<Room>.instance.isRevisit && !(Random.value > appearChance))))
        {
            Forest_LoopCat_SpawnPosition[] componentsInChildren = GetComponentsInChildren<Forest_LoopCat_SpawnPosition>();
            Vector3 vector = componentsInChildren[Random.Range(0, componentsInChildren.Length)].transform.position;
            Vector3 forward = Rift_RoomExit.instance.transform.position - vector;
            CreateActor<Shrine_LoopCat>(vector, Quaternion.LookRotation(forward));
        }
        StartCoroutine(ExecuteZoneOnceCoroutine());
    }

    IEnumerator ExecuteZoneOnceCoroutine()
    {
        while (!SingletonDewNetworkBehaviour<Room>.instance.isActive)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (!NetworkedManagerBase<ZoneManager>.instance.currentRoom.isRevisit)
        {
            AttrCustomizeManager.ExecuteZoneOnce();
        }


    }

    private void MirrorProcessed()
    {
    }
}