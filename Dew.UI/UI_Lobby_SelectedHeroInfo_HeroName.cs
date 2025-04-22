using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Lobby_SelectedHeroInfo_HeroName : UI_Lobby_SelectedHeroInfo_Base
{
	private TextMeshProUGUI _text;

	private Material _textMat;

	protected override void Awake()
	{
		base.Awake();
		_text = GetComponent<TextMeshProUGUI>();
		_textMat = Object.Instantiate(_text.fontSharedMaterial);
		_text.fontSharedMaterial = _textMat;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_textMat != null)
		{
			Object.Destroy(_textMat);
		}
	}

	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (!(base.selectedHero == null))
		{
			base.text.text = DewLocalization.GetUIValue(base.selectedHeroName + "_Name");
			RectTransform obj = (RectTransform)base.transform.parent;
			obj.DOKill(complete: true);
			Vector3 op = obj.localPosition;
			obj.localPosition += Vector3.right * 50f;
			obj.DOLocalMoveX(op.x, 0.4f);
			base.text.DOKill(complete: true);
			base.text.color = new Color(1f, 1f, 1f, 0f);
			base.text.DOColor(Color.white, 0.4f);
			SyncHue("_FaceColor");
			SyncHue("_GlowColor");
		}
		void SyncHue(string propertyID)
		{
			Color color = _textMat.GetColor(propertyID);
			Color.RGBToHSV(base.selectedHero.mainColor, out var h, out var s, out var V);
			Color.RGBToHSV(color, out V, out var _, out var v);
			_textMat.SetColor(propertyID, Color.HSVToRGB(h, s, v, hdr: true));
		}
	}
}
