using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable
{
	public class SackMessageSerializerTests
	{
		public static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0x80, 0x06, 0x01, 0x00, 0x03, 0x06, 0x00, 0x00, 0x07, 0x5D, 0x11, 0x00,
				},
				new SackMessage
				{
					Response = true,
					Retry = 0,
					NextSend = new SequenceId(3),
					NextReceive = new SequenceId(6),
					Timestamp = 0x00115D07,
				},
				false,
			};

			yield return new object?[]
			{
				new byte[]
				{
					0x80, 0x06, 0x01, 0x00, 0x04, 0x04, 0x00, 0x00, 0x64, 0xa2, 0xa2, 0x21,
				},
				new SackMessage
				{
					Response = true,
					Retry = 0,
					NextSend = new SequenceId(4),
					NextReceive = new SequenceId(4),
					Timestamp = 0x21A2A264,
				},
				false,
			};

			yield return new object?[]
			{
				new byte[]
				{
					0x80,
					0x06,
					0x0B,
					0x01,
					0x02,
					0x03,
					0x00, 0x00,
					0x78, 0x56, 0x34, 0x12,
					0x04, 0x00, 0x00, 0x00,
					0x05, 0x00, 0x00, 0x00,
				},
				new SackMessage
				{
					Response = true,
					Retry = 1,
					NextSend = new SequenceId(2),
					NextReceive = new SequenceId(3),
					Timestamp = 0x12345678,
					SackMask = 4,
					SendMask = 5,
				},
				false,
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, SackMessage expected, bool enableSigning)
		{
			var message = SackMessageSerializer.Default.Deserialize(data);
			message.Command.Should().Be(expected.Command);
			message.Opcode.Should().Be(expected.Opcode);
			message.Flags.Should().Be(expected.Flags);
			message.Retry.Should().Be(expected.Retry);
			message.NextSend.Should().Be(expected.NextSend);
			message.NextReceive.Should().Be(expected.NextReceive);
			message.Padding.Should().Be(expected.Padding);
			message.Timestamp.Should().Be(expected.Timestamp);
			message.SackMask.Should().Be(expected.SackMask);
			message.SendMask.Should().Be(expected.SendMask);
			message.Signature.Should().Be(expected.Signature);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, SackMessage message, bool enableSigning)
		{
			SackMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
