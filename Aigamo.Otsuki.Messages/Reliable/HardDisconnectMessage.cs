// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/77c2b46b-996c-4f0b-b362-53ab782fdfbb

using Aigamo.Extensions.Primitives;

namespace Aigamo.Otsuki.Messages.Reliable
{
	/// <summary>
	/// The HARD_DISCONNECT packet is used to quickly disconnect or <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_6aa258ea-917f-461a-9c54-1b1a66791965">acknowledge</see> quick disconnection without waiting for remaining packets to be delivered.
	/// </summary>
	[Immutable]
	public sealed record HardDisconnectMessage : IReliableMessage
	{
		/// <summary>
		/// The command-code bitmask that contains values that are combined by using the bitwise OR operation from the following table. The <b>PACKET_COMMAND_CFRAME</b> flag MUST be set. The <b>PACKET_COMMAND_POLL</b> flag SHOULD NOT be set. All other bits MUST be set to zero and the packet MUST be ignored if they are not.
		/// </summary>
		public PacketCommand Command { get; internal init; } = PacketCommand.CommandFrame;

		/// <summary>
		/// An extended operation code. It MUST be set to the following value:
		/// </summary>
		public ExtendedOpcode Opcode { get; } = ExtendedOpcode.HardDisconnect;

		/// <summary>
		/// The message identifier. The value SHOULD be the next&nbsp; incremented value after the <b>bMsgID</b> value used when sending the previous CFRAME message of any type other than <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_04967b3e-cdc6-4a45-a3d5-3e96a02770b9">SACK</see>, but the actual value used by a sender MUST be ignored on receipt.
		/// </summary>
		public byte MessageId { get; init; }

		/// <summary>
		/// The response identifier. This value SHOULD be set to zero, unless the connection is using <b>PACKET_SIGNING_FULL</b>; in which case, it MUST be set to the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_b62c00dd-75c2-47bf-ab93-bd8681b8fee4">sequence ID</see> of the next <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2d23f9e2-e672-4706-a346-5ca133d47473">data frame (DFRAME)</see> that would have been sent had HARD_DISCONNECT not occurred.
		/// </summary>
		public byte ResponseId { get; init; }

		/// <summary>
		/// The version number, in <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_079478cb-f4c5-4ce5-b72b-2144da5d2ce7">little-endian</see> byte order, of the requestor's DirectPlay 8 Protocol. The value SHOULD match the value previously sent in a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/b3c67ec1-f73c-4c47-bd06-95cd4a3b4219">CONNECT</see>, <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/2377d224-85b7-4c1a-8677-bd18a08dc5da">CONNECTED</see>, or <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/ac197b6f-789f-4905-bae5-362ca920e177">CONNECTED_SIGNED</see> packet, and MUST be ignored on receipt.
		/// </summary>
		public int ProtocolVersion { get; init; }

		/// <summary>
		/// The session identifier. This value MUST be set to the same <b>dwSessID</b> value that is specified in the CONNECT message originally associated with the connection.
		/// </summary>
		public SessionId SessionId { get; init; }

		/// <summary>
		/// The sender's computer system <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2efd2022-ffd1-4be4-9ff3-aa6b1515948f">tick count</see>, in millisecond units, specified in little-endian byte order.
		/// </summary>
		public int Timestamp { get; init; }

		/// <summary>
		/// If the connection was established using signing, this MUST be the signature of the packet using the agreed-upon signing algorithm. The packet sequence ID to be used in the calculation is the value in <b>bRspId</b>. This field MUST NOT be present if signing is not enabled for the connection.
		/// </summary>
		public long Signature { get; init; }

		public HardDisconnectMessage() { }

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

		public override string ToString() => $"{nameof(HardDisconnectMessage)} [" +
			$"{nameof(Command)}={Command}, " +
			$"{nameof(Opcode)}={Opcode}, " +
			$"{nameof(MessageId)}={MessageId}, " +
			$"{nameof(ResponseId)}={ResponseId}, " +
			$"{nameof(ProtocolVersion)}={ProtocolVersion}, " +
			$"{nameof(SessionId)}={SessionId}, " +
			$"{nameof(Timestamp)}={Timestamp}, " +
			$"{nameof(Signature)}={Signature}]";
	}

	internal class HardDisconnectMessageSerializer : IReliableMessageSerializer<HardDisconnectMessage>
	{
		public static HardDisconnectMessageSerializer Default { get; } = new();

		public virtual HardDisconnectMessage? Read(BinaryReader reader)
		{
			var enableSigning = false/* TODO */;

			var command = (PacketCommand)reader.ReadByte();

			var opcode = (ExtendedOpcode)reader.ReadByte();
			if (opcode != ExtendedOpcode.HardDisconnect)
				return null;

			var messageId = reader.ReadByte();
			var responseId = reader.ReadByte();
			var protocolVersion = reader.ReadInt32();
			var sessionId = new SessionId(reader.ReadInt32());
			var timestamp = reader.ReadInt32();
			var signature = enableSigning ? reader.ReadInt64() : 0;

			return new()
			{
				Command = command,
				MessageId = messageId,
				ResponseId = responseId,
				ProtocolVersion = protocolVersion,
				SessionId = sessionId,
				Timestamp = timestamp,
				Signature = signature,
			};
		}

		public virtual void Write(BinaryWriter writer, HardDisconnectMessage message)
		{
			var enableSigning = false/* TODO */;

			writer.Write((byte)message.Command);
			writer.Write((byte)message.Opcode);
			writer.Write(message.MessageId);
			writer.Write(message.ResponseId);
			writer.Write(message.ProtocolVersion);
			writer.Write(message.SessionId.Value);
			writer.Write(message.Timestamp);

			if (enableSigning)
				writer.Write(message.Signature);
		}

		public virtual HardDisconnectMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(HardDisconnectMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
