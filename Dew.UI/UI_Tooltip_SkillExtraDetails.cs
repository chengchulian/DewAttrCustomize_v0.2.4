public class UI_Tooltip_SkillExtraDetails : UI_Tooltip_BaseObj
{
	private string _extraDetails;

	private bool _isShowingDetails;

	public override void OnSetup()
	{
		base.OnSetup();
		text.enabled = false;
	}

	private void ShowHoldKeyText()
	{
		text.text = string.Format(DewLocalization.GetUIValue("InGame_Tooltip_Detail_HoldKeyToExpandDetails"), "[" + DewInput.GetReadableTextForCurrentMode(DewSave.profile.controls.showDetails) + "]");
	}

	private void PopulateDetails()
	{
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (result.players[playerIndex].TryGetSkillData(skillType, out var skillData) && skillData.type == SkillType.Ultimate)
			{
				AddDesc("UltimateSkill");
			}
		}
		else if (base.currentObject is SkillTrigger { type: SkillType.Ultimate })
		{
			AddDesc("UltimateSkill");
		}
	}

	private void AddDesc(string key)
	{
		AddDescRaw(DewLocalization.GetUIValue("InGame_Tooltip_Detail_" + key));
	}

	private void AddDescRaw(string rawText)
	{
		rawText = DewLocalization.HighlightKeywords(rawText);
		if (_extraDetails == null)
		{
			_extraDetails = rawText;
		}
		else
		{
			_extraDetails = _extraDetails + "\n" + rawText;
		}
	}

	private void Update()
	{
		if (_extraDetails != null)
		{
			bool shouldShowDetail = DewInput.GetButton(DewSave.profile.controls.showDetails, checkGameAreaForMouse: false);
			if (_isShowingDetails && !shouldShowDetail)
			{
				ShowHoldKeyText();
				_isShowingDetails = false;
			}
			else if (!_isShowingDetails && shouldShowDetail)
			{
				text.text = _extraDetails;
				_isShowingDetails = true;
			}
		}
	}
}
