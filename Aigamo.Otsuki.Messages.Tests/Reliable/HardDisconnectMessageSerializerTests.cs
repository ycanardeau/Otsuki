using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable;

public class HardDisconnectMessageSerializerTests
{
	public static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
					0x80,
					0x04,
					0x0E,
					0x00,
					0x06, 0x00, 0x01, 0x00,
					0x64, 0x07, 0xAE, 0x22,
					0xD2, 0xB3, 0x10, 0x02,
				},
				new HardDisconnectMessage
				{
					MessageId = 0x0E,
					ResponseId = 0x00,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x22AE0764),
					Timestamp = 0x0210B3D2,
				},
				false,
		};
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, HardDisconnectMessage expected, bool enableSigning)
	{
		var message = HardDisconnectMessageSerializer.Default.Deserialize(data);
		message.Command.Should().Be(expected.Command);
		message.Opcode.Should().Be(expected.Opcode);
		message.MessageId.Should().Be(expected.MessageId);
		message.ResponseId.Should().Be(expected.ResponseId);
		message.ProtocolVersion.Should().Be(expected.ProtocolVersion);
		message.SessionId.Should().Be(expected.SessionId);
		message.Timestamp.Should().Be(expected.Timestamp);
		message.Signature.Should().Be(expected.Signature);
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, HardDisconnectMessage message, bool enableSigning)
	{
		HardDisconnectMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
