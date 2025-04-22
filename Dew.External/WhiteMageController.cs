using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class WhiteMageController : MonoBehaviour
{
	public float velocity = 9f;

	[Space]
	public float InputX;

	public float InputZ;

	public Vector3 desiredMoveDirection;

	public bool blockRotationPlayer;

	public float desiredRotationSpeed = 0.1f;

	public Animator anim;

	public float Speed;

	public float allowPlayerRotation = 0.1f;

	public Camera cam;

	public CharacterController controller;

	public bool isGrounded;

	private float secondLayerWeight;

	[Space]
	[Header("Animation Smoothing")]
	[Range(0f, 1f)]
	public float HorizontalAnimSmoothTime = 0.2f;

	[Range(0f, 1f)]
	public float VerticalAnimTime = 0.2f;

	[Range(0f, 1f)]
	public float StartAnimTime = 0.3f;

	[Range(0f, 1f)]
	public float StopAnimTime = 0.15f;

	private float verticalVel;

	private Vector3 moveVector;

	public bool canMove;

	[Space]
	[Header("Effects")]
	public GameObject TargetMarker;

	public GameObject TargetMarker2;

	public GameObject[] Prefabs;

	public GameObject[] PrefabsCast;

	public Transform parentPlace;

	public GameObject[] UltimatePrefab;

	public float skillsRange = 6f;

	private bool canUlt;

	private bool useUlt;

	private ParticleSystem currEffect;

	private ParticleSystem Effect;

	public float[] castingTime;

	private bool casting;

	private int currNumber;

	public LayerMask collidingLayer = -1;

	private bool[] fastSkillrefresh;

	[Space]
	[Header("Canvas")]
	public GameObject[] ultIcons;

	public Image aim;

	public Vector2 uiOffset;

	public List<Transform> screenTargets = new List<Transform>();

	private Transform target;

	private bool activeTarger;

	public Transform FirePoint;

	public float fireRate = 0.1f;

	private float fireCountdown;

	private bool rotateState;

	[Space]
	[Header("Sound effects")]
	private AudioSource soundComponent;

	private AudioClip clip;

	private AudioSource soundComponentCast;

	private AudioSource soundComponentUlt;

	[Space]
	[Header("Camera Shaker script")]
	public HS_CameraShaker cameraShaker;

	private void Start()
	{
		fastSkillrefresh = new bool[Prefabs.Length];
		for (int i = 0; i < Prefabs.Length; i++)
		{
			fastSkillrefresh[i] = false;
		}
		casting = false;
		anim = GetComponent<Animator>();
		cam = Camera.main;
		controller = GetComponent<CharacterController>();
		if ((bool)Prefabs[8].GetComponent<AudioSource>())
		{
			soundComponent = Prefabs[8].GetComponent<AudioSource>();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, skillsRange);
	}

	private void Update()
	{
		target = screenTargets[targetIndex()];
		if (Input.GetMouseButtonDown(1) && casting)
		{
			casting = false;
		}
		if (Input.GetKeyDown("1"))
		{
			if (canUlt)
			{
				useUlt = true;
			}
			else
			{
				StartCoroutine(PreCast(0));
			}
		}
		if (Input.GetKeyDown("2") && !casting && !fastSkillrefresh[1])
		{
			StartCoroutine(FastPlay(1, 0f, 2.5f));
		}
		if (Input.GetKeyDown("3") && !fastSkillrefresh[2])
		{
			StartCoroutine(FastPlay(2, 0.35f, 2.5f));
		}
		if (Input.GetKeyDown("4") && !fastSkillrefresh[3])
		{
			StartCoroutine(FastPlay(3, 0f, 5f));
		}
		if (Input.GetKeyDown("z"))
		{
			StartCoroutine(FrontAttack(4));
		}
		if (Input.GetKeyDown("x") && !fastSkillrefresh[5])
		{
			StartCoroutine(FastPlay(5, 1.5f, 2.5f));
		}
		if (Input.GetKeyDown("c"))
		{
			if (canUlt)
			{
				useUlt = true;
			}
			else
			{
				StartCoroutine(PreCast(6));
			}
		}
		if (Input.GetKeyDown("v") && !fastSkillrefresh[7])
		{
			StartCoroutine(FastPlay(7, 1.1f, 0.5f));
		}
		UserInterface();
		if (!canMove)
		{
			return;
		}
		if (Input.GetMouseButton(0) && aim.enabled && activeTarger)
		{
			if (!rotateState)
			{
				StartCoroutine(RotateToTarget(fireRate, target.position));
			}
			secondLayerWeight = Mathf.Lerp(secondLayerWeight, 1f, Time.deltaTime * 10f);
			if (fireCountdown <= 0f)
			{
				Object.Instantiate(PrefabsCast[8], FirePoint.position, FirePoint.rotation).GetComponent<TargetProjectile>().UpdateTarget(target, uiOffset);
				Effect = Prefabs[8].GetComponent<ParticleSystem>();
				Effect.Play();
				if ((bool)Prefabs[8].GetComponent<AudioSource>())
				{
					soundComponent = Prefabs[8].GetComponent<AudioSource>();
					clip = soundComponent.clip;
					soundComponent.PlayOneShot(clip);
				}
				StartCoroutine(cameraShaker.Shake(0.1f, 2f, 0.2f, 0f));
				fireCountdown = 0f;
				fireCountdown += fireRate;
			}
		}
		else
		{
			secondLayerWeight = Mathf.Lerp(secondLayerWeight, 0f, Time.deltaTime * 10f);
		}
		fireCountdown -= Time.deltaTime;
		if (Input.GetMouseButtonDown(1) && aim.enabled && activeTarger)
		{
			if (!rotateState)
			{
				StartCoroutine(RotateToTarget(fireRate, target.position));
			}
			secondLayerWeight = Mathf.Lerp(secondLayerWeight, 1f, Time.deltaTime * 10f);
			GameObject obj = Object.Instantiate(PrefabsCast[9], target.position, target.rotation);
			obj.transform.parent = target;
			ParticleSystem buffPS = obj.GetComponent<ParticleSystem>();
			Object.Destroy(obj, buffPS.main.duration);
			Effect = Prefabs[9].GetComponent<ParticleSystem>();
			Effect.Play();
			if ((bool)Prefabs[9].GetComponent<AudioSource>())
			{
				soundComponent = Prefabs[9].GetComponent<AudioSource>();
				clip = soundComponent.clip;
				soundComponent.PlayOneShot(clip);
			}
			StartCoroutine(cameraShaker.Shake(0.15f, 2f, 0.2f, 0f));
		}
		else
		{
			secondLayerWeight = Mathf.Lerp(secondLayerWeight, 0f, Time.deltaTime * 10f);
		}
		if (anim.layerCount > 1)
		{
			anim.SetLayerWeight(1, secondLayerWeight);
		}
		InputMagnitude();
		isGrounded = controller.isGrounded;
		if (isGrounded)
		{
			verticalVel = 0f;
		}
		else
		{
			verticalVel -= 1f * Time.deltaTime;
		}
		moveVector = new Vector3(0f, verticalVel, 0f);
		controller.Move(moveVector);
	}

	public IEnumerator FastPlayTimer(int EffectNumber)
	{
		fastSkillrefresh[EffectNumber] = true;
		yield return new WaitForSeconds(castingTime[EffectNumber]);
		fastSkillrefresh[EffectNumber] = false;
	}

	public IEnumerator FastPlay(int EffectNumber, float castDelay, float endDelay)
	{
		StartCoroutine(FastPlayTimer(EffectNumber));
		if ((bool)Prefabs[EffectNumber].GetComponent<AudioSource>())
		{
			soundComponent = Prefabs[EffectNumber].GetComponent<AudioSource>();
			AudioClip clip = soundComponent.clip;
			soundComponent.PlayOneShot(clip);
		}
		if (EffectNumber == 1 || EffectNumber == 3)
		{
			StartCoroutine(cameraShaker.Shake(0.3f, 5f, 0.5f, 0f));
		}
		if (EffectNumber == 2 || EffectNumber == 5 || EffectNumber == 7)
		{
			casting = true;
			canMove = false;
			SetAnimZero();
			anim.SetTrigger("Attack3");
			StartCoroutine(cameraShaker.Shake(0.4f, 5f, 1f, 0.4f));
			if (EffectNumber == 5)
			{
				StartCoroutine(cameraShaker.Shake(0.5f, 5f, 2f, castDelay));
			}
			if (EffectNumber == 7)
			{
				yield return new WaitForSeconds(0.4f);
			}
		}
		if (UltimatePrefab[EffectNumber] != null)
		{
			StartCoroutine(Ult(EffectNumber, 0f, 1.5f, new Vector3(0f, 0f, 0f), base.transform.rotation, ChangePos: false));
		}
		if (Prefabs[EffectNumber].GetComponent<FrontAttack>() != null)
		{
			FrontAttack[] componentsInChildren = Prefabs[EffectNumber].GetComponentsInChildren<FrontAttack>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].playMeshEffect = true;
			}
			foreach (Transform skillTarget in screenTargets)
			{
				if (Vector3.Distance(skillTarget.position, base.transform.position) <= skillsRange)
				{
					GameObject obj = Object.Instantiate(PrefabsCast[EffectNumber], skillTarget.position, skillTarget.rotation);
					obj.transform.parent = skillTarget;
					Object.Destroy(obj, castingTime[EffectNumber]);
				}
			}
			yield return new WaitForSeconds(castingTime[EffectNumber]);
			yield break;
		}
		Effect = Prefabs[EffectNumber].GetComponent<ParticleSystem>();
		Effect.Play();
		if (EffectNumber == 7)
		{
			Prefabs[EffectNumber].transform.parent = null;
			foreach (Transform skillTarget2 in screenTargets)
			{
				if (Vector3.Distance(skillTarget2.position, base.transform.position) <= skillsRange)
				{
					GameObject obj2 = Object.Instantiate(PrefabsCast[EffectNumber], skillTarget2.position, skillTarget2.rotation);
					obj2.transform.parent = skillTarget2;
					Object.Destroy(obj2, castingTime[EffectNumber]);
				}
			}
			yield return new WaitForSeconds(castDelay);
			casting = false;
			canMove = true;
			yield return new WaitForSeconds(endDelay);
			Prefabs[EffectNumber].transform.parent = parentPlace;
			Prefabs[EffectNumber].transform.position = parentPlace.position;
			yield break;
		}
		yield return new WaitForSeconds(castDelay);
		PrefabsCast[EffectNumber].transform.parent = null;
		currEffect = PrefabsCast[EffectNumber].GetComponent<ParticleSystem>();
		currEffect.Play();
		if ((bool)PrefabsCast[EffectNumber].GetComponent<AudioSource>())
		{
			soundComponentCast = PrefabsCast[EffectNumber].GetComponent<AudioSource>();
			AudioClip clip2 = soundComponentCast.clip;
			soundComponentCast.PlayOneShot(clip2);
		}
		if (EffectNumber == 2)
		{
			yield return new WaitForSeconds(1f);
			SetAnimZero();
		}
		casting = false;
		canMove = true;
		yield return new WaitForSeconds(endDelay);
		PrefabsCast[EffectNumber].transform.parent = parentPlace;
		PrefabsCast[EffectNumber].transform.position = parentPlace.position;
	}

	public IEnumerator Ult(int EffectNumber, float enableTime, float dissableTime, Vector3 pivotPosition, Quaternion pivotRotation, bool ChangePos)
	{
		yield return new WaitForSeconds(enableTime);
		canUlt = true;
		ultIcons[EffectNumber].SetActive(value: true);
		bool flag;
		while (true)
		{
			dissableTime -= Time.deltaTime;
			if ((bool)UltimatePrefab[EffectNumber] && useUlt)
			{
				if (ChangePos)
				{
					UltimatePrefab[EffectNumber].transform.parent = null;
					if (pivotPosition != new Vector3(1f, 1f, 1f))
					{
						UltimatePrefab[EffectNumber].transform.position = pivotPosition;
					}
					UltimatePrefab[EffectNumber].transform.rotation = pivotRotation;
				}
				if ((bool)UltimatePrefab[EffectNumber].GetComponent<AudioSource>())
				{
					soundComponentUlt = UltimatePrefab[EffectNumber].GetComponent<AudioSource>();
					soundComponentUlt.Play(0uL);
				}
				ParticleSystem ultPS = UltimatePrefab[EffectNumber].GetComponent<ParticleSystem>();
				ultPS.Play();
				if (EffectNumber == 0)
				{
					StartCoroutine(cameraShaker.Shake(0.4f, 5f, 0.35f, 0.1f));
				}
				if (EffectNumber == 1)
				{
					StartCoroutine(cameraShaker.Shake(0.15f, 2f, 0.2f, 0f));
				}
				if (EffectNumber == 6)
				{
					StartCoroutine(cameraShaker.Shake(0.2f, 7f, 3f, 0f));
				}
				if (EffectNumber == 7)
				{
					StartCoroutine(cameraShaker.Shake(0.55f, 7.5f, 0.35f, 0f));
				}
				ultIcons[EffectNumber].SetActive(value: false);
				WhiteMageController whiteMageController = this;
				WhiteMageController whiteMageController2 = this;
				flag = false;
				whiteMageController2.useUlt = false;
				whiteMageController.canUlt = flag;
				yield return new WaitForSeconds(ultPS.main.duration);
				if (ChangePos)
				{
					UltimatePrefab[currNumber].transform.parent = parentPlace;
					UltimatePrefab[currNumber].transform.localPosition = new Vector3(0f, 0f, 0f);
				}
				yield break;
			}
			if (dissableTime <= 0f)
			{
				break;
			}
			yield return null;
		}
		ultIcons[EffectNumber].SetActive(value: false);
		WhiteMageController whiteMageController3 = this;
		WhiteMageController whiteMageController4 = this;
		flag = false;
		whiteMageController4.useUlt = false;
		whiteMageController3.canUlt = flag;
	}

	private void UserInterface()
	{
		Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2f;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);
		Vector3 CornerDistance = screenPos - screenCenter;
		Vector3 absCornerDistance = new Vector3(Mathf.Abs(CornerDistance.x), Mathf.Abs(CornerDistance.y), Mathf.Abs(CornerDistance.z));
		if (absCornerDistance.x < screenCenter.x / 3f && absCornerDistance.y < screenCenter.y / 3f && screenPos.x > 0f && screenPos.y > 0f && screenPos.z > 0f && !Physics.Linecast(base.transform.position + (Vector3)uiOffset, target.position + (Vector3)uiOffset * 2f, collidingLayer))
		{
			aim.transform.position = Vector3.MoveTowards(aim.transform.position, screenPos, Time.deltaTime * 3000f);
			if (!activeTarger)
			{
				activeTarger = true;
			}
		}
		else
		{
			aim.transform.position = Vector3.MoveTowards(aim.transform.position, screenCenter, Time.deltaTime * 3000f);
			if (activeTarger)
			{
				activeTarger = false;
			}
		}
	}

	public void MainSoundPlay()
	{
		clip = soundComponent.clip;
		soundComponent.PlayOneShot(clip);
	}

	public void CastSoundPlay()
	{
		soundComponentCast.Play(0uL);
	}

	public IEnumerator RotateToTarget(float rotatingTime, Vector3 targetPoint)
	{
		rotateState = true;
		float delay = rotatingTime;
		Vector3 lookPos = targetPoint - base.transform.position;
		lookPos.y = 0f;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		while (true)
		{
			if (Speed == 0f)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, rotation, Time.deltaTime * 20f);
			}
			delay -= Time.deltaTime;
			if (delay <= 0f || base.transform.rotation == rotation)
			{
				break;
			}
			yield return null;
		}
		rotateState = false;
	}

	public IEnumerator FrontAttack(int EffectNumber)
	{
		if (!TargetMarker2 || casting)
		{
			yield break;
		}
		aim.enabled = false;
		TargetMarker2.SetActive(value: true);
		while (true)
		{
			Vector3 forwardCamera = Camera.main.transform.forward;
			forwardCamera.y = 0f;
			TargetMarker2.transform.rotation = Quaternion.LookRotation(forwardCamera);
			Vector3 vecPos = base.transform.position + forwardCamera * 4f;
			if (Input.GetMouseButtonDown(0) && !casting)
			{
				casting = true;
				canMove = false;
				SetAnimZero();
				TargetMarker2.SetActive(value: false);
				if (!rotateState)
				{
					StartCoroutine(RotateToTarget(1f, vecPos));
				}
				anim.SetTrigger("FrontAttack");
				StartCoroutine(cameraShaker.Shake(0.4f, 7f, 0.45f, 1f));
				if ((bool)Prefabs[EffectNumber].GetComponent<AudioSource>())
				{
					soundComponent = Prefabs[EffectNumber].GetComponent<AudioSource>();
					MainSoundPlay();
				}
				yield return new WaitForSeconds(1f);
				FrontAttack[] componentsInChildren = Prefabs[EffectNumber].GetComponentsInChildren<FrontAttack>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].PrepeareAttack(vecPos);
				}
				if (UltimatePrefab[EffectNumber] != null)
				{
					if (EffectNumber == 7)
					{
						StartCoroutine(Ult(EffectNumber, 0f, 1.5f, new Vector3(1f, 1f, 1f), Quaternion.LookRotation(forwardCamera), ChangePos: true));
					}
					else
					{
						StartCoroutine(Ult(EffectNumber, 0f, 1.5f, new Vector3(0f, 0f, 0f), Quaternion.LookRotation(forwardCamera), ChangePos: false));
					}
				}
				yield return new WaitForSeconds(castingTime[EffectNumber]);
				StopCasting(EffectNumber);
				aim.enabled = true;
				yield break;
			}
			if (Input.GetMouseButtonDown(1))
			{
				break;
			}
			yield return null;
		}
		TargetMarker2.SetActive(value: false);
		aim.enabled = true;
	}

	public IEnumerator PreCast(int EffectNumber)
	{
		if ((bool)PrefabsCast[EffectNumber] && (bool)TargetMarker)
		{
			while (true)
			{
				aim.enabled = false;
				TargetMarker.SetActive(value: true);
				Vector3 forwardCamera = Camera.main.transform.forward;
				forwardCamera.y = 0f;
				if (Physics.Raycast(new Ray(Camera.main.transform.position + new Vector3(0f, 2f, 0f), Camera.main.transform.forward), out var hit, float.PositiveInfinity, collidingLayer))
				{
					TargetMarker.transform.position = hit.point;
					TargetMarker.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.LookRotation(forwardCamera);
				}
				else
				{
					aim.enabled = true;
					TargetMarker.SetActive(value: false);
				}
				if (Input.GetMouseButtonDown(0) && !casting)
				{
					aim.enabled = true;
					TargetMarker.SetActive(value: false);
					soundComponentCast = null;
					if (!rotateState)
					{
						StartCoroutine(RotateToTarget(1f, hit.point));
					}
					casting = true;
					PrefabsCast[EffectNumber].transform.position = hit.point;
					PrefabsCast[EffectNumber].transform.rotation = Quaternion.LookRotation(forwardCamera);
					PrefabsCast[EffectNumber].transform.parent = null;
					currEffect = PrefabsCast[EffectNumber].GetComponent<ParticleSystem>();
					Effect = Prefabs[EffectNumber].GetComponent<ParticleSystem>();
					if ((bool)Prefabs[EffectNumber].GetComponent<AudioSource>())
					{
						soundComponent = Prefabs[EffectNumber].GetComponent<AudioSource>();
					}
					if ((bool)PrefabsCast[EffectNumber].GetComponent<AudioSource>())
					{
						soundComponentCast = PrefabsCast[EffectNumber].GetComponent<AudioSource>();
					}
					StartCoroutine(OnCast(EffectNumber));
					StartCoroutine(Attack(EffectNumber));
					if (UltimatePrefab[EffectNumber] != null)
					{
						StartCoroutine(Ult(EffectNumber, 0.5f, castingTime[EffectNumber], hit.point, Quaternion.LookRotation(forwardCamera), ChangePos: true));
					}
					yield break;
				}
				if (Input.GetMouseButtonDown(1))
				{
					break;
				}
				yield return null;
			}
			aim.enabled = true;
			TargetMarker.SetActive(value: false);
		}
		else if (!casting)
		{
			Effect = Prefabs[EffectNumber].GetComponent<ParticleSystem>();
			if ((bool)Prefabs[EffectNumber].GetComponent<AudioSource>())
			{
				soundComponent = Prefabs[EffectNumber].GetComponent<AudioSource>();
			}
			casting = true;
			StartCoroutine(Attack(EffectNumber));
		}
	}

	private IEnumerator OnCast(int EffectNumber)
	{
		while (casting)
		{
			if (castingTime[EffectNumber] == 0f)
			{
				currEffect.Play();
				if ((bool)soundComponentCast)
				{
					CastSoundPlay();
				}
				yield return new WaitForSeconds(1f);
				continue;
			}
			currEffect.Play();
			if (EffectNumber == 0)
			{
				StartCoroutine(cameraShaker.Shake(0.2f, 5f, 2f, 1.5f));
			}
			if (EffectNumber == 3)
			{
				StartCoroutine(cameraShaker.Shake(0.6f, 6f, 0.3f, 1.45f));
			}
			if ((bool)soundComponentCast)
			{
				CastSoundPlay();
			}
			yield return new WaitForSeconds(castingTime[EffectNumber]);
			break;
		}
	}

	private void SetAnimZero()
	{
		anim.SetFloat("InputMagnitude", 0f);
		anim.SetFloat("InputZ", 0f);
		anim.SetFloat("InputX", 0f);
	}

	public IEnumerator Attack(int EffectNumber)
	{
		canMove = false;
		SetAnimZero();
		while (casting)
		{
			if (castingTime[EffectNumber] == 0f)
			{
				if (EffectNumber == 2)
				{
					anim.SetTrigger("Attack1");
					StartCoroutine(cameraShaker.Shake(0.2f, 6f, 1.5f, 0f));
				}
				Effect.Play();
				if ((bool)soundComponent)
				{
					MainSoundPlay();
				}
				yield return new WaitForSeconds(0.9f);
				yield return null;
				continue;
			}
			if (EffectNumber == 0)
			{
				anim.SetTrigger("Attack2");
				StartCoroutine(cameraShaker.Shake(0.3f, 6f, 3f, 0f));
			}
			if (EffectNumber == 6)
			{
				anim.SetTrigger("Attack2");
				StartCoroutine(cameraShaker.Shake(0.45f, 6f, 0.5f, 0.8f));
			}
			if (EffectNumber == 3)
			{
				anim.SetTrigger("Attack2");
				StartCoroutine(cameraShaker.Shake(0.2f, 5f, 3f, 0f));
			}
			Effect.Play();
			if ((bool)soundComponent)
			{
				MainSoundPlay();
			}
			yield return new WaitForSeconds(castingTime[EffectNumber]);
			StopCasting(EffectNumber);
			yield break;
		}
		StopCasting(EffectNumber);
	}

	public void StopCasting(int EffectNumber)
	{
		soundComponent = null;
		soundComponentCast = null;
		if ((bool)PrefabsCast[EffectNumber])
		{
			PrefabsCast[EffectNumber].transform.parent = parentPlace;
			PrefabsCast[EffectNumber].transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (EffectNumber == 2)
		{
			anim.Play("Blend Tree");
		}
		currNumber = EffectNumber;
		casting = false;
		canMove = true;
	}

	private void PlayerMoveAndRotation()
	{
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");
		_ = Camera.main;
		Vector3 forward = cam.transform.forward;
		Vector3 right = cam.transform.right;
		forward.y = 0f;
		right.y = 0f;
		forward.Normalize();
		right.Normalize();
		desiredMoveDirection = forward * InputZ + right * InputX;
		desiredMoveDirection.Normalize();
		if (!blockRotationPlayer)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(forward), desiredRotationSpeed);
			if ((double)InputZ < -0.5)
			{
				controller.Move(desiredMoveDirection * Time.deltaTime * (velocity / 1.5f));
			}
			else
			{
				controller.Move(desiredMoveDirection * Time.deltaTime * velocity);
			}
		}
	}

	private void InputMagnitude()
	{
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");
		anim.SetFloat("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		anim.SetFloat("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);
		Speed = new Vector2(InputX, InputZ).sqrMagnitude;
		if (Speed > allowPlayerRotation)
		{
			anim.SetFloat("InputMagnitude", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation();
		}
		else if (Speed < allowPlayerRotation)
		{
			anim.SetFloat("InputMagnitude", Speed, StopAnimTime, Time.deltaTime);
		}
	}

	public int targetIndex()
	{
		float[] distances = new float[screenTargets.Count];
		for (int i = 0; i < screenTargets.Count; i++)
		{
			distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].position), new Vector2(Screen.width / 2, Screen.height / 2));
		}
		float minDistance = Mathf.Min(distances);
		int index = 0;
		for (int j = 0; j < distances.Length; j++)
		{
			if (minDistance == distances[j])
			{
				index = j;
			}
		}
		return index;
	}
}
