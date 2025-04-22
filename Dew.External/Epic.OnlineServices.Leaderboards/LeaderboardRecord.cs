namespace Epic.OnlineServices.Leaderboards;

public struct LeaderboardRecord
{
	public ProductUserId UserId { get; set; }

	public uint Rank { get; set; }

	public int Score { get; set; }

	public Utf8String UserDisplayName { get; set; }

	internal void Set(ref LeaderboardRecordInternal other)
	{
		UserId = other.UserId;
		Rank = other.Rank;
		Score = other.Score;
		UserDisplayName = other.UserDisplayName;
	}
}
