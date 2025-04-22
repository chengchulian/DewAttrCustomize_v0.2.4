using TMPro;
using UnityEngine;

public class UI_InGame_OffscreenIndicator_Item_Entity : UI_InGame_OffscreenIndicator_Item
{
	public TextMeshProUGUI ownerNameText;

	public GameObject lowHealthObject;

	public float lowHealthRatio = 0.35f;

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

	protected override void Awake()
	{
		base.Awake();
		_provider = GetComponentInParent<UI_EntityProvider>();
	}

	private void Start()
	{
		if (target != null && ownerNameText != null)
		{
			ownerNameText.text = target.owner.playerName;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (lowHealthObject != null)
		{
			lowHealthObject.SetActive(target.normalizedHealth < lowHealthRatio);
		}
	}

	protected override Vector3 GetTargetPosition()
	{
		return target.Visual.GetCenterPosition();
	}

	protected override bool ShouldBeHidden()
	{
		if (target is Hero h)
		{
			return h.isKnockedOut;
		}
		return false;
	}

	protected override bool ShouldBeDestroyed()
	{
		return target.IsNullOrInactive();
	}
}
