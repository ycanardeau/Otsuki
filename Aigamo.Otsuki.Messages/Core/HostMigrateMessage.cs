// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/95336dad-e3d8-4475-8c52-40d271977f3b

using System.IO;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_HOST_MIGRATE packet is sent from the new <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> to all remaining <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peers</see> in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see> to notify them that a migration is taking place.
	/// </summary>
	[Immutable]
	public sealed record HostMigrateMessage : ICoreMessage
	{
		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.HostMigrate;

		/// <summary>
		/// A 32-bit field that contains the identifier for the host that has just disconnected. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
		/// </summary>
		public Dpnid DpnidOldHost { get; init; }

		/// <summary>
		/// A 32-bit field that contains the identifier for the newly assigned host that is in the process of migrating. For more information, see section 2.2.7.
		/// </summary>
		public Dpnid DpnidNewHost { get; init; }

		public HostMigrateMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(HostMigrateMessage)}:");
			builder.AppendLine($"\t{nameof(DpnidOldHost)}: {DpnidOldHost}");
			builder.AppendLine($"\t{nameof(DpnidNewHost)}: {DpnidNewHost}");
			return builder.ToString();
		}
	}

	internal class HostMigrateMessageSerializer : ICoreMessageSerializer<HostMigrateMessage>
	{
		public static HostMigrateMessageSerializer Default { get; } = new();

		public virtual HostMigrateMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.HostMigrate)
				return null;

			var dpnidOldHost = new Dpnid(reader.ReadInt32());
			var dpnidNewHost = new Dpnid(reader.ReadInt32());

			return new()
			{
				DpnidOldHost = dpnidOldHost,
				DpnidNewHost = dpnidNewHost,
			};
		}

		public virtual void Write(BinaryWriter writer, HostMigrateMessage message)
		{
			writer.Write((int)message.PacketType);
			writer.Write(message.DpnidOldHost.Value);
			writer.Write(message.DpnidNewHost.Value);
		}

		public virtual HostMigrateMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(HostMigrateMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
