using UnityEngine;

public abstract class UI_InGame_FollowAndFaceHero_Base : MonoBehaviour
{
	public Vector3 worldOffset;

	public Vector2 screenOffset;

	public bool doPosition = true;

	public bool doRotation = true;

	public bool stayInSafeArea;

	private Quaternion _cv;

	protected abstract bool ShouldUpdate();

	protected abstract Vector3 GetPosition();

	public void UpdatePositionAndRotation(bool immediately = false)
	{
		if (!ShouldUpdate())
		{
			return;
		}
		Vector3 pos = GetPosition();
		if (doPosition)
		{
			Vector3 screenPos = Dew.mainCamera.WorldToScreenPoint(pos + worldOffset) + (Vector3)screenOffset;
			screenPos.Quantitize();
			base.transform.position = screenPos;
			if (stayInSafeArea)
			{
				ClampToSafeArea();
			}
		}
		if (doRotation)
		{
			Quaternion targetRot = GetTargetRotation(pos);
			if (immediately)
			{
				base.transform.rotation = targetRot;
				_cv = Quaternion.identity;
			}
			else
			{
				base.transform.rotation = QuaternionUtil.SmoothDamp(base.transform.rotation, targetRot, ref _cv, 0.2f);
			}
		}
	}

	private void ClampToSafeArea()
	{
		RectTransform rt = (RectTransform)base.transform;
		ArrayReturnHandle<Vector3> h0;
		Vector3[] myCorners = DewPool.GetArray(out h0, 4);
		ArrayReturnHandle<Vector3> h1;
		Vector3[] array = DewPool.GetArray(out h1, 4);
		rt.GetWorldCorners(myCorners);
		array[0] = new Vector3(70f, 70f, 0f);
		array[1] = new Vector3(70f, Screen.height - 70, 0f);
		array[2] = new Vector3(Screen.width - 70, Screen.height - 70, 0f);
		array[3] = new Vector3(Screen.width - 70, 70f, 0f);
		Vector3 safeLB = array[0];
		Vector3 safeRT = array[2];
		Vector3 tooltipLB = myCorners[0];
		Vector3 tooltipRT = myCorners[2];
		if (tooltipLB.x < safeLB.x)
		{
			base.transform.position += Vector3.right * (safeLB.x - tooltipLB.x);
		}
		else if (tooltipRT.x > safeRT.x)
		{
			base.transform.position += Vector3.left * (tooltipRT.x - safeRT.x);
		}
		if (tooltipLB.y < safeLB.y)
		{
			base.transform.position += Vector3.up * (safeLB.y - tooltipLB.y);
		}
		else if (tooltipRT.y > safeRT.y)
		{
			base.transform.position += Vector3.down * (tooltipRT.y - safeRT.y);
		}
	}

	private void LateUpdate()
	{
		UpdatePositionAndRotation();
	}

	private Quaternion GetTargetRotation(Vector3 pos)
	{
		if (!ShouldUpdate() || ManagerBase<CameraManager>.instance.focusedEntity == null)
		{
			return base.transform.rotation;
		}
		Transform cam = Dew.mainCamera.transform;
		Vector3 delta = pos - ManagerBase<CameraManager>.instance.focusedEntity.position;
		delta.y = 0f;
		Plane camPlane = new Plane(cam.forward.Flattened(), Vector3.zero);
		if (!camPlane.GetSide(delta))
		{
			float dist = camPlane.GetDistanceToPoint(delta);
			delta = camPlane.ClosestPointOnPlane(delta) + camPlane.normal * (dist * -1f);
		}
		if (delta.sqrMagnitude < 0.001f)
		{
			delta = Dew.mainCamera.transform.forward;
		}
		Quaternion targetRot = Quaternion.Slerp(Quaternion.identity, Quaternion.Inverse(cam.rotation) * Quaternion.LookRotation(delta, Vector3.up), 0.3f);
		return Quaternion.RotateTowards(Quaternion.identity, targetRot, 20f);
	}
}
