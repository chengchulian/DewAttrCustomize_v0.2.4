using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vulture : MonoBehaviour
{
	[Header("Vulture general movement settings.")]
	[Tooltip("Speed of the vulture.")]
	public float speed = 3f;

	[Tooltip("Maximum banking angle.")]
	public float maxBankingAngle = 45f;

	[Tooltip("Turn speed")]
	public float turnSpeed = 30f;

	[Header("Flapping")]
	[Tooltip("Flap force power")]
	public float flapForce;

	[Tooltip("How often to flap.")]
	public float flapFrequency = 3f;

	[Tooltip("How long to flap for.")]
	public float flapTime = 1f;

	[Header("Vulture wandering settings")]
	public bool enableWandering = true;

	[Tooltip("How far from starting Pos to wander off to")]
	public float wanderRange = 50f;

	[Tooltip("How far much to offset in height")]
	public float wanderHeightOffset = 10f;

	[Tooltip("How many wander points")]
	[Range(2f, 10f)]
	public int numberOfWanderPoints = 4;

	[Tooltip("How near to get to wander point")]
	public float wanderPointProximity = 20f;

	[Tooltip("Preferred orbit distance")]
	[Range(0f, 1f)]
	public float orbitDistance = 1f;

	[Tooltip("Deadzone radius")]
	public float deadzoneRadius = 1f;

	[Tooltip("Percent chance to keep circling before moving on")]
	[Range(0f, 1f)]
	public float chanceToMoveOn = 0.002f;

	[Header("Vulture Head look settings")]
	[Tooltip("How often to change look")]
	public float changelookEveryX = 2f;

	public bool debugMode;

	private int lookpose;

	private float timeSinceLastFlap;

	private float timeSpentFlapping;

	private bool flap;

	private Vector3[] wanderPoints;

	private int wanderIndex;

	private float timeLastLookChanged;

	public Rigidbody rigidBody;

	public Animator anim;

	public Transform banker;

	private void Start()
	{
		GenerateWanderPoints();
		flapFrequency += (float)Random.Range(0, 100) * 0.02f;
	}

	private void GenerateWanderPoints()
	{
		if (enableWandering)
		{
			wanderPoints = new Vector3[numberOfWanderPoints];
			for (int i = 0; i < numberOfWanderPoints; i++)
			{
				Vector2 circle = Random.insideUnitCircle * wanderRange;
				float circleHeight = Random.Range(0f, wanderHeightOffset);
				Vector3 newPos = new Vector3(circle.x, circleHeight, circle.y) + base.transform.position;
				wanderPoints[i] = newPos;
			}
		}
	}

	private void Update()
	{
		rigidBody.AddForce(base.transform.forward * speed, ForceMode.VelocityChange);
		HandleWandering();
		timeSinceLastFlap += Time.deltaTime;
		if (timeSinceLastFlap > flapFrequency)
		{
			Flap();
		}
		if (flap)
		{
			timeSpentFlapping += Time.deltaTime;
			if (timeSpentFlapping > flapTime)
			{
				flap = false;
				anim.SetBool("Flap", value: false);
				if (debugMode)
				{
					Debug.Log("Vulture exiting flap");
				}
			}
		}
		timeLastLookChanged += Time.deltaTime;
		if (timeLastLookChanged > changelookEveryX)
		{
			timeLastLookChanged = 0f;
			lookpose = Random.Range(0, 4);
		}
		anim.SetInteger("Look", lookpose);
	}

	private void HandleWandering()
	{
		Vector3 point = wanderPoints[wanderIndex];
		Vector3 lookTowardWanderIndex = point - base.transform.position;
		Vector3 lookToward = Quaternion.LookRotation(lookTowardWanderIndex, Vector3.up) * Vector3.forward;
		Vector3 cross = -Vector3.Cross(lookTowardWanderIndex - lookToward, Vector3.up).normalized * (wanderPointProximity * orbitDistance);
		if (debugMode)
		{
			Debug.DrawRay(point, cross);
		}
		float bankingAngle = maxBankingAngle * (0f - Vector3.Dot(base.transform.right, lookTowardWanderIndex.normalized));
		Quaternion lastRollRot = banker.localRotation;
		float turnStep = Time.deltaTime * turnSpeed;
		banker.localRotation = Quaternion.RotateTowards(lastRollRot, Quaternion.AngleAxis(bankingAngle, Vector3.forward), turnStep);
		if (Vector3.Distance(base.transform.position, point) > deadzoneRadius)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(lookTowardWanderIndex + cross), turnStep);
		}
		base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f);
		if (Vector3.Distance(base.transform.position, point) < wanderPointProximity && Random.value < chanceToMoveOn)
		{
			if (wanderIndex < numberOfWanderPoints - 1)
			{
				wanderIndex++;
			}
			else
			{
				wanderIndex = 0;
			}
		}
	}

	private void Flap()
	{
		timeSinceLastFlap = 0f;
		timeSpentFlapping = 0f;
		rigidBody.AddForce(base.transform.up * flapForce, ForceMode.Impulse);
		flap = true;
		anim.SetBool("Flap", value: true);
		if (debugMode)
		{
			Debug.Log("Vulture entering flap");
		}
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying || !enableWandering || !debugMode)
		{
			return;
		}
		for (int i = 0; i < numberOfWanderPoints; i++)
		{
			Vector3 wanderPoint = wanderPoints[i];
			bool inPoint = Vector3.Distance(base.transform.position, wanderPoint) < wanderPointProximity;
			if (i == wanderIndex)
			{
				Gizmos.color = Color.yellow;
				if (inPoint)
				{
					Gizmos.color = Color.green;
				}
			}
			else if (inPoint)
			{
				Gizmos.color = Color.blue;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawSphere(wanderPoint, 0.3f);
			Gizmos.DrawWireSphere(wanderPoint, wanderPointProximity);
		}
		Gizmos.color = Color.red;
		for (int j = 0; j < numberOfWanderPoints - 1; j++)
		{
			Gizmos.DrawLine(wanderPoints[j], wanderPoints[j + 1]);
		}
		Gizmos.DrawLine(wanderPoints[numberOfWanderPoints - 1], wanderPoints[0]);
	}
}
