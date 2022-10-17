using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable;

public class ConnectedMessageSerializerTests
{
	public static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
					0x88, 0x02, 0x00, 0x00, 0x06, 0x00, 0x01, 0x00, 0xC6, 0xAE, 0xC9, 0x79, 0xE1, 0xDF, 0x04, 0x00,
				},
				new ConnectedMessage
				{
					Poll = true,
					MessageId = 0x00,
					ResponseId = 0x00,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x79C9AEC6),
					Timestamp = 0x0004DFE1,
				},
		};

		yield return new object?[]
		{
				new byte[]
				{
					0x80, 0x02, 0x01, 0x00, 0x06, 0x00, 0x01, 0x00, 0xC6, 0xAE, 0xC9, 0x79, 0x9D, 0x36, 0x67, 0x23,
				},
				new ConnectedMessage
				{
					MessageId = 0x01,
					ResponseId = 0x00,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x79C9AEC6),
					Timestamp = 0x2367369D,
				},
		};

		yield return new object?[]
		{
				new byte[]
				{
					0x88, 0x02, 0x00, 0x00, 0x06, 0x00, 0x01, 0x00, 0x7d, 0x99, 0xc5, 0x51, 0x52, 0xa2, 0xa2, 0x21,
				},
				new ConnectedMessage
				{
					Poll = true,
					MessageId = 0x00,
					ResponseId = 0x00,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x51C5997D),
					Timestamp = 0x21A2A252,
				},
		};

		yield return new object?[]
		{
				new byte[]
				{
					0x80, 0x02, 0x01, 0x00, 0x06, 0x00, 0x01, 0x00, 0x7d, 0x99, 0xc5, 0x51, 0x5a, 0xa2, 0xa2, 0x21,
				},
				new ConnectedMessage
				{
					MessageId = 0x01,
					ResponseId = 0x00,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x51C5997D),
					Timestamp = 0x21A2A25A,
				},
		};

		yield return new object?[]
		{
				new byte[]
				{
					0x88, 0x02, 0x11, 0x0E, 0x06, 0x00, 0x01, 0x00, 0xA6, 0xCE, 0x81, 0x7A, 0x0E, 0xE3, 0x44, 0x02,
				},
				new ConnectedMessage
				{
					Poll = true,
					MessageId = 0x11,
					ResponseId = 0x0E,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x7A81CEA6),
					Timestamp = 0x0244E30E,
				},
		};
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, ConnectedMessage expected)
	{
		var message = ConnectedMessageSerializer.Default.Deserialize(data);
		message.Command.Should().Be(expected.Command);
		message.Opcode.Should().Be(expected.Opcode);
		message.MessageId.Should().Be(expected.MessageId);
		message.ResponseId.Should().Be(expected.ResponseId);
		message.ProtocolVersion.Should().Be(expected.ProtocolVersion);
		message.SessionId.Should().Be(expected.SessionId);
		message.Timestamp.Should().Be(expected.Timestamp);
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, ConnectedMessage message)
	{
		ConnectedMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
