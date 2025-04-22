public class UI_Tooltip_ArtifactName : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (base.currentObject is Artifact a)
		{
			string color = Dew.GetHex(a.mainColor);
			string aName = DewLocalization.GetArtifactName(DewLocalization.GetArtifactKey(a.GetType()));
			text.text = "<color=" + color + ">" + aName + "</color>";
		}
	}
}
