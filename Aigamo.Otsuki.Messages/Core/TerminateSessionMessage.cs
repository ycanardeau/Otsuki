// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/f3ae8504-5358-4af8-b67d-330250c45832

using System.Collections.Immutable;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_TERMINATE_SESSION packet instructs the client or the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see> to disconnect from the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
/// </summary>
[Immutable]
public sealed record TerminateSessionMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.TerminateSession;

	/// <summary>
	/// A 32-bit field that contains the size, in bytes, of the terminate data. If <b>dwTerminateDataOffset</b> is 0, <b>dwTerminateDataSize</b> SHOULD also be 0. If <b>dwTerminateDataOffset</b> is not 0, <b>dwTerminateDataSize</b> SHOULD also not be 0.
	/// </summary>
	internal int TerminateDataSize => TerminateData.Count;

	/// <summary>
	/// A variable-length field that contains a byte array from the application that describes why the client or the peer is being terminated from the game session.
	/// </summary>
	public IImmutableList<byte> TerminateData { get; init; } = ImmutableArray<byte>.Empty;

	public TerminateSessionMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(TerminateSessionMessage)}:");
		builder.AppendLine($"\t{nameof(TerminateData)}: {BitConverter.ToString(TerminateData.ToArray())}");
		return builder.ToString();
	}
}

internal class TerminateSessionMessageSerializer : ICoreMessageSerializer<TerminateSessionMessage>
{
	public static TerminateSessionMessageSerializer Default { get; } = new();

	public virtual TerminateSessionMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.TerminateSession)
			return null;

		var endOfPacketType = reader.BaseStream.Position;
		var terminateDataOffset = reader.ReadInt32();
		var terminateDataSize = reader.ReadInt32();

		var terminateData = ImmutableArray<byte>.Empty;
		if (terminateDataOffset != 0)
		{
			reader.BaseStream.Seek(endOfPacketType + terminateDataOffset, SeekOrigin.Begin);
			terminateData = reader.ReadBytes(terminateDataSize).ToImmutableArray();
		}

		return new()
		{
			TerminateData = terminateData,
		};
	}

	public virtual void Write(BinaryWriter writer, TerminateSessionMessage message)
	{
		writer.Write((int)message.PacketType);
		var endOfPacketType = writer.BaseStream.Position;
		var offset = 8 + message.TerminateDataSize;

		var terminateDataOffset = 0;
		if (message.TerminateDataSize != 0)
		{
			terminateDataOffset = offset -= message.TerminateDataSize;
			writer.BaseStream.Seek(endOfPacketType + terminateDataOffset, SeekOrigin.Begin);
			writer.Write(message.TerminateData.ToArray());
		}

		writer.BaseStream.Seek(endOfPacketType, SeekOrigin.Begin);
		writer.Write(terminateDataOffset);
		writer.Write(message.TerminateDataSize);
		writer.Write(message.TerminateData.ToArray());
	}

	public virtual TerminateSessionMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(TerminateSessionMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
