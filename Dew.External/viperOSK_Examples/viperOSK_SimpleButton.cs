using UnityEngine;
using UnityEngine.Events;
using viperTools;

namespace viperOSK_Examples;

public class viperOSK_SimpleButton : MonoBehaviour
{
	public UnityEvent action;

	public Color color1;

	public Color color2;

	private void Start()
	{
		color1 = GetComponent<SpriteRenderer>().color;
	}

	private void OnMouseUp()
	{
		action.Invoke();
		base.transform.Translate(new Vector3(0.05f, -0.05f, 0f));
		Invoke("ReturnToPos", 0.1f);
	}

	public void ReturnToPos()
	{
		base.transform.Translate(new Vector3(-0.05f, 0.05f, 0f));
	}

	public void Enable(bool enabled)
	{
		base.gameObject.SetActive(enabled);
	}

	public void SwitchColor()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		if (renderer.color == color1)
		{
			renderer.color = color2;
		}
		else
		{
			renderer.color = color1;
		}
	}

	private void InputFromPointerDevice()
	{
		Vector2 pointPos = viperInput.GetPointerPos();
		if (viperInput.PointerDown() && Physics.Raycast(Camera.main.ScreenPointToRay(pointPos), out var hit) && hit.collider.gameObject == base.gameObject)
		{
			OnMouseUp();
		}
	}

	private void Update()
	{
	}
}
