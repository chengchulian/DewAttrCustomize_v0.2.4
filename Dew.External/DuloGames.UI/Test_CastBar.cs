using System.Collections;
using UnityEngine;

namespace DuloGames.UI;

public class Test_CastBar : MonoBehaviour
{
	[SerializeField]
	private UICastBar m_CastBar;

	private UISpellInfo spell1;

	private UISpellInfo spell2;

	private void Start()
	{
		if (m_CastBar != null && UISpellDatabase.Instance != null)
		{
			spell1 = UISpellDatabase.Instance.Get(0);
			spell2 = UISpellDatabase.Instance.Get(2);
			StartCoroutine("StartTestRoutine");
		}
	}

	private IEnumerator StartTestRoutine()
	{
		yield return new WaitForSeconds(1f);
		m_CastBar.StartCasting(spell1, spell1.CastTime, Time.time + spell1.CastTime);
		yield return new WaitForSeconds(1f + spell1.CastTime);
		m_CastBar.StartCasting(spell2, spell2.CastTime, Time.time + spell2.CastTime);
		yield return new WaitForSeconds(spell2.CastTime * 0.75f);
		m_CastBar.Interrupt();
		StartCoroutine("StartTestRoutine");
	}
}
