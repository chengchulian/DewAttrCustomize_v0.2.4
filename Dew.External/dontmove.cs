using UnityEngine;

public class dontmove : MonoBehaviour
{
	private Vector3 initialPos;

	private void Start()
	{
		initialPos = base.transform.position;
	}

	private void LateUpdate()
	{
		base.transform.position = new Vector3(initialPos.x, base.transform.position.y, initialPos.z);
	}

	public void resetPos()
	{
		base.transform.position = new Vector3(initialPos.x, initialPos.y, initialPos.z);
	}
}
