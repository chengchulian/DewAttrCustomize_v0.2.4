using System;
using UnityEngine;

public class UI_EntityProvider : MonoBehaviour
{
	private Entity _target;

	public Action<Entity, Entity> onTargetChanged;

	public Entity target
	{
		get
		{
			return _target;
		}
		set
		{
			if (!(_target == value))
			{
				Entity oldValue = _target;
				_target = value;
				onTargetChanged?.Invoke(oldValue, value);
			}
		}
	}
}
