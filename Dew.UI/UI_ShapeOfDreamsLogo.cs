using UnityEngine;

public class UI_ShapeOfDreamsLogo : MonoBehaviour, ILangaugeChangedCallback
{
	public GameObject prologueObject;

	public GameObject prologueChinaObject;

	public GameObject nonPrologueObject;

	public GameObject nonPrologueChinaObject;

	private void Start()
	{
		OnLanguageChanged();
	}

	public void OnLanguageChanged()
	{
		bool num = DewBuildProfile.current.HasFeature(BuildFeatureTag.Prologue);
		bool isChinese = DewSave.profile.language == "zh-CN";
		prologueObject.SetActive(value: false);
		prologueChinaObject.SetActive(value: false);
		nonPrologueObject.SetActive(value: false);
		nonPrologueChinaObject.SetActive(value: false);
		if (num)
		{
			if (isChinese)
			{
				prologueChinaObject.SetActive(value: true);
			}
			else
			{
				prologueObject.SetActive(value: true);
			}
		}
		else if (isChinese)
		{
			nonPrologueChinaObject.SetActive(value: true);
		}
		else
		{
			nonPrologueObject.SetActive(value: true);
		}
	}
}
