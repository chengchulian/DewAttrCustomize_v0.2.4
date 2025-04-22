using DG.Tweening;
using UnityEngine;

public class UI_Lobby_SelectedHeroInfo_StarDecoration : UI_Lobby_SelectedHeroInfo_Base
{
	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (!(DewPlayer.local == null))
		{
			RectTransform rt = (RectTransform)base.transform;
			Vector3 os = rt.localScale;
			rt.DOKill(complete: true);
			rt.localScale = os * 0.8f;
			rt.DOScale(os, 0.4f);
			CanvasGroup component = GetComponent<CanvasGroup>();
			component.DOKill(complete: true);
			component.alpha = 0f;
			component.DOFade(1f, 0.4f);
			if (rt.transform.childCount > 0)
			{
				Object.Destroy(rt.transform.GetChild(0).gameObject);
			}
			Hero hero = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType);
			if (hero != null && hero.decoConstellationPrefab != null)
			{
				GameObject obj = Object.Instantiate(hero.decoConstellationPrefab, base.transform);
				RectTransform obj2 = (RectTransform)obj.transform;
				obj2.anchoredPosition = Vector2.zero;
				obj2.pivot = Vector2.zero;
				obj2.anchorMin = Vector2.zero;
				obj2.anchorMax = Vector2.zero;
				obj2.sizeDelta = rt.sizeDelta;
				obj.GetComponent<UI_Common_DecoConstellation>().color = hero.mainColor;
			}
		}
	}
}
