// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/97f93510-ca87-4df6-9af1-af5d930e42fa

namespace Aigamo.Otsuki.Messages.Reliable;

[Flags]
public enum PacketControl : byte
{
	/// <summary>
	/// indicates whether the frame is a retry for this sequence number
	/// </summary>
	Retry = 0x01,

	/// <summary>
	/// protocol version levels of 0x00010005 and higher indicate that the frame is a keep-alive frame; version levels of less than 0x00010005 indicate a request for a dedicated <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_6aa258ea-917f-461a-9c54-1b1a66791965">acknowledgment (ACK)</see> from the receiver
	/// </summary>
	KeepAliveOrCorrelate = 0x02,

	/// <summary>
	/// protocol version levels of 0x00010005 and higher indicate that the packet contains multiple payloads as described in section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/0ce3a800-f861-4556-9078-2004c2662ab3">2.2.3</see>
	/// </summary>
	Coalesce = 0x04,

	/// <summary>
	/// last packet in the stream; indicates disconnect
	/// </summary>
	EndStream = 0x08,

	/// <summary>
	/// low 32 bits of the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/9ca3f71c-1040-40b0-9866-2e2e40cf65b0">SACK</see> mask are present
	/// </summary>
	Sack1 = 0x10,

	/// <summary>
	/// high 32 bits of the SACK mask are present
	/// </summary>
	Sack2 = 0x20,

	/// <summary>
	/// low 32 bits of the cancel-<see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8r/a4ef7612-93d2-4d94-a96c-dbe6a1890ed6#gt_7a220632-7944-4262-9740-388ba45c010b">send mask</see> are present
	/// </summary>
	Send1 = 0x40,

	/// <summary>
	/// high 32 bits of the cancel-send mask are present
	/// </summary>
	Send2 = 0x80,
}
