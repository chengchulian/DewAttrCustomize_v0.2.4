using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_StarIcon : MonoBehaviour
{
	public Image iconFillMask;

	public Image iconFill;

	public Image iconBg;

	public NicerOutline iconBgOutline;

	public GameObject lockedObject;

	public TextMeshProUGUI requiredLevelText;

	public GameObject skillObject;

	public Image skillIcon;

	public StarEffect star { get; private set; }

	public void Setup(StarEffect s, float fillAmount, bool isLocked)
	{
		star = s;
		if (s.starIcon != null)
		{
			iconFillMask.sprite = s.starIcon;
			iconBg.sprite = s.starIcon;
		}
		Color color = Dew.GetStarCategoryColor(s.type);
		iconFill.color = Color.Lerp(Color.white, color, 0.5f).WithS(color.GetS() * 0.6f).WithV(0.9f);
		iconBg.color = Color.Lerp(Color.black, color, 0.25f);
		iconBgOutline.effectColor = color.WithS(color.GetS() * 0.6f).WithV(0.7f);
		if (Mathf.Approximately(fillAmount, -1f))
		{
			fillAmount = (float)DewSave.profile.newStars[s.GetType().Name].level / (float)DewResources.GetByShortTypeName<StarEffect>(s.GetType().Name).maxStarLevel;
		}
		iconFillMask.fillAmount = fillAmount;
		if (isLocked)
		{
			iconBgOutline.effectColor = iconBgOutline.effectColor.WithA(0.2f);
			iconBg.color = Color.black;
			Color c = ((star.heroType == null) ? new Color(1f, 0.9254902f, 0.7019608f) : new Color(0.76862746f, 0.85490197f, 1f));
			lockedObject.GetComponentInChildren<Image>().color = c;
			requiredLevelText.text = star.requiredLevel.ToString();
			requiredLevelText.color = c;
		}
		lockedObject.SetActive(isLocked);
		skillObject.SetActive(s.skillType != null);
		if (skillObject.activeSelf)
		{
			SkillTrigger skill = DewResources.GetByShortTypeName<SkillTrigger>(s.skillType.Name);
			skillIcon.sprite = skill.configs[0].triggerIcon;
		}
	}
}
