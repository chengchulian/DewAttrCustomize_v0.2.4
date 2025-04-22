using UnityEngine;

public class PlayTutorial_StartGameRift : Rift, IPlayerPathableArea
{
	public View tutorialDiffView;

	public Vector3 pathablePosition => Dew.GetPositionOnGround(base.transform.position);

	protected override bool OnInteractRift(Hero hero)
	{
		tutorialDiffView.Show();
		return false;
	}

	private void MirrorProcessed()
	{
	}
}
