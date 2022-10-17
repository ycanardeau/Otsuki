// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/00550341-76c2-47c8-b8a0-04872f646a13

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_NAMETABLE_VERSION packet specifies the version number of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see>.
/// </summary>
[Immutable]
public sealed record NameTableVersionMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.NameTableVersion;

	/// <summary>
	/// A 32-bit field that contains the current name table version number.
	/// </summary>
	public int Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	internal int VersionNotUsed { get; init; }

	public NameTableVersionMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(NameTableVersionMessage)}:");
		builder.AppendLine($"\t{nameof(Version)}: {Version}");
		builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
		return builder.ToString();
	}
}

internal class NameTableVersionMessageSerializer : ICoreMessageSerializer<NameTableVersionMessage>
{
	public static NameTableVersionMessageSerializer Default { get; } = new();

	public virtual NameTableVersionMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.NameTableVersion)
			return null;

		var version = reader.ReadInt32();
		var versionNotUsed = reader.ReadInt32();

		return new()
		{
			Version = version,
			VersionNotUsed = versionNotUsed,
		};
	}

	public virtual void Write(BinaryWriter writer, NameTableVersionMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.Version);
		writer.Write(message.VersionNotUsed);
	}

	public virtual NameTableVersionMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(NameTableVersionMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
