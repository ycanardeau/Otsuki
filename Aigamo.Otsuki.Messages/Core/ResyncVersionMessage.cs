// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8155a19e-e173-410b-b750-8bb6668074f5

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_RESYNC_VERSION packet is used to request that the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see> version number be resynchronized to the current version number.
/// </summary>
[Immutable]
public sealed record ResyncVersionMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.ResyncVersion;

	/// <summary>
	/// A 32-bit field that contains the current name table version number.
	/// </summary>
	public int Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	internal int VersionNotUsed { get; init; }

	public ResyncVersionMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(ResyncVersionMessage)}:");
		builder.AppendLine($"\t{nameof(Version)}: {Version}");
		builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
		return builder.ToString();
	}
}

internal class ResyncVersionMessageSerializer : ICoreMessageSerializer<ResyncVersionMessage>
{
	public static ResyncVersionMessageSerializer Default { get; } = new();

	public virtual ResyncVersionMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.ResyncVersion)
			return null;

		var version = reader.ReadInt32();
		var versionNotUsed = reader.ReadInt32();

		return new()
		{
			Version = version,
			VersionNotUsed = versionNotUsed,
		};
	}

	public virtual void Write(BinaryWriter writer, ResyncVersionMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.Version);
		writer.Write(message.VersionNotUsed);
	}

	public virtual ResyncVersionMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(ResyncVersionMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
