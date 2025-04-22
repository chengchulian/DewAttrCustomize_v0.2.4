using UnityEngine;

[CreateAssetMenu(fileName = "EosApiKey", menuName = "EOS/API Key", order = 1)]
public class EosApiKey : ScriptableObject
{
	public string epicProductName = "MyApplication";

	public string epicProductVersion = "1.0";

	public string epicProductId = "";

	public string epicSandboxId = "";

	public string epicDeploymentId = "";

	public string epicClientId = "";

	public string epicClientSecret = "";
}
