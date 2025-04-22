using System.Collections.Generic;
using UnityEngine;

public class presGUI : MonoBehaviour
{
	public GameObject animCharacter;

	public List<AnimationClip> ListeAnimations;

	private void OnGUI()
	{
		Animator anim = animCharacter.GetComponent<Animator>();
		for (int a = 0; a < ListeAnimations.Count; a++)
		{
			if (GUI.Button(new Rect(50f, 50 + 60 * a, 200f, 50f), ListeAnimations[a].name))
			{
				animCharacter.GetComponent<dontmove>().resetPos();
				anim.Play(ListeAnimations[a].name);
			}
		}
	}
}
