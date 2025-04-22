namespace Epic.OnlineServices.Leaderboards;

public struct UserScoresQueryStatInfo
{
	public Utf8String StatName { get; set; }

	public LeaderboardAggregation Aggregation { get; set; }

	internal void Set(ref UserScoresQueryStatInfoInternal other)
	{
		StatName = other.StatName;
		Aggregation = other.Aggregation;
	}
}
