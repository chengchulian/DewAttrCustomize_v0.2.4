using System;
using UnityEngine;

public class UI_Lobby_Constellations_FloatingStar : MonoBehaviour
{
	private void Start()
	{
		SingletonBehaviour<UI_Lobby_Constellations>.instance.onIsDraggingChanged += new Action(OnIsDraggingChanged);
		base.gameObject.SetActive(value: false);
	}

	private void OnIsDraggingChanged()
	{
		base.gameObject.SetActive(SingletonBehaviour<UI_Lobby_Constellations>.instance.isDragging);
		if (base.gameObject.activeInHierarchy)
		{
			UpdatePos();
			GetComponentInChildren<UI_StarIcon>().Setup(SingletonBehaviour<UI_Lobby_Constellations>.instance.draggingStar, -1f, isLocked: false);
		}
	}

	private void Update()
	{
		UpdatePos();
	}

	private void UpdatePos()
	{
		base.transform.position = Input.mousePosition;
	}
}
