using System.Collections.Immutable;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class ConnectFailedMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
					0xC5, 0x00, 0x00, 0x00, 0x90, 0x83, 0x15, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new ConnectFailedMessage
				{
					ResultCode = ResultCode.InvalidInterface,
					Reply = ImmutableArray<byte>.Empty,
				},
		};

		yield return new object?[]
		{
				new byte[]
				{
					0xC5, 0x00, 0x00, 0x00, 0x00, 0x83, 0x15, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new ConnectFailedMessage
				{
					ResultCode = ResultCode.InvalidApplication,
					Reply = ImmutableArray<byte>.Empty,
				},
		};

		yield return new object?[]
		{
				new byte[]
				{
					0xC5, 0x00, 0x00, 0x00, 0x10, 0x84, 0x15, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new ConnectFailedMessage
				{
					ResultCode = ResultCode.InvalidPassword,
					Reply = ImmutableArray<byte>.Empty,
				},
		};
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, ConnectFailedMessage expected)
	{
		var message = ConnectFailedMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.ResultCode.Should().Be(expected.ResultCode);
		message.ReplySize.Should().Be(expected.ReplySize);
		message.Reply.Should().Equal(expected.Reply);
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, ConnectFailedMessage message)
	{
		ConnectFailedMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
