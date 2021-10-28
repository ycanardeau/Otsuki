// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/aa00e9b1-8169-4aef-aa4c-1b0619d45c24

using System.Collections.Immutable;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// This is the first message passed into a host/server to initiate the connect sequence.
	/// </summary>
	[Immutable]
	public sealed record PlayerConnectInfoMessage : ICoreMessage
	{
		internal NullTerminatedAsciiString UrlInternal { get; init; } = string.Empty;

		internal NullTerminatedUnicodeString PasswordInternal { get; init; } = string.Empty;

		internal NullTerminatedUnicodeString NameInternal { get; init; } = string.Empty;

		/// <summary>
		/// A 32-bit field that contains the packet type.
		/// </summary>
		public PacketType PacketType { get; } = PacketType.PlayerConnectInfo;

		/// <summary>
		/// A 32-bit field that specifies the connect flags.
		/// </summary>
		public ObjectType Flags { get; init; }

		/// <summary>
		/// A 32-bit field that specifies the DirectPlay version.
		/// </summary>
		public DnetVersion DnetVersion { get; init; }

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the data in the <b>name</b> field. If <b>dwNameOffset</b> is set to 0, <b>dwNameSize</b> SHOULD also be 0. If <b>dwNameOffset</b> is not 0, <b>dwNameSize</b> SHOULD also not be 0.
		/// </summary>
		internal int NameSize => NameInternal.Length;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>data</b> field. If <b>dwDataOffset</b> is set to 0, <b>dwDataSize</b> SHOULD also be 0. If <b>dwDataOffset</b> is not 0, <b>dwDataSize</b> SHOULD also not be 0.
		/// </summary>
		internal int DataSize => Data.Count;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the password. If <b>dwPasswordOffset</b> is set to 0, <b>dwPasswordSize</b> SHOULD also be 0. If <b>dwPasswordOffset</b> is not 0, <b>dwPasswordSize</b> SHOULD also not be 0.
		/// </summary>
		internal int PasswordSize => PasswordInternal.Length;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>connectData</b> field. If <b>dwConnectDataOffset</b> is 0, <b>dwConnectDataSize</b> SHOULD also be 0. If <b>dwConnectDataOffset</b> is not 0, <b>dwConnectDataSize</b> SHOULD also not be 0.
		/// </summary>
		internal int ConnectDataSize => ConnectData.Count;

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>url</b> field. If <b>dwURLOffset</b> is 0, <b>dwURLSize</b> SHOULD also be 0. If <b>dwURLOffset</b> is not 0, <b>dwURLSize</b> SHOULD also not be 0.
		/// </summary>
		internal int UrlSize => UrlInternal.Length;

		/// <summary>
		/// A 128-bit field that contains the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_f49694cc-c350-462d-ab8e-816f0103c6c1">GUID</see> that identifies the particular instance of the server/host application to which the client/peer is attempting to connect. Each instance of a DirectPlay server/host application generates a new unique GUID each time the application hosts a new game session. In order for the client/peer to connect, the value of <b>guidInstance</b> MUST match the value of the GUID instance defined on the server/host or the value MUST be all zeroes. If a different, nonzero GUID instance value is specified, the recipient MUST send a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/7fc9f0e0-27d4-4975-b520-079737d5cb0b">DN_CONNECT_FAILED</see> message with the result code DPNERR_INVALIDINSTANCE (0x80158380) and terminate the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dpl8r/7a35d96c-daca-4311-bc2b-bd6a2f50bf14">[MC-DPL8R]</see> connection. For information on how a client/peer retrieves the value of the GUID instance defined on the server/host, see the description of the <b>ApplicationInstanceGUID</b> field in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dplhp/ef5b29b4-cc5b-4aea-82a4-c0f55296f1a2">EnumResponse</see> message defined in <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/../mc-dplhp/1a901a85-f85c-497c-aac7-1e172a894243">[MC-DPLHP]</see> section 2.2.2.
		/// </summary>
		public Guid GuidInstance { get; init; }

		/// <summary>
		/// A 128-bit field that specifies the application's assigned GUID. This is the unique identifier for the specific application, not per instance.
		/// </summary>
		public Guid GuidApplication { get; init; }

		/// <summary>
		/// A 32-bit field that specifies the size, in bytes, of the <b>alternateAddressData</b> field. If <b>dwAlternateAddressDataOffset</b> is set to 0, <b>dwAlternateAddressDataSize</b> SHOULD also be 0. If <b>dwAlternateAddressDataOffset</b> is not 0, <b>dwAlternateAddressDataSize</b> SHOULD also not be 0.
		/// </summary>
		internal int AlternateAddressDataSize => AlternateAddresses.Sum(a => a.ToByteArray().Length);

		/// <summary>
		/// A variable-length field that specifies alternative address data used to connect the client. This field's position is determined by <b>dwAlternateAddressDataOffset</b> and the size stated in <b>dwAlternateAddressDataSize</b>. The addresses that are passed into the <b>alternateAddressData</b> field are formatted via the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/bb72c589-3c28-4e77-ba36-995e1a825e6b">DN_ALTERNATE_ADDRESS</see> structure. Because DN_ALTERNATE_ADDRESS contains its own size, multiple alternate addresses can be passed in by appending the DN_ALTERNATE_ADDRESS structures together. However, the maximum number of alternate addresses that can be passed in at a single time is limited to 12.
		/// </summary>
		internal/* TODO: make public */ IImmutableList<AlternateAddress> AlternateAddresses { get; init; } = ImmutableArray<AlternateAddress>.Empty;

		/// <summary>
		/// A variable-length field that contains a 0-terminated byte character array that specifies the client URL. This field's position is determined by <b>dwURLOffset</b> and the size stated in <b>dwURLSize</b>. It is defined in <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/180a7d08-8b45-4b32-a971-4dfc6a19f9ac">DN_ADDRESSING_URL</see>.
		/// </summary>
		public string Url
		{
			get => UrlInternal;
			init => UrlInternal = value;
		}

		/// <summary>
		/// A variable-length field that contains a byte array that provides the connection data. This field's position is determined by <b>dwConnectDataOffset</b> and the size stated in <b>dwConnectDataSize</b>.
		/// </summary>
		public IImmutableList<byte> ConnectData { get; init; } = ImmutableArray<byte>.Empty;

		/// <summary>
		/// A variable-length field that contains a 0-terminated <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_30c30de3-1d00-4d0d-9109-fcc0094fc95a">wide character</see> array that specifies the application password data. This field's position is determined by <b>dwPasswordOffset</b> and the size stated in <b>dwPasswordSize</b>. This data is passed in clear text to the protocol layer.
		/// </summary>
		public string Password
		{
			get => PasswordInternal;
			init => PasswordInternal = value;
		}

		/// <summary>
		/// A variable-length field that contains a byte array that specifies the application data. This field's position is determined by <b>dwDataOffset</b> and the size stated in <b>dwDataSize</b>.
		/// </summary>
		public IImmutableList<byte> Data { get; init; } = ImmutableArray<byte>.Empty;

		/// <summary>
		/// A variable-length field that contains a 0-terminated wide character array that specifies the client/peer name. This field's position is determined by <b>dwNameOffset</b> and the size stated in <b>dwNameSize</b>.
		/// </summary>
		public string Name
		{
			get => NameInternal;
			init => NameInternal = value;
		}

		public PlayerConnectInfoMessage() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(PlayerConnectInfoMessage)}:");
			builder.AppendLine($"\t{nameof(Flags)}: {Flags}");
			builder.AppendLine($"\t{nameof(DnetVersion)}: {DnetVersion}");
			builder.AppendLine($"\t{nameof(GuidInstance)}: {GuidInstance}");
			builder.AppendLine($"\t{nameof(GuidApplication)}: {GuidApplication}");

			foreach (var a in AlternateAddresses)
				builder.AppendLine(string.Join("\n", a.ToString().Split('\n').Select(l => "\t" + l)));

			builder.AppendLine($"\t{nameof(Url)}: {Url}");
			builder.AppendLine($"\t{nameof(ConnectData)}: {BitConverter.ToString(ConnectData.ToArray())}");
			builder.AppendLine($"\t{nameof(Password)}: {Password}");
			builder.AppendLine($"\t{nameof(Data)}: {BitConverter.ToString(Data.ToArray())}");
			builder.AppendLine($"\t{nameof(Name)}: {Name}");
			return builder.ToString();
		}
	}

	internal class PlayerConnectInfoMessageSerializer : ICoreMessageSerializer<PlayerConnectInfoMessage>
	{
		public static PlayerConnectInfoMessageSerializer Default { get; } = new();

		public virtual PlayerConnectInfoMessage? Read(BinaryReader reader)
		{
			var packetType = (PacketType)reader.ReadInt32();
			if (packetType != PacketType.PlayerConnectInfo)
				return null;

			var endOfPacketType = reader.BaseStream.Position;
			var flags = (ObjectType)reader.ReadInt32();
			var dnetVersion = (DnetVersion)reader.ReadInt32();
			var nameOffset = reader.ReadInt32();
			var nameSize = reader.ReadInt32();
			var dataOffset = reader.ReadInt32();
			var dataSize = reader.ReadInt32();
			var passwordOffset = reader.ReadInt32();
			var passwordSize = reader.ReadInt32();
			var connectDataOffset = reader.ReadInt32();
			var connectDataSize = reader.ReadInt32();
			var urlOffset = reader.ReadInt32();
			var urlSize = reader.ReadInt32();
			var guidInstance = new Guid(reader.ReadBytes(Guid.Empty.ToByteArray().Length));
			var guidApplication = new Guid(reader.ReadBytes(Guid.Empty.ToByteArray().Length));
			var alternateAddressDataOffset = reader.ReadInt32();
			var alternateAddressDataSize = reader.ReadInt32();

			var alternateAddresses = ImmutableArray<AlternateAddress>.Empty;
			if (alternateAddressDataOffset != 0)
			{
				IEnumerable<AlternateAddress> ReadAlternateAddresses()
				{
					while (reader.BaseStream.Position < (endOfPacketType + alternateAddressDataOffset + alternateAddressDataSize))
						yield return AlternateAddress.FromBinaryReader(reader);
				}

				reader.BaseStream.Seek(endOfPacketType + alternateAddressDataOffset, SeekOrigin.Begin);
				alternateAddresses = ReadAlternateAddresses().ToImmutableArray();
			}

			var url = Array.Empty<byte>();
			if (urlOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + urlOffset, SeekOrigin.Begin);
				url = reader.ReadBytes(urlSize);
			}

			var connectData = ImmutableArray<byte>.Empty;
			if (connectDataOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + connectDataOffset, SeekOrigin.Begin);
				connectData = reader.ReadBytes(connectDataSize).ToImmutableArray();
			}

			var password = Array.Empty<byte>();
			if (passwordOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + passwordOffset, SeekOrigin.Begin);
				password = reader.ReadBytes(passwordSize);
			}

			var data = ImmutableArray<byte>.Empty;
			if (dataOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + dataOffset, SeekOrigin.Begin);
				data = reader.ReadBytes(dataSize).ToImmutableArray();
			}

			var name = Array.Empty<byte>();
			if (nameOffset != 0)
			{
				reader.BaseStream.Seek(endOfPacketType + nameOffset, SeekOrigin.Begin);
				name = reader.ReadBytes(nameSize);
			}

			return new()
			{
				Flags = flags,
				DnetVersion = dnetVersion,
				GuidInstance = guidInstance,
				GuidApplication = guidApplication,
				AlternateAddresses = alternateAddresses,
				UrlInternal = url,
				ConnectData = connectData,
				PasswordInternal = password,
				Data = data,
				NameInternal = name,
			};
		}

		public virtual void Write(BinaryWriter writer, PlayerConnectInfoMessage message)
		{
			writer.Write((int)message.PacketType);
			var endOfPacketType = writer.BaseStream.Position;
			var offset = 88
				+ message.AlternateAddressDataSize
				+ message.UrlSize
				+ message.ConnectDataSize
				+ message.PasswordSize
				+ message.DataSize
				+ message.NameSize;

			var nameOffset = 0;
			if (message.NameSize != 0)
			{
				nameOffset = offset -= message.NameSize;
				writer.BaseStream.Seek(endOfPacketType + nameOffset, SeekOrigin.Begin);
				writer.Write(message.NameInternal.ToByteArray());
			}

			var dataOffset = 0;
			if (message.DataSize != 0)
			{
				dataOffset = offset -= message.DataSize;
				writer.BaseStream.Seek(endOfPacketType + dataOffset, SeekOrigin.Begin);
				writer.Write(message.Data.ToArray());
			}

			var passwordOffset = 0;
			if (message.PasswordSize != 0)
			{
				passwordOffset = offset -= message.PasswordSize;
				writer.BaseStream.Seek(endOfPacketType + passwordOffset, SeekOrigin.Begin);
				writer.Write(message.PasswordInternal.ToByteArray());
			}

			var connectDataOffset = 0;
			if (message.ConnectDataSize != 0)
			{
				connectDataOffset = offset -= message.ConnectDataSize;
				writer.BaseStream.Seek(endOfPacketType + connectDataOffset, SeekOrigin.Begin);
				writer.Write(message.ConnectData.ToArray());
			}

			var urlOffset = 0;
			if (message.UrlSize != 0)
			{
				urlOffset = offset -= message.UrlSize;
				writer.BaseStream.Seek(endOfPacketType + urlOffset, SeekOrigin.Begin);
				writer.Write(message.UrlInternal.ToByteArray());
			}

			var alternateAddressDataOffset = 0;
			if (message.AlternateAddressDataSize != 0)
			{
				alternateAddressDataOffset = offset -= message.AlternateAddressDataSize;
				writer.BaseStream.Seek(endOfPacketType + alternateAddressDataOffset, SeekOrigin.Begin);
				writer.Write(message.AlternateAddresses.SelectMany(a => a.ToByteArray()).ToArray());
			}

			writer.BaseStream.Seek(endOfPacketType, SeekOrigin.Begin);
			writer.Write((int)message.Flags);
			writer.Write((int)message.DnetVersion);
			writer.Write(nameOffset);
			writer.Write(message.NameSize);
			writer.Write(dataOffset);
			writer.Write(message.DataSize);
			writer.Write(passwordOffset);
			writer.Write(message.PasswordSize);
			writer.Write(connectDataOffset);
			writer.Write(message.ConnectDataSize);
			writer.Write(urlOffset);
			writer.Write(message.UrlSize);
			writer.Write(message.GuidInstance.ToByteArray());
			writer.Write(message.GuidApplication.ToByteArray());
			writer.Write(alternateAddressDataOffset);
			writer.Write(message.AlternateAddressDataSize);
		}

		public virtual PlayerConnectInfoMessage? Deserialize(byte[] data)
		{
			using var stream = new MemoryStream(data);
			using var reader = new BinaryReader(stream);
			return Read(reader);
		}

		public virtual byte[] Serialize(PlayerConnectInfoMessage message)
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);
			Write(writer, message);
			return stream.ToArray();
		}
	}
}
