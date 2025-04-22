using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AccList_PreviewWindow : MonoBehaviour
{
	public GameObject shownObject;

	public Camera previewRenderer;

	public RawImage rawImage;

	public GameObject previewSceneObject;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI descText;

	private UI_ToggleGroup _group;

	private Accessory _currentAcc;

	private RenderTexture _rt;

	private void Start()
	{
		_group = SingletonBehaviour<UI_AccList>.instance.GetComponentInChildren<UI_ToggleGroup>();
		_group.onCurrentIndexChanged.AddListener(UpdatePreview);
		UpdatePreview(-1);
	}

	private void UpdatePreview(int index)
	{
		if (_currentAcc != null)
		{
			Object.Destroy(_currentAcc.gameObject);
			_currentAcc = null;
		}
		if (index < 0 || index >= SingletonBehaviour<UI_AccList>.instance.shownItems.Count || !DewSave.profile.accessories[SingletonBehaviour<UI_AccList>.instance.shownItems[index].accName].isUnlocked)
		{
			ShowEmpty();
			return;
		}
		UI_AccList_Item item = SingletonBehaviour<UI_AccList>.instance.shownItems[index];
		Accessory prefab = DewResources.GetByName<Accessory>(item.accName);
		_currentAcc = Object.Instantiate(prefab, null);
		_currentAcc.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Preview"));
		_currentAcc.transform.rotation = Quaternion.identity;
		shownObject.SetActive(value: true);
		Quaternion camRot = Quaternion.LookRotation(_currentAcc.transform.rotation * RuntimePreviewGenerator.PreviewDirection, _currentAcc.transform.up);
		RuntimePreviewGenerator.CalculateBounds(_currentAcc.transform, shouldIgnoreParticleSystems: true, camRot, out var bounds);
		previewRenderer.aspect = (float)_rt.width / (float)_rt.height;
		previewRenderer.transform.rotation = camRot;
		RuntimePreviewGenerator.CalculateCameraPosition(previewRenderer, bounds, _currentAcc.previewPadding);
		previewRenderer.nearClipPlane = 0.001f;
		previewRenderer.orthographic = true;
		if (_currentAcc.previewCustomCenter != null)
		{
			previewRenderer.transform.LookAt(_currentAcc.previewCustomCenter);
		}
		if (previewSceneObject != null)
		{
			previewSceneObject.transform.position = ((_currentAcc.previewCustomCenter != null) ? _currentAcc.previewCustomCenter.position : _currentAcc.transform.position);
			previewSceneObject.transform.rotation = previewRenderer.transform.rotation;
			previewSceneObject.transform.localScale /= previewSceneObject.transform.lossyScale.x;
		}
		nameText.text = DewLocalization.GetUIValue(item.accName + "_Name");
		descText.text = DewLocalization.GetUIValue(item.accName + "_Description");
	}

	private void ShowEmpty()
	{
		shownObject.SetActive(value: false);
		if (_currentAcc != null)
		{
			Object.Destroy(_currentAcc.gameObject);
			_currentAcc = null;
		}
	}

	private void Update()
	{
		Rect rect = rawImage.rectTransform.GetScreenSpaceRect();
		if (_rt == null || _rt.width != Mathf.RoundToInt(rect.width) || _rt.height != Mathf.RoundToInt(rect.height))
		{
			if (_rt != null)
			{
				Object.Destroy(_rt);
			}
			_rt = new RenderTexture(Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height), 24, RenderTextureFormat.ARGB32);
			previewRenderer.targetTexture = _rt;
			rawImage.texture = _rt;
		}
		if (_currentAcc != null)
		{
			Transform center = ((_currentAcc.previewCustomCenter != null) ? _currentAcc.previewCustomCenter : _currentAcc.transform);
			_currentAcc.transform.RotateAround(center.position, center.up, 80f * Time.deltaTime);
		}
	}
}
