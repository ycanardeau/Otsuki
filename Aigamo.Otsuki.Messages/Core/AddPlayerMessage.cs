// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8d48ddad-232f-480a-9a50-5b531d971ec2

using System.Collections.Immutable;
using System.Text;
using Flags = Aigamo.Otsuki.Messages.Core.NameTableEntryFlags;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_ADD_PLAYER packet is sent from the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> and instructs <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peers</see> to add a specified peer to the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
/// </summary>
[Immutable]
public sealed record AddPlayerMessage : ICoreMessage
{
	internal NullTerminatedAsciiString UrlInternal { get; init; } = string.Empty;

	internal NullTerminatedUnicodeString NameInternal { get; init; } = string.Empty;

	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.AddPlayer;

	/// <summary>
	/// A 32-bit field that contains the identifier of the peer to add. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid Dpnid { get; init; }

	/// <summary>
	/// A 32-bit field that contains the identifier of the game session owner. For more information, see section 2.2.7.
	/// </summary>
	public Dpnid DpnidOwner { get; init; }

	/// <summary>
	/// A 32-bit field that contains player flags.
	/// </summary>
	public Flags Flags { get; internal init; }

	/// <summary>
	/// A 32-bit field that specifies the current name table version number.
	/// </summary>
	public int Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	internal int VersionNotUsed { get; init; }

	/// <summary>
	/// A 32-bit field that contains the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_1a73cb63-9f53-49e9-9f3e-19ce82ab5a6d">DirectPlay</see> version of the client being added to the game session.
	/// </summary>
	public DnetVersion DnetClientVersion { get; init; }

	/// <summary>
	/// A 32-bit field that specifies the size, in bytes, of the name. If <b>dwNameOffset</b> is 0, <b>dwNameSize</b> SHOULD also be 0. If <b>dwNameOffset</b> is not 0, <b>dwNameSize</b> SHOULD also not be 0.
	/// </summary>
	internal int NameSize => NameInternal.Length;

	/// <summary>
	/// A 32-bit field that specifies the size, in bytes, of the peer data. If <b>dwDataOffset</b> is 0, <b>dwDataSize</b> SHOULD also be 0. If <b>dwDataOffset</b> is not 0, <b>dwDataSize</b> SHOULD also not be 0.
	/// </summary>
	internal int DataSize => Data.Count;

	/// <summary>
	/// A 32-bit field that specifies the size, in bytes, of the connecting peer's URL address.
	/// </summary>
	internal int UrlSize => UrlInternal.Length;

	/// <summary>
	/// A variable-length field that contains an array of characters that specify the client URL.
	/// </summary>
	public string Url
	{
		get => UrlInternal;
		init => UrlInternal = value;
	}

	/// <summary>
	/// A variable-length field that specifies a byte array of characters that contain user data.
	/// </summary>
	public IImmutableList<byte> Data { get; init; } = ImmutableArray<byte>.Empty;

	/// <summary>
	/// A variable-length field that specifies an array of <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_30c30de3-1d00-4d0d-9109-fcc0094fc95a">wide characters</see> that contain the peer name including the NULL termination character.
	/// </summary>
	public string Name
	{
		get => NameInternal;
		init => NameInternal = value;
	}

	public AddPlayerMessage() { }

	public bool Local
	{
		get => Flags.HasFlag(Flags.Local);
		init => Flags = value ? (Flags | Flags.Local) : (Flags & ~Flags.Local);
	}

	public bool Host
	{
		get => Flags.HasFlag(Flags.Host);
		init => Flags = value ? (Flags | Flags.Host) : (Flags & ~Flags.Host);
	}

	public bool AllPlayersGroup
	{
		get => Flags.HasFlag(Flags.AllPlayersGroup);
		init => Flags = value ? (Flags | Flags.AllPlayersGroup) : (Flags & ~Flags.AllPlayersGroup);
	}

	public bool Group
	{
		get => Flags.HasFlag(Flags.Group);
		init => Flags = value ? (Flags | Flags.Group) : (Flags & ~Flags.Group);
	}

	public bool GroupAutoDestruct
	{
		get => Flags.HasFlag(Flags.GroupAutoDestruct);
		init => Flags = value ? (Flags | Flags.GroupAutoDestruct) : (Flags & ~Flags.GroupAutoDestruct);
	}

	public bool Peer
	{
		get => Flags.HasFlag(Flags.Peer);
		init => Flags = value ? (Flags | Flags.Peer) : (Flags & ~Flags.Peer);
	}

	public bool Client
	{
		get => Flags.HasFlag(Flags.Client);
		init => Flags = value ? (Flags | Flags.Client) : (Flags & ~Flags.Client);
	}

	public bool Server
	{
		get => Flags.HasFlag(Flags.Server);
		init => Flags = value ? (Flags | Flags.Server) : (Flags & ~Flags.Server);
	}

	public bool Connecting
	{
		get => Flags.HasFlag(Flags.Connecting);
		init => Flags = value ? (Flags | Flags.Connecting) : (Flags & ~Flags.Connecting);
	}

	public bool Available
	{
		get => Flags.HasFlag(Flags.Available);
		init => Flags = value ? (Flags | Flags.Available) : (Flags & ~Flags.Available);
	}

	public bool Disconnecting
	{
		get => Flags.HasFlag(Flags.Disconnecting);
		init => Flags = value ? (Flags | Flags.Disconnecting) : (Flags & ~Flags.Disconnecting);
	}

	public bool Indicated
	{
		get => Flags.HasFlag(Flags.Indicated);
		init => Flags = value ? (Flags | Flags.Indicated) : (Flags & ~Flags.Indicated);
	}

	public bool Created
	{
		get => Flags.HasFlag(Flags.Created);
		init => Flags = value ? (Flags | Flags.Created) : (Flags & ~Flags.Created);
	}

	public bool NeedToDestroy
	{
		get => Flags.HasFlag(Flags.NeedToDestroy);
		init => Flags = value ? (Flags | Flags.NeedToDestroy) : (Flags & ~Flags.NeedToDestroy);
	}

	public bool InUse
	{
		get => Flags.HasFlag(Flags.InUse);
		init => Flags = value ? (Flags | Flags.InUse) : (Flags & ~Flags.InUse);
	}

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(AddPlayerMessage)}:");
		builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
		builder.AppendLine($"\t{nameof(DpnidOwner)}: {DpnidOwner}");
		builder.AppendLine($"\t{nameof(Flags)}: {Flags}");
		builder.AppendLine($"\t{nameof(Version)}: {Version}");
		builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
		builder.AppendLine($"\t{nameof(DnetClientVersion)}: {DnetClientVersion}");
		builder.AppendLine($"\t{nameof(Url)}: {Url}");
		builder.AppendLine($"\t{nameof(Data)}: {BitConverter.ToString(Data.ToArray())}");
		builder.AppendLine($"\t{nameof(Name)}: {Name}");
		return builder.ToString();
	}
}

internal class AddPlayerMessageSerializer : ICoreMessageSerializer<AddPlayerMessage>
{
	public static AddPlayerMessageSerializer Default { get; } = new();

	public virtual AddPlayerMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.AddPlayer)
			return null;

		var endOfPacketType = reader.BaseStream.Position;
		var dpnid = new Dpnid(reader.ReadInt32());
		var dpnidOwner = new Dpnid(reader.ReadInt32());
		var flags = (Flags)reader.ReadInt32();
		var version = reader.ReadInt32();
		var versionNotUsed = reader.ReadInt32();
		var dnetClientVersion = (DnetVersion)reader.ReadInt32();
		var nameOffset = reader.ReadInt32();
		var nameSize = reader.ReadInt32();
		var dataOffset = reader.ReadInt32();
		var dataSize = reader.ReadInt32();
		var urlOffset = reader.ReadInt32();
		var urlSize = reader.ReadInt32();

		var url = Array.Empty<byte>();
		if (urlOffset != 0)
		{
			reader.BaseStream.Seek(endOfPacketType + urlOffset, SeekOrigin.Begin);
			url = reader.ReadBytes(urlSize);
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
			Dpnid = dpnid,
			DpnidOwner = dpnidOwner,
			Flags = flags,
			Version = version,
			VersionNotUsed = versionNotUsed,
			DnetClientVersion = dnetClientVersion,
			UrlInternal = url,
			Data = data,
			NameInternal = name,
		};
	}

	public virtual void Write(BinaryWriter writer, AddPlayerMessage message)
	{
		writer.Write((int)message.PacketType);
		var endOfPacketType = writer.BaseStream.Position;
		var offset = 48 + message.UrlSize + message.DataSize + message.NameSize;

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

		var urlOffset = 0;
		if (message.UrlSize != 0)
		{
			urlOffset = offset -= message.UrlSize;
			writer.BaseStream.Seek(endOfPacketType + urlOffset, SeekOrigin.Begin);
			writer.Write(message.UrlInternal.ToByteArray());
		}

		writer.BaseStream.Seek(endOfPacketType, SeekOrigin.Begin);
		writer.Write(message.Dpnid.Value);
		writer.Write(message.DpnidOwner.Value);
		writer.Write((int)message.Flags);
		writer.Write(message.Version);
		writer.Write(message.VersionNotUsed);
		writer.Write((int)message.DnetClientVersion);
		writer.Write(nameOffset);
		writer.Write(message.NameSize);
		writer.Write(dataOffset);
		writer.Write(message.DataSize);
		writer.Write(urlOffset);
		writer.Write(message.UrlSize);
	}

	public virtual AddPlayerMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(AddPlayerMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
