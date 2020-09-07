using System;

namespace Aigamo.Otsuki.Messages.Reliable
{
	public class ReliableMessageSerializer : IReliableMessageSerializer<IReliableMessage>
	{
		public static ReliableMessageSerializer Default { get; } = new();

		public virtual IReliableMessage? Deserialize(byte[] data)
		{
			// OPTIMIZE
			try
			{
				if (data.Length < 4)
					return null;

				var command = (PacketCommand)data[0];

				if (command.HasFlag(PacketCommand.Data))
					return DataFrameMessageSerializer.Default.Deserialize(data);

				if (command.HasFlag(PacketCommand.CommandFrame))
				{
					if (data.Length < 12)
						return null;

					if ((command != PacketCommand.CommandFrame) && (command != (PacketCommand.CommandFrame | PacketCommand.Poll)))
						return null;

					var opcode = (ExtendedOpcode)data[1];
					return opcode switch
					{
						ExtendedOpcode.Connect => ConnectMessageSerializer.Default.Deserialize(data),
						ExtendedOpcode.Connected => ConnectedMessageSerializer.Default.Deserialize(data),
						ExtendedOpcode.HardDisconnect => HardDisconnectMessageSerializer.Default.Deserialize(data),
						ExtendedOpcode.Sack => SackMessageSerializer.Default.Deserialize(data),
						_ => null
					};
				}

				return null;
			}
			catch
			{
				// TODO: trace exception
				return null;
			}
		}

		public virtual byte[] Serialize(IReliableMessage message) => message switch
		{
			ConnectMessage m => ConnectMessageSerializer.Default.Serialize(m),
			ConnectedMessage m => ConnectedMessageSerializer.Default.Serialize(m),
			HardDisconnectMessage m => HardDisconnectMessageSerializer.Default.Serialize(m),
			SackMessage m => SackMessageSerializer.Default.Serialize(m),
			DataFrameMessage m => DataFrameMessageSerializer.Default.Serialize(m),
			_ => throw new ArgumentException(message: null, paramName: nameof(message)),
		};
	}
}
