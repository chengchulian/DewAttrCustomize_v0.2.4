using System;
using UnityEngine;

public class UI_Title_PrivateDemoNotice : MonoBehaviour
{
	private void Start()
	{
		UIManager instance = ManagerBase<UIManager>.instance;
		instance.onStateChanged = (Action<string, string>)Delegate.Combine(instance.onStateChanged, new Action<string, string>(OnStateChanged));
	}

	private void OnStateChanged(string arg1, string arg2)
	{
		if (DewBuildProfile.current.buildType == BuildType.DemoPrivate && !(arg2 != "Title") && !DewSave.profile.didReadPrivateDemoNotice)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				rawContent = DewLocalization.GetUIValue("Title_Message_PrivateDemoNotice"),
				buttons = DewMessageSettings.ButtonType.Ok,
				owner = this
			});
			DewSave.profile.didReadPrivateDemoNotice = true;
			DewSave.SaveProfile();
		}
	}
}
