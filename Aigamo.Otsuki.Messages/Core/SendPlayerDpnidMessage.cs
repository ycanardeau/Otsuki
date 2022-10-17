// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/c25eab29-d8c1-4d92-a80e-d478fa4b6cb5

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_SEND_PLAYER_DPNID packet is used to send a user identification number to another client.
/// </summary>
[Immutable]
public sealed record SendPlayerDpnidMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.SendPlayerDpnid;

	/// <summary>
	/// A 32-bit field that contains the identifier of the client/peer. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid Dpnid { get; init; }

	public SendPlayerDpnidMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(SendPlayerDpnidMessage)}:");
		builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
		return builder.ToString();
	}
}

internal class SendPlayerDpnidMessageSerializer : ICoreMessageSerializer<SendPlayerDpnidMessage>
{
	public static SendPlayerDpnidMessageSerializer Default { get; } = new();

	public virtual SendPlayerDpnidMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.SendPlayerDpnid)
			return null;

		var dpnid = new Dpnid(reader.ReadInt32());

		return new()
		{
			Dpnid = dpnid,
		};
	}

	public virtual void Write(BinaryWriter writer, SendPlayerDpnidMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.Dpnid.Value);
	}

	public virtual SendPlayerDpnidMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(SendPlayerDpnidMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
