// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/cfe3ca4e-8a7b-41fa-82c7-206ab2b74660

using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_INTEGRITY_CHECK packet is a request from a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> to a peer inquiring whether the peer is still in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
	/// </summary>
	[Immutable]
	public sealed record IntegrityCheckMessage : ICoreMessage
	{
		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.IntegrityCheck;

		/// <summary>
		/// A 32-bit field that contains the identifier of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see> requesting this validation. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
		/// </summary>
		public Dpnid DpnidRequesting { get; init; }

		public IntegrityCheckMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(IntegrityCheckMessage)}:");
			builder.AppendLine($"\t{nameof(DpnidRequesting)}: {DpnidRequesting}");
			return builder.ToString();
		}
	}

	internal class IntegrityCheckMessageSerializer : ICoreMessageSerializer<IntegrityCheckMessage>
	{
		public static IntegrityCheckMessageSerializer Default { get; } = new();

		public virtual IntegrityCheckMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.IntegrityCheck)
				return null;

			var dpnidRequesting = new Dpnid(reader.ReadInt32());

			return new()
			{
				DpnidRequesting = dpnidRequesting,
			};
		}

		public virtual void Write(BinaryWriter writer, IntegrityCheckMessage message)
		{
			writer.Write((int)message.PacketType);
			writer.Write(message.DpnidRequesting.Value);
		}

		public virtual IntegrityCheckMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(IntegrityCheckMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
