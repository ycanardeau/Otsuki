namespace Aigamo.Otsuki.Messages.Reliable;

public enum ExtendedOpcode : byte
{
	Connect = 0x01,
	Connected = 0x02,
	ConnectedSigned = 0x03,
	HardDisconnect = 0x04,
	Sack = 0x06,
}
