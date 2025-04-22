using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_InGame_ArtifactStoryView : View
{
	public GameObject newObject;

	public GameObject alreadyDiscoveredObject;

	public ScrollRect scrollRect;

	protected override void Start()
	{
		base.Start();
		if (Application.IsPlaying(this))
		{
			NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnArtifactAppraised += new Action<string, bool>(ClientEventOnArtifactAppraised);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Application.IsPlaying(this) && DewInput.currentMode == InputMode.Gamepad)
		{
			float value = Mathf.Clamp(DewInput.GetLeftJoystick().y + DewInput.GetRightJoystick().y, -1f, 1f);
			if (DewInput.GetButton(GamepadButtonEx.DpadUp))
			{
				value = 1f;
			}
			else if (DewInput.GetButton(GamepadButtonEx.DpadDown))
			{
				value = -1f;
			}
			scrollRect.content.transform.position += Vector3.down * (value * 600f * Time.unscaledDeltaTime);
		}
	}

	private void ClientEventOnArtifactAppraised(string artifactName, bool isNew)
	{
		InGameUIManager.instance.SetState("Artifact");
		UI_Tooltip_BaseObj[] objs = GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
		object[] data = new object[1] { DewResources.GetByShortTypeName<Artifact>(artifactName) };
		UI_Tooltip_BaseObj[] array = objs;
		foreach (UI_Tooltip_BaseObj obj in array)
		{
			obj.currentObjects = data;
			obj.OnSetup();
		}
		newObject.SetActive(isNew);
		alreadyDiscoveredObject.SetActive(!isNew);
		GetComponentInChildren<ScrollRect>().ScrollToTop();
	}

	public void GoBack()
	{
		InGameUIManager.instance.SetState("Playing");
	}
}
