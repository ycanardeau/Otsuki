// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/7ec53834-2541-468e-8b14-f6beb304f454

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_DESTROY_PLAYER packet instructs the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see> to remove a specified user from its <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see>.
/// </summary>
[Immutable]
public sealed record DestroyPlayerMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.DestroyPlayer;

	/// <summary>
	/// A 32-bit field that contains the identifier of the client or server to remove from the name table. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid DpnidLeaving { get; init; }

	/// <summary>
	/// A 32-bit field that contains the current name table version number.
	/// </summary>
	public int Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	internal int VersionNotUsed { get; init; }

	/// <summary>
	/// A 32-bit field that contains the reason for terminating the specified client or server.
	/// </summary>
	public DestroyPlayerFlags Reason { get; init; }

	public DestroyPlayerMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(DestroyPlayerMessage)}:");
		builder.AppendLine($"\t{nameof(DpnidLeaving)}: {DpnidLeaving}");
		builder.AppendLine($"\t{nameof(Version)}: {Version}");
		builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
		builder.AppendLine($"\t{nameof(Reason)}: {Reason}");
		return builder.ToString();
	}
}

internal class DestroyPlayerMessageSerializer : ICoreMessageSerializer<DestroyPlayerMessage>
{
	public static DestroyPlayerMessageSerializer Default { get; } = new();

	public virtual DestroyPlayerMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.DestroyPlayer)
			return null;

		var dpnidLeaving = new Dpnid(reader.ReadInt32());
		var version = reader.ReadInt32();
		var versionNotUsed = reader.ReadInt32();
		var reason = (DestroyPlayerFlags)reader.ReadInt32();

		return new()
		{
			DpnidLeaving = dpnidLeaving,
			Version = version,
			VersionNotUsed = versionNotUsed,
			Reason = reason,
		};
	}

	public virtual void Write(BinaryWriter writer, DestroyPlayerMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.DpnidLeaving.Value);
		writer.Write(message.Version);
		writer.Write(message.VersionNotUsed);
		writer.Write((int)message.Reason);
	}

	public virtual DestroyPlayerMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(DestroyPlayerMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
