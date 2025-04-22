using UnityEngine;

public class UI_InGame_FollowAndFaceHero_AboveEntity : UI_InGame_FollowAndFaceHero_Base
{
	private Entity _target;

	public Entity target
	{
		get
		{
			return _target;
		}
		set
		{
			_target = value;
			UpdatePositionAndRotation(immediately: true);
		}
	}

	protected override bool ShouldUpdate()
	{
		return target != null;
	}

	protected override Vector3 GetPosition()
	{
		return target.Visual.GetAbovePosition();
	}
}
