using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_InGame_WorldMap : LogicBehaviour, IGamepadFocusable, IGamepadFocusListener, IGamepadFocusableOverrideInput, IGamepadPingProxyParent
{
	public Action<int, int> onHoveringNodeChanged;

	public UI_InGame_World_NodeItem nodePrefab;

	public UI_InGame_World_Edge edgePrefab;

	public Transform nodeParent;

	public bool isMain;

	public GameObject canTravelObject;

	public int hoveringNode { get; private set; }

	public MonoBehaviour pingableTarget => GetComponentsInChildren<UI_InGame_World_NodeItem>().FirstOrDefault((UI_InGame_World_NodeItem n) => n.index == hoveringNode);

	private void Start()
	{
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnNodesChanged += (Action)delegate
		{
			if (base.isActiveAndEnabled)
			{
				RefreshNodes();
			}
		};
		if (isMain)
		{
			ManagerBase<GlobalUIManager>.instance.AddGamepadFocusable(this);
		}
	}

	private void OnDestroy()
	{
		if (isMain && ManagerBase<GlobalUIManager>.instance != null)
		{
			ManagerBase<GlobalUIManager>.instance.RemoveGamepadFocusable(this);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		RefreshNodes();
		if (canTravelObject != null)
		{
			canTravelObject.SetActive(InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.Shown);
		}
		if (isMain)
		{
			ManagerBase<GlobalUIManager>.instance.SetFocus(this);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		hoveringNode = -1;
		for (int i = nodeParent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(nodeParent.GetChild(i).gameObject);
		}
	}

	private void RefreshNodes()
	{
		for (int i = nodeParent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(nodeParent.GetChild(i).gameObject);
		}
		if (NetworkedManagerBase<ZoneManager>.instance == null || NetworkedManagerBase<ZoneManager>.instance.currentZone == null || NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex < 0)
		{
			return;
		}
		List<UI_InGame_World_NodeItem> nodes = new List<UI_InGame_World_NodeItem>();
		for (int j = 0; j < NetworkedManagerBase<ZoneManager>.instance.nodes.Count; j++)
		{
			if (!NetworkedManagerBase<ZoneManager>.instance.nodes[j].IsSidetrackNode())
			{
				UI_InGame_World_NodeItem newNode = global::UnityEngine.Object.Instantiate(nodePrefab, nodeParent);
				newNode.Setup(j, this);
				nodes.Add(newNode);
			}
		}
		for (int k = 0; k < nodes.Count; k++)
		{
			for (int l = k + 1; l < nodes.Count; l++)
			{
				if (NetworkedManagerBase<ZoneManager>.instance.IsNodeConnected(k, l))
				{
					global::UnityEngine.Object.Instantiate(edgePrefab, nodeParent).Setup(nodes[k], nodes[l], this);
				}
			}
		}
	}

	public void TravelToNode(int index)
	{
		if (DewPlayer.local == null || DewPlayer.local.hero.IsNullInactiveDeadOrKnockedOut())
		{
			return;
		}
		if (InGameUIManager.instance.isWorldDisplayed != WorldDisplayStatus.Shown)
		{
			if (NetworkedManagerBase<ZoneManager>.instance.currentNode.type == WorldNodeType.ExitBoss)
			{
				InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, "InGame_Message_CannotTravelInExitRoom");
			}
			else if (Rift_RoomExit.instance == null || !Rift_RoomExit.instance.isOpen || Rift_RoomExit.instance.isLocked)
			{
				InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, "InGame_Message_NeedsOpenExitRift");
			}
			else
			{
				InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, "InGame_Message_NeedsNearbyExitRift");
			}
		}
		else
		{
			if (index < 0 || NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex == index || !NetworkedManagerBase<ZoneManager>.instance.IsNodeConnected(NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex, index))
			{
				return;
			}
			if (InGameUIManager.instance.currentMockExit != null)
			{
				ConfirmTravelToNode(index);
				return;
			}
			NetworkedManagerBase<ZoneManager>.instance.TravelWithValidationAndConfirmation(delegate
			{
				ConfirmTravelToNode(index);
			});
		}
	}

	private void ConfirmTravelToNode(int index)
	{
		if (InGameUIManager.instance.currentMockExit != null)
		{
			InGameUIManager.instance.currentMockExit.onTravel.Invoke(index);
		}
		else
		{
			NetworkedManagerBase<ZoneManager>.instance.CmdTravelToNode(index);
		}
		InGameUIManager.instance.isWorldDisplayed = WorldDisplayStatus.None;
	}

	public void HoverNode(int index)
	{
		if (hoveringNode != index)
		{
			int prev = hoveringNode;
			hoveringNode = index;
			onHoveringNodeChanged?.Invoke(prev, index);
		}
	}

	public void UnhoverNode(int index)
	{
		if (hoveringNode == index)
		{
			hoveringNode = -1;
			onHoveringNodeChanged?.Invoke(index, -1);
		}
	}

	public void OnFocusStateChanged(bool state)
	{
		if (state)
		{
			Dew.CallDelayed(delegate
			{
				HoverNode(NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex);
			}, 2);
		}
		else
		{
			SingletonBehaviour<UI_TooltipManager>.instance.Hide();
		}
	}

	public FocusableBehavior GetBehavior()
	{
		return FocusableBehavior.Normal;
	}

	public bool CanBeFocused()
	{
		if (base.gameObject.activeSelf)
		{
			return ManagerBase<ControlManager>.instance.shouldProcessCharacterInputAllowKnockedOut;
		}
		return false;
	}

	public SelectionDisplayType GetSelectionDisplayType()
	{
		return SelectionDisplayType.Dont;
	}

	public bool OnGamepadDpadUp()
	{
		MoveSelection(Vector2.up);
		return true;
	}

	public bool OnGamepadDpadLeft()
	{
		MoveSelection(Vector2.left);
		return true;
	}

	public bool OnGamepadDpadRight()
	{
		MoveSelection(Vector2.right);
		return true;
	}

	public bool OnGamepadDpadDown()
	{
		MoveSelection(Vector2.down);
		return true;
	}

	private void MoveSelection(Vector2 direction)
	{
		if (hoveringNode == -1)
		{
			hoveringNode = NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex;
		}
		List<int> adjacents = new List<int>();
		for (int i = 0; i < NetworkedManagerBase<ZoneManager>.instance.nodes.Count; i++)
		{
			if (NetworkedManagerBase<ZoneManager>.instance.IsNodeConnected(hoveringNode, i))
			{
				adjacents.Add(i);
			}
		}
		if (!TryGetTarget(45f, out var bestIndex))
		{
			TryGetTarget(90f, out bestIndex);
		}
		if (bestIndex >= 0)
		{
			int prev = hoveringNode;
			hoveringNode = bestIndex;
			onHoveringNodeChanged?.Invoke(prev, hoveringNode);
		}
		bool TryGetTarget(float angleLimit, out int nodeIndex)
		{
			List<float> scores = new List<float>();
			for (int j = 0; j < adjacents.Count; j++)
			{
				Vector2 from = NetworkedManagerBase<ZoneManager>.instance.nodes[hoveringNode].position;
				Vector2 to = NetworkedManagerBase<ZoneManager>.instance.nodes[adjacents[j]].position;
				if (Vector2.Angle(to - from, direction) < angleLimit + 0.1f)
				{
					scores.Add(1000f / Vector2.Distance(from, to));
				}
				else
				{
					scores.Add(-1000f);
				}
			}
			nodeIndex = -1;
			float bestScore = float.NegativeInfinity;
			for (int k = 0; k < adjacents.Count; k++)
			{
				if (!(scores[k] < 0f) && !(bestScore > scores[k]))
				{
					bestScore = scores[k];
					nodeIndex = adjacents[k];
				}
			}
			return nodeIndex >= 0;
		}
	}

	public bool OnGamepadConfirm()
	{
		TravelToNode(hoveringNode);
		return true;
	}

	public bool OnGamepadBack()
	{
		InGameUIManager.instance.isWorldDisplayed = WorldDisplayStatus.None;
		SingletonBehaviour<UI_TooltipManager>.instance.Hide();
		return true;
	}
}
