// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/97f93510-ca87-4df6-9af1-af5d930e42fa

namespace Aigamo.Otsuki.Messages.Reliable
{
	[Flags]
	public enum PacketCommand : byte
	{
		/// <summary>
		/// frame contains user data
		/// </summary>
		Data = 0x01,

		/// <summary>
		/// frame is delivered reliably
		/// </summary>
		Reliable = 0x02,

		/// <summary>
		/// frame is indicated sequentially
		/// </summary>
		Sequential = 0x04,

		/// <summary>
		/// <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_7b0ee975-d1b4-4a39-865d-d13e6c96aa76">partner</see> acknowledges immediately
		/// </summary>
		Poll = 0x08,

		/// <summary>
		/// DFRAME is first in message
		/// </summary>
		NewMessage = 0x10,

		/// <summary>
		/// DFRAME is last in message
		/// </summary>
		EndMessage = 0x20,

		/// <summary>
		/// first consumer-controlled flag
		/// </summary>
		User1 = 0x40,

		/// <summary>
		/// second consumer-controlled flag
		/// </summary>
		User2 = 0x80,

		/// <summary>
		/// <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_0cca315e-b199-4f2f-a739-0e2e3582a00e">command frame (CFRAME)</see>
		/// </summary>
		CommandFrame = 0x80,
	}
}
