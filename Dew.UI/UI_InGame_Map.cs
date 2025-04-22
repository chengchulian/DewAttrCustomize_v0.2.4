using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[LogicUpdatePriority(6000)]
public class UI_InGame_Map : LogicBehaviour, IDragHandler, IEventSystemHandler
{
	public RawImage mapImage;

	public float scale = 0.001f;

	public RectTransform mapRect;

	public bool isMinimap;

	public float itemScale = 1f;

	public Transform parentTemplate;

	public UI_InGame_Map_Item_Entity entityItem;

	public UI_InGame_Map_Item_Actor[] actorItems;

	public UI_InGame_Map_Item_NetBehaviour[] netBehaviourItems;

	private UI_InGame_Map_Item _selectedItem;

	private Material _mapImageMaterial;

	private Transform[] _parents;

	public Cells2D<MapCellType> data { get; private set; }

	private void Start()
	{
		mapRect = mapImage.rectTransform;
		_mapImageMaterial = global::UnityEngine.Object.Instantiate(mapImage.material);
		mapImage.material = _mapImageMaterial;
		_parents = new Transform[6];
		for (int i = 0; i < _parents.Length; i++)
		{
			_parents[i] = global::UnityEngine.Object.Instantiate(parentTemplate, parentTemplate.parent);
		}
		parentTemplate.gameObject.SetActive(value: false);
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += (Action<EventInfoLoadRoom>)delegate
		{
			ReloadMap();
		};
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += (Action<EventInfoLoadRoom>)delegate
		{
			RegisterRoomCallbacks();
		};
		DewNetworkManager instance = DewNetworkManager.instance;
		instance.onDewNetworkBehaviourStart = (Action<DewNetworkBehaviour>)Delegate.Combine(instance.onDewNetworkBehaviourStart, new Action<DewNetworkBehaviour>(OnDewNetworkBehaviourStart));
		GameManager.CallOnReady(delegate
		{
			RegisterRoomCallbacks();
			ReloadMap();
			NetworkedManagerBase<ActorManager>.instance.onActorAdd += new Action<Actor>(OnActorAdd);
			foreach (Actor current in NetworkedManagerBase<ActorManager>.instance.allActors)
			{
				OnActorAdd(current);
			}
			NetworkedManagerBase<ActorManager>.instance.onAwakeEntityAdd += new Action<Entity>(OnAwakeEntityAdd);
			foreach (Entity current2 in NetworkedManagerBase<ActorManager>.instance.allEntities)
			{
				if (!current2.isSleeping)
				{
					OnAwakeEntityAdd(current2);
				}
			}
			DewNetworkBehaviour[] array = global::UnityEngine.Object.FindObjectsOfType<DewNetworkBehaviour>(includeInactive: false);
			foreach (DewNetworkBehaviour obj in array)
			{
				OnDewNetworkBehaviourStart(obj);
			}
		});
	}

	private void OnAwakeEntityAdd(Entity obj)
	{
		InstantiateItem(entityItem, obj);
	}

	private void OnDewNetworkBehaviourStart(DewNetworkBehaviour obj)
	{
		UI_InGame_Map_Item_NetBehaviour[] array = netBehaviourItems;
		foreach (UI_InGame_Map_Item_NetBehaviour i2 in array)
		{
			if (i2.IsSupported(obj))
			{
				InstantiateItem(i2, obj);
				break;
			}
		}
	}

	private void OnActorAdd(Actor obj)
	{
		UI_InGame_Map_Item_Actor[] array = actorItems;
		foreach (UI_InGame_Map_Item_Actor i2 in array)
		{
			if (i2.IsSupported(obj))
			{
				InstantiateItem(i2, obj);
				break;
			}
		}
	}

	private void InstantiateItem(UI_InGame_Map_Item prefab, object obj)
	{
		UI_InGame_Map_Item newItem = global::UnityEngine.Object.Instantiate(prefab, _parents[0]);
		MapItemOrder order = newItem.OnSetup(obj);
		if (!(newItem == null))
		{
			newItem.transform.localScale *= itemScale;
			newItem.transform.SetParent(_parents[(int)order], worldPositionStays: false);
		}
	}

	private void OnDestroy()
	{
		if (_mapImageMaterial != null)
		{
			global::UnityEngine.Object.Destroy(_mapImageMaterial);
		}
		if (DewNetworkManager.instance != null)
		{
			DewNetworkManager instance = DewNetworkManager.instance;
			instance.onDewNetworkBehaviourStart = (Action<DewNetworkBehaviour>)Delegate.Remove(instance.onDewNetworkBehaviourStart, new Action<DewNetworkBehaviour>(OnDewNetworkBehaviourStart));
		}
	}

	private void RegisterRoomCallbacks()
	{
	}

	private void ReloadMap()
	{
		data = SingletonDewNetworkBehaviour<Room>.instance.map.mapData.cells;
		Vector2 worldSize = new Vector2((float)data.dataWidth * data.cellSize, (float)data.dataHeight * data.cellSize);
		mapRect.sizeDelta = worldSize * scale;
		mapImage.texture = SingletonDewNetworkBehaviour<Room>.instance.map.mapTexture;
		mapImage.gameObject.SetActive(value: false);
		mapImage.gameObject.SetActive(value: true);
		_mapImageMaterial.SetTexture("_FoWTexture", SingletonDewNetworkBehaviour<Room>.instance.map.fowTexture);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (data != null)
		{
			FocusLocalHero();
			UI_InGame_Map_Item[] componentsInChildren = GetComponentsInChildren<UI_InGame_Map_Item>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DoUpdate();
			}
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (isMinimap)
		{
			FocusLocalHero();
		}
	}

	private void MoveMapToNormalizedPos(Vector2 pos)
	{
		mapRect.pivot = pos;
		mapRect.anchoredPosition = default(Vector2);
	}

	private void MoveMapToWorldPos(Vector3 worldPos)
	{
		if (data != null)
		{
			Vector2 pos = worldPos.ToXY();
			Vector2 normalized = data.GetNormalizedPos(pos);
			MoveMapToNormalizedPos(normalized);
		}
	}

	private void MoveMapToItem(UI_InGame_Map_Item item)
	{
		RectTransform rt = (RectTransform)item.transform;
		MoveMapToNormalizedPos(rt.anchorMin);
	}

	private void MoveMapByScreenDelta(Vector2 delta)
	{
		if (data != null)
		{
			mapRect.position += new Vector3(delta.x, delta.y, 0f);
		}
	}

	private void FocusLocalHero()
	{
		if (data != null && !(ManagerBase<CameraManager>.instance == null) && !(ManagerBase<CameraManager>.instance.focusedEntity == null))
		{
			Vector2 pos = ManagerBase<CameraManager>.instance.focusedEntity.position.ToXY();
			Vector2 normalized = data.GetNormalizedPos(pos);
			mapRect.pivot = normalized;
			mapRect.anchoredPosition = default(Vector2);
			mapRect.rotation = Quaternion.Euler(0f, 0f, ManagerBase<CameraManager>.instance.entityCamAngle);
		}
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (!isMinimap)
		{
			MoveMapByScreenDelta(eventData.delta);
		}
	}
}
