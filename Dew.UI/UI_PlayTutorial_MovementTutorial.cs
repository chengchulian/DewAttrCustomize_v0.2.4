using UnityEngine;

public class UI_PlayTutorial_MovementTutorial : MonoBehaviour
{
	public GameObject dirMoveTutorial;

	public GameObject nonDirMoveTutorial;

	private void OnEnable()
	{
		dirMoveTutorial.SetActive(DewSave.profile.controls.enableDirMoveKeys);
		nonDirMoveTutorial.SetActive(!DewSave.profile.controls.enableDirMoveKeys);
	}
}
