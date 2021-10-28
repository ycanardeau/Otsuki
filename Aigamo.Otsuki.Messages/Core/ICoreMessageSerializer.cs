namespace Aigamo.Otsuki.Messages.Core;

public interface ICoreMessageSerializer<TMessage> where TMessage : ICoreMessage
{
	TMessage? Deserialize(byte[] data);

	byte[] Serialize(TMessage message);
}
