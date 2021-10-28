// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/66f773bc-8237-4acd-8ff7-634ee2791440

using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_REQ_NAMETABLE_OP packet is sent from the new <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> to a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see> with a newer <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see> to request that the peer send back name table operations that have not yet been performed on the host. If no newer name table exists, this message is not sent.
	/// </summary>
	[Immutable]
	public sealed record RequestNameTableOperationsMessage : ICoreMessage
	{
		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.RequestNameTableOperations;

		/// <summary>
		/// A 32-bit field that contains the current name table version number of the host.
		/// </summary>
		public int Version { get; init; }

		/// <summary>
		/// Not used.
		/// </summary>
		internal int VersionNotUsed { get; init; }

		public RequestNameTableOperationsMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(RequestNameTableOperationsMessage)}:");
			builder.AppendLine($"\t{nameof(Version)}: {Version}");
			builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
			return builder.ToString();
		}
	}

	internal class RequestNameTableOperationsMessageSerializer : ICoreMessageSerializer<RequestNameTableOperationsMessage>
	{
		public static RequestNameTableOperationsMessageSerializer Default { get; } = new();

		public virtual RequestNameTableOperationsMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.RequestNameTableOperations)
				return null;

			var version = reader.ReadInt32();
			var versionNotUsed = reader.ReadInt32();

			return new()
			{
				Version = version,
				VersionNotUsed = versionNotUsed,
			};
		}

		public virtual void Write(BinaryWriter writer, RequestNameTableOperationsMessage message)
		{
			writer.Write((int)message.PacketType);
			writer.Write(message.Version);
			writer.Write(message.VersionNotUsed);
		}

		public virtual RequestNameTableOperationsMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(RequestNameTableOperationsMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
