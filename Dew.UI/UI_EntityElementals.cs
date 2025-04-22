using TMPro;
using UnityEngine;

public class UI_EntityElementals : LogicBehaviour
{
	private UI_EntityProvider _provider;

	public GameObject fireObj;

	public GameObject coldObj;

	public GameObject lightObj;

	public GameObject darkObj;

	public TextMeshProUGUI fireText;

	public TextMeshProUGUI lightText;

	public TextMeshProUGUI darkText;

	public Entity target => _provider.target;

	protected virtual void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		fireObj.SetActive(value: false);
		coldObj.SetActive(value: false);
		lightObj.SetActive(value: false);
		darkObj.SetActive(value: false);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (target == null)
		{
			fireObj.SetActive(value: false);
			coldObj.SetActive(value: false);
			lightObj.SetActive(value: false);
			darkObj.SetActive(value: false);
			return;
		}
		coldObj.SetActive(target.Status.HasElemental(ElementalType.Cold));
		int fireStack = target.Status.GetElementalStack(ElementalType.Fire);
		int lightStack = target.Status.GetElementalStack(ElementalType.Light);
		int voidStack = target.Status.GetElementalStack(ElementalType.Dark);
		lightObj.SetActive(lightStack > 0);
		darkObj.SetActive(voidStack > 0);
		fireObj.SetActive(fireStack > 0);
		if (lightStack > 0)
		{
			lightText.text = lightStack.ToString();
		}
		if (voidStack > 0)
		{
			darkText.text = voidStack.ToString();
		}
		if (fireStack > 0)
		{
			fireText.text = fireStack.ToString();
		}
	}
}
