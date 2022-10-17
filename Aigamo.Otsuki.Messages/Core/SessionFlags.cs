// Comments from: https://docs.microsoft.com/en-us/previous-versions/windows/desktop/bb320089(v=vs.85)
// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/075e84f0-b26c-4f10-9f69-21a0813dfc54

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// Flag that controls how this method is processed.
/// </summary>
[Flags]
public enum SessionFlags
{
	/// <summary>
	/// A client/server game session.
	/// </summary>
	ClientServer = 0x00000001,

	/// <summary>
	/// <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_72e4727d-d1b3-4503-b803-78da6f5e9fea">Host migration</see> is allowed.
	/// </summary>
	MigrateHost = 0x00000004,

	/// <summary>
	/// The <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_1a73cb63-9f53-49e9-9f3e-19ce82ab5a6d">DirectPlay</see> enumeration server is not running.
	/// </summary>
	NoDpnServer = 0x00000040,

	/// <summary>
	/// Password is REQUIRED.
	/// </summary>
	RequirePassword = 0x00000080,

	/// <summary>
	/// No enumerations are allowed from the game session. This value is only available in DirectPlay 9.
	/// </summary>
	NoEnumerations = 0x00000100,

	/// <summary>
	/// Fast signing is turned on for the game session. Passed to protocol layer. Cannot be used with <b>DPNSESSION_FULL_SIGNED</b>. This value is available only in DirectPlay 9.
	/// </summary>
	FastSigned = 0x00000200,

	/// <summary>
	/// Full signing turned on for the game session. Passed to protocol layer. Cannot be used with <b>DPNSESSION_FAST_SIGNED</b>. This value is available only in DirectPlay 9.
	/// </summary>
	FullSigned = 0x00000400,
}
