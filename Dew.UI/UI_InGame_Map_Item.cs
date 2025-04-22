using System;
using UnityEngine;

[LogicUpdatePriority(6001)]
[RequireComponent(typeof(CanvasGroup))]
public abstract class UI_InGame_Map_Item : LogicBehaviour
{
	public MapItemVisibility visibility;

	private RectTransform _rt;

	public UI_InGame_Map map { get; private set; }

	public CanvasGroup cg { get; private set; }

	public object target { get; private set; }

	public virtual MapItemOrder OnSetup(object t)
	{
		target = t;
		return MapItemOrder.Default;
	}

	public abstract bool ShouldBeDestroyed();

	public abstract Vector3 GetWorldPosition();

	public virtual bool IsVisible()
	{
		return true;
	}

	private void Awake()
	{
		_rt = (RectTransform)base.transform;
		cg = GetComponent<CanvasGroup>();
		map = GetComponentInParent<UI_InGame_Map>();
	}

	private void Start()
	{
		DoUpdate();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		DoUpdate();
	}

	internal void DoUpdate()
	{
		if (!(SingletonDewNetworkBehaviour<Room>.softInstance == null) && SingletonDewNetworkBehaviour<Room>.softInstance.isActive)
		{
			OnUpdateItem();
		}
	}

	private bool CheckVisibility(Vector3 worldPos)
	{
		return visibility switch
		{
			MapItemVisibility.Always => true, 
			MapItemVisibility.NeedsToBeDiscovered => SingletonDewNetworkBehaviour<Room>.instance.map.IsWorldPosVisited(worldPos), 
			MapItemVisibility.NeedsToBeVisible => SingletonDewNetworkBehaviour<Room>.instance.map.IsWorldPosVisible(worldPos), 
			MapItemVisibility.Hidden => false, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	protected virtual void OnUpdateItem()
	{
		if (ShouldBeDestroyed())
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (_rt == null)
		{
			_rt = (RectTransform)base.transform;
			map = GetComponentInParent<UI_InGame_Map>();
			cg = GetComponent<CanvasGroup>();
		}
		Vector3 pos = GetWorldPosition();
		if (!IsVisible() || !CheckVisibility(pos))
		{
			cg.alpha = 0f;
			return;
		}
		cg.alpha = 1f;
		Vector2 a = map.data.GetNormalizedPos(pos.ToXY());
		_rt.anchorMin = a;
		_rt.anchorMax = a;
		_rt.anchoredPosition = default(Vector2);
		_rt.rotation = Quaternion.identity;
	}
}
