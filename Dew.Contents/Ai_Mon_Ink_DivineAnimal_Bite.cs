using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_DivineAnimal_Bite : AbilityInstance
{
	public struct Ad_CheckBiteDuplication
	{
		public float LastHitTime;
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_DivineAnimal_Bite _003C_003E4__this;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

		private Entity _003Cent_003E5__4;

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
		public _003COnCreateSequenced_003Ed__20(int _003C_003E1__state)
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
			Ai_Mon_Ink_DivineAnimal_Bite ai_Mon_Ink_DivineAnimal_Bite = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_DivineAnimal_Bite.isServer)
				{
					return false;
				}
				ai_Mon_Ink_DivineAnimal_Bite.DestroyOnDeath(ai_Mon_Ink_DivineAnimal_Bite.info.caster);
				ai_Mon_Ink_DivineAnimal_Bite._entsHit = new List<Entity>();
				ai_Mon_Ink_DivineAnimal_Bite._originPos = ai_Mon_Ink_DivineAnimal_Bite.info.caster.agentPosition;
				ai_Mon_Ink_DivineAnimal_Bite.info.caster.Control.StartDaze(ai_Mon_Ink_DivineAnimal_Bite.chargeDuration);
				ai_Mon_Ink_DivineAnimal_Bite.info.caster.Control.StartDisplacement(new DispByDestination
				{
					destination = ai_Mon_Ink_DivineAnimal_Bite.info.caster.position + ai_Mon_Ink_DivineAnimal_Bite.info.forward * ai_Mon_Ink_DivineAnimal_Bite.length,
					duration = ai_Mon_Ink_DivineAnimal_Bite.chargeDuration,
					ease = ai_Mon_Ink_DivineAnimal_Bite.ease,
					isFriendly = true,
					rotateForward = true,
					canGoOverTerrain = false
				});
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Ink_DivineAnimal_Bite.range.GetEntities(out handle, ai_Mon_Ink_DivineAnimal_Bite.hittable, ai_Mon_Ink_DivineAnimal_Bite.info.caster);
				Vector3 end = ai_Mon_Ink_DivineAnimal_Bite._originPos + ai_Mon_Ink_DivineAnimal_Bite.info.forward * (ai_Mon_Ink_DivineAnimal_Bite.fowardDistance + ai_Mon_Ink_DivineAnimal_Bite.length);
				Vector3 validAgentDestination_Closest = Dew.GetValidAgentDestination_Closest(ai_Mon_Ink_DivineAnimal_Bite.info.caster.position, end);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					if (ai_Mon_Ink_DivineAnimal_Bite._entsHit.Contains(entity))
					{
						continue;
					}
					if (entity.HasData<Ad_CheckBiteDuplication>())
					{
						if (Time.time - entity.GetData<Ad_CheckBiteDuplication>().LastHitTime < ai_Mon_Ink_DivineAnimal_Bite.chainStunPreventTime)
						{
							continue;
						}
						entity.RemoveData<Ad_CheckBiteDuplication>();
					}
					entities[i].Control.StartDaze(ai_Mon_Ink_DivineAnimal_Bite.displaceDuration);
					entities[i].Control.StartDisplacement(new DispByDestination
					{
						canGoOverTerrain = false,
						affectedByMovementSpeed = false,
						destination = validAgentDestination_Closest,
						duration = ai_Mon_Ink_DivineAnimal_Bite.displaceDuration,
						ease = ai_Mon_Ink_DivineAnimal_Bite.ease,
						isFriendly = false,
						rotateForward = false,
						isCanceledByCC = false
					});
					if (entity != null)
					{
						ai_Mon_Ink_DivineAnimal_Bite.FxPlayNetworked(ai_Mon_Ink_DivineAnimal_Bite.catchSound);
						ai_Mon_Ink_DivineAnimal_Bite.CreateBasicEffect(entity, new StunEffect(), ai_Mon_Ink_DivineAnimal_Bite.stunDuration);
						ai_Mon_Ink_DivineAnimal_Bite._entsHit.Add(entity);
						if (!entity.HasData<Ad_CheckBiteDuplication>())
						{
							entity.AddData(new Ad_CheckBiteDuplication
							{
								LastHitTime = Time.time
							});
						}
					}
				}
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_DivineAnimal_Bite.chargeDuration + 0.001f);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (ai_Mon_Ink_DivineAnimal_Bite._entsHit.Count == 0)
				{
					ai_Mon_Ink_DivineAnimal_Bite.info.caster.Animation.StopAbilityAnimation(ai_Mon_Ink_DivineAnimal_Bite.firstTrigger.currentConfig.endAnim);
					ai_Mon_Ink_DivineAnimal_Bite.info.caster.Animation.PlayAbilityAnimation(ai_Mon_Ink_DivineAnimal_Bite.failedAnim);
					ai_Mon_Ink_DivineAnimal_Bite.firstTrigger.ApplyCooldownReductionByRatio(0.5f);
					ai_Mon_Ink_DivineAnimal_Bite.Destroy();
				}
				_003Ccount_003E5__2 = 0;
				goto IL_0497;
			case 2:
				_003C_003E1__state = -1;
				ai_Mon_Ink_DivineAnimal_Bite.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_DivineAnimal_Bite.ripDmgFactor).Dispatch(_003Cent_003E5__4);
				ai_Mon_Ink_DivineAnimal_Bite.FxPlay(ai_Mon_Ink_DivineAnimal_Bite.ripEffect, _003Cent_003E5__4);
				_003Ccount_003E5__2++;
				goto IL_0449;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_0497;
				}
				IL_045b:
				if (_003Ci_003E5__3 < ai_Mon_Ink_DivineAnimal_Bite._entsHit.Count)
				{
					_003Cent_003E5__4 = ai_Mon_Ink_DivineAnimal_Bite._entsHit[_003Ci_003E5__3];
					if (_003Ccount_003E5__2 == 2)
					{
						_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_DivineAnimal_Bite.ripAtkDelay);
						_003C_003E1__state = 2;
						return true;
					}
					ai_Mon_Ink_DivineAnimal_Bite.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_DivineAnimal_Bite.biteDmgFactor).Dispatch(_003Cent_003E5__4);
					ai_Mon_Ink_DivineAnimal_Bite.FxPlay(ai_Mon_Ink_DivineAnimal_Bite.biteEffect, _003Cent_003E5__4);
					_003Ccount_003E5__2++;
					_003Cent_003E5__4 = null;
					goto IL_0449;
				}
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_DivineAnimal_Bite.dmgInterval);
				_003C_003E1__state = 3;
				return true;
				IL_0497:
				if (_003Ccount_003E5__2 < 3)
				{
					_003Ci_003E5__3 = 0;
					goto IL_045b;
				}
				ai_Mon_Ink_DivineAnimal_Bite.Destroy();
				return false;
				IL_0449:
				_003Ci_003E5__3++;
				goto IL_045b;
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

	public DewCollider range;

	public float length;

	public float chargeDuration;

	public float fowardDistance;

	public float displaceDuration;

	public float stunDuration;

	public DewEase ease;

	public float ripAtkDelay;

	public float dmgInterval;

	public ScalingValue biteDmgFactor;

	public ScalingValue ripDmgFactor;

	public AbilityTargetValidator hittable;

	public GameObject catchSound;

	public DewAnimationClip failedAnim;

	public GameObject biteEffect;

	public GameObject ripEffect;

	public float chainStunPreventTime;

	private List<Entity> _entsHit;

	private Vector3 _originPos;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__20))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void DoDamage(GameObject eff, ScalingValue damage)
	{
	}

	private void MirrorProcessed()
	{
	}
}
