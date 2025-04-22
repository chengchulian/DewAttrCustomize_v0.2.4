using System.Collections.Generic;
using UnityEngine;

public class UI_Tooltip_Items : UI_Tooltip_BaseObj
{
	public UI_ItemIcon itemPrefab;

	public Transform itemsParent;

	private List<UI_ItemIcon> _instantiated = new List<UI_ItemIcon>();

	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is string[] items))
		{
			PrepareItemIcons(0);
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		PrepareItemIcons(items.Length);
		for (int i = 0; i < _instantiated.Count; i++)
		{
			_instantiated[i].objectName = items[i];
			_instantiated[i].color = Color.white;
			_instantiated[i].transform.localScale = Vector3.one * 0.8f;
		}
	}

	private void PrepareItemIcons(int count)
	{
		while (_instantiated.Count > count)
		{
			Object.Destroy(_instantiated[_instantiated.Count - 1].gameObject);
			_instantiated.RemoveAt(_instantiated.Count - 1);
		}
		while (_instantiated.Count < count)
		{
			UI_ItemIcon item = Object.Instantiate(itemPrefab, itemsParent);
			_instantiated.Add(item);
		}
	}
}
