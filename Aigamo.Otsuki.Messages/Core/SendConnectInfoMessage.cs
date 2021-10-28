// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/075e84f0-b26c-4f10-9f69-21a0813dfc54

using System.Collections.Immutable;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_SEND_CONNECT_INFO packet is sent from the host/server indicating to the connecting peer/client that it has joined the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
	/// </summary>
	[Immutable]
	public sealed record SendConnectInfoMessage : ICoreMessage
	{
		internal NullTerminatedUnicodeString PasswordInternal { get; init; } = string.Empty;

		internal NullTerminatedUnicodeString SessionNameInternal { get; init; } = string.Empty;

		/// <summary>
		/// A 32-bit integer that indicates the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.SendConnectInfo;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>reply</b> field. If <b>dwReplyOffset</b> is set to 0, <b>dwReplySize</b> MUST be 0. If <b>dwReplyOffset</b> is not 0, <b>dwReplySize</b> MUST NOT be 0.
		/// </summary>
		internal int ReplySize => Reply.Count;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the application description information. This includes all fields starting with <b>dwSize</b> through <b>guidApplication</b>.
		/// </summary>
		internal int Size => 80;

		/// <summary>
		/// A 32-bit integer that specifies the application flags.
		/// </summary>
		public SessionFlags Flags { get; init; }

		/// <summary>
		/// A 32-bit integer that specifies the maximum number of clients/peers allowed in the game session. A value of 0 indicates that the maximum number of <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_0258d4e2-d4f7-4099-ae0f-a02fad73e824">players</see> is not specified.
		/// </summary>
		public int MaxPlayers { get; init; }

		/// <summary>
		/// A 32-bit integer that specifies the current number of clients/peers in the game session.
		/// </summary>
		public int CurrentPlayers { get; init; }

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>sessionName</b> field. If <b>dwSessionNameOffset</b> is 0, <b>dwSessionNameSize</b> MUST be 0. If <b>dwSessionNameOffset</b> is not 0, <b>dwSessionNameSize</b> MUST NOT be 0.
		/// </summary>
		internal int SessionNameSize => SessionNameInternal.Length;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the password. If <b>dwPasswordOffset</b> is 0, <b>dwPasswordSize</b> MUST be 0. If <b>dwPasswordOffset</b> is not 0, <b>dwPasswordSize</b> MUST NOT be 0.
		/// </summary>
		internal int PasswordSize => PasswordInternal.Length;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>reservedData</b> field. If <b>dwReservedDataOffset</b> is 0, <b>dwReservedDataSize</b> MUST be 0. If <b>dwReservedDataOffset</b> is not 0, <b>dwReservedDataSize</b> MUST NOT be 0.
		/// </summary>
		internal int ReservedDataSize => ReservedData.Count;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>applicationReservedData</b> field. If <b>dwApplicationReservedDataOffset</b> is 0, <b>dwApplicationReservedDataSize</b> MUST also be 0. If <b>dwApplicationReservedDataOffset</b> is not 0, <b>dwApplicationReservedDataSize</b> MUST NOT be 0.
		/// </summary>
		internal int ApplicationReservedDataSize => ApplicationReservedData.Count;

		/// <summary>
		/// A 128-bit field that contains the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_f49694cc-c350-462d-ab8e-816f0103c6c1">GUID</see> that identifies the particular instance of the server/host application. The value of this field implicitly SHOULD match the value of the <b>guidInstance</b> field specified in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/d2f5c735-5947-4b76-98c0-d447d9d36de8">DN_INTERNAL_MESSAGE_PLAYER_CONNECT_INFO</see> or <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/aa00e9b1-8169-4aef-aa4c-1b0619d45c24">DN_INTERNAL_MESSAGE_PLAYER_CONNECT_INFO_EX</see> message, unless that field contained all zeroes, in which case this <b>guidInstance</b> value informs the receiving client of the actual game session instance GUID.
		/// </summary>
		public Guid GuidInstance { get; init; }

		/// <summary>
		/// The application GUID as defined by the host/server.
		/// </summary>
		public Guid GuidApplication { get; init; }

		/// <summary>
		/// A 32-bit integer created by the server/host that provides the identifier for the new client joining the game session. For more information, see <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">DN_DPNID</see>.
		/// </summary>
		public Dpnid Dpnid { get; init; }

		/// <summary>
		/// A 32-bit integer that specifies the current <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see> version.
		/// </summary>
		public int Version { get; init; }

		/// <summary>
		/// Not used.
		/// </summary>
		internal int VersionNotUsed { get; init; }

		/// <summary>
		/// A 32-bit integer that provides the number of entries in the name table contained in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/87370443-4103-4153-a8ac-a6728c39b7e5">DN_NAMETABLE_ENTRY_INFO</see> field below. These are in essence players in the game session.
		/// </summary>
		public int EntryCount => NameTableEntries.Count();

		/// <summary>
		/// A 32-bit integer that provides the number of memberships in the name table contained in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/772ec104-55b6-4375-9111-cebd7bca1690">DN_NAMETABLE_MEMBERSHIP_INFO</see> field below. These are in essence player to <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_51c51c14-7f9d-4c0b-a69c-d3e059bfffac">group</see> combinations.
		/// </summary>
		public int MembershipCount => NameTableMemberships.Count();

		/// <summary>
		/// This field contains a variable-length array of DN_NAMETABLE_ENTRY_INFO structures. The length of this array is described above in the <b>dwEntryCount</b> field. Each entry in this array describes a player or group in the game session. In peer-to-peer mode, the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> MUST transmit entries for all existing participants and the new participant. In <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_a907c749-671d-466b-b589-8c6dea31403f">client/server mode</see>, the server MUST transmit only two entries: one for the server player and one for the new participant.
		/// </summary>
		public IImmutableList<NameTableEntryInfo> NameTableEntries { get; init; } = ImmutableArray<NameTableEntryInfo>.Empty;

		/// <summary>
		/// This field contains a variable-length array of DN_NAMETABLE_MEMBERSHIP_INFO structures. The length of this array is described above in the <b>dwMembershipCount</b> field. Each entry in this array describes a player/group combination.
		/// </summary>
		public IImmutableList<NameTableMembershipInfo> NameTableMemberships { get; init; } = ImmutableArray<NameTableMembershipInfo>.Empty;

		/// <summary>
		/// A variable-length field that contains a 0-terminated character array that specifies the application reserved data. This field's position is determined by <b>dwApplicationReservedDataOffset</b> and the size stated in <b>dwApplicationReservedDataSize</b>.
		/// </summary>
		public IImmutableList<byte> ApplicationReservedData { get; init; } = ImmutableArray<byte>.Empty;

		/// <summary>
		/// A variable-length field that contains a byte array that provides the reserved data. This field's position is determined by <b>dwReservedDataOffset</b> and the size stated in <b>dwReservedDataSize</b>.
		/// </summary>
		public IImmutableList<byte> ReservedData { get; init; } = ImmutableArray<byte>.Empty;

		/// <summary>
		/// A variable-length field that contains a 0-terminated wide character array that specifies the application password data. This field's position is determined by <b>dwPasswordOffset</b> and the size stated in <b>dwPasswordSize</b>. This data is passed in clear text to the protocol layer.
		/// </summary>
		public string Password
		{
			get => PasswordInternal;
			init => PasswordInternal = value;
		}

		/// <summary>
		/// A variable-length field that contains a 0-terminated wide character array that specifies the game session name. This field's position is determined by <b>dwSessionNameOffset</b> and the size stated in <b>dwSessionNameSize</b>.
		/// </summary>
		public string SessionName
		{
			get => SessionNameInternal;
			init => SessionNameInternal = value;
		}

		/// <summary>
		/// A variable-length field that contains a byte array that provides the reply. This field's position is determined by <b>dwReplyOffset</b> and the size stated in <b>dwReplySize</b>.
		/// </summary>
		public IImmutableList<byte> Reply { get; init; } = ImmutableArray<byte>.Empty;

		public SendConnectInfoMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(SendConnectInfoMessage)}:");
			builder.AppendLine($"\t{nameof(Flags)}: {Flags}");
			builder.AppendLine($"\t{nameof(MaxPlayers)}: {MaxPlayers}");
			builder.AppendLine($"\t{nameof(CurrentPlayers)}: {CurrentPlayers}");
			builder.AppendLine($"\t{nameof(GuidInstance)}: {GuidInstance}");
			builder.AppendLine($"\t{nameof(GuidApplication)}: {GuidApplication}");
			builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
			builder.AppendLine($"\t{nameof(Version)}: {Version}");
			builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");

			foreach (var e in NameTableEntries)
				builder.AppendLine(string.Join('\n', e.ToString().Split('\n').Select(l => "\t" + l)));

			foreach (var m in NameTableMemberships)
				builder.AppendLine(string.Join('\n', m.ToString().Split('\n').Select(l => "\t" + l)));

			builder.AppendLine($"\t{nameof(ApplicationReservedData)}: {BitConverter.ToString(ApplicationReservedData.ToArray())}");
			builder.AppendLine($"\t{nameof(ReservedData)}: {BitConverter.ToString(ReservedData.ToArray())}");
			builder.AppendLine($"\t{nameof(Password)}: {Password}");
			builder.AppendLine($"\t{nameof(SessionName)}: {SessionName}");
			builder.AppendLine($"\t{nameof(Reply)}: {BitConverter.ToString(Reply.ToArray())}");
			return builder.ToString();
		}
	}

	internal class SendConnectInfoMessageSerializer : ICoreMessageSerializer<SendConnectInfoMessage>
	{
		public static SendConnectInfoMessageSerializer Default { get; } = new();

		public virtual NameTableEntryInfo ReadNameTableEntryInfo(BinaryReader reader)
		{
			var dpnid = new Dpnid(reader.ReadInt32());
			var dpnidOwner = new Dpnid(reader.ReadInt32());
			var flags = (NameTableEntryFlags)reader.ReadInt32();
			var version = reader.ReadInt32();
			var versionNotUsed = reader.ReadInt32();
			var dnetVersion = (DnetVersion)reader.ReadInt32();

			return new()
			{
				Dpnid = dpnid,
				DpnidOwner = dpnidOwner,
				Flags = flags,
				Version = version,
				VersionNotUsed = versionNotUsed,
				DnetVersion = dnetVersion,
			};
		}

		public virtual NameTableMembershipInfo ReadNameTableMembershipInfo(BinaryReader reader)
		{
			var dpnidPlayer = new Dpnid(reader.ReadInt32());
			var dpnidGroup = new Dpnid(reader.ReadInt32());
			var version = reader.ReadInt32();
			var versionNotUsed = reader.ReadInt32();

			return new()
			{
				DpnidPlayer = dpnidPlayer,
				DpnidGroup = dpnidGroup,
				Version = version,
				VersionNotUsed = versionNotUsed,
			};
		}

		public virtual SendConnectInfoMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.SendConnectInfo)
				return null;

			var endOfPacketType = reader.BaseStream.Position;
			var replyOffset = reader.ReadInt32();
			var replySize = reader.ReadInt32();

			var size = reader.ReadInt32();
			if (size != 80)
				return null;

			var flags = (SessionFlags)reader.ReadInt32();
			var maxPlayers = reader.ReadInt32();
			var currentPlayers = reader.ReadInt32();
			var sessionNameOffset = reader.ReadInt32();
			var sessionNameSize = reader.ReadInt32();
			var passwordOffset = reader.ReadInt32();
			var passwordSize = reader.ReadInt32();
			var reservedDataOffset = reader.ReadInt32();
			var reservedDataSize = reader.ReadInt32();
			var applicationReservedDataOffset = reader.ReadInt32();
			var applicationReservedDataSize = reader.ReadInt32();
			var guidInstance = new Guid(reader.ReadBytes(Guid.Empty.ToByteArray().Length));
			var guidApplication = new Guid(reader.ReadBytes(Guid.Empty.ToByteArray().Length));
			var dpnid = new Dpnid(reader.ReadInt32());
			var version = reader.ReadInt32();
			var versionNotUsed = reader.ReadInt32();
			var entryCount = reader.ReadInt32();
			var membershipCount = reader.ReadInt32();

			var nameTableEntries = new NameTableEntryInfo[entryCount];
			for (var i = 0; i < nameTableEntries.Length; i++)
			{
				reader.BaseStream.Seek(endOfPacketType + 108 + 48 * i + 24, SeekOrigin.Begin);
				var nameOffset = reader.ReadInt32();
				var nameSize = reader.ReadInt32();
				var dataOffset = reader.ReadInt32();
				var dataSize = reader.ReadInt32();
				var urlOffset = reader.ReadInt32();
				var urlSize = reader.ReadInt32();

				byte[] url;
				if (urlOffset != 0)
				{
					reader.BaseStream.Seek(endOfPacketType + urlOffset, SeekOrigin.Begin);
					url = reader.ReadBytes(urlSize);
				}
				else
					url = Array.Empty<byte>();

				byte[] data;
				if (dataOffset != 0)
				{
					reader.BaseStream.Seek(endOfPacketType + dataOffset, SeekOrigin.Begin);
					data = reader.ReadBytes(dataSize);
				}
				else
					data = Array.Empty<byte>();

				byte[] name;
				if (nameOffset != 0)
				{
					reader.BaseStream.Seek(endOfPacketType + nameOffset, SeekOrigin.Begin);
					name = reader.ReadBytes(nameSize);
				}
				else
					name = Array.Empty<byte>();

				reader.BaseStream.Seek(endOfPacketType + 108 + 48 * i, SeekOrigin.Begin);
				nameTableEntries[i] = ReadNameTableEntryInfo(reader) with
				{
					UrlInternal = url,
					Data = data.ToImmutableArray(),
					NameInternal = name,
				};
			}

			var nameTableMemberships = new NameTableMembershipInfo[membershipCount];
			for (var i = 0; i < nameTableMemberships.Length; i++)
			{
				reader.BaseStream.Seek(endOfPacketType + 108 + 48 * entryCount + 16 * i, SeekOrigin.Begin);
				nameTableMemberships[i] = ReadNameTableMembershipInfo(reader);
			}

			var applicationReservedData = ImmutableArray<byte>.Empty;
			if (applicationReservedDataOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + applicationReservedDataOffset, SeekOrigin.Begin);
				applicationReservedData = reader.ReadBytes(applicationReservedDataSize).ToImmutableArray();
			}

			var reservedData = ImmutableArray<byte>.Empty;
			if (reservedDataOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + reservedDataOffset, SeekOrigin.Begin);
				reservedData = reader.ReadBytes(reservedDataSize).ToImmutableArray();
			}

			var password = Array.Empty<byte>();
			if (passwordOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + passwordOffset, SeekOrigin.Begin);
				password = reader.ReadBytes(passwordSize);
			}

			var sessionName = Array.Empty<byte>();
			if (sessionNameOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + sessionNameOffset, SeekOrigin.Begin);
				sessionName = reader.ReadBytes(sessionNameSize);
			}

			var reply = ImmutableArray<byte>.Empty;
			if (replyOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + replyOffset, SeekOrigin.Begin);
				reply = reader.ReadBytes(replySize).ToImmutableArray();
			}

			return new()
			{
				Flags = flags,
				MaxPlayers = maxPlayers,
				CurrentPlayers = currentPlayers,
				GuidInstance = guidInstance,
				GuidApplication = guidApplication,
				Dpnid = dpnid,
				Version = version,
				VersionNotUsed = versionNotUsed,
				NameTableEntries = nameTableEntries.ToImmutableArray(),
				NameTableMemberships = nameTableMemberships.ToImmutableArray(),
				ApplicationReservedData = applicationReservedData,
				ReservedData = reservedData,
				PasswordInternal = password,
				SessionNameInternal = sessionName,
				Reply = reply,
			};
		}

		public virtual void WriteNameTableEntryInfo(BinaryWriter writer, NameTableEntryInfo value)
		{
			writer.Write(value.Dpnid.Value);
			writer.Write(value.DpnidOwner.Value);
			writer.Write((int)value.Flags);
			writer.Write(value.Version);
			writer.Write(value.VersionNotUsed);
			writer.Write((int)value.DnetVersion);
		}

		public virtual void WriteNameTableMembershipInfo(BinaryWriter writer, NameTableMembershipInfo value)
		{
			writer.Write(value.DpnidPlayer.Value);
			writer.Write(value.DpnidGroup.Value);
			writer.Write(value.Version);
			writer.Write(value.VersionNotUsed);
		}

		public virtual void Write(BinaryWriter writer, SendConnectInfoMessage message)
		{
			writer.Write((int)message.PacketType);
			var endOfPacketType = writer.BaseStream.Position;
			var offset = 108
				+ 48 * message.EntryCount
				+ 16 * message.MembershipCount
				+ message.NameTableEntries.Sum(e => e.UrlSize + e.DataSize + e.NameSize)
				+ message.ApplicationReservedDataSize
				+ message.ReservedDataSize
				+ message.PasswordSize
				+ message.SessionNameSize
				+ message.ReplySize;

			var replyOffset = 0;
			if (message.ReplySize != 0)
			{
				replyOffset = offset -= message.ReplySize;
				writer.BaseStream.Seek(endOfPacketType + replyOffset, SeekOrigin.Begin);
				writer.Write(message.Reply.ToArray());
			}

			var sessionNameOffset = 0;
			if (message.SessionNameSize != 0)
			{
				sessionNameOffset = offset -= message.SessionNameSize;
				writer.BaseStream.Seek(endOfPacketType + sessionNameOffset, SeekOrigin.Begin);
				writer.Write(message.SessionNameInternal.ToByteArray());
			}

			var passwordOffset = 0;
			if (message.PasswordSize != 0)
			{
				passwordOffset = offset -= message.PasswordSize;
				writer.BaseStream.Seek(endOfPacketType + passwordOffset, SeekOrigin.Begin);
				writer.Write(message.PasswordInternal.ToByteArray());
			}

			var reservedDataOffset = 0;
			if (message.ReservedDataSize != 0)
			{
				reservedDataOffset = offset -= message.ReservedDataSize;
				writer.BaseStream.Seek(endOfPacketType + reservedDataOffset, SeekOrigin.Begin);
				writer.Write(message.ReservedData.ToArray());
			}

			var applicationReservedDataOffset = 0;
			if (message.ApplicationReservedDataSize != 0)
			{
				applicationReservedDataOffset = offset -= message.ApplicationReservedDataSize;
				writer.BaseStream.Seek(endOfPacketType + applicationReservedDataOffset, SeekOrigin.Begin);
				writer.Write(message.ApplicationReservedData.ToArray());
			}

			var nameOffsets = new int[message.EntryCount];
			var dataOffsets = new int[message.EntryCount];
			var urlOffsets = new int[message.EntryCount];
			for (var i = 0; i < message.EntryCount; i++)
			{
				var e = message.NameTableEntries[i];

				nameOffsets[i] = 0;
				if (e.NameSize != 0)
				{
					nameOffsets[i] = offset -= e.NameSize;
					writer.BaseStream.Seek(endOfPacketType + nameOffsets[i], SeekOrigin.Begin);
					writer.Write(e.NameInternal.ToByteArray());
				}

				dataOffsets[i] = 0;
				if (e.DataSize != 0)
				{
					dataOffsets[i] = offset -= e.DataSize;
					writer.BaseStream.Seek(endOfPacketType + dataOffsets[i], SeekOrigin.Begin);
					writer.Write(e.Data.ToArray());
				}

				urlOffsets[i] = 0;
				if (e.UrlSize != 0)
				{
					urlOffsets[i] = offset -= e.UrlSize;
					writer.BaseStream.Seek(endOfPacketType + urlOffsets[i], SeekOrigin.Begin);
					writer.Write(e.UrlInternal.ToByteArray());
				}
			}

			writer.BaseStream.Seek(endOfPacketType, SeekOrigin.Begin);
			writer.Write(replyOffset);
			writer.Write(message.ReplySize);
			writer.Write(message.Size);
			writer.Write((int)message.Flags);
			writer.Write(message.MaxPlayers);
			writer.Write(message.CurrentPlayers);
			writer.Write(sessionNameOffset);
			writer.Write(message.SessionNameSize);
			writer.Write(passwordOffset);
			writer.Write(message.PasswordSize);
			writer.Write(reservedDataOffset);
			writer.Write(message.ReservedDataSize);
			writer.Write(applicationReservedDataOffset);
			writer.Write(message.ApplicationReservedDataSize);
			writer.Write(message.GuidInstance.ToByteArray());
			writer.Write(message.GuidApplication.ToByteArray());
			writer.Write(message.Dpnid.Value);
			writer.Write(message.Version);
			writer.Write(message.VersionNotUsed);
			writer.Write(message.EntryCount);
			writer.Write(message.MembershipCount);

			for (var i = 0; i < message.EntryCount; i++)
			{
				var e = message.NameTableEntries[i];
				WriteNameTableEntryInfo(writer, e);
				writer.Write(nameOffsets[i]);
				writer.Write(e.NameSize);
				writer.Write(dataOffsets[i]);
				writer.Write(e.DataSize);
				writer.Write(urlOffsets[i]);
				writer.Write(e.UrlSize);
			}

			foreach (var m in message.NameTableMemberships)
				WriteNameTableMembershipInfo(writer, m);
		}

		public virtual SendConnectInfoMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(SendConnectInfoMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
