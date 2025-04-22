namespace UnityJSON;

public interface IDeserializationListener
{
	void OnDeserializationWillBegin(Deserializer deserializer);

	void OnDeserializationSucceeded(Deserializer deserializer);

	void OnDeserializationFailed(Deserializer deserializer);
}
