// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/c726feed-e889-43c4-a142-2c077d02487c

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_INTEGRITY_CHECK_RESPONSE packet is a response from a peer to the host confirming that it is still in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
/// </summary>
[Immutable]
public sealed record IntegrityCheckResponseMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.IntegrityCheckResponse;

	/// <summary>
	/// Identifier of the peer that requested the validation. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid DpnidRequesting { get; init; }

	public IntegrityCheckResponseMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(IntegrityCheckResponseMessage)}:");
		builder.AppendLine($"\t{nameof(DpnidRequesting)}: {DpnidRequesting}");
		return builder.ToString();
	}
}

internal class IntegrityCheckResponseMessageSerializer : ICoreMessageSerializer<IntegrityCheckResponseMessage>
{
	public static IntegrityCheckResponseMessageSerializer Default { get; } = new();

	public virtual IntegrityCheckResponseMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.IntegrityCheckResponse)
			return null;

		var dpnidRequesting = new Dpnid(reader.ReadInt32());

		return new()
		{
			DpnidRequesting = dpnidRequesting,
		};
	}

	public virtual void Write(BinaryWriter writer, IntegrityCheckResponseMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.DpnidRequesting.Value);
	}

	public virtual IntegrityCheckResponseMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(IntegrityCheckResponseMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
