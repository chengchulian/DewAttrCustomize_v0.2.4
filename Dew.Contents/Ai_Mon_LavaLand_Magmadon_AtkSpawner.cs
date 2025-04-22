using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_LavaLand_Magmadon_AtkSpawner : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public Vector3 customStartPoint;

		internal void _003COnCreateSequenced_003Eb__0(Ai_Mon_LavaLand_Magmadon_Projectile b)
		{
			b.SetCustomStartPosition(customStartPoint);
		}
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_Magmadon_AtkSpawner _003C_003E4__this;

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
			Ai_Mon_LavaLand_Magmadon_AtkSpawner ai_Mon_LavaLand_Magmadon_AtkSpawner = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ai_Mon_LavaLand_Magmadon_AtkSpawner.isServer)
				{
					return false;
				}
				ai_Mon_LavaLand_Magmadon_AtkSpawner.DestroyOnDeath(ai_Mon_LavaLand_Magmadon_AtkSpawner.info.caster);
				ai_Mon_LavaLand_Magmadon_AtkSpawner._centerPos = ai_Mon_LavaLand_Magmadon_AtkSpawner.info.caster.agentPosition;
				ai_Mon_LavaLand_Magmadon_AtkSpawner.maxAngle -= ai_Mon_LavaLand_Magmadon_AtkSpawner.maxAngle / 2f;
				float num2 = ai_Mon_LavaLand_Magmadon_AtkSpawner.maxAngle / (float)(ai_Mon_LavaLand_Magmadon_AtkSpawner.projectilCount - 1);
				float num3 = (0f - ai_Mon_LavaLand_Magmadon_AtkSpawner.maxAngle) / 2f;
				for (int i = 0; i < ai_Mon_LavaLand_Magmadon_AtkSpawner.projectilCount; i++)
				{
					_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass7_0();
					float num4 = num3 + num2 * (float)i;
					Vector3 vector = Quaternion.Euler(0f, num4 / 2f, 0f) * ai_Mon_LavaLand_Magmadon_AtkSpawner.info.forward;
					Vector3 vector2 = Quaternion.Euler(0f, num4, 0f) * ai_Mon_LavaLand_Magmadon_AtkSpawner.info.forward;
					CS_0024_003C_003E8__locals3.customStartPoint = ai_Mon_LavaLand_Magmadon_AtkSpawner._centerPos + vector * ai_Mon_LavaLand_Magmadon_AtkSpawner.projectileStartDistance + Vector3.up * ai_Mon_LavaLand_Magmadon_AtkSpawner.startHeight;
					Vector3 position = ai_Mon_LavaLand_Magmadon_AtkSpawner._centerPos + vector2 * ai_Mon_LavaLand_Magmadon_AtkSpawner.projectileEndDistance;
					position = Dew.GetPositionOnGround(position);
					global::UnityEngine.Debug.DrawRay(CS_0024_003C_003E8__locals3.customStartPoint, ai_Mon_LavaLand_Magmadon_AtkSpawner.info.forward * 5f, Color.red);
					ai_Mon_LavaLand_Magmadon_AtkSpawner.CreateAbilityInstance(ai_Mon_LavaLand_Magmadon_AtkSpawner.info.caster.position, null, new CastInfo(ai_Mon_LavaLand_Magmadon_AtkSpawner.info.caster, position), delegate(Ai_Mon_LavaLand_Magmadon_Projectile b)
					{
						b.SetCustomStartPosition(CS_0024_003C_003E8__locals3.customStartPoint);
					});
				}
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_Magmadon_AtkSpawner.atkInstanceDelay);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				ai_Mon_LavaLand_Magmadon_AtkSpawner.CreateAbilityInstance<Ai_Mon_LavaLand_Magmadon_Atk>(ai_Mon_LavaLand_Magmadon_AtkSpawner.info.caster.position, Quaternion.Euler(0f, ai_Mon_LavaLand_Magmadon_AtkSpawner.info.angle, 0f), ai_Mon_LavaLand_Magmadon_AtkSpawner.info);
				ai_Mon_LavaLand_Magmadon_AtkSpawner.Destroy();
				return false;
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

	public float maxAngle;

	public int projectilCount;

	public float projectileStartDistance;

	public float startHeight;

	public float projectileEndDistance;

	public float atkInstanceDelay;

	private Vector3 _centerPos;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__7))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
