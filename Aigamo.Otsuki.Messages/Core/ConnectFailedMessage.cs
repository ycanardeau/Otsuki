// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/7fc9f0e0-27d4-4975-b520-079737d5cb0b

using System.Collections.Immutable;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_CONNECT_FAILED packet indicates that a connection attempt failed.
	/// </summary>
	[Immutable]
	public sealed record ConnectFailedMessage : ICoreMessage
	{
		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.ConnectFailed;

		/// <summary>
		/// A 32-bit field that contains the failure code.
		/// </summary>
		public ResultCode ResultCode { get; init; }

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the data in the <b>reply</b> field. If <b>dwReplyOffset</b> is 0, <b>dwReplySize</b> SHOULD also be 0. If <b>dwReplyOffset</b> is not 0, <b>dwReplySize</b> SHOULD also not be 0.
		/// </summary>
		internal int ReplySize => Reply.Count;

		/// <summary>
		/// A variable-length field that contains an array of bytes that provides a reply message from the application identifying the connection failure. Reply data is only expected when the failure type is <b>DPNERR_HOSTREJECTEDCONNECTION</b>.
		/// </summary>
		public IImmutableList<byte> Reply { get; init; } = ImmutableArray<byte>.Empty;

		public ConnectFailedMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(ConnectFailedMessage)}:");
			builder.AppendLine($"\t{nameof(ResultCode)}: {ResultCode}");
			builder.AppendLine($"\t{nameof(Reply)}: {BitConverter.ToString(Reply.ToArray())}");
			return builder.ToString();
		}
	}

	internal class ConnectFailedMessageSerializer : ICoreMessageSerializer<ConnectFailedMessage>
	{
		public static ConnectFailedMessageSerializer Default { get; } = new();

		public virtual ConnectFailedMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.ConnectFailed)
				return null;

			var endOfPacketType = reader.BaseStream.Position;
			var resultCode = (ResultCode)reader.ReadInt32();
			var replyOffset = reader.ReadInt32();
			var replySize = reader.ReadInt32();

			var reply = ImmutableArray<byte>.Empty;
			if (replyOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + replyOffset, SeekOrigin.Begin);
				reply = reader.ReadBytes(replySize).ToImmutableArray();
			}

			return new()
			{
				ResultCode = resultCode,
				Reply = reply,
			};
		}

		public virtual void Write(BinaryWriter writer, ConnectFailedMessage message)
		{
			writer.Write((int)message.PacketType);
			var endOfPacketType = writer.BaseStream.Position;
			var offset = 12 + message.ReplySize;

			var replyOffset = 0;
			if (message.ReplySize != 0)
			{
				replyOffset = offset -= message.ReplySize;
				writer.BaseStream.Seek(endOfPacketType + replyOffset, SeekOrigin.Begin);
				writer.Write(message.Reply.ToArray());
			}

			writer.BaseStream.Seek(endOfPacketType, SeekOrigin.Begin);
			writer.Write((int)message.ResultCode);
			writer.Write(replyOffset);
			writer.Write(message.ReplySize);
		}

		public virtual ConnectFailedMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(ConnectFailedMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
