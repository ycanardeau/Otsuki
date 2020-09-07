// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/97f93510-ca87-4df6-9af1-af5d930e42fa

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Aigamo.Extensions.Primitives;

namespace Aigamo.Otsuki.Messages.Reliable
{
	/// <summary>
	/// Data frames exist in the standard connection sequence space and typically carry application <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_ba5b8e95-9bae-4562-af24-dca6e860bc38">payload</see> data. They all are identified by having the <b>PACKET_COMMAND_DATA</b> flag (0x01) set in their <b>bCommand</b> field. The total size of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2d23f9e2-e672-4706-a346-5ca133d47473">data frame (DFRAME)</see> header and the application <b>payload</b> data SHOULD be less than the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_03aae42f-32fd-47ab-b413-d5ec92d29d45">maximum transmission unit (MTU)</see> of the underlying protocols and network. If larger messages are to be transmitted, the implementation MUST break the application <b>payload</b> data into multiple DFRAME packets, send the portions sequentially, and set the <b>PACKET_COMMAND_NEW_MSG</b> flag on the first DFRAME and the <b>PACKET_COMMAND_END_MSG</b> flag on the final DFRAME. Otherwise, the single DFRAME MUST have both the <b>PACKET_COMMAND_NEW_MSG</b> and <b>PACKET_COMMAND_END_MSG</b> flags. Application payload data that is split into multiple DFRAMEs MUST NOT be <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_2e830c5b-5096-4360-a2e6-f88ec8962dce">coalesced</see> with other payloads.
	/// </summary>
	[Immutable]
	public sealed record DataFrameMessage : IReliableMessage
	{
		private readonly SessionId _sessionId;
		private readonly uint _sackMask1;
		private readonly uint _sackMask2;
		private readonly uint _sendMask1;
		private readonly uint _sendMask2;

		/// <summary>
		/// Command field. The <b>PACKET_COMMAND_DATA</b> flag MUST be set. If the packet is a <b>KeepAlive</b>, the <b>PACKET_COMMAND_RELIABLE</b>, <b>PACKET_COMMAND_SEQUENTIAL</b>, and <b>PACKET_COMMAND_END_MSG</b> flags MUST be set. If the packet contains coalesced payloads, the <b>PACKET_COMMAND_NEW_MSG</b> and <b>PACKET_COMMAND_END_MSG</b> flags MUST be set. All other flags are optional.
		/// </summary>
		public PacketCommand Command { get; internal init; } = PacketCommand.Data;

		/// <summary>
		/// Control field. The following flags can be specified.
		/// </summary>
		public PacketControl Control { get; internal init; }

		/// <summary>
		/// The sequence number of the packet.
		/// </summary>
		public SequenceId SequenceId { get; init; }

		/// <summary>
		/// The expected sequence number of the next packet received.
		/// </summary>
		public SequenceId NextReceive { get; init; }

		/// <summary>
		/// Optional low 32 bits of the SACK mask, in <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_079478cb-f4c5-4ce5-b72b-2144da5d2ce7">little-endian</see> byte order. The existence of this field in the packet is dependent upon the <b>bControl</b> field having <b>PACKET_CONTROL_SACK1</b> set.
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
		/// Optional high 32 bits of the SACK mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bControl</b> field having <b>PACKET_CONTROL_SACK2</b> set.
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
		/// Optional low 32 bits of the send mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bControl</b> field having <b>PACKET_CONTROL_SEND1</b> set.
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
		/// Optional high 32 bits of the send mask, in little-endian byte order. The existence of this field in the packet is dependent upon the <b>bControl</b> field having <b>PACKET_CONTROL_SEND2</b> set.
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
		/// If the connection was established by using signing, this MUST be the signature of the packet using the agreed-upon signing algorithm. The packet <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_b62c00dd-75c2-47bf-ab93-bd8681b8fee4">sequence ID</see> to be used in the calculation is the value in <b>bSeq</b>. This field MUST NOT be present if signing is not enabled for the connection.
		/// </summary>
		public long Signature { get; init; }

		/// <summary>
		/// The session identifier. When the packet is marked as <b>PACKET_CONTROL_KEEPALIVE_OR_CORRELATE</b> on connections reported as version 0x00010005 or higher, the <b>dwSessID</b> identifier MUST be set to the same <b>dwSessID</b> value specified in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/b3c67ec1-f73c-4c47-bd06-95cd4a3b4219">CONNECT</see> message originally associated with the connection, and there MUST NOT be any application <b>payload</b> data for the packet. Otherwise, <b>dwSessID</b> MUST NOT be present.
		/// </summary>
		public SessionId SessionId
		{
			get => _sessionId;
			init
			{
				_sessionId = value;
				KeepAliveOrCorrelate = value != SessionId.Empty;
			}
		}

		/// <summary>
		/// Application payload data. The size of the <b>payload</b> field is the total <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_a70f5e84-6960-42f0-a160-ba0281eb548d">UDP</see> payload size minus the amount of data consumed by DFRAME headers up to this point. If the <b>PACKET_CONTROL_COALESCE</b> flag is set, the application <b>payload</b> data is not a single message or portion of a message; it is instead organized according to the coalesced payload format, as specified in section 2.2.3.
		/// </summary>
		public IImmutableList<byte> Payload { get; init; } = ImmutableArray<byte>.Empty;

		public DataFrameMessage() { }

		public bool Data
		{
			get => Command.HasFlag(PacketCommand.Data);
		}

		public bool Reliable
		{
			get => Command.HasFlag(PacketCommand.Reliable);
			init => Command = value ? (Command | PacketCommand.Reliable) : (Command & ~PacketCommand.Reliable);
		}

		public bool Sequential
		{
			get => Command.HasFlag(PacketCommand.Sequential);
			init => Command = value ? (Command | PacketCommand.Sequential) : (Command & ~PacketCommand.Sequential);
		}

		public bool Poll
		{
			get => Command.HasFlag(PacketCommand.Poll);
			init => Command = value ? (Command | PacketCommand.Poll) : (Command & ~PacketCommand.Poll);
		}

		public bool NewMessage
		{
			get => Command.HasFlag(PacketCommand.NewMessage);
			init => Command = value ? (Command | PacketCommand.NewMessage) : (Command & ~PacketCommand.NewMessage);
		}

		public bool EndMessage
		{
			get => Command.HasFlag(PacketCommand.EndMessage);
			init => Command = value ? (Command | PacketCommand.EndMessage) : (Command & ~PacketCommand.EndMessage);
		}

		public bool User1
		{
			get => Command.HasFlag(PacketCommand.User1);
			init => Command = value ? (Command | PacketCommand.User1) : (Command & ~PacketCommand.User1);
		}

		public bool User2
		{
			get => Command.HasFlag(PacketCommand.User2);
			init => Command = value ? (Command | PacketCommand.User2) : (Command & ~PacketCommand.User2);
		}

		public bool Retry
		{
			get => Control.HasFlag(PacketControl.Retry);
			init => Control = value ? (Control | PacketControl.Retry) : (Control & ~PacketControl.Retry);
		}

		public bool KeepAliveOrCorrelate
		{
			get => Control.HasFlag(PacketControl.KeepAliveOrCorrelate);
			private init => Control = value ? (Control | PacketControl.KeepAliveOrCorrelate) : (Control & ~PacketControl.KeepAliveOrCorrelate);
		}

		public bool Coalesce
		{
			get => Control.HasFlag(PacketControl.Coalesce);
			init => Control = value ? (Control | PacketControl.Coalesce) : (Control & ~PacketControl.Coalesce);
		}

		public bool EndStream
		{
			get => Control.HasFlag(PacketControl.EndStream);
			init => Control = value ? (Control | PacketControl.EndStream) : (Control & ~PacketControl.EndStream);
		}

		public bool Sack1
		{
			get => Control.HasFlag(PacketControl.Sack1);
			private init => Control = value ? (Control | PacketControl.Sack1) : (Control & ~PacketControl.Sack1);
		}

		public bool Sack2
		{
			get => Control.HasFlag(PacketControl.Sack2);
			private init => Control = value ? (Control | PacketControl.Sack2) : (Control & ~PacketControl.Sack2);
		}

		public bool Send1
		{
			get => Control.HasFlag(PacketControl.Send1);
			private init => Control = value ? (Control | PacketControl.Send1) : (Control & ~PacketControl.Send1);
		}

		public bool Send2
		{
			get => Control.HasFlag(PacketControl.Send2);
			private init => Control = value ? (Control | PacketControl.Send2) : (Control & ~PacketControl.Send2);
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

		public override string ToString() => $"{nameof(DataFrameMessage)} [" +
			$"{nameof(Command)}={Command}, " +
			$"{nameof(Control)}={Control}, " +
			$"{nameof(SequenceId)}={SequenceId}, " +
			$"{nameof(NextReceive)}={NextReceive}, " +
			$"{nameof(SackMask)}={SackMask}, " +
			$"{nameof(SendMask)}={SendMask}, " +
			$"{nameof(Signature)}={Signature}, " +
			$"{nameof(SessionId)}={SessionId}, " +
			$"{nameof(Payload)}={BitConverter.ToString(Payload.ToArray())}]";
	}

	internal class DataFrameMessageSerializer : IReliableMessageSerializer<DataFrameMessage>
	{
		public static DataFrameMessageSerializer Default { get; } = new();

		public virtual DataFrameMessage? Read(BinaryReader reader)
		{
			var enableSigning = false/* TODO */;

			var command = (PacketCommand)reader.ReadByte();
			var control = (PacketControl)reader.ReadByte();
			var sequenceId = new SequenceId(reader.ReadByte());
			var nextReceive = new SequenceId(reader.ReadByte());
			var sackMask1 = control.HasFlag(PacketControl.Sack1) ? reader.ReadUInt32() : 0;
			var sackMask2 = control.HasFlag(PacketControl.Sack2) ? reader.ReadUInt32() : 0;
			var sendMask1 = control.HasFlag(PacketControl.Send1) ? reader.ReadUInt32() : 0;
			var sendMask2 = control.HasFlag(PacketControl.Send2) ? reader.ReadUInt32() : 0;
			var signature = enableSigning ? reader.ReadInt64() : 0;
			var sessionId = control.HasFlag(PacketControl.KeepAliveOrCorrelate) ? new SessionId(reader.ReadInt32()) : SessionId.Empty;

			static byte[] ReadPayload(Stream source)
			{
				using var stream = new MemoryStream();
				source.CopyTo(stream);
				return stream.ToArray();
			}
			var payload = !control.HasFlag(PacketControl.KeepAliveOrCorrelate) ? ReadPayload(reader.BaseStream).ToImmutableArray() : ImmutableArray<byte>.Empty;

			return new()
			{
				Command = command,
				Control = control,
				SequenceId = sequenceId,
				NextReceive = nextReceive,
				SackMask1 = sackMask1,
				SackMask2 = sackMask2,
				SendMask1 = sendMask1,
				SendMask2 = sendMask2,
				Signature = signature,
				SessionId = sessionId,
				Payload = payload,
			};
		}

		public virtual void Write(BinaryWriter writer, DataFrameMessage message)
		{
			var enableSigning = false/* TODO */;

			writer.Write((byte)message.Command);
			writer.Write((byte)message.Control);
			writer.Write(message.SequenceId.Value);
			writer.Write(message.NextReceive.Value);

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

			if (message.KeepAliveOrCorrelate)
				writer.Write(message.SessionId.Value);
			else
				writer.Write(message.Payload.ToArray());
		}

		public virtual DataFrameMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(DataFrameMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
