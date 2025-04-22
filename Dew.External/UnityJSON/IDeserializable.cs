using SimpleJSON;

namespace UnityJSON;

public interface IDeserializable
{
	void Deserialize(JSONNode node, Deserializer deserializer);
}
