using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class Se_Curse_Somnambulism : CurseStatusEffect
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003C_003Ec__DisplayClass1_0
	{
		public List<SkillTrigger> list;
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Curse_Somnambulism _003C_003E4__this;

		private _003C_003Ec__DisplayClass1_0 _003C_003E8__1;

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
		public _003COnCreateSequenced_003Ed__1(int _003C_003E1__state)
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
			Se_Curse_Somnambulism se_Curse_Somnambulism = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (!NetworkedManagerBase<ZoneManager>.instance.isInAnyTransition && !ManagerBase<CameraManager>.instance.isPlayingCutscene && !se_Curse_Somnambulism.victim.Status.isInConversation && se_Curse_Somnambulism.victim is Hero { isInCombat: not false } hero)
				{
					_003C_003E8__1.list.Clear();
					_003COnCreateSequenced_003Eg__Check_007C1_0(hero.Skill.Q, ref _003C_003E8__1);
					_003COnCreateSequenced_003Eg__Check_007C1_0(hero.Skill.W, ref _003C_003E8__1);
					_003COnCreateSequenced_003Eg__Check_007C1_0(hero.Skill.E, ref _003C_003E8__1);
					_003COnCreateSequenced_003Eg__Check_007C1_0(hero.Skill.R, ref _003C_003E8__1);
					_003COnCreateSequenced_003Eg__Check_007C1_0(hero.Skill.Movement, ref _003C_003E8__1);
					if (_003C_003E8__1.list.Count > 0)
					{
						_003C_003E8__1.list.Shuffle();
						foreach (SkillTrigger item in _003C_003E8__1.list)
						{
							CastInfo info = new CastInfo(se_Curse_Somnambulism.victim);
							switch (item.currentConfig.castMethod.type)
							{
							case CastMethodType.Cone:
							case CastMethodType.Arrow:
								info.angle = global::UnityEngine.Random.Range(0f, 360f);
								break;
							case CastMethodType.Target:
							{
								ArrayReturnHandle<Entity> handle;
								ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, se_Curse_Somnambulism.victim.agentPosition, item.currentConfig.castMethod._range, item.currentConfig.targetValidator, se_Curse_Somnambulism.victim);
								if (readOnlySpan.Length == 0)
								{
									handle.Return();
									continue;
								}
								info.target = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
								handle.Return();
								break;
							}
							case CastMethodType.Point:
								info.point = Dew.GetPositionOnGround(se_Curse_Somnambulism.victim.agentPosition + global::UnityEngine.Random.insideUnitCircle.ToXZ() * item.currentConfig.castMethod._range);
								break;
							default:
								throw new ArgumentOutOfRangeException();
							case CastMethodType.None:
								break;
							}
							se_Curse_Somnambulism.victim.Control.Cast(item, info, allowMoveToCast: false);
						}
					}
				}
			}
			else
			{
				_003C_003E1__state = -1;
				if (!se_Curse_Somnambulism.isServer)
				{
					return false;
				}
				_003C_003E8__1.list = new List<SkillTrigger>();
			}
			_003C_003E2__current = new SI.WaitForSeconds(se_Curse_Somnambulism.GetValue(se_Curse_Somnambulism.castInterval) * global::UnityEngine.Random.Range(0.7f, 1.3f));
			_003C_003E1__state = 1;
			return true;
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

	public float[] castInterval;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__1))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	[CompilerGenerated]
	internal static void _003COnCreateSequenced_003Eg__Check_007C1_0(SkillTrigger s, ref _003C_003Ec__DisplayClass1_0 P_1)
	{
	}

	private void MirrorProcessed()
	{
	}
}
