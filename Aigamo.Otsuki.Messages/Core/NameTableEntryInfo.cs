// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/87370443-4103-4153-a8ac-a6728c39b7e5

using System.Collections.Immutable;
using System.Text;
using Flags = Aigamo.Otsuki.Messages.Core.NameTableEntryFlags;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_NAMETABLE_ENTRY_INFO contains a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_0258d4e2-d4f7-4099-ae0f-a02fad73e824">player</see> or <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_51c51c14-7f9d-4c0b-a69c-d3e059bfffac">group</see> that exists in a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cbddab17-6b4a-45ad-b121-a5d8695501c5">DirectPlay 8</see> <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table</see>. This includes all the information that the DirectPlay 8 Protocol: Core and Service Providers would need about a certain entry.
/// </summary>
[Immutable]
public sealed record NameTableEntryInfo
{
	internal NullTerminatedAsciiString UrlInternal { get; init; } = string.Empty;

	internal NullTerminatedUnicodeString NameInternal { get; init; } = string.Empty;

	/// <summary>
	/// A 32-bit integer that specifies the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_1a73cb63-9f53-49e9-9f3e-19ce82ab5a6d">DirectPlay</see> identifier (<see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_92d4e8fa-d8ba-4d9e-bddf-5d9ce7567002">DPNID</see>) of the player or group that has been defined by the host/server. For more information about DPNIDs, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid Dpnid { get; init; }

	/// <summary>
	/// A 32-bit integer that provides the DirectPlay identifier (DPNID) for the owner of the player or group. When the DN_NAMETABLE_ENTRY_INFO message represents a group, that is, <b>NAMETABLE_ENTRY_FLAG_GROUP</b> is set in the <b>dwFlags</b> field, the <b>dpnidOwner</b> field MUST be nonzero. When DN_NAMETABLE_ENTRY_INFO represents a player, <b>dpnidOwner</b> SHOULD be set to zero when sending and MUST be ignored on receipt. For more information about DPNIDs, see section 2.2.7.
	/// </summary>
	public Dpnid DpnidOwner { get; init; }

	/// <summary>
	/// A 32-bit integer that specifies the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_f3af3b08-b79e-477d-a4ad-76f4168485e4">name table entry</see> flags. Entries are OR'd together.
	/// </summary>
	public Flags Flags { get; internal init; }

	/// <summary>
	/// A 32-bit integer that specifies the version number of the name table.
	/// </summary>
	public int Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	internal int VersionNotUsed { get; init; }

	/// <summary>
	/// A 32-bit integer that provides the DirectPlay version.
	/// </summary>
	public DnetVersion DnetVersion { get; init; }

	/// <summary>
	/// The size, in bytes, of the <b>name</b> field. (Specified in section 2.2.1.4). If <b>dwNameOffset</b> is 0, <b>dwNameSize</b> SHOULD also be 0. If <b>dwNameOffset</b> is not 0, <b>dwNameSize</b> SHOULD also not be 0.
	/// </summary>
	internal int NameSize => NameInternal.Length;

	/// <summary>
	/// The size, in bytes, of the <b>data</b> field. If <b>dwDataOffset</b> is 0, <b>dwDataSize</b> SHOULD also be 0. If <b>dwDataOffset</b> is not 0, <b>dwDataSize</b> SHOULD also not be 0.
	/// </summary>
	internal int DataSize => Data.Count;

	/// <summary>
	/// The size, in bytes, of the <b>url</b> field.
	/// </summary>
	internal int UrlSize => UrlInternal.Length;

	/// <summary>
	/// A variable-length field that contains a 0-terminated character array that provides the URL of a user in the game session. This field's position is determined by <b>dwURLOffset</b> and the size stated in <b>dwURLSize</b>, both fields in the corresponding DN_NAMETABLE_ENTRY_INFO structure. There can be multiple instances of the URL field, as defined by the number of DN_NAMETABLE_ENTRY_INFO sections that are included.
	/// </summary>
	public string Url
	{
		get => UrlInternal;
		init => UrlInternal = value;
	}

	/// <summary>
	/// A variable-length field that contains a 0-terminated character array that specifies the user data. This field's position is determined by <b>dwDataOffset</b> and the size stated in <b>dwDataSize</b>, both fields in the corresponding DN_NAMETABLE_ENTRY_INFO structure. There can be multiple instances of the Data field, as defined by the number of DN_NAMETABLE_ENTRY_INFO sections that are included.
	/// </summary>
	public IImmutableList<byte> Data { get; init; } = ImmutableArray<byte>.Empty;

	/// <summary>
	/// A variable-length field that contains a 0-terminated <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_30c30de3-1d00-4d0d-9109-fcc0094fc95a">wide character</see> array that contains the client name. This field's position is determined by <b>dwNameOffset</b> and the size stated in <b>dwNameSize</b>, both fields in the corresponding DN_NAMETABLE_ENTRY_INFO structure. There can be multiple instances of the Name field, as defined by the number of DN_NAMETABLE_ENTRY_INFO sections that are included.
	/// </summary>
	public string Name
	{
		get => NameInternal;
		init => NameInternal = value;
	}

	public NameTableEntryInfo() { }

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
		builder.AppendLine($"{nameof(NameTableEntryInfo)}:");
		builder.AppendLine($"\t{nameof(Dpnid)}: {Dpnid}");
		builder.AppendLine($"\t{nameof(DpnidOwner)}: {DpnidOwner}");
		builder.AppendLine($"\t{nameof(Flags)}: {Flags}");
		builder.AppendLine($"\t{nameof(Version)}: {Version}");
		builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
		builder.AppendLine($"\t{nameof(DnetVersion)}: {DnetVersion}");
		builder.AppendLine($"\t{nameof(Url)}: {Url}");
		builder.AppendLine($"\t{nameof(Data)}: {BitConverter.ToString(Data.ToArray())}");
		builder.AppendLine($"\t{nameof(Name)}: {Name}");
		return builder.ToString();
	}
}
