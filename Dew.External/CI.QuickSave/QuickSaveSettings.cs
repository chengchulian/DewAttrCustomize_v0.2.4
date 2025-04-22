namespace CI.QuickSave;

public class QuickSaveSettings
{
	public SecurityMode SecurityMode { get; set; }

	public CompressionMode CompressionMode { get; set; }

	public string Password { get; set; }

	public QuickSaveSettings()
	{
		SecurityMode = SecurityMode.None;
		CompressionMode = CompressionMode.None;
	}
}
