using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	public Transform target;

	private bool enable;

	private Vector3 playerOffset;

	private void Start()
	{
		if (target == null)
		{
			if (Camera.main != null)
			{
				target = Camera.main.transform;
			}
			else
			{
				Debug.Log("FollowTarget needs a target to be assigned");
				enable = false;
			}
		}
		else
		{
			enable = true;
		}
		if (enable)
		{
			playerOffset = target.position - base.transform.position;
		}
	}

	private void Update()
	{
		if (enable)
		{
			base.transform.position = target.position - playerOffset;
		}
	}
}
