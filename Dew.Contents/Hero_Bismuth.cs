using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;

public class Hero_Bismuth : Hero
{
	private static AbilityTargetValidator _hittableEnemy;

	private static AbilityTargetValidator _hittableNeutral;

	public Hero_Bismuth_BookController book
	{
		[CompilerGenerated]
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		private set
		{
		}
	}

	protected override void Awake()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	[Server]
	public void SpendAttack()
	{
	}

	public static List<Entity> GetTargetEntities(out ListReturnHandle<Entity> handle, Entity self, bool canBeNeutral, float range)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
