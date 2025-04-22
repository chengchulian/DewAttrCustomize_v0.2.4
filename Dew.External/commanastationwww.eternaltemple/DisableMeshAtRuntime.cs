using UnityEngine;

namespace commanastationwww.eternaltemple;

public class DisableMeshAtRuntime : MonoBehaviour
{
	private void Start()
	{
		Renderer rendererComponent = GetComponent<Renderer>();
		if (rendererComponent != null)
		{
			rendererComponent.enabled = false;
		}
	}
}
