// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/87a60f3e-e8d7-4277-bb58-a25a7d74f5ef

using System.IO;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_INSTRUCTED_CONNECT_FAILED packet is sent from a peer to indicate that it was unable to carry out a host instruction to connect to a new peer.
	/// </summary>
	[Immutable]
	public sealed record InstructedConnectFailedMessage : ICoreMessage
	{
		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.InstructedConnectFailed;

		/// <summary>
		/// A 32-bit field that contains the identifier for the peer to which the attempted connection failed. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
		/// </summary>
		public Dpnid Dpnid { get; init; }

		public InstructedConnectFailedMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(InstructedConnectFailedMessage)}:");
			builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
			return builder.ToString();
		}
	}

	internal class InstructedConnectFailedMessageSerializer : ICoreMessageSerializer<InstructedConnectFailedMessage>
	{
		public static InstructedConnectFailedMessageSerializer Default { get; } = new();

		public virtual InstructedConnectFailedMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.InstructedConnectFailed)
				return null;

			var dpnid = new Dpnid(reader.ReadInt32());

			return new()
			{
				Dpnid = dpnid,
			};
		}

		public virtual void Write(BinaryWriter writer, InstructedConnectFailedMessage message)
		{
			writer.Write((int)message.PacketType);
			writer.Write(message.Dpnid.Value);
		}

		public virtual InstructedConnectFailedMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(InstructedConnectFailedMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
