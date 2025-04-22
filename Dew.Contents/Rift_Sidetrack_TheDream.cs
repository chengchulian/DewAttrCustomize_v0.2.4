public class Rift_Sidetrack_TheDream : Rift_Sidetrack
{
	protected override string GetConfirmMessage()
	{
		return DewLocalization.GetUIValue("InGame_Message_EndYourJourney");
	}

	public override void TravelImmediately()
	{
		if (DewBuildProfile.current.buildType == BuildType.DemoLite)
		{
			NetworkedManagerBase<ZoneManager>.instance.DoDeadEndTravel();
			NetworkedManagerBase<GameManager>.instance.WrapUpAndShowResult(DewGameResult.ResultType.DemoFinish);
		}
		else
		{
			base.TravelImmediately();
		}
	}

	private void MirrorProcessed()
	{
	}
}
