// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/9ca3f71c-1040-40b0-9866-2e2e40cf65b0

namespace Aigamo.Otsuki.Messages.Reliable;

[Flags]
public enum SackFlags : byte
{
	/// <summary>
	/// <b>bRetry</b> field is valid
	/// </summary>
	Response = 0x01,

	/// <summary>
	/// low 32 bits of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_04967b3e-cdc6-4a45-a3d5-3e96a02770b9">SACK</see> mask are present in <b>dwSACKMask1</b>
	/// </summary>
	Sack1 = 0x02,

	/// <summary>
	/// high 32 bits of the SACK mask are present in <b>dwSACKMask2</b>
	/// </summary>
	Sack2 = 0x04,

	/// <summary>
	/// low 32 bits of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_7a220632-7944-4262-9740-388ba45c010b">send mask</see> are present in <b>dwSendMask1</b>
	/// </summary>
	Send1 = 0x08,

	/// <summary>
	/// high 32 bits of the send mask are present in <b>dwSendMask2</b>
	/// </summary>
	Send2 = 0x10,
}
