using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
	public Transform Target;

	private void Update()
	{
		base.transform.LookAt(Target);
	}
}
