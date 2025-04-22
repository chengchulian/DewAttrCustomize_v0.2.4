using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_LavaLand_BossInfernus_MeteorSpawner : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_BossInfernus_MeteorSpawner _003C_003E4__this;

		private float _003Cdelay_003E5__2;

		private int _003Cj_003E5__3;

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
		public _003COnCreateSequenced_003Ed__3(int _003C_003E1__state)
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
			Ai_Mon_LavaLand_BossInfernus_MeteorSpawner ai_Mon_LavaLand_BossInfernus_MeteorSpawner = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0152;
			}
			_003C_003E1__state = -1;
			if (!ai_Mon_LavaLand_BossInfernus_MeteorSpawner.isServer)
			{
				return false;
			}
			_003Cdelay_003E5__2 = DewResources.GetByType<Ai_Mon_LavaLand_BossInfernus_Meteor>().damageDelay;
			if (ai_Mon_LavaLand_BossInfernus_MeteorSpawner.info.caster.section == null)
			{
				ai_Mon_LavaLand_BossInfernus_MeteorSpawner.Destroy();
				return false;
			}
			_003Cj_003E5__3 = 0;
			goto IL_0164;
			IL_0164:
			if (_003Cj_003E5__3 < ai_Mon_LavaLand_BossInfernus_MeteorSpawner.count)
			{
				Vector3 position;
				if (global::UnityEngine.Random.value < ai_Mon_LavaLand_BossInfernus_MeteorSpawner.targetedMeteorChance)
				{
					Hero target = Dew.SelectRandomAliveHero();
					position = AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.Range(0.25f, 1f), target, _003Cdelay_003E5__2) + global::UnityEngine.Random.insideUnitSphere.Flattened() * 1.5f;
				}
				else
				{
					if (!(ai_Mon_LavaLand_BossInfernus_MeteorSpawner.info.caster.section != null))
					{
						goto IL_0152;
					}
					position = ai_Mon_LavaLand_BossInfernus_MeteorSpawner.info.caster.section.GetAnyRandomNode() + global::UnityEngine.Random.insideUnitSphere.Flattened() * 1.5f;
				}
				position = Dew.GetPositionOnGround(position);
				ai_Mon_LavaLand_BossInfernus_MeteorSpawner.CreateAbilityInstance<Ai_Mon_LavaLand_BossInfernus_Meteor>(position, Quaternion.identity, new CastInfo(ai_Mon_LavaLand_BossInfernus_MeteorSpawner.info.caster, position));
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_BossInfernus_MeteorSpawner.interval);
				_003C_003E1__state = 1;
				return true;
			}
			ai_Mon_LavaLand_BossInfernus_MeteorSpawner.Destroy();
			return false;
			IL_0152:
			_003Cj_003E5__3++;
			goto IL_0164;
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

	public float targetedMeteorChance;

	public float interval;

	[NonSerialized]
	public int count;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__3))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
