// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/0f03ca85-5bf5-4262-a846-117735a3edca

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_CONNECT_ATTEMPT_FAILED packet is sent from the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> to a connecting <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see> to indicate that an existing peer in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see> was unable to carry out the host's instruction to connect to a new peer.
/// </summary>
[Immutable]
public sealed record ConnectAttemptFailedMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.ConnectAttemptFailed;

	/// <summary>
	/// A 32-bit field that contains the identifier for the existing peer in the game session that was unable to connect to the new peer. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid Dpnid { get; init; }

	public ConnectAttemptFailedMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(ConnectAttemptFailedMessage)}:");
		builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
		return builder.ToString();
	}
}

internal class ConnectAttemptFailedMessageSerializer : ICoreMessageSerializer<ConnectAttemptFailedMessage>
{
	public static ConnectAttemptFailedMessageSerializer Default { get; } = new();

	public virtual ConnectAttemptFailedMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.ConnectAttemptFailed)
			return null;

		var dpnid = new Dpnid(reader.ReadInt32());

		return new()
		{
			Dpnid = dpnid,
		};
	}

	public virtual void Write(BinaryWriter writer, ConnectAttemptFailedMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.Dpnid.Value);
	}

	public virtual ConnectAttemptFailedMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(ConnectAttemptFailedMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
