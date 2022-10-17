// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/aa00e9b1-8169-4aef-aa4c-1b0619d45c24

namespace Aigamo.Otsuki.Messages.Core;

public enum ObjectType
{
	/// <summary>
	/// Connecting application is a client.
	/// </summary>
	Client = 0x00000002,

	/// <summary>
	/// Connecting application is a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_e5d0d91c-9a39-493f-ab1b-f36ce840e6a2">peer</see>.
	/// </summary>
	Peer = 0x00000004,
}
