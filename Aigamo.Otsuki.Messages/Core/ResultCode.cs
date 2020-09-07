// Code from: https://source.winehq.org/git/wine.git/blob/HEAD:/include/dplay8.h
// Comments from: https://docs.microsoft.com/en-us/previous-versions/windows/desktop/bb320091(v=vs.85)
// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/7fc9f0e0-27d4-4975-b520-079737d5cb0b

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// Result codes that can be returned via events.
	/// </summary>
	public enum ResultCode
	{
		Success = 0x00000000,

		/// <summary>
		/// An undetermined error occurred inside a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_94fdbc37-5f95-4e9a-a289-964c7da35edc">DirectX</see> subsystem. This includes uncommon errors that cannot be generalized.
		/// </summary>
		Generic = unchecked((int)0x80004005),

		InvalidParam = unchecked((int)0x80070057),
		Unsupported = unchecked((int)0x80004001),
		NoInterface = unchecked((int)0x80004002),
		OutOfMemory = unchecked((int)0x8007000E),
		InvalidPointer = unchecked((int)0x80004003),
		//Pending = unchecked((int)),
		Aborted = unchecked((int)0x80158030),
		Addressing = unchecked((int)0x80158040),

		/// <summary>
		/// Server/host is closing or host is migrating.
		/// </summary>
		AlreadyClosing = unchecked((int)0x80158050),

		AlreadyConnected = unchecked((int)0x80158060),
		AlreadyDisconnecting = unchecked((int)0x80158070),
		AlreadyInitialized = unchecked((int)0x80158080),
		AlreadyRegistered = unchecked((int)0x80158090),
		BufferTooSmall = unchecked((int)0x80158100),
		CannotCancel = unchecked((int)0x80158110),
		CannotCreateGroup = unchecked((int)0x80158120),
		CannotCreatePlayer = unchecked((int)0x80158130),
		CannotLaunchApplication = unchecked((int)0x80158140),
		Connecting = unchecked((int)0x80158150),
		ConnectionLost = unchecked((int)0x80158160),
		Conversion = unchecked((int)0x80158170),
		DataTooLarge = unchecked((int)0x80158175),
		DoesNotExist = unchecked((int)0x80158180),
		DpnServerNotAvailable = unchecked((int)0x80158185),
		DuplicateCommand = unchecked((int)0x80158190),
		EndPointNotReceiving = unchecked((int)0x80158200),
		EnumQueryTooLarge = unchecked((int)0x80158210),
		EnumResponseTooLarge = unchecked((int)0x80158220),
		Exception = unchecked((int)0x80158230),
		GroupNotEmpty = unchecked((int)0x80158240),
		Hosting = unchecked((int)0x80158250),

		/// <summary>
		/// Application declined connection attempt.
		/// </summary>
		HostRejectedConnection = unchecked((int)0x80158260),

		HostTerminatedSession = unchecked((int)0x80158270),
		IncompleteAddress = unchecked((int)0x80158280),
		InvalidAddressFormat = unchecked((int)0x80158290),

		/// <summary>
		/// Application GUID is not valid for this application.
		/// </summary>
		InvalidApplication = unchecked((int)0x80158300),

		InvalidCommand = unchecked((int)0x80158310),
		InvalidDeviceAddress = unchecked((int)0x80158320),
		InvalidEndPoint = unchecked((int)0x80158330),
		InvalidFlags = unchecked((int)0x80158340),
		InvalidGroup = unchecked((int)0x80158350),
		InvalidHandle = unchecked((int)0x80158360),
		InvalidHostAddress = unchecked((int)0x80158370),

		/// <summary>
		/// Instance <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_f49694cc-c350-462d-ab8e-816f0103c6c1">GUID</see> is not valid for this <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
		/// </summary>
		InvalidInstance = unchecked((int)0x80158380),

		/// <summary>
		/// Nonclient attempting to connect to a server. Nonpeer attempting to connect to a host/peer.
		/// </summary>
		InvalidInterface = unchecked((int)0x80158390),

		InvalidObject = unchecked((int)0x80158400),

		/// <summary>
		/// Password passed in does not match what is expected.
		/// </summary>
		InvalidPassword = unchecked((int)0x80158410),

		InvalidPlayer = unchecked((int)0x80158420),
		InvalidPriority = unchecked((int)0x80158430),
		InvalidString = unchecked((int)0x80158440),
		InvalidUrl = unchecked((int)0x80158450),

		/// <summary>
		/// Version passed in is not a valid <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_1a73cb63-9f53-49e9-9f3e-19ce82ab5a6d">DirectPlay</see> version.
		/// </summary>
		InvalidVersion = unchecked((int)0x80158460),

		NoCaps = unchecked((int)0x80158470),
		NoConnection = unchecked((int)0x80158480),
		NoHostPlayer = unchecked((int)0x80158490),
		NoMoreAddressComponents = unchecked((int)0x80158500),
		NoResponse = unchecked((int)0x80158510),
		NotAllowed = unchecked((int)0x80158520),

		/// <summary>
		/// Attempting to connect to an application that is not the host/server.
		/// </summary>
		NotHost = unchecked((int)0x80158530),

		NotReady = unchecked((int)0x80158540),
		NotRegistered = unchecked((int)0x80158550),
		PlayerAlreadyInGroup = unchecked((int)0x80158560),
		PlayerLost = unchecked((int)0x80158570),
		PlayerNotInGroup = unchecked((int)0x80158580),
		PlayerNotReachable = unchecked((int)0x80158590),
		SendTooLarge = unchecked((int)0x80158600),
		SessionFull = unchecked((int)0x80158610),
		TableFull = unchecked((int)0x80158620),
		TimedOut = unchecked((int)0x80158630),
		Uninitialized = unchecked((int)0x80158640),
		UserCancel = unchecked((int)0x80158650),
	}
}
