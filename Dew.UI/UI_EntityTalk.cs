using UnityEngine;

[LogicUpdatePriority(500)]
public class UI_EntityTalk : LogicBehaviour
{
	public GameObject talkObject;

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

	private void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		talkObject.SetActive(value: false);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(target is IInteractable interactable) || ManagerBase<ControlManager>.softInstance == null)
		{
			talkObject.SetActive(value: false);
		}
		else
		{
			talkObject.SetActive(ManagerBase<ControlManager>.softInstance.focusedInteractable == interactable);
		}
	}
}
