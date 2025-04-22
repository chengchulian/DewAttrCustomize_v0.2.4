using UnityEngine;

public class UI_EntityUnstoppable : LogicBehaviour
{
	private UI_EntityProvider _provider;

	private CanvasGroup _cg;

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
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (target.IsNullInactiveDeadOrKnockedOut())
		{
			_cg.alpha = 0f;
		}
		else
		{
			_cg.alpha = ((target.Status.hasCrowdControlImmunity && !(target is PropEntity)) ? 1 : 0);
		}
	}
}
