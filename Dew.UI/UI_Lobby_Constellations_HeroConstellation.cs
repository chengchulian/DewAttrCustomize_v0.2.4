using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby_Constellations_HeroConstellation : SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>
{
	public UI_Lobby_Constellations_HeroConstellation_StarSlot slotPrefab;

	public UI_Lobby_Constellations_HeroConstellation_StarEdge edgePrefab;

	public Transform alphaStar;

	public Transform starPlacements;

	private List<UI_Lobby_Constellations_HeroConstellation_StarSlot> _slots = new List<UI_Lobby_Constellations_HeroConstellation_StarSlot>();

	private List<UI_Lobby_Constellations_HeroConstellation_StarEdge> _edges = new List<UI_Lobby_Constellations_HeroConstellation_StarEdge>();

	private UI_Lobby_Constellations_HeroConstellation_StarSlot _hoveredStarSlot;

	public SafeAction onHoveredStarSlotChanged;

	public UI_Lobby_Constellations_HeroConstellation_StarSlot hoveredStarSlot
	{
		get
		{
			return _hoveredStarSlot;
		}
		set
		{
			if (!(_hoveredStarSlot == value))
			{
				_hoveredStarSlot = value;
				onHoveredStarSlotChanged?.Invoke();
			}
		}
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void FixedUpdate()
	{
	}

	private void Refresh()
	{
		if (DewPlayer.local == null)
		{
			return;
		}
		foreach (UI_Lobby_Constellations_HeroConstellation_StarSlot slot in _slots)
		{
			global::UnityEngine.Object.Destroy(slot.gameObject);
		}
		_slots.Clear();
		foreach (UI_Lobby_Constellations_HeroConstellation_StarEdge edge in _edges)
		{
			global::UnityEngine.Object.Destroy(edge.gameObject);
		}
		_edges.Clear();
		Hero hero = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType);
		Place(StarType.Destruction, 0f);
		Place(StarType.Life, 90f);
		Place(StarType.Imagination, 180f);
		Place(StarType.Flexible, 270f);
		void Place(StarType type, float addedAngle)
		{
			HeroConstellationSettings s = hero.GetConstellationSettings(type);
			int startIndex = _slots.Count;
			int count = s.maxCount;
			float angle = hero.cDisplayBaseAngle + s.angleOffset + addedAngle;
			UI_Lobby_Constellations_StarPlacementGuide guide = starPlacements.Find(count.ToString()).GetComponent<UI_Lobby_Constellations_StarPlacementGuide>();
			for (int i = 0; i < guide.transform.childCount; i++)
			{
				Transform child = guide.transform.GetChild(i);
				UI_Lobby_Constellations_HeroConstellation_StarSlot newSlot = global::UnityEngine.Object.Instantiate(slotPrefab, base.transform);
				Vector3 centerPoint = starPlacements.position;
				Vector3 relativePosition = child.position - centerPoint;
				float f = angle * (MathF.PI / 180f);
				float cos = Mathf.Cos(f);
				float sin = Mathf.Sin(f);
				Vector3 rotatedPosition = new Vector3(relativePosition.x * cos - relativePosition.y * sin, relativePosition.x * sin + relativePosition.y * cos, relativePosition.z);
				newSlot.transform.position = centerPoint + rotatedPosition;
				newSlot.Setup(i, type);
				_slots.Add(newSlot);
			}
			for (int j = -1; j < guide.edges.Count; j++)
			{
				if (j == -1)
				{
					UI_Lobby_Constellations_HeroConstellation_StarEdge firstEdge = global::UnityEngine.Object.Instantiate(edgePrefab, base.transform);
					firstEdge.Setup(type, alphaStar, _slots[startIndex].transform);
					firstEdge.transform.SetAsFirstSibling();
					_edges.Add(firstEdge);
				}
				else
				{
					StarEdge e = guide.edges[j];
					UI_Lobby_Constellations_HeroConstellation_StarEdge newEdge = global::UnityEngine.Object.Instantiate(edgePrefab, base.transform);
					newEdge.Setup(type, _slots[startIndex + e.a].transform, _slots[startIndex + e.b].transform);
					newEdge.transform.SetAsFirstSibling();
					_slots[startIndex + e.a].adjacentSlots.Add(_slots[startIndex + e.b]);
					_slots[startIndex + e.b].adjacentSlots.Add(_slots[startIndex + e.a]);
					_edges.Add(newEdge);
				}
			}
		}
	}
}
