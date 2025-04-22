using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UI_PlayTutorial_IntroActivateStatue : MonoBehaviour
{
	public GameObject fxFlash;

	private void OnEnable()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (true)
			{
				yield return new WaitForSeconds(2.65f);
				DewEffect.PlayNew(fxFlash);
				base.transform.DOKill(complete: true);
				base.transform.DOPunchScale(Vector3.one * 0.2f, 0.6f);
			}
		}
	}
}
