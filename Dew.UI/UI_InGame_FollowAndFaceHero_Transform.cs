using UnityEngine;

public class UI_InGame_FollowAndFaceHero_Transform : UI_InGame_FollowAndFaceHero_Base
{
	private Transform _target;

	private UI_EntityProvider _entityProvider;

	public Transform target
	{
		get
		{
			if (!(_entityProvider == null) && !(_entityProvider.target == null))
			{
				return _entityProvider.target.transform;
			}
			return _target;
		}
		set
		{
			_target = value;
			UpdatePositionAndRotation(immediately: true);
		}
	}

	private void Start()
	{
		_entityProvider = GetComponent<UI_EntityProvider>();
	}

	protected override bool ShouldUpdate()
	{
		return target != null;
	}

	protected override Vector3 GetPosition()
	{
		return target.position;
	}
}
