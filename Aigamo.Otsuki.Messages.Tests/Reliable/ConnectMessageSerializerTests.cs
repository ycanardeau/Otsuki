using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable
{
	public class ConnectMessageSerializerTests
	{
		public static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0x88, 0x01, 0x00, 0x00, 0x06, 0x00, 0x01, 0x00, 0xC6, 0xAE, 0xC9, 0x79, 0x9D, 0x36, 0x67, 0x23,
				},
				new ConnectMessage
				{
					Poll = true,
					MessageId = 0x00,
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
					0x88, 0x01, 0x00, 0x00, 0x06, 0x00, 0x01, 0x00, 0x7d, 0x99, 0xc5, 0x51, 0x59, 0xa2, 0xa2, 0x21,
				},
				new ConnectMessage
				{
					Poll = true,
					MessageId = 0,
					ResponseId = 0,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x51C5997D),
					Timestamp = 0x21A2A259,
				},
			};

			yield return new object?[]
			{
				new byte[]
				{
					0x88, 0x01, 0x0E, 0x00, 0x06, 0x00, 0x01, 0x00, 0x64, 0x07, 0xAE, 0x22, 0xD2, 0xB3, 0x10, 0x02,
				},
				new ConnectMessage
				{
					Poll = true,
					MessageId = 0x0E,
					ResponseId = 0x00,
					ProtocolVersion = 0x00010006,
					SessionId = new SessionId(0x22AE0764),
					Timestamp = 0x0210B3D2,
				},
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, ConnectMessage expected)
		{
			var message = ConnectMessageSerializer.Default.Deserialize(data);
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
		internal void Serialize(byte[] expected, ConnectMessage message)
		{
			ConnectMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
