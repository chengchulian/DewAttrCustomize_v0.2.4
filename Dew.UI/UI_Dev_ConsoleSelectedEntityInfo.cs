using System;
using System.Collections.Generic;
using System.Reflection;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[LogicUpdatePriority(2100)]
public class UI_Dev_ConsoleSelectedEntityInfo : LogicBehaviour
{
	public TextMeshProUGUI mainText;

	public TextMeshProUGUI statusText;

	public TextMeshProUGUI controlText;

	public TextMeshProUGUI abilityText;

	private void SetTextsEnabled(bool value)
	{
		mainText.enabled = value;
		statusText.enabled = value;
		controlText.enabled = value;
		abilityText.enabled = value;
	}

	private void Awake()
	{
		SetTextsEnabled(value: false);
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (NetworkedManagerBase<ConsoleManager>.softInstance == null || NetworkedManagerBase<ConsoleManager>.softInstance.localSelectedEntity == null || !NetworkedManagerBase<ConsoleManager>.softInstance.isConsoleWindowOpen)
		{
			SetTextsEnabled(value: false);
			return;
		}
		SetTextsEnabled(value: true);
		try
		{
			UpdateMainText();
			UpdateStatusText();
			UpdateControlText();
			UpdateAbilityText();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void UpdateMainText()
	{
		Entity ent = NetworkedManagerBase<ConsoleManager>.instance.localSelectedEntity;
		mainText.text = string.Format("<size=150%>{0}</size>\r\nOwner: {1}\r\nParent: {2}\r\nPos/Rot: {3}/{4}`\r\n\r\nAnimation: {5} {6}\r\n", ent.GetActorReadableName(), ent.owner.name, (ent.parentActor == null) ? "null" : ent.parentActor.GetActorReadableName(), ent.position, ent.rotation.eulerAngles, ent.Animation.abilityAnimStatus.isPlaying, ent.Animation.abilityAnimStatus.normalizedTime);
	}

	private void UpdateStatusText()
	{
		Entity ent = NetworkedManagerBase<ConsoleManager>.instance.localSelectedEntity;
		string seText = "";
		foreach (StatusEffect s in ent.Status.statusEffects)
		{
			if (s.isActive && !(s == null))
			{
				seText = seText + s.GetActorReadableName() + "\n";
			}
		}
		string beText = "";
		foreach (object enumValue in Enum.GetValues(typeof(BasicEffectMask)))
		{
			string enumName = Enum.GetName(typeof(BasicEffectMask), enumValue);
			if (ent.Status.basicEffectMask.HasFlag((BasicEffectMask)enumValue))
			{
				beText = beText + enumName + " ";
			}
		}
		statusText.text = $"<size=150%>Status</size>\r\nAlive: {ent.Status.isAlive}\r\nLevel: {ent.Status.level}\r\nHealth: {ent.currentHealth}/{ent.maxHealth}\r\nMana: {ent.currentMana}/{ent.maxMana}\r\nBasic Effects: {beText}\r\nStatus Effects:\r\n{seText}\r\n";
	}

	private void UpdateControlText()
	{
		Entity ent = NetworkedManagerBase<ConsoleManager>.instance.localSelectedEntity;
		controlText.text = "<size=150%>Controls</size>\r\n";
		if (!NetworkServer.active)
		{
			return;
		}
		string serverOnlyText = "";
		if (NetworkServer.active)
		{
			string actions = "";
			foreach (ActionBase a in ent.Control.queuedActions)
			{
				actions += $"{a} ";
				if (a.isAllowedToMove)
				{
					actions += "MOVABLE";
				}
				actions += "\n";
			}
			NavMeshAgent agent = (NavMeshAgent)typeof(EntityControl).GetField("_agent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ent.Control);
			serverOnlyText = $"\r\nAgent Destination: {agent.destination}\r\nAgent Desired Velocity: {agent.desiredVelocity}\r\nAgent Velocity: {agent.velocity}\r\n\r\nAttack Target: {ent.Control.attackTarget}\r\n\r\n{actions}\r\n";
		}
		controlText.text += string.Format("\r\nWalk Strength: {0} {1}\r\nIs Doing Move to Action: {2}\r\nMove: {3}\r\nAttack: {4}\r\nAbility: {5}\r\n\r\n{6}\r\n", ent.Control.walkStrength, ent.Control.isWalking ? "WALKING" : "", (NetworkServer.active || ent.Control.isLocalMovementProcessor) ? ((object)ent.Control.isDoingMoveToAction) : "?", ent.Control.IsActionBlocked(EntityControl.BlockableAction.Move), ent.Control.IsActionBlocked(EntityControl.BlockableAction.Attack), ent.Control.IsActionBlocked(EntityControl.BlockableAction.Ability), serverOnlyText);
	}

	private void UpdateAbilityText()
	{
		Entity ent = NetworkedManagerBase<ConsoleManager>.instance.localSelectedEntity;
		string abilitiesText = "";
		if (ent.Ability.attackAbility != null)
		{
			AbilityTrigger a = ent.Ability.attackAbility;
			abilitiesText = abilitiesText + "Attack: " + a.GetActorReadableName() + "\n";
			for (int i = 0; i < a.configs.Length; i++)
			{
				abilitiesText += $" - {a.currentCharges[i]}/{a.configs[i].maxCharges} {a.currentUnscaledCooldownTimes[i]:0.0}s/{a.GetMaxCooldownTime(i, scaled: false):0.0}s\n";
			}
		}
		foreach (KeyValuePair<int, AbilityTrigger> a2 in ent.Ability.abilities)
		{
			abilitiesText += $"{a2.Key}: {a2.Value.GetActorReadableName()}\n";
			for (int j = 0; j < a2.Value.configs.Length; j++)
			{
				abilitiesText += $" - {a2.Value.currentCharges[j]}/{a2.Value.configs[j].maxCharges} {a2.Value.currentUnscaledCooldownTimes[j]:0.0}s/{a2.Value.GetMaxCooldownTime(j, scaled: false):0.0}s\n";
			}
		}
		abilityText.text = $"<size=150%>Abilities</size>\r\nAbilities({ent.Ability.abilities.Count}):\r\n{abilitiesText}\r\n";
	}
}
