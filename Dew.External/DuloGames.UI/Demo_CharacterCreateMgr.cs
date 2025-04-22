using UnityEngine;

namespace DuloGames.UI;

public class Demo_CharacterCreateMgr : MonoBehaviour
{
	private static Demo_CharacterCreateMgr m_Mgr;

	public static Demo_CharacterCreateMgr instance => m_Mgr;

	protected void Awake()
	{
		m_Mgr = this;
	}

	protected void OnDestroy()
	{
		m_Mgr = null;
	}
}
