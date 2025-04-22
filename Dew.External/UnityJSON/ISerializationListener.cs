namespace UnityJSON;

public interface ISerializationListener
{
	void OnSerializationWillBegin(Serializer serializer);

	void OnSerializationSucceeded(Serializer serializer);

	void OnSerializationFailed(Serializer serializer);
}
