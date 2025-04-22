using UnityEngine;

public class IwasiMove : MonoBehaviour
{
	public float speed = 1.5f;

	public float rotateSpeed = 1f;

	public Transform target;

	private Vector3 targetRelPos;

	private void Update()
	{
		targetRelPos = target.position - base.transform.position;
		Rigidbody iwasirigid = GetComponent<Rigidbody>();
		float dottigawa = Vector3.Dot(targetRelPos, base.transform.right);
		if (dottigawa < 0f)
		{
			iwasirigid.AddTorque(-Vector3.up * rotateSpeed);
		}
		else if (dottigawa > 0f)
		{
			iwasirigid.AddTorque(Vector3.up * rotateSpeed);
		}
		iwasirigid.velocity = base.transform.forward * speed;
	}
}
