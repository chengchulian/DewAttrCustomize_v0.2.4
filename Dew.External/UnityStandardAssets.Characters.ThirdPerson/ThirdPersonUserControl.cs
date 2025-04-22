using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
	private ThirdPersonCharacter m_Character;

	private Transform m_Cam;

	private Vector3 m_CamForward;

	private Vector3 m_Move;

	private bool m_Jump;

	private void Start()
	{
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", base.gameObject);
		}
		m_Character = GetComponent<ThirdPersonCharacter>();
	}

	private void Update()
	{
		if (!m_Jump)
		{
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}

	private void FixedUpdate()
	{
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		bool crouch = Input.GetKey(KeyCode.C);
		if (m_Cam != null)
		{
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1f, 0f, 1f)).normalized;
			m_Move = v * m_CamForward + h * m_Cam.right;
		}
		else
		{
			m_Move = v * Vector3.forward + h * Vector3.right;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			m_Move *= 0.5f;
		}
		m_Character.Move(m_Move, crouch, m_Jump);
		m_Jump = false;
	}
}
