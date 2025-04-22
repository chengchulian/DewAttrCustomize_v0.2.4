using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_LavaLand_BossInfernus_Jump : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_BossInfernus_Jump _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__13(int _003C_003E1__state)
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
			Ai_Mon_LavaLand_BossInfernus_Jump ai_Mon_LavaLand_BossInfernus_Jump = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				ai_Mon_LavaLand_BossInfernus_Jump._entTransform = ai_Mon_LavaLand_BossInfernus_Jump.info.caster.Visual.GetNewTransformModifier();
				if (!ai_Mon_LavaLand_BossInfernus_Jump.isServer)
				{
					return false;
				}
				ai_Mon_LavaLand_BossInfernus_Jump.DestroyOnDeath(ai_Mon_LavaLand_BossInfernus_Jump.info.caster);
				ai_Mon_LavaLand_BossInfernus_Jump.CreateStatusEffect<Se_Mon_LavaLand_BossInfernus_Jump_Invul>(ai_Mon_LavaLand_BossInfernus_Jump.info.caster).DestroyOnDestroy(ai_Mon_LavaLand_BossInfernus_Jump);
				ai_Mon_LavaLand_BossInfernus_Jump.info.caster.Control.IncrementBlockCounters(Channel.BlockedAction.Everything);
				ai_Mon_LavaLand_BossInfernus_Jump.RpcAscend();
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_BossInfernus_Jump.ascendTime + ai_Mon_LavaLand_BossInfernus_Jump.beforeTelegraphDelay);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Hero hero = Dew.SelectRandomAliveHero();
				Vector3 position = Dew.GetValidAgentDestination_Closest(end: Dew.GetPositionOnGround(hero.agentPosition + global::UnityEngine.Random.onUnitSphere * ai_Mon_LavaLand_BossInfernus_Jump.landPosRandomMag), start: hero.agentPosition);
				ai_Mon_LavaLand_BossInfernus_Jump.FxPlayNetworked(ai_Mon_LavaLand_BossInfernus_Jump.fxTelegraph, position, Quaternion.identity);
				ai_Mon_LavaLand_BossInfernus_Jump.Teleport(ai_Mon_LavaLand_BossInfernus_Jump.info.caster, position);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_BossInfernus_Jump.telegraphTime);
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				ai_Mon_LavaLand_BossInfernus_Jump.RpcDescendAndFinish();
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

	public float ascendTime;

	public float ascendHeight;

	public DewAnimationClip animAscend;

	public float beforeTelegraphDelay;

	public float telegraphTime;

	public float landPosRandomMag;

	public GameObject fxTelegraph;

	public DewAnimationClip animDescendFalling;

	public DewAnimationClip animLand;

	public float descendTime;

	public int landMeteorCount;

	public float postDaze;

	private EntityTransformModifier _entTransform;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__13))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void OnDestroyActor()
	{
	}

	[ClientRpc]
	private void RpcAscend()
	{
	}

	[ClientRpc]
	private void RpcDescendAndFinish()
	{
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcAscend()
	{
	}

	protected static void InvokeUserCode_RpcAscend(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	protected void UserCode_RpcDescendAndFinish()
	{
	}

	protected static void InvokeUserCode_RpcDescendAndFinish(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	static Ai_Mon_LavaLand_BossInfernus_Jump()
	{
	}
}
