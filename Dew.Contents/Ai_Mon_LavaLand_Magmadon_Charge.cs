using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_LavaLand_Magmadon_Charge : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_Magmadon_Charge _003C_003E4__this;

		private float _003CstartAngle_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003COnCreateSequenced_003Ed__7(int _003C_003E1__state)
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
			Ai_Mon_LavaLand_Magmadon_Charge ai_Mon_LavaLand_Magmadon_Charge = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ai_Mon_LavaLand_Magmadon_Charge.isServer)
				{
					return false;
				}
				ai_Mon_LavaLand_Magmadon_Charge.DestroyOnDeath(ai_Mon_LavaLand_Magmadon_Charge.info.caster);
				ai_Mon_LavaLand_Magmadon_Charge.range.transform.position = ai_Mon_LavaLand_Magmadon_Charge.info.caster.agentPosition;
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_LavaLand_Magmadon_Charge.range.GetEntities(out handle, ai_Mon_LavaLand_Magmadon_Charge.tvDefaultHarmfulEffectTargets);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_LavaLand_Magmadon_Charge.FxPlayNewNetworked(ai_Mon_LavaLand_Magmadon_Charge.fxHit, entity);
					ai_Mon_LavaLand_Magmadon_Charge.CreateDamage(DamageData.SourceType.Default, ai_Mon_LavaLand_Magmadon_Charge.dmgFactor).SetOriginPosition(ai_Mon_LavaLand_Magmadon_Charge.info.caster.agentPosition).SetElemental(ElementalType.Fire)
						.Dispatch(entity);
					ai_Mon_LavaLand_Magmadon_Charge.knockback.ApplyWithOrigin(ai_Mon_LavaLand_Magmadon_Charge.info.caster.agentPosition, entity);
				}
				handle.Return();
				_003CstartAngle_003E5__2 = global::UnityEngine.Random.Range(0f, 360f);
				_003Ci_003E5__3 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < ai_Mon_LavaLand_Magmadon_Charge.projectileCount)
			{
				float num2 = 360f / (float)ai_Mon_LavaLand_Magmadon_Charge.projectileCount * (float)_003Ci_003E5__3;
				float y = _003CstartAngle_003E5__2 + num2;
				Vector3 position = ai_Mon_LavaLand_Magmadon_Charge.info.caster.agentPosition + Quaternion.Euler(0f, y, 0f) * ai_Mon_LavaLand_Magmadon_Charge.info.forward * ai_Mon_LavaLand_Magmadon_Charge.projectileDistance;
				position = Dew.GetPositionOnGround(position);
				ai_Mon_LavaLand_Magmadon_Charge.CreateAbilityInstance<Ai_Mon_LavaLand_Magmadon_Charge_Projectile>(ai_Mon_LavaLand_Magmadon_Charge.info.caster.agentPosition, null, new CastInfo(ai_Mon_LavaLand_Magmadon_Charge.info.caster, position));
				ai_Mon_LavaLand_Magmadon_Charge.FxPlayNewNetworked(ai_Mon_LavaLand_Magmadon_Charge.fxTelegraph, position, null);
				_003C_003E2__current = new SI.WaitForSeconds(0.1f);
				_003C_003E1__state = 1;
				return true;
			}
			ai_Mon_LavaLand_Magmadon_Charge.Destroy();
			return false;
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

	public int projectileCount;

	public float projectileDistance;

	public ScalingValue dmgFactor;

	public Knockback knockback;

	public DewCollider range;

	public GameObject fxHit;

	public GameObject fxTelegraph;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__7))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
