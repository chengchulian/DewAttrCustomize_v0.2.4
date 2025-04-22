using UnityEngine;

public class UI_EntityCrowdControl : LogicBehaviour
{
	public GameObject stunObject;

	public GameObject rootObject;

	public GameObject silenceObject;

	public GameObject blindObject;

	public GameObject nameObject;

	private UI_EntityProvider _provider;

	public Entity target
	{
		get
		{
			if (!(_provider != null))
			{
				return null;
			}
			return _provider.target;
		}
	}

	protected virtual void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		stunObject.SetActive(value: false);
		rootObject.SetActive(value: false);
		silenceObject.SetActive(value: false);
		blindObject.SetActive(value: false);
		nameObject.SetActive(value: true);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (target.IsNullInactiveDeadOrKnockedOut())
		{
			stunObject.SetActive(value: false);
			rootObject.SetActive(value: false);
			silenceObject.SetActive(value: false);
			blindObject.SetActive(value: false);
			nameObject.SetActive(value: true);
		}
		else if (target.Status.hasStun || target.Control.isAirborne)
		{
			stunObject.SetActive(value: true);
			rootObject.SetActive(value: false);
			silenceObject.SetActive(value: false);
			blindObject.SetActive(value: false);
			nameObject.SetActive(value: false);
		}
		else if (target.Status.hasRoot)
		{
			stunObject.SetActive(value: false);
			rootObject.SetActive(value: true);
			silenceObject.SetActive(value: false);
			blindObject.SetActive(value: false);
			nameObject.SetActive(value: false);
		}
		else if (target.Status.hasSilence)
		{
			stunObject.SetActive(value: false);
			rootObject.SetActive(value: false);
			silenceObject.SetActive(value: true);
			blindObject.SetActive(value: false);
			nameObject.SetActive(value: false);
		}
		else if (target.Status.hasBlind)
		{
			stunObject.SetActive(value: false);
			rootObject.SetActive(value: false);
			silenceObject.SetActive(value: false);
			blindObject.SetActive(value: true);
			nameObject.SetActive(value: false);
		}
		else
		{
			stunObject.SetActive(value: false);
			rootObject.SetActive(value: false);
			silenceObject.SetActive(value: false);
			blindObject.SetActive(value: false);
			nameObject.SetActive(value: true);
		}
	}
}
