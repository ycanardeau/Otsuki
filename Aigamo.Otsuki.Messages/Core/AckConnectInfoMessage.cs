// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8b46432b-bebb-4f94-9961-fed0c9b0fa09

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_ACK_CONNECT_INFO packet is sent from the client/peer to the server/host to <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_6aa258ea-917f-461a-9c54-1b1a66791965">acknowledge</see> the receipt of connection information. This packet contains no user data beyond the packet type field.
/// </summary>
[Immutable]
public sealed record AckConnectInfoMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.AckConnectInfo;

	public AckConnectInfoMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(AckConnectInfoMessage)}:");
		return builder.ToString();
	}
}

internal class AckConnectInfoMessageSerializer : ICoreMessageSerializer<AckConnectInfoMessage>
{
	public static AckConnectInfoMessageSerializer Default { get; } = new();

	public virtual AckConnectInfoMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.AckConnectInfo)
			return null;

		return new();
	}

	public virtual void Write(BinaryWriter writer, AckConnectInfoMessage message)
	{
		writer.Write((int)message.PacketType);
	}

	public virtual AckConnectInfoMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(AckConnectInfoMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
