// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/b3c67ec1-f73c-4c47-bd06-95cd4a3b4219

using Aigamo.Extensions.Primitives;

namespace Aigamo.Otsuki.Messages.Reliable
{
	/// <summary>
	/// The CONNECT packet is used to request a connection. If accepted, the response is a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/2377d224-85b7-4c1a-8677-bd18a08dc5da">CONNECTED (section 2.2.1.2)</see> packet or a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/ac197b6f-789f-4905-bae5-362ca920e177">CONNECTED_SIGNED (section 2.2.1.3)</see> packet, depending on whether packet signing is enabled.
	/// </summary>
	[Immutable]
	public sealed record ConnectMessage : IReliableMessage
	{
		/// <summary>
		/// A command-code bitmask that contains values that are combined by using the bitwise OR operation from the following table. The <b>PACKET_COMMAND_CFRAME</b> flag MUST be set, and the <b>PACKET_COMMAND_POLL</b> flag SHOULD be set. All other bits MUST be set to zero, and the packet MUST be ignored if they are not.
		/// </summary>
		public PacketCommand Command { get; internal init; } = PacketCommand.CommandFrame;

		/// <summary>
		/// Extended operation code. It MUST be set to the following value:
		/// </summary>
		public ExtendedOpcode Opcode { get; } = ExtendedOpcode.Connect;

		/// <summary>
		/// A message identifier used to correlate responses. The initial value SHOULD be set to zero and SHOULD be incremented each time the connect packet is retried. The recipient MUST echo the value in <b>bRspId</b> when responding.
		/// </summary>
		public byte MessageId { get; init; }

		/// <summary>
		/// Not used in connect packets. This MUST be set to zero when sent and ignored on receipt.
		/// </summary>
		public byte ResponseId { get; init; }

		/// <summary>
		/// The version number of the sender's DirectPlay 8 Protocol, in <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_079478cb-f4c5-4ce5-b72b-2144da5d2ce7">little-endian</see> byte order, where the upper 16 bits are considered a major version number and the lower 16 bits are considered a minor version number. The major version number MUST be set to 0x0001; otherwise, the packet MUST be ignored. The minor version number SHOULD<a id="Appendix_A_Target_1"></a><a aria-label="Product behavior note 1" href="8a440fe2-28b1-44de-8a7e-abe94ce23cf9#Appendix_A_1" data-linktype="relative-path">&lt;1&gt;</a> be set to 0x0006 to indicate support for all features, including <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2e830c5b-5096-4360-a2e6-f88ec8962dce">coalescence</see> and signing.
		/// </summary>
		public int ProtocolVersion { get; init; }

		/// <summary>
		/// The session identifier used to correlate responses. The value is dependent upon the implementation and SHOULD be a random, nonpredictable number. This MUST NOT be set to zero unless <b>dwCurrentProtocolVersion</b> indicates a minor version less than 0x0005. This MUST remain the same value when retrying the CONNECT packet. The recipient MUST echo the value in <b>dwSessID</b> when responding.
		/// </summary>
		public SessionId SessionId { get; init; }

		/// <summary>
		/// The requestor's computer system <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2efd2022-ffd1-4be4-9ff3-aa6b1515948f">tick count</see>, in millisecond units and specified in little-endian byte order.
		/// </summary>
		public int Timestamp { get; init; }

		public ConnectMessage() { }

		public bool CommandFrame
		{
			get => Command.HasFlag(PacketCommand.CommandFrame);
		}

		public bool Poll
		{
			get => Command.HasFlag(PacketCommand.Poll);
			init => Command = value ? (Command | PacketCommand.Poll) : (Command & ~PacketCommand.Poll);
		}

		public ushort MajorVersion
		{
			get => ProtocolVersion.HighUInt16();
			init => ProtocolVersion = ProtocolVersion.WithHighUInt16(value);
		}

		public ushort MinorVersion
		{
			get => ProtocolVersion.LowUInt16();
			init => ProtocolVersion = ProtocolVersion.WithLowUInt16(value);
		}

		public override string ToString() => $"{nameof(ConnectMessage)} [" +
			$"{nameof(Command)}={Command}, " +
			$"{nameof(Opcode)}={Opcode}, " +
			$"{nameof(MessageId)}={MessageId}, " +
			$"{nameof(ResponseId)}={ResponseId}, " +
			$"{nameof(ProtocolVersion)}={ProtocolVersion}, " +
			$"{nameof(SessionId)}={SessionId}, " +
			$"{nameof(Timestamp)}={Timestamp}]";
	}

	internal class ConnectMessageSerializer : IReliableMessageSerializer<ConnectMessage>
	{
		public static ConnectMessageSerializer Default { get; } = new();

		public virtual ConnectMessage? Read(BinaryReader reader)
		{
			var command = (PacketCommand)reader.ReadByte();

			var opcode = (ExtendedOpcode)reader.ReadByte();
			if (opcode != ExtendedOpcode.Connect)
				return null;

			var messageId = reader.ReadByte();
			var responseId = reader.ReadByte();
			var protocolVersion = reader.ReadInt32();
			var sessionId = new SessionId(reader.ReadInt32());
			var timestamp = reader.ReadInt32();

			return new()
			{
				Command = command,
				MessageId = messageId,
				ResponseId = responseId,
				ProtocolVersion = protocolVersion,
				SessionId = sessionId,
				Timestamp = timestamp,
			};
		}

		public virtual void Write(BinaryWriter writer, ConnectMessage message)
		{
			writer.Write((byte)message.Command);
			writer.Write((byte)message.Opcode);
			writer.Write(message.MessageId);
			writer.Write(message.ResponseId);
			writer.Write(message.ProtocolVersion);
			writer.Write(message.SessionId.Value);
			writer.Write(message.Timestamp);
		}

		public virtual ConnectMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(ConnectMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
