using UnityEngine;
using UnityEngine.UI;

public class UI_EntitySummonDuration : LogicBehaviour
{
	public Image fillImage;

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
		UpdateBar();
	}

	private void UpdateBar()
	{
		if (target.IsNullInactiveDeadOrKnockedOut() || !(target is Summon s) || float.IsPositiveInfinity(s.maxDuration))
		{
			_cg.alpha = 0f;
			return;
		}
		_cg.alpha = 1f;
		fillImage.fillAmount = s.normalizedRemainingDuration;
	}
}
