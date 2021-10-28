// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/2968b3eb-a248-4281-b718-8a7d55fd9b36

namespace Aigamo.Otsuki.Messages.Core;

public enum PacketType
{
	/// <summary>
	/// Sends client/peer connection information to the server/host.
	/// </summary>
	PlayerConnectInfo = 0x000000C1,

	/// <summary>
	/// Connection attempt failed.
	/// </summary>
	ConnectFailed = 0x000000C5,

	/// <summary>
	/// The server/host response to a client/peer that contains game session information.
	/// </summary>
	SendConnectInfo = 0x000000C2,

	/// <summary>
	/// Instructs peers to add the specified peer to the game session.
	/// </summary>
	AddPlayer = 0x000000D0,

	/// <summary>
	/// Acknowledges (ACK) the receipt of <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see> information.
	/// </summary>
	AckConnectInfo = 0x000000C3,

	/// <summary>
	/// Instructs a peer to connect to a designated peer.
	/// </summary>
	InstructConnect = 0x000000C6,

	/// <summary>
	/// Sends user identification to another client/peer.
	/// </summary>
	SendPlayerDpnid = 0x000000C4,

	/// <summary>
	/// Indicates that a peer was unable to carry out a host's instruction to connect to a new peer.
	/// </summary>
	InstructedConnectFailed = 0x000000C7,

	/// <summary>
	/// Indicates from the host that an existing peer was unable to carry out the host's instruction to connect to a new peer.
	/// </summary>
	ConnectAttemptFailed = 0x000000C8,

	/// <summary>
	/// Instructs the client or the peer to close and disconnect itself from the game session.
	/// </summary>
	TerminateSession = 0x000000DF,

	/// <summary>
	/// Instructs the peer to remove the specified peer from the name table.
	/// </summary>
	DestroyPlayer = 0x000000D1,

	/// <summary>
	/// Notified peers in the game session that the host is currently migrating.
	/// </summary>
	HostMigrate = 0x000000CD,

	/// <summary>
	/// Specifies the version number of the name table.
	/// </summary>
	NameTableVersion = 0x000000C9,

	/// <summary>
	/// Requests that the name table version number be resynchronized to the current version number.
	/// </summary>
	ResyncVersion = 0x000000CA,

	/// <summary>
	/// Requests that the host determine whether a target peer is still in the game session.
	/// </summary>
	RequestIntegrityCheck = 0x000000E2,

	/// <summary>
	/// Host is requesting a peer to validate that it is still in the game session.
	/// </summary>
	IntegrityCheck = 0x000000E3,

	/// <summary>
	/// Host is requesting a peer to validate that it is still in the game session.
	/// </summary>
	IntegrityCheckResponse = 0x000000E4,

	/// <summary>
	/// Sent from the host after a migration requesting the name table from a peer with a newer name table, if any exists.
	/// </summary>
	RequestNameTableOperations = 0x000000CB,

	/// <summary>
	/// Sent from the peer to the new host, acknowledging the new name table information.
	/// </summary>
	AckNameTableOperations = 0x000000CC,

	/// <summary>
	/// Informs peers that the session-hosting responsibilities have successfully migrated from the departing old host.
	/// </summary>
	HostMigrateComplete = 0x000000CE,
}
