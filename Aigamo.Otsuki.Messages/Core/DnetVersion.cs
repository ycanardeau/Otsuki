// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/d2f5c735-5947-4b76-98c0-d447d9d36de8

namespace Aigamo.Otsuki.Messages.Core;

public enum DnetVersion
{
	/// <summary>
	/// DirectX 8.0
	/// </summary>
	DirectX80 = 0x00000001,

	/// <summary>
	/// DirectX 8.1
	/// </summary>
	DirectX81 = 0x00000002,

	/// <summary>
	/// PocketPC
	/// </summary>
	PocketPC = 0x00000003,

	/// <summary>
	/// Windows Server 2003 operating system
	/// </summary>
	WindowsServer2003 = 0x00000005,

	/// <summary>
	/// DirectX 8.2
	/// </summary>
	DirectX82 = 0x00000006,

	/// <summary>
	/// DirectX 9.0
	/// </summary>
	DirectX90 = 0x00000007,
}
