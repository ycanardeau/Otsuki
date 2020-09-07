// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/9ca3f71c-1040-40b0-9866-2e2e40cf65b0

using System.IO;
using Aigamo.Extensions.Primitives;

namespace Aigamo.Otsuki.Messages.Reliable
{
	/// <summary>
	/// The SACK packet is used to selectively <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_6aa258ea-917f-461a-9c54-1b1a66791965">acknowledge</see> outstanding packets. Packet acknowledgment (ACK) is typically bundled in all user data packets using the <b>bSeq</b> and <b>bNRec</b> fields found in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2d23f9e2-e672-4706-a346-5ca133d47473">data frame (DFRAME)</see> header. However, the SACK packet is used when a dedicated ACK is requested (that is, when the <b>PACKET_COMMAND_POLL</b> bit in the <b>bCommand</b> header field is set) or when no user data remains for further bundled ACKs.
	/// </summary>
	[Immutable]
	public sealed record SackMessage : IReliableMessage
	{
		private readonly uint _sackMask1;
		private readonly uint _sackMask2;
		private readonly uint _sendMask1;
		private readonly uint _sendMask2;

		/// <summary>
		/// The command-code bitmask that contains bitwise OR values from the following table. The <b>PACKET_COMMAND_CFRAME</b> flag MUST be set. The <b>PACKET_COMMAND_POLL</b> flag SHOULD NOT be set and SHOULD be ignored on receipt. All other bits MUST be set to zero and the packet MUST be ignored if they are not.
		/// </summary>
		public PacketCommand Command { get; internal init; } = PacketCommand.CommandFrame;

		/// <summary>
		/// An extended operation code. It MUST be set to the following value:
		/// </summary>
		public ExtendedOpcode Opcode { get; } = ExtendedOpcode.Sack;

		/// <summary>
		/// A status flag or flags. The value can be one or more of the following values. All other bits MUST be set to zero. The <b>SACK_FLAGS_RESPONSE</b> flag SHOULD be set and <b>bRetry</b> SHOULD be filled in properly.
		/// </summary>
		public SackFlags Flags { get; internal init; }

		/// <summary>
		/// Indicates whether the last received packet was a retry. This value MUST be ignored if <b>SACK_FLAGS_RESPONSE</b> is not set. The value SHOULD be set to zero if the last received DFRAME for the connection was not marked as a retry; otherwise, the value SHOULD be nonzero. Recipients MUST NOT require that any particular bit or bits be set in the nonzero case—only that at least one bit is set.
		/// </summary>
		public byte Retry { get; init; }

		/// <summary>
		/// This field represents the sequence number of the next DFRAME to send. SACK packets do not have sequence numbers of their own.
		/// </summary>
		public SequenceId NextSend { get; init; }

		/// <summary>
		/// The expected sequence number of the next packet received. If the <b>SACK_FLAGS_SACK_MASK1</b> or <b>SACK_FLAGS_SACK_MASK2</b> flag is set, the <b>bNRcv</b> field is supplemented with the corresponding additional <b>dwSACKMask1</b> or <b>dwSACKMask2</b> bitmask field that selectively acknowledges frames with sequence numbers higher than <b>bNRcv</b>.
		/// </summary>
		public SequenceId NextReceive { get; init; }

		/// <summary>
		/// This SHOULD be set to zero when sent and MUST be ignored on receipt.
		/// </summary>
		internal short Padding { get; init; }

		/// <summary>
		/// The sender's computer system <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2efd2022-ffd1-4be4-9ff3-aa6b1515948f">tick count</see>, in millisecond units, specified in little-endian byte order.
		/// </summary>
		public int Timestamp { get; init; }

		/// <summary>
		/// The optional low 32 bits of the SACK mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bFlags</b> field having <b>SACK_FLAGS_SACK_MASK1</b> set.
		/// </summary>
		public uint SackMask1
		{
			get => _sackMask1;
			internal init
			{
				_sackMask1 = value;
				Sack1 = value != 0;
			}
		}

		/// <summary>
		/// The optional high 32 bits of the SACK mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bFlags</b> field having <b>SACK_FLAGS_SACK_MASK2</b> set.
		/// </summary>
		public uint SackMask2
		{
			get => _sackMask2;
			internal init
			{
				_sackMask2 = value;
				Sack2 = value != 0;
			}
		}

		/// <summary>
		/// The optional low 32 bits of the send mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bFlags</b> field having <b>SACK_FLAGS_SEND_MASK1</b> set.
		/// </summary>
		public uint SendMask1
		{
			get => _sendMask1;
			internal init
			{
				_sendMask1 = value;
				Send1 = value != 0;
			}
		}

		/// <summary>
		/// The optional high 32 bits of the send mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bFlags</b> field having <b>SACK_FLAGS_SEND_MASK2</b> set.
		/// </summary>
		public uint SendMask2
		{
			get => _sendMask2;
			internal init
			{
				_sendMask2 = value;
				Send2 = value != 0;
			}
		}

		/// <summary>
		/// If the connection was established using signing, this MUST be the signature of the packet using the agreed-upon signing algorithm. The packet <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_b62c00dd-75c2-47bf-ab93-bd8681b8fee4">sequence ID</see> to be used in the calculation is the value in <b>bNSeq</b>. This field MUST NOT be present if signing is not enabled for the connection.
		/// </summary>
		public long Signature { get; init; }

		public SackMessage() { }

		public bool CommandFrame
		{
			get => Command.HasFlag(PacketCommand.CommandFrame);
		}

		public bool Poll
		{
			get => Command.HasFlag(PacketCommand.Poll);
			init => Command = value ? (Command | PacketCommand.Poll) : (Command & ~PacketCommand.Poll);
		}

		public bool Response
		{
			get => Flags.HasFlag(SackFlags.Response);
			init => Flags = value ? (Flags | SackFlags.Response) : (Flags & ~SackFlags.Response);
		}

		public bool Sack1
		{
			get => Flags.HasFlag(SackFlags.Sack1);
			private init => Flags = value ? (Flags | SackFlags.Sack1) : (Flags & ~SackFlags.Sack1);
		}

		public bool Sack2
		{
			get => Flags.HasFlag(SackFlags.Sack2);
			private init => Flags = value ? (Flags | SackFlags.Sack2) : (Flags & ~SackFlags.Sack2);
		}

		public bool Send1
		{
			get => Flags.HasFlag(SackFlags.Send1);
			private init => Flags = value ? (Flags | SackFlags.Send1) : (Flags & ~SackFlags.Send1);
		}

		public bool Send2
		{
			get => Flags.HasFlag(SackFlags.Send2);
			private init => Flags = value ? (Flags | SackFlags.Send2) : (Flags & ~SackFlags.Send2);
		}

		public ulong SackMask
		{
			get => (SackMask1, SackMask2).ToUInt64();
			init => (SackMask1, SackMask2) = (value.LowUInt32(), value.HighUInt32());
		}

		public ulong SendMask
		{
			get => (SendMask1, SendMask2).ToUInt64();
			init => (SendMask1, SendMask2) = (value.LowUInt32(), value.HighUInt32());
		}

		public override string ToString() => $"{nameof(SackMessage)} [" +
			$"{nameof(Command)}={Command}, " +
			$"{nameof(Opcode)}={Opcode}, " +
			$"{nameof(Flags)}={Flags}, " +
			$"{nameof(Retry)}={Retry}, " +
			$"{nameof(NextSend)}={NextSend}, " +
			$"{nameof(NextReceive)}={NextReceive}, " +
			$"{nameof(Timestamp)}={Timestamp}, " +
			$"{nameof(SackMask)}={SackMask}, " +
			$"{nameof(SendMask)}={SendMask}, " +
			$"{nameof(Signature)}={Signature}]";
	}

	internal class SackMessageSerializer : IReliableMessageSerializer<SackMessage>
	{
		public static SackMessageSerializer Default { get; } = new();

		public virtual SackMessage? Read(BinaryReader reader)
		{
			var enableSigning = false/* TODO */;

			var command = (PacketCommand)reader.ReadByte();

			var opcode = (ExtendedOpcode)reader.ReadByte();
			if (opcode != ExtendedOpcode.Sack)
				return null;

			var flags = (SackFlags)reader.ReadByte();
			var retry = reader.ReadByte();
			var nextSend = new SequenceId(reader.ReadByte());
			var nextReceive = new SequenceId(reader.ReadByte());
			var padding = reader.ReadInt16();
			var timestamp = reader.ReadInt32();
			var sackMask1 = flags.HasFlag(SackFlags.Sack1) ? reader.ReadUInt32() : 0;
			var sackMask2 = flags.HasFlag(SackFlags.Sack2) ? reader.ReadUInt32() : 0;
			var sendMask1 = flags.HasFlag(SackFlags.Send1) ? reader.ReadUInt32() : 0;
			var sendMask2 = flags.HasFlag(SackFlags.Send2) ? reader.ReadUInt32() : 0;
			var signature = enableSigning ? reader.ReadInt64() : 0;

			return new()
			{
				Command = command,
				Flags = flags,
				Retry = retry,
				NextSend = nextSend,
				NextReceive = nextReceive,
				Padding = padding,
				Timestamp = timestamp,
				SackMask1 = sackMask1,
				SackMask2 = sackMask2,
				SendMask1 = sendMask1,
				SendMask2 = sendMask2,
				Signature = signature,
			};
		}

		public virtual void Write(BinaryWriter writer, SackMessage message)
		{
			var enableSigning = false/* TODO */;

			writer.Write((byte)message.Command);
			writer.Write((byte)message.Opcode);
			writer.Write((byte)message.Flags);
			writer.Write(message.Retry);
			writer.Write(message.NextSend.Value);
			writer.Write(message.NextReceive.Value);
			writer.Write(message.Padding);
			writer.Write(message.Timestamp);

			if (message.Sack1)
				writer.Write(message.SackMask1);

			if (message.Sack2)
				writer.Write(message.SackMask2);

			if (message.Send1)
				writer.Write(message.SendMask1);

			if (message.Send2)
				writer.Write(message.SendMask2);

			if (enableSigning)
				writer.Write(message.Signature);
		}

		public virtual SackMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(SackMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
