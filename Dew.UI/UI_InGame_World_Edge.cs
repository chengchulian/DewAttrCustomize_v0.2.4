using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class UI_InGame_World_Edge : MonoBehaviour
{
	public UILineRenderer lineRenderer;

	public Material matNormal;

	public Material matHover;

	public Material matAdjacentCanMove;

	public Material matAdjacentCantMove;

	public Material matTravel;

	private UI_InGame_World_NodeItem _a;

	private UI_InGame_World_NodeItem _b;

	private UI_InGame_WorldMap _parent;

	private Vector2[] _points = new Vector2[2];

	public void Setup(UI_InGame_World_NodeItem a, UI_InGame_World_NodeItem b, UI_InGame_WorldMap parent)
	{
		RectTransform obj = (RectTransform)base.transform;
		obj.anchorMin = Vector2.zero;
		obj.anchorMax = Vector2.zero;
		obj.anchoredPosition = Vector2.zero;
		base.transform.SetSiblingIndex(0);
		_a = a;
		_b = b;
		_parent = parent;
		UI_InGame_WorldMap parent2 = _parent;
		parent2.onHoveringNodeChanged = (Action<int, int>)Delegate.Combine(parent2.onHoveringNodeChanged, new Action<int, int>(UpdateStatus));
		UpdateStatus(0, 0);
		lineRenderer.Points = _points;
		UpdatePosition();
	}

	private void OnDestroy()
	{
		UI_InGame_WorldMap parent = _parent;
		parent.onHoveringNodeChanged = (Action<int, int>)Delegate.Remove(parent.onHoveringNodeChanged, new Action<int, int>(UpdateStatus));
	}

	private void Update()
	{
		UpdatePosition();
	}

	public void UpdateStatus(int arg1, int arg2)
	{
		if (this == null)
		{
			return;
		}
		bool isHovering = _parent.hoveringNode == _a.index || _parent.hoveringNode == _b.index;
		bool isAdjacent = _a.index == NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex || _b.index == NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex;
		bool canMove = InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.Shown;
		if (isAdjacent && NetworkedManagerBase<ZoneManager>.instance.voteType == VoteType.NextNode && (_a.index == NetworkedManagerBase<ZoneManager>.instance.voteData || _b.index == NetworkedManagerBase<ZoneManager>.instance.voteData))
		{
			lineRenderer.material = matTravel;
			if (_a.index == NetworkedManagerBase<ZoneManager>.instance.voteData)
			{
				UI_InGame_World_NodeItem b = _b;
				UI_InGame_World_NodeItem a = _a;
				_a = b;
				_b = a;
			}
		}
		else if (isAdjacent)
		{
			lineRenderer.material = (canMove ? matAdjacentCanMove : matAdjacentCantMove);
		}
		else if (isHovering)
		{
			lineRenderer.material = matHover;
		}
		else
		{
			lineRenderer.material = matNormal;
		}
	}

	private void UpdatePosition()
	{
		_points[0] = base.transform.InverseTransformPoint(_a.transform.position);
		_points[1] = base.transform.InverseTransformPoint(_b.transform.position);
		lineRenderer.SetVerticesDirty();
	}
}
