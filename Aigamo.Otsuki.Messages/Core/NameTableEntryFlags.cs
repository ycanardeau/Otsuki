// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/87370443-4103-4153-a8ac-a6728c39b7e5

namespace Aigamo.Otsuki.Messages.Core;

[Flags]
public enum NameTableEntryFlags
{
	/// <summary>
	/// The name table entry is the local player.
	/// </summary>
	Local = 0x00000001,

	/// <summary>
	/// The name table entry is the host.
	/// </summary>
	Host = 0x00000002,

	/// <summary>
	/// The name table entry is the All Players Group.
	/// </summary>
	AllPlayersGroup = 0x00000004,

	/// <summary>
	/// The name table entry is a group.
	/// </summary>
	Group = 0x00000010,

	/// <summary>
	/// The name table entry supports group autodestruct.
	/// </summary>
	GroupAutoDestruct = 0x00000040,

	/// <summary>
	/// The name table entry is a peer. In peer-to-peer mode, the name table entry representing the host of the game session is also marked as a peer.
	/// </summary>
	Peer = 0x00000100,

	/// <summary>
	/// The name table entry is a client.
	/// </summary>
	Client = 0x00000200,

	/// <summary>
	/// The name table entry is a server.
	/// </summary>
	Server = 0x00000400,

	/// <summary>
	/// The name table entry is connecting.
	/// </summary>
	Connecting = 0x00001000,

	/// <summary>
	/// The name table entry is to make the member available for use.
	/// </summary>
	Available = 0x00002000,

	/// <summary>
	/// The name table entry to indicate disconnecting.
	/// </summary>
	Disconnecting = 0x00004000,

	/// <summary>
	/// The name table entry to indicate connection to the application.
	/// </summary>
	Indicated = 0x00010000,

	/// <summary>
	/// The name table entry to indicate the application was given a created player.
	/// </summary>
	Created = 0x00020000,

	/// <summary>
	/// The name table entry to indicate the need to destroy the player.
	/// </summary>
	NeedToDestroy = 0x00040000,

	/// <summary>
	/// The name table entry to indicate that the player is in use.
	/// </summary>
	InUse = 0x00080000,
}
