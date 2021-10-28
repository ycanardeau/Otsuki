namespace Aigamo.Otsuki.Messages.Core
{
	public class CoreMessageSerializer : ICoreMessageSerializer<ICoreMessage>
	{
		public static CoreMessageSerializer Default { get; } = new();

		public virtual ICoreMessage? Deserialize(byte[] data)
		{
			try
			{
				var packetType = (PacketType)BitConverter.ToInt32(data, 0);
				return packetType switch
				{
					PacketType.PlayerConnectInfo => PlayerConnectInfoMessageSerializer.Default.Deserialize(data),
					PacketType.ConnectFailed => ConnectFailedMessageSerializer.Default.Deserialize(data),
					PacketType.SendConnectInfo => SendConnectInfoMessageSerializer.Default.Deserialize(data),
					PacketType.AddPlayer => AddPlayerMessageSerializer.Default.Deserialize(data),
					PacketType.AckConnectInfo => AckConnectInfoMessageSerializer.Default.Deserialize(data),
					PacketType.InstructConnect => InstructConnectMessageSerializer.Default.Deserialize(data),
					PacketType.SendPlayerDpnid => SendPlayerDpnidMessageSerializer.Default.Deserialize(data),
					PacketType.InstructedConnectFailed => InstructedConnectFailedMessageSerializer.Default.Deserialize(data),
					PacketType.ConnectAttemptFailed => ConnectAttemptFailedMessageSerializer.Default.Deserialize(data),
					PacketType.TerminateSession => TerminateSessionMessageSerializer.Default.Deserialize(data),
					PacketType.DestroyPlayer => DestroyPlayerMessageSerializer.Default.Deserialize(data),
					PacketType.HostMigrate => HostMigrateMessageSerializer.Default.Deserialize(data),
					PacketType.NameTableVersion => NameTableVersionMessageSerializer.Default.Deserialize(data),
					PacketType.ResyncVersion => ResyncVersionMessageSerializer.Default.Deserialize(data),
					PacketType.RequestIntegrityCheck => RequestIntegrityCheckMessageSerializer.Default.Deserialize(data),
					PacketType.IntegrityCheck => IntegrityCheckMessageSerializer.Default.Deserialize(data),
					PacketType.IntegrityCheckResponse => IntegrityCheckResponseMessageSerializer.Default.Deserialize(data),
					PacketType.RequestNameTableOperations => RequestNameTableOperationsMessageSerializer.Default.Deserialize(data),
					PacketType.AckNameTableOperations => AckNameTableOperationsMessageSerializer.Default.Deserialize(data),
					PacketType.HostMigrateComplete => HostMigrateCompleteMessageSerializer.Default.Deserialize(data),
					_ => null
				};
			}
			catch
			{
				// TODO: trace exception
				return null;
			}
		}

		public virtual byte[] Serialize(ICoreMessage message) => message switch
		{
			PlayerConnectInfoMessage m => PlayerConnectInfoMessageSerializer.Default.Serialize(m),
			ConnectFailedMessage m => ConnectFailedMessageSerializer.Default.Serialize(m),
			SendConnectInfoMessage m => SendConnectInfoMessageSerializer.Default.Serialize(m),
			AddPlayerMessage m => AddPlayerMessageSerializer.Default.Serialize(m),
			AckConnectInfoMessage m => AckConnectInfoMessageSerializer.Default.Serialize(m),
			InstructConnectMessage m => InstructConnectMessageSerializer.Default.Serialize(m),
			SendPlayerDpnidMessage m => SendPlayerDpnidMessageSerializer.Default.Serialize(m),
			InstructedConnectFailedMessage m => InstructedConnectFailedMessageSerializer.Default.Serialize(m),
			ConnectAttemptFailedMessage m => ConnectAttemptFailedMessageSerializer.Default.Serialize(m),
			TerminateSessionMessage m => TerminateSessionMessageSerializer.Default.Serialize(m),
			DestroyPlayerMessage m => DestroyPlayerMessageSerializer.Default.Serialize(m),
			HostMigrateMessage m => HostMigrateMessageSerializer.Default.Serialize(m),
			NameTableVersionMessage m => NameTableVersionMessageSerializer.Default.Serialize(m),
			ResyncVersionMessage m => ResyncVersionMessageSerializer.Default.Serialize(m),
			RequestIntegrityCheckMessage m => RequestIntegrityCheckMessageSerializer.Default.Serialize(m),
			IntegrityCheckMessage m => IntegrityCheckMessageSerializer.Default.Serialize(m),
			IntegrityCheckResponseMessage m => IntegrityCheckResponseMessageSerializer.Default.Serialize(m),
			RequestNameTableOperationsMessage m => RequestNameTableOperationsMessageSerializer.Default.Serialize(m),
			AckNameTableOperationsMessage m => AckNameTableOperationsMessageSerializer.Default.Serialize(m),
			HostMigrateCompleteMessage m => HostMigrateCompleteMessageSerializer.Default.Serialize(m),
			_ => throw new ArgumentException(message: null, paramName: nameof(message)),
		};
	}
}
