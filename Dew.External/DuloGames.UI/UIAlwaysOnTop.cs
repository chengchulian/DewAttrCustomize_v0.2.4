using System;
using UnityEngine;

namespace DuloGames.UI;

[AddComponentMenu("UI/Always On Top", 8)]
[DisallowMultipleComponent]
public class UIAlwaysOnTop : MonoBehaviour, IComparable
{
	public const int ModalBoxOrder = 99996;

	public const int SelectFieldBlockerOrder = 99997;

	public const int SelectFieldOrder = 99998;

	public const int TooltipOrder = 99999;

	[SerializeField]
	private int m_Order;

	public int order
	{
		get
		{
			return m_Order;
		}
		set
		{
			m_Order = value;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj != null)
		{
			UIAlwaysOnTop comp = obj as UIAlwaysOnTop;
			if (comp != null)
			{
				return order.CompareTo(comp.order);
			}
		}
		return 1;
	}
}
