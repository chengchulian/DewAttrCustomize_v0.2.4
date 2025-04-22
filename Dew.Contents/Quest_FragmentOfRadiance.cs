using UnityEngine;

public class Quest_FragmentOfRadiance : DewQuest
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			SetNextGoal_ReachNode(new NextGoalSettings
			{
				nodeIndexSettings = new GetNodeIndexSettings
				{
					desiredDistance = new Vector2Int(2, 4),
					preferCloserToExit = true,
					preferNoMainModifier = true
				},
				addedModifier = "RoomMod_FragmentOfRadiance_MysteriousPlace",
				roomOverride = "Room_Special_FragmentOfRadiance_MysteriousPlace",
				revertModifierOnRemove = false,
				revertRoomOnRemove = false,
				onReachDestination = base.CompleteQuest,
				failQuestOnFail = true
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
