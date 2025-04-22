using UnityEngine;

public class RoomMod_GravityTraining : RoomModifierBase
{
	private AudioSource _audioSource;

	public override void OnStartClient()
	{
		base.OnStartClient();
		_audioSource = ManagerBase<MusicManager>.instance.GetComponent<AudioSource>();
		_audioSource.pitch = 0.8f;
		if (!base.isServer)
		{
			return;
		}
		ModifyEntities(delegate(Entity e)
		{
			if (e is Hero || e is Monster)
			{
				e.CreateStatusEffect<Se_GravityTraining>(e, new CastInfo(e));
			}
		}, delegate(Entity e)
		{
			if (e.Status.TryGetStatusEffect<Se_GravityTraining>(out var effect))
			{
				effect.Destroy();
			}
		});
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		_audioSource.pitch = 1f;
	}

	private void MirrorProcessed()
	{
	}
}
