using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomModifiers : RoomComponent
{
	[NonSerialized]
	public List<RoomModifierBase> modifierInstances = new List<RoomModifierBase>();

	private WorldNodeSaveData _currentSave;

	public override void OnRoomStartServer(WorldNodeSaveData save)
	{
		base.OnRoomStartServer(save);
		List<ModifierData> mods = NetworkedManagerBase<ZoneManager>.instance.currentNode.modifiers;
		for (int i = 0; i < mods.Count; i++)
		{
			HandleRuntimeAddition(mods[i].id);
		}
	}

	internal void HandleRuntimeRemoval(int id)
	{
		int index = modifierInstances.FindIndex((RoomModifierBase m) => m.id == id);
		if (index < 0)
		{
			Debug.Log($"Runtime removal failed; Modifier instance with id {id} not found");
			return;
		}
		if (!modifierInstances[index].IsNullOrInactive())
		{
			modifierInstances[index].Destroy();
		}
		modifierInstances.RemoveAt(index);
	}

	internal void HandleRuntimeAddition(int id)
	{
		List<ModifierData> mods = NetworkedManagerBase<ZoneManager>.instance.currentNode.modifiers;
		int index = NetworkedManagerBase<ZoneManager>.instance.currentNode.modifiers.FindIndex((ModifierData m) => m.id == id);
		if (index < 0)
		{
			Debug.Log($"Runtime addition failed; Modifier instance with id {id} not found");
			return;
		}
		ModifierData m2 = mods[index];
		RoomModifierBase byShortTypeName = DewResources.GetByShortTypeName<RoomModifierBase>(m2.type);
		bool isNewInstance = !NetworkedManagerBase<ZoneManager>.instance.modifierServerData[id].ContainsKey("didCreateInstance");
		RoomModifierBase newMod = Dew.CreateActor(byShortTypeName, Vector3.zero, null, null, delegate(RoomModifierBase mod)
		{
			mod.isNewInstance = isNewInstance;
			mod.id = id;
		});
		modifierInstances.Add(newMod);
		if (isNewInstance)
		{
			NetworkedManagerBase<ZoneManager>.instance.modifierServerData[id].Add("didCreateInstance", true);
		}
		Debug.Log("Added modifier of type " + m2.type);
	}

	private void MirrorProcessed()
	{
	}
}
