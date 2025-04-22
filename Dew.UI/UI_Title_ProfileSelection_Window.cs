using System;
using UnityEngine;

public abstract class UI_Title_ProfileSelection_Window : MonoBehaviour
{
	public UI_Title_ProfileSelection parent { get; private set; }

	public abstract UI_Title_ProfileSelection.StateType GetMyState();

	protected virtual void Awake()
	{
		parent = GetComponentInParent<UI_Title_ProfileSelection>();
	}

	protected virtual void Start()
	{
		UI_Title_ProfileSelection uI_Title_ProfileSelection = parent;
		uI_Title_ProfileSelection.onStateChanged = (Action<UI_Title_ProfileSelection.StateType>)Delegate.Combine(uI_Title_ProfileSelection.onStateChanged, new Action<UI_Title_ProfileSelection.StateType>(OnStateChanged));
		OnStateChanged(parent.state);
	}

	private void OnStateChanged(UI_Title_ProfileSelection.StateType obj)
	{
		base.gameObject.SetActive(obj == GetMyState());
	}
}
