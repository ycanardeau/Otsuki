// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/9022cd57-de73-42c6-bfb3-5f996be40623

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_ACK_NAMETABLE_OP packet is sent from the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see> that is being queried for <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see> information back to the new <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see>. It will include all entries missing from the new host's name table.
	/// </summary>
	[Immutable]
	public sealed record AckNameTableOperationsMessage : ICoreMessage
	{
		[Immutable]
		internal sealed record Entry
		{
			/// <summary>
			/// A 32-bit field that contains the internal message for the given <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_f3af3b08-b79e-477d-a4ad-76f4168485e4">name table entry</see>.
			/// </summary>
			public PacketType MessageId { get; init; }

			/// <summary>
			/// A 32-bit field that contains the size for the given operation buffer.
			/// </summary>
			internal int OperationSize => Operation.Count;

			/// <summary>
			/// A variable length field that contains the portion of the packet originally associated with the name table operation, except for the <b>dwPacketType</b> field, as indicated by the <b>dwMsgId</b> field. Each operation buffer is atomic to itself. For example, an <b>op</b> value corresponding to a <b>dwMsgId</b> field value of 0x000000D1 would contain the <b>dpnidLeaving</b>, <b>dwVersion</b>, <b>dwVersionNotUsed</b>, and <b>dwDestroyReason</b> field information from an original DN_DESTROY_PLAYER packet.
			/// </summary>
			public IImmutableList<byte> Operation { get; init; } = ImmutableArray<byte>.Empty;

			public Entry() { }

			public Entry(BinaryReader reader)
			{
				MessageId = (PacketType)reader.ReadInt32();
			}

			public byte[] ToByteArray()
			{
				using var stream = new MemoryStream();
				using var writer = new BinaryWriter(stream);

				writer.Write((int)MessageId);

				return stream.ToArray();
			}

			public override string ToString()
			{
				var builder = new StringBuilder();
				builder.AppendLine($"{nameof(Entry)}:");
				builder.AppendLine($"\t{nameof(MessageId)}: {MessageId}");
				builder.AppendLine($"\t{nameof(Operation)}: {BitConverter.ToString(Operation.ToArray())}");
				return builder.ToString();
			}
		}

		internal IImmutableList<Entry> EntriesInternal { get; init; } = ImmutableArray<Entry>.Empty;

		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.AckNameTableOperations;

		/// <summary>
		/// A 32-bit field that contains the number of name table entries included. The <b>dwMsgId</b>, <b>dwOpOffset</b>, <b>dwOpSize</b>, and <b>op</b> fields are present in a DN_ACK_NAMETABLE_OP message <b>dwNumEntries</b> times.
		/// </summary>
		internal int NumEntries => EntriesInternal.Count;

		public IImmutableList<ICoreMessage?> Entries
		{
			get => EntriesInternal
				.Select(entry => CoreMessageSerializer.Default.Deserialize(BitConverter.GetBytes((int)entry.MessageId).Concat(entry.Operation).ToArray()))
				.ToImmutableArray();
			init => EntriesInternal = value
				.Select(entry => new Entry
				{
					MessageId = entry?.PacketType ?? 0,
					Operation = entry is not null ? CoreMessageSerializer.Default.Serialize(entry).Skip(4).ToImmutableArray() : ImmutableArray<byte>.Empty,
				})
				.ToImmutableArray();
		}

		public AckNameTableOperationsMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(AckNameTableOperationsMessage)}:");
			builder.AppendLine($"\t{nameof(NumEntries)}: {NumEntries}");

			foreach (var e in Entries)
				builder.AppendLine(string.Join('\n', (e?.ToString() ?? string.Empty).Split('\n').Select(l => "\t" + l)));

			return builder.ToString();
		}
	}

	internal class AckNameTableOperationsMessageSerializer : ICoreMessageSerializer<AckNameTableOperationsMessage>
	{
		public static AckNameTableOperationsMessageSerializer Default { get; } = new();

		public virtual AckNameTableOperationsMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.AckNameTableOperations)
				return null;

			var endOfPacketType = reader.BaseStream.Position;
			var numEntries = reader.ReadInt32();

			var entries = new AckNameTableOperationsMessage.Entry[numEntries];
			for (var i = 0; i < entries.Length; i++)
			{
				reader.BaseStream.Seek(endOfPacketType + 4 + 12 * i + 4, SeekOrigin.Begin);
				var operationOffset = reader.ReadInt32();
				var operationSize = reader.ReadInt32();

				byte[] operation;
				if (operationOffset != 0)
				{
					reader.BaseStream.Seek(endOfPacketType + operationOffset, SeekOrigin.Begin);
					operation = reader.ReadBytes(operationSize);
				}
				else
					operation = Array.Empty<byte>();

				reader.BaseStream.Seek(endOfPacketType + 4 + 12 * i, SeekOrigin.Begin);
				entries[i] = new AckNameTableOperationsMessage.Entry(reader)
				{
					Operation = operation.ToImmutableArray(),
				};
			}

			return new()
			{
				EntriesInternal = entries.ToImmutableArray(),
			};
		}

		public virtual void Write(BinaryWriter writer, AckNameTableOperationsMessage message)
		{
			writer.Write((int)message.PacketType);
			var endOfPacketType = writer.BaseStream.Position;
			var offset = 4
				+ 12 * message.NumEntries
				+ message.EntriesInternal.Sum(e => e.OperationSize);

			var operationOffsets = new int[message.NumEntries];
			for (var i = 0; i < message.NumEntries; i++)
			{
				var e = message.EntriesInternal[i];

				operationOffsets[i] = 0;
				if (e.OperationSize != 0)
				{
					operationOffsets[i] = offset -= e.OperationSize;
					writer.BaseStream.Seek(endOfPacketType + operationOffsets[i], SeekOrigin.Begin);
					writer.Write(e.Operation.ToArray());
				}
			}

			writer.BaseStream.Seek(endOfPacketType, SeekOrigin.Begin);
			writer.Write(message.NumEntries);

			for (var i = 0; i < message.NumEntries; i++)
			{
				var e = message.EntriesInternal[i];
				writer.Write(e.ToByteArray());
				writer.Write(operationOffsets[i]);
				writer.Write(e.OperationSize);
			}
		}

		public virtual AckNameTableOperationsMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(AckNameTableOperationsMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
