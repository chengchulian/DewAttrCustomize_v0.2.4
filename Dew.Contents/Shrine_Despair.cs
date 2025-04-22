using UnityEngine;

public class Shrine_Despair : Shrine, IPlayerPathableArea
{
	public bool isOpen;

	public Transform targetPos;

	private Vector3 _destination;

	Vector3 IPlayerPathableArea.pathablePosition
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected override void OnCreate()
	{
	}

	protected override bool OnUse(Entity entity)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	public Transform GetPathableAreaStart()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
