using UnityEngine;
using UnityEngine.UI;

public class UI_EntityHealthBar : UI_EntityBar
{
	private static readonly int ClipUvLeft = Shader.PropertyToID("_ClipUvLeft");

	private static readonly int ClipUvRight = Shader.PropertyToID("_ClipUvRight");

	private static readonly int GradTopLeftCol = Shader.PropertyToID("_GradTopLeftCol");

	private static readonly int GradBottomLeftCol = Shader.PropertyToID("_GradBotLeftCol");

	public Image shieldFillImage;

	public GameObject deadIndicator;

	public bool enableSizeAdjustment;

	public RectTransform sizeTarget;

	public Vector2 healthRange;

	public Vector2 widthRange;

	public GameObject mirageObject;

	public Image mirageFill;

	public Image mirageDelta;

	public GameObject isTargetObject;

	public bool hideIfUndamaged;

	public Image specialFill;

	private Material _mirageFillMat;

	private Material _specialFillMat;

	private bool _isRevealed;

	private CanvasGroup _cg;

	private float _mirageDeltaCv;

	private UI_TickedBar _tickedBar;

	protected override void Awake()
	{
		base.Awake();
		_cg = GetComponent<CanvasGroup>();
		if (!hideIfUndamaged)
		{
			_isRevealed = true;
		}
		if (_cg != null)
		{
			_cg.alpha = (_isRevealed ? 1f : 0f);
		}
		if (isTargetObject != null)
		{
			isTargetObject.SetActive(value: false);
		}
		if (specialFill != null)
		{
			_specialFillMat = Object.Instantiate(specialFill.GetComponent<Image>().material);
			specialFill.material = _specialFillMat;
		}
		_tickedBar = GetComponentInChildren<UI_TickedBar>();
	}

	private void OnDestroy()
	{
		if (_specialFillMat != null)
		{
			Object.Destroy(_specialFillMat);
		}
		if (_mirageFillMat != null)
		{
			Object.Destroy(_mirageFillMat);
		}
	}

	protected override float GetFillAmount()
	{
		if (base.target == null || base.target.Status.isHealthHidden)
		{
			return 0f;
		}
		return base.target.currentHealth / GetMaxAmount();
	}

	private float GetMaxAmount()
	{
		if (base.target == null)
		{
			return 1f;
		}
		if (base.target.Status.hasMirageSkin)
		{
			return base.target.maxHealth;
		}
		return Mathf.Max(base.target.currentHealth + base.target.Status.currentShield, base.target.maxHealth);
	}

	protected override void UpdateFillAmounts()
	{
		base.UpdateFillAmounts();
		if (base.target is Monster { type: Monster.MonsterType.Boss } m && m.Visual.isSpawning)
		{
			if (_cg != null)
			{
				_cg.alpha = 0f;
			}
		}
		else
		{
			if (base.target == null)
			{
				return;
			}
			if (base.target.isAlive && base.target.Visual.isRendererOff && _cg != null)
			{
				_cg.alpha = 0f;
				return;
			}
			if (base.target is Hero { isKnockedOut: not false } && _cg != null)
			{
				_cg.alpha = 0f;
				return;
			}
			if (base.target.Status.isInConversation && _cg != null)
			{
				_cg.alpha = 0f;
				return;
			}
			bool didDoMirage = false;
			if (mirageFill != null)
			{
				if (base.target.Status.hasMirageSkin)
				{
					if (_mirageFillMat == null)
					{
						_mirageFillMat = Object.Instantiate(mirageFill.material);
						mirageFill.material = _mirageFillMat;
					}
					_isRevealed = true;
					didDoMirage = true;
					mirageFill.fillAmount = base.target.Status.currentShield / base.target.Status.mirageSkinInitAmount;
					mirageDelta.fillAmount = Mathf.SmoothDamp(mirageDelta.fillAmount, mirageFill.fillAmount, ref _mirageDeltaCv, 0.2f);
					Color c = base.target.Visual.highlight.meshHighlight.glowPasses[0].color;
					_mirageFillMat.SetColor(GradTopLeftCol, c.WithV(1f).WithS(1f));
					_mirageFillMat.SetColor(GradBottomLeftCol, c.WithV(1f).WithS(1f));
					mirageDelta.color = c.WithV(1f).WithS(0.5f);
				}
				else
				{
					mirageFill.fillAmount = 0f;
					mirageDelta.fillAmount = 0f;
				}
			}
			if (mirageObject != null)
			{
				mirageObject.SetActive(didDoMirage);
			}
			float maxAmount = GetMaxAmount();
			if (_tickedBar != null)
			{
				_tickedBar.SetMaxValue(maxAmount);
			}
			shieldFillImage.fillAmount = ((didDoMirage || base.target.Status.isHealthHidden) ? 0f : ((base.target.currentHealth + base.target.Status.currentShield) / maxAmount));
			if (deadIndicator != null)
			{
				deadIndicator.SetActive(base.target.Status.isDead);
			}
			if (enableSizeAdjustment)
			{
				Vector2 size = sizeTarget.sizeDelta;
				size.x = Mathf.Lerp(widthRange.x, widthRange.y, (base.target.Status.maxHealth + base.target.Status.mirageSkinInitAmount - healthRange.x) / (healthRange.y - healthRange.x));
				sizeTarget.sizeDelta = size;
			}
			if (!_isRevealed && !base.target.IsNullInactiveDeadOrKnockedOut() && (base.target.currentHealth < base.target.maxHealth - 1f || (base.target.Status.hasMirageSkin && base.target.Status.currentShield < base.target.Status.mirageSkinInitAmount - 1f)))
			{
				_isRevealed = true;
			}
			bool shouldShow = _isRevealed;
			if (isTargetObject != null)
			{
				bool isTarget = DewInput.currentMode == InputMode.Gamepad && ManagerBase<ControlManager>.instance != null && base.target == ManagerBase<ControlManager>.instance.targetEnemy;
				isTargetObject.SetActive(isTarget);
				if (isTarget)
				{
					shouldShow = true;
				}
			}
			if (_specialFillMat != null)
			{
				Vector2 fill = base.target.Status.specialFill;
				if (base.target.Status.isHealthHidden)
				{
					fill = Vector2.zero;
				}
				float max = GetMaxAmount();
				float from = fill.x / max;
				float to = fill.y / max;
				_specialFillMat.SetFloat(ClipUvLeft, from);
				_specialFillMat.SetFloat(ClipUvRight, 1f - to);
			}
			if (_cg != null)
			{
				_cg.alpha = (shouldShow ? 1 : 0);
			}
		}
	}
}
