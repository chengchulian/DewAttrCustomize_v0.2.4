using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_DivineAnimal_Stomp : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_DivineAnimal_Stomp _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003COnCreateSequenced_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Ai_Mon_Ink_DivineAnimal_Stomp ai_Mon_Ink_DivineAnimal_Stomp = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_DivineAnimal_Stomp.isServer)
				{
					return false;
				}
				ai_Mon_Ink_DivineAnimal_Stomp.DestroyOnDeath(ai_Mon_Ink_DivineAnimal_Stomp.info.caster);
				ai_Mon_Ink_DivineAnimal_Stomp.range.transform.position = Dew.GetPositionOnGround(ai_Mon_Ink_DivineAnimal_Stomp.range.transform.position);
				ai_Mon_Ink_DivineAnimal_Stomp.FxPlayNewNetworked(ai_Mon_Ink_DivineAnimal_Stomp.telegraph);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_DivineAnimal_Stomp.spawnDuration);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				ai_Mon_Ink_DivineAnimal_Stomp.FxPlayNetworked(ai_Mon_Ink_DivineAnimal_Stomp.landEffect);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Ink_DivineAnimal_Stomp.range.GetEntities(out handle, ai_Mon_Ink_DivineAnimal_Stomp.hittable, ai_Mon_Ink_DivineAnimal_Stomp.info.caster);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					entity.Visual.KnockUp(ai_Mon_Ink_DivineAnimal_Stomp.knockupStrengh, isFriendly: false);
					ai_Mon_Ink_DivineAnimal_Stomp.knockback.ApplyWithOrigin(ai_Mon_Ink_DivineAnimal_Stomp.info.caster.agentPosition, entity);
					ai_Mon_Ink_DivineAnimal_Stomp.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_DivineAnimal_Stomp.dmgFactor).Dispatch(entity);
				}
				handle.Return();
				ai_Mon_Ink_DivineAnimal_Stomp.Destroy();
				return false;
			}
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public GameObject telegraph;

	public float spawnDuration;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public ScalingValue dmgFactor;

	public float knockupStrengh;

	public Knockback knockback;

	public GameObject landEffect;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__8))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
