using UnityEngine;

namespace viperOSK;

public interface I_OSK_Key
{
	void Click(OSK_Receiver inputfield = null);

	void Highlight(bool hi, Color c);

	OSK_KEY_TYPES KeyType();

	Transform GetKeyTransform();

	Vector2Int GetLayoutLocation();

	string GetKeyName();

	object GetObject();
}
