namespace Aigamo.Otsuki.Messages.Reliable;

public interface IReliableMessage
{
	PacketCommand Command { get; }
}
