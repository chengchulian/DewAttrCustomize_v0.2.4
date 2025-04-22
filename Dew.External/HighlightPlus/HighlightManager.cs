using System.Collections.Generic;
using UnityEngine;

namespace HighlightPlus;

[RequireComponent(typeof(HighlightEffect))]
[DefaultExecutionOrder(100)]
[HelpURL("https://kronnect.com/guides/highlight-plus-introduction/")]
public class HighlightManager : MonoBehaviour
{
	[Tooltip("Enables highlight when pointer is over this object.")]
	[SerializeField]
	private bool _highlightOnHover = true;

	public LayerMask layerMask = -1;

	public Camera raycastCamera;

	public RayCastSource raycastSource;

	[Tooltip("Minimum distance for target.")]
	public float minDistance;

	[Tooltip("Maximum distance for target. 0 = infinity")]
	public float maxDistance;

	[Tooltip("Blocks interaction if pointer is over an UI element")]
	public bool respectUI = true;

	[Tooltip("If the object will be selected by clicking with mouse or tapping on it.")]
	public bool selectOnClick;

	[Tooltip("Optional profile for objects selected by clicking on them")]
	public HighlightProfile selectedProfile;

	[Tooltip("Profile to use whtn object is selected and highlighted.")]
	public HighlightProfile selectedAndHighlightedProfile;

	[Tooltip("Automatically deselects other previously selected objects")]
	public bool singleSelection;

	[Tooltip("Toggles selection on/off when clicking object")]
	public bool toggle;

	[Tooltip("Keeps current selection when clicking outside of any selectable object")]
	public bool keepSelection = true;

	private HighlightEffect baseEffect;

	private HighlightEffect currentEffect;

	private Transform currentObject;

	private RaycastHit2D[] hitInfo2D;

	public static readonly List<HighlightEffect> selectedObjects = new List<HighlightEffect>();

	public static int lastTriggerFrame;

	private static HighlightManager _instance;

	public bool highlightOnHover
	{
		get
		{
			return _highlightOnHover;
		}
		set
		{
			if (_highlightOnHover != value)
			{
				_highlightOnHover = value;
				if (!_highlightOnHover && currentEffect != null)
				{
					Highlight(state: false);
				}
			}
		}
	}

	public static HighlightManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Misc.FindObjectOfType<HighlightManager>();
			}
			return _instance;
		}
	}

	public event OnObjectSelectionEvent OnObjectSelected;

	public event OnObjectSelectionEvent OnObjectUnSelected;

	public event OnObjectHighlightEvent OnObjectHighlightStart;

	public event OnObjectHighlightEvent OnObjectHighlightEnd;

	[RuntimeInitializeOnLoadMethod]
	private static void DomainReloadDisabledSupport()
	{
		selectedObjects.Clear();
		lastTriggerFrame = 0;
		_instance = null;
	}

	private void OnEnable()
	{
		currentObject = null;
		currentEffect = null;
		if (baseEffect == null)
		{
			baseEffect = GetComponent<HighlightEffect>();
			if (baseEffect == null)
			{
				baseEffect = base.gameObject.AddComponent<HighlightEffect>();
			}
		}
		raycastCamera = GetComponent<Camera>();
		if (raycastCamera == null)
		{
			raycastCamera = GetCamera();
			if (raycastCamera == null)
			{
				Debug.LogError("Highlight Manager: no camera found!");
			}
		}
		hitInfo2D = new RaycastHit2D[1];
		InputProxy.Init();
	}

	private void OnDisable()
	{
		SwitchesObject(null);
		internal_DeselectAll();
	}

	private void Update()
	{
		if (raycastCamera == null)
		{
			return;
		}
		Ray ray = ((raycastSource != 0) ? new Ray(raycastCamera.transform.position, raycastCamera.transform.forward) : raycastCamera.ScreenPointToRay(InputProxy.mousePosition));
		if (Physics.Raycast(ray, out var hitInfo, (maxDistance > 0f) ? maxDistance : raycastCamera.farClipPlane, layerMask) && Vector3.Distance(hitInfo.point, ray.origin) >= minDistance)
		{
			Transform t = hitInfo.collider.transform;
			if (t.GetComponent<HighlightTrigger>() != null)
			{
				return;
			}
			if (InputProxy.GetMouseButtonDown(0))
			{
				if (selectOnClick)
				{
					ToggleSelection(t, !toggle);
				}
			}
			else if (t != currentObject)
			{
				SwitchesObject(t);
			}
		}
		else if (Physics2D.GetRayIntersectionNonAlloc(ray, hitInfo2D, (maxDistance > 0f) ? maxDistance : raycastCamera.farClipPlane, layerMask) > 0 && Vector3.Distance(hitInfo2D[0].point, ray.origin) >= minDistance)
		{
			Transform t2 = hitInfo2D[0].collider.transform;
			if (t2.GetComponent<HighlightTrigger>() != null)
			{
				return;
			}
			if (InputProxy.GetMouseButtonDown(0))
			{
				if (selectOnClick)
				{
					ToggleSelection(t2, !toggle);
				}
			}
			else if (t2 != currentObject)
			{
				SwitchesObject(t2);
			}
		}
		else
		{
			if (selectOnClick && !keepSelection && InputProxy.GetMouseButtonDown(0) && lastTriggerFrame < Time.frameCount)
			{
				internal_DeselectAll();
			}
			SwitchesObject(null);
		}
	}

	private void SwitchesObject(Transform newObject)
	{
		if (currentEffect != null)
		{
			if (highlightOnHover)
			{
				Highlight(state: false);
			}
			currentEffect = null;
		}
		currentObject = newObject;
		if (newObject == null)
		{
			return;
		}
		HighlightTrigger ht = newObject.GetComponent<HighlightTrigger>();
		if (ht != null && ht.enabled)
		{
			return;
		}
		HighlightEffect otherEffect = newObject.GetComponent<HighlightEffect>();
		if (otherEffect == null)
		{
			HighlightEffect parentEffect = newObject.GetComponentInParent<HighlightEffect>();
			if (parentEffect != null && parentEffect.Includes(newObject))
			{
				currentEffect = parentEffect;
				if (highlightOnHover)
				{
					Highlight(state: true);
				}
				return;
			}
		}
		currentEffect = ((otherEffect != null) ? otherEffect : baseEffect);
		baseEffect.enabled = currentEffect == baseEffect;
		currentEffect.SetTarget(currentObject);
		if (highlightOnHover)
		{
			Highlight(state: true);
		}
	}

	private void ToggleSelection(Transform t, bool forceSelection)
	{
		HighlightEffect hb = t.GetComponent<HighlightEffect>();
		if (hb == null)
		{
			HighlightEffect parentEffect = t.GetComponentInParent<HighlightEffect>();
			if (parentEffect != null && parentEffect.Includes(t))
			{
				hb = parentEffect;
				if (hb.previousSettings == null)
				{
					hb.previousSettings = ScriptableObject.CreateInstance<HighlightProfile>();
				}
				hb.previousSettings.Save(hb);
			}
			else
			{
				hb = t.gameObject.AddComponent<HighlightEffect>();
				hb.previousSettings = ScriptableObject.CreateInstance<HighlightProfile>();
				hb.previousSettings.Save(baseEffect);
				hb.previousSettings.Load(hb);
			}
		}
		bool currentState = hb.isSelected;
		bool newState = forceSelection || !currentState;
		if (newState == currentState)
		{
			return;
		}
		if (newState)
		{
			if (this.OnObjectSelected != null && !this.OnObjectSelected(t.gameObject))
			{
				return;
			}
		}
		else if (this.OnObjectUnSelected != null && !this.OnObjectUnSelected(t.gameObject))
		{
			return;
		}
		if (singleSelection)
		{
			internal_DeselectAll();
		}
		currentEffect = hb;
		currentEffect.isSelected = newState;
		baseEffect.enabled = false;
		if (currentEffect.isSelected)
		{
			if (currentEffect.previousSettings == null)
			{
				currentEffect.previousSettings = ScriptableObject.CreateInstance<HighlightProfile>();
			}
			hb.previousSettings.Save(hb);
			if (!selectedObjects.Contains(currentEffect))
			{
				selectedObjects.Add(currentEffect);
			}
		}
		else
		{
			if (currentEffect.previousSettings != null)
			{
				currentEffect.previousSettings.Load(hb);
			}
			if (selectedObjects.Contains(currentEffect))
			{
				selectedObjects.Remove(currentEffect);
			}
		}
		Highlight(newState);
	}

	private void Highlight(bool state)
	{
		if (state)
		{
			if (!currentEffect.highlighted && this.OnObjectHighlightStart != null && currentEffect.target != null && !this.OnObjectHighlightStart(currentEffect.target.gameObject))
			{
				return;
			}
		}
		else if (currentEffect.highlighted && this.OnObjectHighlightEnd != null && currentEffect.target != null)
		{
			this.OnObjectHighlightEnd(currentEffect.target.gameObject);
		}
		if (selectOnClick || currentEffect.isSelected)
		{
			if (currentEffect.isSelected)
			{
				if (state && selectedAndHighlightedProfile != null)
				{
					selectedAndHighlightedProfile.Load(currentEffect);
				}
				else if (selectedProfile != null)
				{
					selectedProfile.Load(currentEffect);
				}
				else
				{
					currentEffect.previousSettings.Load(currentEffect);
				}
				if (currentEffect.highlighted && currentEffect.fading != HighlightEffect.FadingState.FadingOut)
				{
					currentEffect.UpdateMaterialProperties();
				}
				else
				{
					currentEffect.SetHighlighted(state: true);
				}
				return;
			}
			if (!highlightOnHover)
			{
				currentEffect.SetHighlighted(state: false);
				return;
			}
		}
		currentEffect.SetHighlighted(state);
	}

	public static Camera GetCamera()
	{
		Camera raycastCamera = Camera.main;
		if (raycastCamera == null)
		{
			raycastCamera = Misc.FindObjectOfType<Camera>();
		}
		return raycastCamera;
	}

	private void internal_DeselectAll()
	{
		foreach (HighlightEffect hb in selectedObjects)
		{
			if (hb != null && hb.gameObject != null && (this.OnObjectUnSelected == null || this.OnObjectUnSelected(hb.gameObject)))
			{
				hb.RestorePreviousHighlightEffectSettings();
				hb.isSelected = false;
				hb.SetHighlighted(state: false);
			}
		}
		selectedObjects.Clear();
	}

	public static void DeselectAll()
	{
		foreach (HighlightEffect hb in selectedObjects)
		{
			if (hb != null && hb.gameObject != null)
			{
				hb.isSelected = false;
				if (hb.highlighted && _instance != null)
				{
					_instance.Highlight(state: false);
				}
				else
				{
					hb.SetHighlighted(state: false);
				}
			}
		}
		selectedObjects.Clear();
	}

	public void SelectObject(Transform t)
	{
		ToggleSelection(t, forceSelection: true);
	}

	public void ToggleObject(Transform t)
	{
		ToggleSelection(t, forceSelection: false);
	}

	public void UnselectObject(Transform t)
	{
		if (!(t == null))
		{
			HighlightEffect hb = t.GetComponent<HighlightEffect>();
			if (!(hb == null) && hb.isSelected)
			{
				ToggleSelection(t, forceSelection: false);
			}
		}
	}
}
