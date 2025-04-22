using UnityEngine;
using UnityEngine.UI;

public class UI_ItemIcon : MonoBehaviour
{
	public GameObject accObject;

	public Image accImage;

	public GameObject[] accTypeObjects;

	public GameObject nametagObject;

	public RectTransform nametagContainer;

	public GameObject emoteObject;

	public UI_StationaryEmote emote;

	private string _objectName;

	private Color _color;

	private GameObject _currentInstantiated;

	public string objectName
	{
		get
		{
			return _objectName;
		}
		set
		{
			if (Application.IsPlaying(this) && !(_objectName == value))
			{
				_objectName = value;
				Refresh();
			}
		}
	}

	public Color color
	{
		get
		{
			return _color;
		}
		set
		{
			if (Application.IsPlaying(this) && !(_color == value))
			{
				_color = value;
				Refresh();
			}
		}
	}

	private void Awake()
	{
		accObject.SetActive(value: false);
		nametagObject.SetActive(value: false);
		emoteObject.SetActive(value: false);
	}

	private void Refresh()
	{
		if (_currentInstantiated != null)
		{
			Object.Destroy(_currentInstantiated);
			_currentInstantiated = null;
		}
		if (objectName.StartsWith("Acc_"))
		{
			accObject.SetActive(value: true);
			nametagObject.SetActive(value: false);
			emoteObject.SetActive(value: false);
			Accessory prefab = DewResources.GetByName<Accessory>(objectName);
			if (prefab == null)
			{
				return;
			}
			accImage.sprite = prefab.previewImage;
			accImage.color = color;
			for (int i = 0; i < accTypeObjects.Length; i++)
			{
				accTypeObjects[i].SetActive(prefab.type == (AccType)i);
			}
		}
		if (objectName.StartsWith("Nametag_"))
		{
			accObject.SetActive(value: false);
			nametagObject.SetActive(value: true);
			emoteObject.SetActive(value: false);
			Nametag prefab2 = DewResources.GetByName<Nametag>(objectName);
			if (prefab2 == null)
			{
				return;
			}
			Nametag nt = Object.Instantiate(prefab2, nametagContainer);
			nt.Setup(nametagContainer, isIcon: true);
			Image[] componentsInChildren = nt.GetComponentsInChildren<Image>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].color *= color;
			}
			_currentInstantiated = nt.gameObject;
		}
		if (objectName.StartsWith("Emote_"))
		{
			accObject.SetActive(value: false);
			nametagObject.SetActive(value: false);
			emoteObject.SetActive(value: true);
			emote.Setup(objectName);
			Image[] componentsInChildren = emote.GetComponentsInChildren<Image>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].color *= color;
			}
			if (emote.currentEmoteInstance != null)
			{
				emote.currentEmoteInstance.transform.localScale *= 0.675f;
				((RectTransform)emote.currentEmoteInstance.transform).anchoredPosition = Vector2.zero;
			}
		}
	}
}
