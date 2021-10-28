// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/6b9cdb3e-09b4-49ba-a7c7-31a4c9a8a02f

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_INSTRUCT_CONNECT packet instructs a peer to connect to a designated peer. This packet uses the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dpl8r/b3c67ec1-f73c-4c47-bd06-95cd4a3b4219">CONNECT</see> and <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dpl8r/2377d224-85b7-4c1a-8677-bd18a08dc5da">CONNECTED</see> packets defined in <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dpl8r/7a35d96c-daca-4311-bc2b-bd6a2f50bf14">[MC-DPL8R]</see> sections 2.2.1.1 and 2.2.1.2. For an example of the message sequence for these packets, see [MC-DPL8R] section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dpl8r/f02dc0e7-3ab0-4f40-80da-bf8dc09f9f7f">4.1</see>.
/// </summary>
[Immutable]
public sealed record InstructConnectMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.InstructConnect;

	/// <summary>
	/// A 32-bit field that contains the identifier of the designated client to which the connection is being made. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid Dpnid { get; init; }

	/// <summary>
	/// A 32-bit field that contains the current version of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see>.
	/// </summary>
	public int Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	internal int VersionNotUsed { get; init; }

	public InstructConnectMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(InstructConnectMessage)}:");
		builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
		builder.AppendLine($"\t{nameof(Version)}: {Version}");
		builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
		return builder.ToString();
	}
}

internal class InstructConnectMessageSerializer : ICoreMessageSerializer<InstructConnectMessage>
{
	public static InstructConnectMessageSerializer Default { get; } = new();

	public virtual InstructConnectMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.InstructConnect)
			return null;

		var dpnid = new Dpnid(reader.ReadInt32());
		var version = reader.ReadInt32();
		var versionNotUsed = reader.ReadInt32();

		return new()
		{
			Dpnid = dpnid,
			Version = version,
			VersionNotUsed = versionNotUsed,
		};
	}

	public virtual void Write(BinaryWriter writer, InstructConnectMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.Dpnid.Value);
		writer.Write(message.Version);
		writer.Write(message.VersionNotUsed);
	}

	public virtual InstructConnectMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(InstructConnectMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
