using System;
using TMPro;
using UnityEngine;

public class UI_DamageNumberText : MonoBehaviour
{
	private readonly AnimationCurve FlatCurve = new AnimationCurve(new Keyframe(1f, 1f));

	private const float MergeTime = 0.6f;

	public float lifeTime = 2f;

	public bool followTarget = true;

	public AnimationCurve offsetCurve;

	public TextMeshProUGUI text;

	public Material matDamage;

	public Material matAttack;

	public Material matAttackCrit;

	public Material matSkill;

	public Material matSkillCrit;

	public Material matHeal;

	public Material matHealCrit;

	public Material matFire;

	public Material matCold;

	public Material matLight;

	public Material matDark;

	public Vector4 healPopOffsetRange;

	public Vector4 attackPopOffsetRange;

	public Vector4 skillPopOffsetRange;

	public Vector4 otherPopOffsetRange;

	public Vector2 healFlatOffset;

	public Vector2 attackFlatOffset;

	public Vector2 gemFlatOffset;

	public Vector2 skillFlatOffset;

	public Vector2 otherFlatOffset;

	private bool _isDamage;

	private EventInfoDamage _damageInfo;

	private EventInfoHeal _healInfo;

	private float _startTime;

	private Vector2 _popOffset;

	private Vector2 _flatOffset;

	private Vector3 _lastKnownPosition;

	private Animation _animation;

	private Vector3 _originalScale;

	private bool _isImportant;

	internal Action _removeCallback;

	private Entity _victim
	{
		get
		{
			if (!_isDamage)
			{
				return _healInfo.target;
			}
			return _damageInfo.victim;
		}
	}

	private bool _isCrit
	{
		get
		{
			if (!_isDamage)
			{
				return _healInfo.isCrit;
			}
			return _damageInfo.damage.HasAttr(DamageAttribute.IsCrit);
		}
	}

	private void Awake()
	{
		_animation = GetComponent<Animation>();
		_originalScale = base.transform.localScale;
	}

	public bool CanMerge(EventInfoDamage info)
	{
		if (!_isDamage)
		{
			return false;
		}
		if (this == null || !base.enabled)
		{
			return false;
		}
		if (Time.time - _startTime > 0.6f)
		{
			return false;
		}
		if (!_damageInfo.damage.canMerge || !info.damage.canMerge)
		{
			return false;
		}
		if (!(_damageInfo.actor is ElementalStatusEffect) || !(info.actor is ElementalStatusEffect) || !(_damageInfo.victim == info.victim))
		{
			if (_damageInfo.actor != null && info.actor != null && _damageInfo.actor.GetType() == info.actor.GetType() && _damageInfo.victim == info.victim && _damageInfo.damage.HasAttr(DamageAttribute.IsCrit) == info.damage.HasAttr(DamageAttribute.IsCrit) && _damageInfo.damage.type == info.damage.type)
			{
				return _damageInfo.damage.elemental == info.damage.elemental;
			}
			return false;
		}
		return true;
	}

	public bool CanMerge(EventInfoHeal info)
	{
		if (_isDamage)
		{
			return false;
		}
		if (this == null || !base.enabled)
		{
			return false;
		}
		if (Time.time - _startTime > 0.6f)
		{
			return false;
		}
		if (!_healInfo.canMerge || !info.canMerge)
		{
			return false;
		}
		if (_healInfo.actor != null && info.actor != null && _healInfo.actor.GetType() == info.actor.GetType() && _healInfo.target == info.target)
		{
			return _healInfo.isCrit == info.isCrit;
		}
		return false;
	}

	private void LateUpdate()
	{
		UpdatePosition();
		if (Time.time - _startTime > lifeTime)
		{
			base.gameObject.SetActive(value: false);
			_removeCallback?.Invoke();
		}
	}

	private void UpdatePosition()
	{
		if (_victim != null && followTarget)
		{
			_lastKnownPosition = _victim.Visual.GetCenterPosition();
		}
		base.transform.position = Dew.mainCamera.WorldToScreenPoint(_lastKnownPosition) + (Vector3)_popOffset * offsetCurve.Evaluate(Time.time - _startTime) + (Vector3)_flatOffset;
	}

	public void Merge(EventInfoDamage info)
	{
		offsetCurve = FlatCurve;
		_startTime = Time.time;
		_popOffset = Vector2.zero;
		_damageInfo.damage.amount += info.damage.amount;
		_damageInfo.damage.discardedAmount += info.damage.discardedAmount;
		text.text = GetDamageStringRepresentation();
		GetComponent<Animation>().Rewind();
	}

	public void Merge(EventInfoHeal info)
	{
		offsetCurve = FlatCurve;
		_startTime = Time.time;
		_popOffset = Vector2.zero;
		_healInfo.amount += info.amount;
		_healInfo.discardedAmount += info.discardedAmount;
		text.text = GetDamageStringRepresentation();
		GetComponent<Animation>().Rewind();
	}

	public void Setup(EventInfoDamage info, Action removeCallback)
	{
		_isImportant = IsImportant(info.actor, info.victim);
		SetScale();
		_removeCallback = removeCallback;
		_damageInfo = info;
		_isDamage = true;
		if (info.damage.amount == -1f)
		{
			_flatOffset = otherFlatOffset;
			SetRandomPopOffset(otherPopOffsetRange);
			SetupUniversal();
			text.text = DewLocalization.GetUIValue("InGame_Immune");
			return;
		}
		if (info.damage.amount == -2f)
		{
			_flatOffset = otherFlatOffset;
			SetRandomPopOffset(otherPopOffsetRange);
			SetupUniversal();
			text.text = DewLocalization.GetUIValue("InGame_Unstoppable");
			return;
		}
		if (info.damage.elemental.HasValue)
		{
			switch (info.damage.elemental.Value)
			{
			case ElementalType.Fire:
				text.fontSharedMaterial = matFire;
				break;
			case ElementalType.Cold:
				text.fontSharedMaterial = matCold;
				break;
			case ElementalType.Light:
				text.fontSharedMaterial = matLight;
				break;
			case ElementalType.Dark:
				text.fontSharedMaterial = matDark;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		else if (info.damage.type.HasFlag(DamageData.SourceType.Physical))
		{
			text.fontSharedMaterial = (info.damage.HasAttr(DamageAttribute.IsCrit) ? matAttackCrit : matAttack);
		}
		else if (info.damage.type.HasFlag(DamageData.SourceType.Magic))
		{
			text.fontSharedMaterial = (info.damage.HasAttr(DamageAttribute.IsCrit) ? matSkillCrit : matSkill);
		}
		else
		{
			text.fontSharedMaterial = matDamage;
		}
		if (info.damage.type.HasFlag(DamageData.SourceType.Physical))
		{
			_flatOffset = attackFlatOffset;
			SetRandomPopOffset(attackPopOffsetRange);
		}
		else if (info.damage.type.HasFlag(DamageData.SourceType.Magic))
		{
			if (info.actor != null && info.actor.FindFirstOfType<Gem>() != null)
			{
				_flatOffset = gemFlatOffset;
			}
			else
			{
				_flatOffset = skillFlatOffset;
			}
			SetRandomPopOffset(skillPopOffsetRange);
		}
		else
		{
			_flatOffset = otherFlatOffset;
			SetRandomPopOffset(otherPopOffsetRange);
		}
		SetupUniversal();
	}

	private void SetRandomPopOffset(Vector4 range)
	{
		_popOffset = new Vector2(global::UnityEngine.Random.Range(range.x, range.y), global::UnityEngine.Random.Range(range.z, range.w));
		if (!_isImportant)
		{
			_popOffset *= 0.25f;
		}
	}

	public void Setup(EventInfoHeal info, Action removeCallback)
	{
		_isImportant = IsImportant(info.actor, info.target);
		SetScale();
		_removeCallback = removeCallback;
		_healInfo = info;
		_isDamage = false;
		if (info.isCrit)
		{
			text.fontSharedMaterial = matHealCrit;
		}
		else
		{
			text.fontSharedMaterial = matHeal;
		}
		_flatOffset = healFlatOffset;
		SetRandomPopOffset(healPopOffsetRange);
		SetupUniversal();
	}

	private bool IsImportant(Actor from, Entity to)
	{
		if (to == null || from == null)
		{
			return true;
		}
		Entity focused = ManagerBase<CameraManager>.instance.focusedEntity;
		if (focused == null)
		{
			return true;
		}
		if (to == focused || to.IsDescendantOf(focused))
		{
			return true;
		}
		if (from == focused || from.IsDescendantOf(focused))
		{
			return true;
		}
		return false;
	}

	private void SetScale()
	{
		if (_isImportant)
		{
			base.transform.localScale = _originalScale;
		}
		else
		{
			base.transform.localScale = _originalScale * 0.65f;
		}
	}

	private void SetupUniversal()
	{
		base.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: true);
		_startTime = Time.time;
		text.text = GetDamageStringRepresentation();
		UpdatePosition();
		_animation.Play(_isCrit ? "DamageNumberAnimCrit" : "DamageNumberAnim");
	}

	private string GetDamageStringRepresentation()
	{
		float amount = ((!_isDamage) ? Mathf.Max(_healInfo.amount + _healInfo.discardedAmount, 1f) : Mathf.Max(_damageInfo.damage.amount + _damageInfo.damage.discardedAmount, 1f));
		string text = (DewSave.profile.gameplay.abbreviateBigDamageNumbers ? Dew.FormatBigNumbers(amount, 100000f, "#,##0") : amount.ToString("#,##0"));
		if (text.Contains("\u00a0"))
		{
			text = text.Replace("\u00a0", " ");
		}
		return text;
	}
}
