public class Rev_EnterPureWhiteDream : DewReverieItem
{
	public override int grantedStardust => 30;

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		AchWithGameResult(delegate(DewGameResult r)
		{
			if (r.result == DewGameResult.ResultType.DemoFinish)
			{
				Complete();
			}
		});
	}
}
