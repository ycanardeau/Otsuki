// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/327e43ea-e63a-4d7f-87fd-83b6166cfd98

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_HOST_MIGRATE_COMPLETE packet informs <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peers</see> that the session-hosting responsibilities have successfully migrated from the departing old <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see>.
/// </summary>
[Immutable]
public sealed record HostMigrateCompleteMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.HostMigrateComplete;

	public HostMigrateCompleteMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(HostMigrateCompleteMessage)}:");
		return builder.ToString();
	}
}

internal class HostMigrateCompleteMessageSerializer : ICoreMessageSerializer<HostMigrateCompleteMessage>
{
	public static HostMigrateCompleteMessageSerializer Default { get; } = new();

	public virtual HostMigrateCompleteMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.HostMigrateComplete)
			return null;

		return new();
	}

	public virtual void Write(BinaryWriter writer, HostMigrateCompleteMessage message)
	{
		writer.Write((int)message.PacketType);
	}

	public virtual HostMigrateCompleteMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(HostMigrateCompleteMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
