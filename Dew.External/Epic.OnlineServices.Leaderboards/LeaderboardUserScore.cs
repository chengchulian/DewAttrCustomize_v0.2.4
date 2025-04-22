namespace Epic.OnlineServices.Leaderboards;

public struct LeaderboardUserScore
{
	public ProductUserId UserId { get; set; }

	public int Score { get; set; }

	internal void Set(ref LeaderboardUserScoreInternal other)
	{
		UserId = other.UserId;
		Score = other.Score;
	}
}
