using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HighlightPlus;

[RequireComponent(typeof(HighlightEffect))]
[ExecuteInEditMode]
[HelpURL("https://kronnect.com/guides/highlight-plus-introduction/")]
public class HighlightTrigger : MonoBehaviour
{
	[Tooltip("Enables highlight when pointer is over this object.")]
	public bool highlightOnHover = true;

	[Tooltip("Used to trigger automatic highlighting including children objects.")]
	public TriggerMode triggerMode = TriggerMode.RaycastOnThisObjectAndChildren;

	public Camera raycastCamera;

	public RayCastSource raycastSource;

	public LayerMask raycastLayerMask = -1;

	[Tooltip("Minimum distance for target.")]
	public float minDistance;

	[Tooltip("Maximum distance for target. 0 = infinity")]
	public float maxDistance;

	[Tooltip("Blocks interaction if pointer is over an UI element")]
	public bool respectUI = true;

	public LayerMask volumeLayerMask;

	private const int MAX_RAYCAST_HITS = 100;

	[Tooltip("If the object will be selected by clicking with mouse or tapping on it.")]
	public bool selectOnClick;

	[Tooltip("Profile to use when object is selected by clicking on it.")]
	public HighlightProfile selectedProfile;

	[Tooltip("Profile to use whtn object is selected and highlighted.")]
	public HighlightProfile selectedAndHighlightedProfile;

	[Tooltip("Automatically deselects any other selected object prior selecting this one")]
	public bool singleSelection;

	[Tooltip("Toggles selection on/off when clicking object")]
	public bool toggle;

	[Tooltip("Keeps current selection when clicking outside of any selectable object")]
	public bool keepSelection = true;

	[NonSerialized]
	public Collider[] colliders;

	[NonSerialized]
	public Collider2D[] colliders2D;

	private global::UnityEngine.Object currentCollider;

	private static RaycastHit[] hits;

	private static RaycastHit2D[] hits2D;

	private HighlightEffect hb;

	private TriggerMode currentTriggerMode;

	public bool hasColliders
	{
		get
		{
			if (colliders != null)
			{
				return colliders.Length != 0;
			}
			return false;
		}
	}

	public bool hasColliders2D
	{
		get
		{
			if (colliders2D != null)
			{
				return colliders2D.Length != 0;
			}
			return false;
		}
	}

	public HighlightEffect highlightEffect => hb;

	public event OnObjectSelectionEvent OnObjectSelected;

	public event OnObjectSelectionEvent OnObjectUnSelected;

	public event OnObjectHighlightEvent OnObjectHighlightStart;

	public event OnObjectHighlightEvent OnObjectHighlightEnd;

	[RuntimeInitializeOnLoadMethod]
	private static void DomainReloadDisabledSupport()
	{
		HighlightManager.selectedObjects.Clear();
	}

	private void OnEnable()
	{
		Init();
	}

	private void OnValidate()
	{
		if (currentTriggerMode != triggerMode)
		{
			UpdateTriggers();
		}
	}

	private void UpdateTriggers()
	{
		currentTriggerMode = triggerMode;
		if (currentTriggerMode != TriggerMode.RaycastOnThisObjectAndChildren)
		{
			return;
		}
		colliders = GetComponentsInChildren<Collider>();
		colliders2D = GetComponentsInChildren<Collider2D>();
		if (hits == null || hits.Length != 100)
		{
			hits = new RaycastHit[100];
		}
		if (hits2D == null || hits2D.Length != 100)
		{
			hits2D = new RaycastHit2D[100];
		}
		if (Application.isPlaying)
		{
			StopAllCoroutines();
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(DoRayCast());
			}
		}
	}

	public void Init()
	{
		if (raycastCamera == null)
		{
			raycastCamera = HighlightManager.GetCamera();
		}
		UpdateTriggers();
		if (hb == null)
		{
			hb = GetComponent<HighlightEffect>();
		}
		InputProxy.Init();
	}

	private void Start()
	{
		UpdateTriggers();
		if (triggerMode == TriggerMode.RaycastOnThisObjectAndChildren)
		{
			if (raycastCamera == null)
			{
				raycastCamera = HighlightManager.GetCamera();
				if (raycastCamera == null)
				{
					Debug.LogError("Highlight Trigger on " + base.gameObject.name + ": no camera found!");
				}
			}
		}
		else if (GetComponent<Collider>() == null && GetComponent<MeshFilter>() != null)
		{
			base.gameObject.AddComponent<MeshCollider>();
		}
	}

	private IEnumerator DoRayCast()
	{
		yield return null;
		WaitForEndOfFrame w = new WaitForEndOfFrame();
		while (triggerMode == TriggerMode.RaycastOnThisObjectAndChildren)
		{
			if (raycastCamera == null)
			{
				yield return null;
				continue;
			}
			bool hit = false;
			Ray ray = ((raycastSource != 0) ? new Ray(raycastCamera.transform.position, raycastCamera.transform.forward) : raycastCamera.ScreenPointToRay(InputProxy.mousePosition));
			bool isMouseButonDown = InputProxy.GetMouseButtonDown(0);
			if (hasColliders2D)
			{
				int hitCount = ((!(maxDistance > 0f)) ? Physics2D.GetRayIntersectionNonAlloc(ray, hits2D, float.MaxValue, raycastLayerMask) : Physics2D.GetRayIntersectionNonAlloc(ray, hits2D, maxDistance, raycastLayerMask));
				for (int k = 0; k < hitCount; k++)
				{
					if (Vector3.Distance(hits2D[k].point, ray.origin) < minDistance)
					{
						continue;
					}
					Collider2D theCollider = hits2D[k].collider;
					int colliders2DCount = colliders2D.Length;
					for (int c = 0; c < colliders2DCount; c++)
					{
						if (colliders2D[c] == theCollider)
						{
							hit = true;
							if (selectOnClick && isMouseButonDown)
							{
								ToggleSelection();
								break;
							}
							if (theCollider != currentCollider)
							{
								SwitchCollider(theCollider);
								k = hitCount;
								break;
							}
						}
					}
				}
			}
			if (hasColliders)
			{
				int hitCount = ((!(maxDistance > 0f)) ? Physics.RaycastNonAlloc(ray, hits, float.MaxValue, raycastLayerMask) : Physics.RaycastNonAlloc(ray, hits, maxDistance, raycastLayerMask));
				for (int i = 0; i < hitCount; i++)
				{
					if (Vector3.Distance(hits[i].point, ray.origin) < minDistance)
					{
						continue;
					}
					Collider theCollider2 = hits[i].collider;
					int collidersCount = colliders.Length;
					for (int j = 0; j < collidersCount; j++)
					{
						if (colliders[j] == theCollider2)
						{
							hit = true;
							if (selectOnClick && isMouseButonDown)
							{
								ToggleSelection();
								break;
							}
							if (theCollider2 != currentCollider)
							{
								SwitchCollider(theCollider2);
								i = hitCount;
								break;
							}
						}
					}
				}
			}
			if (!hit && currentCollider != null)
			{
				SwitchCollider(null);
			}
			if (selectOnClick && isMouseButonDown && !keepSelection && !hit)
			{
				yield return w;
				if (HighlightManager.lastTriggerFrame < Time.frameCount)
				{
					HighlightManager.DeselectAll();
				}
			}
			yield return null;
		}
	}

	private void SwitchCollider(global::UnityEngine.Object newCollider)
	{
		if (highlightOnHover || hb.isSelected)
		{
			currentCollider = newCollider;
			if (currentCollider != null)
			{
				Highlight(state: true);
			}
			else
			{
				Highlight(state: false);
			}
		}
	}

	private bool CanInteract()
	{
		if (!respectUI)
		{
			return true;
		}
		EventSystem es = EventSystem.current;
		if (es == null)
		{
			return true;
		}
		if (Application.isMobilePlatform && InputProxy.touchCount > 0 && es.IsPointerOverGameObject(InputProxy.GetFingerIdFromTouch(0)))
		{
			return false;
		}
		if (es.IsPointerOverGameObject(-1))
		{
			return false;
		}
		return true;
	}

	private void OnMouseDown()
	{
		if (base.isActiveAndEnabled && triggerMode == TriggerMode.ColliderEventsOnlyOnThisObject && CanInteract())
		{
			if (selectOnClick && InputProxy.GetMouseButtonDown(0))
			{
				ToggleSelection();
			}
			else
			{
				Highlight(state: true);
			}
		}
	}

	private void OnMouseEnter()
	{
		if (base.isActiveAndEnabled && triggerMode == TriggerMode.ColliderEventsOnlyOnThisObject && CanInteract())
		{
			Highlight(state: true);
		}
	}

	private void OnMouseExit()
	{
		if (base.isActiveAndEnabled && triggerMode == TriggerMode.ColliderEventsOnlyOnThisObject && CanInteract())
		{
			Highlight(state: false);
		}
	}

	private void Highlight(bool state)
	{
		if (state)
		{
			if (!hb.highlighted && this.OnObjectHighlightStart != null && hb.target != null && !this.OnObjectHighlightStart(hb.target.gameObject))
			{
				return;
			}
		}
		else if (hb.highlighted && this.OnObjectHighlightEnd != null && hb.target != null)
		{
			this.OnObjectHighlightEnd(hb.target.gameObject);
		}
		if (selectOnClick || hb.isSelected)
		{
			if (hb.isSelected)
			{
				if (state && selectedAndHighlightedProfile != null)
				{
					selectedAndHighlightedProfile.Load(hb);
				}
				else if (selectedProfile != null)
				{
					selectedProfile.Load(hb);
				}
				else
				{
					hb.previousSettings.Load(hb);
				}
				if (hb.highlighted)
				{
					hb.UpdateMaterialProperties();
				}
				else
				{
					hb.SetHighlighted(state: true);
				}
				return;
			}
			if (!highlightOnHover)
			{
				hb.SetHighlighted(state: false);
				return;
			}
		}
		hb.SetHighlighted(state);
	}

	private void ToggleSelection()
	{
		HighlightManager.lastTriggerFrame = Time.frameCount;
		bool newState = !toggle || !hb.isSelected;
		if (newState)
		{
			if (this.OnObjectSelected != null && !this.OnObjectSelected(base.gameObject))
			{
				return;
			}
		}
		else if (this.OnObjectUnSelected != null && !this.OnObjectUnSelected(base.gameObject))
		{
			return;
		}
		if (singleSelection && newState)
		{
			HighlightManager.DeselectAll();
		}
		hb.isSelected = newState;
		if (newState && !HighlightManager.selectedObjects.Contains(hb))
		{
			HighlightManager.selectedObjects.Add(hb);
		}
		else if (!newState && HighlightManager.selectedObjects.Contains(hb))
		{
			HighlightManager.selectedObjects.Remove(hb);
		}
		if (hb.isSelected)
		{
			if (hb.previousSettings == null)
			{
				hb.previousSettings = ScriptableObject.CreateInstance<HighlightProfile>();
			}
			hb.previousSettings.Save(hb);
		}
		else
		{
			hb.RestorePreviousHighlightEffectSettings();
		}
		Highlight(state: true);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (triggerMode == TriggerMode.Volume && ((int)volumeLayerMask & (1 << other.gameObject.layer)) != 0)
		{
			Highlight(state: true);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (triggerMode == TriggerMode.Volume && ((int)volumeLayerMask & (1 << other.gameObject.layer)) != 0)
		{
			Highlight(state: false);
		}
	}
}
