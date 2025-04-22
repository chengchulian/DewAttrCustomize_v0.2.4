using Cysharp.Threading.Tasks;
using Mirror;

public class TutorialNetworkManager : DewNetworkManager
{
	protected override async UniTaskVoid StartGame()
	{
		DewNetworkManager.networkMode = Mode.Singleplayer;
		NetworkServer.dontListen = true;
		StartHost();
	}
}
