public class UI_PlayerStardust : UI_AnimatedValueText
{
	public bool useConstellationState;

	protected override float GetValue()
	{
		if (useConstellationState && SingletonBehaviour<UI_Constellations>.softInstance != null && SingletonBehaviour<UI_Constellations>.softInstance.state != null)
		{
			return SingletonBehaviour<UI_Constellations>.softInstance.state.stardust;
		}
		if (DewSave.profile == null)
		{
			return 0f;
		}
		return DewSave.profile.stardust;
	}
}
