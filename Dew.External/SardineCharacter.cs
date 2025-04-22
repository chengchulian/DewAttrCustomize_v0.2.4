using UnityEngine;

public class SardineCharacter : MonoBehaviour
{
	public Animator sardineAnimator;

	private Rigidbody sardineRigid;

	public float turnSpeed = 5f;

	public float forwardSpeed = 5f;

	private void Start()
	{
		sardineAnimator = GetComponent<Animator>();
		sardineAnimator.SetFloat("Forward", forwardSpeed);
		sardineRigid = GetComponent<Rigidbody>();
	}

	public void TurnLeft()
	{
		sardineRigid.AddTorque(-base.transform.up * turnSpeed, ForceMode.Impulse);
		sardineAnimator.SetTrigger("TurnLeft");
	}

	public void TurnRight()
	{
		sardineRigid.AddTorque(base.transform.up * turnSpeed, ForceMode.Impulse);
		sardineAnimator.SetTrigger("TurnRight");
	}

	public void MoveForward()
	{
		sardineRigid.AddForce(base.transform.forward * forwardSpeed, ForceMode.Impulse);
		sardineAnimator.SetTrigger("MoveForward");
	}

	public void TurnUp()
	{
		sardineRigid.AddTorque(-base.transform.right * turnSpeed, ForceMode.Impulse);
	}

	public void TurnDown()
	{
		sardineRigid.AddTorque(base.transform.right * turnSpeed, ForceMode.Impulse);
	}

	public void setForwardSpeed(float fSpeed)
	{
		forwardSpeed = fSpeed;
		sardineAnimator.SetFloat("Forward", forwardSpeed);
	}

	public void Move(float v, float h)
	{
		sardineAnimator.SetFloat("Up", 0f - v);
		sardineAnimator.SetFloat("Turn", h);
		sardineRigid.AddForce(base.transform.forward * forwardSpeed);
		sardineRigid.AddTorque(base.transform.right * turnSpeed * v);
		sardineRigid.AddTorque(base.transform.up * turnSpeed * h);
	}
}
